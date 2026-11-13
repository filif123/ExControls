namespace ExControls.Docking;

/// <summary>
/// Uzol stromu rozlozenia zony: skupina panelov (<see cref="DockGroup" />) alebo rozdelenie (<see cref="DockSplit" />).
/// </summary>
internal abstract class DockNode
{
    /// <summary>
    /// Podiel uzla na dlzke rodicovskeho rozdelenia (pocitaju sa len viditelni surodenci).
    /// </summary>
    public double Weight { get; set; } = 1;

    /// <summary>
    /// Rozdelenie, v ktorom uzol je; <see langword="null" /> pre koren zony, plavajucu alebo nezaradenu skupinu.
    /// </summary>
    public DockSplit? Parent { get; set; }
}
