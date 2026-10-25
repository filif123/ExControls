using System.Drawing.Text;

namespace ExControls;

/// <summary>
/// Custom title bar of an <see cref="ExForm" />. As a direct child of the form (docked to the top) it replaces the system
/// caption: the window icon (system menu) on the left, the window buttons on the right, the title bar controls (e.g. the main
/// menu) between them and the form text in the free space. The free space and empty parts of tool strips drag the window,
/// their items stay clickable.
/// </summary>
/// <remarks>
/// Only tool strips directly in the title bar are handled (their items are always laid out side by side); other controls in
/// the title bar are clickable as a whole.
/// An inactive form has dimmed text, button glyphs and also menu items and labels in these tool strips (like a system menu bar)
/// - through <see cref="ToolStripItem.ForeColor" />, so a renderer that determines the text color of items itself does not show
/// the dimming.
/// The buttons and the icon follow the system caption: help (<see cref="Form.HelpButton" />) only without minimize and
/// maximize, a tool window only close, a dialog (FixedDialog) without its own icon has no icon.
/// The caption colors of the form (<see cref="ExForm.TitleBarBackColor" />, <see cref="ExForm.TitleBarForeColor" />) and in an
/// active form the Windows accent color (if the system shows it on title bars) take precedence over the colors of the title bar;
/// tool strips in the title bar then have a transparent background and text in the caption color, other controls keep their colors.
/// A menu (<see cref="MenuStrip" />) whose items do not fit into the title bar collapses at runtime into a ☰ button
/// (<see cref="CollapseMenu" />).
/// When the form does not use the custom title bar (<see cref="ExForm.UsesCustomTitleBar" />), it is a plain panel without icon,
/// text and buttons.
/// </remarks>
[Designer("ExControls.Designers.ExTitleBarDesigner, ExControls")]
#if NETFRAMEWORK
[ToolboxBitmap(typeof(MenuStrip), "MenuStrip.bmp")]
#else
[ToolboxBitmap(typeof(ToolboxIcons), "Toolbox.ExTitleBar.ico")]
#endif
public class ExTitleBar : Panel, IThemeable
{
    // rozmery pri 96 DPI
    private const int MinHeight = 32;
    private const int IconAreaWidth = 36;
    private const int IconSize = 16;
    private const int ButtonWidth = 46;
    private const int GlyphSize = 10;
    // pomocnika kresli systemove zahlavie obycajnym otaznikom pisma Segoe UI (pismo ikon ma iny tvar) - vysoky ako krizik
    private const string HelpGlyph = "?";
    private const string HelpGlyphFamily = "Segoe UI";
    private const int HelpGlyphSize = 14;
    private const int MenuGlyphSize = 16;
    private const int TitleMargin = 8;
    private const int MinTitleWidth = 40;

    private static readonly CaptionGlyphs Glyphs = CaptionGlyphs.Find();

    // text okna pismom systemoveho zahlavia (vo Windows 10 a 11 Segoe UI 9 b.) ako titulok ostatnych okien,
    // nie pismom prvku - to aplikacie nastavuju podla konfiguracie
    private static readonly Font CaptionFont = SystemFonts.CaptionFont ?? DefaultFont;

    private readonly Dictionary<ToolStrip, ToolStripHook> _hooks = [];
    private readonly List<ColorOverride> _colorOverrides = [];
    private Color? _accent = ExTools.CaptionAccentColor();
    private bool _applyingTheme;
    private readonly Dictionary<MenuStrip, CollapsedMenu> _collapsedMenus = [];
    private bool _collapseMenu = true;
    private bool _updatingMenus;
    private Font? _menuGlyphFont;
    private bool _shown = true;
    private bool _showTitle = true;
    private bool _formActive = true;
    private TitleBarHit? _hotButton;
    private TitleBarHit? _pressedButton;
    private Form? _form;
    private Font? _glyphFont;
    private Font? _helpGlyphFont;
    private Icon? _smallIcon;
    private Color _inactiveForeColor;
    private Color _buttonHoverBackColor;
    private Color _buttonPressedBackColor;
    private Color _closeButtonHoverBackColor;
    private Color _topBorderColor;

