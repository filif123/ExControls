namespace ExControls.Docking;

/// <summary>
/// Navadzacie znacky pre jednu polohu kurzora: tlacidla s cielmi a podlozky, na ktorych lezia (na obrazovke).
/// </summary>
internal sealed class DockGuideSet
{
    /// <summary>
    /// Tlacidla; neskor pridane maju pri zasahu prednost.
    /// </summary>
    public List<DockGuide> Guides { get; } = [];

    /// <summary>
    /// Podlozky (mnohouholniky) pod tlacidlami.
    /// </summary>
    public List<Point[]> Plates { get; } = [];

    /// <summary>
    /// Obdlznik kompasu - kym je v nom kurzor, kompas ostava pri svojom cieli (aj ked presahuje mimo neho).
    /// </summary>
    public Rectangle CompassBounds { get; set; }

    /// <summary>
    /// Skupina kompasu; <see langword="null" /> pri kompase nad dokumentom alebo bez kompasu.
    /// </summary>
    public DockGroup? CompassGroup { get; set; }

    /// <summary>
    /// Kompas je nad dokumentom.
    /// </summary>
    public bool CompassDocument { get; set; }
}
