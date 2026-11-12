using System.Drawing.Drawing2D;

namespace ExControls.Docking;

/// <summary>
/// Kresby ramu spravcu panelov: znaky tlacidiel zahlavia (zavriet, pripnut, ponuka), navadzacie znacky a zvisly text
/// list.
/// </summary>
internal static class DockPainter
{
    /// <summary>
    /// Krizik zatvorenia (skrytia) panela.
    /// </summary>
    /// <param name="g">grafika</param>
    /// <param name="bounds">tlacidlo</param>
    /// <param name="color">farba znaku</param>
    /// <param name="size">velkost znaku v bodoch zariadenia</param>
    /// <param name="width">hrubka ciary</param>
    public static void DrawClose(Graphics g, Rectangle bounds, Color color, int size, float width)
    {
        var x = bounds.X + (bounds.Width - size) / 2f;
        var y = bounds.Y + (bounds.Height - size) / 2f;
        var smoothing = g.SmoothingMode;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using (var pen = new Pen(color, width))
        {
            g.DrawLine(pen, x, y, x + size, y + size);
            g.DrawLine(pen, x + size, y, x, y + size);
        }

        g.SmoothingMode = smoothing;
    }

    /// <summary>
    /// Spendlik: zvisly (panel je ukotveny - klik ho zbali na listu) alebo vodorovny (panel je zbaleny - klik ho ukotvi).
    /// </summary>
    /// <param name="g">grafika</param>
    /// <param name="bounds">tlacidlo</param>
    /// <param name="color">farba znaku</param>
    /// <param name="size">velkost znaku v bodoch zariadenia</param>
    /// <param name="pinned">zvisly spendlik ukotveneho panela</param>
    public static void DrawPin(Graphics g, Rectangle bounds, Color color, int size, bool pinned)
    {
        var x = bounds.X + (bounds.Width - size) / 2;
        var y = bounds.Y + (bounds.Height - size) / 2;
        var bar = size * 45 / 100;
        using var pen = new Pen(color);
        if (pinned)
        {
            g.DrawRectangle(pen, x + size * 3 / 10, y, size * 4 / 10, bar);
            g.DrawLine(pen, x + size / 10, y + bar, x + size * 9 / 10, y + bar);
            g.DrawLine(pen, x + size / 2, y + bar, x + size / 2, y + size);
        }
        else
        {
            var head = x + size - bar;
            g.DrawRectangle(pen, head, y + size * 3 / 10, bar, size * 4 / 10);
            g.DrawLine(pen, head, y + size / 10, head, y + size * 9 / 10);
            g.DrawLine(pen, x, y + size / 2, head, y + size / 2);
        }
    }

    /// <summary>
    /// Sipka ponuky okna.
    /// </summary>
    public static void DrawMenuArrow(Graphics g, Rectangle bounds, Color color, int size) =>
        ExButtonRenderer.DrawTriangle(g, color, new Point(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2), size, ArrowDirection.Down);

