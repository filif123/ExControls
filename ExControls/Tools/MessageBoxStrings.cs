using System.Runtime.InteropServices;

namespace ExControls;

/// <summary>
///     This class contains a method to get a localized text for MessageBox's button.
/// </summary>
public static class MessageBoxStrings
{
    /// <summary>
    ///     Gets a localized text to specified MessageBox button.
    /// </summary>
    /// <param name="type">Type of MessageBox button.</param>
    /// <returns></returns>
    public static string GetLocalizedString(MessageBoxCmdType type) => Marshal.PtrToStringAuto(Win32.MB_GetString((int)type));
}

/// <summary>
///     Represents possible MessageBox command types.
/// </summary>
public enum MessageBoxCmdType
{
    /// <summary>
    ///     OK button.
    /// </summary>
    OK = 0,

    /// <summary>
    ///     Cancel button.
    /// </summary>
    Cancel = 1,

    /// <summary>
    ///     Abort button.
    /// </summary>
    Abort = 2,

    /// <summary>
    ///     Retry button.
    /// </summary>
    Retry = 3,

    /// <summary>
    ///     Ignore button.
    /// </summary>
    Ignore = 4,

    /// <summary>
    ///     Yes button.
    /// </summary>
    Yes = 5,

    /// <summary>
    ///     No button.
    /// </summary>
    No = 6,

    /// <summary>
    ///     Close button.
    /// </summary>
    Close = 7,

    /// <summary>
    ///     Help button.
    /// </summary>
    Help = 8,

    /// <summary>
    ///     Try Again button.
    /// </summary>
    TryAgain = 9,

    /// <summary>
    ///     Continue button.
    /// </summary>
    Continue = 10
}