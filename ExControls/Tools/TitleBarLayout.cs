namespace ExControls;

/// <summary>
/// Rozlozenie vlastneho zahlavia (ciste, bez okna): ikona vlavo, tlacidla okna vpravo, medzi nimi prvky zahlavia a text.
/// Pravidla ako systemove zahlavie - bez <see cref="Form.ControlBox" /> nie je ikona ani tlacidla, minimalizacia
/// a maximalizacia su vzdy spolu (vypnute je len zakazane).
/// </summary>
/// <param name="Size">velkost zahlavia</param>
/// <param name="Icon">ikona okna (systemova ponuka)</param>
/// <param name="Minimize">tlacidlo minimalizacie</param>
/// <param name="Maximize">tlacidlo maximalizacie a obnovenia</param>
/// <param name="Close">tlacidlo zatvorenia</param>
/// <param name="MinimizeEnabled">minimalizacia je povolena (<see cref="Form.MinimizeBox" />)</param>
/// <param name="MaximizeEnabled">maximalizacia je povolena (<see cref="Form.MaximizeBox" />)</param>
internal readonly record struct TitleBarLayout(
    Size Size,
    Rectangle Icon,
    Rectangle Minimize,
    Rectangle Maximize,
    Rectangle Close,
    bool MinimizeEnabled,
    bool MaximizeEnabled)
{
    /// <summary>
    /// Rozlozi zahlavie danej velkosti.
    /// </summary>
    /// <param name="size">velkost zahlavia</param>
    /// <param name="iconWidth">sirka miesta pre ikonu</param>
    /// <param name="buttonWidth">sirka tlacidla okna</param>
    /// <param name="showIcon">okno ukazuje ikonu (<see cref="Form.ShowIcon" />)</param>
    /// <param name="controlBox">okno ma systemovu ponuku a tlacidla (<see cref="Form.ControlBox" />)</param>
    /// <param name="minimizeBox"><see cref="Form.MinimizeBox" /></param>
    /// <param name="maximizeBox"><see cref="Form.MaximizeBox" /></param>
    public static TitleBarLayout Compute(Size size, int iconWidth, int buttonWidth, bool showIcon, bool controlBox, bool minimizeBox, bool maximizeBox)
    {
        if (!controlBox)
            return new TitleBarLayout(size, Rectangle.Empty, Rectangle.Empty, Rectangle.Empty, Rectangle.Empty, false, false);

        var icon = showIcon ? new Rectangle(0, 0, iconWidth, size.Height) : Rectangle.Empty;
        var close = new Rectangle(size.Width - buttonWidth, 0, buttonWidth, size.Height);
        var minMax = minimizeBox || maximizeBox;
        var maximize = minMax ? new Rectangle(close.Left - buttonWidth, 0, buttonWidth, size.Height) : Rectangle.Empty;
        var minimize = minMax ? new Rectangle(close.Left - 2 * buttonWidth, 0, buttonWidth, size.Height) : Rectangle.Empty;
        return new TitleBarLayout(size, icon, minimize, maximize, close, minimizeBox, maximizeBox);
    }

    /// <summary>
    /// Miesto vlavo, ktore zabera ikona.
    /// </summary>
    public int LeftInset => Icon.Right;

    /// <summary>
    /// Miesto vpravo, ktore zaberaju tlacidla okna.
    /// </summary>
    public int RightInset => Close.IsEmpty ? 0 : Size.Width - (Minimize.IsEmpty ? Close.Left : Minimize.Left);

    /// <summary>
    /// Cast zahlavia nad ikonou alebo tlacidlom; zakazane tlacidlo je volne miesto zahlavia.
    /// </summary>
    /// <returns><see langword="null" />, ak bod nie je nad ikonou ani tlacidlom</returns>
    public TitleBarHit? HitTest(Point point)
    {
        if (Close.Contains(point))
            return TitleBarHit.CloseButton;
        if (Maximize.Contains(point))
            return MaximizeEnabled ? TitleBarHit.MaximizeButton : TitleBarHit.Caption;
        if (Minimize.Contains(point))
            return MinimizeEnabled ? TitleBarHit.MinimizeButton : TitleBarHit.Caption;
        return Icon.Contains(point) ? TitleBarHit.SystemMenu : null;
    }

    /// <summary>
    /// Plocha tlacidla okna (prazdna, ak tlacidlo nie je).
    /// </summary>
    public Rectangle BoundsOf(TitleBarHit button) => button switch
    {
        TitleBarHit.MinimizeButton => Minimize,
        TitleBarHit.MaximizeButton => Maximize,
        TitleBarHit.CloseButton => Close,
        _ => Rectangle.Empty
    };

    /// <summary>
    /// Umiestni text okna do najsirsieho volneho useku medzi prvkami zahlavia - na stred zahlavia, ak sa tam zmesti,
    /// inak co najblizsie k nemu; dlhy text sa skrati na sirku useku.
    /// </summary>
    /// <param name="textWidth">sirka textu</param>
    /// <param name="area">oblast pre text (medzi ikonou a tlacidlami)</param>
    /// <param name="occupied">plochy prvkov v zahlavi</param>
    /// <param name="center">vodorovny stred zahlavia</param>
    /// <param name="minWidth">najuzsi usek, do ktoreho sa text este kresli</param>
    /// <returns>plocha textu na celu vysku oblasti; prazdna, ak nie je dost miesta</returns>
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
