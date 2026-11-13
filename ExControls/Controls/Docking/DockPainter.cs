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
    /// Navadzacia znacka ako vo Visual Studiu: tlacidlo s oknom na strane ukotvenia a trojuholnikom k nemu, v strede okno
    /// cez cele tlacidlo (vlozenie medzi karty).
    /// </summary>
    /// <param name="g">grafika</param>
    /// <param name="bounds">znacka</param>
    /// <param name="side">strana ukotvenia; <see langword="null" /> = karta do skupiny</param>
    /// <param name="colors">farby</param>
    /// <param name="hover">znacka je pod mysou</param>
    public static void DrawGuide(Graphics g, Rectangle bounds, ExDockSide? side, DockColors colors, bool hover)
    {
        var fore = hover ? colors.ActiveFore : colors.Mark;
        using (var brush = new SolidBrush(hover ? colors.ActiveBack : colors.ButtonBack))
            g.FillRectangle(brush, bounds);
        using (var pen = new Pen(colors.Border))
            g.DrawRectangle(pen, bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);

        var inset = Math.Max(3, bounds.Width * 6 / 32);
        var inner = Rectangle.Inflate(bounds, -inset, -inset);
        var half = side is ExDockSide.Left or ExDockSide.Right ? inner.Width * 9 / 20 : inner.Height * 9 / 20;
        var window = side switch
        {
            ExDockSide.Left => new Rectangle(inner.X, inner.Y, half, inner.Height),
            ExDockSide.Right => new Rectangle(inner.Right - half, inner.Y, half, inner.Height),
            ExDockSide.Top => new Rectangle(inner.X, inner.Y, inner.Width, half),
            ExDockSide.Bottom => new Rectangle(inner.X, inner.Bottom - half, inner.Width, half),
            _ => inner
        };
        DrawWindow(g, window, fore);
        if (side is not { } direction)
            return;

        // trojuholnik vo zvysku tlacidla smeruje k oknu
        var rest = direction switch
        {
            ExDockSide.Left => new Rectangle(window.Right, inner.Y, inner.Right - window.Right, inner.Height),
            ExDockSide.Right => new Rectangle(inner.X, inner.Y, window.X - inner.X, inner.Height),
            ExDockSide.Top => new Rectangle(inner.X, window.Bottom, inner.Width, inner.Bottom - window.Bottom),
            _ => new Rectangle(inner.X, inner.Y, inner.Width, window.Y - inner.Y)
        };
        var arrow = direction switch
        {
            ExDockSide.Left => ArrowDirection.Left,
            ExDockSide.Right => ArrowDirection.Right,
            ExDockSide.Top => ArrowDirection.Up,
            _ => ArrowDirection.Down
        };
        ExButtonRenderer.DrawTriangle(g, fore, new Point(rest.X + rest.Width / 2, rest.Y + rest.Height / 2),
            Math.Max(2, bounds.Width / 10), arrow);
    }

    // okno: obrys a hrubsie zahlavie
    private static void DrawWindow(Graphics g, Rectangle window, Color color)
    {
        using var pen = new Pen(color);
        g.DrawRectangle(pen, window.X, window.Y, window.Width - 1, window.Height - 1);
        using var brush = new SolidBrush(color);
        g.FillRectangle(brush, window.X, window.Y, window.Width, Math.Max(2, window.Height / 7));
    }

    /// <summary>
    /// Podlozka navadzacich znaciek (kompas alebo stvorec pri okraji).
    /// </summary>
    public static void DrawPlate(Graphics g, Point[] plate, DockColors colors)
    {
        using (var brush = new SolidBrush(colors.Back))
            g.FillPolygon(brush, plate);
        using var pen = new Pen(colors.Border);
        g.DrawPolygon(pen, plate);
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
