using System.Diagnostics;
using Microsoft.Win32;
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable InconsistentNaming

namespace ExControls;

/// <summary>
///     Contains various stuff for controls and forms.
/// </summary>
public static class ExTools
{
    private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
    private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

    private const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;

    private const int DWMWA_TEXT_COLOR = 34;
    private const int DWMWA_CAPTION_COLOR = 35;
    private const int DWMWA_BORDER_COLOR = 36;

    private const int DWMWA_VISIBLE_FRAME_BORDER_THICKNESS = 37;

    private const int DWMWA_MICA_EFFECT = 1029;
    private const int DWMWA_SYSTEMBACKDROP_TYPE = 1029;

    /// <summary>
    ///  Gets 
    /// </summary>
    public static bool IsWin11Build22000 { get; } = IsWindows10OrGreater(22000);

    private static bool IsWindows10OrGreater(int build = -1)
    {
        var ver = HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName");
        if (int.TryParse(HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentBuild"), out var cb))
            return ver.StartsWith("Windows 10") && cb >= build;
        return false;
    }

    private static string HKLM_GetString(string path, string key)
    {
        try
        {
            var rk = Registry.LocalMachine.OpenSubKey(path);
            if (rk == null) return "";
            return (string)rk.GetValue(key);
        }
        catch
        {
            return "";
        }
    }

    /// <summary>
    ///     Changes background color of window's caption (immersive dark mode). <br></br>
    ///     Works only in Windows 10+.
    /// </summary>
    /// <param name="form">form where dark mode is applying</param>
    /// <param name="enabled">whether to turn dark mode on or off</param>
    /// <returns>whether dark mode has been successfully activated/deactivated</returns>
    public static bool SetImmersiveDarkMode(this Form form, bool enabled)
    {
        return SetImmersiveDarkMode(form.Handle, enabled);
    }

    /// <summary>
    ///     Changes background color of window's caption (immersive dark mode). <br></br>
    ///     Works only in Windows 10+.
    /// </summary>
    /// <param name="handle">handle to form where dark mode is applying</param>
    /// <param name="enabled">whether to turn dark mode on or off</param>
    /// <returns>whether dark mode has been successfully activated/deactivated</returns>
    public static bool SetImmersiveDarkMode(IntPtr handle, bool enabled)
    {
        if (!IsWindows10OrGreater(17763)) 
            return false;
        var attribute = DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1;
        if (IsWindows10OrGreater(18985)) 
            attribute = DWMWA_USE_IMMERSIVE_DARK_MODE;

        var useImmersiveDarkMode = enabled ? 1 : 0;
        return Win32.DwmSetWindowAttribute(handle, attribute, ref useImmersiveDarkMode, sizeof(int)) == 0;

    }

    /// <summary>
    ///     Changes background color of window's caption and border color. <br></br>
    ///     Works only in Windows 11+.
    /// </summary>
    /// <param name="handle">handle to form where the change is applying</param>
    /// <param name="titlebarColor">color of caption</param>
    /// <param name="textColor">color of text in the caption</param>
    /// <param name="borderColor">color of border</param>
    /// <returns>whether dark mode has been successfully activated/deactivated</returns>
    public static bool SetTitlebarAndBorderColor(IntPtr handle, Color titlebarColor = default, Color textColor = default, Color borderColor = default)
    {
        if (!IsWin11Build22000)
            return false;

        if (titlebarColor != default)
        {
            var titlebarColorNative = ColorTranslator.ToWin32(titlebarColor);
            Win32.DwmSetWindowAttribute(handle, DWMWA_CAPTION_COLOR, ref titlebarColorNative, sizeof(int));
        }

        if (textColor != default)
        {
            var textColorNative = ColorTranslator.ToWin32(textColor);
            Win32.DwmSetWindowAttribute(handle, DWMWA_TEXT_COLOR, ref textColorNative, sizeof(int));
        }

        if (borderColor != default)
        {
            var borderColorNative = ColorTranslator.ToWin32(borderColor);
            Win32.DwmSetWindowAttribute(handle, DWMWA_BORDER_COLOR, ref borderColorNative, sizeof(int));
        }

        return true;
    }

    /// <summary>
    ///     Changes background color of window's caption and border color. <br></br>
    ///     Works only in Windows 11+.
    /// </summary>
    /// <param name="form">form where the change is applying</param>
    /// <param name="titlebarColor">color of caption</param>
    /// <param name="textColor">color of text in the caption</param>
    /// <param name="borderColor">color of border</param>
    /// <returns>whether dark mode has been successfully activated/deactivated</returns>
    public static bool SetTitlebarAndBorderColor(this Form form, Color titlebarColor = default, Color textColor = default, Color borderColor = default)
    {
        return SetTitlebarAndBorderColor(form.Handle, titlebarColor, borderColor, textColor);
    }

    /// <summary>
    ///     Enables or disables Mica effect to the form.
    ///     Works only in Windows 11+.
    /// </summary>
    /// <param name="handle">handle to form where change is applying</param>
    /// <param name="style">style of form (between builds 22000 and 22523 works only Default and Mica style).</param>
    /// <returns>whether Mica effect has been successfully activated/deactivated</returns>
    public static bool SetFormStyle(IntPtr handle, FormStyle style)
    {
        if (!IsWin11Build22000)
            return false;

        var useNewSetting = IsWindows10OrGreater(22523);
        if (!useNewSetting)
        {
            var useMica = style == FormStyle.Mica ? 1 : 0;
            return Win32.DwmSetWindowAttribute(handle, DWMWA_MICA_EFFECT, ref useMica, sizeof(int)) == 0;
        }

        var styleInt = (int) style;
        return Win32.DwmSetWindowAttribute(handle, DWMWA_SYSTEMBACKDROP_TYPE, ref styleInt, sizeof(int)) == 0;
    }

    /// <summary>
    ///     Enables or disables Mica effect to the form.
    ///     Works only in Windows 11+.
    /// </summary>
    /// <param name="form">form where the change is applying</param>
    /// <param name="style">whether to turn Mica effect on or off</param>
    /// <returns>whether Mica effect has been successfully activated/deactivated</returns>
    public static bool SetFormStyle(this Form form, FormStyle style)
    {
        return SetFormStyle(form.Handle, style);
    }

    /// <summary>
    ///     Sets type of form's corners.
    ///     Works only in Windows 11+.
    /// </summary>
    /// <param name="handle">handle to form where change is applying</param>
    /// <param name="type">type of corners</param>
    /// <returns>whether change has been successfully applied</returns>
    public static bool SetFormCorners(IntPtr handle, FormCornersType type)
    {
        if (!IsWin11Build22000)
            return false;

        var typeInt = (int)type;
        return Win32.DwmSetWindowAttribute(handle, DWMWA_WINDOW_CORNER_PREFERENCE, ref typeInt, sizeof(int)) == 0;
    }

    /// <summary>
    ///     Sets type of form's corners.
    ///     Works only in Windows 11+.
    /// </summary>
    /// <param name="form">form where the change is applying</param>
    /// <param name="type">type of corners</param>
    /// <returns>whether change has been successfully applied</returns>
    public static bool SetFormCorners(this Form form, FormCornersType type)
    {
        return SetFormCorners(form.Handle, type);
    }

    /// <summary>
    ///     Gets width of the form's border in points.
    ///     Works only in Windows 11+.
    /// </summary>
    /// <param name="handle">handle to form where change is applying</param>
    /// <param name="sizeInPoints">width of border in points</param>
    /// <returns></returns>
    public static bool GetFormBorderThickness(IntPtr handle, out int sizeInPoints)
    {
        sizeInPoints = 0;
        if (!IsWin11Build22000)
            return false;

        return Win32.DwmGetWindowAttribute(handle, DWMWA_VISIBLE_FRAME_BORDER_THICKNESS, out sizeInPoints, sizeof(uint)) == 0;
    }

    /// <summary>
    ///     Gets width of the form's border in points.
    ///     Works only in Windows 11+.
    /// </summary>
    /// <param name="form">form where the change is applying</param>
    /// <param name="sizeInPoints">width of border in points</param>
    /// <returns></returns>
    public static bool GetFormBorderThickness(this Form form, out int sizeInPoints)
    {
        return GetFormBorderThickness(form.Handle, out sizeInPoints);
    }

    /// <summary>
    ///     Sets theme for the control. Works only on some controls.
    /// </summary>
    /// <param name="control">editing control</param>
    /// <param name="theme">theme that will be used</param>
    /// <param name="customThemeName">
    ///     used only when <paramref name="theme" /> is set to <see cref="WindowsTheme.Other" /> and
    ///     specify theme name.
    /// </param>
    public static void SetTheme(this Control control, WindowsTheme theme, string customThemeName = "")
    {
        if (control is null)
            throw new ArgumentNullException(nameof(control));

        SetTheme(control.Handle, theme, customThemeName);
    }

    /// <summary>
    ///     Sets theme for the control. Works only on some controls.
    /// </summary>
    /// <param name="handle">editing control handle</param>
    /// <param name="theme">theme that will be used</param>
    /// <param name="customThemeName">
    ///     used only when <paramref name="theme" /> is set to <see cref="WindowsTheme.Other" /> and
    ///     specifify theme name
    /// </param>
    public static void SetTheme(IntPtr handle, WindowsTheme theme, string customThemeName = "")
    {
        if (handle == IntPtr.Zero)
            throw new ArgumentNullException(nameof(handle));

        switch (theme)
        {
            case WindowsTheme.None:
                Win32.SetWindowTheme(handle, "", "");
                break;
            case WindowsTheme.Explorer:
                Win32.SetWindowTheme(handle, "Explorer", null);
                break;
            case WindowsTheme.DarkExplorer:
                Win32.SetWindowTheme(handle, "DarkMode_Explorer", null);
                break;
            case WindowsTheme.Other:
                Win32.SetWindowTheme(handle, customThemeName, null);
                break;
            case WindowsTheme.Default:
                Win32.SetWindowTheme(handle, null, null);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(theme), theme, null);
        }
    }

    /// <summary>
    ///     Sets a color of Progress bar.
    /// </summary>
    /// <param name="bar">editing ProgressBar</param>
    /// <param name="color">style for this ProgressBar</param>
    /// <exception cref="ArgumentOutOfRangeException">when <paramref name="color" /> is invalid.</exception>
    public static void SetProgressBarColor(this ProgressBar bar, ProgressBarColor color)
    {
        if (bar is null)
            throw new ArgumentNullException(nameof(bar));

        switch (color)
        {
            case ProgressBarColor.Green:
                Win32.SendMessage(bar.Handle, 0x400 + 16, (IntPtr)1, (IntPtr)0);
                break;
            case ProgressBarColor.Yellow:
                Win32.SendMessage(bar.Handle, 0x400 + 16, (IntPtr)3, (IntPtr)0);
                break;
            case ProgressBarColor.Red:
                Win32.SendMessage(bar.Handle, 0x400 + 16, (IntPtr)2, (IntPtr)0);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(color), color, null);
        }
    }

