using System.Globalization;

namespace ExControls;

/// <inheritdoc />
public class ExDescriptionAttribute : DescriptionAttribute
{
    /// <inheritdoc />
    public ExDescriptionAttribute(string keyDescription) : base(KeyToString(keyDescription))
    {
    }

    /// <inheritdoc />
    public ExDescriptionAttribute(string description, bool useExact) : base(useExact ? description : KeyToString(description))
    {
    }

    /// <summary>
    /// Converts key of description to resource string.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private static string KeyToString(string key)
    {
        var text = SystemResources.GetString<Form>(key, CultureInfo.CurrentCulture);
        return text ?? key;
    }
}