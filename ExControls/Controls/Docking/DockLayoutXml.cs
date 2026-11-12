using System.Globalization;
using System.Xml;
using System.Xml.Linq;

namespace ExControls.Docking;

/// <summary>
/// Zapis a citanie rozlozenia <see cref="DockLayoutData" /> ako XML na jednom riadku (program si ho ulozi napr. do svojej
/// konfiguracie). Nazvy stran a atributov su pevne kluce, nie nazvy z C#.
/// </summary>
internal static class DockLayoutXml
{
    /// <summary>
    /// Verzia formatu.
    /// </summary>
    public const int Version = 1;

    private const string RootName = "dockLayout";
    private const string ZoneName = "zone";
    private const string GroupName = "group";
    private const string PanelName = "panel";
    private const string FloatName = "float";

    /// <summary>
    /// Zapise rozlozenie.
    /// </summary>
    public static string Write(DockLayoutData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        var root = new XElement(RootName, new XAttribute("version", Version));
        foreach (var zone in data.Zones)
        {
            var element = new XElement(ZoneName, new XAttribute("side", SideKey(zone.Side)), new XAttribute("size", Int(zone.Size)));
            element.Add(zone.Groups.Select(WriteGroup));
            root.Add(element);
        }

        foreach (var window in data.Floats)
            root.Add(new XElement(FloatName,
                new XAttribute("x", Int(window.Bounds.X)),
                new XAttribute("y", Int(window.Bounds.Y)),
                new XAttribute("width", Int(window.Bounds.Width)),
                new XAttribute("height", Int(window.Bounds.Height)),
                new XAttribute("side", SideKey(window.Side)),
                WriteGroup(window.Group)));

        return root.ToString(SaveOptions.DisableFormatting);
    }

    /// <summary>
    /// Precita rozlozenie.
    /// </summary>
    /// <exception cref="FormatException">text nie je rozlozenie tejto verzie</exception>
    public static DockLayoutData Parse(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        XElement root;
        try
        {
            root = XElement.Parse(text);
        }
        catch (XmlException e)
        {
            throw new FormatException("The dock layout is not valid XML.", e);
        }

        if (root.Name.LocalName != RootName)
            throw new FormatException($"The dock layout must have the root element {RootName}.");
        var version = OptionalInt(root, "version", 0);
        if (version != Version)
            throw new FormatException($"Unsupported dock layout version {version}.");

        var data = new DockLayoutData();
        var keys = new HashSet<string>(StringComparer.Ordinal);
        var sides = new HashSet<ExDockSide>();
        foreach (var element in root.Elements())
        {
            switch (element.Name.LocalName)
            {
                case ZoneName:
                    var zone = new DockLayoutData.ZoneData { Side = ParseSide(element), Size = NonNegative(element, "size") };
                    if (!sides.Add(zone.Side))
                        throw new FormatException($"The dock layout has the zone {SideKey(zone.Side)} twice.");
                    foreach (var group in element.Elements(GroupName))
                        zone.Groups.Add(ParseGroup(group, keys));
                    data.Zones.Add(zone);
                    break;
                case FloatName:
                    var groupElement = element.Element(GroupName) ?? throw new FormatException("A floating window has no group.");
                    data.Floats.Add(new DockLayoutData.FloatData
                    {
                        Bounds = new Rectangle(RequiredInt(element, "x"), RequiredInt(element, "y"), NonNegative(element, "width"),
                            NonNegative(element, "height")),
                        Side = element.Attribute("side") is null ? ExDockSide.Right : ParseSide(element),
                        Group = ParseGroup(groupElement, keys)
                    });
                    break;
            }
        }

        return data;
    }

    private static XElement WriteGroup(DockLayoutData.GroupData group)
    {
        var element = new XElement(GroupName);
        if (Math.Abs(group.Weight - 1) > 1e-9)
            element.Add(new XAttribute("weight", group.Weight.ToString("R", CultureInfo.InvariantCulture)));
        if (group.AutoHide)
            element.Add(new XAttribute("autoHide", "true"));
        if (group.AutoHideSize > 0)
            element.Add(new XAttribute("autoHideSize", Int(group.AutoHideSize)));
        if (group.Active is { Length: > 0 } active)
            element.Add(new XAttribute("active", active));
        foreach (var panel in group.Panels)
        {
            var panelElement = new XElement(PanelName, new XAttribute("key", panel.Key));
            if (panel.Hidden)
                panelElement.Add(new XAttribute("hidden", "true"));
            element.Add(panelElement);
        }

        return element;
    }

    private static DockLayoutData.GroupData ParseGroup(XElement element, HashSet<string> keys)
    {
        var group = new DockLayoutData.GroupData
        {
            Weight = OptionalDouble(element, "weight", 1),
            AutoHide = OptionalBool(element, "autoHide"),
            AutoHideSize = element.Attribute("autoHideSize") is null ? 0 : NonNegative(element, "autoHideSize"),
            Active = element.Attribute("active")?.Value
        };
        foreach (var panel in element.Elements(PanelName))
        {
            var key = panel.Attribute("key")?.Value;
            if (string.IsNullOrEmpty(key))
                throw new FormatException("A panel of the dock layout has no key.");
            if (!keys.Add(key!))
                throw new FormatException($"The dock layout has the panel {key} twice.");
            group.Panels.Add(new DockLayoutData.PanelData(key!, OptionalBool(panel, "hidden")));
        }

        return group;
    }

    /// <summary>
    /// Kluc strany v subore.
    /// </summary>
    public static string SideKey(ExDockSide side) => side switch
    {
        ExDockSide.Left => "left",
        ExDockSide.Top => "top",
        ExDockSide.Right => "right",
        _ => "bottom"
    };

    private static ExDockSide ParseSide(XElement element) => element.Attribute("side")?.Value switch
    {
        "left" => ExDockSide.Left,
        "top" => ExDockSide.Top,
        "right" => ExDockSide.Right,
        "bottom" => ExDockSide.Bottom,
        var value => throw new FormatException($"Unknown side '{value}' in the dock layout.")
    };

    private static string Int(int value) => value.ToString(CultureInfo.InvariantCulture);

    private static int RequiredInt(XElement element, string name)
    {
        var value = element.Attribute(name)?.Value;
        return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result)
            ? result
            : throw new FormatException($"The attribute {name} of {element.Name.LocalName} must be an integer.");
    }

    private static int NonNegative(XElement element, string name)
    {
        var value = RequiredInt(element, name);
        return value >= 0 ? value : throw new FormatException($"The attribute {name} of {element.Name.LocalName} must not be negative.");
    }

    private static int OptionalInt(XElement element, string name, int fallback) =>
        element.Attribute(name) is null ? fallback : RequiredInt(element, name);

    private static double OptionalDouble(XElement element, string name, double fallback)
    {
        var value = element.Attribute(name)?.Value;
        if (value is null)
            return fallback;
        return double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var result) && result > 0 && !double.IsInfinity(result)
            ? result
            : throw new FormatException($"The attribute {name} of {element.Name.LocalName} must be a positive number.");
    }

    private static bool OptionalBool(XElement element, string name) => element.Attribute(name)?.Value switch
    {
        null or "false" => false,
        "true" => true,
        var value => throw new FormatException($"The attribute {name} of {element.Name.LocalName} must be true or false, not '{value}'.")
    };
}
