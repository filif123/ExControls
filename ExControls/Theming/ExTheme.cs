namespace ExControls;

/// <summary>
/// Farebna tema ovladacich prvkov. Ex* prvky ju pouziju cez <see cref="IThemeable" />, ostatne prvky cez obsluhy
/// zaregistrovane v <see cref="ExThemer" />.
/// </summary>
public sealed class ExTheme
{
    /// <summary>
    /// Pouzit systemovy vzhlad prvkov (Ex* prvky kreslia ako standardne prvky Windows).
    /// </summary>
    public bool UseSystemStyle { get; set; }

    /// <summary>
    /// Tmave posuvniky (tema Windows DarkMode_Explorer).
    /// </summary>
    public bool DarkScrollBars { get; set; }

    /// <summary>
    /// Pozadie panelov, okien a ponuk.
    /// </summary>
    public Color PanelBackColor { get; set; } = SystemColors.Control;

    /// <summary>
    /// Text panelov, okien a ponuk.
    /// </summary>
    public Color PanelForeColor { get; set; } = SystemColors.ControlText;

    /// <summary>
    /// Pozadie poli (text, zoznamy, tabulky).
    /// </summary>
    public Color BoxBackColor { get; set; } = SystemColors.Window;

    /// <summary>
    /// Text poli.
    /// </summary>
    public Color BoxForeColor { get; set; } = SystemColors.WindowText;

    /// <summary>
    /// Pozadie tlacidiel a hlaviciek.
    /// </summary>
    public Color ButtonBackColor { get; set; } = SystemColors.Control;

    /// <summary>
    /// Text tlacidiel a hlaviciek.
    /// </summary>
    public Color ButtonForeColor { get; set; } = SystemColors.ControlText;

    /// <summary>
    /// Farba okrajov.
    /// </summary>
    public Color BorderColor { get; set; } = SystemColors.ControlDark;

    /// <summary>
    /// Pozadie zvyraznenia (vyber, prechod mysou, zameranie).
    /// </summary>
    public Color HighlightBackColor { get; set; } = SystemColors.Highlight;

    /// <summary>
    /// Text zvyraznenia.
    /// </summary>
    public Color HighlightForeColor { get; set; } = SystemColors.HighlightText;

    /// <summary>
    /// Farba znaciek (zaskrtnutie, prepinac, cisla tyzdnov).
    /// </summary>
    public Color MarkColor { get; set; } = SystemColors.ControlText;

    /// <summary>
    /// Text popisov.
    /// </summary>
    public Color LabelForeColor { get; set; } = SystemColors.ControlText;

    /// <summary>
    /// Text tlacidla Dnes v kalendari; <see langword="null" /> = ponechat.
    /// </summary>
    public string? TodayText { get; set; }
}
