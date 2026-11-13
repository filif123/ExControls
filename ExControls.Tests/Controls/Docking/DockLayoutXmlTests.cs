using System.Globalization;
using ExControls.Docking;

namespace ExControls.Tests.Controls.Docking;

/// <summary>
/// Saved dock layout: single-line XML with fixed keys, nested splits, default values omitted, invariant numbers; version 1
/// (without splits) is still read, anything else is a <see cref="FormatException" />.
/// </summary>
[TestClass]
public class DockLayoutXmlTests
{
    private static DockLayoutData Sample()
    {
        var data = new DockLayoutData();
        var bottom = new DockLayoutData.ZoneData { Side = ExDockSide.Bottom, Size = 180 };
        var group = new DockLayoutData.GroupData { Weight = 0.35, Active = "output" };
        group.Panels.Add(new DockLayoutData.PanelData("analysis", false));
        group.Panels.Add(new DockLayoutData.PanelData("output", false));
        group.Panels.Add(new DockLayoutData.PanelData("log", true));
        var nested = new DockLayoutData.SplitData { SideBySide = false, Weight = 2 };
        nested.Children.Add(group);
        var log = new DockLayoutData.GroupData();
        log.Panels.Add(new DockLayoutData.PanelData("errors", false));
        nested.Children.Add(log);
        bottom.Nodes.Add(nested);
        var collapsed = new DockLayoutData.GroupData { AutoHide = true, AutoHideSize = 260, Active = "find" };
        collapsed.Panels.Add(new DockLayoutData.PanelData("find", false));
        bottom.Nodes.Add(collapsed);
        data.Zones.Add(bottom);
        data.Zones.Add(new DockLayoutData.ZoneData { Side = ExDockSide.Left, Size = 250 });
        var byDocument = new DockLayoutData.GroupData { Side = ExDockSide.Right, Weight = 0.3 };
        byDocument.Panels.Add(new DockLayoutData.PanelData("tree", false));
        data.Center = [new DockLayoutData.DocumentData { Weight = 0.7 }, byDocument];

        var window = new DockLayoutData.FloatData { Bounds = new Rectangle(-1200, 40, 300, 200), Side = ExDockSide.Left };
        window.Group.Panels.Add(new DockLayoutData.PanelData("props", false));
        data.Floats.Add(window);
        return data;
    }

    [TestMethod]
    public void Write_Parse_RoundTrip()
    {
        var text = DockLayoutXml.Write(Sample());

        var parsed = DockLayoutXml.Parse(text);

        Assert.AreEqual(text, DockLayoutXml.Write(parsed));
        Assert.AreEqual(2, parsed.Zones.Count);
        var split = (DockLayoutData.SplitData)parsed.Zones[0].Nodes[0];
        Assert.IsFalse(split.SideBySide);
        Assert.AreEqual(2, split.Weight);
        var first = (DockLayoutData.GroupData)split.Children[0];
        Assert.AreEqual(0.35, first.Weight);
        Assert.IsTrue(first.Panels[2].Hidden);
        Assert.AreEqual(0.7, parsed.Center![0].Weight);
        Assert.AreEqual(ExDockSide.Right, ((DockLayoutData.GroupData)parsed.Center[1]).Side);
        CollectionAssert.AreEqual(new[] { "analysis", "output", "log", "errors", "find", "tree", "props" },
            parsed.AllGroups.SelectMany(g => g.Panels).Select(p => p.Key).ToList());
        Assert.AreEqual(new Rectangle(-1200, 40, 300, 200), parsed.Floats[0].Bounds);
        Assert.AreEqual(ExDockSide.Left, parsed.Floats[0].Side);
    }

    [TestMethod]
    public void Write_JedenRiadokBezPredvolenychHodnot()
    {
        var data = new DockLayoutData();
        var zone = new DockLayoutData.ZoneData { Side = ExDockSide.Bottom, Size = 200 };
        var group = new DockLayoutData.GroupData { Active = "analysis" };
        group.Panels.Add(new DockLayoutData.PanelData("analysis", false));
        zone.Nodes.Add(group);
        data.Zones.Add(zone);

        var text = DockLayoutXml.Write(data);

        Assert.AreEqual("<dockLayout version=\"2\"><zone side=\"bottom\" size=\"200\"><group active=\"analysis\"><panel key=\"analysis\" /></group></zone></dockLayout>", text);
    }

