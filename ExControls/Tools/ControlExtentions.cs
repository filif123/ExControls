namespace ExControls;

/// <summary>
/// 
/// </summary>
public static class ControlExtentions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cb"></param>
    /// <param name="dictionary"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void BindEnum<T>(this ComboBox cb, Dictionary<T, string> dictionary) where T : Enum
    {
        if (dictionary == null)
            throw new ArgumentNullException(nameof(dictionary));
        if (cb == null)
            throw new ArgumentNullException(nameof(cb));

        cb.DataSource = dictionary.ToList();
        cb.DisplayMember = "Value";
        cb.ValueMember = "Key";
    }
}