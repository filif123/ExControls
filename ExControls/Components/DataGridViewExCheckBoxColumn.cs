namespace ExControls;

/// <summary>
/// DataGridView column of <see cref="DataGridViewExCheckBoxCell" /> cells, which can be drawn in the custom style like
/// <see cref="ExCheckBox" />.
/// </summary>
public class DataGridViewExCheckBoxColumn : DataGridViewCheckBoxColumn, IThemeable
{
    /// <inheritdoc />
    public void ApplyTheme(ExTheme theme)
    {
        ExThemer.KeepDefaults(this, theme, nameof(BorderColor), nameof(MarkColor), nameof(SquareBackColor), nameof(HighlightColor));
        DefaultStyle = theme.UseSystemStyle;
        if (theme.UseSystemStyle)
            return;

        BorderColor = theme.BorderColor;
        MarkColor = theme.MarkColor;
        SquareBackColor = theme.PanelBackColor;
        HighlightColor = theme.HighlightBackColor;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataGridViewExCheckBoxColumn" /> class with cells in the default style.
    /// </summary>
    public DataGridViewExCheckBoxColumn()
    {
        CellTemplate = new DataGridViewExCheckBoxCell();
        DefaultStyle = true;
        BorderColor = Color.Black;
        MarkColor = Color.Black;
        SquareBackColor = Color.White;
        HighlightColor = Color.FromArgb(0, 120, 215);
    }

    /// <inheritdoc />
    public sealed override DataGridViewCell? CellTemplate
    {
        get => base.CellTemplate;
        set
        {
            base.CellTemplate = value switch
            {
                null or DataGridViewCheckBoxCell => value,
                _ => throw new InvalidCastException(),
            };
        }
    }

    private DataGridViewExCheckBoxCell DynCheckBoxCellTemplate => (DataGridViewExCheckBoxCell)CellTemplate!;

    /// <summary>
    /// Default style of the Control
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(true)]
    [Description("Default style of the Control.")]
    public bool DefaultStyle
    {
        get => DynCheckBoxCellTemplate.DefaultStyle;
        set
        {
            if (DefaultStyle == value)
                return;
            DynCheckBoxCellTemplate.DefaultStyle = value;
            if (DataGridView == null)
                return;
            var rows = DataGridView.Rows;
            for (var i = 1; i < DataGridView.Rows.Count; i++)
                if (rows.SharedRow(i).Cells[Index] is DataGridViewExCheckBoxCell cell)
                    cell.DefaultStyle = value;
            DataGridView.InvalidateColumn(Index);
        }
    }

    /// <summary>
    /// Color of the CheckBox's border
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "Black")]
    [Description("Color of the CheckBox's border.")]
    public Color BorderColor
    {
        get => DynCheckBoxCellTemplate.BorderColor;
        set
        {
            if (BorderColor == value)
                return;
            DynCheckBoxCellTemplate.BorderColor = value;
            if (DataGridView == null)
                return;
            var rows = DataGridView.Rows;
            for (var i = 1; i < DataGridView.Rows.Count; i++)
                if (rows.SharedRow(i).Cells[Index] is DataGridViewExCheckBoxCell cell)
                    cell.BorderColor = value;
            DataGridView.InvalidateColumn(Index);
        }
    }

    /// <summary>
    /// Color of the CheckBox's mark
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "Black")]
    [Description("Color of the CheckBox's mark.")]
    public Color MarkColor
    {
        get => DynCheckBoxCellTemplate.MarkColor;
        set
        {
            if (MarkColor == value)
                return;
            DynCheckBoxCellTemplate.MarkColor = value;
            if (DataGridView == null)
                return;
            var rows = DataGridView.Rows;
            for (var i = 1; i < DataGridView.Rows.Count; i++)
                if (rows.SharedRow(i).Cells[Index] is DataGridViewExCheckBoxCell cell)
                    cell.MarkColor = value;
            DataGridView.InvalidateColumn(Index);
        }
    }

    /// <summary>
    /// Background color of CheckBox's square
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "Black")]
    [Description("Background color of CheckBox's square.")]
    public Color SquareBackColor
    {
        get => DynCheckBoxCellTemplate.SquareBackColor;
        set
        {
            if (SquareBackColor == value)
                return;
            DynCheckBoxCellTemplate.SquareBackColor = value;
            if (DataGridView == null)
                return;
            var rows = DataGridView.Rows;
            for (var i = 1; i < DataGridView.Rows.Count; i++)
                if (rows.SharedRow(i).Cells[Index] is DataGridViewExCheckBoxCell cell)
                    cell.SquareBackColor = value;
            DataGridView.InvalidateColumn(Index);
        }
    }

    /// <summary>
    /// Color of the border and mark of CheckBox when mouse is over the Control
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "0, 120, 215")]
    [Description("Color of the border and mark of CheckBox when mouse is over the Control.")]
    public Color HighlightColor
    {
        get => DynCheckBoxCellTemplate.HighlightColor;
        set
        {
            if (HighlightColor == value)
                return;
            DynCheckBoxCellTemplate.HighlightColor = value;
            if (DataGridView == null)
                return;
            var rows = DataGridView.Rows;
            for (var i = 1; i < DataGridView.Rows.Count; i++)
                if (rows.SharedRow(i).Cells[Index] is DataGridViewExCheckBoxCell cell)
                    cell.HighlightColor = value;
            DataGridView.InvalidateColumn(Index);
        }
    }
}
