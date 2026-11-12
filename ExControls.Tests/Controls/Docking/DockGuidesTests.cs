using ExControls.Docking;

namespace ExControls.Tests.Controls.Docking;

/// <summary>
/// Docking guides: squares in the middle of the edges of the manager, a diamond over the group under the cursor with
/// split arrows only along the zone, the diamond wins over an outer guide.
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
    public void SplitDirections_LenPozdlzZony()
    {
        CollectionAssert.AreEqual(new[] { ExDockSide.Top, ExDockSide.Bottom }, DockGuides.SplitDirections(ExDockSide.Left));
        CollectionAssert.AreEqual(new[] { ExDockSide.Left, ExDockSide.Right }, DockGuides.SplitDirections(ExDockSide.Bottom));
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
