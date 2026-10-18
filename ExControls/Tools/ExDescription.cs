using System.Globalization;

namespace ExControls;

/// <summary>
/// Description of a property or event in the property grid, looked up by its key in the resources of WinForms (the language
/// of the descriptions of the standard properties).
/// </summary>
public class ExDescriptionAttribute : DescriptionAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExDescriptionAttribute" /> class with the description stored under the key;
    /// the key itself is the description if no resource is found.
    /// </summary>
    /// <param name="keyDescription">resource key of the description</param>
    public ExDescriptionAttribute(string keyDescription) : base(KeyToString(keyDescription))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExDescriptionAttribute" /> class.
    /// </summary>
    /// <param name="description">resource key of the description, or the description itself with <paramref name="useExact" /></param>
    /// <param name="useExact"><see langword="true" /> to use <paramref name="description" /> as is</param>
    public ExDescriptionAttribute(string description, bool useExact) : base(useExact ? description : KeyToString(description))
    {
    }

    /// <summary>
    /// Converts key of description to resource string.
    /// </summary>
    /// <param name="key">resource key of the description</param>
    /// <returns>the localized description; the key itself if the resource is not found</returns>
    private static string KeyToString(string key)
    {
        var text = SystemResources.GetString<Form>(key, CultureInfo.CurrentCulture);
        return text ?? key;
    }
}