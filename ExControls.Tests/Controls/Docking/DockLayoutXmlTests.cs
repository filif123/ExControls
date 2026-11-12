using System.Globalization;
using ExControls.Docking;

namespace ExControls.Tests.Controls.Docking;

/// <summary>
/// Saved dock layout: single-line XML with fixed keys, default values omitted, invariant numbers; anything else than a
/// layout of this version is a <see cref="FormatException" />.
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
        bottom.Groups.Add(group);
        var collapsed = new DockLayoutData.GroupData { AutoHide = true, AutoHideSize = 260, Active = "find" };
        collapsed.Panels.Add(new DockLayoutData.PanelData("find", false));
        bottom.Groups.Add(collapsed);
        data.Zones.Add(bottom);
        data.Zones.Add(new DockLayoutData.ZoneData { Side = ExDockSide.Left, Size = 250 });

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
        Assert.AreEqual(0.35, parsed.Zones[0].Groups[0].Weight);
        Assert.IsTrue(parsed.Zones[0].Groups[0].Panels[2].Hidden);
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
        zone.Groups.Add(group);
        data.Zones.Add(zone);

        var text = DockLayoutXml.Write(data);

        Assert.AreEqual("<dockLayout version=\"1\"><zone side=\"bottom\" size=\"200\"><group active=\"analysis\"><panel key=\"analysis\" /></group></zone></dockLayout>", text);
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
            Assert.AreEqual(0.35, DockLayoutXml.Parse(text).Zones[0].Groups[0].Weight);
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
    [DataRow("<dockLayout version=\"2\" />")]
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
    public void Parse_NeznamePrvky_Preskoci()
    {
        var data = DockLayoutXml.Parse("<dockLayout version=\"1\"><future /><zone side=\"top\" size=\"90\" /></dockLayout>");

        Assert.AreEqual(1, data.Zones.Count);
        Assert.AreEqual(ExDockSide.Top, data.Zones[0].Side);
        Assert.AreEqual(90, data.Zones[0].Size);
    }
}
