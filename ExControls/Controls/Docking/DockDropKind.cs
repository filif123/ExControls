namespace ExControls.Docking;

/// <summary>
/// Druh ciela presunu panela alebo skupiny.
/// </summary>
internal enum DockDropKind
{
    /// <summary>
    /// Novy stlpec alebo riadok na vonkajsom okraji zony strany (znacka pri okraji spravcu).
    /// </summary>
    Edge,

    /// <summary>
    /// Novy stlpec alebo riadok zony strany hned pri dokumente (sipka kompasu nad dokumentom).
    /// </summary>
    Document,

    /// <summary>
    /// Karty do cielovej skupiny (stred kompasu).
    /// </summary>
    Tab,

    /// <summary>
    /// Cielova skupina sa rozdeli a zdroj bude na strane sipky (sipka kompasu nad skupinou).
    /// </summary>
    Split,

    /// <summary>
    /// Plavajuce okno pri kurzore (mimo znaciek).
    /// </summary>
    Float
}
