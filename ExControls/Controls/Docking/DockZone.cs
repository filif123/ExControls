namespace ExControls.Docking;

/// <summary>
/// Zona spravcu na jednej strane: skupiny v poradi pozdlz strany (ukotvene aj zbalene na liste) a hrubka zony.
/// </summary>
/// <param name="side">strana zony</param>
/// <param name="size">hrubka zony v logickych bodoch</param>
internal sealed class DockZone(ExDockSide side, int size)
{
    /// <summary>
    /// Strana zony.
    /// </summary>
    public ExDockSide Side { get; } = side;

    /// <summary>
    /// Hrubka zony v logickych bodoch (96 DPI); ostava aj ked je zona prazdna.
    /// </summary>
    public int Size { get; set; } = size;

    /// <summary>
    /// Skupiny zony v poradi (zlava doprava, zhora nadol).
    /// </summary>
    public List<DockGroup> Groups { get; } = [];

    /// <summary>
    /// Ukotvene skupiny s aspon jednym zobrazenym panelom - tie zaberaju miesto v zone.
    /// </summary>
    public List<DockGroup> DockedGroups => Groups.Where(g => !g.AutoHide && g.HasVisiblePanels).ToList();

    /// <summary>
    /// Zbalene skupiny so zobrazenymi panelmi - tie maju tlacidla na liste.
    /// </summary>
    public List<DockGroup> AutoHideGroups => Groups.Where(g => g.AutoHide && g.HasVisiblePanels).ToList();
}
