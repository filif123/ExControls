using ExControls.Docking;

namespace ExControls.Tests.Controls.Docking;

/// <summary>
/// Operations of <see cref="ExDockManager" /> on controls that are never shown: docking to a side and as a tab, hiding and
/// showing on the same place, auto-hide strip and slide-out, floating and docking back, moving by the docking guides,
/// layout of the zones and the group view, theme and disposing.
/// </summary>
[TestClass]
public class ExDockManagerTests
{
    private static ExDockManager CreateManager() => new() { Size = new Size(800, 600) };

    private static DockGroupView ViewOf(ExDockPanel panel) => (DockGroupView)panel.Parent!;

    [TestMethod]
    public void AddPanel_UkotviPanelNaStranuAZobrazi()
    {
        using var manager = CreateManager();
        var panel = new ExDockPanel("analysis", "Kontrola");

        manager.AddPanel(panel, ExDockSide.Bottom);

        Assert.AreEqual(manager, panel.Manager);
        Assert.AreEqual(ExDockState.Docked, panel.State);
        Assert.AreEqual(ExDockSide.Bottom, panel.Side);
        Assert.IsTrue(panel.IsShown);
        CollectionAssert.AreEqual(new[] { panel }, manager.Panels.ToList());
        Assert.AreEqual(manager, ViewOf(panel).Parent);
    }

    [TestMethod]
    public void AddPanel_ChybnyPanel_ArgumentException()
    {
        using var manager = CreateManager();
        using var other = CreateManager();
        manager.AddPanel(new ExDockPanel("a", "A"), ExDockSide.Left);
        var added = new ExDockPanel("b", "B");
        other.AddPanel(added, ExDockSide.Left);

        Assert.ThrowsExactly<ArgumentException>(() => manager.AddPanel(new ExDockPanel("a", "A2"), ExDockSide.Left));
        Assert.ThrowsExactly<ArgumentException>(() => manager.AddPanel(new ExDockPanel(), ExDockSide.Left));
        Assert.ThrowsExactly<ArgumentException>(() => manager.AddPanel(added, ExDockSide.Left));
        Assert.ThrowsExactly<ArgumentException>(() => manager.HidePanel(added));
        Assert.ThrowsExactly<InvalidOperationException>(() => added.Key = "c");
    }

    [TestMethod]
    public void AddPanel_KartaKSkupine_AktivnyPanelOstava()
    {
        using var manager = CreateManager();
        var analysis = new ExDockPanel("analysis", "Kontrola");
        var output = new ExDockPanel("output", "Vystup");
        manager.AddPanel(analysis, ExDockSide.Bottom);

        manager.AddPanel(output, analysis);

        Assert.AreEqual(ViewOf(analysis), ViewOf(output));
        Assert.IsTrue(analysis.Visible);
        Assert.IsFalse(output.Visible);
        Assert.AreEqual(1, manager.Zones[(int)ExDockSide.Bottom].Groups.Count);
    }

    [TestMethod]
    public void HidePanel_ShowPanel_VratiNaRovnakeMiesto()
    {
        using var manager = CreateManager();
        var analysis = new ExDockPanel("analysis", "Kontrola");
        var output = new ExDockPanel("output", "Vystup");
        var tree = new ExDockPanel("tree", "Strom");
        manager.AddPanel(analysis, ExDockSide.Bottom);
        manager.AddPanel(output, analysis);
        manager.AddPanel(tree, ExDockSide.Bottom);

        manager.HidePanel(analysis);

        Assert.AreEqual(ExDockState.Hidden, analysis.State);
        Assert.IsNull(analysis.Parent);
        Assert.IsTrue(output.Visible);

        manager.ShowPanel(analysis);

        Assert.AreEqual(ExDockState.Docked, analysis.State);
        Assert.AreEqual(ViewOf(output), ViewOf(analysis));
        Assert.AreEqual(analysis, manager.Zones[(int)ExDockSide.Bottom].Groups[0].Active);
        Assert.AreEqual(2, manager.Zones[(int)ExDockSide.Bottom].Groups.Count);
    }

    [TestMethod]
    public void HidePanel_PoslednyVZone_DokumentZaberieCeluPlochu()
    {
        using var manager = CreateManager();
        var analysis = new ExDockPanel("analysis", "Kontrola");
        manager.AddPanel(analysis, ExDockSide.Bottom);
        Assert.AreNotEqual(manager.ClientRectangle, manager.Document.Bounds);

        manager.HidePanel(analysis);

        Assert.AreEqual(manager.ClientRectangle, manager.Document.Bounds);
        Assert.AreEqual(1, manager.Zones[(int)ExDockSide.Bottom].Groups.Count, "the hidden panel keeps its group");
    }

