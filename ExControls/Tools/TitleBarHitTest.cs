namespace ExControls;

/// <summary>
/// Rozdelenie vrchu okna s vlastnym zahlavim na casti pre WM_NCHITTEST (ciste, bez okna).
/// </summary>
internal static class TitleBarHitTest
{
    /// <summary>
    /// Urci cast okna v bode klientskej oblasti, ktory systemova obsluha vyhodnotila ako klientsky.
    /// </summary>
    /// <param name="point">bod v klientskych suradniciach okna</param>
    /// <param name="clientWidth">sirka klientskej oblasti</param>
    /// <param name="resizeBorder">
    /// hrubka horneho okraja na zmenu velkosti; 0 = okno sa zhora nemeni (maximalizovane, pevny ram)
    /// </param>
    /// <param name="titleBar">poloha zahlavia v klientskej oblasti</param>
    /// <param name="titleBarHitTest">cast zahlavia v bode zahlavia (<see cref="ExTitleBar.HitTest" />)</param>
    public static TitleBarHit Classify(Point point, int clientWidth, int resizeBorder, Rectangle titleBar, Func<Point, TitleBarHit> titleBarHitTest)
    {
        // horny okraj ma prednost pred zahlavim - inak by sa okno zhora nedalo zmensit
        if (point.Y >= 0 && point.Y < resizeBorder)
        {
            var corner = resizeBorder * 2;
            if (point.X < corner)
                return TitleBarHit.ResizeTopLeft;
            return point.X >= clientWidth - corner ? TitleBarHit.ResizeTopRight : TitleBarHit.ResizeTop;
        }

        return titleBar.Contains(point) ? titleBarHitTest(new Point(point.X - titleBar.X, point.Y - titleBar.Y)) : TitleBarHit.Client;
    }

    /// <summary>
    /// Navratova hodnota WM_NCHITTEST pre cast okna.
    /// </summary>
    public static int ToHitCode(TitleBarHit hit) => (int)(hit switch
    {
        TitleBarHit.Caption => Win32.HitTest.HTCAPTION,
        TitleBarHit.SystemMenu => Win32.HitTest.HTSYSMENU,
        TitleBarHit.MinimizeButton => Win32.HitTest.HTMINBUTTON,
        TitleBarHit.MaximizeButton => Win32.HitTest.HTMAXBUTTON,
        TitleBarHit.CloseButton => Win32.HitTest.HTCLOSE,
        TitleBarHit.ResizeTop => Win32.HitTest.HTTOP,
        TitleBarHit.ResizeTopLeft => Win32.HitTest.HTTOPLEFT,
        TitleBarHit.ResizeTopRight => Win32.HitTest.HTTOPRIGHT,
        _ => Win32.HitTest.HTCLIENT
    });

    /// <summary>
    /// Tlacidlo okna podla kodu WM_NCHITTEST zo sprav mysi mimo klientskej oblasti (wParam).
    /// </summary>
    /// <returns><see langword="null" />, ak kod nie je tlacidlo okna</returns>
    public static TitleBarHit? CaptionButtonOf(int hitCode) => (Win32.HitTest)hitCode switch
    {
        Win32.HitTest.HTMINBUTTON => TitleBarHit.MinimizeButton,
        Win32.HitTest.HTMAXBUTTON => TitleBarHit.MaximizeButton,
        Win32.HitTest.HTCLOSE => TitleBarHit.CloseButton,
        _ => null
    };

    /// <summary>
    /// Systemovy prikaz (WM_SYSCOMMAND) tlacidla okna - cez neho ma okno systemove animacie a FormClosing.
    /// </summary>
    public static int SysCommandOf(TitleBarHit button, bool maximized) => (int)(button switch
    {
        TitleBarHit.MinimizeButton => Win32.SysCommand.SC_MINIMIZE,
        TitleBarHit.MaximizeButton => maximized ? Win32.SysCommand.SC_RESTORE : Win32.SysCommand.SC_MAXIMIZE,
        TitleBarHit.CloseButton => Win32.SysCommand.SC_CLOSE,
        _ => throw new ArgumentOutOfRangeException(nameof(button), button, null)
    });

    /// <summary>
    /// Bod obrazovky z lParam sprav mysi mimo klientskej oblasti (suradnice so znamienkom - monitor vlavo/hore od hlavneho).
    /// </summary>
    public static Point ScreenPoint(IntPtr lParam)
    {
        var value = unchecked((int)lParam.ToInt64());
        return new Point(unchecked((short)value), unchecked((short)(value >> 16)));
    }
}
