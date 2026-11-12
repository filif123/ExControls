using ExControls.Docking;

namespace ExControls.Tests.Controls.Docking;

/// <summary>
/// Layout of the dock manager without controls: auto-hide strips at the edges, left and right zones over the full height,
/// top and bottom zones between them, groups along the zone by weights and the document in the rest.
/// </summary>
[TestClass]
public class DockLayoutEngineTests
{
    private static DockLayoutInput Input(int width = 800, int height = 600) => new()
    {
        Client = new Rectangle(0, 0, width, height),
        Splitter = 4,
        MinDocument = 60,
        MinZone = 40
    };

    [TestMethod]
    public void Compute_BezZon_DokumentZaberaCeluOblast()
    {
        var result = DockLayoutEngine.Compute(Input());

        Assert.AreEqual(new Rectangle(0, 0, 800, 600), result.Document);
        Assert.AreEqual(new Rectangle(0, 0, 800, 600), result.Inner);
        Assert.IsTrue(result.Zones.All(z => z.IsEmpty));
    }

    [TestMethod]
    public void Compute_ZonaDole_DokumentNadNou()
    {
        var input = Input();
        input.ZoneSizes[(int)ExDockSide.Bottom] = 200;
        input.Weights[(int)ExDockSide.Bottom] = [1];

        var result = DockLayoutEngine.Compute(input);

        Assert.AreEqual(new Rectangle(0, 400, 800, 200), result.Zones[(int)ExDockSide.Bottom]);
        Assert.AreEqual(new Rectangle(0, 396, 800, 4), result.ZoneSplitters[(int)ExDockSide.Bottom]);
        Assert.AreEqual(new Rectangle(0, 0, 800, 396), result.Document);
        CollectionAssert.AreEqual(new[] { new Rectangle(0, 400, 800, 200) }, result.Groups[(int)ExDockSide.Bottom]);
    }

    [TestMethod]
    public void Compute_ZonyVlavoAVpravoCezCeluVysku_DoleMedziNimi()
    {
        var input = Input();
        input.ZoneSizes[(int)ExDockSide.Left] = 200;
        input.ZoneSizes[(int)ExDockSide.Right] = 150;
        input.ZoneSizes[(int)ExDockSide.Bottom] = 100;
        input.Weights[(int)ExDockSide.Left] = [1];
        input.Weights[(int)ExDockSide.Right] = [1];
        input.Weights[(int)ExDockSide.Bottom] = [1];

        var result = DockLayoutEngine.Compute(input);

        Assert.AreEqual(new Rectangle(0, 0, 200, 600), result.Zones[(int)ExDockSide.Left]);
        Assert.AreEqual(new Rectangle(200, 0, 4, 600), result.ZoneSplitters[(int)ExDockSide.Left]);
        Assert.AreEqual(new Rectangle(650, 0, 150, 600), result.Zones[(int)ExDockSide.Right]);
        Assert.AreEqual(new Rectangle(646, 0, 4, 600), result.ZoneSplitters[(int)ExDockSide.Right]);
        Assert.AreEqual(new Rectangle(204, 500, 442, 100), result.Zones[(int)ExDockSide.Bottom]);
        Assert.AreEqual(new Rectangle(204, 0, 442, 496), result.Document);
    }

    [TestMethod]
    public void Compute_VelkaZona_ZuziSaAbyOstalDokument()
    {
        var input = Input(300, 200);
        input.ZoneSizes[(int)ExDockSide.Left] = 500;
        input.Weights[(int)ExDockSide.Left] = [1];

        var result = DockLayoutEngine.Compute(input);

        Assert.AreEqual(236, result.Zones[(int)ExDockSide.Left].Width);
        Assert.AreEqual(60, result.Document.Width);
    }

    [TestMethod]
    public void Compute_ZonaSaNezmesti_NezobraziSa()
    {
        var input = Input(50, 200);
        input.ZoneSizes[(int)ExDockSide.Left] = 100;
        input.Weights[(int)ExDockSide.Left] = [1];

        var result = DockLayoutEngine.Compute(input);

        Assert.IsTrue(result.Zones[(int)ExDockSide.Left].IsEmpty);
        Assert.AreEqual(0, result.Groups[(int)ExDockSide.Left].Length);
        Assert.AreEqual(new Rectangle(0, 0, 50, 200), result.Document);
    }

    [TestMethod]
    public void Compute_MalaZona_AspoNajmensiaHrubka()
    {
        var input = Input();
        input.ZoneSizes[(int)ExDockSide.Top] = 5;
        input.Weights[(int)ExDockSide.Top] = [1];

        var result = DockLayoutEngine.Compute(input);

        Assert.AreEqual(new Rectangle(0, 0, 800, 40), result.Zones[(int)ExDockSide.Top]);
    }

    [TestMethod]
    public void Compute_SkupinyZonyVlavoPodSebouPodlaVah()
    {
        var input = Input();
        input.ZoneSizes[(int)ExDockSide.Left] = 200;
        input.Weights[(int)ExDockSide.Left] = [1, 3];

        var result = DockLayoutEngine.Compute(input);

        // 600 - 4 (rozdelovac) = 596, prva skupina stvrtinu
        CollectionAssert.AreEqual(new[] { new Rectangle(0, 0, 200, 149), new Rectangle(0, 153, 200, 447) }, result.Groups[(int)ExDockSide.Left]);
        CollectionAssert.AreEqual(new[] { new Rectangle(0, 149, 200, 4) }, result.GroupSplitters[(int)ExDockSide.Left]);
    }

    [TestMethod]
    public void Compute_SkupinyZonyDoleVedlaSeba()
    {
        var input = Input();
        input.ZoneSizes[(int)ExDockSide.Bottom] = 200;
        input.Weights[(int)ExDockSide.Bottom] = [1, 1];

        var result = DockLayoutEngine.Compute(input);

        CollectionAssert.AreEqual(new[] { new Rectangle(0, 400, 398, 200), new Rectangle(402, 400, 398, 200) }, result.Groups[(int)ExDockSide.Bottom]);
    }

    [TestMethod]
    public void Compute_ListyAutoHide_VlavoCezCeluVyskuDoleMedziNimi()
    {
        var input = Input();
        input.Strips[(int)ExDockSide.Left] = 24;
        input.Strips[(int)ExDockSide.Bottom] = 24;
        input.ZoneSizes[(int)ExDockSide.Bottom] = 100;
        input.Weights[(int)ExDockSide.Bottom] = [1];

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
        input.ZoneSizes[(int)ExDockSide.Left] = 200;
        input.Weights[(int)ExDockSide.Left] = [1, 1];
        var result = DockLayoutEngine.Compute(input);

        var zone = result.HitSplitter(new Point(202, 300));
        var groups = result.HitSplitter(new Point(100, 299));

        Assert.AreEqual(new DockSplitterHit(ExDockSide.Left, -1), zone);
        Assert.IsTrue(zone!.Value.IsZone && zone.Value.IsVertical);
        Assert.AreEqual(new DockSplitterHit(ExDockSide.Left, 0), groups);
        Assert.IsFalse(groups!.Value.IsVertical);
        Assert.IsNull(result.HitSplitter(new Point(400, 300)));
    }
}
