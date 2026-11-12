namespace ExControls.Docking;

/// <summary>
/// Rozdelovac pod mysou: medzi zonou a dokumentom (<see cref="Index" /> -1) alebo medzi skupinami zony
/// (<see cref="Index" /> = poradie skupiny pred rozdelovacom).
/// </summary>
/// <param name="Side">strana zony</param>
/// <param name="Index">-1 = rozdelovac zony, inak skupina pred rozdelovacom</param>
internal readonly record struct DockSplitterHit(ExDockSide Side, int Index)
{
    /// <summary>
    /// Rozdelovac medzi zonou a dokumentom.
    /// </summary>
    public bool IsZone => Index < 0;

    /// <summary>
    /// Rozdelovac sa taha vodorovne (zvisla ciara): zona vlavo/vpravo alebo skupiny zony hore/dole.
    /// </summary>
    public bool IsVertical => IsZone ? Side is ExDockSide.Left or ExDockSide.Right : Side is ExDockSide.Top or ExDockSide.Bottom;
}
