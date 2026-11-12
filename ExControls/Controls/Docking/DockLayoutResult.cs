namespace ExControls.Docking;

/// <summary>
/// Vysledok <see cref="DockLayoutEngine" />: obdlzniky v klientskej oblasti spravcu, polia su indexovane stranou
/// (<see cref="ExDockSide" />); nezobrazena cast ma prazdny obdlznik.
/// </summary>
internal sealed class DockLayoutResult
{
    /// <summary>
    /// Oblast dokumentu.
    /// </summary>
    public Rectangle Document { get; set; }

    /// <summary>
    /// Oblast medzi listami auto-hide (zony a dokument) - v nej sa vysuvaju zbalene panely.
    /// </summary>
    public Rectangle Inner { get; set; }

    /// <summary>
    /// Listy auto-hide pri okrajoch.
    /// </summary>
    public Rectangle[] Strips { get; } = new Rectangle[4];

    /// <summary>
    /// Zony s ukotvenymi skupinami.
    /// </summary>
    public Rectangle[] Zones { get; } = new Rectangle[4];

    /// <summary>
    /// Rozdelovace medzi zonou a dokumentom.
    /// </summary>
    public Rectangle[] ZoneSplitters { get; } = new Rectangle[4];

    /// <summary>
    /// Ukotvene skupiny zony v poradi vstupnych vah.
    /// </summary>
    public Rectangle[][] Groups { get; } = [[], [], [], []];

    /// <summary>
    /// Rozdelovace medzi skupinami zony (i = medzi skupinou i a i + 1).
    /// </summary>
    public Rectangle[][] GroupSplitters { get; } = [[], [], [], []];

    /// <summary>
    /// Najde rozdelovac pod bodom.
    /// </summary>
    /// <param name="point">bod v klientskej oblasti spravcu</param>
    /// <returns>rozdelovac alebo <see langword="null" /></returns>
    public DockSplitterHit? HitSplitter(Point point)
    {
        for (var side = 0; side < 4; side++)
        {
            var splitters = GroupSplitters[side];
            for (var i = 0; i < splitters.Length; i++)
                if (splitters[i].Contains(point))
                    return new DockSplitterHit((ExDockSide)side, i);

            if (ZoneSplitters[side].Contains(point))
                return new DockSplitterHit((ExDockSide)side, -1);
        }

        return null;
    }
}
