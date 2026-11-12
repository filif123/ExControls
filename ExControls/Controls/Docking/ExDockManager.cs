using ExControls.Docking;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable EventNeverSubscribedTo.Global

namespace ExControls;

/// <summary>
/// Layout of a window into dockable panels like Visual Studio or Rider: the main content in <see cref="Document" /> and
/// tool panels (<see cref="ExDockPanel" />) docked to the left, right, top or bottom. Panels of a zone are stacked along the
/// side, several panels in one group are tabs; splitters resize zones and groups. The user can hide a panel, collapse its
/// group to the strip at the edge (auto-hide), drag it to another place with docking guides or to a floating window.
/// <see cref="SaveLayout" /> and <see cref="LoadLayout" /> keep the layout in a string for the application settings.
/// </summary>
/// <remarks>
/// The manager is created in code (place it with <see cref="DockStyle.Fill" />, move the content of the form into
/// <see cref="Document" />), add all panels and then load the saved layout. It owns the panels: disposing the manager
/// disposes all of them, including the hidden ones. Texts of the tool tips and the window menu are in <see cref="Strings" />.
/// </remarks>
[ToolboxItem(false)]
public partial class ExDockManager : Control, IThemeable
{
    private static readonly ExDockSide[] AllSides = [ExDockSide.Left, ExDockSide.Top, ExDockSide.Right, ExDockSide.Bottom];

    // zony indexovane stranou (poradie ExDockSide)
    private readonly DockZone[] _zones =
    [
        new(ExDockSide.Left, 250), new(ExDockSide.Top, 200), new(ExDockSide.Right, 250), new(ExDockSide.Bottom, 200)
    ];

