namespace ExControls;

/// <summary>
/// Splits the top of a form with a custom title bar into parts for WM_NCHITTEST (pure, without a window).
/// </summary>
internal static class TitleBarHitTest
{
    /// <summary>
    /// Determines the part of the window at a point of the client area that the default handling evaluated as client.
    /// </summary>
    /// <param name="point">point in client coordinates of the window</param>
    /// <param name="clientWidth">width of the client area</param>
    /// <param name="resizeBorder">
    /// thickness of the top resize border; 0 = the window is not resized from the top (maximized, fixed frame)
    /// </param>
    /// <param name="titleBar">bounds of the title bar in the client area</param>
    /// <param name="titleBarHitTest">part of the title bar at a point of the title bar (<see cref="ExTitleBar.HitTest" />)</param>
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
    /// Return value of WM_NCHITTEST for a part of the window.
    /// </summary>
    /// <param name="hit">part of the window</param>
    public static int ToHitCode(TitleBarHit hit) => (int)(hit switch
    {
        TitleBarHit.Caption => Win32.HitTest.HTCAPTION,
        TitleBarHit.SystemMenu => Win32.HitTest.HTSYSMENU,
        TitleBarHit.MinimizeButton => Win32.HitTest.HTMINBUTTON,
        TitleBarHit.MaximizeButton => Win32.HitTest.HTMAXBUTTON,
        TitleBarHit.CloseButton => Win32.HitTest.HTCLOSE,
        TitleBarHit.HelpButton => Win32.HitTest.HTHELP,
        TitleBarHit.ResizeTop => Win32.HitTest.HTTOP,
        TitleBarHit.ResizeTopLeft => Win32.HitTest.HTTOPLEFT,
        TitleBarHit.ResizeTopRight => Win32.HitTest.HTTOPRIGHT,
        _ => Win32.HitTest.HTCLIENT
    });

    /// <summary>
    /// Window button by the WM_NCHITTEST code from non-client mouse messages (wParam).
    /// </summary>
    /// <param name="hitCode">WM_NCHITTEST code</param>
    /// <returns><see langword="null" /> if the code is not a window button</returns>
    public static TitleBarHit? CaptionButtonOf(int hitCode) => (Win32.HitTest)hitCode switch
    {
        Win32.HitTest.HTMINBUTTON => TitleBarHit.MinimizeButton,
        Win32.HitTest.HTMAXBUTTON => TitleBarHit.MaximizeButton,
        Win32.HitTest.HTCLOSE => TitleBarHit.CloseButton,
        Win32.HitTest.HTHELP => TitleBarHit.HelpButton,
        _ => null
    };

    /// <summary>
    /// System command (WM_SYSCOMMAND) of a window button - through it the window gets the system animations, FormClosing and
    /// HelpButtonClicked.
    /// </summary>
    /// <param name="button">window button</param>
    /// <param name="maximized">the window is maximized (the maximize button restores it)</param>
    public static int SysCommandOf(TitleBarHit button, bool maximized) => (int)(button switch
    {
        TitleBarHit.MinimizeButton => Win32.SysCommand.SC_MINIMIZE,
        TitleBarHit.MaximizeButton => maximized ? Win32.SysCommand.SC_RESTORE : Win32.SysCommand.SC_MAXIMIZE,
        TitleBarHit.CloseButton => Win32.SysCommand.SC_CLOSE,
        TitleBarHit.HelpButton => Win32.SysCommand.SC_CONTEXTHELP,
        _ => throw new ArgumentOutOfRangeException(nameof(button), button, null)
    });

    /// <summary>
    /// Screen point from lParam of non-client mouse messages (signed coordinates - a monitor left of or above the primary one).
    /// </summary>
    /// <param name="lParam">lParam of the message</param>
    public static Point ScreenPoint(IntPtr lParam)
    {
        var value = unchecked((int)lParam.ToInt64());
        return new Point(unchecked((short)value), unchecked((short)(value >> 16)));
    }
}
