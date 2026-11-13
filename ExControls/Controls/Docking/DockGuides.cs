namespace ExControls.Docking;

/// <summary>
/// Poloha navadzacich znaciek (bez prvkov, testovatelne): tlacidla pri okrajoch oblasti spravcu a kompas (krizik ako
/// vo Visual Studiu) nad skupinou alebo dokumentom pod kurzorom; kazde tlacidlo lezi na podlozke.
/// </summary>
internal static class DockGuides
{
    /// <summary>
    /// Strany kompasu.
    /// </summary>
    public static readonly ExDockSide[] Sides = [ExDockSide.Left, ExDockSide.Top, ExDockSide.Right, ExDockSide.Bottom];

    /// <summary>
    /// Vonkajsia znacka strany: stvorec v strede okraja oblasti, odsadeny dovnutra.
    /// </summary>
    public static Rectangle Outer(Rectangle area, ExDockSide side, int size, int margin) => side switch
    {
        ExDockSide.Left => new Rectangle(area.X + margin, area.Y + (area.Height - size) / 2, size, size),
        ExDockSide.Right => new Rectangle(area.Right - margin - size, area.Y + (area.Height - size) / 2, size, size),
        ExDockSide.Top => new Rectangle(area.X + (area.Width - size) / 2, area.Y + margin, size, size),
        _ => new Rectangle(area.X + (area.Width - size) / 2, area.Bottom - margin - size, size, size)
    };

    /// <summary>
    /// Stred kompasu nad cielom.
    /// </summary>
    public static Rectangle Center(Rectangle target, int size) =>
        new(target.X + (target.Width - size) / 2, target.Y + (target.Height - size) / 2, size, size);

    /// <summary>
    /// Sipka kompasu vedla stredu.
    /// </summary>
    public static Rectangle Arrow(Rectangle center, ExDockSide side, int gap) => side switch
    {
        ExDockSide.Left => new Rectangle(center.X - gap - center.Width, center.Y, center.Width, center.Height),
        ExDockSide.Right => new Rectangle(center.Right + gap, center.Y, center.Width, center.Height),
        ExDockSide.Top => new Rectangle(center.X, center.Y - gap - center.Height, center.Width, center.Height),
        _ => new Rectangle(center.X, center.Bottom + gap, center.Width, center.Height)
    };

    /// <summary>
    /// Podlozka jedneho tlacidla (stvorec okolo neho).
    /// </summary>
    public static Point[] Plate(Rectangle button, int pad)
    {
        var r = Rectangle.Inflate(button, pad, pad);
        return [new Point(r.X, r.Y), new Point(r.Right, r.Y), new Point(r.Right, r.Bottom), new Point(r.X, r.Bottom)];
    }

    /// <summary>
    /// Obdlznik okolo podlozky.
    /// </summary>
    public static Rectangle BoundsOf(Point[] plate)
    {
        var x = plate.Min(p => p.X);
        var y = plate.Min(p => p.Y);
        return new Rectangle(x, y, plate.Max(p => p.X) - x, plate.Max(p => p.Y) - y);
    }

    /// <summary>
    /// Podlozka kompasu: krizik okolo stredu a styroch sipok so skosenymi vnutornymi rohmi.
    /// </summary>
    /// <param name="center">stred kompasu</param>
    /// <param name="gap">medzera medzi stredom a sipkou</param>
    /// <param name="pad">okraj podlozky okolo tlacidiel</param>
    public static Point[] CompassPlate(Rectangle center, int gap, int pad)
    {
        int x0 = center.X - pad, x1 = center.Right + pad, y0 = center.Y - pad, y1 = center.Bottom + pad;
        var arm = gap + center.Width;
        int top = y0 - arm, bottom = y1 + arm, left = x0 - arm, right = x1 + arm;
        var c = Math.Max(1, gap / 2);
        return
        [
            new Point(x0, top), new Point(x1, top), new Point(x1, y0 - c), new Point(x1 + c, y0),
            new Point(right, y0), new Point(right, y1), new Point(x1 + c, y1), new Point(x1, y1 + c),
            new Point(x1, bottom), new Point(x0, bottom), new Point(x0, y1 + c), new Point(x0 - c, y1),
            new Point(left, y1), new Point(left, y0), new Point(x0 - c, y0), new Point(x0, y0 - c)
        ];
    }

    /// <summary>
    /// Znacka pri okraji spravcu a jej podlozka.
    /// </summary>
    /// <param name="Side">okraj</param>
    /// <param name="Plate">obdlznik podlozky</param>
    internal readonly record struct OuterGuide(ExDockSide Side, Rectangle Plate);

    /// <summary>
    /// Posun kompasu, aby neprekryl znacky pri okrajoch: od kazdej prekryvajucej sa znacky smerom dovnutra.
    /// </summary>
    /// <param name="compass">obdlznik podlozky kompasu</param>
    /// <param name="outer">znacky pri okrajoch</param>
    /// <param name="gap">medzera medzi kompasom a znackou</param>
    public static Point AvoidOuter(Rectangle compass, IReadOnlyList<OuterGuide> outer, int gap)
    {
        ArgumentNullException.ThrowIfNull(outer);
        var bounds = compass;
        foreach (var guide in outer)
        {
            if (!bounds.IntersectsWith(guide.Plate))
                continue;
            var plate = guide.Plate;
            switch (guide.Side)
            {
                case ExDockSide.Left:
                    bounds.X = plate.Right + gap;
                    break;
                case ExDockSide.Right:
                    bounds.X = plate.Left - gap - bounds.Width;
                    break;
                case ExDockSide.Top:
                    bounds.Y = plate.Bottom + gap;
                    break;
                default:
                    bounds.Y = plate.Top - gap - bounds.Height;
                    break;
            }
        }

        return new Point(bounds.X - compass.X, bounds.Y - compass.Y);
    }

    /// <summary>
    /// Znacka pod bodom; neskor pridane (kompas) maju prednost.
    /// </summary>
    public static DockGuide? HitTest(IReadOnlyList<DockGuide> guides, Point point)
    {
        ArgumentNullException.ThrowIfNull(guides);
        for (var i = guides.Count - 1; i >= 0; i--)
            if (guides[i].Bounds.Contains(point))
                return guides[i];
        return null;
    }

    /// <summary>
    /// Nahlad ciela v skupine: cela skupina (karta) alebo polovica na strane sipky.
    /// </summary>
    public static Rectangle Preview(Rectangle group, DockDropTarget target)
    {
        ArgumentNullException.ThrowIfNull(target);
        if (target.Kind != DockDropKind.Split)
            return group;
        return target.Side switch
        {
            ExDockSide.Left => new Rectangle(group.X, group.Y, group.Width / 2, group.Height),
            ExDockSide.Right => new Rectangle(group.X + group.Width / 2, group.Y, group.Width - group.Width / 2, group.Height),
            ExDockSide.Top => new Rectangle(group.X, group.Y, group.Width, group.Height / 2),
            _ => new Rectangle(group.X, group.Y + group.Height / 2, group.Width, group.Height - group.Height / 2)
        };
    }
}
