namespace ExControls;

/// <summary>
/// Layout of the custom title bar (pure, without a window): the icon on the left, the window buttons on the right, the title
/// bar controls and the text between them. The rules follow the system caption - without <see cref="Form.ControlBox" /> there
/// is neither icon nor buttons, minimize and maximize always come together (a turned off one is only disabled), help
/// (<see cref="Form.HelpButton" />) only without them.
/// </summary>
/// <param name="Size">size of the title bar</param>
/// <param name="Icon">window icon (system menu)</param>
/// <param name="Minimize">minimize button</param>
/// <param name="Maximize">maximize and restore button</param>
/// <param name="Close">close button</param>
/// <param name="Help">help button (context help)</param>
/// <param name="MinimizeEnabled">minimize is enabled (<see cref="Form.MinimizeBox" />)</param>
/// <param name="MaximizeEnabled">maximize is enabled (<see cref="Form.MaximizeBox" />)</param>
internal readonly record struct TitleBarLayout(
    Size Size,
    Rectangle Icon,
    Rectangle Minimize,
    Rectangle Maximize,
    Rectangle Close,
    Rectangle Help,
    bool MinimizeEnabled,
    bool MaximizeEnabled)
{
    /// <summary>
    /// Lays out a title bar of the given size.
    /// </summary>
    /// <param name="size">size of the title bar</param>
    /// <param name="iconWidth">width of the space for the icon</param>
    /// <param name="buttonWidth">width of a window button</param>
    /// <param name="showIcon">the form shows the icon (<see cref="Form.ShowIcon" />)</param>
    /// <param name="controlBox">the form has the system menu and buttons (<see cref="Form.ControlBox" />)</param>
    /// <param name="minimizeBox"><see cref="Form.MinimizeBox" /></param>
    /// <param name="maximizeBox"><see cref="Form.MaximizeBox" /></param>
    /// <param name="helpButton"><see cref="Form.HelpButton" /></param>
    public static TitleBarLayout Compute(Size size, int iconWidth, int buttonWidth, bool showIcon, bool controlBox, bool minimizeBox, bool maximizeBox,
        bool helpButton)
    {
        if (!controlBox)
            return new TitleBarLayout(size, Rectangle.Empty, Rectangle.Empty, Rectangle.Empty, Rectangle.Empty, Rectangle.Empty, false, false);

        var icon = showIcon ? new Rectangle(0, 0, iconWidth, size.Height) : Rectangle.Empty;
        var close = new Rectangle(size.Width - buttonWidth, 0, buttonWidth, size.Height);
        var minMax = minimizeBox || maximizeBox;
        var maximize = minMax ? new Rectangle(close.Left - buttonWidth, 0, buttonWidth, size.Height) : Rectangle.Empty;
        var minimize = minMax ? new Rectangle(close.Left - 2 * buttonWidth, 0, buttonWidth, size.Height) : Rectangle.Empty;
        var help = helpButton && !minMax ? new Rectangle(close.Left - buttonWidth, 0, buttonWidth, size.Height) : Rectangle.Empty;
        return new TitleBarLayout(size, icon, minimize, maximize, close, help, minimizeBox, maximizeBox);
    }

    /// <summary>
    /// Space on the left taken by the icon.
    /// </summary>
    public int LeftInset => Icon.Right;

    /// <summary>
    /// Space on the right taken by the window buttons.
    /// </summary>
    public int RightInset => Close.IsEmpty ? 0 : Size.Width - (!Minimize.IsEmpty ? Minimize.Left : !Help.IsEmpty ? Help.Left : Close.Left);

    /// <summary>
    /// Part of the title bar over the icon or a button; a disabled button is free space of the title bar.
    /// </summary>
    /// <param name="point">point in client coordinates of the title bar</param>
    /// <returns><see langword="null" /> if the point is over neither the icon nor a button</returns>
    public TitleBarHit? HitTest(Point point)
    {
        if (Close.Contains(point))
            return TitleBarHit.CloseButton;
        if (Help.Contains(point))
            return TitleBarHit.HelpButton;
        if (Maximize.Contains(point))
            return MaximizeEnabled ? TitleBarHit.MaximizeButton : TitleBarHit.Caption;
        if (Minimize.Contains(point))
            return MinimizeEnabled ? TitleBarHit.MinimizeButton : TitleBarHit.Caption;
        return Icon.Contains(point) ? TitleBarHit.SystemMenu : null;
    }

    /// <summary>
    /// Bounds of a window button (empty if there is no such button).
    /// </summary>
    /// <param name="button">window button</param>
    public Rectangle BoundsOf(TitleBarHit button) => button switch
    {
        TitleBarHit.MinimizeButton => Minimize,
        TitleBarHit.MaximizeButton => Maximize,
        TitleBarHit.CloseButton => Close,
        TitleBarHit.HelpButton => Help,
        _ => Rectangle.Empty
    };

    /// <summary>
    /// Places the form text into the widest free gap between the title bar controls - in the center of the title bar if it
    /// fits there, otherwise as close to it as possible; a long text is trimmed to the width of the gap.
    /// </summary>
    /// <param name="textWidth">width of the text</param>
    /// <param name="area">area for the text (between the icon and the buttons)</param>
    /// <param name="occupied">bounds of the controls in the title bar</param>
    /// <param name="center">horizontal center of the title bar</param>
    /// <param name="minWidth">narrowest gap the text is still drawn into</param>
    /// <returns>bounds of the text over the full height of the area; empty if there is not enough space</returns>
    public static Rectangle PlaceTitle(int textWidth, Rectangle area, IEnumerable<Rectangle> occupied, int center, int minWidth)
    {
        var best = Rectangle.Empty;
        var gapLeft = area.Left;
        var edges = occupied.Where(r => r.Right > area.Left && r.Left < area.Right).OrderBy(r => r.Left)
            .Append(new Rectangle(area.Right, area.Top, 0, area.Height));
        foreach (var r in edges)
        {
            var gapRight = Math.Min(r.Left, area.Right);
            if (gapRight - gapLeft > best.Width)
                best = Rectangle.FromLTRB(gapLeft, area.Top, gapRight, area.Bottom);
            gapLeft = Math.Max(gapLeft, r.Right);
        }

        if (best.Width < minWidth || textWidth <= 0)
            return Rectangle.Empty;

        var width = Math.Min(textWidth, best.Width);
        var left = Math.Max(best.Left, Math.Min(center - width / 2, best.Right - width));
        return new Rectangle(left, area.Top, width, area.Height);
    }
}
