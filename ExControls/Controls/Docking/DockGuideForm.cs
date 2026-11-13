using System.Drawing.Drawing2D;

namespace ExControls.Docking;

/// <summary>
/// Navadzacie znacky pri tahani panela: jedno okno cez vsetky znacky, tvar okna je zjednotenie podloziek (kompas,
/// stvorce pri okrajoch).
/// </summary>
internal sealed class DockGuideForm : DockOverlayForm
{
    private DockGuideSet _set = new();
    private DockGuide? _hover;

    /// <param name="colors">farby ramu spravcu</param>
    public DockGuideForm(DockColors colors) : base(colors) => BackColor = colors.Back;

    /// <summary>
    /// Zobrazi znacky (na obrazovke) a zvyrazni znacku pod mysou.
    /// </summary>
    public void SetGuides(DockGuideSet set, DockGuide? hover)
    {
        var same = set.Plates.Count == _set.Plates.Count && set.Plates.Zip(_set.Plates, (a, b) => a.SequenceEqual(b)).All(x => x);
        _set = set;
        _hover = hover;
        if (set.Plates.Count == 0)
        {
            ShowAt(Rectangle.Empty);
            return;
        }

        // o bod vacsie, aby obrys podlozky na pravom a dolnom okraji nebol orezany
        var bounds = set.Plates.Select(DockGuides.BoundsOf).Aggregate(Rectangle.Union);
        bounds.Width++;
        bounds.Height++;
        if (!same || Bounds != bounds)
        {
            using var path = new GraphicsPath(FillMode.Winding);
            foreach (var plate in set.Plates)
                path.AddPolygon(plate.Select(p => new Point(p.X - bounds.X, p.Y - bounds.Y)).ToArray());
            using var widened = (GraphicsPath)path.Clone();
            using (var pen = new Pen(Color.Black, 2))
                widened.Widen(pen);
            var region = new Region(path);
            region.Union(widened);
            var old = Region;
            Region = region;
            old?.Dispose();
        }

        ShowAt(bounds);
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            var region = Region;
            Region = null;
            region?.Dispose();
        }

        base.Dispose(disposing);
    }

    /// <inheritdoc />
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var origin = Location;
        foreach (var plate in _set.Plates)
            DockPainter.DrawPlate(e.Graphics, plate.Select(p => new Point(p.X - origin.X, p.Y - origin.Y)).ToArray(), Colors);
        foreach (var guide in _set.Guides)
        {
            var bounds = new Rectangle(guide.Bounds.X - origin.X, guide.Bounds.Y - origin.Y, guide.Bounds.Width, guide.Bounds.Height);
            DockPainter.DrawGuide(e.Graphics, bounds, guide.Glyph, Colors, guide == _hover);
        }
    }
}
