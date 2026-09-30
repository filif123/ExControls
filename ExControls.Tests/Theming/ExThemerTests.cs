namespace ExControls.Tests.Theming;

/// <summary>
/// Pouzitie temy: Ex* prvky sa nastyluju samy, ostatne podla zaregistrovanej obsluhy najblizsieho predka.
/// Obsluhy su globalne - testy registruju len vlastne typy prvkov.
/// </summary>
[TestClass]
public class ExThemerTests
{
    private sealed class TestPanel : Panel;

    private sealed class DerivedPanel : Panel;

    private sealed class NestedPanel : UserControl;

    private sealed class Marker : Control;

    private static readonly ExTheme Dark = new()
    {
        PanelBackColor = Color.FromArgb(30, 30, 30), PanelForeColor = Color.White,
        ButtonBackColor = Color.FromArgb(60, 60, 60), ButtonForeColor = Color.Gainsboro,
        BorderColor = Color.Gray, HighlightBackColor = Color.SteelBlue, BoxBackColor = Color.Black, BoxForeColor = Color.White
    };

    [ClassInitialize]
    public static void Register(TestContext _)
    {
        ExThemer.Register<TestPanel>((panel, theme) =>
        {
            ExThemer.Apply(panel.Controls, theme);
            panel.BackColor = theme.PanelBackColor;
        });
        ExThemer.Register<Marker>((marker, theme) => marker.ForeColor = theme.HighlightBackColor);
    }

    [TestMethod]
    public void Apply_ExPrvok_NastylujeSaSamPodlaTemy()
    {
        using var button = new ExButton();

        ExThemer.Apply(button, Dark);

        Assert.IsFalse(button.DefaultStyle);
        Assert.AreEqual(Dark.ButtonBackColor, button.BackColor);
        Assert.AreEqual(Dark.HighlightBackColor, button.ExFlatAppearance.MouseOverBackColor);
    }

    [TestMethod]
    public void Apply_SystemovyVzhlad_ExPrvokSaNeprefarbi()
    {
        using var button = new ExButton();
        var before = button.BackColor;

        ExThemer.Apply(button, new ExTheme { UseSystemStyle = true, ButtonBackColor = Color.Red });

        Assert.IsTrue(button.DefaultStyle);
        Assert.AreEqual(before, button.BackColor);
    }

    [TestMethod]
    public void Apply_ObsluhaKontajnera_PrejdeVnorenePrvky()
    {
        using var panel = new TestPanel();
        var marker = new Marker();
        var button = new ExButton();
        panel.Controls.Add(marker);
        panel.Controls.Add(button);

        ExThemer.Apply(new Control[] { panel }, Dark);

        Assert.AreEqual(Dark.PanelBackColor, panel.BackColor);
        Assert.AreEqual(Dark.HighlightBackColor, marker.ForeColor);
        Assert.AreEqual(Dark.ButtonBackColor, button.BackColor);
    }

    [TestMethod]
    public void Apply_PrvokBezObsluhy_ZostaneAjSVnorenymiPrvkami()
    {
        using var nested = new NestedPanel();
        var marker = new Marker();
        nested.Controls.Add(marker);
        var back = nested.BackColor;
        var fore = marker.ForeColor;

        ExThemer.Apply(nested, Dark);

        Assert.AreEqual(back, nested.BackColor);
        Assert.AreEqual(fore, marker.ForeColor);
    }

    [TestMethod]
    public void Apply_OdvodenyTypBezVlastnejObsluhy_PouzijeObsluhuPredka()
    {
        // Panel ma obsluhu len v aplikacii (ToolsCore) - tu sa zaregistruje obsluha pre typ medzi nimi
        ExThemer.Register<Panel>((panel, theme) => panel.ForeColor = theme.PanelForeColor);
        using var derived = new DerivedPanel();

        ExThemer.Apply(derived, Dark);

        Assert.AreEqual(Dark.PanelForeColor, derived.ForeColor);
    }
}
