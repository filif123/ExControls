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
    /// Viditelne uzly stromov zon (skupiny aj rozdelenia).
    /// </summary>
    public Dictionary<DockNode, Rectangle> Nodes { get; } = [];

    /// <summary>
    /// Rozdelovace zon a uzlov (rozdelovac zony je pred rozdelovacmi jej uzlov).
    /// </summary>
    public List<DockSplitterInfo> Splitters { get; } = [];

    /// <summary>
    /// Obdlznik viditelneho uzla; prazdny, ak sa uzol nezobrazuje.
    /// </summary>
    public Rectangle BoundsOf(DockNode node) => Nodes.TryGetValue(node, out var bounds) ? bounds : Rectangle.Empty;

    /// <summary>
    /// Rozdelovac zony (medzi zonou a dokumentom); prazdny, ak sa zona nezobrazuje.
    /// </summary>
    public Rectangle ZoneSplitter(ExDockSide side) => Splitters.Find(s => s.IsZone && s.Side == side)?.Bounds ?? Rectangle.Empty;

    /// <summary>
    /// Najde rozdelovac pod bodom.
    /// </summary>
    /// <param name="point">bod v klientskej oblasti spravcu</param>
    /// <returns>rozdelovac alebo <see langword="null" /></returns>
    public DockSplitterInfo? HitSplitter(Point point) => Splitters.Find(s => s.Bounds.Contains(point));
}
