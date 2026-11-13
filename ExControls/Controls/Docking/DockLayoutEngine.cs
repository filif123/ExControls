namespace ExControls.Docking;

/// <summary>
/// Vypocet rozlozenia spravcu panelov bez prvkov (testovatelny): listy auto-hide pri okrajoch, zony vlavo a vpravo cez
/// celu vysku, zony hore a dole medzi nimi, v zone strom rozdeleni podla podielov a vo zvysku stredovy strom s dokumentom.
/// </summary>
internal static class DockLayoutEngine
{
    // poradie: vlavo a vpravo su vonkajsie (cez celu vysku), hore a dole medzi nimi - ako Visual Studio
    internal static readonly ExDockSide[] Order = [ExDockSide.Left, ExDockSide.Right, ExDockSide.Top, ExDockSide.Bottom];

    /// <summary>
    /// Vypocita rozlozenie.
    /// </summary>
    public static DockLayoutResult Compute(DockLayoutInput input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var result = new DockLayoutResult();
        var area = input.Client;
        foreach (var side in Order)
        {
            var thickness = Math.Min(input.Strips[(int)side], Thickness(area, side));
            if (thickness > 0)
                result.Strips[(int)side] = Take(ref area, side, thickness);
        }

        result.Inner = area;
        foreach (var side in Order)
        {
            var index = (int)side;
            if (input.Roots[index] is not { } root || !DockTree.IsVisible(root, input.Visible))
                continue;

            // zona sa zuzi, aby ostal dokument; ak sa nezmesti ani s dokumentom najmensim, nezobrazi sa
            var max = Thickness(area, side) - input.MinDocument - input.Splitter;
            if (max <= 0)
                continue;

            var size = Math.Max(Math.Min(input.ZoneSizes[index], max), Math.Min(input.MinZone, max));
            var zone = Take(ref area, side, size);
            result.Zones[index] = zone;
            result.Splitters.Add(new DockSplitterInfo(Take(ref area, side, input.Splitter), side));
            LayoutNode(root, zone, side, input, result);
        }

        // medzi zonami dokument, pripadne rozdeleny so skupinami stredoveho stromu
        if (input.Center is { } center)
        {
            LayoutNode(center, area, ExDockSide.Left, input, result);
            result.Document = input.Document is { } document ? result.BoundsOf(document) : area;
        }
        else
        {
            result.Document = area;
        }

        return result;
    }

    // uzol dostane obdlznik, rozdelenie ho rozdeli medzi viditelne uzly podla podielov
    private static void LayoutNode(DockNode node, Rectangle bounds, ExDockSide side, DockLayoutInput input, DockLayoutResult result)
    {
        result.Nodes[node] = bounds;
        if (node is not DockSplit split)
            return;

        var visible = split.Children.Where(c => DockTree.IsVisible(c, input.Visible)).ToList();
        Split(bounds, split.SideBySide, visible.Select(c => c.Weight).ToArray(), input.Splitter, out var parts, out var gaps);
        for (var i = 0; i < visible.Count; i++)
        {
            if (i < gaps.Length)
                result.Splitters.Add(new DockSplitterInfo(gaps[i], side, split, visible[i], visible[i + 1]));
            LayoutNode(visible[i], parts[i], side, input, result);
        }
    }

    /// <summary>
    /// Hrubka oblasti v smere strany (sirka pre vlavo/vpravo, vyska pre hore/dole).
    /// </summary>
    public static int Thickness(Rectangle area, ExDockSide side) => IsHorizontalSide(side) ? area.Width : area.Height;

    /// <summary>
    /// Zona na strane vlavo alebo vpravo (rozdelovac zony sa taha vodorovne).
    /// </summary>
    public static bool IsHorizontalSide(ExDockSide side) => side is ExDockSide.Left or ExDockSide.Right;

    /// <summary>
    /// Opacna strana.
    /// </summary>
    public static ExDockSide Opposite(ExDockSide side) => side switch
    {
        ExDockSide.Left => ExDockSide.Right,
        ExDockSide.Right => ExDockSide.Left,
        ExDockSide.Top => ExDockSide.Bottom,
        _ => ExDockSide.Top
    };

    /// <summary>
    /// Odoberie z oblasti pas danej hrubky na strane.
    /// </summary>
    public static Rectangle Take(ref Rectangle area, ExDockSide side, int thickness)
    {
        Rectangle part;
        switch (side)
        {
            case ExDockSide.Left:
                part = new Rectangle(area.X, area.Y, thickness, area.Height);
                area = new Rectangle(area.X + thickness, area.Y, area.Width - thickness, area.Height);
                break;
            case ExDockSide.Right:
                part = new Rectangle(area.Right - thickness, area.Y, thickness, area.Height);
                area = new Rectangle(area.X, area.Y, area.Width - thickness, area.Height);
                break;
            case ExDockSide.Top:
                part = new Rectangle(area.X, area.Y, area.Width, thickness);
                area = new Rectangle(area.X, area.Y + thickness, area.Width, area.Height - thickness);
                break;
            default:
                part = new Rectangle(area.X, area.Bottom - thickness, area.Width, thickness);
                area = new Rectangle(area.X, area.Y, area.Width, area.Height - thickness);
                break;
        }

        return part;
    }

    /// <summary>
    /// Rozdeli obdlznik na casti podla vah vedla seba alebo pod sebou; medzi castami rozdelovace.
    /// </summary>
    public static void Split(Rectangle bounds, bool sideBySide, double[] weights, int splitter, out Rectangle[] parts,
        out Rectangle[] splitters)
    {
        ArgumentNullException.ThrowIfNull(weights);

        var count = weights.Length;
        parts = new Rectangle[count];
        splitters = new Rectangle[Math.Max(0, count - 1)];
        var length = sideBySide ? bounds.Width : bounds.Height;
        var total = Math.Max(0, length - (count - 1) * splitter);
        var sum = weights.Sum(w => Math.Max(w, 0));
        double cumulative = 0;
        var start = 0;
        for (var i = 0; i < count; i++)
        {
            cumulative += sum > 0 ? Math.Max(weights[i], 0) : 1;
            var end = i == count - 1 ? total : (int)Math.Round(total * cumulative / (sum > 0 ? sum : count));
            var offset = start + i * splitter;
            var size = Math.Max(0, end - start);
            parts[i] = sideBySide
                ? new Rectangle(bounds.X + offset, bounds.Y, size, bounds.Height)
                : new Rectangle(bounds.X, bounds.Y + offset, bounds.Width, size);
            if (i < count - 1)
                splitters[i] = sideBySide
                    ? new Rectangle(bounds.X + offset + size, bounds.Y, splitter, bounds.Height)
                    : new Rectangle(bounds.X, bounds.Y + offset + size, bounds.Width, splitter);
            start = end;
        }
    }
}
