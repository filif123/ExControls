namespace ExControls.Docking;

/// <summary>
/// Navadzacia znacka pri tahani panela.
/// </summary>
/// <param name="Bounds">znacka na obrazovke</param>
/// <param name="Target">ciel, ak sa panel pusti nad znackou</param>
/// <param name="Glyph">smer kresby; <see langword="null" /> = karta do skupiny</param>
internal sealed record DockGuide(Rectangle Bounds, DockDropTarget Target, ExDockSide? Glyph);
