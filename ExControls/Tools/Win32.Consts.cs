// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
namespace ExControls;

internal static partial class Win32
{
    public delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    public const int AW_VER_POSITIVE = 0x00000004;
    public const int AW_VER_NEGATIVE = 0x00000008;
    public const int AW_SLIDE = 0x00040000;
    public const int AW_HIDE = 0x00010000;

    public const int SC_CONTEXTHELP = 0xf180;

    public const int GWL_WNDPROC = -4;
    public const int GWL_HINSTANCE = -6;
    public const int GWL_HWNDPARENT = -8;
    public const int GWL_STYLE = -16;
    public const int GWL_EXSTYLE = -20;
    public const int GWL_USERDATA = -21;
    public const int GWL_ID = -12;

}