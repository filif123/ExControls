// ReSharper disable UnusedMember.Global

namespace ExControls;

/// <summary>
/// DataGridView cell with a combo box that can be drawn in the custom style with the state colors of <see cref="ExComboBoxStyle" />
/// like <see cref="ExComboBox" />.
/// </summary>
public class DataGridViewExComboBoxCell : DataGridViewComboBoxCell
{
    private bool _drawing;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataGridViewExComboBoxCell" /> class in the default style.
    /// </summary>
    public DataGridViewExComboBoxCell()
    {
        DefaultStyle = true;
        DropDownSelectedBackColor = SystemColors.Highlight;
        DropDownBackColor = Color.White;

        StyleNormal = new ExComboBoxStyle(StyleType.Normal);
        StyleHighlight = new ExComboBoxStyle(StyleType.Hover);
        StyleSelected = new ExComboBoxStyle(StyleType.Selected);
        StyleDisabled = new ExComboBoxStyle(StyleType.Disabled);

        StyleNormal.PropertyChanged += StyleOnPropertyChanged;
        StyleDisabled.PropertyChanged += StyleOnPropertyChanged;
        StyleHighlight.PropertyChanged += StyleOnPropertyChanged;
        StyleSelected.PropertyChanged += StyleOnPropertyChanged;
    }

    /// <inheritdoc />
    public override Type EditType => typeof(DataGridViewExComboBoxEditingControl);

    /// <summary>
    /// Default style of the Control
    /// </summary>
    public bool DefaultStyle
    {
        get;
        set
        {
            field = value;
            DataGridView?.InvalidateCell(ColumnIndex, RowIndex);
        }
    }

    /// <summary>
    /// Color of the selected row in drop down menu
    /// </summary>
    public Color DropDownSelectedBackColor
    {
        get;
        set
        {
            field = value;
            DataGridView?.InvalidateCell(ColumnIndex, RowIndex);
        }
    }

    /// <summary>
    /// Color of the selected row in drop down menu
    /// </summary>
    public Color DropDownBackColor
    {
        get;
        set
        {
            field = value;
            DataGridView?.InvalidateCell(ColumnIndex, RowIndex);
        }
    }

    /// <summary>
    /// Normal style of the Control (when is inactive).
    /// </summary>
    public ExComboBoxStyle StyleNormal { get; set; }

    /// <summary>
    /// Highlight style of the Control (when mouse is over control).
    /// </summary>
    public ExComboBoxStyle StyleHighlight { get; set; }

    /// <summary>
    /// Selected style of the Control (when control is selected).
    /// </summary>
    public ExComboBoxStyle StyleSelected { get; set; }

    /// <summary>
    /// Disabled style of the Control (when control is not Enabled).
    /// </summary>
    public ExComboBoxStyle StyleDisabled { get; set; }

    private void StyleOnPropertyChanged(object? sender, ExPropertyChangedEventArgs e)
    {
        if (!_drawing) DataGridView?.InvalidateCell(ColumnIndex, RowIndex);
    }

    /// <inheritdoc />
    public override object Clone()
    {
        var cell = (DataGridViewExComboBoxCell)base.Clone();
        cell.DefaultStyle = DefaultStyle;
        cell.DropDownSelectedBackColor = DropDownSelectedBackColor;
        cell.DropDownBackColor = DropDownBackColor;
        cell.StyleNormal = (ExComboBoxStyle)StyleNormal.Clone();
        cell.StyleHighlight = (ExComboBoxStyle)StyleHighlight.Clone();
        cell.StyleSelected = (ExComboBoxStyle)StyleSelected.Clone();
        cell.StyleDisabled = (ExComboBoxStyle)StyleDisabled.Clone();
        return cell;
    }

