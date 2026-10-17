using System.Drawing.Text;

namespace ExControls;

/// <summary>
/// Vlastne zahlavie okna <see cref="ExForm" />. Ako priamy potomok formulara (ukotveny hore) nahradi systemove zahlavie:
/// vlavo ikona okna (systemova ponuka), vpravo tlacidla okna, medzi nimi prvky zahlavia (napr. hlavna ponuka) a text okna
/// vo volnom mieste. Volne miesto a prazdne casti panelov nastrojov tahaju okno, ich polozky ostavaju klikatelne.
/// </summary>
/// <remarks>
/// Spracuju sa len panely nastrojov priamo v zahlavi (polozky maju vzdy vedla seba); ostatne prvky v zahlavi su klikatelne cele.
/// Neaktivne okno ma stlmeny text okna, znacky tlacidiel aj polozky ponuky a popisy v tychto paneloch (ako systemova ponuka)
/// - cez <see cref="ToolStripItem.ForeColor" />, takze renderer, ktory farbu textu poloziek urcuje sam, stlmenie neukaze.
/// Tlacidla a ikona su ako v systemovom zahlavi: pomocnik (<see cref="Form.HelpButton" />) len bez minimalizacie
/// a maximalizacie, nastrojove okno len zatvorenie, dialog (FixedDialog) bez vlastnej ikony bez ikony.
/// Farby titulku formulara (<see cref="ExForm.TitleBarBackColor" />, <see cref="ExForm.TitleBarForeColor" />) maju prednost
/// pred vlastnymi farbami zahlavia; prvky v zahlavi si nechavaju svoje.
/// Ked okno vlastne zahlavie nepouziva (<see cref="ExForm.UsesCustomTitleBar" />), je to obycajny panel bez ikony, textu a tlacidiel.
/// </remarks>
[Designer("ExControls.Designers.ExTitleBarDesigner, ExControls")]
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
    private const int TitleMargin = 8;
    private const int MinTitleWidth = 40;

    private static readonly CaptionGlyphs Glyphs = CaptionGlyphs.Find();

    // text okna pismom systemoveho zahlavia (vo Windows 10 a 11 Segoe UI 9 b.) ako titulok ostatnych okien,
    // nie pismom prvku - to aplikacie nastavuju podla konfiguracie
    private static readonly Font CaptionFont = SystemFonts.CaptionFont ?? DefaultFont;

    private readonly Dictionary<ToolStrip, ToolStripHook> _hooks = [];
    private readonly Dictionary<ToolStripItem, DimmedItem> _dimmedItems = [];
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
    /// Vytvori zahlavie ukotvene hore.
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
    /// <remarks>Prvky zahlavia sa ukladaju medzi ikonu a tlacidla okna.</remarks>
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
    /// Zahlavie ma byt zobrazene (vlastna hodnota <see cref="Control.Visible" /> - nezavisi od toho, ci je zobrazeny formular).
    /// </summary>
    internal bool Shown => _shown;


    /// <summary>
    /// Zahlavie prave nahradza systemove zahlavie svojho formulara.
    /// </summary>
    private bool IsActive => Parent is ExForm form && form.TitleBar == this && form.UsesCustomTitleBar;

    /// <summary>
    /// Zahlavie kresli ikonu, text a tlacidla okna - za behu s vlastnym zahlavim, v navrhari vzdy (nahlad).
    /// </summary>
    private bool ShowsCaption => Parent is ExForm form && form.TitleBar == this && (form.UsesCustomTitleBar || DesignMode);

    /// <summary>
    /// Horny okraj okna kresli zahlavie len vo Windows 10 (vo Windows 11 ho kresli system) a nie pri maximalizacii.
    /// </summary>
    private bool DrawsTopBorder => ShowsCaption && !ExTools.IsWin11Build22000 && _form is { WindowState: not FormWindowState.Maximized };

    private Color EffectiveInactiveForeColor => _inactiveForeColor.IsEmpty ? TitleBarColors.Inactive(CaptionBackColor, CaptionForeColor) : _inactiveForeColor;

    /// <summary>
    /// Pozadie zahlavia: farba titulku formulara (<see cref="ExForm.TitleBarBackColor" />), ak je zadana, inak vlastna.
    /// </summary>
    private Color CaptionBackColor => ShowsCaption && Parent is ExForm { TitleBarBackColor.IsEmpty: false } form ? form.TitleBarBackColor : BackColor;

    /// <summary>
    /// Text a znacky tlacidiel: farba textu titulku formulara (<see cref="ExForm.TitleBarForeColor" />), ak je zadana, inak vlastna.
    /// </summary>
    private Color CaptionForeColor => ShowsCaption && Parent is ExForm { TitleBarForeColor.IsEmpty: false } form ? form.TitleBarForeColor : ForeColor;

    /// <summary>
    /// Urci cast zahlavia v bode.
    /// </summary>
    /// <param name="point">bod v klientskych suradniciach zahlavia</param>
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
    /// Vyska podla prvkov zahlavia (pri <see cref="Control.AutoSize" />): najvyssi viditelny prvok s odsadenim, aspon 32 px pri 96 DPI.
    /// </summary>
    public override Size GetPreferredSize(Size proposedSize)
    {
        var content = Controls.Cast<Control>().Where(c => c.Visible).Select(c => c.GetPreferredSize(Size.Empty).Height).DefaultIfEmpty(0).Max();
        var top = DrawsTopBorder ? Math.Max(Padding.Top, 1) : Padding.Top;
        return new Size(Width, Math.Max(LogicalToDeviceUnits(MinHeight), content + top + Padding.Bottom));
    }

    /// <summary>
    /// Nastavi farby zahlavia podla temy (pozadie a text panelov) a pouzije temu na prvky v zahlavi.
    /// </summary>
    public void ApplyTheme(ExTheme theme)
    {
        ArgumentNullException.ThrowIfNull(theme);

        // tema nastavuje farby poloziek panelov - stlmene sa vratia a po teme stlmia novou farbou
        RestoreItems();
        BackColor = theme.PanelBackColor;
        ForeColor = theme.PanelForeColor;
        ExThemer.Apply(Controls, theme);
        if (!_formActive)
            DimItems();
        Invalidate(true);
    }

    /// <summary>
    /// Formular je aktivny (WM_NCACTIVATE) - neaktivny ma text a znacky tlacidiel tlmene.
    /// </summary>
    internal void SetFormActive(bool active)
    {
        if (_formActive == active)
            return;
        _formActive = active;
        if (active)
            RestoreItems();
        else
            DimItems();
        Invalidate();
    }

    /// <summary>
    /// Stlmi text poloziek ponuky a popisov v paneloch zahlavia (len vlastne zahlavie za behu).
    /// </summary>
    private void DimItems()
    {
        if (!IsActive)
            return;

        var dimmed = EffectiveInactiveForeColor;
        foreach (var strip in _hooks.Keys)
            foreach (ToolStripItem item in strip.Items)
            {
                if (item is not (ToolStripMenuItem or ToolStripLabel) || _dimmedItems.ContainsKey(item))
                    continue;
                // nenastavena farba sa dedi z panela - po aktivacii ju treba vratit ako nenastavenu
                var isSet = TypeDescriptor.GetProperties(item)[nameof(ToolStripItem.ForeColor)]?.ShouldSerializeValue(item) == true;
                _dimmedItems[item] = new DimmedItem(item.ForeColor, isSet, dimmed);
                item.ForeColor = dimmed;
            }
    }

    /// <summary>
    /// Vrati stlmenym polozkam povodnu farbu - okrem tych, ktorym ju medzicasom zmenila aplikacia.
    /// </summary>
    private void RestoreItems()
    {
        foreach (var pair in _dimmedItems)
        {
            var item = pair.Key;
            var state = pair.Value;
            if (item.IsDisposed || item.ForeColor != state.Dimmed)
                continue;
            if (state.IsSet)
                item.ForeColor = state.Original;
            else
                item.ResetForeColor();
        }

        _dimmedItems.Clear();
    }

    /// <summary>
    /// Mys je nad tlacidlom okna (alebo inde v zahlavi - <see langword="null" />).
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
    /// Tlacidlo okna je stlacene.
    /// </summary>
    internal void PressButton(TitleBarHit button)
    {
        _pressedButton = button;
        TrackButton(button);
        InvalidateButton(button);
    }

    /// <summary>
    /// Mys je uvolnena nad tlacidlom <paramref name="button" />.
    /// </summary>
    /// <returns>tlacidlo bolo aj stlacene - je to klik</returns>
    internal bool ReleaseButton(TitleBarHit button)
    {
        var clicked = _pressedButton == button;
        InvalidateButton(_pressedButton);
        _pressedButton = null;
        return clicked;
    }

    /// <summary>
    /// Mys opustila zahlavie - ziadne tlacidlo nie je pod mysou ani stlacene.
    /// </summary>
    internal void ResetButtons()
    {
        InvalidateButton(_hotButton);
        InvalidateButton(_pressedButton);
        _hotButton = null;
        _pressedButton = null;
    }

    /// <summary>
    /// Formular zmenil ikonu, tlacidla, stav okna alebo ram - nove rozlozenie a prekreslenie.
    /// </summary>
    internal void RefreshCaption()
    {
        _smallIcon?.Dispose();
        _smallIcon = null;
        PerformLayout();
        Invalidate();
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
        var border = _topBorderColor.IsEmpty ? TitleBarColors.TopBorder(CaptionBackColor, CaptionForeColor, _formActive) : _topBorderColor;
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
    /// Okno ma ikonu ako pri systemovom zahlavi: dialog (FixedDialog) len vlastnu, nie predvolenu ikonu WinForms.
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
    /// Miesto panela nastrojov bez klikatelnej polozky (prazdne, oddelovac, popis bez odkazu).
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
    }

    /// <inheritdoc />
    protected override void OnControlRemoved(ControlEventArgs e)
    {
        base.OnControlRemoved(e);
        if (e.Control is ToolStrip strip && _hooks.TryGetValue(strip, out var hook))
        {
            hook.Detach();
            _hooks.Remove(strip);
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
            _helpGlyphFont?.Dispose();
            _smallIcon?.Dispose();
        }

        base.Dispose(disposing);
    }

    /// <summary>
    /// Farba polozky pred stlmenim: <paramref name="IsSet" /> - nastavena (inak zdedena z panela), <paramref name="Dimmed" /> -
    /// stlmena farba (ak ju polozka uz nema, zmenila ju aplikacia).
    /// </summary>
    private readonly record struct DimmedItem(Color Original, bool IsSet, Color Dimmed);

    /// <summary>
    /// Znaky tlacidiel okna z pisma systemoveho zahlavia: Windows 11 Segoe Fluent Icons, Windows 10 Segoe MDL2 Assets
    /// (aj ked ma Fluent Icons nainstalovane) - znaky maju v oboch rovnake kody; bez nich Marlett (v kazdych Windows).
    /// </summary>
    private sealed record CaptionGlyphs(string Family, string Minimize, string Maximize, string Restore, string Close)
    {
        public static CaptionGlyphs Find()
        {
            string[] families = ExTools.IsWin11Build22000
                ? ["Segoe Fluent Icons", "Segoe MDL2 Assets"]
                : ["Segoe MDL2 Assets", "Segoe Fluent Icons"];
            using var fonts = new InstalledFontCollection();
            foreach (var family in families)
                if (fonts.Families.Any(f => f.Name == family))
                    return new CaptionGlyphs(family, "\uE921", "\uE922", "\uE923", "\uE8BB");

            return new CaptionGlyphs("Marlett", "0", "1", "2", "r");
        }
    }

    /// <summary>
    /// Posiela WM_NCHITTEST z volneho miesta panela nastrojov zahlaviu (HTTRANSPARENT), aby nim slo tahat okno.
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
            if (_strip.IsHandleCreated)
                AssignHandle(_strip.Handle);
        }

        public void Detach()
        {
            _strip.HandleCreated -= Strip_HandleCreated;
            _strip.HandleDestroyed -= Strip_HandleDestroyed;
            ReleaseHandle();
        }

        private void Strip_HandleCreated(object? sender, EventArgs e) => AssignHandle(_strip.Handle);

        private void Strip_HandleDestroyed(object? sender, EventArgs e) => ReleaseHandle();

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
