namespace ExControls.Tests.Controls;

/// <summary>
/// Search highlight in ExOptionsPanel: matching labels, check boxes and radio buttons (also nested) get the highlight color
/// and the next search returns their original color - an inherited color stays inherited.
/// </summary>
[TestClass]
public class ExOptionsPanelTests
{
    [TestMethod]
    public void Zvyraznenie_ZodpovedajucePrvky_AjVnorene_BezOhladuNaVelkostPismen()
    {
        using var form = CreatePanel(out var panel, out var label, out var check, out var radio);

        var count = panel.HighlightMatches("TMAV", Color.Yellow);

        Assert.AreEqual(2, count);
        Assert.AreEqual(Color.Yellow, check.BackColor);
        Assert.AreEqual(Color.Yellow, radio.BackColor);
        Assert.AreNotEqual(Color.Yellow, label.BackColor);
    }

    [TestMethod]
    public void Zvyraznenie_PrazdnyText_VratiPovodneFarbyAjZdedenu()
    {
        using var form = CreatePanel(out var panel, out var label, out var check, out _);
        panel.HighlightMatches("a", Color.Yellow);

        var count = panel.HighlightMatches("", Color.Yellow);

        Assert.AreEqual(0, count);
        Assert.AreEqual(Color.Red, label.BackColor);
        Assert.AreEqual(panel.BackColor, check.BackColor);
        panel.BackColor = Color.Green;
        Assert.AreEqual(Color.Green, check.BackColor, "farba zaskrtavacieho pola sa ma znova dedit z panela");
    }

    [TestMethod]
    public void Zvyraznenie_AmpersandMnemoniky_SaIgnoruje()
    {
        using var form = CreatePanel(out var panel, out _, out _, out _);
        var button = new CheckBox { Text = "&Uložiť" };
        panel.Controls.Add(button);

        Assert.AreEqual(1, panel.HighlightMatches("ulož", Color.Yellow));
        Assert.IsTrue(panel.Search("ulož"));
    }

    [TestMethod]
    public void Hladanie_NazovStranky_BezOhladuNaVelkostPismen()
    {
        using var form = CreatePanel(out var panel, out _, out _, out _);
        panel.NodeText = "Farebná schéma";

        Assert.IsTrue(panel.Search("SCHÉMA"));
    }

    private static Form CreatePanel(out ExOptionsPanel panel, out Label label, out CheckBox check, out RadioButton radio)
    {
        var form = new Form();
        panel = new ExOptionsPanel { BackColor = Color.White, Node = new OptionsNode() };
        label = new Label { Text = "Jazyk a oblasť", BackColor = Color.Red };
        check = new CheckBox { Text = "Tmavé záhlavie" };
        var nested = new Panel();
        radio = new RadioButton { Text = "Tmavá téma" };
        nested.Controls.Add(radio);
        panel.Controls.AddRange([label, check, nested]);
        form.Controls.Add(panel);
        return form;
    }
}