    /// <summary>
    /// Creates a title bar docked to the top.
    /// </summary>
    public ExTitleBar()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        Dock = DockStyle.Top;
        AutoSize = true;
    }

    /// <inheritdoc />
    protected override Size DefaultSize => new(200, 32);

    /// <inheritdoc />
    [DefaultValue(DockStyle.Top)]
    public override DockStyle Dock
    {
        get => base.Dock;
        set => base.Dock = value;
    }

    /// <summary>
    /// Gets or sets whether the height of the title bar follows its controls (at least 32 px at 96 DPI).
    /// </summary>
    [Browsable(true)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [DefaultValue(true)]
    public override bool AutoSize
    {
        get => base.AutoSize;
        set => base.AutoSize = value;
    }

    /// <summary>
    /// Gets or sets whether the title bar shows the form's text in the free space between its controls
    /// (in the font of the system caption).
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("Gets or sets whether the title bar shows the form's text in the free space between its controls.", true)]
    [DefaultValue(true)]
    public bool ShowTitle
    {
        get => _showTitle;
        set
        {
            if (_showTitle == value)
                return;
            _showTitle = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets whether a menu in the title bar collapses into one button (☰) when the title bar is too narrow
    /// for all its menu items (only at runtime).
    /// </summary>
    [ExCategory(CategoryType.Behavior)]
    [ExDescription("Gets or sets whether a menu in the title bar collapses into one button when the title bar is too narrow for its items.", true)]
    [DefaultValue(true)]
    public bool CollapseMenu
    {
        get => _collapseMenu;
        set
        {
            if (_collapseMenu == value)
                return;
            _collapseMenu = value;
            PerformLayout();
        }
    }

    /// <summary>
    /// Gets or sets the color of the text and button glyphs of an inactive form. Color.Empty derives it from ForeColor.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("Gets or sets the color of the text and button glyphs of an inactive form. Color.Empty derives it from ForeColor.", true)]
    [DefaultValue(typeof(Color), "")]
    public Color InactiveForeColor
    {
        get => _inactiveForeColor;
        set => SetColor(ref _inactiveForeColor, value);
    }

    /// <summary>
    /// Gets or sets the background of a window button under the mouse. Color.Empty derives it from BackColor and ForeColor.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("Gets or sets the background of a window button under the mouse. Color.Empty derives it from BackColor and ForeColor.", true)]
    [DefaultValue(typeof(Color), "")]
    public Color ButtonHoverBackColor
    {
        get => _buttonHoverBackColor;
        set => SetColor(ref _buttonHoverBackColor, value);
    }

    /// <summary>
    /// Gets or sets the background of a pressed window button. Color.Empty derives it from BackColor and ForeColor.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("Gets or sets the background of a pressed window button. Color.Empty derives it from BackColor and ForeColor.", true)]
    [DefaultValue(typeof(Color), "")]
    public Color ButtonPressedBackColor
    {
        get => _buttonPressedBackColor;
        set => SetColor(ref _buttonPressedBackColor, value);
    }

    /// <summary>
    /// Gets or sets the background of the close button under the mouse. Color.Empty is the red of the system caption
    /// (Windows 11 or Windows 10).
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("Gets or sets the background of the close button under the mouse. Color.Empty is the red of the system caption.", true)]
    [DefaultValue(typeof(Color), "")]
    public Color CloseButtonHoverBackColor
    {
        get => _closeButtonHoverBackColor;
        set => SetColor(ref _closeButtonHoverBackColor, value);
    }

    /// <summary>
    /// Gets or sets the 1 px top border of the window in Windows 10 (Windows 11 draws it itself). Color.Empty derives it
    /// from BackColor and ForeColor.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("Gets or sets the 1 px top border of the window in Windows 10. Color.Empty derives it from BackColor and ForeColor.", true)]
    [DefaultValue(typeof(Color), "")]
    public Color TopBorderColor
    {
        get => _topBorderColor;
        set => SetColor(ref _topBorderColor, value);
    }

    /// <inheritdoc />
    /// <remarks>The title bar controls are laid out between the icon and the window buttons.</remarks>
    public override Rectangle DisplayRectangle
    {
        get
        {
            var rect = base.DisplayRectangle;
            if (!ShowsCaption)
                return rect;

            var layout = ComputeLayout();
            var left = rect.Left + layout.LeftInset;
            var top = DrawsTopBorder ? Math.Max(rect.Top, 1) : rect.Top;
            return Rectangle.FromLTRB(left, top, Math.Max(left, rect.Right - layout.RightInset), rect.Bottom);
        }
    }

    /// <summary>
    /// The title bar should be shown (own value of <see cref="Control.Visible" /> - does not depend on whether the form is shown).
    /// </summary>
    internal bool Shown => _shown;


    /// <summary>
    /// The title bar currently replaces the system caption of its form.
    /// </summary>
    private bool IsActive => Parent is ExForm form && form.TitleBar == this && form.UsesCustomTitleBar;

    /// <summary>
    /// The title bar draws the window icon, text and buttons - at runtime with the custom title bar, always in the designer
    /// (preview).
    /// </summary>
    private bool ShowsCaption => Parent is ExForm form && form.TitleBar == this && (form.UsesCustomTitleBar || DesignMode);

    /// <summary>
    /// The title bar draws the top border of the window only in Windows 10 (Windows 11 draws it itself) and not when maximized.
    /// </summary>
    private bool DrawsTopBorder => ShowsCaption && !ExTools.IsWin11Build22000 && _form is { WindowState: not FormWindowState.Maximized };

    private Color EffectiveInactiveForeColor => _inactiveForeColor.IsEmpty ? TitleBarColors.Inactive(CaptionBackColor, CaptionForeColor) : _inactiveForeColor;

    /// <summary>
    /// The Windows accent color on the title bar (setting "Show accent color on title bars and window borders") - like the
    /// system caption only in an active form and only without a caption color of the form (<see cref="ExForm.TitleBarBackColor" />).
    /// </summary>
    private Color? ShownAccent => _accent is { } accent && _formActive && IsActive && Parent is ExForm { TitleBarBackColor.IsEmpty: true } ? accent : null;

    /// <summary>
    /// Title bar background: the caption color of the form (<see cref="ExForm.TitleBarBackColor" />), the Windows accent color,
    /// otherwise its own.
    /// </summary>
    private Color CaptionBackColor
    {
        get
        {
            if (!ShowsCaption)
                return BackColor;
            if (Parent is ExForm { TitleBarBackColor.IsEmpty: false } form)
                return form.TitleBarBackColor;
            return ShownAccent ?? BackColor;
        }
    }

    /// <summary>
    /// Text and button glyphs: the caption text color of the form (<see cref="ExForm.TitleBarForeColor" />), text on the accent
    /// color, otherwise its own.
    /// </summary>
    private Color CaptionForeColor
    {
        get
        {
            if (!ShowsCaption)
                return ForeColor;
            if (Parent is ExForm { TitleBarForeColor.IsEmpty: false } form)
                return form.TitleBarForeColor;
            return ShownAccent is { } accent ? TitleBarColors.AccentText(accent) : ForeColor;
        }
    }

    /// <summary>
    /// Determines the part of the title bar at a point.
    /// </summary>
    /// <param name="point">point in client coordinates of the title bar</param>
    public TitleBarHit HitTest(Point point)
    {
        if (ComputeLayout().HitTest(point) is { } hit)
            return hit;

        var child = GetChildAtPoint(point, GetChildAtPointSkip.Invisible | GetChildAtPointSkip.Disabled);
        return child switch
        {
            null => TitleBarHit.Caption,
            ToolStrip strip when IsFreeArea(strip, new Point(point.X - strip.Left, point.Y - strip.Top)) => TitleBarHit.Caption,
            _ => TitleBarHit.Client
        };
    }

    /// <summary>
    /// Height by the title bar controls (with <see cref="Control.AutoSize" />): the tallest visible control with padding, at least
    /// 32 px at 96 DPI.
    /// </summary>
    /// <param name="proposedSize">proposed size (ignored, the title bar keeps its width)</param>
    public override Size GetPreferredSize(Size proposedSize)
    {
        var content = Controls.Cast<Control>().Where(c => c.Visible).Select(c => c.GetPreferredSize(Size.Empty).Height).DefaultIfEmpty(0).Max();
        var top = DrawsTopBorder ? Math.Max(Padding.Top, 1) : Padding.Top;
        return new Size(Width, Math.Max(LogicalToDeviceUnits(MinHeight), content + top + Padding.Bottom));
    }

    /// <summary>
    /// Sets the title bar colors by the theme (panel background and text) and applies the theme to the controls in the title bar.
    /// </summary>
    /// <param name="theme">theme to apply</param>
    public void ApplyTheme(ExTheme theme)
    {
        ArgumentNullException.ThrowIfNull(theme);

        // tema nastavuje farby panelov a ich poloziek - prevzate farby zahlavia sa vratia a po teme pouziju znova
        RestoreStripColors();
        _applyingTheme = true;
        try
        {
            BackColor = theme.PanelBackColor;
            ForeColor = theme.PanelForeColor;
            ExThemer.Apply(Controls, theme);
        }
        finally
        {
            _applyingTheme = false;
        }

        UpdateStripColors();
        Invalidate(true);
    }

    /// <summary>
    /// The form is active (WM_NCACTIVATE) - an inactive one has dimmed text and button glyphs.
    /// </summary>
    internal void SetFormActive(bool active)
    {
        if (_formActive == active)
            return;
        _formActive = active;
        UpdateStripColors();
        Invalidate();
    }

    /// <summary>
    /// The Windows accent color or its display on title bars changed (WM_SETTINGCHANGE, WM_DWMCOLORIZATIONCOLORCHANGED).
    /// </summary>
    internal void RefreshAccent() => SetAccent(ExTools.CaptionAccentColor());

    /// <summary>
    /// The accent color on the title bar (<see langword="null" /> = the system does not show it on title bars).
    /// </summary>
    internal void SetAccent(Color? accent)
    {
        if (_accent == accent)
            return;
        _accent = accent;
        RefreshColors();
    }

    /// <summary>
    /// The caption colors of the form changed - repaint and tool strip colors.
    /// </summary>
    internal void RefreshColors()
    {
        UpdateStripColors();
        Invalidate();
    }

    /// <summary>
    /// Colors of the tool strips in the title bar (only the custom title bar at runtime): a title bar color other than its own
    /// (caption of the form, accent color) shows through the background of the strips and of their menu items and labels, which
    /// get its text color; an inactive form dims them. Only what changed is updated; a color the application has changed in
    /// the meantime is not restored.
    /// </summary>
    private void UpdateStripColors()
    {
        var wanted = new List<(Component Target, string Property, Color Color)>();
        if (IsActive && !_applyingTheme)
        {
            var back = CaptionBackColor != BackColor;
            Color? fore = !_formActive ? EffectiveInactiveForeColor : CaptionForeColor != ForeColor ? CaptionForeColor : null;
            foreach (var strip in _hooks.Keys)
            {
                if (back)
                    wanted.Add((strip, nameof(BackColor), Color.Transparent));
                foreach (ToolStripItem item in strip.Items)
                {
                    if (item is not (ToolStripMenuItem or ToolStripLabel))
                        continue;
                    // polozka s vlastnym pozadim (temy ho nastavuju aj polozkam) by zakryla farbu zahlavia
                    if (back)
                        wanted.Add((item, nameof(ToolStripItem.BackColor), Color.Transparent));
                    if (fore is { } color)
                        wanted.Add((item, nameof(ToolStripItem.ForeColor), color));
                }
            }
        }

        for (var i = _colorOverrides.Count - 1; i >= 0; i--)
        {
            var colorOverride = _colorOverrides[i];
            if (colorOverride.IsCurrent && wanted.Remove((colorOverride.Target, colorOverride.Property, colorOverride.Requested)))
                continue;
            colorOverride.Restore();
            _colorOverrides.RemoveAt(i);
        }

        foreach (var (target, property, color) in wanted)
            if (TypeDescriptor.GetProperties(target)[property] is { } descriptor)
                _colorOverrides.Add(ColorOverride.Apply(target, descriptor, color));
    }

    /// <summary>
    /// Before a tool strip is painted (the Paint event precedes painting the items): a selected or open item has the highlight
    /// background from the theme - its text in its original color, the others in the title bar color.
    /// </summary>
    private void ShowItemStates(ToolStrip strip)
    {
        foreach (var colorOverride in _colorOverrides)
            if (colorOverride is { Target: ToolStripItem item, Property: nameof(ToolStripItem.ForeColor) } && item.Owner == strip)
                colorOverride.Show(item.Selected || item.Pressed ? colorOverride.Original : colorOverride.Requested);
    }

    /// <summary>
    /// Restores the colors the tool strips and items had before the title bar changed them.
    /// </summary>
    private void RestoreStripColors()
    {
        foreach (var colorOverride in _colorOverrides)
            colorOverride.Restore();
        _colorOverrides.Clear();
    }

    /// <summary>
    /// The mouse is over a window button (or elsewhere in the title bar - <see langword="null" />).
    /// </summary>
    internal void TrackButton(TitleBarHit? button)
    {
        if (_hotButton == button)
            return;
        InvalidateButton(_hotButton);
        _hotButton = button;
        InvalidateButton(_hotButton);
    }

    /// <summary>
    /// A window button is pressed.
    /// </summary>
    internal void PressButton(TitleBarHit button)
    {
        _pressedButton = button;
        TrackButton(button);
        InvalidateButton(button);
    }

    /// <summary>
    /// The mouse is released over <paramref name="button" />.
    /// </summary>
    /// <returns>the button was also pressed - it is a click</returns>
    internal bool ReleaseButton(TitleBarHit button)
    {
        var clicked = _pressedButton == button;
        InvalidateButton(_pressedButton);
        _pressedButton = null;
        return clicked;
    }

    /// <summary>
    /// The mouse left the title bar - no button is under the mouse or pressed.
    /// </summary>
    internal void ResetButtons()
    {
        InvalidateButton(_hotButton);
        InvalidateButton(_pressedButton);
        _hotButton = null;
        _pressedButton = null;
    }

    /// <summary>
    /// The form changed its icon, buttons, window state or frame - new layout and repaint.
    /// </summary>
    internal void RefreshCaption()
    {
        _smallIcon?.Dispose();
        _smallIcon = null;
        UpdateStripColors();
        PerformLayout();
        Invalidate();
    }

    /// <inheritdoc />
    protected override void OnLayout(LayoutEventArgs levent)
    {
        base.OnLayout(levent);
        UpdateCollapsedMenus();
    }

    /// <summary>
    /// Collapses the menus in the title bar that do not fit and expands those that fit again (only at runtime - the designer
    /// would save the moved items), and applies the title bar colors to new items.
    /// </summary>
    private void UpdateCollapsedMenus()
    {
        if (_updatingMenus)
            return;

        _updatingMenus = true;
        try
        {
            foreach (var menu in _hooks.Keys.OfType<MenuStrip>().ToList())
            {
                var collapse = _collapseMenu && !DesignMode && ExpandedWidth(menu) > SpaceFor(menu);
                _ = collapse ? Collapse(menu) : Expand(menu);
            }

            // aj polozky, ktore aplikacia pridala do ponuky za behu (meni sa len to, co chyba)
            UpdateStripColors();
        }
        finally
        {
            _updatingMenus = false;
        }
    }

    /// <summary>
    /// Width of the menu with all its items - a collapsed menu counts the width its items had before collapsing instead of
    /// the ☰ button.
    /// </summary>
    private int ExpandedWidth(MenuStrip menu)
    {
        var width = menu.GetPreferredSize(Size.Empty).Width;
        return _collapsedMenus.TryGetValue(menu, out var collapsed)
            ? width - ItemWidth(collapsed.Button) + collapsed.ItemsWidth
            : width;
    }

    private static int ItemWidth(ToolStripItem item) => item.GetPreferredSize(Size.Empty).Width + item.Margin.Horizontal;

    /// <summary>
    /// Space for the menu: the title bar content without the other controls docked left and right; a menu with a fixed width
    /// gets no more than its width.
    /// </summary>
    private int SpaceFor(MenuStrip menu)
    {
        var others = Controls.Cast<Control>()
            .Where(c => c != menu && c.Visible && c.Dock is DockStyle.Left or DockStyle.Right)
            .Sum(c => c.Width);
        var space = DisplayRectangle.Width - others;
        return menu.AutoSize ? space : Math.Min(space, menu.Width);
    }

    /// <summary>
    /// Moves the menu items under a ☰ button at the start of the menu (other kinds of items stay in the menu) - the items are
    /// moved, so the commands, events and shortcut keys of the application keep working.
    /// </summary>
    /// <returns>the menu was collapsed</returns>
    private bool Collapse(MenuStrip menu)
    {
        if (_collapsedMenus.ContainsKey(menu))
            return false;
        var items = menu.Items.OfType<ToolStripMenuItem>().Select(item => (Item: item, Index: menu.Items.IndexOf(item))).ToList();
        if (items.Count == 0)
            return false;

        var button = new ToolStripMenuItem(Glyphs.Menu)
        {
            Font = MenuGlyphFont(),
            AccessibleName = "Menu",
            AccessibleRole = AccessibleRole.ButtonMenu
        };
        // rozbalovacia ponuka by zdedila pismo znaku tlacidla - polozky v nej maju pismo ponuky
        button.DropDown.Font = menu.Font;
        menu.FontChanged += Menu_FontChanged;
        var collapsed = new CollapsedMenu(button, items, items.Sum(i => ItemWidth(i.Item)));
        menu.SuspendLayout();
        foreach (var (item, _) in items)
            button.DropDownItems.Add(item);
        menu.Items.Insert(0, button);
        menu.ResumeLayout();
        _collapsedMenus.Add(menu, collapsed);
        return true;
    }

    /// <summary>
    /// Returns the items from the ☰ button to their places in the menu.
    /// </summary>
    /// <returns>the menu was expanded</returns>
    private bool Expand(MenuStrip menu)
    {
        if (!_collapsedMenus.TryGetValue(menu, out var collapsed))
            return false;

        _collapsedMenus.Remove(menu);
        menu.FontChanged -= Menu_FontChanged;
        menu.SuspendLayout();
        menu.Items.Remove(collapsed.Button);
        foreach (var (item, index) in collapsed.Items.OrderBy(i => i.Index))
            if (!item.IsDisposed)
                menu.Items.Insert(Math.Min(index, menu.Items.Count), item);
        menu.ResumeLayout();
        collapsed.Button.Dispose();
        return true;
    }

    private void Menu_FontChanged(object? sender, EventArgs e)
    {
        if (sender is MenuStrip menu && _collapsedMenus.TryGetValue(menu, out var collapsed))
            collapsed.Button.DropDown.Font = menu.Font;
    }

    private Font MenuGlyphFont()
    {
        var size = LogicalToDeviceUnits(MenuGlyphSize);
        if (_menuGlyphFont is not null && (int)_menuGlyphFont.Size == size)
            return _menuGlyphFont;

        _menuGlyphFont?.Dispose();
        _menuGlyphFont = new Font(Glyphs.MenuFamily, size, GraphicsUnit.Pixel);
        return _menuGlyphFont;
    }

    /// <inheritdoc />
    protected override void OnBackColorChanged(EventArgs e)
    {
        base.OnBackColorChanged(e);
        if (!_applyingTheme)
            UpdateStripColors();
    }

    /// <inheritdoc />
    protected override void OnForeColorChanged(EventArgs e)
    {
        base.OnForeColorChanged(e);
        if (!_applyingTheme)
            UpdateStripColors();
    }

    /// <inheritdoc />
    protected override void OnPaintBackground(PaintEventArgs e)
    {
        var back = CaptionBackColor;
        if (back == BackColor)
        {
            base.OnPaintBackground(e);
            return;
        }

        using var brush = new SolidBrush(back);
        e.Graphics.FillRectangle(brush, ClientRectangle);
    }

    /// <inheritdoc />
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        if (!ShowsCaption || _form is null)
            return;

        var layout = ComputeLayout();
        var fore = _formActive ? CaptionForeColor : EffectiveInactiveForeColor;
        DrawIcon(e.Graphics, layout.Icon);
        DrawTitle(e.Graphics, layout, fore);
        DrawButton(e.Graphics, layout.Minimize, TitleBarHit.MinimizeButton, Glyphs.Minimize, fore, layout.MinimizeEnabled);
        var maximize = _form.WindowState == FormWindowState.Maximized ? Glyphs.Restore : Glyphs.Maximize;
        DrawButton(e.Graphics, layout.Maximize, TitleBarHit.MaximizeButton, maximize, fore, layout.MaximizeEnabled);
        DrawButton(e.Graphics, layout.Help, TitleBarHit.HelpButton, HelpGlyph, fore, true);
        DrawButton(e.Graphics, layout.Close, TitleBarHit.CloseButton, Glyphs.Close, fore, true);

        if (!DrawsTopBorder)
            return;
        // s farbou motivu na zahlaviach ma okno vo Windows 10 okraj farbou motivu
        var border = !_topBorderColor.IsEmpty ? _topBorderColor : ShownAccent ?? TitleBarColors.TopBorder(CaptionBackColor, CaptionForeColor, _formActive);
        using var brush = new SolidBrush(border);
        e.Graphics.FillRectangle(brush, 0, 0, Width, 1);
    }

    private void DrawIcon(Graphics g, Rectangle area)
    {
        if (area.IsEmpty || SmallIcon() is not { } icon)
            return;

        var size = LogicalToDeviceUnits(IconSize);
        g.DrawIcon(icon, new Rectangle(area.Left + (area.Width - size) / 2, area.Top + (area.Height - size) / 2, size, size));
    }

    private void DrawTitle(Graphics g, TitleBarLayout layout, Color fore)
    {
        var text = _form?.Text;
        if (!_showTitle || string.IsNullOrEmpty(text))
            return;

        const TextFormatFlags flags = TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis |
                                      TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding;
        var margin = LogicalToDeviceUnits(TitleMargin);
        var area = Rectangle.FromLTRB(layout.LeftInset + margin, 0, Width - layout.RightInset - margin, Height);
        var occupied = Controls.Cast<Control>().Where(c => c.Visible).Select(c => Rectangle.Inflate(c.Bounds, margin, 0));
        // meria sa cely text - s EndEllipsis by MeasureText vratil skrateny na navrhovanu (nulovu) sirku
        var width = TextRenderer.MeasureText(g, text, CaptionFont, Size.Empty, flags & ~TextFormatFlags.EndEllipsis).Width;
        var rect = TitleBarLayout.PlaceTitle(width, area, occupied, Width / 2, LogicalToDeviceUnits(MinTitleWidth));
        if (!rect.IsEmpty)
            TextRenderer.DrawText(g, text, CaptionFont, rect, fore, flags);
    }

    private void DrawButton(Graphics g, Rectangle bounds, TitleBarHit button, string glyph, Color fore, bool enabled)
    {
        if (bounds.IsEmpty)
            return;

        if (enabled && _hotButton == button)
        {
            var pressed = _pressedButton == button;
            Color back;
            if (button == TitleBarHit.CloseButton)
            {
                var close = _closeButtonHoverBackColor.IsEmpty ? TitleBarColors.CloseHover(ExTools.IsWin11Build22000) : _closeButtonHoverBackColor;
                back = pressed ? TitleBarColors.ClosePressed(close, CaptionBackColor, ExTools.IsWin11Build22000) : close;
                fore = Color.White;
            }
            else if (pressed)
                back = _buttonPressedBackColor.IsEmpty ? TitleBarColors.ButtonPressed(CaptionBackColor, CaptionForeColor) : _buttonPressedBackColor;
            else
                back = _buttonHoverBackColor.IsEmpty ? TitleBarColors.ButtonHover(CaptionBackColor, CaptionForeColor) : _buttonHoverBackColor;

            using var brush = new SolidBrush(back);
            g.FillRectangle(brush, bounds);
        }
        else if (!enabled)
            fore = EffectiveInactiveForeColor;

        var font = button == TitleBarHit.HelpButton
            ? GlyphFont(ref _helpGlyphFont, HelpGlyphFamily, HelpGlyphSize)
            : GlyphFont(ref _glyphFont, Glyphs.Family, GlyphSize);
        TextRenderer.DrawText(g, glyph, font, bounds, fore,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix |
            TextFormatFlags.NoPadding);
    }

    private TitleBarLayout ComputeLayout()
    {
        if (!ShowsCaption || _form is not { } form)
            return TitleBarLayout.Compute(ClientSize, 0, 0, false, false, false, false, false);

        // nastrojove okno ma ako v systemovom zahlavi len tlacidlo zatvorenia
        var toolWindow = form.FormBorderStyle is FormBorderStyle.FixedToolWindow or FormBorderStyle.SizableToolWindow;
        return TitleBarLayout.Compute(ClientSize, LogicalToDeviceUnits(IconAreaWidth), LogicalToDeviceUnits(ButtonWidth),
            !toolWindow && ShowsIcon(form), form.ControlBox, form.MinimizeBox && !toolWindow, form.MaximizeBox && !toolWindow,
            form.HelpButton && !toolWindow);
    }

    /// <summary>
    /// The form has an icon as with the system caption: a dialog (FixedDialog) only its own one, not the default WinForms icon.
    /// </summary>
    private static bool ShowsIcon(Form form)
    {
        if (!form.ShowIcon || form.Icon is null)
            return false;
        return form.FormBorderStyle != FormBorderStyle.FixedDialog ||
               TypeDescriptor.GetProperties(form)[nameof(Form.Icon)]?.ShouldSerializeValue(form) == true;
    }

    private Icon? SmallIcon()
    {
        var size = LogicalToDeviceUnits(IconSize);
        if (_smallIcon is not null && _smallIcon.Width == size)
            return _smallIcon;

        _smallIcon?.Dispose();
        _smallIcon = _form?.Icon is { } icon ? new Icon(icon, size, size) : null;
        return _smallIcon;
    }

    private Font GlyphFont(ref Font? cached, string family, int logicalSize)
    {
        var size = LogicalToDeviceUnits(logicalSize);
        if (cached is not null && (int)cached.Size == size)
            return cached;

        cached?.Dispose();
        cached = new Font(family, size, GraphicsUnit.Pixel);
        return cached;
    }

    private void InvalidateButton(TitleBarHit? button)
    {
        if (button is { } b)
            Invalidate(ComputeLayout().BoundsOf(b));
    }

    private void SetColor(ref Color field, Color value)
    {
        if (field == value)
            return;
        field = value;
        Invalidate();
    }

    /// <summary>
    /// A place in the tool strip without a clickable item (empty space, separator, label without a link).
    /// </summary>
    private static bool IsFreeArea(ToolStrip strip, Point point) =>
        strip.GetItemAt(point) is null or ToolStripSeparator or ToolStripLabel { IsLink: false };

    /// <inheritdoc />
    protected override void SetVisibleCore(bool value)
    {
        _shown = value;
        base.SetVisibleCore(value);
        (Parent as ExForm)?.UpdateFrame();
    }

    /// <inheritdoc />
    protected override void OnParentChanged(EventArgs e)
    {
        if (_form is not null)
            _form.TextChanged -= Form_TextChanged;
        _form = Parent as Form;
        if (_form is not null)
            _form.TextChanged += Form_TextChanged;
        base.OnParentChanged(e);
    }

    private void Form_TextChanged(object? sender, EventArgs e) => Invalidate();

    /// <inheritdoc />
    protected override void OnControlAdded(ControlEventArgs e)
    {
        base.OnControlAdded(e);
        if (e.Control is not ToolStrip strip || _hooks.ContainsKey(strip))
            return;

        // automaticke rozlozenie by sa po ukotveni vlavo/vpravo prepolo na zvisle - v zahlavi su polozky vzdy vedla seba
        if (strip.LayoutStyle is ToolStripLayoutStyle.HorizontalStackWithOverflow or ToolStripLayoutStyle.VerticalStackWithOverflow)
            strip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
        _hooks.Add(strip, new ToolStripHook(this, strip));
        UpdateStripColors();
    }

    /// <inheritdoc />
    protected override void OnControlRemoved(ControlEventArgs e)
    {
        base.OnControlRemoved(e);
        if (e.Control is ToolStrip strip && _hooks.TryGetValue(strip, out var hook))
        {
            if (strip is MenuStrip menu)
                Expand(menu);
            hook.Detach();
            _hooks.Remove(strip);
            UpdateStripColors();
        }
    }

    /// <inheritdoc />
    protected override void WndProc(ref Message m)
    {
        // cele zahlavie vyhodnocuje formular (ExForm.WmNcHitTest) - samo je len plocha pod prvkami
        if (m.Msg == (int)Win32.WM.NCHITTEST && IsActive)
        {
            m.Result = (IntPtr)Win32.HitTest.HTTRANSPARENT;
            return;
        }

        base.WndProc(ref m);
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            foreach (var hook in _hooks.Values)
                hook.Detach();
            _hooks.Clear();
            if (_form is not null)
                _form.TextChanged -= Form_TextChanged;
            _glyphFont?.Dispose();
            _menuGlyphFont?.Dispose();
            _helpGlyphFont?.Dispose();
            _smallIcon?.Dispose();
        }

        base.Dispose(disposing);
    }

    /// <summary>
    /// A collapsed menu: the ☰ button, the moved items with their original positions and their width in the menu.
    /// </summary>
    private sealed record CollapsedMenu(ToolStripMenuItem Button, List<(ToolStripMenuItem Item, int Index)> Items, int ItemsWidth);

    /// <summary>
    /// A tool strip or item color taken over from the title bar - remembers the original one (set or inherited) and restores
    /// it only if the application has not changed the color in the meantime.
    /// </summary>
    private sealed class ColorOverride
    {
        private readonly PropertyDescriptor _property;
        private readonly bool _isSet;

        private ColorOverride(Component target, PropertyDescriptor property, Color requested)
        {
            Target = target;
            _property = property;
            Requested = requested;
            Applied = requested;
            Original = (Color)property.GetValue(target)!;
            // nenastavena farba sa dedi (z panela, z rodica) - vrati sa ako nenastavena
            _isSet = property.ShouldSerializeValue(target);
        }

        public Component Target { get; }

        public string Property => _property.Name;

        /// <summary>
        /// The color before the title bar changed it.
        /// </summary>
        public Color Original { get; }

        /// <summary>
        /// The color from the title bar.
        /// </summary>
        public Color Requested { get; }

        /// <summary>
        /// The color the target currently has (the original one for a selected item).
        /// </summary>
        public Color Applied { get; private set; }

        /// <summary>
        /// The target still has the color from the title bar.
        /// </summary>
        public bool IsCurrent => Target is not (Control { IsDisposed: true } or ToolStripItem { IsDisposed: true }) &&
                                 (Color)_property.GetValue(Target)! == Applied;

        public static ColorOverride Apply(Component target, PropertyDescriptor property, Color color)
        {
            var colorOverride = new ColorOverride(target, property, color);
            property.SetValue(target, color);
            return colorOverride;
        }

        /// <summary>
        /// Shows the other of the target colors (the original one or the one from the title bar) - only if the application has
        /// not changed the color.
        /// </summary>
        /// <param name="color">color to show</param>
        public void Show(Color color)
        {
            if (Applied == color || !IsCurrent)
                return;
            _property.SetValue(Target, color);
            Applied = color;
        }

        public void Restore()
        {
            if (!IsCurrent)
                return;
            if (_isSet)
                _property.SetValue(Target, Original);
            else
                _property.ResetValue(Target);
        }
    }

    /// <summary>
    /// Glyphs of the window buttons (and of the collapsed menu button) from the font of the system caption: Segoe Fluent Icons
    /// in Windows 11, Segoe MDL2 Assets in Windows 10 (even with Fluent Icons installed) - the glyphs have the same codes in both;
    /// without them Marlett (in every Windows).
    /// </summary>
    private sealed record CaptionGlyphs(string Family, string Minimize, string Maximize, string Restore, string Close, string Menu, string MenuFamily)
    {
        public static CaptionGlyphs Find()
        {
            string[] families = ExTools.IsWin11Build22000
                ? ["Segoe Fluent Icons", "Segoe MDL2 Assets"]
                : ["Segoe MDL2 Assets", "Segoe Fluent Icons"];
            using var fonts = new InstalledFontCollection();
            foreach (var family in families)
                if (fonts.Families.Any(f => f.Name == family))
                    return new CaptionGlyphs(family, "\uE921", "\uE922", "\uE923", "\uE8BB", "\uE700", family);

            return new CaptionGlyphs("Marlett", "0", "1", "2", "r", "\u2261", "Segoe UI");
        }
    }

    /// <summary>
    /// Passes WM_NCHITTEST from the free space of a tool strip to the title bar (HTTRANSPARENT) so that it drags the window,
    /// and sets the colors of selected items before the tool strip is painted.
    /// </summary>
    private sealed class ToolStripHook : NativeWindow
    {
        private readonly ExTitleBar _owner;
        private readonly ToolStrip _strip;

        public ToolStripHook(ExTitleBar owner, ToolStrip strip)
        {
            _owner = owner;
            _strip = strip;
            _strip.HandleCreated += Strip_HandleCreated;
            _strip.HandleDestroyed += Strip_HandleDestroyed;
            _strip.Paint += Strip_Paint;
            if (_strip.IsHandleCreated)
                AssignHandle(_strip.Handle);
        }

        public void Detach()
        {
            _strip.HandleCreated -= Strip_HandleCreated;
            _strip.HandleDestroyed -= Strip_HandleDestroyed;
            _strip.Paint -= Strip_Paint;
            ReleaseHandle();
        }

        private void Strip_HandleCreated(object? sender, EventArgs e) => AssignHandle(_strip.Handle);

        private void Strip_HandleDestroyed(object? sender, EventArgs e) => ReleaseHandle();

        private void Strip_Paint(object? sender, PaintEventArgs e) => _owner.ShowItemStates(_strip);

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)Win32.WM.NCHITTEST && _owner.IsActive && IsFreeArea(_strip, _strip.PointToClient(TitleBarHitTest.ScreenPoint(m.LParam))))
            {
                m.Result = (IntPtr)Win32.HitTest.HTTRANSPARENT;
                return;
            }

            base.WndProc(ref m);
        }
    }
}