    [TestMethod]
    public void Layout_PanelDole_DokumentNadNimRozmeryPodlaDpi()
    {
        using var manager = CreateManager();
        var analysis = new ExDockPanel("analysis", "Kontrola");
        manager.AddPanel(analysis, ExDockSide.Bottom);
        manager.SetZoneSize(ExDockSide.Bottom, 150);

        var view = ViewOf(analysis);
        var height = manager.LogicalToDeviceUnits(150);

        Assert.AreEqual(new Rectangle(0, 600 - height, 800, height), view.Bounds);
        Assert.AreEqual(new Rectangle(0, 0, 800, 600 - height - manager.SplitterSize), manager.Document.Bounds);
        Assert.AreEqual(view.ContentBounds, analysis.Bounds);
        Assert.AreEqual(manager.CaptionHeight + 1, view.ContentBounds.Y);
        Assert.AreEqual(150, manager.GetZoneSize(ExDockSide.Bottom));
    }

    [TestMethod]
    public void Layout_ViacPanelov_KartyDolePodObsahom()
    {
        using var manager = CreateManager();
        var analysis = new ExDockPanel("analysis", "Kontrola");
        var output = new ExDockPanel("output", "Vystup");
        manager.AddPanel(analysis, ExDockSide.Bottom);
        manager.AddPanel(output, analysis);

        var view = ViewOf(analysis);

        Assert.IsTrue(view.ShowsTabs);
        Assert.AreEqual(2, view.TabBounds.Count);
        Assert.AreEqual(view.ContentBounds.Bottom, view.TabBounds[0].Y);
        Assert.IsTrue(view.TabBounds[0].Right <= view.TabBounds[1].X);
        Assert.AreEqual(DockGroupView.HitKind.Tab, view.HitTest(new Point(view.TabBounds[1].X + 2, view.TabBounds[1].Y + 2)).Kind);
        Assert.AreEqual(output, view.HitTest(new Point(view.TabBounds[1].X + 2, view.TabBounds[1].Y + 2)).Panel);
        Assert.AreEqual(DockGroupView.HitKind.Close, view.HitTest(new Point(view.CaptionBounds.Right - 2, view.CaptionBounds.Y + 2)).Kind);
    }

    [TestMethod]
    public void SetAutoHide_ZbaliNaListuAShowPanelVysunie()
    {
        using var manager = CreateManager();
        var props = new ExDockPanel("props", "Vlastnosti");
        manager.AddPanel(props, ExDockSide.Right);

        manager.SetAutoHide(props, true);

        Assert.AreEqual(ExDockState.AutoHide, props.State);
        Assert.AreEqual(1, manager.StripButtons.Count);
        Assert.AreEqual(ExDockSide.Right, manager.StripButtons[0].Side);
        Assert.AreEqual(800 - manager.StripThickness, manager.Document.Bounds.Width);
        Assert.IsFalse(manager.Controls.OfType<DockGroupView>().Any());

        manager.ShowPanel(props);

        Assert.IsTrue(manager.Popup.Visible);
        Assert.AreEqual(props.Group, manager.Popup.Group);
        Assert.AreEqual(manager.LogicalToDeviceUnits(250), manager.Popup.Width);
        Assert.AreEqual(manager.Document.Bounds.Right, manager.Popup.Right);
        Assert.AreEqual(DockViewMode.AutoHide, ViewOf(props).Mode);

        manager.SetAutoHide(props, false);

        Assert.IsFalse(manager.Popup.Visible);
        Assert.AreEqual(ExDockState.Docked, props.State);
        Assert.AreEqual(manager, ViewOf(props).Parent);
        Assert.AreEqual(0, manager.StripButtons.Count);
    }

    [TestMethod]
    public void FloatPanel_PlavajuciAPotomUkotvenyNaStranu()
    {
        using var manager = CreateManager();
        var analysis = new ExDockPanel("analysis", "Kontrola");
        var output = new ExDockPanel("output", "Vystup");
        manager.AddPanel(analysis, ExDockSide.Bottom);
        manager.AddPanel(output, analysis);

        manager.FloatPanel(output);

        Assert.AreEqual(ExDockState.Floating, output.State);
        Assert.AreEqual(ExDockSide.Bottom, output.Side, "a floating panel returns to its side");
        Assert.AreEqual(ExDockState.Docked, analysis.State);
        Assert.AreEqual(1, manager.FloatingGroups.Count);

        manager.DockPanel(output, ExDockSide.Left);

        Assert.AreEqual(ExDockState.Docked, output.State);
        Assert.AreEqual(ExDockSide.Left, output.Side);
        Assert.AreEqual(0, manager.FloatingGroups.Count);
    }