    /// <summary>Attaches and initializes the hosted editing control.</summary>
    /// <param name="rowIndex">The index of the cell's parent row.</param>
    /// <param name="initialFormattedValue">The initial value to be displayed in the control.</param>
    /// <param name="dataGridViewCellStyle">
    /// A <see cref="System.Windows.Forms.DataGridViewCellStyle" /> that determines the
    /// appearance of the hosted control.
    /// </param>
    public override void InitializeEditingControl(int rowIndex, object? initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
    {
        base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
        if (DataGridView?.EditingControl is not DataGridViewExComboBoxEditingControl ctl)
            return;

        ctl.DefaultStyle = DefaultStyle;
        ctl.DropDownSelectedRowBackColor = DropDownSelectedBackColor;
        ctl.DropDownBackColor = DropDownBackColor;
        ctl.StyleNormal = StyleNormal;
        ctl.StyleHighlight = StyleHighlight;
        ctl.StyleSelected = StyleSelected;
        ctl.StyleDisabled = StyleDisabled;

        ctl.Invalidate();
    }

    /// <inheritdoc />
    protected override void Paint(Graphics graphics,
        Rectangle clipBounds, Rectangle cellBounds,
        int rowIndex,
        DataGridViewElementStates elementState,
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

        _drawing = true;

        var formattedText = formattedValue?.ToString() ?? string.Empty;
        var texts = TextRenderer.MeasureText(graphics, formattedText, cellStyle.Font);
        var textStart = new Point(cellBounds.X + 2, cellBounds.Y + (int)Math.Round(cellBounds.Height / 2d - texts.Height / 2d));
        var dropButton = cellBounds with { X = cellBounds.X + cellBounds.Width - 23, Width = 20 };

        var back = StyleNormal.BackColor ??= Color.White;
        var fore = StyleNormal.ForeColor ??= Color.Black;
        var border = StyleNormal.BorderColor ??= Color.DimGray;
        var arrow = StyleNormal.ArrowColor ??= Color.Black;
        var backbut = StyleNormal.ButtonBackColor ??= back;
        var borbut = StyleNormal.ButtonBorderColor ??= back;
        var bbfirst = StyleNormal.ButtonRenderFirst ??= true;

        if (!DataGridView!.Enabled)
        {
            StyleDisabled.BackColor = back = StyleDisabled.BackColor ?? DataGridView.BackColor;
            StyleDisabled.ForeColor = fore = StyleDisabled.ForeColor ?? SystemColors.GrayText;
            if (StyleDisabled.BorderColor.HasValue) border = StyleDisabled.BorderColor.Value;
            StyleDisabled.ArrowColor = arrow = StyleDisabled.ArrowColor ?? SystemColors.GrayText;
            StyleDisabled.ButtonBackColor = backbut = StyleDisabled.ButtonBackColor ?? back;
            StyleDisabled.ButtonBorderColor = borbut = StyleDisabled.ButtonBorderColor ?? back;
            if (StyleDisabled.ButtonRenderFirst.HasValue) bbfirst = StyleDisabled.ButtonRenderFirst.Value;
        }

        using (var brushBack = new SolidBrush(back))
            graphics.FillRectangle(brushBack, cellBounds);

        using var penBorder = new Pen(border);

        if (bbfirst)
        {
            ExButtonRenderer.DrawDropDownButton(graphics, dropButton, backbut, borbut, arrow);
            graphics.DrawRectangle(penBorder, cellBounds.X, cellBounds.Y, cellBounds.Width - 1, cellBounds.Height - 1);
        }
        else
        {
            graphics.DrawRectangle(penBorder, cellBounds.X, cellBounds.Y, cellBounds.Width - 1, cellBounds.Height - 1);
            ExButtonRenderer.DrawDropDownButton(graphics, dropButton, backbut, borbut, arrow);
        }

        TextRenderer.DrawText(graphics, formattedText, cellStyle.Font, textStart, fore, back,
            DataGridView.RightToLeft == RightToLeft.Yes ? TextFormatFlags.Right : TextFormatFlags.Default);

        _drawing = false;
    }
}
