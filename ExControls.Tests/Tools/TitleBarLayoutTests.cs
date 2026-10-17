namespace ExControls.Tests.Tools;

/// <summary>
/// Rozlozenie vlastneho zahlavia: ikona vlavo, tlacidla okna vpravo podla pravidiel systemoveho zahlavia, text vo volnom mieste.
/// </summary>
[TestClass]
public class TitleBarLayoutTests
{
    private static readonly Size Bar = new(600, 32);

    [TestMethod]
    public void Compute_IkonaVlavoTlacidlaVpravo()
    {
        var layout = TitleBarLayout.Compute(Bar, 36, 46, true, true, true, true);

        Assert.AreEqual(new Rectangle(0, 0, 36, 32), layout.Icon);
        Assert.AreEqual(new Rectangle(554, 0, 46, 32), layout.Close);
        Assert.AreEqual(new Rectangle(508, 0, 46, 32), layout.Maximize);
        Assert.AreEqual(new Rectangle(462, 0, 46, 32), layout.Minimize);
        Assert.AreEqual(36, layout.LeftInset);
        Assert.AreEqual(138, layout.RightInset);
    }

    [TestMethod]
    public void Compute_BezControlBox_BezIkonyATlacidiel()
    {
        var layout = TitleBarLayout.Compute(Bar, 36, 46, true, false, true, true);

        Assert.IsTrue(layout.Icon.IsEmpty && layout.Minimize.IsEmpty && layout.Maximize.IsEmpty && layout.Close.IsEmpty);
        Assert.AreEqual(0, layout.LeftInset);
        Assert.AreEqual(0, layout.RightInset);
        Assert.IsNull(layout.HitTest(new Point(590, 10)));
    }

    [TestMethod]
    public void Compute_BezMinimalizacieAMaximalizacie_LenZatvorenie()
    {
        var layout = TitleBarLayout.Compute(Bar, 36, 46, false, true, false, false);

        Assert.IsTrue(layout.Icon.IsEmpty);
        Assert.IsTrue(layout.Minimize.IsEmpty && layout.Maximize.IsEmpty);
        Assert.AreEqual(46, layout.RightInset);
    }

    [TestMethod]
    [DataRow(false, true)]
    [DataRow(true, false)]
    public void Compute_JednoZMinimalizacieAMaximalizacie_ObeTlacidlaVypnuteZakazane(bool minimizeBox, bool maximizeBox)
    {
        var layout = TitleBarLayout.Compute(Bar, 36, 46, true, true, minimizeBox, maximizeBox);

        Assert.IsFalse(layout.Minimize.IsEmpty);
        Assert.IsFalse(layout.Maximize.IsEmpty);
        Assert.AreEqual(minimizeBox, layout.MinimizeEnabled);
        Assert.AreEqual(maximizeBox, layout.MaximizeEnabled);
        // zakazane tlacidlo je volne miesto - da sa za neho tahat okno
        Assert.AreEqual(minimizeBox ? TitleBarHit.MinimizeButton : TitleBarHit.Caption, layout.HitTest(new Point(480, 10)));
        Assert.AreEqual(maximizeBox ? TitleBarHit.MaximizeButton : TitleBarHit.Caption, layout.HitTest(new Point(530, 10)));
    }

    [TestMethod]
    [DataRow(10, 10, TitleBarHit.SystemMenu)]
    [DataRow(470, 0, TitleBarHit.MinimizeButton)]
    [DataRow(520, 31, TitleBarHit.MaximizeButton)]
    [DataRow(599, 0, TitleBarHit.CloseButton)]
    [DataRow(554, 16, TitleBarHit.CloseButton)]
    public void HitTest_IkonaATlacidla(int x, int y, TitleBarHit expected)
    {
        var layout = TitleBarLayout.Compute(Bar, 36, 46, true, true, true, true);

        Assert.AreEqual(expected, layout.HitTest(new Point(x, y)));
    }

    [TestMethod]
    [DataRow(36, 10)]
    [DataRow(300, 16)]
    [DataRow(461, 16)]
    public void HitTest_MimoIkonyATlacidielNic(int x, int y)
    {
        var layout = TitleBarLayout.Compute(Bar, 36, 46, true, true, true, true);

        Assert.IsNull(layout.HitTest(new Point(x, y)));
    }

