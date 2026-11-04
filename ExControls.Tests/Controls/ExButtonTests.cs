namespace ExControls.Tests.Controls;

/// <summary>
/// Border of ExButton in the custom style: focus and hover temporarily change the border color, BorderColor meanwhile returns
/// the original color and the original border comes back afterwards.
/// </summary>
[TestClass]
public class ExButtonTests
{
    [TestMethod]
    public void Zameranie_RamikFarbouZamerania_BorderColorVraciPovodnu()
    {
        using var button = CreateButton();

        button.RaiseGotFocus();

        Assert.AreEqual(Color.Blue, NativeBorderColor(button));
        Assert.AreEqual(Color.Red, button.ExFlatAppearance.BorderColor);
    }

    [TestMethod]
    public void PrechodMysouPocasZamerania_PoOdchodeMysiAStrateZamerania_PovodnyRamik()
    {
        using var button = CreateButton();

        button.RaiseGotFocus();
        button.RaiseMouseEnter();
        Assert.AreEqual(Color.Green, NativeBorderColor(button));
        button.RaiseMouseLeave();
        Assert.AreEqual(Color.Blue, NativeBorderColor(button));
        button.RaiseLostFocus();

        Assert.AreEqual(Color.Red, NativeBorderColor(button));
        Assert.AreEqual(Color.Red, button.ExFlatAppearance.BorderColor);
    }

    [TestMethod]
    public void NeaktivneTlacidlo_VlastnyVzhladNaTmavomPozadi_TextJeSvetlejsiAkoPozadie()
    {
        using var button = new ExButton { DefaultStyle = false, Enabled = false, Text = "Odstranit", Size = new Size(120, 30) };
        button.BackColor = Color.FromArgb(45, 45, 48);
        button.ForeColor = Color.FromArgb(241, 241, 241);
        button.ExFlatAppearance.BorderColor = Color.FromArgb(67, 67, 70);

        using var bitmap = new Bitmap(button.Width, button.Height);
        button.DrawToBitmap(bitmap, new Rectangle(Point.Empty, bitmap.Size));

        var darkest = 255;
        var brightest = 0;
        for (var x = 3; x < bitmap.Width - 3; x++)
        for (var y = 3; y < bitmap.Height - 3; y++)
        {
            darkest = Math.Min(darkest, bitmap.GetPixel(x, y).R);
            brightest = Math.Max(brightest, bitmap.GetPixel(x, y).R);
        }

        Assert.AreEqual(45, darkest, "text nesmie byt tmavsi ako pozadie");
        Assert.IsGreaterThan(90, brightest, "text musi byt citatelny");
    }

    [TestMethod]
    public void NeaktivnaFarbaTextu_LeziMedziPozadimATextom()
    {
        using var button = new ExButton { BackColor = Color.FromArgb(40, 40, 40), ForeColor = Color.FromArgb(240, 240, 240) };

        Assert.IsGreaterThan(40, (int)button.DisabledForeColor.R);
        Assert.IsLessThan(240, (int)button.DisabledForeColor.R);
    }

    private static TestButton CreateButton()
    {
        var button = new TestButton { DefaultStyle = false };
        button.ExFlatAppearance.BorderColor = Color.Red;
        button.ExFlatAppearance.MouseOverBorderColor = Color.Green;
        button.ExFlatAppearance.FocusBorderColor = Color.Blue;
        return button;
    }

    // skutocna farba ramika - ExButton.FlatAppearance je zastarana, preto cez ButtonBase
    private static Color NativeBorderColor(ButtonBase button) => button.FlatAppearance.BorderColor;

    private sealed class TestButton : ExButton
    {
        public void RaiseGotFocus() => OnGotFocus(EventArgs.Empty);

        public void RaiseLostFocus() => OnLostFocus(EventArgs.Empty);

        public void RaiseMouseEnter() => OnMouseEnter(EventArgs.Empty);

        public void RaiseMouseLeave() => OnMouseLeave(EventArgs.Empty);
    }
}
