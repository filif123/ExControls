using System.Reflection;

namespace ExControls;

/// <summary>
/// Extension methods for controls.
/// </summary>
public static class ControlExtensions
{
    /// <summary>
    /// Binds the combo box to enum values: the items show the texts from <paramref name="dictionary" /> and
    /// <see cref="ListControl.SelectedValue" /> is the enum value.
    /// </summary>
    /// <typeparam name="T">type of the enum</typeparam>
    /// <param name="cb">combo box to bind</param>
    /// <param name="dictionary">enum values with their display texts, in the order of the items</param>
    /// <exception cref="ArgumentNullException">
    /// when <paramref name="cb" /> or <paramref name="dictionary" /> is <see langword="null" />.
    /// </exception>
    public static void BindEnum<T>(this ComboBox cb, Dictionary<T, string> dictionary) where T : Enum
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        ArgumentNullException.ThrowIfNull(cb);

        cb.DataSource = dictionary.ToList();
        cb.DisplayMember = "Value";
        cb.ValueMember = "Key";
    }

    internal static Win32.WM GetMsg(this Message msg)
    {
        return (Win32.WM)msg.Msg;
    }

    internal static IntPtr GetHbrush(this Brush b)
    {
        var field = typeof(Brush).GetField("nativeBrush", BindingFlags.NonPublic | BindingFlags.Instance);
        if (field is not null)
            return (IntPtr) field.GetValue(b)!;
        return IntPtr.Zero;
    }
}