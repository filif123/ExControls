using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming
// ReSharper disable NotAccessedField.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
#pragma warning disable CS0649

namespace ExControls;

internal static partial class Win32
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SHSTOCKICONINFO
    {
        public uint cbSize;
        public IntPtr hIcon;
        public int iSysIconIndex;
        public int iIcon;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260 /*MAX_PATH*/)]
        public string szPath;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NMHDR
    {
        public IntPtr HWND;
        public uint idFrom;
        public int code;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT(int x, int y)
    {
        public int X = x;
        public int Y = y;

        public static implicit operator Point(POINT point) => new(point.X, point.Y);

        public static implicit operator POINT(Point point) => new(point.X, point.Y);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left, Top, Right, Bottom;

        public int Width => Right - Left;

        public int Height => Bottom - Top;

        public static implicit operator Rectangle(RECT r) => new(r.Left, r.Top, r.Width, r.Height);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct COMBOBOXINFO
    {
        public int cbSize;
        public RECT rcItem;
        public RECT rcButton;
        public ComboBoxButtonState buttonState;
        public IntPtr hwndCombo;
        public IntPtr hwndEdit;
        public IntPtr hwndList;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PAINTSTRUCT
    {
        public IntPtr hdc;
        public bool fErase;
        public RECT rcPaint;
        public bool fRestore;
        public bool fIncUpdate;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] rgbReserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TVHITTESTINFO
    {
        public POINT pt;
        public int flags;
        public IntPtr hItem;
    }
}
