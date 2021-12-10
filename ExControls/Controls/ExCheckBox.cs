using ExControls.Controls;

namespace ExControls;

/// <summary>
///     Expanded CheckBox Control
/// </summary>
[ToolboxBitmap(typeof(CheckBox), "CheckBox.bmp")]
public class ExCheckBox : CheckBox, IExControl, ICheckableExControl
{
    private const int BOX_SIZE = 16;
    private const int BOX_OFFSET = 3;

    private Color _borderColor;
    private Color _boxBackColor;
    private bool _defaultStyle;
    private Color _disabledForeColor;
    private Color _highlightColor;

    private bool _hover;
    private Color _markColor;

    /// <summary>
    ///     Constructor
    /// </summary>
    public ExCheckBox()
    {
        _defaultStyle = true;
        _borderColor = Color.Black;
        _markColor = Color.Black;
        _boxBackColor = Color.White;
        _highlightColor = SystemColors.Highlight;
        _disabledForeColor = Color.DimGray;

        Invalidate();
    }

    /// <summary>
    ///     Color of the CheckBox's text and box when the Control is disabled
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "DimGray")]
    [Description("Color of the CheckBox's text and box when the Control is disabled.")]
    public Color DisabledForeColor
    {
        get => _disabledForeColor;
        set
        {
            if (_disabledForeColor == value)
                return;
            _disabledForeColor = value;
            Invalidate();
            OnDisabledForeColorChanged();
        }
    }

    /// <summary>
    ///     Color of the CheckBox's border
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "Black")]
    [Description("Color of the CheckBox's border.")]
    public Color BorderColor
    {
        get => _borderColor;
        set
        {
            if (_borderColor == value)
                return;
            _borderColor = value;
            Invalidate();
            OnBorderColorChanged();
        }
    }

    /// <summary>
    ///     Color of the CheckBox's mark
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "Black")]
    [Description("Color of the CheckBox's mark.")]
    public Color MarkColor
    {
        get => _markColor;
        set
        {
            if (_markColor == value)
                return;
            _markColor = value;
            Invalidate();
            OnMarkColorChanged();
        }
    }

    /// <summary>
    ///     Background color of CheckBox's square
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "Black")]
    [Description("Background color of CheckBox's square.")]
    public Color BoxBackColor
    {
        get => _boxBackColor;
        set
        {
            if (_boxBackColor == value)
                return;
            _boxBackColor = value;
            Invalidate();
            OnBoxBackColorChanged();
        }
    }

    /// <summary>
    ///     Foreground color of text if Control is disabled
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "Highlight")]
    [Description("Color of the border and mark of CheckBox when mouse is over the Control.")]
    public Color HighlightColor
    {
        get => _highlightColor;
        set
        {
            if (_highlightColor == value)
                return;
            _highlightColor = value;
            Invalidate();
            OnHighlightColorChanged();
        }
    }

    /// <summary>Occurs when the <see cref="DefaultStyle" /> property changes.</summary>
    [ExCategory("Changed Property")]
    [Description("Occurs when the DefaultStyle property changes.")]
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
            Invalidate();
            OnDefaultStyleChanged();
        }
    }

    /// <summary>Occurs when the <see cref="DisabledForeColor" /> property changes.</summary>
    [ExCategory("Changed Property")]
    [Description("Occurs when the DisabledForeColor property changes.")]
    public event EventHandler DisabledForeColorChanged;

    /// <summary>Occurs when the <see cref="BorderColor" /> property changes.</summary>
    [ExCategory("Changed Property")]
    [Description("Occurs when the BorderColor property changes.")]
    public event EventHandler BorderColorChanged;

    /// <summary>Occurs when the <see cref="MarkColor" /> property changes.</summary>
    [ExCategory("Changed Property")]
    [Description("Occurs when the MarkColor property changes.")]
    public event EventHandler MarkColorChanged;

    /// <summary>Occurs when the <see cref="BoxBackColor" /> property changes.</summary>
    [ExCategory("Changed Property")]
    [Description("Occurs when the BoxBackColor property changes.")]
    public event EventHandler BoxBackColorChanged;

    /// <summary>Occurs when the <see cref="HighlightColor" /> property changes.</summary>
    [ExCategory("Changed Property")]
    [Description("Occurs when the HighlightColor property changes.")]
    public event EventHandler HighlightColorChanged;

    /// <inheritdoc />
    protected override void OnCreateControl()
    {
        base.OnCreateControl();

        if (!DefaultStyle)
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
        }
    }

    /// <inheritdoc />
    protected override void OnPaint(PaintEventArgs e)
    {
        if (DefaultStyle)
        {
            base.OnPaint(e);
            return;
        }

        e.Graphics.Clear(BackColor);

        //Colors preparing
        var colorMark = _hover ? HighlightColor : Enabled ? MarkColor : DisabledForeColor;
        var colorBorder = _hover ? HighlightColor : Enabled ? BorderColor : DisabledForeColor;
        var colorText = Enabled ? ForeColor : DisabledForeColor;
        using var brushMark = new SolidBrush(colorMark);
        using var background = new SolidBrush(BoxBackColor);
        using var penMark = new Pen(brushMark, 2);
        using var border = new SolidBrush(colorBorder);
        using var penBorder = new Pen(border);

        //Positons and Size preparing
        var rects = ExButtonRenderer.GetBoxAndTextRectangle(e.Graphics, this, BOX_SIZE, BOX_OFFSET);
        var boxRec = rects.BoxRectangle;
        var textRec = rects.TextRectangle;
        var rectBorder = new Rectangle(boxRec.Location, new Size(boxRec.Width - 1, boxRec.Height - 1));

        //Text render
        TextRenderer.DrawText(e.Graphics, Text, Font, textRec.Location, colorText);

        //Box background render
        e.Graphics.FillRectangle(background, boxRec);

        //Box Border render
        e.Graphics.DrawRectangle(penBorder, rectBorder);

        //Mark render
        if (CheckState == CheckState.Checked)
        {
            e.Graphics.DrawLine(penMark, boxRec.X + 3, boxRec.Y + 7, boxRec.X + 6, boxRec.Y + 10);
            e.Graphics.DrawLine(penMark, boxRec.X + 6, boxRec.Y + 10, boxRec.X + 12, boxRec.Y + 4);
            e.Graphics.FillRectangle(brushMark, boxRec.X + 6, boxRec.Y + 11, 1, 1);
        }
        else if (CheckState == CheckState.Indeterminate)
        {
            e.Graphics.FillRectangle(brushMark, boxRec.X + 4, boxRec.Y + 4, boxRec.Width - 8, boxRec.Height - 8);
        }
    }

    /// <inheritdoc />
    protected override void OnMouseEnter(EventArgs eventargs)
    {
        base.OnMouseEnter(eventargs);

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

        if (_hover)
        {
            _hover = false;
            Invalidate();
        }
    }

    /// <summary>Raises the <see cref="DefaultStyleChanged" /> event.</summary>
    protected virtual void OnDefaultStyleChanged()
    {
        DefaultStyleChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>Raises the <see cref="DisabledForeColorChanged" /> event.</summary>
    protected virtual void OnDisabledForeColorChanged()
    {
        DisabledForeColorChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>Raises the <see cref="BorderColorChanged" /> event.</summary>
    protected virtual void OnBorderColorChanged()
    {
        BorderColorChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>Raises the <see cref="MarkColorChanged" /> event.</summary>
    protected virtual void OnMarkColorChanged()
    {
        MarkColorChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>Raises the <see cref="BoxBackColorChanged" /> event.</summary>
    protected virtual void OnBoxBackColorChanged()
    {
        BoxBackColorChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>Raises the <see cref="HighlightColorChanged" /> event.</summary>
    protected virtual void OnHighlightColorChanged()
    {
        HighlightColorChanged?.Invoke(this, EventArgs.Empty);
    }
}