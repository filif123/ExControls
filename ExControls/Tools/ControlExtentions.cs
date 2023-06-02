using System.Reflection;

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