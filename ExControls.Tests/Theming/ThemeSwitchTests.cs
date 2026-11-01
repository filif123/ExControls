using System.Collections;
using System.Reflection;

namespace ExControls.Tests.Theming;

/// <summary>
/// Switching the theme at run time: a control that had one theme and then got another one looks the same as a control that
/// only ever had the second theme (in both directions - the custom style and the system one). Compared are the public properties of the control and of its style objects;
/// properties that differ between two fresh controls (handles, counters) are ignored.
/// </summary>
[STATestClass]
public class ThemeSwitchTests
{
    private static readonly ExTheme Dark = new()
    {
        DarkScrollBars = true,
        PanelBackColor = Color.FromArgb(30, 30, 30), PanelForeColor = Color.White,
        BoxBackColor = Color.Black, BoxForeColor = Color.White,
        ButtonBackColor = Color.FromArgb(60, 60, 60), ButtonForeColor = Color.Gainsboro,
        BorderColor = Color.Gray, HighlightBackColor = Color.SteelBlue, HighlightForeColor = Color.White,
        MarkColor = Color.Orange, LabelForeColor = Color.Silver
    };

    private static readonly ExTheme Light = new() { UseSystemStyle = true };

    public static IEnumerable<object[]> ThemeableTypes => LoadableTypes()
        .Where(t => typeof(IThemeable).IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false, IsPublic: true }
                    && t.GetConstructor(Type.EmptyTypes) is not null)
        .OrderBy(t => t.Name, StringComparer.Ordinal)
        .Select(t => new object[] { t });

    [TestMethod]
    [DynamicData(nameof(ThemeableTypes))]
    public void TmavaPotomSvetla_RovnakoAkoLenSvetla(Type type) => AssertSwitch(type, Dark, Light);

    [TestMethod]
    [DynamicData(nameof(ThemeableTypes))]
    public void SvetlaPotomTmava_RovnakoAkoLenTmava(Type type) => AssertSwitch(type, Light, Dark);

    private static void AssertSwitch(Type type, ExTheme first, ExTheme second)
    {
        var fresh = Create(type);
        var other = Create(type);
        var switched = Create(type);
        try
        {
            Apply(fresh, second);
            Apply(other, second);
            Apply(switched, first);
            // program pouzije novu temu aj viackrat (vsetky okna, potom hlavne okno znova)
            Apply(switched, second);
            Apply(switched, second);

            var expected = Dump(fresh);
            var noise = Dump(other);
            var actual = Dump(switched);
            var differences = expected.Keys
                .Where(key => expected[key] == Value(noise, key) && expected[key] != Value(actual, key))
                .Select(key => $"{key}: {expected[key]} -> {Value(actual, key)}")
                .ToList();

            Assert.AreEqual(0, differences.Count, string.Join(Environment.NewLine, differences));
        }
        finally
        {
            foreach (var item in new[] { fresh, other, switched })
                (item as IDisposable)?.Dispose();
        }
    }

    [TestMethod]
    public void ExButton_PoTmavejTeme_VratiFlatStyleZNavrhu()
    {
        using var button = new ExButton { FlatStyle = FlatStyle.System };

        ExThemer.Apply(button, Dark);
        Assert.AreEqual(FlatStyle.Flat, button.FlatStyle);
        ExThemer.Apply(button, Light);

        Assert.AreEqual(FlatStyle.System, button.FlatStyle);
        Assert.IsTrue(button.UseVisualStyleBackColor);
    }

    private sealed class MarkerMenu : ContextMenuStrip;

    [TestMethod]
    public void ExDropDownButton_TemaPrefarbiAjPonukuTlacidla()
    {
        // ponuku styluje obsluha aplikacie (ToolsCore), tu znacka typu menu
        ExThemer.Register<MarkerMenu>((menu, theme) => menu.BackColor = theme.PanelBackColor);
        using var menu = new MarkerMenu();
        using var button = new ExDropDownButton { DropDownMenu = menu };

        ExThemer.Apply(button, Dark);

        Assert.AreEqual(Dark.PanelBackColor, menu.BackColor);
    }

    [TestMethod]
    public void KeepDefaults_ZdedenaFarba_PoSvetlejTemeSaZnovaDedi()
    {
        using var parent = new Panel { BackColor = Color.Red };
        var button = new ExButton();
        parent.Controls.Add(button);

        ExThemer.Apply(button, Dark);
        ExThemer.Apply(button, Light);
        parent.BackColor = Color.Green;

        Assert.AreEqual(Color.Green, button.BackColor);
    }

    [TestMethod]
    public void KeepDefaults_SvetlaTemaDvakrat_ZdedenaFarbaOstaneZdedena()
    {
        using var parent = new Panel { BackColor = Color.Red };
        var button = new ExButton();
        parent.Controls.Add(button);

        ExThemer.Apply(button, Dark);
        ExThemer.Apply(button, Light);
        ExThemer.Apply(button, Light);
        parent.BackColor = Color.Green;

        Assert.AreEqual(Color.Green, button.BackColor);
    }

    // niektore typy kniznice odkazuju na kniznice navrhara, ktore testy nemaju
    private static IEnumerable<Type> LoadableTypes()
    {
        try
        {
            return typeof(IThemeable).Assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException e)
        {
            return e.Types.OfType<Type>();
        }
    }

    // GetValueOrDefault nie je v .NET Framework
    private static string? Value(Dictionary<string, string> dump, string key) => dump.TryGetValue(key, out var value) ? value : null;

    private static object Create(Type type)
    {
        try
        {
            return Activator.CreateInstance(type)!;
        }
        catch (TargetInvocationException e) when (e.InnerException is not null)
        {
            System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(e.InnerException).Throw();
            throw;
        }
    }

    private static void Apply(object item, ExTheme theme)
    {
        // v okne maju prvky pri aplikovani temy vytvorene handle (niektore sa podla neho prisposobia, napr. vyska zoznamu)
        if (item is Control control)
        {
            control.CreateControl();
            ExThemer.Apply(control, theme);
        }
        else
            ((IThemeable)item).ApplyTheme(theme);
    }

    private static Dictionary<string, string> Dump(object item)
    {
        var result = new Dictionary<string, string>(StringComparer.Ordinal);
        Dump(item, item.GetType().Name, 0, result);
        return result;
    }

    private static void Dump(object item, string path, int depth, Dictionary<string, string> result)
    {
        foreach (var property in item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!property.CanRead || property.GetIndexParameters().Length > 0 || property.IsDefined(typeof(ObsoleteAttribute)))
                continue;

            object? value;
            try
            {
                value = property.GetValue(item);
            }
            catch (TargetInvocationException)
            {
                continue;
            }

            var key = path + "." + property.Name;
            switch (value)
            {
                case null:
                    result[key] = "null";
                    break;
                case string or Color or Enum or Size or Point or Padding or Rectangle or bool or int or float or double or decimal:
                    result[key] = Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture) ?? "";
                    break;
                case Font font:
                    result[key] = font.ToString();
                    break;
                // casti prvku z kniznice (styly, vzhlad tlacidla, kalendar vyberu datumu)
                case not IEnumerable when depth < 2 && value.GetType().Namespace == typeof(IThemeable).Namespace
                                          && !ReferenceEquals(value, item):
                    Dump(value, key, depth + 1, result);
                    break;
            }
        }
    }
}