    /// <summary>
    /// Sends a message to the specified process.
    /// </summary>
    /// <param name="process"></param>
    /// <param name="msg"></param>
    /// <param name="wParam"></param>
    /// <param name="lParam"></param>
    /// <returns></returns>
    public static int SendMessage(this Process process, uint msg, IntPtr wParam, IntPtr lParam)
    {
        if (process is null || process.HasExited)
            return -1;
            
        return Win32.SendMessage(process.MainWindowHandle, msg, wParam, lParam);
    }

    /// <summary>
    ///     Method to call interop for system beep.
    /// </summary>
    /// <remarks>Calls Windows to make computer beep</remarks>
    /// <param name="type">The kind of beep you would like to hear</param>
    public static void Beep(BeepType type) => Win32.MessageBeep((uint)type);
}

/// <summary>
///     Enum type that enables intellisense on the private Beep method.
/// </summary>
/// <remarks>
///     Used by the public Beep
/// </remarks>
public enum BeepType : uint
{
    /// <summary>
    ///     A simple windows beep.
    /// </summary>
    SimpleBeep = 0xFFFFFFFF,

    /// <summary>
    ///     A standard windows OK beep.
    /// </summary>
    OK = 0x00,

    /// <summary>
    ///     A standard windows Question beep.
    /// </summary>
    Question = 0x20,

