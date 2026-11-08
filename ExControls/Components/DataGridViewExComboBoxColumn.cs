// ReSharper disable UnusedMember.Global

namespace ExControls;

/// <summary>
/// DataGridView column of <see cref="DataGridViewExComboBoxCell" /> cells, which can be drawn in the custom style like
/// <see cref="ExComboBox" />.
/// </summary>
public class DataGridViewExComboBoxColumn : DataGridViewComboBoxColumn, IThemeable
{
    /// <inheritdoc />
    public void ApplyTheme(ExTheme theme)
    {
        ExThemer.KeepDefaults(this, theme, nameof(DropDownBackColor), nameof(DropDownSelectedBackColor));
        foreach (var style in new[] { StyleNormal, StyleSelected, StyleHighlight })
            ExThemer.KeepDefaults(style, theme, nameof(ExComboBoxStyle.ForeColor), nameof(ExComboBoxStyle.BackColor),
                nameof(ExComboBoxStyle.BorderColor), nameof(ExComboBoxStyle.ArrowColor), nameof(ExComboBoxStyle.ButtonBackColor),
                nameof(ExComboBoxStyle.ButtonBorderColor), nameof(ExComboBoxStyle.ButtonRenderFirst));
        DefaultStyle = theme.UseSystemStyle;
        if (theme.UseSystemStyle)
            return;

        DropDownBackColor = theme.PanelBackColor;
        DropDownSelectedBackColor = theme.HighlightBackColor;
        StyleNormal.ForeColor = theme.ButtonForeColor;
        StyleNormal.BackColor = theme.ButtonBackColor;
        StyleNormal.BorderColor = theme.BorderColor;
        StyleNormal.ArrowColor = theme.BoxForeColor;
        StyleNormal.ButtonBackColor = theme.ButtonBackColor;
        StyleNormal.ButtonBorderColor = theme.ButtonBackColor;
        StyleSelected.BorderColor = theme.HighlightBackColor;
        StyleSelected.ButtonRenderFirst = false;
        StyleSelected.ButtonBackColor = theme.HighlightBackColor;
        StyleSelected.ButtonBorderColor = theme.HighlightBackColor;
        StyleHighlight.BorderColor = theme.HighlightBackColor;
        StyleHighlight.ButtonBorderColor = theme.HighlightBackColor;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataGridViewExComboBoxColumn" /> class with cells in the default style.
    /// </summary>
    public DataGridViewExComboBoxColumn()
    {
        CellTemplate = new DataGridViewExComboBoxCell();
        DefaultStyle = true;
        DropDownSelectedBackColor = SystemColors.Highlight;
        DropDownBackColor = Color.White;

        StyleNormal = new ExComboBoxStyle(StyleType.Normal);
        StyleHighlight = new ExComboBoxStyle(StyleType.Hover);
        StyleSelected = new ExComboBoxStyle(StyleType.Selected);
        StyleDisabled = new ExComboBoxStyle(StyleType.Disabled);

        StyleNormal.BackColor = Color.White;
        StyleNormal.ForeColor = Color.Black;
        StyleNormal.BorderColor = Color.DimGray;
        StyleNormal.ArrowColor = Color.Black;
        StyleNormal.ButtonBackColor = StyleNormal.BackColor;
        StyleNormal.ButtonBorderColor = StyleNormal.BackColor;
        StyleNormal.ButtonRenderFirst = true;

        SubscribeTemplateStyles();
        DataGridView?.InvalidateColumn(Index);
    }

    // Bunky maju vlastne kopie stylov (Clone), preto zmena vlastnosti stylu sablony
    // (napr. StyleNormal.ForeColor pri prepnuti temy) sa musi prenasat na vsetky existujuce bunky.
    private void SubscribeTemplateStyles()
    {
        StyleNormal.PropertyChanged += TemplateStyleOnPropertyChanged;
        StyleHighlight.PropertyChanged += TemplateStyleOnPropertyChanged;
        StyleSelected.PropertyChanged += TemplateStyleOnPropertyChanged;
        StyleDisabled.PropertyChanged += TemplateStyleOnPropertyChanged;
    }

    private void TemplateStyleOnPropertyChanged(object? sender, ExPropertyChangedEventArgs e)
    {
        if (sender is not ExComboBoxStyle style)
            return;

        if (ReferenceEquals(style, StyleNormal))
            ForEachCell(cell => cell.StyleNormal = (ExComboBoxStyle)style.Clone());
        else if (ReferenceEquals(style, StyleHighlight))
            ForEachCell(cell => cell.StyleHighlight = (ExComboBoxStyle)style.Clone());
        else if (ReferenceEquals(style, StyleSelected))
            ForEachCell(cell => cell.StyleSelected = (ExComboBoxStyle)style.Clone());
        else if (ReferenceEquals(style, StyleDisabled))
            ForEachCell(cell => cell.StyleDisabled = (ExComboBoxStyle)style.Clone());
    }

    private void ForEachCell(Action<DataGridViewExComboBoxCell> action)
    {
        if (DataGridView == null)
            return;
        var rows = DataGridView.Rows;
        for (var i = 0; i < rows.Count; i++)
            if (rows.SharedRow(i).Cells[Index] is DataGridViewExComboBoxCell cell)
                action(cell);
        DataGridView.InvalidateColumn(Index);
    }

    /// <inheritdoc />
    public sealed override DataGridViewCell? CellTemplate
    {
        get => base.CellTemplate;
        set
        {
            base.CellTemplate = value switch
            {
                null or DataGridViewComboBoxCell => value,
                _ => throw new InvalidCastException()
            };
        }
    }

    private DataGridViewExComboBoxCell ExComboBoxCellTemplate => (DataGridViewExComboBoxCell)CellTemplate!;

    /// <summary>
    /// Default style of the Control
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(true)]
    [Description("Default style of the Control.")]
    public bool DefaultStyle
    {
        get => ExComboBoxCellTemplate.DefaultStyle;
        set
        {
            if (DefaultStyle == value)
                return;
            ExComboBoxCellTemplate.DefaultStyle = value;
            ForEachCell(cell => cell.DefaultStyle = value);
        }
    }

