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
