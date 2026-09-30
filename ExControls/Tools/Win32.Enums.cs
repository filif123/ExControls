// ReSharper disable InconsistentNaming
namespace ExControls;

internal static partial class Win32
{
    public const int GWL_STYLE = -16;

    /// <summary>
    /// Window styles (len pouzite).
    /// </summary>
    [Flags]
    public enum WindowStyles : uint
    {
        /// <summary>The window has a thin-line border.</summary>
        WS_BORDER = 0x800000
    }

    /// <summary>
    /// Extended window styles (len pouzite).
    /// </summary>
    [Flags]
    public enum WindowStylesEx : uint
    {
        /// <summary>Paints all descendants of a window in bottom-to-top painting order using double-buffering.</summary>
        WS_EX_COMPOSITED = 0x02000000
    }

    public enum ComboBoxButtonState
    {
        STATE_SYSTEM_NONE = 0,
        STATE_SYSTEM_INVISIBLE = 0x00008000,
        STATE_SYSTEM_PRESSED = 0x00000008
    }

    /// <summary>
    /// Flags for SHGetStockIconInfo.
    /// </summary>
    [Flags]
#pragma warning disable CA1069 // ICONLOCATION a LARGEICON su v Win32 obe 0
    public enum SHGSI : uint
    {
        /// <summary>The szPath and iIcon members receive the path and icon index of the requested icon.</summary>
        ICONLOCATION = 0,

        /// <summary>The hIcon member receives a handle to the specified icon.</summary>
        ICON = 0b_1_00000000,

        /// <summary>The iSysImageImage member receives the index of the specified icon in the system imagelist.</summary>
        SYSICONINDEX = 0b_01000000_00000000,

        /// <summary>Adds the link overlay to the file's icon.</summary>
        LINKOVERLAY = 0b_10000000_00000000,

        /// <summary>Blends the icon with the system highlight color.</summary>
        SELECTED = 0b_1_00000000_00000000,

        /// <summary>Retrieves the large version of the icon (SM_CXICON, SM_CYICON).</summary>
        LARGEICON = 0b000,

        /// <summary>Retrieves the small version of the icon (SM_CXSMICON, SM_CYSMICON).</summary>
        SMALLICON = 0b001,

        /// <summary>Retrieves the Shell-sized icons rather than the sizes specified by the system metrics.</summary>
        SHELLICONSIZE = 0b100
    }
#pragma warning restore CA1069

    /// <summary>
    /// Flags for SetWindowPos (len pouzite).
    /// </summary>
    [Flags]
    public enum SetWindowPosFlags : uint
    {
        /// <summary>Retains the current size (ignores the cx and cy parameters).</summary>
        IgnoreResize = 0x0001,

        /// <summary>Retains the current position (ignores X and Y parameters).</summary>
        IgnoreMove = 0x0002,

        /// <summary>Retains the current Z order (ignores the hWndInsertAfter parameter).</summary>
        IgnoreZOrder = 0x0004,

        /// <summary>Does not activate the window.</summary>
        DoNotActivate = 0x0010,

        /// <summary>Sends a WM_NCCALCSIZE message to the window, even if the window's size is not being changed.</summary>
        FrameChanged = 0x0020
    }

    /// <summary>
    /// Window messages (len pouzite).
    /// </summary>
    public enum WM : uint
    {
        PAINT = 0x000F,
        ERASEBKGND = 0x0014,
        NOTIFY = 0x004E,
        NCPAINT = 0x0085,
        CTLCOLORLISTBOX = 0x0134,
        LBUTTONDOWN = 0x0201,
        PRINTCLIENT = 0x0318,
        THEMECHANGED = 0x031A,
        USER = 0x0400
    }
}