    [TestMethod]
    public void FloatPanel_Zakazane_InvalidOperationException()
    {
        using var manager = CreateManager();
        var panel = new ExDockPanel("analysis", "Kontrola") { CanFloat = false };
        manager.AddPanel(panel, ExDockSide.Bottom);

        Assert.ThrowsExactly<InvalidOperationException>(() => manager.FloatPanel(panel));
        manager.AllowFloating = false;
        panel.CanFloat = true;
        Assert.ThrowsExactly<InvalidOperationException>(() => manager.FloatPanel(panel));
    }

    [TestMethod]
    public void Drop_KartaDoInejSkupiny_ZdrojovaSkupinaZanikne()
    {
        using var manager = CreateManager();
        var tree = new ExDockPanel("tree", "Strom");
        var analysis = new ExDockPanel("analysis", "Kontrola");
        manager.AddPanel(tree, ExDockSide.Left);
        manager.AddPanel(analysis, ExDockSide.Bottom);

        manager.Drop(tree.Group!, null, new DockDropTarget(DockDropKind.Tab, ExDockSide.Bottom, analysis.Group), Rectangle.Empty);

        Assert.AreEqual(analysis.Group, tree.Group);
        Assert.AreEqual(tree, analysis.Group!.Active);
        Assert.AreEqual(0, manager.Zones[(int)ExDockSide.Left].Groups.Count);
        Assert.AreEqual(ExDockSide.Bottom, tree.Side);
    }

    [TestMethod]
    public void Drop_SipkaPredSkupinou_NovaSkupinaSPolovicouVahy()
    {
        using var manager = CreateManager();
        var analysis = new ExDockPanel("analysis", "Kontrola");
        var output = new ExDockPanel("output", "Vystup");
        manager.AddPanel(analysis, ExDockSide.Bottom);
        manager.AddPanel(output, analysis);
        var target = analysis.Group!;

        manager.Drop(target, output, new DockDropTarget(DockDropKind.Split, ExDockSide.Left, target), Rectangle.Empty);

        var groups = manager.Zones[(int)ExDockSide.Bottom].Groups;
        Assert.AreEqual(2, groups.Count);
        Assert.AreEqual(output.Group, groups[0]);
        Assert.AreEqual(target, groups[1]);
        Assert.AreEqual(0.5, groups[0].Weight);
        Assert.AreEqual(0.5, groups[1].Weight);
        Assert.IsTrue(ViewOf(output).Bounds.Right <= ViewOf(analysis).Bounds.X);
    }

    [TestMethod]
    public void Drop_OkrajSpravcu_StlpecNaVonkajsejStraneZony()
    {
        using var manager = CreateManager();
        var tree = new ExDockPanel("tree", "Strom");
        var search = new ExDockPanel("search", "Hladanie");
        var analysis = new ExDockPanel("analysis", "Kontrola");
        manager.AddPanel(tree, ExDockSide.Left);
        manager.AddPanel(search, ExDockSide.Left);
        manager.AddPanel(analysis, ExDockSide.Bottom);

        manager.Drop(analysis.Group!, null, new DockDropTarget(DockDropKind.Edge, ExDockSide.Left), Rectangle.Empty);

        var groups = manager.Zones[(int)ExDockSide.Left].Groups;
        CollectionAssert.AreEqual(new[] { analysis.Group, tree.Group, search.Group }, groups);
        Assert.AreEqual(manager.ClientRectangle.Height, manager.Document.Bounds.Height);
        var column = ViewOf(analysis).Bounds;
        Assert.AreEqual(0, column.X);
        Assert.AreEqual(600, column.Height, "the new column spans the zone");
        Assert.IsTrue(column.Right < ViewOf(tree).Bounds.X);
        Assert.AreEqual(ViewOf(tree).Bounds.X, ViewOf(search).Bounds.X);
    }

