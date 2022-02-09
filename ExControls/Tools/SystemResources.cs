using System.Globalization;
using System.Resources;

namespace ExControls;

public class SystemResources
{
    public static string GetString<T>(string key, CultureInfo culture)
    {
        var baseName = typeof(T).Namespace;
        if (baseName != null)
        {
            var resources = new ResourceManager(baseName, typeof(T).Assembly);
            return resources.GetString(key, culture);
        }

        return null;
    }
}