namespace ExControls;

/// <summary>
/// Farby vlastneho zahlavia odvodene z pozadia a popredia, ak ich prvok nema nastavene (ciste).
/// </summary>
internal static class TitleBarColors
{
    /// <summary>
    /// Pozadie tlacidla zatvorenia pod mysou ako v systemovom zahlavi Windows 11 alebo Windows 10.
    /// </summary>
    public static Color CloseHover(bool windows11) => windows11 ? Color.FromArgb(196, 43, 28) : Color.FromArgb(232, 17, 35);

    /// <summary>
    /// Zmes farieb: 0 = <paramref name="from" />, 1 = <paramref name="to" />.
    /// </summary>
    public static Color Blend(Color from, Color to, double amount) => Color.FromArgb(
        Mix(from.R, to.R, amount),
        Mix(from.G, to.G, amount),
        Mix(from.B, to.B, amount));

    /// <summary>
    /// Pozadie tlacidla okna pod mysou.
    /// </summary>
    public static Color ButtonHover(Color back, Color fore) => Blend(back, fore, 0.1);

    /// <summary>
    /// Pozadie stlaceneho tlacidla okna.
    /// </summary>
    public static Color ButtonPressed(Color back, Color fore) => Blend(back, fore, 0.2);

    /// <summary>
    /// Pozadie stlaceneho tlacidla zatvorenia: Windows 11 ho mierne prekryje pozadim, Windows 10 zosvetli.
    /// </summary>
    public static Color ClosePressed(Color closeHover, Color back, bool windows11) =>
        windows11 ? Blend(closeHover, back, 0.1) : Blend(closeHover, Color.White, 0.4);

    /// <summary>
    /// Text a znacky neaktivneho okna.
    /// </summary>
    public static Color Inactive(Color back, Color fore) => Blend(fore, back, 0.55);

    /// <summary>
    /// Horny okraj okna vo Windows 10 (system ho pri vlastnom zahlavi nekresli).
    /// </summary>
    public static Color TopBorder(Color back, Color fore, bool active) => Blend(back, fore, active ? 0.35 : 0.2);

    private static int Mix(int from, int to, double amount) => (int)Math.Round(from + (to - from) * amount);
}
