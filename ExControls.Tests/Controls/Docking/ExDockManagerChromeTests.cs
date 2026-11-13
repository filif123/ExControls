using ExControls.Docking;

namespace ExControls.Tests.Controls.Docking;

/// <summary>
/// Frame of <see cref="ExDockManager" />: the splitter cursor only over the splitters (panels inside never inherit it) and
/// the window menu in the colors of the theme also without a menu handler of the application.
/// </summary>
[TestClass]
public class ExDockManagerChromeTests
{
    private static readonly ExTheme Dark = new()
    {
        PanelBackColor = Color.FromArgb(43, 45, 48),
        PanelForeColor = Color.FromArgb(223, 225, 229),
        BoxBackColor = Color.FromArgb(30, 31, 34),
        BoxForeColor = Color.FromArgb(223, 225, 229),
        HighlightBackColor = Color.FromArgb(46, 67, 110),
        HighlightForeColor = Color.White
    };

    private sealed class TestDockManager : ExDockManager
    {
        public void MoveMouse(Point point) => OnMouseMove(new MouseEventArgs(MouseButtons.None, 0, point.X, point.Y, 0));
    }

    private sealed class Plain : Control;

    private sealed class HandledMenu : ContextMenuStrip;

    [TestMethod]
    public void Kurzor_RozdelovacLenNadRozdelovacomPanelyHoNededia()
    {
        using var manager = new TestDockManager { Size = new Size(800, 600) };
        var tree = new ExDockPanel("tree", "Stanice");
        var content = new TreeView { Dock = DockStyle.Fill };
        tree.Controls.Add(content);
        manager.AddPanel(tree, ExDockSide.Left);
        var splitter = manager.LayoutResult.ZoneSplitter(ExDockSide.Left);
        var center = new Point(splitter.X + splitter.Width / 2, splitter.Y + splitter.Height / 2);

        manager.MoveMouse(center);

        Assert.AreEqual(Cursors.VSplit, manager.SplitterCursor(center));
        Assert.IsNull(manager.SplitterCursor(new Point(20, 300)), "inside the panel");
        Assert.AreEqual(Cursors.Default, manager.Cursor);
        Assert.AreEqual(Cursors.Default, content.Cursor, "the tree in the panel inherits the cursor of the manager");
    }

    [TestMethod]
    public void Kurzor_VysunutyPanelLenNadOkrajom()
    {
        using var manager = new ExDockManager { Size = new Size(800, 600) };
        var props = new ExDockPanel("props", "Vlastnosti");
        manager.AddPanel(props, ExDockSide.Right);
        manager.SetAutoHide(props, true);
        manager.ShowPanel(props);
        var popup = manager.Popup;

        Assert.AreEqual(Cursors.VSplit, popup.EdgeCursor(new Point(1, popup.Height / 2)));
        Assert.IsNull(popup.EdgeCursor(new Point(popup.Width / 2, popup.Height / 2)));
        Assert.AreEqual(Cursors.Default, popup.Cursor);
    }

    [TestMethod]
    public void CreateMenu_BezObsluhyProgramu_FarbyTemy()
    {
        using var manager = new ExDockManager { Size = new Size(800, 600), Strings = new ExDockStrings { AutoHide = "Automaticky skryť" } };
        var tree = new ExDockPanel("tree", "Stanice");
        manager.AddPanel(tree, ExDockSide.Left);
        manager.ApplyTheme(Dark);

        using var menu = manager.CreateMenu(tree.Group!, tree)!;

        Assert.IsInstanceOfType<DockMenuRenderer>(menu.Renderer);
        Assert.AreEqual(Dark.BoxBackColor, menu.BackColor);
        Assert.AreEqual(Dark.BoxForeColor, menu.ForeColor);
        CollectionAssert.AreEqual(new[] { "Float", "Automaticky skryť", "", "Hide" }, menu.Items.Cast<ToolStripItem>().Select(i => i.Text).ToList());
        Assert.IsFalse(((ToolStripMenuItem)menu.Items[1]).Checked);
    }

    [TestMethod]
    public void CreateMenu_PlavajuciZbalenyASystemovyStyl()
    {
        using var manager = new ExDockManager { Size = new Size(800, 600) };
        var tree = new ExDockPanel("tree", "Stanice") { CanClose = false };
        manager.AddPanel(tree, ExDockSide.Left);
        manager.ApplyTheme(new ExTheme { UseSystemStyle = true });

        manager.SetAutoHide(tree, true);
        using (var collapsed = manager.CreateMenu(tree.Group!, tree)!)
        {
            CollectionAssert.AreEqual(new[] { "Auto Hide" }, collapsed.Items.Cast<ToolStripItem>().Select(i => i.Text).ToList());
            Assert.IsTrue(((ToolStripMenuItem)collapsed.Items[0]).Checked);
            Assert.IsNotInstanceOfType<DockMenuRenderer>(collapsed.Renderer);
        }

        manager.FloatPanel(tree);
        using var floating = manager.CreateMenu(tree.Group!, tree)!;
        CollectionAssert.AreEqual(new[] { "Dock" }, floating.Items.Cast<ToolStripItem>().Select(i => i.Text).ToList());
    }

