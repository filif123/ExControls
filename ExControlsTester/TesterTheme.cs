namespace ExControls.Test;

/// <summary>
/// Dark and light theme of the test forms and the menu look by it (like the theme handlers in the applications).
/// </summary>
internal static class TesterTheme
{
    public static readonly ExTheme Dark = new()
    {
        DarkScrollBars = true,
        PanelBackColor = Color.FromArgb(43, 45, 48),
        PanelForeColor = Color.FromArgb(223, 225, 229),
        BoxBackColor = Color.FromArgb(30, 31, 34),
        BoxForeColor = Color.FromArgb(223, 225, 229),
        ButtonBackColor = Color.FromArgb(57, 59, 64),
        ButtonForeColor = Color.FromArgb(223, 225, 229),
        BorderColor = Color.FromArgb(78, 81, 87),
        HighlightBackColor = Color.FromArgb(46, 67, 110),
        HighlightForeColor = Color.White,
        MarkColor = Color.FromArgb(223, 225, 229),
        LabelForeColor = Color.FromArgb(223, 225, 229)
    };

    public static readonly ExTheme Light = new()
    {
        PanelBackColor = Color.FromArgb(247, 248, 250),
        PanelForeColor = Color.Black,
        BoxBackColor = Color.White,
        BoxForeColor = Color.Black,
        ButtonBackColor = Color.FromArgb(235, 236, 240),
        ButtonForeColor = Color.Black,
        BorderColor = Color.FromArgb(200, 202, 208),
        HighlightBackColor = Color.FromArgb(211, 226, 250),
        HighlightForeColor = Color.Black,
        MarkColor = Color.Black,
        LabelForeColor = Color.Black
    };

    /// <summary>
    /// Registers the menu look by the theme (before the first form).
    /// </summary>
    public static void Register()
    {
        ExThemer.Register<MenuStrip>((menu, theme) =>
        {
            menu.BackColor = theme.PanelBackColor;
            menu.ForeColor = theme.PanelForeColor;
            menu.Renderer = new MenuRenderer(theme);
        });
    }

    private sealed class MenuRenderer(ExTheme theme) : ToolStripProfessionalRenderer(new MenuColors(theme))
    {
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            // polozky v paneli maju farbu panela (ExTitleBar ju v neaktivnom okne stlmi), rozbalena ponuka farbu temy
            if (e.Item.Enabled && (e.Item.Selected || e.Item.Pressed))
                e.TextColor = theme.HighlightForeColor;
            else if (e.Item.Enabled && e.Item.IsOnDropDown)
                e.TextColor = theme.PanelForeColor;
            base.OnRenderItemText(e);
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            e.ArrowColor = theme.PanelForeColor;
            base.OnRenderArrow(e);
        }
    }

    private sealed class MenuColors(ExTheme theme) : ProfessionalColorTable
    {
        public override Color MenuStripGradientBegin => theme.PanelBackColor;
        public override Color MenuStripGradientEnd => theme.PanelBackColor;
        public override Color MenuItemSelected => theme.HighlightBackColor;
        public override Color MenuItemSelectedGradientBegin => theme.HighlightBackColor;
        public override Color MenuItemSelectedGradientEnd => theme.HighlightBackColor;
        public override Color MenuItemPressedGradientBegin => theme.BoxBackColor;
        public override Color MenuItemPressedGradientMiddle => theme.BoxBackColor;
        public override Color MenuItemPressedGradientEnd => theme.BoxBackColor;
        public override Color MenuItemBorder => theme.BorderColor;
        public override Color MenuBorder => theme.BorderColor;
        public override Color ToolStripDropDownBackground => theme.BoxBackColor;
        public override Color ImageMarginGradientBegin => theme.BoxBackColor;
        public override Color ImageMarginGradientMiddle => theme.BoxBackColor;
        public override Color ImageMarginGradientEnd => theme.BoxBackColor;
        public override Color SeparatorDark => theme.BorderColor;
        public override Color SeparatorLight => theme.BoxBackColor;
        public override Color CheckBackground => theme.HighlightBackColor;
        public override Color CheckSelectedBackground => theme.HighlightBackColor;
        public override Color CheckPressedBackground => theme.HighlightBackColor;
    }
}
