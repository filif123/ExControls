using System.Globalization;
using System.Xml;
using System.Xml.Linq;

namespace ExControls.Docking;

/// <summary>
/// Zapis a citanie rozlozenia <see cref="DockLayoutData" /> ako XML na jednom riadku (program si ho ulozi napr. do svojej
/// konfiguracie). Nazvy stran a atributov su pevne kluce, nie nazvy z C#. Verzia 1 nemala vnorene rozdelenia
/// (<c>split</c>), cita sa rovnako.
/// </summary>
internal static class DockLayoutXml
{
    /// <summary>
    /// Verzia formatu.
    /// </summary>
    public const int Version = 2;

    private const string RootName = "dockLayout";
    private const string ZoneName = "zone";
    private const string GroupName = "group";
    private const string SplitName = "split";
    private const string CenterName = "center";
    private const string DocumentName = "document";
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
            element.Add(zone.Nodes.Select(WriteNode));
            root.Add(element);
        }

        if (data.Center is { } center)
            root.Add(new XElement(CenterName, center.Select(WriteNode)));

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
        if (version is < 1 or > Version)
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
                    zone.Nodes.AddRange(ParseNodes(element, keys, false));
                    data.Zones.Add(zone);
                    break;
                case CenterName:
                    if (data.Center is not null)
                        throw new FormatException("The dock layout has the center twice.");
                    data.Center = ParseNodes(element, keys, true).ToList();
                    if (data.Center.Sum(CountDocuments) > 1)
                        throw new FormatException("The center of the dock layout has more documents.");
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

    private static int CountDocuments(DockLayoutData.NodeData node) => node switch
    {
        DockLayoutData.DocumentData => 1,
        DockLayoutData.SplitData split => split.Children.Sum(CountDocuments),
        _ => 0
    };

    private static XElement WriteNode(DockLayoutData.NodeData node)
    {
        if (node is DockLayoutData.DocumentData document)
        {
            var documentElement = new XElement(DocumentName);
            WriteWeight(documentElement, document);
            return documentElement;
        }

        if (node is not DockLayoutData.SplitData split)
            return WriteGroup((DockLayoutData.GroupData)node);

        var element = new XElement(SplitName, new XAttribute("arrange", split.SideBySide ? "columns" : "rows"));
        WriteWeight(element, split);
        element.Add(split.Children.Select(WriteNode));
        return element;
    }

    private static void WriteWeight(XElement element, DockLayoutData.NodeData node)
    {
        if (Math.Abs(node.Weight - 1) > 1e-9)
            element.Add(new XAttribute("weight", node.Weight.ToString("R", CultureInfo.InvariantCulture)));
    }

    private static XElement WriteGroup(DockLayoutData.GroupData group)
    {
        var element = new XElement(GroupName);
        WriteWeight(element, group);
        if (group.Side is { } side)
            element.Add(new XAttribute("side", SideKey(side)));
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

    // uzly zony, stredu alebo rozdelenia; ine prvky sa preskocia, dokument je len v strede
    private static IEnumerable<DockLayoutData.NodeData> ParseNodes(XElement parent, HashSet<string> keys, bool center)
    {
        foreach (var element in parent.Elements())
        {
            switch (element.Name.LocalName)
            {
                case GroupName:
                    yield return ParseGroup(element, keys);
                    break;
                case DocumentName:
                    if (!center)
                        throw new FormatException("The document of the dock layout can only be in the center.");
                    yield return new DockLayoutData.DocumentData { Weight = OptionalDouble(element, "weight", 1) };
                    break;
                case SplitName:
                    var split = new DockLayoutData.SplitData
                    {
                        Weight = OptionalDouble(element, "weight", 1),
                        SideBySide = element.Attribute("arrange")?.Value switch
                        {
                            "columns" => true,
                            "rows" => false,
                            var value => throw new FormatException($"Unknown arrangement '{value}' of a split in the dock layout.")
                        }
                    };
                    split.Children.AddRange(ParseNodes(element, keys, center));
                    yield return split;
                    break;
            }
        }
    }

    private static DockLayoutData.GroupData ParseGroup(XElement element, HashSet<string> keys)
    {
        var group = new DockLayoutData.GroupData
        {
            Weight = OptionalDouble(element, "weight", 1),
            AutoHide = OptionalBool(element, "autoHide"),
            AutoHideSize = element.Attribute("autoHideSize") is null ? 0 : NonNegative(element, "autoHideSize"),
            Active = element.Attribute("active")?.Value,
            Side = element.Attribute("side") is null ? null : ParseSide(element)
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
