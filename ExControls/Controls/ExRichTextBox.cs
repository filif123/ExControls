using ExControls.Controls;

namespace ExControls;

/// <summary>
///     Expanded RichTextBox Control
/// </summary>
[ToolboxBitmap(typeof(RichTextBox), "RichTextBox.bmp")]
public class ExRichTextBox : RichTextBox, IExControl
{
    private Color _borderColor;
    private int _borderThickness;
    private bool _defaultStyle;
    private Color _disabledBackColor;
    private Color _disabledBorderColor;
    private Color _disabledForeColor;
    private Color _highlightColor;

    private bool _hover;
    private bool _selected;

    /// <summary>
    ///     Constructor
    /// </summary>
    public ExRichTextBox()
    {
        _defaultStyle = true;
        _borderColor = Color.DimGray;
        _highlightColor = SystemColors.Highlight;
        _borderThickness = 1;
        _disabledBorderColor = SystemColors.InactiveBorder;
        _disabledBackColor = SystemColors.Control;
        _disabledForeColor = SystemColors.GrayText;

        Invalidate();
    }

    /// <summary>
    /// </summary>
    public new BorderStyle BorderStyle
    {
        get => base.BorderStyle;
        set
        {
            if (base.BorderStyle == value)
                return;

            if (!DefaultStyle)
                base.BorderStyle = BorderStyle.None;
        }
    }

    /// <summary>
    ///     Color of the TextBox's border
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "Black")]
    [Description("Color of the TextBox's border.")]
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
    ///     Color of the TextBox's border when it is disabled
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "InactiveBorder")]
    [Description("Color of the TextBox's border when it is disabled.")]
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
    ///     Background color of the TextBox's when it is disabled
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "Control")]
    [Description("Background color of the TextBox's when it is disabled.")]
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
    ///     Foreground color of the TextBox's when it is disabled
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "GrayText")]
    [Description("Foreground color of the TextBox's when it is disabled.")]
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
    ///     Color of the border of TextBox when mouse is over the Control
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "Highlight")]
    [Description("Color of the border of TextBox when mouse is over the Control.")]
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
    ///     Width of the TextBox's border
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(1)]
    [Description("Width of the TextBox's border.")]
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

    /// <summary>Occurs when the <see cref="IExControl.DefaultStyle" /> property changed.</summary>
    public event EventHandler DefaultStyleChanged;

    /// <inheritdoc />
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(true)]
    [Description("Default style of the Control.")]
    public bool DefaultStyle
    {
        get => _defaultStyle;
        set
        {
            if (_defaultStyle == value)
                return;
            _defaultStyle = value;
            if (!value)
            {
                base.BorderStyle = BorderStyle.None;
                return;
            }

            Invalidate();
        }
    }

    /// <inheritdoc />
    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);

        if (!DefaultStyle && m.Msg == Win32.WM.PAINT.ToInt())
        {
            using var args = new PaintEventArgs(Graphics.FromHwnd(Handle), ClientRectangle);
            OnPaint(args);
            if (!DefaultStyle)
                m.Result = IntPtr.Zero;
        }
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.</summary>
    /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data. </param>
    protected override void OnPaint(PaintEventArgs e)
    {
        if (DefaultStyle)
            return;

        if (!Enabled)
        {
            e.Graphics.FillRectangle(new SolidBrush(DisabledBackColor), ClientRectangle);
            TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, DisabledForeColor, DisabledBackColor, TextFormatFlags.TextBoxControl);
        }

        var border = _hover || _selected ? HighlightColor : BorderColor;
        var rec = e.ClipRectangle;
        rec.Height -= 1;
        rec.Width -= 1;

        e.Graphics.DrawRectangle(new Pen(border, BorderThickness), rec);
    }

    /// <inheritdoc />
    protected override void OnMouseEnter(EventArgs eventargs)
    {
        base.OnMouseEnter(eventargs);
        if (DefaultStyle)
            return;

        if (!_hover)
        {
            _hover = true;
            Invalidate();
        }
    }

    /// <inheritdoc />
    protected override void OnMouseLeave(EventArgs eventargs)
    {
        base.OnMouseLeave(eventargs);
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

    /// <summary>Raises the <see cref="E:System.Windows.Forms.RichTextBox.HScroll" /> event.</summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
    protected override void OnHScroll(EventArgs e)
    {
        base.OnHScroll(e);
        if (!DefaultStyle) Invalidate();
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.RichTextBox.VScroll" /> event.</summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
    protected override void OnVScroll(EventArgs e)
    {
        base.OnVScroll(e);
        if (!DefaultStyle) Invalidate();
    }

    /// <summary>Raises the <see cref="IExControl.DefaultStyleChanged" /> event.</summary>
    protected virtual void OnDefaultStyleChanged()
    {
        DefaultStyleChanged?.Invoke(this, EventArgs.Empty);
    }
}