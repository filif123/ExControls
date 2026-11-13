using ExControls.Docking;

namespace ExControls.Tests.Controls.Docking;

/// <summary>
/// Docking guides: squares in the middle of the edges of the manager, a compass (cross like in Visual Studio) over the
/// group or the document under the cursor, the compass wins over an outer guide.
/// </summary>
[TestClass]
public class DockGuidesTests
{
    private static readonly Rectangle Area = new(100, 50, 800, 600);

    [TestMethod]
    [DataRow(ExDockSide.Left, 108, 334)]
    [DataRow(ExDockSide.Right, 860, 334)]
    [DataRow(ExDockSide.Top, 484, 58)]
    [DataRow(ExDockSide.Bottom, 484, 610)]
    public void Outer_VStredeOkrajaOdsadene(ExDockSide side, int x, int y)
    {
        Assert.AreEqual(new Rectangle(x, y, 32, 32), DockGuides.Outer(Area, side, 32, 8));
    }

    [TestMethod]
    public void Arrow_VedlaStreduSMedzerou()
    {
        var center = DockGuides.Center(new Rectangle(0, 0, 200, 100), 32);

        Assert.AreEqual(new Rectangle(84, 34, 32, 32), center);
        Assert.AreEqual(new Rectangle(50, 34, 32, 32), DockGuides.Arrow(center, ExDockSide.Left, 2));
        Assert.AreEqual(new Rectangle(84, 68, 32, 32), DockGuides.Arrow(center, ExDockSide.Bottom, 2));
    }

    [TestMethod]
    public void CompassPlate_KrizikOkoloVsetkychTlacidielSoSkosenymiRohmi()
    {
        var center = new Rectangle(100, 100, 32, 32);
        var plate = DockGuides.CompassPlate(center, 12, 4);

        Assert.AreEqual(16, plate.Length);
        // krajne body ramien = sipky + okraj 4
        Assert.AreEqual(100 - 12 - 32 - 4, plate.Min(p => p.X));
        Assert.AreEqual(132 + 12 + 32 + 4, plate.Max(p => p.X));
        Assert.AreEqual(100 - 12 - 32 - 4, plate.Min(p => p.Y));
        Assert.AreEqual(132 + 12 + 32 + 4, plate.Max(p => p.Y));
        // skosenie vnutorneho rohu medzi hornym a pravym ramenom
        CollectionAssert.Contains(plate, new Point(136, 90));
        CollectionAssert.Contains(plate, new Point(142, 96));
        foreach (var side in DockGuides.Sides)
            Assert.IsTrue(Contains(plate, Center(DockGuides.Arrow(center, side, 12))), side.ToString());
        Assert.IsFalse(Contains(plate, new Point(60, 60)), "outside the cross");
    }

    [TestMethod]
    public void AvoidOuter_KompasSaOdsunieOdZnaciekPriOkraji()
    {
        var bottom = new DockGuides.OuterGuide(ExDockSide.Bottom, new Rectangle(380, 550, 40, 40));
        var right = new DockGuides.OuterGuide(ExDockSide.Right, new Rectangle(750, 280, 40, 40));
        var compass = new Rectangle(340, 480, 128, 128);

        Assert.AreEqual(new Point(0, -4 - 58), DockGuides.AvoidOuter(compass, [bottom, right], 4));
        Assert.AreEqual(new Point(-4 - 68, 0), DockGuides.AvoidOuter(new Rectangle(690, 250, 128, 128), [bottom, right], 4));
        Assert.AreEqual(Point.Empty, DockGuides.AvoidOuter(new Rectangle(100, 100, 128, 128), [bottom, right], 4));
    }

    [TestMethod]
    public void Plate_StvorecOkoloTlacidla()
    {
        CollectionAssert.AreEqual(new[] { new Point(6, 6), new Point(46, 6), new Point(46, 46), new Point(6, 46) },
            DockGuides.Plate(new Rectangle(10, 10, 32, 32), 4));
    }

    private static Point Center(Rectangle r) => new(r.X + r.Width / 2, r.Y + r.Height / 2);

    private static bool Contains(Point[] polygon, Point point)
    {
        using var path = new System.Drawing.Drawing2D.GraphicsPath();
        path.AddPolygon(polygon);
        return path.IsVisible(point);
    }

    [TestMethod]
    public void HitTest_NeskorPridanaZnackaMaPrednost()
    {
        var edge = new DockGuide(new Rectangle(0, 0, 40, 40), new DockDropTarget(DockDropKind.Edge, ExDockSide.Left), ExDockSide.Left);
        var tab = new DockGuide(new Rectangle(20, 20, 40, 40), new DockDropTarget(DockDropKind.Tab, ExDockSide.Bottom), null);

        Assert.AreEqual(tab, DockGuides.HitTest([edge, tab], new Point(30, 30)));
        Assert.AreEqual(edge, DockGuides.HitTest([edge, tab], new Point(5, 5)));
        Assert.IsNull(DockGuides.HitTest([edge, tab], new Point(100, 100)));
    }

    [TestMethod]
    public void Preview_KartaCelaSkupinaSipkaPolovica()
    {
        var group = new Rectangle(0, 0, 200, 100);

        Assert.AreEqual(group, DockGuides.Preview(group, new DockDropTarget(DockDropKind.Tab, ExDockSide.Bottom)));
        Assert.AreEqual(new Rectangle(100, 0, 100, 100), DockGuides.Preview(group, new DockDropTarget(DockDropKind.Split, ExDockSide.Right)));
        Assert.AreEqual(new Rectangle(0, 0, 200, 50), DockGuides.Preview(group, new DockDropTarget(DockDropKind.Split, ExDockSide.Top)));
        Assert.IsTrue(new DockDropTarget(DockDropKind.Split, ExDockSide.Top).Before);
        Assert.IsFalse(new DockDropTarget(DockDropKind.Split, ExDockSide.Bottom).Before);
    }
}
