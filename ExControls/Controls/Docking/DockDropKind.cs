namespace ExControls.Docking;

/// <summary>
/// Druh ciela presunu panela alebo skupiny.
/// </summary>
internal enum DockDropKind
{
    /// <summary>
    /// Nova skupina na konci zony strany (vonkajsia znacka pri okraji spravcu).
    /// </summary>
    Edge,

    /// <summary>
    /// Karty do cielovej skupiny (stred kosostvorca).
    /// </summary>
    Tab,

    /// <summary>
    /// Nova skupina pred alebo za cielovou skupinou v jej zone (sipka kosostvorca).
    /// </summary>
    Split,

    /// <summary>
    /// Plavajuce okno pri kurzore (mimo znaciek).
    /// </summary>
    Float
}