    /// <summary>
    /// Color of the selected row in drop down menu
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "Highlight")]
    [Description("Color of the selected row in drop down menu.")]
    public Color DropDownSelectedBackColor
    {
        get => ExComboBoxCellTemplate.DropDownSelectedBackColor;
        set
        {
            if (DropDownSelectedBackColor == value)
                return;
            ExComboBoxCellTemplate.DropDownSelectedBackColor = value;
            ForEachCell(cell => cell.DropDownSelectedBackColor = value);
        }
    }

    /// <summary>
    /// Background color of the drop down menu.
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "White")]
    [Description("Background color of the drop down menu.")]
    public Color DropDownBackColor
    {
        get => ExComboBoxCellTemplate.DropDownBackColor;
        set
        {
            if (DropDownBackColor == value)
                return;
            ExComboBoxCellTemplate.DropDownBackColor = value;
            ForEachCell(cell => cell.DropDownBackColor = value);
        }
    }

    /// <summary>
    /// Normal style of the Control (when is inactive).
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [Description("Normal style of the Control (when is inactive).")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public ExComboBoxStyle StyleNormal
    {
        get => ExComboBoxCellTemplate.StyleNormal;
        set
        {
            ExComboBoxCellTemplate.StyleNormal.PropertyChanged -= TemplateStyleOnPropertyChanged;
            ExComboBoxCellTemplate.StyleNormal = value;
            value.PropertyChanged += TemplateStyleOnPropertyChanged;
            ForEachCell(cell => cell.StyleNormal = (ExComboBoxStyle)value.Clone());
        }
    }

    /// <summary>
    /// Highlight style of the Control (when mouse is over control).
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [Description("Highlight style of the Control (when mouse is over control).")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public ExComboBoxStyle StyleHighlight
    {
        get => ExComboBoxCellTemplate.StyleHighlight;
        set
        {
            ExComboBoxCellTemplate.StyleHighlight.PropertyChanged -= TemplateStyleOnPropertyChanged;
            ExComboBoxCellTemplate.StyleHighlight = value;
            value.PropertyChanged += TemplateStyleOnPropertyChanged;
            ForEachCell(cell => cell.StyleHighlight = (ExComboBoxStyle)value.Clone());
        }
    }

    /// <summary>
    /// Selected style of the Control (when control is selected).
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [Description("Selected style of the Control (when control is selected).")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public ExComboBoxStyle StyleSelected
    {
        get => ExComboBoxCellTemplate.StyleSelected;
        set
        {
            ExComboBoxCellTemplate.StyleSelected.PropertyChanged -= TemplateStyleOnPropertyChanged;
            ExComboBoxCellTemplate.StyleSelected = value;
            value.PropertyChanged += TemplateStyleOnPropertyChanged;
            ForEachCell(cell => cell.StyleSelected = (ExComboBoxStyle)value.Clone());
        }
    }

    /// <summary>
    /// Disabled style of the Control (when control is not Enabled).
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [Description("Disabled style of the Control (when control is not Enabled).")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public ExComboBoxStyle StyleDisabled
    {
        get => ExComboBoxCellTemplate.StyleDisabled;
        set
        {
            ExComboBoxCellTemplate.StyleDisabled.PropertyChanged -= TemplateStyleOnPropertyChanged;
            ExComboBoxCellTemplate.StyleDisabled = value;
            value.PropertyChanged += TemplateStyleOnPropertyChanged;
            ForEachCell(cell => cell.StyleDisabled = (ExComboBoxStyle)value.Clone());
        }
    }
}
