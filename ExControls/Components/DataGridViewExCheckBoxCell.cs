namespace ExControls;

/// <summary>
/// DataGridView cell with a check box that can be drawn in the custom style with its own colors like <see cref="ExCheckBox" />.
/// </summary>
public class DataGridViewExCheckBoxCell : DataGridViewCheckBoxCell
{
    private bool _hover;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataGridViewExCheckBoxCell" /> class in the default style.
    /// </summary>
    public DataGridViewExCheckBoxCell()
    {
        DefaultStyle = true;
        BorderColor = Color.Black;
        MarkColor = Color.Black;
        SquareBackColor = Color.White;
        HighlightColor = Color.FromArgb(0, 120, 215);
    }

    /// <summary>
    /// Default style of the Control
    /// </summary>
    public bool DefaultStyle
    {
        get;
        set
        {
            field = value;
            DataGridView?.InvalidateColumn(ColumnIndex);
        }
    }

    /// <summary>
    /// Color of the CheckBox's border
    /// </summary>
    public Color BorderColor
    {
        get;
        set
        {
            field = value;
            DataGridView?.InvalidateColumn(ColumnIndex);
        }
    }

    /// <summary>
    /// Color of the CheckBox's mark
    /// </summary>
    public Color MarkColor
    {
        get;
        set
        {
            field = value;
            DataGridView?.InvalidateColumn(ColumnIndex);
        }
    }

    /// <summary>
    /// Background color of CheckBox's square
    /// </summary>
    public Color SquareBackColor
    {
        get;
        set
        {
            field = value;
            DataGridView?.InvalidateColumn(ColumnIndex);
        }
    }

    /// <summary>
    /// Color of the border and mark of CheckBox when mouse is over the Control
    /// </summary>
    public Color HighlightColor
    {
        get;
        set
        {
            field = value;
            DataGridView?.InvalidateColumn(ColumnIndex);
        }
    }

    /// <inheritdoc />
    public override object Clone()
    {
        var cell = (DataGridViewExCheckBoxCell)base.Clone();
        cell.DefaultStyle = DefaultStyle;
        cell.BorderColor = BorderColor;
        cell.MarkColor = MarkColor;
        cell.SquareBackColor = SquareBackColor;
        cell.HighlightColor = HighlightColor;
        return cell;
    }

    /// <inheritdoc />
    protected override void Paint(Graphics graphics,
        Rectangle clipBounds, Rectangle cellBounds,
        int rowIndex, DataGridViewElementStates elementState,
        object? value, object? formattedValue, string? errorText,
        DataGridViewCellStyle cellStyle,
        DataGridViewAdvancedBorderStyle advancedBorderStyle,
        DataGridViewPaintParts paintParts)
    {
        if (DefaultStyle)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
            return;
        }

        // Draw the cell background, if specified.
        if ((paintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
        {
            var selected = (elementState & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected;
            using var cellBackground = selected ? new SolidBrush(cellStyle.SelectionBackColor) : new SolidBrush(cellStyle.BackColor);
            graphics.FillRectangle(cellBackground, cellBounds);
        }

        // Draw the cell borders, if specified.
        if ((paintParts & DataGridViewPaintParts.Border) == DataGridViewPaintParts.Border)
            PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);

        var pt = GetCheckBoxPoint(cellBounds);
        var rect = new Rectangle(pt, new Size(16, 16));
        using var background = new SolidBrush(SquareBackColor);
        graphics.FillRectangle(background, rect);

        Color colorMark;
        if (_hover)
            colorMark = HighlightColor;
        else
            colorMark = elementState != DataGridViewElementStates.ReadOnly ? MarkColor : Color.DimGray;

        Color colorBorder;
        if (_hover)
            colorBorder = HighlightColor;
        else
            colorBorder = elementState != DataGridViewElementStates.ReadOnly ? BorderColor : Color.DimGray;

        var bs = ButtonState.Normal;

        if (formattedValue is bool val) 
            bs = !val ? ButtonState.Normal : ButtonState.Checked;

        //mark
        if (bs == ButtonState.Checked)
        {
            using var brushMark = new SolidBrush(colorMark);
            using var penMark = new Pen(brushMark, 2);
            graphics.DrawLine(penMark, pt.X + 3, pt.Y + 8, pt.X + 6, pt.Y + 11);
            graphics.DrawLine(penMark, pt.X + 6, pt.Y + 11, pt.X + 12, pt.Y + 5);
            graphics.FillRectangle(brushMark, pt.X + 6, pt.Y + 12, 1, 1);
        }

        //border
        using var border = new SolidBrush(colorBorder);
        using var penborder = new Pen(border);
        var rectb = new Rectangle(pt, new Size(15, 15));
        graphics.DrawRectangle(penborder, rectb);
    }

    private static Point GetCheckBoxPoint(Rectangle cellBounds)
    {
        var x = cellBounds.Width / 2 - 8;
        var y = cellBounds.Height / 2 - 8;

        if (x < 0) x = 0;
        if (y < 0) y = 0;

        return new Point(cellBounds.X + x, cellBounds.Y + y);
    }

    /// <inheritdoc />
    protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
    {
        if (DefaultStyle)
        {
            base.OnMouseMove(e);
            return;
        }

        if (DataGridView == null)
            return;
        var mouseInContentBounds = _hover;
        _hover = GetContentBounds(e.RowIndex).Contains(e.X, e.Y);
        if (mouseInContentBounds != _hover) DataGridView.InvalidateCell(ColumnIndex, e.RowIndex);
    }

    /// <inheritdoc />
    protected override void OnMouseLeave(int rowIndex)
    {
        if (DefaultStyle)
        {
            base.OnMouseLeave(rowIndex);
            return;
        }

        if (DataGridView == null)
            return;

        if (_hover)
        {
            _hover = false;
            DataGridView.InvalidateCell(ColumnIndex, rowIndex);
        }
    }
}
