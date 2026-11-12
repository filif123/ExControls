namespace ExControls.Docking;

/// <summary>
/// Farby ramu spravcu panelov (zahlavia, karty, listy, navadzacie znacky) odvodene z <see cref="ExTheme" />; systemovy
/// styl ma systemove farby.
/// </summary>
internal sealed class DockColors
{
    /// <summary>
    /// Pozadie spravcu, medzier, list a neaktivnych zahlavi.
    /// </summary>
    public Color Back { get; private init; } = SystemColors.Control;

    /// <summary>
    /// Text neaktivnych zahlavi a list.
    /// </summary>
    public Color Fore { get; private init; } = SystemColors.ControlText;

    /// <summary>
    /// Okraje skupin a znaciek.
    /// </summary>
    public Color Border { get; private init; } = SystemColors.ControlDark;

    /// <summary>
    /// Pozadie zahlavia skupiny s fokusom, otvoreneho tlacidla listy a zvyraznenej znacky.
    /// </summary>
    public Color ActiveBack { get; private init; } = SystemColors.Highlight;

    /// <summary>
    /// Text na <see cref="ActiveBack" />.
    /// </summary>
    public Color ActiveFore { get; private init; } = SystemColors.HighlightText;

    /// <summary>
    /// Pozadie vybranej karty a tlacidla pod mysou.
    /// </summary>
    public Color ButtonBack { get; private init; } = SystemColors.ControlLight;

    /// <summary>
    /// Text vybranej karty.
    /// </summary>
    public Color ButtonFore { get; private init; } = SystemColors.ControlText;

    /// <summary>
    /// Pozadie navadzacich znaciek.
    /// </summary>
    public Color Box { get; private init; } = SystemColors.Window;

    /// <summary>
    /// Kresba v navadzacich znackach.
    /// </summary>
    public Color Mark { get; private init; } = SystemColors.ControlText;

    /// <summary>
    /// Farby systemoveho stylu.
    /// </summary>
    public static DockColors System { get; } = new();

    /// <summary>
    /// Farby podla temy.
    /// </summary>
    public static DockColors From(ExTheme? theme) =>
        theme is null || theme.UseSystemStyle
            ? System
            : new DockColors
            {
                Back = theme.PanelBackColor,
                Fore = theme.PanelForeColor,
                Border = theme.BorderColor,
                ActiveBack = theme.HighlightBackColor,
                ActiveFore = theme.HighlightForeColor,
                ButtonBack = theme.ButtonBackColor,
                ButtonFore = theme.ButtonForeColor,
                Box = theme.BoxBackColor,
                Mark = theme.MarkColor
            };

    /// <summary>
    /// Priesvitne prekrytie pre tlacidlo pod mysou na pozadi s textom danej farby.
    /// </summary>
    public static Color Hover(Color fore) => Color.FromArgb(48, fore);
}
