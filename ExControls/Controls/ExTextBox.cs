using ExControls.Controls;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace ExControls;

/// <summary>
///     Expanded TextBox Control
/// </summary>
[ToolboxBitmap(typeof(TextBox), "TextBox.bmp")]
[Designer("ExControls.Designers.ExTextBoxDesigner, ExControls")]
[DefaultProperty(nameof(Text))]
[DefaultEvent(nameof(TextChanged))]
public class ExTextBox : TextBox, IExControl
{
    private const int RgnDiff = 0x4;

    private Color _borderColor;
    private int _borderThickness;

    private bool _defaultStyle;
    private Color _disabledBackColor;
    private Color _disabledBorderColor;
    private Color _disabledForeColor;
    private Color _highlightColor;
    private Color _hintForeColor;
    private string? _hintText;
    private bool _hover;
    private bool _selected;

    /// <summary>
    ///     Constructor
    /// </summary>
    public ExTextBox()
    {
        _defaultStyle = true;
        _borderColor = Color.DimGray;
        _highlightColor = SystemColors.Highlight;
        _borderThickness = 1;
        _disabledBorderColor = SystemColors.InactiveBorder;
        _disabledBackColor = SystemColors.Control;
        _disabledForeColor = SystemColors.GrayText;
        _hintText = null;
        _hintForeColor = SystemColors.GrayText;
        Invalidate();
    }

