namespace ExControls;

/// <summary>
/// Colors of the custom title bar derived from the background and foreground when the control has not set them (pure).
/// </summary>
internal static class TitleBarColors
{
    /// <summary>
    /// Background of the close button under the mouse as in the system caption of Windows 11 or Windows 10.
    /// </summary>
    /// <param name="windows11">the color of Windows 11 (otherwise Windows 10)</param>
    public static Color CloseHover(bool windows11) => windows11 ? Color.FromArgb(196, 43, 28) : Color.FromArgb(232, 17, 35);

    /// <summary>
    /// Mix of colors: 0 = <paramref name="from" />, 1 = <paramref name="to" />.
    /// </summary>
    /// <param name="from">first color</param>
    /// <param name="to">second color</param>
    /// <param name="amount">share of <paramref name="to" /> from 0 to 1</param>
    public static Color Blend(Color from, Color to, double amount) => Color.FromArgb(
        Mix(from.R, to.R, amount),
        Mix(from.G, to.G, amount),
        Mix(from.B, to.B, amount));

    /// <summary>
    /// Background of a window button under the mouse.
    /// </summary>
    /// <param name="back">background of the title bar</param>
    /// <param name="fore">foreground of the title bar</param>
    public static Color ButtonHover(Color back, Color fore) => Blend(back, fore, 0.1);

    /// <summary>
    /// Background of a pressed window button.
    /// </summary>
    /// <param name="back">background of the title bar</param>
    /// <param name="fore">foreground of the title bar</param>
    public static Color ButtonPressed(Color back, Color fore) => Blend(back, fore, 0.2);

    /// <summary>
    /// Background of the pressed close button: Windows 11 slightly blends it with the background, Windows 10 lightens it.
    /// </summary>
    /// <param name="closeHover">background of the close button under the mouse</param>
    /// <param name="back">background of the title bar</param>
    /// <param name="windows11">the color of Windows 11 (otherwise Windows 10)</param>
    public static Color ClosePressed(Color closeHover, Color back, bool windows11) =>
        windows11 ? Blend(closeHover, back, 0.1) : Blend(closeHover, Color.White, 0.4);

    /// <summary>
    /// Text and glyphs of an inactive form.
    /// </summary>
    /// <param name="back">background of the title bar</param>
    /// <param name="fore">foreground of the title bar</param>
    public static Color Inactive(Color back, Color fore) => Blend(fore, back, 0.55);

    /// <summary>
    /// Text on the accent color: white, black on a light color (yellow, light green).
    /// </summary>
    /// <param name="accent">accent color</param>
    public static Color AccentText(Color accent) => (accent.R * 299 + accent.G * 587 + accent.B * 114) / 1000 >= 128 ? Color.Black : Color.White;

    /// <summary>
    /// Top border of the window in Windows 10 (the system does not draw it with a custom title bar).
    /// </summary>
    /// <param name="back">background of the title bar</param>
    /// <param name="fore">foreground of the title bar</param>
    /// <param name="active">the form is active</param>
    public static Color TopBorder(Color back, Color fore, bool active) => Blend(back, fore, active ? 0.35 : 0.2);

    private static int Mix(int from, int to, double amount) => (int)Math.Round(from + (to - from) * amount);
}