    /// <summary>
    /// Navadzacia znacka: okno s vyplnenym pasom na strane ukotvenia a sipkou, alebo okno s kartou (vlozenie medzi karty).
    /// </summary>
    /// <param name="g">grafika</param>
    /// <param name="bounds">znacka</param>
    /// <param name="side">strana ukotvenia; <see langword="null" /> = karta do skupiny</param>
    /// <param name="colors">farby</param>
    /// <param name="hover">znacka je pod mysou</param>
    public static void DrawGuide(Graphics g, Rectangle bounds, ExDockSide? side, DockColors colors, bool hover)
    {
        var back = hover ? colors.ActiveBack : colors.Box;
        var fore = hover ? colors.ActiveFore : colors.Mark;
        var band = hover ? colors.ActiveFore : colors.ActiveBack;
        using (var brush = new SolidBrush(back))
            g.FillRectangle(brush, bounds);
        using (var pen = new Pen(colors.Border))
            g.DrawRectangle(pen, bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);

        var inset = bounds.Width * 7 / 32;
        var window = Rectangle.Inflate(bounds, -inset, -inset);
        var part = side switch
        {
            ExDockSide.Left => new Rectangle(window.X, window.Y, window.Width * 2 / 5, window.Height),
            ExDockSide.Right => new Rectangle(window.Right - window.Width * 2 / 5, window.Y, window.Width * 2 / 5, window.Height),
            ExDockSide.Top => new Rectangle(window.X, window.Y, window.Width, window.Height * 2 / 5),
            ExDockSide.Bottom => new Rectangle(window.X, window.Bottom - window.Height * 2 / 5, window.Width, window.Height * 2 / 5),
            _ => new Rectangle(window.X, window.Y, window.Width * 2 / 5, Math.Max(2, window.Height / 5))
        };
        using (var brush = new SolidBrush(band))
            g.FillRectangle(brush, part);
        using (var pen = new Pen(fore))
        {
            g.DrawRectangle(pen, window.X, window.Y, window.Width - 1, window.Height - 1);
            if (side is null)
                g.DrawLine(pen, window.X, part.Bottom, window.Right - 1, part.Bottom);
        }

        if (side is not { } direction)
            return;

        // sipka v zvysku okna smerom k strane ukotvenia
        var rest = direction switch
        {
            ExDockSide.Left => new Rectangle(part.Right, window.Y, window.Right - part.Right, window.Height),
            ExDockSide.Right => new Rectangle(window.X, window.Y, part.X - window.X, window.Height),
            ExDockSide.Top => new Rectangle(window.X, part.Bottom, window.Width, window.Bottom - part.Bottom),
            _ => new Rectangle(window.X, window.Y, window.Width, part.Y - window.Y)
        };
        var arrow = direction switch
        {
            ExDockSide.Left => ArrowDirection.Left,
            ExDockSide.Right => ArrowDirection.Right,
            ExDockSide.Top => ArrowDirection.Up,
            _ => ArrowDirection.Down
        };
        ExButtonRenderer.DrawTriangle(g, fore, new Point(rest.X + rest.Width / 2, rest.Y + rest.Height / 2),
            Math.Max(2, bounds.Width / 8), arrow);
    }

    /// <summary>
    /// Text tlacidla listy: na boku zvislo (zhora nadol), hore a dole vodorovne.
    /// </summary>
    public static void DrawStripText(Graphics g, string text, Font font, Rectangle bounds, Color color, bool vertical)
    {
        if (!vertical)
        {
            TextRenderer.DrawText(g, text, font, bounds, color,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine |
                TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
            return;
        }

        using var format = VerticalFormat();
        using var brush = new SolidBrush(color);
        var hint = g.TextRenderingHint;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
        g.DrawString(text, font, brush, bounds, format);
        g.TextRenderingHint = hint;
    }

    /// <summary>
    /// Dlzka textu tlacidla listy: zvisly text kresli GDI+ (iny rozmer ako TextRenderer), meria sa na obrazovke (DPI
    /// systemu).
    /// </summary>
    public static int MeasureStripText(string text, Font font, bool vertical)
    {
        if (!vertical)
            return TextRenderer.MeasureText(text, font, Size.Empty, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix).Width;

        using var g = Graphics.FromHwnd(IntPtr.Zero);
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
        using var format = VerticalFormat();
        var size = g.MeasureString(text, font, new PointF(0, 0), format);
        return (int)Math.Ceiling(Math.Max(size.Width, size.Height));
    }

    // zvisly text zhora nadol bez skracovania (dlzku tlacidla urcuje MeasureStripText)
    private static StringFormat VerticalFormat() => new(StringFormatFlags.DirectionVertical | StringFormatFlags.NoWrap)
    {
        Alignment = StringAlignment.Center,
        LineAlignment = StringAlignment.Center,
        Trimming = StringTrimming.None,
        HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None
    };
}
