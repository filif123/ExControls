namespace ExControls.Docking;

/// <summary>
/// Poloha navadzacich znaciek (bez prvkov, testovatelne): stvorce pri okrajoch oblasti spravcu a kosostvorec nad
/// skupinou pod kurzorom.
/// </summary>
internal static class DockGuides
{
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
    /// Stred kosostvorca nad cielom.
    /// </summary>
    public static Rectangle Center(Rectangle target, int size) =>
        new(target.X + (target.Width - size) / 2, target.Y + (target.Height - size) / 2, size, size);

    /// <summary>
    /// Sipka kosostvorca vedla stredu.
    /// </summary>
    public static Rectangle Arrow(Rectangle center, ExDockSide side, int gap) => side switch
    {
        ExDockSide.Left => new Rectangle(center.X - gap - center.Width, center.Y, center.Width, center.Height),
        ExDockSide.Right => new Rectangle(center.Right + gap, center.Y, center.Width, center.Height),
        ExDockSide.Top => new Rectangle(center.X, center.Y - gap - center.Height, center.Width, center.Height),
        _ => new Rectangle(center.X, center.Bottom + gap, center.Width, center.Height)
    };

    /// <summary>
    /// Smery deliacich sipiek nad skupinou zony: skupiny sa radia len pozdlz zony (vlavo a vpravo pod sebou, hore a dole
    /// vedla seba).
    /// </summary>
    public static ExDockSide[] SplitDirections(ExDockSide zoneSide) =>
        DockLayoutEngine.IsHorizontalSide(zoneSide) ? [ExDockSide.Top, ExDockSide.Bottom] : [ExDockSide.Left, ExDockSide.Right];

    /// <summary>
    /// Znacka pod bodom; neskor pridane (kosostvorec) maju prednost.
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
