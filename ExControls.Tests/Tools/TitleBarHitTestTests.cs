namespace ExControls.Tests.Tools;

/// <summary>
/// Rozdelenie vrchu okna s vlastnym zahlavim: horny okraj na zmenu velkosti ma prednost, potom rozhoduje zahlavie.
/// </summary>
[TestClass]
public class TitleBarHitTestTests
{
    private static readonly Rectangle TitleBar = new(0, 0, 400, 32);

    [TestMethod]
    [DataRow(5, 2, 8, TitleBarHit.ResizeTopLeft)]
    [DataRow(15, 7, 8, TitleBarHit.ResizeTopLeft)]
    [DataRow(16, 2, 8, TitleBarHit.ResizeTop)]
    [DataRow(200, 0, 8, TitleBarHit.ResizeTop)]
    [DataRow(384, 2, 8, TitleBarHit.ResizeTopRight)]
    [DataRow(399, 7, 8, TitleBarHit.ResizeTopRight)]
    [DataRow(200, 8, 8, TitleBarHit.Caption)]
    [DataRow(200, 2, 0, TitleBarHit.Caption)]
    [DataRow(5, 2, 0, TitleBarHit.Caption)]
    [DataRow(200, 32, 8, TitleBarHit.Client)]
    [DataRow(200, 200, 8, TitleBarHit.Client)]
    public void Classify_HornyOkrajMaPrednostPredZahlavim(int x, int y, int resizeBorder, TitleBarHit expected)
    {
        var hit = TitleBarHitTest.Classify(new Point(x, y), 400, resizeBorder, TitleBar, _ => TitleBarHit.Caption);

        Assert.AreEqual(expected, hit);
    }

    [TestMethod]
    public void Classify_ZahlaviuPosleBodVJehoSuradniciach()
    {
        var received = Point.Empty;

        var hit = TitleBarHitTest.Classify(new Point(120, 20), 400, 8, new Rectangle(100, 10, 300, 32), p =>
        {
            received = p;
            return TitleBarHit.MaximizeButton;
        });

        Assert.AreEqual(TitleBarHit.MaximizeButton, hit);
        Assert.AreEqual(new Point(20, 10), received);
    }

    [TestMethod]
    [DataRow(TitleBarHit.Client, 1)]
    [DataRow(TitleBarHit.Caption, 2)]
    [DataRow(TitleBarHit.SystemMenu, 3)]
    [DataRow(TitleBarHit.MinimizeButton, 8)]
    [DataRow(TitleBarHit.MaximizeButton, 9)]
    [DataRow(TitleBarHit.ResizeTop, 12)]
    [DataRow(TitleBarHit.ResizeTopLeft, 13)]
    [DataRow(TitleBarHit.ResizeTopRight, 14)]
    [DataRow(TitleBarHit.CloseButton, 20)]
    public void ToHitCode_ZodpovedaHodnotamWin32(TitleBarHit hit, int expected)
    {
        Assert.AreEqual(expected, TitleBarHitTest.ToHitCode(hit));
    }

    [TestMethod]
    [DataRow(8, TitleBarHit.MinimizeButton)]
    [DataRow(9, TitleBarHit.MaximizeButton)]
    [DataRow(20, TitleBarHit.CloseButton)]
    [DataRow(2, null)]
    [DataRow(3, null)]
    [DataRow(12, null)]
    public void CaptionButtonOf_LenTlacidlaOkna(int hitCode, TitleBarHit? expected)
    {
        Assert.AreEqual(expected, TitleBarHitTest.CaptionButtonOf(hitCode));
    }

    [TestMethod]
    [DataRow(TitleBarHit.MinimizeButton, false, 0xF020)]
    [DataRow(TitleBarHit.MinimizeButton, true, 0xF020)]
    [DataRow(TitleBarHit.MaximizeButton, false, 0xF030)]
    [DataRow(TitleBarHit.MaximizeButton, true, 0xF120)]
    [DataRow(TitleBarHit.CloseButton, false, 0xF060)]
    public void SysCommandOf_MaximalizovaneOknoTlacidlomObnovi(TitleBarHit button, bool maximized, int expected)
    {
        Assert.AreEqual(expected, TitleBarHitTest.SysCommandOf(button, maximized));
    }

    [TestMethod]
    [DataRow(10, 20)]
    [DataRow(-5, -10)]
    [DataRow(-1920, 300)]
    [DataRow(2560, -1)]
    public void ScreenPoint_ZachovaZnamienkoSuradnic(int x, int y)
    {
        var lParam = (IntPtr)((y << 16) | (x & 0xFFFF));

        Assert.AreEqual(new Point(x, y), TitleBarHitTest.ScreenPoint(lParam));
    }
}
