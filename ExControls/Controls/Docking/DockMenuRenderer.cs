namespace ExControls.Docking;

/// <summary>
/// Vzhlad ponuky okna panela podla temy, ked program nezaregistroval vlastnu obsluhu ponuk (<see cref="ExThemer" />).
/// </summary>
/// <param name="colors">farby ramu spravcu</param>
internal sealed class DockMenuRenderer(DockColors colors) : ToolStripProfessionalRenderer(new MenuColors(colors))
{
    /// <summary>
    /// Farby ramu spravcu.
    /// </summary>
    public DockColors Colors { get; } = colors;

    /// <inheritdoc />
    protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
    {
        e.TextColor = !e.Item.Enabled ? Colors.Border : e.Item.Selected ? Colors.ActiveFore : Colors.BoxFore;
        base.OnRenderItemText(e);
    }

    /// <inheritdoc />
    protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
    {
        e.ArrowColor = Colors.BoxFore;
        base.OnRenderArrow(e);
    }

    /// <inheritdoc />
    protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
    {
        // systemova fajka je cierna - na tmavom pozadi by nebola vidiet
        var box = e.ImageRectangle;
        using (var brush = new SolidBrush(e.Item.Selected ? Colors.ActiveBack : Colors.ButtonBack))
            e.Graphics.FillRectangle(brush, box);
        ExButtonRenderer.DrawCheckMark(e.Graphics, box, e.Item.Selected ? Colors.ActiveFore : Colors.BoxFore);
    }

    private sealed class MenuColors(DockColors colors) : ProfessionalColorTable
    {
        public override Color ToolStripDropDownBackground => colors.Box;
        public override Color ImageMarginGradientBegin => colors.Box;
        public override Color ImageMarginGradientMiddle => colors.Box;
        public override Color ImageMarginGradientEnd => colors.Box;
        public override Color MenuBorder => colors.Border;
        public override Color MenuItemBorder => colors.ActiveBack;
        public override Color MenuItemSelected => colors.ActiveBack;
        public override Color MenuItemSelectedGradientBegin => colors.ActiveBack;
        public override Color MenuItemSelectedGradientEnd => colors.ActiveBack;
        public override Color MenuItemPressedGradientBegin => colors.ActiveBack;
        public override Color MenuItemPressedGradientMiddle => colors.ActiveBack;
        public override Color MenuItemPressedGradientEnd => colors.ActiveBack;
        public override Color SeparatorDark => colors.Border;
        public override Color SeparatorLight => colors.Box;
        public override Color CheckBackground => colors.ButtonBack;
        public override Color CheckSelectedBackground => colors.ActiveBack;
        public override Color CheckPressedBackground => colors.ActiveBack;
    }
}
