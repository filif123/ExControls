namespace ExControls.Docking;

/// <summary>
/// Rozdelovac v rozlozeni: medzi zonou a dokumentom (<see cref="Split" /> je <see langword="null" />) alebo medzi dvoma
/// viditelnymi uzlami rozdelenia.
/// </summary>
/// <param name="Bounds">rozdelovac v klientskej oblasti spravcu</param>
/// <param name="Side">strana zony</param>
/// <param name="Split">rozdelenie; <see langword="null" /> = rozdelovac zony</param>
/// <param name="First">uzol pred rozdelovacom</param>
/// <param name="Second">uzol za rozdelovacom</param>
internal sealed record DockSplitterInfo(Rectangle Bounds, ExDockSide Side, DockSplit? Split = null, DockNode? First = null,
    DockNode? Second = null)
{
    /// <summary>
    /// Rozdelovac medzi zonou a dokumentom.
    /// </summary>
    public bool IsZone => Split is null;

    /// <summary>
    /// Zvisla ciara (taha sa vodorovne): zona vlavo/vpravo alebo uzly vedla seba.
    /// </summary>
    public bool IsVertical => Split?.SideBySide ?? DockLayoutEngine.IsHorizontalSide(Side);
}
