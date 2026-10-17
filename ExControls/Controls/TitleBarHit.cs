namespace ExControls;

/// <summary>
/// Cast vlastneho zahlavia okna v danom bode - urcuje, co Windows urobi s mysou (<see cref="ExTitleBar.HitTest" />).
/// </summary>
public enum TitleBarHit
{
    /// <summary>
    /// Prvok v zahlavi (polozka ponuky, tlacidlo panela nastrojov) - mys spracuje on.
    /// </summary>
    Client,

    /// <summary>
    /// Volne miesto zahlavia - tahanie okna, dvojklik na maximalizaciu, systemova ponuka pravym klikom.
    /// </summary>
    Caption,

    /// <summary>
    /// Ikona okna - systemova ponuka.
    /// </summary>
    SystemMenu,

    /// <summary>
    /// Tlacidlo minimalizacie.
    /// </summary>
    MinimizeButton,

    /// <summary>
    /// Tlacidlo maximalizacie a obnovenia - vo Windows 11 nad nim system ponukne rozlozenia (Snap Layouts).
    /// </summary>
    MaximizeButton,

    /// <summary>
    /// Tlacidlo zatvorenia.
    /// </summary>
    CloseButton,

    /// <summary>
    /// Horny okraj okna - zmena vysky.
    /// </summary>
    ResizeTop,

    /// <summary>
    /// Lavy horny roh okna - zmena velkosti.
    /// </summary>
    ResizeTopLeft,

    /// <summary>
    /// Pravy horny roh okna - zmena velkosti.
    /// </summary>
    ResizeTopRight,

    /// <summary>
    /// Tlacidlo pomocnika (<see cref="Form.HelpButton" />, len bez minimalizacie a maximalizacie) - kontextova napoveda.
    /// </summary>
    HelpButton
}
