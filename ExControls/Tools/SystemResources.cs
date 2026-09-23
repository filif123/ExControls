using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace ExControls;

/// <summary>
/// Reads localized strings from the resources of system assemblies (e.g. System.Windows.Forms).
/// </summary>
public static class SystemResources
{
    private static readonly ConcurrentDictionary<Assembly, ResourceManager?> Managers = new();

    /// <summary>
    /// Returns the resource string with the specified key from the assembly containing type <typeparamref name="T"/>,
    /// or <see langword="null"/> if the string (or the whole resource table) was not found.
    /// </summary>
    /// <remarks>
    /// Used by attribute constructors (<see cref="ExCategoryAttribute"/>, <see cref="ExDescriptionAttribute"/>), so it must never throw -
    /// an exception there makes TypeDescriptor silently drop all attributes of the member
    /// (DesignerSerializationVisibility, DefaultValue, Browsable...) and the designer then serializes the control wrongly.
    /// </remarks>
    /// <param name="key">Resource key.</param>
    /// <param name="culture">Culture of the string.</param>
    /// <typeparam name="T">Any type from the assembly with the resources.</typeparam>
    public static string? GetString<T>(string key, CultureInfo culture)
    {
        var manager = Managers.GetOrAdd(typeof(T).Assembly, _ => CreateManager(typeof(T)));
        if (manager is null)
            return null;

        try
        {
            return manager.GetString(key, culture);
        }
        catch (MissingManifestResourceException)
        {
            return null;
        }
    }

    private static ResourceManager? CreateManager(Type type)
    {
        // .NET Framework: resources "System.Windows.Forms.resources"; .NET: "System.SR.resources"
        var names = type.Assembly.GetManifestResourceNames();
        foreach (var baseName in new[] { type.Namespace, "System.SR" })
        {
            if (baseName is not null && names.Contains(baseName + ".resources"))
                return new ResourceManager(baseName, type.Assembly);
        }

        return null;
    }
}
