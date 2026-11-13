using ExControls.Docking;

namespace ExControls.Tests.Controls.Docking;

/// <summary>
/// Layout of the dock manager without controls: auto-hide strips at the edges, left and right zones over the full height,
/// top and bottom zones between them, nested rows and columns of the zone tree by weights and the document in the rest.
/// </summary>
[TestClass]
public class DockLayoutEngineTests
{
    private static DockLayoutInput Input(int width = 800, int height = 600) => new()
    {
        Client = new Rectangle(0, 0, width, height),
        Splitter = 4,
        MinDocument = 60,
        MinZone = 40,
        Visible = _ => true
    };

    /// <summary>
    /// Koren zony strany so skupinami.
    /// </summary>
    private static DockSplit Zone(DockLayoutInput input, ExDockSide side, int size, params DockNode[] nodes)
    {
        var root = new DockSplit(side is ExDockSide.Top or ExDockSide.Bottom);
        foreach (var node in nodes)
            root.Add(node);
        input.Roots[(int)side] = root;
        input.ZoneSizes[(int)side] = size;
        return root;
    }

    private static DockSplit Split(bool sideBySide, params DockNode[] nodes)
    {
        var split = new DockSplit(sideBySide);
        foreach (var node in nodes)
            split.Add(node);
        return split;
    }

    [TestMethod]
    public void Compute_BezZon_DokumentZaberaCeluOblast()
    {
        var input = Input();
        Zone(input, ExDockSide.Left, 200);

        var result = DockLayoutEngine.Compute(input);

        Assert.AreEqual(new Rectangle(0, 0, 800, 600), result.Document);
        Assert.AreEqual(new Rectangle(0, 0, 800, 600), result.Inner);
        Assert.IsTrue(result.Zones.All(z => z.IsEmpty));
        Assert.AreEqual(0, result.Splitters.Count);
    }

    [TestMethod]
    public void Compute_ZonaDole_DokumentNadNou()
    {
        var input = Input();
        var group = new DockGroup();
        Zone(input, ExDockSide.Bottom, 200, group);

        var result = DockLayoutEngine.Compute(input);

        Assert.AreEqual(new Rectangle(0, 400, 800, 200), result.Zones[(int)ExDockSide.Bottom]);
        Assert.AreEqual(new Rectangle(0, 396, 800, 4), result.ZoneSplitter(ExDockSide.Bottom));
        Assert.AreEqual(new Rectangle(0, 0, 800, 396), result.Document);
        Assert.AreEqual(new Rectangle(0, 400, 800, 200), result.BoundsOf(group));
    }

    [TestMethod]
    public void Compute_ZonyVlavoAVpravoCezCeluVysku_DoleMedziNimi()
    {
        var input = Input();
        Zone(input, ExDockSide.Left, 200, new DockGroup());
        Zone(input, ExDockSide.Right, 150, new DockGroup());
        Zone(input, ExDockSide.Bottom, 100, new DockGroup());

        var result = DockLayoutEngine.Compute(input);

        Assert.AreEqual(new Rectangle(0, 0, 200, 600), result.Zones[(int)ExDockSide.Left]);
        Assert.AreEqual(new Rectangle(200, 0, 4, 600), result.ZoneSplitter(ExDockSide.Left));
        Assert.AreEqual(new Rectangle(650, 0, 150, 600), result.Zones[(int)ExDockSide.Right]);
        Assert.AreEqual(new Rectangle(646, 0, 4, 600), result.ZoneSplitter(ExDockSide.Right));
        Assert.AreEqual(new Rectangle(204, 500, 442, 100), result.Zones[(int)ExDockSide.Bottom]);
        Assert.AreEqual(new Rectangle(204, 0, 442, 496), result.Document);
    }

    [TestMethod]
    public void Compute_VelkaZona_ZuziSaAbyOstalDokument()
    {
        var input = Input(300, 200);
        Zone(input, ExDockSide.Left, 500, new DockGroup());

        var result = DockLayoutEngine.Compute(input);

        Assert.AreEqual(236, result.Zones[(int)ExDockSide.Left].Width);
        Assert.AreEqual(60, result.Document.Width);
    }

    [TestMethod]
    public void Compute_ZonaSaNezmesti_NezobraziSa()
    {
        var input = Input(50, 200);
        var group = new DockGroup();
        Zone(input, ExDockSide.Left, 100, group);

        var result = DockLayoutEngine.Compute(input);

        Assert.IsTrue(result.Zones[(int)ExDockSide.Left].IsEmpty);
        Assert.IsTrue(result.BoundsOf(group).IsEmpty);
        Assert.AreEqual(new Rectangle(0, 0, 50, 200), result.Document);
    }

    [TestMethod]
    public void Compute_MalaZona_AspoNajmensiaHrubka()
    {
        var input = Input();
        Zone(input, ExDockSide.Top, 5, new DockGroup());

        var result = DockLayoutEngine.Compute(input);

        Assert.AreEqual(new Rectangle(0, 0, 800, 40), result.Zones[(int)ExDockSide.Top]);
    }