    [TestMethod]
    public void Write_CislaNezavisleOdKultury()
    {
        var culture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("sk-SK");
            var text = DockLayoutXml.Write(Sample());

            StringAssert.Contains(text, "weight=\"0.35\"");
            Assert.AreEqual(0.35, DockLayoutXml.Parse(text).AllGroups.First().Weight);
        }
        finally
        {
            CultureInfo.CurrentCulture = culture;
        }
    }

    [TestMethod]
    [DataRow("not xml")]
    [DataRow("<layout version=\"1\" />")]
    [DataRow("<dockLayout />")]
    [DataRow("<dockLayout version=\"3\" />")]
    [DataRow("<dockLayout version=\"2\"><zone side=\"left\" size=\"1\"><document /></zone></dockLayout>")]
    [DataRow("<dockLayout version=\"2\"><center><document /><split arrange=\"rows\"><document /></split></center></dockLayout>")]
    [DataRow("<dockLayout version=\"2\"><center /><center /></dockLayout>")]
    [DataRow("<dockLayout version=\"2\"><zone side=\"left\" size=\"1\"><split arrange=\"diagonal\" /></zone></dockLayout>")]
    [DataRow("<dockLayout version=\"2\"><zone side=\"left\" size=\"1\"><split arrange=\"rows\"><group><panel key=\"a\" /></group></split><group><panel key=\"a\" /></group></zone></dockLayout>")]
    [DataRow("<dockLayout version=\"1\"><zone side=\"middle\" size=\"10\" /></dockLayout>")]
    [DataRow("<dockLayout version=\"1\"><zone side=\"left\" size=\"-1\" /></dockLayout>")]
    [DataRow("<dockLayout version=\"1\"><zone side=\"left\" size=\"x\" /></dockLayout>")]
    [DataRow("<dockLayout version=\"1\"><zone side=\"left\" size=\"1\" /><zone side=\"left\" size=\"2\" /></dockLayout>")]
    [DataRow("<dockLayout version=\"1\"><zone side=\"left\" size=\"1\"><group weight=\"0\"><panel key=\"a\" /></group></zone></dockLayout>")]
    [DataRow("<dockLayout version=\"1\"><zone side=\"left\" size=\"1\"><group autoHide=\"yes\"><panel key=\"a\" /></group></zone></dockLayout>")]
    [DataRow("<dockLayout version=\"1\"><zone side=\"left\" size=\"1\"><group><panel /></group></zone></dockLayout>")]
    [DataRow("<dockLayout version=\"1\"><zone side=\"left\" size=\"1\"><group><panel key=\"a\" /></group></zone><float x=\"0\" y=\"0\" width=\"1\" height=\"1\"><group><panel key=\"a\" /></group></float></dockLayout>")]
    [DataRow("<dockLayout version=\"1\"><float x=\"0\" y=\"0\" width=\"1\" height=\"1\" /></dockLayout>")]
    public void Parse_ChybneRozlozenie_FormatException(string text)
    {
        Assert.ThrowsExactly<FormatException>(() => DockLayoutXml.Parse(text));
    }

    [TestMethod]
    public void Parse_Verzia1BezRozdeleni()
    {
        var data = DockLayoutXml.Parse("<dockLayout version=\"1\"><zone side=\"left\" size=\"250\"><group><panel key=\"tree\" /></group><group weight=\"2\"><panel key=\"props\" /></group></zone></dockLayout>");

        Assert.AreEqual(2, data.Zones[0].Nodes.Count);
        Assert.AreEqual(2, data.Zones[0].Nodes[1].Weight);
        Assert.AreEqual("props", ((DockLayoutData.GroupData)data.Zones[0].Nodes[1]).Panels[0].Key);
    }

    [TestMethod]
    public void Parse_NeznamePrvky_Preskoci()
    {
        var data = DockLayoutXml.Parse("<dockLayout version=\"1\"><future /><zone side=\"top\" size=\"90\" /></dockLayout>");

        Assert.AreEqual(1, data.Zones.Count);
        Assert.AreEqual(ExDockSide.Top, data.Zones[0].Side);
        Assert.AreEqual(90, data.Zones[0].Size);
    }
}