    /// <summary>
    ///     A standard windows Exclamation beep.
    /// </summary>
    Exclamation = 0x30,

    /// <summary>
    ///     A standard windows Asterisk beep.
    /// </summary>
    Asterisk = 0x40
}

/// <summary>
///     Specify theme for Windows controls.
/// </summary>
public enum WindowsTheme
{
    /// <summary>
    ///     No style will be used.
    /// </summary>
    None,

    /// <summary>
    ///     Style of Windows Explorer in current Windows version.
    /// </summary>
    Explorer,

    /// <summary>
    ///     Style of Windows Explorer (dark mode) in current Windows version. Works only in Windows 10.
    /// </summary>
    DarkExplorer,

    /// <summary>
    ///     Other style will be used.
    /// </summary>
    Other,

    /// <summary>
    ///     Default style will be used.
    /// </summary>
    Default
}

/// <summary>
///     Specify style of ProgressBar.
/// </summary>
public enum ProgressBarColor
{
    /// <summary>
    ///     Green ProgressBar (Normal style)
    /// </summary>
    Green,

    /// <summary>
    ///     Yellow ProgressBar (Warning style)
    /// </summary>
    Yellow,

    /// <summary>
    ///     Red ProgressBar (Error style)
    /// </summary>
    Red,

    /// <summary>
    ///     Green ProgressBar (Normal style)
    /// </summary>
    Normal = Green,

    /// <summary>
    ///     Yellow ProgressBar (Warning style)
    /// </summary>
    Warning = Yellow,

    /// <summary>
    ///     Red ProgressBar (Error style)
    /// </summary>
    Error = Red
}

/// <summary>
///     Defines form's type of style. Works only in Windows 11+.
/// </summary>
public enum FormStyle
{
    /// <summary>
    ///     Auto style. Does not work in build lower than 22523.
    /// </summary>
    Auto,

    /// <summary>
    ///     Default style. Does not work in build lower than 22000.
    /// </summary>
    Default,

    /// <summary>
    ///     Mica style. Does not work in build lower than 22000.
    /// </summary>
    Mica,

    /// <summary>
    ///     Acrylic style. Does not work in build lower than 22523.
    /// </summary>
    Acrylic,

    /// <summary>
    ///     Tabbed style. Does not work in build lower than 22523. 
    /// </summary>
    Tabbed
}

/// <summary>
///     Defines type of form's corner. Works only in Windows 11+.
/// </summary>
public enum FormCornersType
{
    /// <summary>
    ///     Default type corner of the form.
    /// </summary>
    Default,

    /// <summary>
    ///     Rectangular corner of the form.
    /// </summary>
    Rectangular,

    /// <summary>
    ///     Round corner of the form.
    /// </summary>
    Round,

    /// <summary>
    ///     Semi-round corner of the form.
    /// </summary>
    SmallRound
}