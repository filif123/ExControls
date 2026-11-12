using System.Diagnostics.CodeAnalysis;
using ExControls.Docking;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace ExControls;

/// <summary>
/// Tool panel of an <see cref="ExDockManager" /> (like a tool window in Visual Studio or Rider): a container for the
/// content with a stable <see cref="Key" /> for the saved layout and a <see cref="Control.Text" /> shown in the caption, on
/// its tab and on the auto-hide strip. The manager places it into a zone, a floating window or hides it; the panel only
/// hosts the content (usually one control with <see cref="DockStyle.Fill" />).
/// </summary>
[ToolboxItem(false)]
public class ExDockPanel : ContainerControl, IThemeable
{
    private string _key = "";
    private Image? _image;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExDockPanel" /> class.
    /// </summary>
    public ExDockPanel()
    {
        SetStyle(ControlStyles.Selectable, false);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExDockPanel" /> class with a key and a caption.
    /// </summary>
    /// <param name="key">stable key of the panel in the saved layout</param>
    /// <param name="text">caption of the panel</param>
    public ExDockPanel(string key, string text) : this()
    {
        Key = key;
        Text = text;
    }

    /// <summary>
    /// Gets or sets the stable key of the panel in the saved layout (<see cref="ExDockManager.SaveLayout" />); unique in the
    /// manager. When empty, <see cref="Control.Name" /> is used.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string Key
    {
        get => _key.Length > 0 ? _key : Name;
        set
        {
            if (Manager is not null && !string.Equals(Key, value, StringComparison.Ordinal))
                throw new InvalidOperationException("The key of a panel added to a dock manager cannot change.");
            _key = value ?? "";
        }
    }

    /// <summary>
    /// Gets or sets the caption of the panel (caption bar, tab, auto-hide strip).
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [AllowNull]
    public override string Text
    {
        get => base.Text;
        set => base.Text = value;
    }

    /// <summary>
    /// Gets or sets the image shown before the caption on the tab and on the auto-hide strip (16 logical pixels; it is not
    /// scaled - the application supplies an image for the DPI).
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Image? Image
    {
        get => _image;
        set
        {
            _image = value;
            Manager?.OnPanelAppearanceChanged(this);
        }
    }

    /// <summary>
    /// Gets or sets whether the user can hide the panel (close button and the menu item); the application can always hide
    /// it by <see cref="ExDockManager.HidePanel" />.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool CanClose { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the panel can be moved to a floating window (when <see cref="ExDockManager.AllowFloating" />).
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool CanFloat { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the panel can be collapsed to the auto-hide strip.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool CanAutoHide { get; set; } = true;

    /// <summary>
    /// Gets the manager of the panel; <see langword="null" /> until <see cref="ExDockManager.AddPanel(ExDockPanel, ExDockSide)" />.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ExDockManager? Manager { get; internal set; }

    /// <summary>
    /// Gets the state of the panel.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ExDockState State =>
        Group is not { } group || IsClosed ? ExDockState.Hidden
        : group.IsFloating ? ExDockState.Floating
        : group.AutoHide ? ExDockState.AutoHide
        : ExDockState.Docked;

    /// <summary>
    /// Gets the side of the panel: the side of its zone, or for a floating or hidden panel the side it returns to.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ExDockSide Side => Group?.Side ?? ExDockSide.Bottom;

    /// <summary>
    /// Gets whether the panel is shown (docked, collapsed to the strip or floating), i.e. not <see cref="ExDockState.Hidden" />.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsShown => State != ExDockState.Hidden;

    /// <summary>
    /// Skupina panela (aj ked je skryty).
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    internal DockGroup? Group { get; set; }

    /// <summary>
    /// Panel je skryty (ostava clenom skupiny).
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    internal bool IsClosed { get; set; }

    /// <inheritdoc />
    public virtual void ApplyTheme(ExTheme theme)
    {
        ArgumentNullException.ThrowIfNull(theme);
        ExThemer.KeepDefaults(this, theme, nameof(BackColor), nameof(ForeColor));
        if (!theme.UseSystemStyle)
        {
            BackColor = theme.PanelBackColor;
            ForeColor = theme.PanelForeColor;
        }

        ExThemer.Apply(Controls, theme);
    }

    /// <inheritdoc />
    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        Manager?.OnPanelAppearanceChanged(this);
    }
}