    [TestMethod]
    public void Compute_SkupinyZonyVlavoPodSebouPodlaVah()
    {
        var input = Input();
        var first = new DockGroup();
        var second = new DockGroup { Weight = 3 };
        var root = Zone(input, ExDockSide.Left, 200, first, second);

        var result = DockLayoutEngine.Compute(input);

        // 600 - 4 (rozdelovac) = 596, prva skupina stvrtinu
        Assert.AreEqual(new Rectangle(0, 0, 200, 149), result.BoundsOf(first));
        Assert.AreEqual(new Rectangle(0, 153, 200, 447), result.BoundsOf(second));
        var splitter = result.Splitters.Single(s => !s.IsZone);
        Assert.AreEqual(new Rectangle(0, 149, 200, 4), splitter.Bounds);
        Assert.AreEqual(root, splitter.Split);
        Assert.AreEqual(first, splitter.First);
        Assert.AreEqual(second, splitter.Second);
        Assert.IsFalse(splitter.IsVertical);
    }

    [TestMethod]
    public void Compute_VnoreneStlpceVZoneDole()
    {
        var input = Input();
        var analysis = new DockGroup();
        var output = new DockGroup();
        var log = new DockGroup();
        // dole: stlpec kontrola | stlpec (vystup nad logom)
        Zone(input, ExDockSide.Bottom, 204, analysis, Split(false, output, log));

        var result = DockLayoutEngine.Compute(input);

        Assert.AreEqual(new Rectangle(0, 396, 398, 204), result.BoundsOf(analysis));
        Assert.AreEqual(new Rectangle(402, 396, 398, 100), result.BoundsOf(output));
        Assert.AreEqual(new Rectangle(402, 500, 398, 100), result.BoundsOf(log));
        Assert.AreEqual(3, result.Splitters.Count, "zone, between the columns, between output and log");
        Assert.IsTrue(result.HitSplitter(new Point(400, 450))!.IsVertical);
        Assert.AreEqual(output, result.HitSplitter(new Point(600, 498))!.First);
    }

    [TestMethod]
    public void Compute_StredovyStrom_SkupinaVedlaDokumentuMedziZonami()
    {
        var input = Input();
        Zone(input, ExDockSide.Bottom, 196, new DockGroup());
        var document = new DockDocumentNode { Weight = 0.7 };
        var beside = new DockGroup { Weight = 0.3 };
        input.Center = Split(true, document, beside);
        input.Document = document;

        var result = DockLayoutEngine.Compute(input);

        // medzi zonami 800 x 400, dokument 0.7 z 796
        Assert.AreEqual(new Rectangle(0, 0, 557, 400), result.Document);
        Assert.AreEqual(new Rectangle(561, 0, 239, 400), result.BoundsOf(beside));
        Assert.IsTrue(result.HitSplitter(new Point(559, 200))!.IsVertical);
    }

    [TestMethod]
    public void Compute_NeviditelnaSkupina_SurodenciSiRozdeliaMiesto()
    {
        var input = Input();
        var hidden = new DockGroup();
        var shown = new DockGroup();
        input.Visible = g => g != hidden;
        Zone(input, ExDockSide.Bottom, 200, hidden, Split(false, shown));

        var result = DockLayoutEngine.Compute(input);

        Assert.IsTrue(result.BoundsOf(hidden).IsEmpty);
        Assert.AreEqual(new Rectangle(0, 400, 800, 200), result.BoundsOf(shown));
        Assert.AreEqual(1, result.Splitters.Count);
    }

    [TestMethod]
    public void Compute_ListyAutoHide_VlavoCezCeluVyskuDoleMedziNimi()
    {
        var input = Input();
        input.Strips[(int)ExDockSide.Left] = 24;
        input.Strips[(int)ExDockSide.Bottom] = 24;
        Zone(input, ExDockSide.Bottom, 100, new DockGroup());

        var result = DockLayoutEngine.Compute(input);

        Assert.AreEqual(new Rectangle(0, 0, 24, 600), result.Strips[(int)ExDockSide.Left]);
        Assert.AreEqual(new Rectangle(24, 576, 776, 24), result.Strips[(int)ExDockSide.Bottom]);
        Assert.AreEqual(new Rectangle(24, 0, 776, 576), result.Inner);
        Assert.AreEqual(new Rectangle(24, 476, 776, 100), result.Zones[(int)ExDockSide.Bottom]);
    }

    [TestMethod]
    public void HitSplitter_NajdeRozdelovacZonyASkupin()
    {
        var input = Input();
        Zone(input, ExDockSide.Left, 200, new DockGroup(), new DockGroup());
        var result = DockLayoutEngine.Compute(input);

        var zone = result.HitSplitter(new Point(202, 300));
        var groups = result.HitSplitter(new Point(100, 299));

        Assert.IsTrue(zone!.IsZone && zone.IsVertical);
        Assert.AreEqual(ExDockSide.Left, zone.Side);
        Assert.IsFalse(groups!.IsZone || groups.IsVertical);
        Assert.IsNull(result.HitSplitter(new Point(400, 300)));
    }
}
