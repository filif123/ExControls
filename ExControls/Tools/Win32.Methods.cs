using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace ExControls;

/// <summary>
/// Volania Win32 API, ktore kniznica pouziva. Len [DllImport] - [LibraryImport] na .NET Framework neexistuje.
/// </summary>
internal static partial class Win32
{
    private const string USER32 = "user32.dll";
    private const string GDI32 = "gdi32.dll";
    private const string DWMAPI = "dwmapi.dll";
    private const string SHELL32 = "shell32.dll";
    private const string UXTHEME = "uxtheme.dll";

    #region USER32

    [DllImport(USER32, EntryPoint = "GetWindowLong")]
    private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

    [DllImport(USER32, EntryPoint = "GetWindowLongPtr")]
    private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

    /// <summary>
    /// Retrieves information about the specified window (GetWindowLong on 32-bit, GetWindowLongPtr on 64-bit processes).
    /// </summary>
    public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex) =>
        IntPtr.Size == 8 ? GetWindowLongPtr64(hWnd, nIndex) : GetWindowLongPtr32(hWnd, nIndex);

    [DllImport(USER32)]
    public static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport(USER32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

    [DllImport(USER32, EntryPoint = "SendMessageA")]
    public static extern int SendMessage(IntPtr hwnd, uint wMsg, IntPtr wParam, IntPtr lParam);

    [DllImport(USER32)]
    public static extern IntPtr GetWindowDC(IntPtr hWnd);

    [DllImport(USER32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

    [DllImport(USER32)]
    public static extern IntPtr BeginPaint(IntPtr hWnd, out PAINTSTRUCT lpPaint);

    [DllImport(USER32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);

    [DllImport(USER32)]
    public static extern IntPtr WindowFromPoint(POINT point);

    [DllImport(USER32, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DestroyIcon(IntPtr hIcon);

    /// <summary>
    /// Plays a waveform sound. The waveform sound for each sound type is identified by an entry in the registry.
    /// </summary>
    [DllImport(USER32, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool MessageBeep(uint type);

    [DllImport(USER32, SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern IntPtr MB_GetString(int strId);

    [DllImport(USER32, ExactSpelling = true, SetLastError = true)]
    public static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, [In] [Out] ref POINT rect, int cPoints);

    [DllImport(USER32, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

    #endregion

    #region GDI32

    [DllImport(GDI32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool FillRgn(IntPtr hdc, IntPtr hrgn, IntPtr hbr);

    [DllImport(GDI32)]
    public static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

    [DllImport(GDI32)]
    public static extern IntPtr CombineRgn(IntPtr hrgnDst, IntPtr hrgnDst1, IntPtr hrgnDst2, int mode);

    [DllImport(GDI32)]
    public static extern IntPtr CreateSolidBrush(uint crColor);

    [DllImport(GDI32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DeleteObject(IntPtr hObject);

    #endregion

    #region DWMAPI

    [DllImport(DWMAPI)]
    public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

    [DllImport(DWMAPI)]
    public static extern int DwmGetWindowAttribute(IntPtr hwnd, int attr, out int attrValue, int attrSize);

    #endregion

    [DllImport(UXTHEME, ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int SetWindowTheme(IntPtr hWnd, string? textSubAppName, string? textSubIdList);

    [DllImport(SHELL32, SetLastError = false)]
    public static extern int SHGetStockIconInfo(ShellIconType type, SHGSI uFlags, ref SHSTOCKICONINFO psii);

    public static int ToInt(this WM msg) => (int)msg;

    public static uint RGBtoInt(Color color) => (uint)((color.R << 0) | (color.G << 8) | (color.B << 16));
}