    private readonly List<ExDockPanel> _panels = [];
    private readonly List<DockGroup> _floating = [];
    private readonly Panel _document;
    private readonly DockAutoHidePopup _popup;
    private readonly System.Windows.Forms.Timer _popupTimer;
    private readonly ToolTip _toolTip;
    private ExTheme? _theme;
    private ExDockStrings _strings = new();
    private ExDockPanel? _activePanel;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExDockManager" /> class.
    /// </summary>
    public ExDockManager()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw, true);
        SetStyle(ControlStyles.Selectable, false);
        _document = new Panel { Name = "Document" };
        _document.Enter += (_, _) => SetActivePanel(null);
        _popup = new DockAutoHidePopup(this);
        Controls.Add(_popup);
        Controls.Add(_document);
        _popupTimer = new System.Windows.Forms.Timer { Interval = 300 };
        _popupTimer.Tick += PopupTimer_Tick;
        _toolTip = new ToolTip();
    }

    /// <summary>
    /// Gets the container of the main content (the area between the docked zones).
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Panel Document => _document;

    /// <summary>
    /// Gets the panels added to the manager (shown and hidden) in the order of adding.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IReadOnlyList<ExDockPanel> Panels => _panels;

    /// <summary>
    /// Gets or sets the texts of the tool tips and the window menu.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ExDockStrings Strings
    {
        get => _strings;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _strings = value;
        }
    }

    /// <summary>
    /// Gets or sets whether panels can be moved to floating windows (dragging out of the guides, the window menu,
    /// double-click on the caption).
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool AllowFloating { get; set; } = true;

    /// <summary>
    /// Gets the panel with the keyboard focus; <see langword="null" /> when the focus is in <see cref="Document" />.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ExDockPanel? ActivePanel => _activePanel;

    /// <summary>
    /// Occurs when a panel changes its state or side (shown, hidden, docked elsewhere, collapsed, floated, removed).
    /// </summary>
    public event EventHandler<ExDockPanelEventArgs>? PanelStateChanged;

    /// <summary>
    /// Occurs before the user hides a panel; the application can keep it open.
    /// </summary>
    public event EventHandler<ExDockPanelClosingEventArgs>? PanelClosing;

    /// <summary>
    /// Occurs when <see cref="ActivePanel" /> changes.
    /// </summary>
    public event EventHandler? ActivePanelChanged;

    /// <inheritdoc />
    protected override Size DefaultSize => new(400, 300);

    /// <summary>
    /// Adds a panel to a new group at the end of the zone on a side and shows it.
    /// </summary>
    /// <param name="panel">panel with a unique non-empty <see cref="ExDockPanel.Key" /></param>
    /// <param name="side">side of the zone</param>
    /// <exception cref="ArgumentException">the panel is already in a manager, has no key or its key is used</exception>
    public void AddPanel(ExDockPanel panel, ExDockSide side)
    {
        CheckNew(panel);
        var zone = Zone(side);
        Change(() =>
        {
            Register(panel);
            var group = new DockGroup();
            group.Panels.Add(panel);
            panel.Group = group;
            group.Active = panel;
            InsertGroup(zone, group, int.MaxValue);
        });
    }

    /// <summary>
    /// Adds a panel as a tab to the group of another panel (the active tab of the group stays).
    /// </summary>
    /// <param name="panel">panel with a unique non-empty <see cref="ExDockPanel.Key" /></param>
    /// <param name="tabWith">panel of this manager whose group gets the new tab</param>
    /// <exception cref="ArgumentException">the panel is already in a manager, has no key or its key is used, or
    /// <paramref name="tabWith" /> is not in this manager</exception>
    public void AddPanel(ExDockPanel panel, ExDockPanel tabWith)
    {
        CheckNew(panel);
        CheckPanel(tabWith, nameof(tabWith));
        Change(() =>
        {
            Register(panel);
            var group = tabWith.Group!;
            group.Panels.Add(panel);
            panel.Group = group;
            group.FixActive();
        });
    }

    /// <summary>
    /// Removes a panel from the manager (the panel is not disposed).
    /// </summary>
    /// <param name="panel">panel of this manager</param>
    public void RemovePanel(ExDockPanel panel)
    {
        CheckPanel(panel, nameof(panel));
        Change(() =>
        {
            if (_popup.Group == panel.Group)
                HidePopup();
            if (panel.Group is { } group)
            {
                group.Panels.Remove(panel);
                group.FixActive();
            }

            panel.Group = null;
            panel.IsClosed = false;
            panel.Manager = null;
            _panels.Remove(panel);
            if (panel.Parent is DockGroupView)
                panel.Parent = null;
            if (_activePanel == panel)
                SetActivePanel(null);
        });
    }

    /// <summary>
    /// Shows a hidden panel at its last place and selects its tab; with <paramref name="activate" /> it also moves the focus
    /// into the panel (a collapsed panel slides out, a floating window comes to the front).
    /// </summary>
    /// <param name="panel">panel of this manager</param>
    /// <param name="activate">move the focus into the panel</param>
    public void ShowPanel(ExDockPanel panel, bool activate = true)
    {
        CheckPanel(panel, nameof(panel));
        var group = panel.Group!;
        if (panel.IsClosed || group.Active != panel)
            Change(() =>
            {
                panel.IsClosed = false;
                group.Active = panel;
            });

        if (!activate)
            return;

        if (group.AutoHide)
        {
            ShowPopup(group, panel, true);
            return;
        }

        if (group.FloatForm is { IsDisposed: false } form)
            form.Activate();
        FocusPanel(panel);
    }

    /// <summary>
    /// Hides a panel; it keeps its place and <see cref="ShowPanel" /> returns it there. It does not raise
    /// <see cref="PanelClosing" />.
    /// </summary>
    /// <param name="panel">panel of this manager</param>
    public void HidePanel(ExDockPanel panel)
    {
        CheckPanel(panel, nameof(panel));
        if (panel.IsClosed)
            return;

        Change(() =>
        {
            if (_popup.Group == panel.Group)
                HidePopup();
            panel.IsClosed = true;
        });
    }

    /// <summary>
    /// Docks a panel (shown or hidden) to a new group at the end of the zone on a side and shows it.
    /// </summary>
    /// <param name="panel">panel of this manager</param>
    /// <param name="side">side of the zone</param>
    public void DockPanel(ExDockPanel panel, ExDockSide side)
    {
        CheckPanel(panel, nameof(panel));
        var zone = Zone(side);
        Change(() =>
        {
            panel.IsClosed = false;
            var group = panel.Group!;
            var moved = Take(group, group.Panels.Count == 1 ? null : panel);
            InsertGroup(zone, moved, int.MaxValue);
        });
    }

    /// <summary>
    /// Moves a panel (shown or hidden) as a tab into the group of another panel, makes it the active tab and shows it.
    /// </summary>
    /// <param name="panel">panel of this manager</param>
    /// <param name="tabWith">panel of this manager whose group gets the tab</param>
    public void DockPanel(ExDockPanel panel, ExDockPanel tabWith)
    {
        CheckPanel(panel, nameof(panel));
        CheckPanel(tabWith, nameof(tabWith));
        if (panel == tabWith)
            return;

        Change(() =>
        {
            panel.IsClosed = false;
            var target = tabWith.Group!;
            MoveIntoGroup(panel.Group!, panel, target);
            target.Active = panel;
        });
    }

    /// <summary>
    /// Moves a panel (shown or hidden) to a floating window next to its current place and shows it; a panel that is
    /// alone in its group takes the whole group.
    /// </summary>
    /// <param name="panel">panel of this manager</param>
    /// <exception cref="InvalidOperationException"><see cref="AllowFloating" /> or <see cref="ExDockPanel.CanFloat" /> is false</exception>
    public void FloatPanel(ExDockPanel panel)
    {
        CheckPanel(panel, nameof(panel));
        if (!AllowFloating || !panel.CanFloat)
            throw new InvalidOperationException("The panel cannot float.");

        Change(() =>
        {
            panel.IsClosed = false;
            var group = panel.Group!;
            if (group.IsFloating)
                return;

            var bounds = ScreenBoundsOf(group.View);
            var moved = Take(group, group.Panels.Count == 1 ? null : panel);
            MakeFloating(moved, bounds);
        });
    }

    /// <summary>
    /// Collapses the group of a panel to the strip at the edge or docks it again; a floating group is docked first.
    /// </summary>
    /// <param name="panel">panel of this manager</param>
    /// <param name="autoHide">collapse (<see langword="true" />) or dock (<see langword="false" />)</param>
    public void SetAutoHide(ExDockPanel panel, bool autoHide)
    {
        CheckPanel(panel, nameof(panel));
        Change(() =>
        {
            var group = panel.Group!;
            if (group.IsFloating)
            {
                if (!autoHide)
                    return;
                DetachGroup(group);
                InsertGroup(Zone(group.HomeSide), group, int.MaxValue);
            }

            if (_popup.Group == group)
                HidePopup();
            group.AutoHide = autoHide;
        });
    }

    /// <summary>
    /// Gets the size of the zone on a side in logical pixels (96 DPI): the width of the left and right zones, the height
    /// of the top and bottom ones.
    /// </summary>
    public int GetZoneSize(ExDockSide side) => Zone(side).Size;

    /// <summary>
    /// Sets the size of the zone on a side in logical pixels (96 DPI); the layout shrinks it when the document would not fit.
    /// </summary>
    /// <param name="side">side of the zone</param>
    /// <param name="size">width of the left and right zones, height of the top and bottom ones</param>
    public void SetZoneSize(ExDockSide side, int size)
    {
        if (size < 0)
            throw new ArgumentOutOfRangeException(nameof(size), size, "The zone size must not be negative.");
        Zone(side).Size = size;
        PerformLayout();
    }

    /// <inheritdoc />
    public void ApplyTheme(ExTheme theme)
    {
        ArgumentNullException.ThrowIfNull(theme);
        _theme = theme;
        Colors = DockColors.From(theme);
        ExThemer.KeepDefaults(this, theme, nameof(BackColor), nameof(ForeColor));
        ExThemer.KeepDefaults(_document, theme, nameof(BackColor), nameof(ForeColor));
        if (!theme.UseSystemStyle)
        {
            BackColor = theme.PanelBackColor;
            ForeColor = theme.PanelForeColor;
            _document.BackColor = theme.PanelBackColor;
            _document.ForeColor = theme.PanelForeColor;
        }

        ExThemer.Apply(_document.Controls, theme);
        foreach (var panel in _panels)
            panel.ApplyTheme(theme);
        foreach (var group in _floating)
            group.FloatForm?.UpdateColors();
        Invalidate(true);
    }

    /// <summary>
    /// Raises <see cref="PanelStateChanged" />.
    /// </summary>
    protected virtual void OnPanelStateChanged(ExDockPanelEventArgs e) => PanelStateChanged?.Invoke(this, e);

    /// <summary>
    /// Raises <see cref="PanelClosing" />.
    /// </summary>
    protected virtual void OnPanelClosing(ExDockPanelClosingEventArgs e) => PanelClosing?.Invoke(this, e);

    /// <summary>
    /// Raises <see cref="ActivePanelChanged" />.
    /// </summary>
    protected virtual void OnActivePanelChanged(EventArgs e) => ActivePanelChanged?.Invoke(this, e);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _popupTimer.Dispose();
            _toolTip.Dispose();
            foreach (var group in _floating.ToList())
                CloseFloatForm(group);
            // pohlady a panely mimo stromu prvkov (skryte, zbalene, plavajuce) by inak ostali nezrusene
            foreach (var group in AllGroups.ToList())
                if (group.View is { Parent: null } view)
                    view.Dispose();
            foreach (var panel in _panels.ToList())
                panel.Dispose();
        }

        base.Dispose(disposing);
    }

    private void CheckNew(ExDockPanel panel)
    {
        ArgumentNullException.ThrowIfNull(panel);
        if (panel.Manager is not null)
            throw new ArgumentException("The panel is already added to a dock manager.", nameof(panel));
        if (string.IsNullOrEmpty(panel.Key))
            throw new ArgumentException("The panel has no key.", nameof(panel));
        if (_panels.Exists(p => string.Equals(p.Key, panel.Key, StringComparison.Ordinal)))
            throw new ArgumentException($"A panel with the key '{panel.Key}' is already added.", nameof(panel));
    }

    private void CheckPanel(ExDockPanel panel, string name)
    {
        ArgumentNullException.ThrowIfNull(panel, name);
        if (panel.Manager != this)
            throw new ArgumentException("The panel is not added to this dock manager.", name);
    }

    private void Register(ExDockPanel panel)
    {
        panel.Manager = this;
        panel.IsClosed = false;
        _panels.Add(panel);
        if (_theme is not null)
            panel.ApplyTheme(_theme);
    }

    private DockZone Zone(ExDockSide side) =>
        (uint)side < (uint)_zones.Length ? _zones[(int)side] : throw new ArgumentOutOfRangeException(nameof(side), side, "Unknown side.");
}