    [TestMethod]
    public void Znacky_OkrajoveVzdyKompasOdsunutyALepivy()
    {
        using var manager = new ExDockManager { Size = new Size(800, 600) };
        _ = manager.Handle;
        var tree = new ExDockPanel("tree", "Stanice");
        var output = new ExDockPanel("output", "Vystup");
        // zona hore nemeni vodorovny stred - Vystup cez celu sirku je presne pod znackou dolneho okraja
        manager.AddPanel(tree, ExDockSide.Top);
        manager.AddPanel(output, ExDockSide.Bottom);
        manager.SetZoneSize(ExDockSide.Bottom, 100);
        var bounds = manager.RectangleToScreen(manager.LayoutResult.BoundsOf(output.Group!));
        var over = new Point(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2);

        var set = manager.GuidesAt(over, tree.Group!, null, null);

        var edges = set.Guides.Where(g => g.Target.Kind == DockDropKind.Edge).ToList();
        Assert.AreEqual(4, edges.Count, "the guides at the edges stay");
        Assert.AreEqual(output.Group, set.CompassGroup);
        var bottom = edges.Single(g => g.Target.Side == ExDockSide.Bottom).Bounds;
        Assert.IsTrue(set.CompassBounds.Bottom < bottom.Top, "the compass moves above the bottom guide");
        Assert.IsFalse(bounds.Contains(Center(set.Guides.Last(g => g.Target.Kind == DockDropKind.Split && g.Target.Side == ExDockSide.Top).Bounds)));

        // horna sipka je uz nad dokumentom - kompas ostava pri skupine Vystup
        var top = Center(set.Guides.Last(g => g.Target.Kind == DockDropKind.Split && g.Target.Side == ExDockSide.Top).Bounds);
        var kept = manager.GuidesAt(top, tree.Group!, null, null, set);
        var fresh = manager.GuidesAt(top, tree.Group!, null, null);

        Assert.AreEqual(output.Group, kept.CompassGroup);
        Assert.AreEqual(DockDropKind.Split, DockGuides.HitTest(kept.Guides, top)!.Target.Kind);
        Assert.IsTrue(fresh.CompassDocument);
    }

    private static Point Center(Rectangle r) => new(r.X + r.Width / 2, r.Y + r.Height / 2);

    [TestMethod]
    public void Lista_TlacidloMaDlzkuCelehoTextuAkJeMiesto()
    {
        using var manager = new ExDockManager { Size = new Size(800, 600) };
        var analysis = new ExDockPanel("analysis", "Kontrola inštalácie");
        manager.AddPanel(analysis, ExDockSide.Bottom);

        manager.SetAutoHide(analysis, true);

        var button = manager.StripButtons.Single().Bounds;
        var padding = manager.LogicalToDeviceUnits(8);
        // cely text (meranie bez skracovania) sa zmesti do tlacidla aj s priznakmi kreslenia
        var full = TextRenderer.MeasureText(analysis.Text, manager.Font, Size.Empty, TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding).Width;
        Assert.AreEqual(full + 2 * padding, button.Width);
        var drawn = TextRenderer.MeasureText(analysis.Text, manager.Font, new Size(button.Width - 2 * padding, button.Height),
            DockPainter.StripTextFlags);
        Assert.AreEqual(full, drawn.Width);
    }

    [TestMethod]
    public void Lista_UzkaLista_TlacidlaSaZuziaAZmestia()
    {
        using var manager = new ExDockManager { Size = new Size(300, 400) };
        for (var i = 0; i < 5; i++)
        {
            var panel = new ExDockPanel("p" + i, "Kontrola inštalácie " + i);
            manager.AddPanel(panel, ExDockSide.Bottom);
            manager.SetAutoHide(panel, true);
        }

        var strip = manager.LayoutResult.Strips[(int)ExDockSide.Bottom];
        var buttons = manager.StripButtons.Select(b => b.Bounds).ToList();

        Assert.AreEqual(5, buttons.Count);
        Assert.IsTrue(buttons.TrueForAll(b => b.Left >= strip.Left && b.Right <= strip.Right), "all buttons fit the strip");
        Assert.IsTrue(buttons.Zip(buttons.Skip(1), (a, b) => a.Right <= b.Left).All(x => x), "no overlap");
    }

    [TestMethod]
    public void ApplyHandler_LenZaregistrovanaObsluha()
    {
        var handled = new List<Control>();
        ExThemer.Register<HandledMenu>((menu, _) => handled.Add(menu));
        using var menu = new HandledMenu();
        using var plain = new Plain();

        Assert.IsTrue(ExThemer.ApplyHandler(menu, Dark));
        Assert.IsFalse(ExThemer.ApplyHandler(plain, Dark));
        CollectionAssert.AreEqual(new Control[] { menu }, handled);
    }
}