    [TestMethod]
    public void BoundsOf_PlochaTlacidla()
    {
        var layout = TitleBarLayout.Compute(Bar, 36, 46, true, true, true, true);

        Assert.AreEqual(layout.Close, layout.BoundsOf(TitleBarHit.CloseButton));
        Assert.AreEqual(layout.Maximize, layout.BoundsOf(TitleBarHit.MaximizeButton));
        Assert.AreEqual(layout.Minimize, layout.BoundsOf(TitleBarHit.MinimizeButton));
        Assert.AreEqual(Rectangle.Empty, layout.BoundsOf(TitleBarHit.Caption));
    }

    private static readonly Rectangle Area = new(40, 0, 400, 32);

    [TestMethod]
    public void PlaceTitle_ZmestiSaNaStredZahlavia()
    {
        Rectangle[] menu = [new(40, 4, 120, 24)];

        var rect = TitleBarLayout.PlaceTitle(100, Area, menu, 300, 40);

        Assert.AreEqual(new Rectangle(250, 0, 100, 32), rect);
    }

    [TestMethod]
    public void PlaceTitle_StredJeObsadeny_CoNajblizsieKStreduVoVolnomUseku()
    {
        Rectangle[] menu = [new(40, 4, 300, 24)];

        var rect = TitleBarLayout.PlaceTitle(60, Area, menu, 300, 40);

        Assert.AreEqual(new Rectangle(340, 0, 60, 32), rect);
    }

    [TestMethod]
    public void PlaceTitle_VyberieNajsirsiUsek()
    {
        Rectangle[] controls = [new(40, 4, 100, 24), new(200, 4, 40, 24)];

        var rect = TitleBarLayout.PlaceTitle(80, Area, controls, 180, 40);

        // useky: 140-200 (60), 240-440 (200) - text ide do sirsieho co najblizsie k stredu 180
        Assert.AreEqual(new Rectangle(240, 0, 80, 32), rect);
    }

    [TestMethod]
    public void PlaceTitle_DlhyTextSaSkratiNaSirkuUseku()
    {
        Rectangle[] menu = [new(40, 4, 250, 24)];

        var rect = TitleBarLayout.PlaceTitle(500, Area, menu, 300, 40);

        Assert.AreEqual(new Rectangle(290, 0, 150, 32), rect);
    }

    [TestMethod]
    public void PlaceTitle_MaloMiesta_BezTextu()
    {
        Rectangle[] menu = [new(40, 4, 380, 24)];

        Assert.AreEqual(Rectangle.Empty, TitleBarLayout.PlaceTitle(100, Area, menu, 300, 40));
        Assert.AreEqual(Rectangle.Empty, TitleBarLayout.PlaceTitle(0, Area, [], 300, 40));
    }

    [TestMethod]
    public void PlaceTitle_PrvkyMimoOblastiSaIgnoruju()
    {
        Rectangle[] outside = [new(0, 0, 30, 32), new(450, 0, 100, 32)];

        Assert.AreEqual(new Rectangle(250, 0, 100, 32), TitleBarLayout.PlaceTitle(100, Area, outside, 300, 40));
    }

    [TestMethod]
    public void Blend_ZmesFarieb()
    {
        Assert.AreEqual(Color.FromArgb(0, 0, 0), TitleBarColors.Blend(Color.Black, Color.White, 0));
        Assert.AreEqual(Color.FromArgb(255, 255, 255), TitleBarColors.Blend(Color.Black, Color.White, 1));
        Assert.AreEqual(Color.FromArgb(128, 128, 128), TitleBarColors.Blend(Color.Black, Color.White, 0.5));
        Assert.AreEqual(Color.FromArgb(230, 230, 230), TitleBarColors.ButtonHover(Color.White, Color.Black));
    }

    [TestMethod]
    public void CloseHover_CervenaPodlaSystemu()
    {
        // Windows 11 #C42B1C, stlacene mierne prekryte pozadim; Windows 10 #E81123, stlacene zosvetlene (#F1707A)
        Assert.AreEqual(Color.FromArgb(196, 43, 28), TitleBarColors.CloseHover(true));
        Assert.AreEqual(Color.FromArgb(232, 17, 35), TitleBarColors.CloseHover(false));
        Assert.AreEqual(Color.FromArgb(202, 64, 51), TitleBarColors.ClosePressed(TitleBarColors.CloseHover(true), Color.White, true));
        Assert.AreEqual(Color.FromArgb(241, 112, 123), TitleBarColors.ClosePressed(TitleBarColors.CloseHover(false), Color.Black, false));
    }
}
