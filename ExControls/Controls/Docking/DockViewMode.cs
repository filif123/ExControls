namespace ExControls.Docking;

/// <summary>
/// Kde je pohlad skupiny zobrazeny - urcuje tlacidla zahlavia a karty.
/// </summary>
internal enum DockViewMode
{
    /// <summary>
    /// Ukotveny v zone spravcu (spendlik zbali skupinu na listu).
    /// </summary>
    Docked,

    /// <summary>
    /// Vysunuty zo zbalenej listy - jeden panel bez kariet, spendlik skupinu ukotvi.
    /// </summary>
    AutoHide,

    /// <summary>
    /// V plavajucom okne (bez spendlika, tahanie zahlavia presuva okno).
    /// </summary>
    Floating
}
