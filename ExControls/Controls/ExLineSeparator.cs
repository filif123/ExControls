using System.Drawing.Drawing2D;

namespace ExControls;

/// <summary>
///     LineSeparator Control
/// </summary>
[ToolboxBitmap(typeof(ExLineSeparator), "ExLineSeparator.bmp")]
public class ExLineSeparator : Control
{
    private Color _lineColor;
    private LineOrientation _lineOrientation;
    private DashStyle _lineStyle;
    private int _lineThickness;

    /// <summary>
    ///     Constructor
    /// </summary>
    public ExLineSeparator()
    {
        Height = 4;
        Width = 350;

        _lineThickness = 1;
        _lineOrientation = LineOrientation.Horizontal;
        _lineColor = Color.LightGray;
        _lineStyle = DashStyle.Solid;

        Invalidate();
    }

    /// <summary>
    ///     Thickness of the LineSeparator's line
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(1)]
    [Description("Thickness of the LineSeparator's line.")]
    public int LineThickness
    {
        get => _lineThickness;
        set
        {
            if (value == _lineThickness)
                return;
            _lineThickness = value;
            Invalidate();
            OnLineThicknessChanged();
        }
    }

    /// <summary>
    ///     Orientation of the LineSeparator's line
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(LineOrientation), "Horizontal")]
    [Description("Orientation of the LineSeparator's line.")]
    public LineOrientation LineOrientation
    {
        get => _lineOrientation;
        set
        {
            if (value == _lineOrientation)
                return;
            _lineOrientation = value;
            Invalidate();
            OnLineOrientationChanged();
        }
    }

    /// <summary>
    ///     Orientation of the LineSeparator's line
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(DashStyle), "Solid")]
    [Description("Style of the LineSeparator's line.")]
    public DashStyle LineStyle
    {
        get => _lineStyle;
        set
        {
            if (value == _lineStyle)
                return;
            _lineStyle = value;
            Invalidate();
            OnLineStyleChanged();
        }
    }

    /// <summary>
    ///     Color of the LineSeparator's line
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "LightGray")]
    [Description("Color of the LineSeparator's line.")]
    public Color LineColor
    {
        get => _lineColor;
        set
        {
            if (value == _lineColor)
                return;
            _lineColor = value;
            Invalidate();
            OnLineColorChanged();
        }
    }

    /// <inheritdoc />
    [Browsable(false)]
    public override string Text { get; set; }

    /// <summary>Occurs when the <see cref="LineColor" /> property changes.</summary>
    [ExCategory("Changed Property")]
    [Description("Occurs when the LineColor property changes.")]
    public event EventHandler LineColorChanged;

    /// <summary>Occurs when the <see cref="LineThickness" /> property changes.</summary>
    [ExCategory("Changed Property")]
    [Description("Occurs when the LineThickness property changes.")]
    public event EventHandler LineThicknessChanged;

    /// <summary>Occurs when the <see cref="LineOrientation" /> property changes.</summary>
    [ExCategory("Changed Property")]
    [Description("Occurs when the LineOrientation property changes.")]
    public event EventHandler LineOrientationChanged;

    /// <summary>Occurs when the <see cref="LineStyle" /> property changes.</summary>
    [ExCategory("Changed Property")]
    [Description("Occurs when the LineStyle property changes.")]
    public event EventHandler LineStyleChanged;

    /// <summary>Occurs when the Line is drawing.</summary>
    [ExCategory(CategoryType.Appearance)]
    [Description("Occurs when the Line is drawing.")]
    public event EventHandler<LinePenEventArgs> LineDrawing;


    /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.</summary>
    /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data. </param>
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        using var g = e.Graphics;
        using var pen = new Pen(LineColor, LineThickness) { DashStyle = LineStyle };

        OnLineDrawing(new LinePenEventArgs(pen));

        switch (LineOrientation)
        {
            case LineOrientation.Horizontal:
                g.DrawLine(pen, new Point(0, 0), new Point(Width, 0));
                break;
            case LineOrientation.Vertical:
                g.DrawLine(pen, new Point(0, 0), new Point(0, Height));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>Raises the <see cref="LineColorChanged" /> event.</summary>
    protected virtual void OnLineColorChanged()
    {
        LineColorChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>Raises the <see cref="LineThicknessChanged" /> event.</summary>
    protected virtual void OnLineThicknessChanged()
    {
        LineThicknessChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>Raises the <see cref="LineOrientationChanged" /> event.</summary>
    protected virtual void OnLineOrientationChanged()
    {
        LineOrientationChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>Raises the <see cref="LineStyleChanged" /> event.</summary>
    protected virtual void OnLineStyleChanged()
    {
        LineStyleChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>Raises the <see cref="LineDrawing" /> event.</summary>
    protected virtual void OnLineDrawing(LinePenEventArgs e)
    {
        LineDrawing?.Invoke(this, e);
    }
}

/// <summary>
///     Orientation of the LineSeparator
/// </summary>
public enum LineOrientation
{
    /// <summary>
    ///     Horizontal orientation
    /// </summary>
    Horizontal,

    /// <summary>
    ///     Vertical orientation
    /// </summary>
    Vertical
}