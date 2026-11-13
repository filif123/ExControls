using ExControls.Docking;

namespace ExControls.Tests.Controls.Docking;

/// <summary>
/// Nested rows and columns of <see cref="ExDockManager" />: a group split in any direction by an arrow of the compass or
/// from code, a column next to the document, the tree simplified after removing a panel, the nested splitter and the
/// preview of a new column.
/// </summary>
[TestClass]
public class ExDockManagerNestingTests
{
    private sealed class TestDockManager : ExDockManager
    {
        public TestDockManager() => Size = new Size(800, 600);

        public void Drag(Point from, Point to)
        {
            OnMouseDown(new MouseEventArgs(MouseButtons.Left, 1, from.X, from.Y, 0));
            OnMouseMove(new MouseEventArgs(MouseButtons.Left, 0, to.X, to.Y, 0));
            OnMouseUp(new MouseEventArgs(MouseButtons.Left, 1, to.X, to.Y, 0));
        }
    }

    private static Rectangle ViewOf(ExDockPanel panel) => panel.Parent!.Bounds;

    [TestMethod]
    public void Drop_SipkaNapriecZonou_RiadkyVStlpciZonyDole()
    {
        using var manager = new TestDockManager();
        var analysis = new ExDockPanel("analysis", "Kontrola");
        var output = new ExDockPanel("output", "Vystup");
        var tree = new ExDockPanel("tree", "Stanice");
        manager.AddPanel(analysis, ExDockSide.Bottom);
        manager.AddPanel(output, ExDockSide.Bottom);
        manager.AddPanel(tree, ExDockSide.Left);

        manager.Drop(tree.Group!, null, new DockDropTarget(DockDropKind.Split, ExDockSide.Top, analysis.Group), Rectangle.Empty);

        var root = manager.Zones[(int)ExDockSide.Bottom].Root;
        Assert.AreEqual(2, root.Children.Count);
        var column = (DockSplit)root.Children[0];
        Assert.IsFalse(column.SideBySide);
        CollectionAssert.AreEqual(new DockNode[] { tree.Group!, analysis.Group! }, column.Children.ToList());
        Assert.AreEqual(ExDockSide.Bottom, tree.Side);
        Assert.AreEqual(ViewOf(analysis).X, ViewOf(tree).X);
        Assert.AreEqual(ViewOf(analysis).Width, ViewOf(tree).Width);
        Assert.IsTrue(ViewOf(tree).Bottom < ViewOf(analysis).Y);
        Assert.IsTrue(ViewOf(analysis).Right < ViewOf(output).X);
        Assert.AreEqual(ViewOf(output).Y, ViewOf(tree).Y, "the output column spans both rows");
    }

    [TestMethod]
    public void Drop_SipkaVlastnejSkupiny_KartaVedlaSkupiny()
    {
        using var manager = new TestDockManager();
        var analysis = new ExDockPanel("analysis", "Kontrola");
        var output = new ExDockPanel("output", "Vystup");
        manager.AddPanel(analysis, ExDockSide.Left);
        manager.AddPanel(output, analysis);

        manager.Drop(analysis.Group!, output, new DockDropTarget(DockDropKind.Split, ExDockSide.Right, analysis.Group), Rectangle.Empty);

        Assert.AreNotEqual(analysis.Group, output.Group);
        Assert.AreEqual(ViewOf(analysis).Y, ViewOf(output).Y);
        Assert.IsTrue(ViewOf(analysis).Right < ViewOf(output).X);
    }

    [TestMethod]
    public void Drop_SipkaDokumentu_RozdeliDokumentNieZonu()
    {
        // ako Visual Studio: vpravo od dokumentu, nad panelom dole, vlavo od zony vpravo cez celu vysku
        using var manager = new TestDockManager();
        var find = new ExDockPanel("find", "Hladanie");
        var output = new ExDockPanel("output", "Vystup");
        var analysis = new ExDockPanel("analysis", "Kontrola");
        manager.AddPanel(find, ExDockSide.Right);
        manager.AddPanel(output, ExDockSide.Bottom);
        manager.AddPanel(analysis, ExDockSide.Left);
        var findBefore = ViewOf(find);

        manager.Drop(analysis.Group!, null, new DockDropTarget(DockDropKind.Document, ExDockSide.Right), Rectangle.Empty);

        var view = ViewOf(analysis);
        Assert.AreEqual(ExDockState.Docked, analysis.State);
        Assert.AreEqual(ExDockSide.Right, analysis.Side);
        Assert.AreEqual(manager.Center, analysis.Group!.Zone);
        Assert.AreEqual(findBefore, ViewOf(find), "the right zone does not change");
        Assert.AreEqual(manager.Document.Bounds.Y, view.Y);
        Assert.AreEqual(manager.Document.Bounds.Height, view.Height, "only the height of the document");
        Assert.IsTrue(view.Bottom < ViewOf(output).Y);
        Assert.IsTrue(view.Right < ViewOf(find).X);
        Assert.AreEqual(manager.Document.Bounds.Right + manager.SplitterSize, view.X);
        // dokument a skupina si delia miesto medzi zonami (vlavo uz zona nie je)
        Assert.AreEqual(ViewOf(find).X - manager.SplitterSize, manager.Document.Bounds.Width + manager.SplitterSize + view.Width);
        Assert.AreEqual(0, manager.Zones[(int)ExDockSide.Left].Groups.Count);
    }

