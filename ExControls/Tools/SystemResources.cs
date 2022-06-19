using System.Globalization;
using System.Resources;

namespace ExControls;

/// <summary>
/// 
/// </summary>
public static class SystemResources
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="culture"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static string GetString<T>(string key, CultureInfo culture)
    {
        var baseName = typeof(T).Namespace;
        if (baseName == null) 
            return null;

        var resources = new ResourceManager(baseName, typeof(T).Assembly);
        return resources.GetString(key, culture);
    }
}