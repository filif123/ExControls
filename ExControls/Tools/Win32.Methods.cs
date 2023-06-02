using System.Runtime.InteropServices;
using System.Text;
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming

namespace ExControls;

internal static partial class Win32
{
    private const string USER32 = "user32.dll";
    private const string GDI32 = "gdi32.dll";
    private const string DWMAPI = "dwmapi.dll";
    private const string SHELL32 = "shell32.dll";
    private const string KERNEL32 = "kernel32.dll";
    private const string UXTHEME = "uxtheme.dll";

    #region USER32

    /// <summary>
    ///     Retrieves the show state and the restored, minimized, and maximized positions of the specified window.
    /// </summary>
    /// <param name="hWnd">A handle to the window.</param>
    /// <param name="lpwndpl">A pointer to the WINDOWPLACEMENT structure that receives the show state and position information. Before calling GetWindowPlacement, set the length member to sizeof(WINDOWPLACEMENT). GetWindowPlacement fails if lpwndpl-&gt; length is not set correctly.</param>
    /// <returns>
    /// If the function succeeds, the return value is nonzero.
    /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
    /// </returns>
    [DllImport(USER32, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

    [DllImport(USER32, EntryPoint="GetWindowLong")]
    private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

    [DllImport(USER32, EntryPoint="GetWindowLongPtr")]
    private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

    /// <summary>
    ///     Retrieves information about the specified window. The function also retrieves the 32-bit (DWORD) value at the specified offset into the extra window memory.
    /// </summary>
    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
    /// <param name="nIndex">The zero-based offset to the value to be retrieved. Valid values are in the range zero through the number of bytes of extra window memory, minus four; for example, if you specified 12 or more bytes of extra memory, a value of 8 would be an index to the third 32-bit integer. To retrieve any other value, specify one of the following values.</param>
    /// <returns>
    /// If the function succeeds, the return value is the requested value.
    /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
    /// If SetWindowLong has not been called previously, GetWindowLong returns zero for values in the extra window or class memory.</returns>
    // This static method is required because Win32 does not support
    // GetWindowLongPtr directly
    public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
    {
        if (IntPtr.Size == 8)
            return GetWindowLongPtr64(hWnd, nIndex);
        return GetWindowLongPtr32(hWnd, nIndex);
    }

    [DllImport(USER32)]
    public static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    /// <summary>
    ///     Releases the mouse capture from a window in the current thread and restores normal mouse input processing.
    /// A window that has captured the mouse receives all mouse input, regardless of the position of the cursor,
    /// except when a mouse button is clicked while the cursor hot spot is in the window of another thread.
    /// </summary>
    /// <returns>
    /// If the function succeeds, the return value is nonzero.
    /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
    /// </returns>
    [DllImport(USER32)]
    public static extern bool ReleaseCapture();

    /// <summary>
    ///     Enables you to produce special effects when showing or hiding windows.
    /// There are four types of animation: roll, slide, collapse or expand, and alpha-blended fade.<br></br>
    ///     https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-animatewindow
    /// </summary>
    /// <param name="hwnd">A handle to the window to animate. The calling thread must own this window.</param>
    /// <param name="dwTime">The time it takes to play the animation, in milliseconds. Typically, an animation takes 200 milliseconds to play.</param>
    /// <param name="dwFlags">The type of animation. This parameter can be one or more of the following values. Note that, by default, these flags take effect when showing a window. To take effect when hiding a window, use AW_HIDE and a logical OR operator with the appropriate flags.</param>
    /// <returns>
    /// If the function succeeds, the return value is nonzero.
    /// If the function fails, the return value is zero.
    /// </returns>
    [DllImport(USER32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);

    [DllImport(USER32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

    [DllImport(USER32, EntryPoint = "SendMessageA")]
    public static extern int SendMessage(IntPtr hwnd, uint wMsg, IntPtr wParam, IntPtr lParam);

    public static unsafe nint SendMessage<T>(IntPtr hWnd, uint Msg, nint wParam, ref T lParam) where T : unmanaged
    {
        fixed (void* l = &lParam)
        {
            return SendMessage(hWnd, Msg, wParam, (nint)l);
        }
    }

    public static void SendMessage(IntPtr handle, WM wMsg, IntPtr wParam, IntPtr lParam) => SendMessage(handle, (uint)wMsg, wParam, lParam);

    [DllImport(USER32, CharSet = CharSet.Unicode)]
    public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string lclassName, string windowTitle);

    [DllImport(USER32)]
    public static extern IntPtr GetWindowDC(IntPtr hWnd);

    [DllImport(USER32)]
    public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

    [DllImport(USER32)]
    public static extern IntPtr WindowFromPoint(POINT point);

    [DllImport(USER32, CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
    public static extern IntPtr GetFocus();

    [DllImport(USER32, SetLastError = true)]
    public static extern bool DestroyIcon(IntPtr hIcon);

    /// <summary>
    ///     Plays a waveform sound. The waveform sound for each sound type is identified by an entry in the registry.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    [DllImport(USER32, ExactSpelling = true)]
    public static extern bool MessageBeep(uint type);

    [DllImport(USER32)]
    public static extern IntPtr GetActiveWindow();

    [DllImport(USER32, CharSet = CharSet.Auto)]
    public static extern int LoadString(IntPtr hInstance, uint uID, StringBuilder lpBuffer, int nBufferMax);

    [DllImport(USER32, SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern IntPtr MB_GetString(int strId);

    [DllImport(USER32)]
    public static extern IntPtr CallWindowProc(WndProcDelegate lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    [DllImport(USER32)]
    public static extern IntPtr DefWindowProc(IntPtr hWnd, WM uMsg, IntPtr wParam, IntPtr lParam);

    [DllImport(USER32)]
    public static extern void PostQuitMessage(int nExitCode);

    [DllImport(USER32, ExactSpelling = true, SetLastError = true)]
    public static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, [In] [Out] ref POINT rect, int cPoints);

    [DllImport(USER32, SetLastError = true)]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

    [DllImport(USER32, SetLastError = true)]
    public static extern void DisableProcessWindowsGhosting();

    [DllImport(USER32)]
    public static extern bool AdjustWindowRectEx(ref RECT lpRect, uint dwStyle, bool bMenu, uint dwExStyle);

    #endregion

    #region GDI32

    [DllImport(GDI32)]
    public static extern bool FillRgn(IntPtr hdc, IntPtr hrgn, IntPtr hbr);

    [DllImport(GDI32)]
    public static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

    [DllImport(GDI32)]
    public static extern IntPtr CombineRgn(IntPtr hrgnDst, IntPtr hrgnDst1, IntPtr hrgnDst2, int mode);

    [DllImport(GDI32)]
    public static extern IntPtr CreateSolidBrush(uint crColor);

    [DllImport(GDI32)]
    public static extern bool DeleteObject(IntPtr hObject);

    [DllImport(GDI32, SetLastError = true)]
    public static extern uint SetBkColor(IntPtr hdc, int crColor);

    [DllImport(GDI32, SetLastError = true)]
    public static extern uint SetTextColor(IntPtr hdc, int crColor);

    #endregion


    #region DWMAPI

    [DllImport(DWMAPI)]
    public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

    [DllImport(DWMAPI)]
    public static extern int DwmGetWindowAttribute(IntPtr hwnd, int attr, out int attrValue, int attrSize);

    [DllImport(DWMAPI, PreserveSig = true)]
    public static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);

    [DllImport(DWMAPI)]
    public static extern int DwmIsCompositionEnabled(out bool enabled);

    [DllImport(DWMAPI)]
    public static extern bool DwmDefWindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref IntPtr plResult);

    #endregion

    #region UXTHEME

    [DllImport(UXTHEME, ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern IntPtr OpenThemeData(IntPtr hWnd, string classList);

    [DllImport(UXTHEME, ExactSpelling = true)]
    public static extern int CloseThemeData(IntPtr hTheme);

    [DllImport(UXTHEME, ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int SetWindowTheme(IntPtr hWnd, string textSubAppName, string textSubIdList);

    #endregion

    [DllImport(KERNEL32, CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
    public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    [DllImport(KERNEL32, SetLastError = true, CharSet = CharSet.Ansi)]
    public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string dllToLoad);

    [DllImport(SHELL32, SetLastError = false)]
    public static extern int SHGetStockIconInfo(ShellIconType type, SHGSI uFlags, ref SHSTOCKICONINFO psii);

    public static int SignedHIWORD(int n)
    {
        int i = (short)((n >> 16) & 0xffff);
        return i;
    }

    public static int SignedLOWORD(int n)
    {
        int i = (short)(n & 0xFFFF);
        return i;
    }

    public static int ToInt(this WM msg) => (int) msg;

    public static uint RGBtoInt(Color color)
    {
        return (uint)((color.R << 0) | (color.G << 8) | (color.B << 16));
    }
}