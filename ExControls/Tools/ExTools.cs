using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ExControls
{
    /// <summary>
    ///     Contains various stuff for controls and forms.
    /// </summary>
    public static class ExTools
    {
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

        /// <summary>
        ///     Changes background color of window's caption (immersive dark mode). <br></br>
        ///     Works only in Windows 10.
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
        ///     Works only in Windows 10.
        /// </summary>
        /// <param name="handle">handle to form where dark mode is applying</param>
        /// <param name="enabled">whether to turn dark mode on or off</param>
        /// <returns>whether dark mode has been successfully activated/deactivated</returns>
        public static bool SetImmersiveDarkMode(IntPtr handle, bool enabled)
        {
            if (IsWindows10OrGreater(17763))
            {
                var attribute = DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1;
                if (IsWindows10OrGreater(18985)) attribute = DWMWA_USE_IMMERSIVE_DARK_MODE;

                var useImmersiveDarkMode = enabled ? 1 : 0;
                return Win32.DwmSetWindowAttribute(handle, attribute, ref useImmersiveDarkMode, sizeof(int)) == 0;
            }

            return false;
        }

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
        ///     Sets theme for the control. Works only on some controls.
        /// </summary>
        /// <param name="control">editing control</param>
        /// <param name="theme">theme that will be used</param>
        /// <param name="customThemeName">
        ///     used only when <paramref name="theme" /> is set to <see cref="WindowsTheme.Other" /> and
        ///     specifify theme name
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(theme), theme, null);
            }
        }

        /// <summary>
        ///     Set color of Progress bar.
        /// </summary>
        /// <param name="bar">editing ProgressBar</param>
        /// <param name="style">style for this ProgressBar</param>
        /// <exception cref="ArgumentOutOfRangeException">when <paramref name="style" /> is invalid.</exception>
        public static void SetProgressBarColor(this ProgressBar bar, ProgressBarStyle style)
        {
            if (bar is null)
                throw new ArgumentNullException(nameof(bar));

            switch (style)
            {
                case ProgressBarStyle.Green:
                    Win32.SendMessage(bar.Handle, 0x400 + 16, (IntPtr)1, (IntPtr)0);
                    break;
                case ProgressBarStyle.Yellow:
                    Win32.SendMessage(bar.Handle, 0x400 + 16, (IntPtr)3, (IntPtr)0);
                    break;
                case ProgressBarStyle.Red:
                    Win32.SendMessage(bar.Handle, 0x400 + 16, (IntPtr)2, (IntPtr)0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(style), style, null);
            }
        }

        /// <summary>
        ///     Converts collection to <see cref="ExBindingList{TSource}"/>.
        /// </summary>
        /// <typeparam name="TSource">Type of item in collection.</typeparam>
        /// <param name="source">The collection.</param>
        /// <returns>list of the type <see cref="ExBindingList{TSource}"/>.</returns>
        /// <exception cref="ArgumentNullException">when <paramref name="source"/> is <see langword="null"/>.</exception>
        public static ExBindingList<TSource> ToExBindingList<TSource>(this ICollection<TSource> source)
        {
            return source != null ? new ExBindingList<TSource>(source.ToList()) : throw new ArgumentNullException(nameof(source));
        }

        /// <summary>
        /// 
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
        ///     Method to call interop for system beep
        /// </summary>
        /// <remarks>Calls Windows to make computer beep</remarks>
        /// <param name="type">The kind of beep you would like to hear</param>
        public static void Beep(BeepType type)
        {
            Win32.MessageBeep((uint)type);
        }
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
        ///     A simple windows beep
        /// </summary>
        SimpleBeep = 0xFFFFFFFF,

        /// <summary>
        ///     A standard windows OK beep
        /// </summary>
        OK = 0x00,

        /// <summary>
        ///     A standard windows Question beep
        /// </summary>
        Question = 0x20,

        /// <summary>
        ///     A standard windows Exclamation beep
        /// </summary>
        Exclamation = 0x30,

        /// <summary>
        ///     A standard windows Asterisk beep
        /// </summary>
        Asterisk = 0x40
    }

    /// <summary>
    ///     Specify theme for Windows controls
    /// </summary>
    public enum WindowsTheme
    {
        /// <summary>
        ///     No style will be used
        /// </summary>
        None,

        /// <summary>
        ///     Style of Windows Explorer in current Windows version
        /// </summary>
        Explorer,

        /// <summary>
        ///     Style of Windows Explorer (dark mode) in current Windows version. Works only in Windows 10.
        /// </summary>
        DarkExplorer,

        /// <summary>
        ///     Other style will be used.
        /// </summary>
        Other
    }

    /// <summary>
    ///     Specify style of ProgressBar.
    /// </summary>
    public enum ProgressBarStyle
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
}