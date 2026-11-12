namespace ExControls.Docking;

/// <summary>
/// Navadzacie znacky pri tahani panela: jedno okno cez vsetky znacky, tvar okna je zjednotenie znaciek.
/// </summary>
internal sealed class DockGuideForm : DockOverlayForm
{
    private IReadOnlyList<DockGuide> _guides = [];
    private DockGuide? _hover;

    /// <param name="colors">farby ramu spravcu</param>
    public DockGuideForm(DockColors colors) : base(colors) => BackColor = colors.Box;

    /// <summary>
    /// Zobrazi znacky (na obrazovke) a zvyrazni znacku pod mysou.
    /// </summary>
    public void SetGuides(IReadOnlyList<DockGuide> guides, DockGuide? hover)
    {
        var same = guides.Count == _guides.Count && guides.Select(g => g.Bounds).SequenceEqual(_guides.Select(g => g.Bounds));
        _guides = guides;
        _hover = hover;
        if (guides.Count == 0)
        {
            ShowAt(Rectangle.Empty);
            return;
        }

        var bounds = guides.Select(g => g.Bounds).Aggregate(Rectangle.Union);
        if (!same || Bounds != bounds)
        {
            var region = new Region(Rectangle.Empty);
            foreach (var guide in guides)
                region.Union(new Rectangle(guide.Bounds.X - bounds.X, guide.Bounds.Y - bounds.Y, guide.Bounds.Width, guide.Bounds.Height));
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
        foreach (var guide in _guides)
        {
            var bounds = new Rectangle(guide.Bounds.X - origin.X, guide.Bounds.Y - origin.Y, guide.Bounds.Width, guide.Bounds.Height);
            DockPainter.DrawGuide(e.Graphics, bounds, guide.Glyph, Colors, guide == _hover);
        }
    }
}
