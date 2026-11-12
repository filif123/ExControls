namespace ExControls;

/// <summary>
/// Side of an <see cref="ExDockManager" /> where tool panels are docked or collapsed to the auto-hide strip.
/// </summary>
public enum ExDockSide
{
    /// <summary>
    /// Left edge; the zone spans the full height between the auto-hide strips.
    /// </summary>
    Left,

    /// <summary>
    /// Top edge; the zone spans the width between the left and right zones.
    /// </summary>
    Top,

    /// <summary>
    /// Right edge; the zone spans the full height between the auto-hide strips.
    /// </summary>
    Right,

    /// <summary>
    /// Bottom edge; the zone spans the width between the left and right zones (like the Error List in Visual Studio).
    /// </summary>
    Bottom
}
