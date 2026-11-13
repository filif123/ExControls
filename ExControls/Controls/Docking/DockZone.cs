namespace ExControls.Docking;

/// <summary>
/// Zona spravcu na jednej strane: strom rozlozenia s korenom pozdlz strany (ukotvene aj zbalene skupiny) a hrubka zony.
/// Stredova zona (<see cref="CreateCenter" />) zabera miesto medzi zonami a jej strom obsahuje dokument.
/// </summary>
/// <param name="side">strana zony</param>
/// <param name="size">hrubka zony v logickych bodoch</param>
internal sealed class DockZone(ExDockSide side, int size)
{
    private DockZone(DockDocumentNode document) : this(ExDockSide.Left, 0)
    {
        Document = document;
        Root.Add(document);
    }

    /// <summary>
    /// Stredova zona s dokumentom.
    /// </summary>
    public static DockZone CreateCenter() => new(new DockDocumentNode());

    /// <summary>
    /// Strana zony (stredova zona ju nema - jej skupiny maju vlastnu <see cref="DockGroup.HomeSide" />).
    /// </summary>
    public ExDockSide Side { get; } = side;

    /// <summary>
    /// Dokument v strome stredovej zony; <see langword="null" /> pre zony na stranach.
    /// </summary>
    public DockDocumentNode? Document { get; }

    /// <summary>
    /// Stredova zona s dokumentom.
    /// </summary>
    public bool IsCenter => Document is not null;

    /// <summary>
    /// Hrubka zony v logickych bodoch (96 DPI); ostava aj ked je zona prazdna.
    /// </summary>
    public int Size { get; set; } = size;

    /// <summary>
    /// Koren stromu: vlavo a vpravo riadky pod sebou, hore a dole stlpce vedla seba.
    /// </summary>
    public DockSplit Root { get; } = new(!DockLayoutEngine.IsHorizontalSide(side));

    /// <summary>
    /// Skupiny zony v poradi stromu (zlava doprava, zhora nadol).
    /// </summary>
    public List<DockGroup> Groups => DockTree.Groups(Root).ToList();

    /// <summary>
    /// Ukotvene skupiny s aspon jednym zobrazenym panelom - tie zaberaju miesto v zone.
    /// </summary>
    public List<DockGroup> DockedGroups => DockTree.Groups(Root).Where(IsDocked).ToList();

    /// <summary>
    /// Zbalene skupiny so zobrazenymi panelmi - tie maju tlacidla na liste.
    /// </summary>
    public List<DockGroup> AutoHideGroups => DockTree.Groups(Root).Where(g => g.AutoHide && g.HasVisiblePanels).ToList();

    /// <summary>
    /// Skupina zabera miesto v zone (ukotvena so zobrazenym panelom).
    /// </summary>
    public static bool IsDocked(DockGroup group) => !group.AutoHide && group.HasVisiblePanels;
}