    [TestMethod]
    public void SkupinaPriDokumente_ZbaliSaNaListuSvojejStrany()
    {
        using var manager = new TestDockManager();
        var analysis = new ExDockPanel("analysis", "Kontrola");
        manager.AddPanel(analysis, ExDockSide.Left);
        manager.Drop(analysis.Group!, null, new DockDropTarget(DockDropKind.Document, ExDockSide.Bottom), Rectangle.Empty);

        manager.SetAutoHide(analysis, true);

        Assert.AreEqual(ExDockState.AutoHide, analysis.State);
        Assert.AreEqual(ExDockSide.Bottom, manager.StripButtons.Single().Side);
        Assert.AreEqual(manager.LayoutResult.Inner.Height, manager.Document.Bounds.Height);

        manager.ShowPanel(analysis);
        Assert.AreEqual(manager.LayoutResult.Inner.Bottom, manager.Popup.Bottom);
        Assert.AreEqual(manager.LogicalToDeviceUnits(manager.GetZoneSize(ExDockSide.Bottom)), manager.Popup.Height);

        manager.SetAutoHide(analysis, false);
        Assert.AreEqual(manager.Center, analysis.Group!.Zone);
        Assert.AreEqual(manager.Document.Bounds.Bottom + manager.SplitterSize, ViewOf(analysis).Y);
    }

    [TestMethod]
    public void AddPanel_VedlaPanela_RozdeliSkupinuNaRiadkyAStlpce()
    {
        using var manager = new TestDockManager();
        var analysis = new ExDockPanel("analysis", "Kontrola");
        var output = new ExDockPanel("output", "Vystup");
        var log = new ExDockPanel("log", "Log");
        manager.AddPanel(analysis, ExDockSide.Bottom);

        manager.AddPanel(output, analysis, ExDockSide.Right);
        manager.AddPanel(log, output, ExDockSide.Bottom);

        Assert.IsTrue(ViewOf(analysis).Right < ViewOf(output).X);
        Assert.AreEqual(ViewOf(output).X, ViewOf(log).X);
        Assert.IsTrue(ViewOf(output).Bottom < ViewOf(log).Y);
        Assert.AreEqual(ViewOf(analysis).Height, ViewOf(log).Bottom - ViewOf(output).Y);
        Assert.ThrowsExactly<InvalidOperationException>(() => manager.DockPanel(log, log, ExDockSide.Left));
    }

    [TestMethod]
    public void RemovePanel_StromSaZjednodusi()
    {
        using var manager = new TestDockManager();
        var analysis = new ExDockPanel("analysis", "Kontrola");
        var output = new ExDockPanel("output", "Vystup");
        manager.AddPanel(analysis, ExDockSide.Bottom);
        manager.AddPanel(output, analysis, ExDockSide.Top);

        manager.RemovePanel(output);

        var root = manager.Zones[(int)ExDockSide.Bottom].Root;
        CollectionAssert.AreEqual(new DockNode[] { analysis.Group! }, root.Children.ToList());
        Assert.AreEqual(1, analysis.Group!.Weight);
        output.Dispose();
    }

    [TestMethod]
    public void HidePanel_VnorenaSkupinaNezaberaMiestoAleOstavaVStrome()
    {
        using var manager = new TestDockManager();
        var analysis = new ExDockPanel("analysis", "Kontrola");
        var output = new ExDockPanel("output", "Vystup");
        manager.AddPanel(analysis, ExDockSide.Bottom);
        manager.AddPanel(output, analysis, ExDockSide.Top);
        var before = ViewOf(output);

        manager.HidePanel(output);
        Assert.AreEqual(manager.LayoutResult.Zones[(int)ExDockSide.Bottom], ViewOf(analysis));

        manager.ShowPanel(output);
        Assert.AreEqual(before, ViewOf(output));
    }

    [TestMethod]
    public void Rozdelovac_VnorenyRiadokSaDaTiahnut()
    {
        using var manager = new TestDockManager();
        var analysis = new ExDockPanel("analysis", "Kontrola");
        var output = new ExDockPanel("output", "Vystup");
        manager.AddPanel(analysis, ExDockSide.Left);
        manager.AddPanel(output, analysis, ExDockSide.Right);
        var splitter = manager.LayoutResult.Splitters.Single(s => !s.IsZone).Bounds;
        var width = ViewOf(analysis).Width;
        var start = new Point(splitter.X + 1, splitter.Y + 50);

        manager.Drag(start, new Point(start.X - 30, start.Y));

        Assert.AreEqual(width - 30, ViewOf(analysis).Width);
        Assert.AreEqual(ViewOf(analysis).Right + manager.SplitterSize, ViewOf(output).X);
    }

    [TestMethod]
    public void Nahlad_OkrajZonyACastDokumentu()
    {
        using var manager = new TestDockManager();
        var tree = new ExDockPanel("tree", "Stanice");
        manager.AddPanel(tree, ExDockSide.Left);
        var zone = manager.LayoutResult.Zones[(int)ExDockSide.Left];
        var document = manager.Document.Bounds;

        var outer = manager.ZonePreview(ExDockSide.Left);
        var empty = manager.ZonePreview(ExDockSide.Bottom);
        var right = manager.DocumentPreview(ExDockSide.Right);

        Assert.AreEqual(new Rectangle(zone.X, zone.Y, zone.Width / 2, zone.Height), outer);
        Assert.AreEqual(manager.LogicalToDeviceUnits(200), empty.Height);
        Assert.AreEqual(600, empty.Bottom);
        Assert.AreEqual(document.Right, right.Right);
        Assert.AreEqual(document.Height, right.Height);
        Assert.AreEqual((int)(document.Width * 0.3), right.Width);
    }
}
