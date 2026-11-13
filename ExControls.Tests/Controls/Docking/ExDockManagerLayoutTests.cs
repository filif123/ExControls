namespace ExControls.Tests.Controls.Docking;

/// <summary>
/// Saving and restoring the layout of <see cref="ExDockManager" />: zones and sizes, tabs and the active tab, collapsed,
/// hidden and floating panels survive the round trip; panels missing in the layout stay, unknown keys are ignored and an
/// invalid layout changes nothing.
/// </summary>
[TestClass]
public class ExDockManagerLayoutTests
{
    private static readonly string[] Keys = ["tree", "analysis", "output", "props", "log", "find"];

    /// <summary>
    /// Spravca s panelmi v predvolenom rozlozeni (ako po prvom spusteni programu).
    /// </summary>
    private static (ExDockManager Manager, Dictionary<string, ExDockPanel> Panels) Create()
    {
        var manager = new ExDockManager { Size = new Size(1000, 700) };
        var panels = Keys.ToDictionary(k => k, k => new ExDockPanel(k, k.ToUpperInvariant()));
        manager.AddPanel(panels["tree"], ExDockSide.Left);
        manager.AddPanel(panels["analysis"], ExDockSide.Bottom);
        manager.AddPanel(panels["output"], panels["analysis"]);
        manager.AddPanel(panels["props"], ExDockSide.Right);
        manager.AddPanel(panels["log"], ExDockSide.Bottom);
        manager.AddPanel(panels["find"], ExDockSide.Right);
        return (manager, panels);
    }

    [TestMethod]
    public void SaveLayout_LoadLayout_ObnoviRozlozenie()
    {
        var (source, a) = Create();
        using var sourceManager = source;
        source.SetZoneSize(ExDockSide.Left, 320);
        source.DockPanel(a["output"], ExDockSide.Left);
        source.DockPanel(a["analysis"], a["log"]);
        source.DockPanel(a["props"], a["output"], ExDockSide.Right);
        source.Drop(a["log"].Group!, null, new ExControls.Docking.DockDropTarget(ExControls.Docking.DockDropKind.Document, ExDockSide.Bottom),
            Rectangle.Empty);
        source.SetAutoHide(a["props"], true);
        source.HidePanel(a["tree"]);
        source.FloatPanel(a["find"]);
        var layout = source.SaveLayout();

        var (target, b) = Create();
        using var targetManager = target;
        target.LoadLayout(layout);

        Assert.AreEqual(layout, target.SaveLayout());
        foreach (var key in Keys)
        {
            Assert.AreEqual(a[key].State, b[key].State, key);
            Assert.AreEqual(a[key].Side, b[key].Side, key);
        }

        Assert.AreEqual(320, target.GetZoneSize(ExDockSide.Left));
        Assert.AreEqual(b["log"].Group, b["analysis"].Group);
        Assert.AreEqual(b["analysis"], b["log"].Group!.Active);
        Assert.AreEqual(source.Document.Bounds, target.Document.Bounds);
    }

    [TestMethod]
    public void LoadLayout_ChybajuciPanelOstaneNeznamyKlucSaIgnoruje()
    {
        var (manager, panels) = Create();
        using var _ = manager;

        manager.LoadLayout("<dockLayout version=\"1\"><zone side=\"top\" size=\"120\"><group><panel key=\"analysis\" /><panel key=\"removed\" /></group></zone></dockLayout>");

        Assert.AreEqual(ExDockSide.Top, panels["analysis"].Side);
        Assert.AreEqual(ExDockState.Docked, panels["output"].State);
        Assert.AreEqual(ExDockSide.Bottom, panels["output"].Side);
        Assert.AreEqual(ExDockSide.Left, panels["tree"].Side);
        Assert.AreEqual(120, manager.GetZoneSize(ExDockSide.Top));
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("  ")]
    public void LoadLayout_Prazdny_NemeniNic(string? layout)
    {
        var (manager, _) = Create();
        using var _1 = manager;
        var before = manager.SaveLayout();

        manager.LoadLayout(layout);

        Assert.AreEqual(before, manager.SaveLayout());
    }

    [TestMethod]
    public void LoadLayout_Chybny_FormatExceptionRozlozenieOstane()
    {
        var (manager, _) = Create();
        using var _1 = manager;
        var before = manager.SaveLayout();

        Assert.ThrowsExactly<FormatException>(() => manager.LoadLayout("<dockLayout version=\"1\"><zone side=\"nowhere\" size=\"1\" /></dockLayout>"));

        Assert.AreEqual(before, manager.SaveLayout());
    }

    [TestMethod]
    public void SaveLayout_RozmeryVLogickychBodoch()
    {
        var (manager, _) = Create();
        using var _1 = manager;
        manager.SetZoneSize(ExDockSide.Bottom, 175);

        var layout = manager.SaveLayout();

        StringAssert.Contains(layout, "<zone side=\"bottom\" size=\"175\">");
    }
}
