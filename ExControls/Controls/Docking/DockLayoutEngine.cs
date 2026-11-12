namespace ExControls.Docking;

/// <summary>
/// Vypocet rozlozenia spravcu panelov bez prvkov (testovatelny): listy auto-hide pri okrajoch, zony vlavo a vpravo cez
/// celu vysku, zony hore a dole medzi nimi, skupiny pozdlz zony podla vah a dokument vo zvysku.
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
            var weights = input.Weights[index];
            if (weights.Length == 0)
                continue;

            // zona sa zuzi, aby ostal dokument; ak sa nezmesti ani s dokumentom najmensim, nezobrazi sa
            var max = Thickness(area, side) - input.MinDocument - input.Splitter;
            if (max <= 0)
                continue;

            var size = Math.Max(Math.Min(input.ZoneSizes[index], max), Math.Min(input.MinZone, max));
            var zone = Take(ref area, side, size);
            result.Zones[index] = zone;
            result.ZoneSplitters[index] = Take(ref area, side, input.Splitter);
            Split(zone, side, weights, input.Splitter, out var groups, out var splitters);
            result.Groups[index] = groups;
            result.GroupSplitters[index] = splitters;
        }

        result.Document = area;
        return result;
    }

    /// <summary>
    /// Hrubka oblasti v smere strany (sirka pre vlavo/vpravo, vyska pre hore/dole).
    /// </summary>
    public static int Thickness(Rectangle area, ExDockSide side) => IsHorizontalSide(side) ? area.Width : area.Height;

    /// <summary>
    /// Zona na strane vlavo alebo vpravo (skupiny pod sebou, rozdelovac zony sa taha vodorovne).
    /// </summary>
    public static bool IsHorizontalSide(ExDockSide side) => side is ExDockSide.Left or ExDockSide.Right;

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
    /// Rozdeli zonu na skupiny podla vah: vlavo a vpravo pod sebou, hore a dole vedla seba; medzi skupinami rozdelovace.
    /// </summary>
    public static void Split(Rectangle zone, ExDockSide side, double[] weights, int splitter, out Rectangle[] groups,
        out Rectangle[] splitters)
    {
        ArgumentNullException.ThrowIfNull(weights);

        var stacked = IsHorizontalSide(side);
        var count = weights.Length;
        groups = new Rectangle[count];
        splitters = new Rectangle[Math.Max(0, count - 1)];
        var length = stacked ? zone.Height : zone.Width;
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
            groups[i] = stacked
                ? new Rectangle(zone.X, zone.Y + offset, zone.Width, size)
                : new Rectangle(zone.X + offset, zone.Y, size, zone.Height);
            if (i < count - 1)
                splitters[i] = stacked
                    ? new Rectangle(zone.X, zone.Y + offset + size, zone.Width, splitter)
                    : new Rectangle(zone.X + offset + size, zone.Y, splitter, zone.Height);
            start = end;
        }
    }
}
