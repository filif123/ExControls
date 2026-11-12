namespace ExControls.Docking;

/// <summary>
/// Ciel presunu panela alebo skupiny.
/// </summary>
/// <param name="Kind">druh ciela</param>
/// <param name="Side">strana zony (<see cref="DockDropKind.Edge" />) alebo smer sipky (<see cref="DockDropKind.Split" />:
/// vlavo a hore = pred skupinou)</param>
/// <param name="Group">cielova skupina (<see cref="DockDropKind.Tab" />, <see cref="DockDropKind.Split" />)</param>
internal sealed record DockDropTarget(DockDropKind Kind, ExDockSide Side, DockGroup? Group = null)
{
    /// <summary>
    /// Plavajuce okno.
    /// </summary>
    public static DockDropTarget Floating { get; } = new(DockDropKind.Float, ExDockSide.Right);

    /// <summary>
    /// Nova skupina sa vlozi pred cielovu (sipka vlavo alebo hore).
    /// </summary>
    public bool Before => Side is ExDockSide.Left or ExDockSide.Top;
}
