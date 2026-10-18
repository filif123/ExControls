namespace ExControls.Tests.Controls;

/// <summary>
/// BorderStyle of ExRichTextBox: the default style uses the set value, the custom style draws its own border with the native
/// border None and keeps the value for switching back to the default style.
/// </summary>
[TestClass]
public class ExRichTextBoxTests
{
    [TestMethod]
    [DataRow(BorderStyle.None)]
    [DataRow(BorderStyle.FixedSingle)]
    public void BorderStyle_PredvolenyStyl_UlozenaHodnota(BorderStyle style)
    {
        using var box = new ExRichTextBox { BorderStyle = style };

        Assert.AreEqual(style, box.BorderStyle);
        Assert.AreEqual(style, NativeBorderStyle(box));
    }

    [TestMethod]
    public void BorderStyle_VlastnyStyl_SystemovyRamikNoneHodnotaOstava()
    {
        using var box = new ExRichTextBox { DefaultStyle = false, BorderStyle = BorderStyle.FixedSingle };

        Assert.AreEqual(BorderStyle.FixedSingle, box.BorderStyle);
        Assert.AreEqual(BorderStyle.None, NativeBorderStyle(box));
    }

    [TestMethod]
    public void DefaultStyle_NavratZVlastnehoStylu_VratiRamikZBorderStyle()
    {
        // poradie ako v InitializeComponent: BorderStyle pred DefaultStyle
        using var box = new ExRichTextBox { BorderStyle = BorderStyle.FixedSingle, DefaultStyle = false };
        Assert.AreEqual(BorderStyle.None, NativeBorderStyle(box));

        box.DefaultStyle = true;

        Assert.AreEqual(BorderStyle.FixedSingle, NativeBorderStyle(box));
    }

    [TestMethod]
    public void DefaultStyle_BezZmenyBorderStyle_VratiPredvolenyRamik()
    {
        using var box = new ExRichTextBox { DefaultStyle = false };

        box.DefaultStyle = true;

        Assert.AreEqual(BorderStyle.Fixed3D, NativeBorderStyle(box));
    }

    // ramik, ktory ma okno naozaj - ExRichTextBox.BorderStyle skryva vlastnost RichTextBox
    private static BorderStyle NativeBorderStyle(RichTextBox box) => box.BorderStyle;
}