    /// <summary>
    ///     Color of the TextBox's border.
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "Black")]
    [ExDescription("Color of the TextBox's border.")]
    public Color BorderColor
    {
        get => _borderColor;
        set
        {
            if (_borderColor == value)
                return;
            _borderColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Color of the TextBox's border when it is disabled.
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "InactiveBorder")]
    [ExDescription("Color of the TextBox's border when it is disabled.")]
    public Color DisabledBorderColor
    {
        get => _disabledBorderColor;
        set
        {
            if (_disabledBorderColor == value)
                return;
            _disabledBorderColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Background color of the TextBox's when it is disabled.
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "Control")]
    [ExDescription("Background color of the TextBox's when it is disabled.")]
    public Color DisabledBackColor
    {
        get => _disabledBackColor;
        set
        {
            if (_disabledBackColor == value)
                return;
            _disabledBackColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Foreground color of the TextBox's when it is disabled.
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "GrayText")]
    [ExDescription("Foreground color of the TextBox's when it is disabled.")]
    public Color DisabledForeColor
    {
        get => _disabledForeColor;
        set
        {
            if (_disabledForeColor == value)
                return;
            _disabledForeColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Color of the border of TextBox when mouse is over the Control.
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "Highlight")]
    [ExDescription("Color of the border of TextBox when mouse is over the Control.")]
    public Color HighlightColor
    {
        get => _highlightColor;
        set
        {
            if (_highlightColor == value)
                return;
            _highlightColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Width of the TextBox's border.
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(1)]
    [ExDescription("Width of the TextBox's border.")]
    public int BorderThickness
    {
        get => _borderThickness;
        set
        {
            if (_borderThickness == value)
                return;
            _borderThickness = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Hint text for TextBox.
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("Hint text for TextBox.")]
    public string? HintText
    {
        get => _hintText;
        set
        {
            if (_hintText == value)
                return;
            _hintText = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     TextBox hint foreground color.
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("TextBox hint foreground color.")]
    [DefaultValue(typeof(SystemColors), "GrayText")]
    public Color HintForeColor
    {
        get => _hintForeColor;
        set
        {
            if (_hintForeColor == value)
                return;
            _hintForeColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Use dark (DarkMode_Explorer) scroll bars for the multiline TextBox.
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(false)]
    [ExDescription("Use dark (DarkMode_Explorer) scroll bars.")]
    public bool UseDarkScrollBar
    {
        get;
        set
        {
            if (field == value)
                return;
            field = value;
            ApplyScrollBarTheme();
        }
    }

    /// <summary>
    ///     Nastavi temu okna podla UseDarkScrollBar (tema DarkMode_Explorer da tmave scrollbary).
    /// </summary>
    private void ApplyScrollBarTheme()
    {
        if (!IsHandleCreated || DesignMode)
            return;

        this.SetTheme(UseDarkScrollBar ? WindowsTheme.DarkExplorer : WindowsTheme.Default);
        EnsureNativeBorder();
        DrawBorder();
    }

    /// <inheritdoc />
    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        if (UseDarkScrollBar)
            ApplyScrollBarTheme();
        else
            EnsureNativeBorder();
    }

    /// <summary>
    ///     Tematicky Edit (comctl32 v6, Windows 11) pri vytvoreni okna odstrani WS_BORDER a ramik kresli sam
    ///     v klientskej oblasti - neklientska oblast je potom nulova a scrollbar siaha az po okraj okna, takze
    ///     prekryva nas ramik. WS_BORDER sa preto vrati, aby system vyhradil skutocny 1px ram a scrollbar
    ///     ostal vnutri neho.
    /// </summary>
    private void EnsureNativeBorder()
    {
        if (DefaultStyle || !IsHandleCreated || DesignMode)
            return;

        var style = (uint)Win32.GetWindowLongPtr(Handle, Win32.GWL_STYLE).ToInt64();
        if ((style & (uint)Win32.WindowStyles.WS_BORDER) != 0)
            return;

        Win32.SetWindowLong(Handle, Win32.GWL_STYLE, style | (uint)Win32.WindowStyles.WS_BORDER);
        Win32.SetWindowPos(Handle, IntPtr.Zero, 0, 0, 0, 0,
            Win32.SetWindowPosFlags.FrameChanged | Win32.SetWindowPosFlags.IgnoreMove | Win32.SetWindowPosFlags.IgnoreResize
            | Win32.SetWindowPosFlags.IgnoreZOrder | Win32.SetWindowPosFlags.DoNotActivate);
    }

    /// <summary>Occurs when the <see cref="IExControl.DefaultStyle" /> property changes.</summary>
    [ExCategory("Changed Property")]
    [ExDescription("Occurs when the BorderColor property changes.")]
    public event EventHandler? DefaultStyleChanged;

    /// <inheritdoc />
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(true)]
    [ExDescription("Default style of the Control.")]
    public bool DefaultStyle
    {
        get => _defaultStyle;
        set
        {
            if (_defaultStyle == value)
                return;
            _defaultStyle = value;
            BorderStyle = value ? BorderStyle.Fixed3D : BorderStyle.FixedSingle;
        }
    }

    /// <inheritdoc />
    protected override void OnEnabledChanged(EventArgs e)
    {
        Invalidate();
        base.OnEnabledChanged(e);
    }

    /// <inheritdoc />
    protected override void OnCreateControl()
    {
        base.OnCreateControl();

        if (!DefaultStyle)
        {
            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
        }
    }

    /// <inheritdoc />
    protected override void WndProc(ref Message m)
    {
        if (!DefaultStyle && m.Msg == (int)Win32.WM.PAINT && m.WParam == IntPtr.Zero && IsHandleCreated)
        {
            PaintBuffered(ref m);
            return;
        }

        if (!DefaultStyle && m.Msg == (int)Win32.WM.ERASEBKGND)
        {
            // Pozadie vyplni Edit v PaintBuffered, mazanie priamo na obrazovke by len blikalo.
            m.Result = (IntPtr)1;
            return;
        }

        base.WndProc(ref m);

        if (!DefaultStyle && m.Msg == (int)Win32.WM.THEMECHANGED)
            EnsureNativeBorder();

        if (!DefaultStyle && m.Msg == (int)Win32.WM.NCPAINT)
        {
            // Neklientsku oblast (ramik + scrollbary) necha vykreslit system a az potom sa cez systemovy ramik
            // nakresli vlastny. Ak by sa WM_NCPAINT zahodil, scrollbary by sa nevykreslili, kym ich edit sam neprekresli.
            DrawBorder();
            return;
        }

        if (m.Msg == (int)Win32.WM.PAINT)
            DrawHint(Graphics.FromHwnd(m.HWnd));
    }

    private const int PrfClient = 0x0004;
    private const int PrfEraseBkgnd = 0x0008;

    /// <summary>
    ///     WM_PAINT bez blikania: Edit nakresli obsah (aj svoj tematicky vnutorny ramik) cez WM_PRINTCLIENT do bufferu,
    ///     v nom sa ramik prekryje (OnPaint) a na obrazovku ide jeden blit. Kreslenie priamo na obrazovku
    ///     (Edit a potom my) sposobovalo pri hoveri blikanie vnutorneho ramika.
    /// </summary>
    private void PaintBuffered(ref Message m)
    {
        var hdc = Win32.BeginPaint(m.HWnd, out var ps);
        try
        {
            var client = ClientRectangle;
            if (client.Width > 0 && client.Height > 0)
            {
                using var buffer = BufferedGraphicsManager.Current.Allocate(hdc, client);
                // Multiline Edit vyplni len formatovaci obdlznik, pas medzi nim a okrajom by ostal z bufferu cierny
                buffer.Graphics.Clear(Enabled ? BackColor : DisabledBackColor);
                var memHdc = buffer.Graphics.GetHdc();
                try
                {
                    Win32.SendMessage(Handle, (uint)Win32.WM.PRINTCLIENT, memHdc, (IntPtr)(PrfClient | PrfEraseBkgnd));
                }
                finally
                {
                    buffer.Graphics.ReleaseHdc(memHdc);
                }

                OnPaint(new PaintEventArgs(buffer.Graphics, client));
                buffer.Render(hdc);
            }
        }
        finally
        {
            Win32.EndPaint(m.HWnd, ref ps);
        }

        m.Result = IntPtr.Zero;
    }

    /// <summary>
    ///     Nakresli vlastny ramik do neklientskej oblasti okna (cez system. ramik BorderStyle.FixedSingle).
    /// </summary>
    private void DrawBorder()
    {
        if (!IsHandleCreated)
            return;

        var hdc = Win32.GetWindowDC(Handle);
        var rgn = Win32.CreateRectRgn(0, 0, Width, Height);
        var inner = Win32.CreateRectRgn(BorderThickness, BorderThickness, Width - BorderThickness, Height - BorderThickness);
        var border = _hover || _selected ? HighlightColor : BorderColor;
        if (!Enabled) border = DisabledBorderColor;
        var brush = Win32.CreateSolidBrush(Win32.RGBtoInt(border));

        Win32.CombineRgn(rgn, rgn, inner, RgnDiff);
        Win32.FillRgn(hdc, rgn, brush);

        Win32.ReleaseDC(Handle, hdc);
        Win32.DeleteObject(inner);
        Win32.DeleteObject(rgn);
        Win32.DeleteObject(brush);
    }

    /// <inheritdoc />
    protected override void OnPaint(PaintEventArgs e)
    {
        DrawHint(e.Graphics);

        if (DefaultStyle) 
            return;

        if (!Enabled)
        {
            using var back = new SolidBrush(DisabledBackColor);
            e.Graphics.FillRectangle(back, ClientRectangle);
            TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, DisabledForeColor, DisabledBackColor, ConvertAligment(TextAlign));
        }
        else if (IsHandleCreated)
        {
            // Tematicky Edit si kresli vlastny 1px ramik po okraji klientskej oblasti (kvoli nemu odstranil WS_BORDER,
            // pozri EnsureNativeBorder) - prekryje sa farbou pozadia, nas ramik je v neklientskej oblasti.
            using var pen = new Pen(BackColor);
            e.Graphics.DrawRectangle(pen, 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
        }

        DrawBorder();
    }

    /// <summary>
    ///     Draws a hint text on TextBox
    /// </summary>
    /// <param name="g"></param>
    protected virtual void DrawHint(Graphics g)
    {
        if (!Focused && string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(HintText))
            TextRenderer.DrawText(g, HintText, Font, ClientRectangle, HintForeColor, BackColor, ConvertAligment(TextAlign));
    }

    internal static TextFormatFlags ConvertAligment(HorizontalAlignment alignment)
    {
        return alignment switch
        {
            HorizontalAlignment.Center => TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter,
            HorizontalAlignment.Right => TextFormatFlags.VerticalCenter | TextFormatFlags.Right,
            _ => TextFormatFlags.VerticalCenter | TextFormatFlags.Left
        };
    }

    /// <inheritdoc />
    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        if (DefaultStyle)
            return;

        if (!_hover)
        {
            _hover = true;
            Invalidate();
        }
    }

    /// <inheritdoc />
    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        if (DefaultStyle)
            return;

        if (_hover && !_selected)
        {
            _hover = false;
            Invalidate();
        }
    }

    /// <inheritdoc />
    protected override void OnGotFocus(EventArgs e)
    {
        base.OnGotFocus(e);
        if (DefaultStyle)
            return;

        if (!_selected)
        {
            _hover = true;
            _selected = true;
            Invalidate();
        }
    }

    /// <inheritdoc />
    protected override void OnLostFocus(EventArgs e)
    {
        base.OnLostFocus(e);
        if (DefaultStyle)
            return;

        if (_selected)
        {
            _hover = false;
            _selected = false;
            Invalidate();
        }
    }

    /// <inheritdoc />
    protected override void OnEnter(EventArgs e)
    {
        base.OnEnter(e);
        if (DefaultStyle)
            return;

        if (!_selected)
        {
            _hover = true;
            _selected = true;
            Invalidate();
        }
    }

    /// <inheritdoc />
    protected override void OnLeave(EventArgs e)
    {
        base.OnLeave(e);
        if (DefaultStyle)
            return;

        if (_selected)
        {
            _hover = false;
            _selected = false;
            Invalidate();
        }
    }

    /// <summary>Raises the <see cref="IExControl.DefaultStyleChanged" /> event.</summary>
    protected virtual void OnDefaultStyleChanged()
    {
        DefaultStyleChanged?.Invoke(this, EventArgs.Empty);
    }
}