    [TestMethod]
    public void Drop_MimoZnaciek_PlavajuceOkno()
    {
        using var manager = CreateManager();
        var analysis = new ExDockPanel("analysis", "Kontrola");
        manager.AddPanel(analysis, ExDockSide.Bottom);
        var bounds = new Rectangle(100, 100, 300, 200);

        manager.Drop(analysis.Group!, null, DockDropTarget.Floating, bounds);

        Assert.AreEqual(ExDockState.Floating, analysis.State);
        Assert.AreEqual(bounds, analysis.Group!.FloatBounds);
        Assert.AreEqual(manager.ClientRectangle, manager.Document.Bounds);
    }

    [TestMethod]
    public void PanelStateChanged_OhlasiPanelySoZmenenymStavom()
    {
        using var manager = CreateManager();
        var analysis = new ExDockPanel("analysis", "Kontrola");
        var output = new ExDockPanel("output", "Vystup");
        var changes = new List<string>();
        manager.PanelStateChanged += (_, e) => changes.Add(e.Panel.Key + ":" + e.Panel.State);

        manager.AddPanel(analysis, ExDockSide.Bottom);
        manager.AddPanel(output, analysis);
        manager.HidePanel(analysis);
        manager.ShowPanel(analysis, false);
        manager.SetAutoHide(output, true);
        manager.RemovePanel(output);

        CollectionAssert.AreEqual(new[]
        {
            "analysis:Docked", "output:Docked", "analysis:Hidden", "analysis:Docked", "analysis:AutoHide", "output:AutoHide",
            "output:Hidden"
        }, changes);
        Assert.IsNull(output.Manager);
    }

    [TestMethod]
    public void ClosePanelByUser_PanelClosingMozeZrusit()
    {
        using var manager = CreateManager();
        var analysis = new ExDockPanel("analysis", "Kontrola");
        manager.AddPanel(analysis, ExDockSide.Bottom);
        var cancel = true;
        manager.PanelClosing += (_, e) => e.Cancel = cancel;

        manager.ClosePanelByUser(analysis);
        Assert.AreEqual(ExDockState.Docked, analysis.State);

        cancel = false;
        manager.ClosePanelByUser(analysis);
        Assert.AreEqual(ExDockState.Hidden, analysis.State);
    }

    [TestMethod]
    public void ApplyTheme_FarbyAjSkrytymPanelomSystemovyStylIchVrati()
    {
        using var manager = CreateManager();
        var analysis = new ExDockPanel("analysis", "Kontrola");
        manager.AddPanel(analysis, ExDockSide.Bottom);
        manager.HidePanel(analysis);
        var dark = new ExTheme { PanelBackColor = Color.FromArgb(43, 45, 48), PanelForeColor = Color.FromArgb(223, 225, 229) };

        manager.ApplyTheme(dark);

        Assert.AreEqual(dark.PanelBackColor, analysis.BackColor);
        Assert.AreEqual(dark.PanelBackColor, manager.Document.BackColor);
        Assert.AreEqual(dark.PanelBackColor, manager.Colors.Back);

        manager.ApplyTheme(new ExTheme { UseSystemStyle = true });

        Assert.AreEqual(SystemColors.Control, analysis.BackColor);
        Assert.AreEqual(SystemColors.Control, manager.Colors.Back);
    }

    [TestMethod]
    public void AddPanel_PoTeme_PanelDostaneTemu()
    {
        using var manager = CreateManager();
        var theme = new ExTheme { PanelBackColor = Color.Navy };
        manager.ApplyTheme(theme);
        var panel = new ExDockPanel("analysis", "Kontrola");

        manager.AddPanel(panel, ExDockSide.Bottom);

        Assert.AreEqual(Color.Navy, panel.BackColor);
    }

    [TestMethod]
    public void Dispose_ZrusiAjSkryteAZbalenePanely()
    {
        var manager = CreateManager();
        var hidden = new ExDockPanel("hidden", "Skryty");
        var collapsed = new ExDockPanel("collapsed", "Zbaleny");
        var removed = new ExDockPanel("removed", "Odobraty");
        manager.AddPanel(hidden, ExDockSide.Bottom);
        manager.AddPanel(collapsed, ExDockSide.Left);
        manager.AddPanel(removed, ExDockSide.Left);
        manager.HidePanel(hidden);
        manager.SetAutoHide(collapsed, true);
        manager.RemovePanel(removed);

        manager.Dispose();

        Assert.IsTrue(hidden.IsDisposed);
        Assert.IsTrue(collapsed.IsDisposed);
        Assert.IsFalse(removed.IsDisposed);
        removed.Dispose();
    }
}
