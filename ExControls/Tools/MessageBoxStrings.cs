using System.Runtime.InteropServices;

// ReSharper disable UnusedMember.Global

namespace ExControls;

/// <summary>
/// This class contains a method to get a localized text for MessageBox's button.
/// </summary>
public static class MessageBoxStrings
{
    /// <summary>
    /// Gets a localized text to specified MessageBox button.
    /// </summary>
    /// <param name="type">Type of MessageBox button.</param>
    /// <returns>The text of the button in the language of Windows; an empty string if it is not found.</returns>
    public static string GetLocalizedString(MessageBoxCmdType type) => Marshal.PtrToStringAuto(Win32.MB_GetString((int)type)) ?? "";
}
