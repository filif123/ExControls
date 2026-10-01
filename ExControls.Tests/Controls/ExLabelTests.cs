namespace ExControls.Tests.Controls;

/// <summary>
/// Zakazany ExLabel kresli text farbou DisabledForeColor, inak rovnako ako povoleny Label
/// (zarovnanie, zalamovanie, mnemotechnika, vypustka, okraje, sprava dolava) - v GDI aj GDI+.
/// </summary>
[TestClass]
public class ExLabelTests
{
    private const string LongText = "Dlhy text popisu, ktory sa do sirky popisu nezmesti a musi sa zalomit na viac riadkov.";

    [TestMethod]
    [DataRow(ContentAlignment.TopLeft, 300, 40, "Kratky text", false, true, 0, false)]
    [DataRow(ContentAlignment.MiddleLeft, 300, 40, "Kratky text", false, true, 0, false)]
    [DataRow(ContentAlignment.MiddleRight, 300, 40, "Kratky text", false, true, 0, false)]
    [DataRow(ContentAlignment.BottomCenter, 300, 40, "Kratky text", false, true, 0, false)]
    [DataRow(ContentAlignment.TopLeft, 120, 90, LongText, false, true, 0, false)]
    [DataRow(ContentAlignment.TopLeft, 120, 20, LongText, true, true, 0, false)]
    [DataRow(ContentAlignment.TopLeft, 200, 30, "&Subor && dalsie", false, true, 0, false)]
    [DataRow(ContentAlignment.TopLeft, 200, 30, "&Subor && dalsie", false, false, 0, false)]
    [DataRow(ContentAlignment.TopLeft, 200, 40, "Kratky text", false, true, 24, false)]
    [DataRow(ContentAlignment.TopLeft, 200, 40, "Kratky text", false, true, 0, true)]
    public void OnPaint_Zakazany_KresliAkoLabelFarbouDisabledForeColor(ContentAlignment align, int width, int height, string text,
        bool autoEllipsis, bool useMnemonic, int padding, bool rightToLeft)
    {
        foreach (var compatible in new[] { false, true })
        {
            using var expected = Configure(new Label { ForeColor = Color.Blue }, compatible, align, width, height, text, autoEllipsis, useMnemonic, padding, rightToLeft);
            using var actual = Configure(new ExLabel { DisabledForeColor = Color.Blue, Enabled = false }, compatible, align, width, height, text,
                autoEllipsis, useMnemonic, padding, rightToLeft);

            AssertSameImage(expected, actual, compatible ? "GDI+" : "GDI");
        }
    }

    [TestMethod]
    public void OnPaint_ZakazanyZarovnanyVlavo_TextZacinaPriLavomOkraji()
    {
        using var label = Configure(new ExLabel { Enabled = false }, false, ContentAlignment.MiddleLeft, 300, 40, "Kratky text");

        using var bitmap = Render(label);

        Assert.IsTrue(FirstInkColumn(bitmap) < 10, "text ma byt zarovnany vlavo, nie na stred");
    }

    [TestMethod]
    public void OnPaint_ZakazanyAutoSizeSMaximalnouSirkou_ZalomiTextDoVyskyZGetPreferredSize()
    {
        using var panel = new Panel { Size = new Size(400, 400) };
        using var expected = Configure(new Label { ForeColor = Color.Blue }, false, ContentAlignment.TopLeft, 0, 0, LongText);
        using var actual = Configure(new ExLabel { DisabledForeColor = Color.Blue, Enabled = false }, false, ContentAlignment.TopLeft, 0, 0, LongText);
        foreach (var label in new Label[] { expected, actual })
        {
            panel.Controls.Add(label);
            label.MaximumSize = new Size(150, 0);
            label.AutoSize = true;
        }
        panel.PerformLayout();

        Assert.AreEqual(expected.Size, actual.Size);
        Assert.IsTrue(actual.Height > 2 * actual.Font.Height, "text sa ma zalomit na viac riadkov");
        AssertSameImage(expected, actual, "AutoSize");
    }

    private static T Configure<T>(T label, bool compatible, ContentAlignment align, int width, int height, string text,
        bool autoEllipsis = false, bool useMnemonic = true, int padding = 0, bool rightToLeft = false) where T : Label
    {
        // testy nevolaju Application.SetCompatibleTextRenderingDefault - rezim kreslenia sa nastavi priamo
        label.UseCompatibleTextRendering = compatible;
        label.AutoSize = false;
        label.Font = new Font("Segoe UI", 9f);
        label.BackColor = Color.White;
        label.Size = new Size(width, height);
        label.TextAlign = align;
        label.Text = text;
        label.AutoEllipsis = autoEllipsis;
        label.UseMnemonic = useMnemonic;
        label.Padding = new Padding(padding, padding / 2, 0, 0);
        label.RightToLeft = rightToLeft ? RightToLeft.Yes : RightToLeft.No;
        return label;
    }

    private static Bitmap Render(Control control)
    {
        var bitmap = new Bitmap(control.Width, control.Height);
        control.DrawToBitmap(bitmap, new Rectangle(Point.Empty, control.Size));
        return bitmap;
    }

    private static void AssertSameImage(Control expected, Control actual, string mode)
    {
        using var expectedBitmap = Render(expected);
        using var actualBitmap = Render(actual);
        Assert.AreEqual(expectedBitmap.Size, actualBitmap.Size, mode);
        Assert.IsTrue(FirstInkColumn(expectedBitmap) < expectedBitmap.Width, $"{mode}: Label nenakreslil ziadny text");

        for (var y = 0; y < expectedBitmap.Height; y++)
        for (var x = 0; x < expectedBitmap.Width; x++)
            Assert.AreEqual(expectedBitmap.GetPixel(x, y), actualBitmap.GetPixel(x, y), $"{mode}: bod [{x}, {y}]");
    }

    private static int FirstInkColumn(Bitmap bitmap)
    {
        var background = Color.White.ToArgb();
        for (var x = 0; x < bitmap.Width; x++)
        for (var y = 0; y < bitmap.Height; y++)
            if (bitmap.GetPixel(x, y).ToArgb() != background)
                return x;
        return bitmap.Width;
    }
}
