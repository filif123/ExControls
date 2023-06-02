using System.Globalization;
using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming
// ReSharper disable NotAccessedField.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
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

        public override string ToString() => $"Hwnd: {HWND}, ControlID: {idFrom}, Code: {code}";
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator Point(POINT point) => new(point.X, point.Y);

        public static implicit operator POINT(Point point) => new((int) point.X, (int) point.Y);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left, Top, Right, Bottom;

        public RECT(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public RECT(Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom)
        {
        }

        public int X
        {
            get => Left;
            set
            {
                Right -= Left - value;
                Left = value;
            }
        }

        public int Y
        {
            get => Top;
            set
            {
                Bottom -= Top - value;
                Top = value;
            }
        }

        public int Height
        {
            get => Bottom - Top;
            set => Bottom = value + Top;
        }

        public int Width
        {
            get => Right - Left;
            set => Right = value + Left;
        }

        public Point Location
        {
            get => new(Left, Top);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public Size Size
        {
            get => new(Width, Height);
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        public static implicit operator Rectangle(RECT r)
        {
            return new Rectangle(r.Left, r.Top, r.Width, r.Height);
        }

        public static implicit operator RECT(Rectangle r)
        {
            return new RECT(r);
        }

        public static bool operator ==(RECT r1, RECT r2)
        {
            return r1.Equals(r2);
        }

        public static bool operator !=(RECT r1, RECT r2)
        {
            return !r1.Equals(r2);
        }

        public bool Equals(RECT r)
        {
            return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
        }

        public override bool Equals(object obj)
        {
            return obj switch
            {
                RECT rect => Equals(rect),
                Rectangle rectangle => Equals(new RECT(rectangle)),
                _ => false
            };
        }

        public override int GetHashCode()
        {
            return ((Rectangle)this).GetHashCode();
        }

        public override string ToString() => string.Format(CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
    }

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

    /// <summary>
    /// Contains information about the placement of a window on the screen.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPLACEMENT
    {
        /// <summary>
        /// The length of the structure, in bytes. Before calling the GetWindowPlacement or SetWindowPlacement functions, set this member to sizeof(WINDOWPLACEMENT).
        /// <para>
        /// GetWindowPlacement and SetWindowPlacement fail if this member is not set correctly.
        /// </para>
        /// </summary>
        public int Length;

        /// <summary>
        /// Specifies flags that control the position of the minimized window and the method by which the window is restored.
        /// </summary>
        public int Flags;

        /// <summary>
        /// The current show state of the window.
        /// </summary>
        public ShowWindowCommands ShowCmd;

        /// <summary>
        /// The coordinates of the window's upper-left corner when the window is minimized.
        /// </summary>
        public POINT MinPosition;

        /// <summary>
        /// The coordinates of the window's upper-left corner when the window is maximized.
        /// </summary>
        public POINT MaxPosition;

        /// <summary>
        /// The window's coordinates when the window is in the restored position.
        /// </summary>
        public RECT NormalPosition;

        /// <summary>
        /// Gets the default (empty) value.
        /// </summary>
        public static WINDOWPLACEMENT Default
        {
            get
            {
                var result = new WINDOWPLACEMENT();
                result.Length = Marshal.SizeOf( result );
                return result;
            }
        }    
    }

    /// <summary>
    ///     https://www.pinvoke.net/default.aspx/Structures/NCCALCSIZE_PARAMS.html
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class NCCALCSIZE_PARAMS
    {
        /// <summary>
        ///     An array of rectangles. The meaning of the array of rectangles changes during the processing of the WM_NCCALCSIZE message.<br></br>
        ///     <br></br>
        ///     When the window procedure receives the WM_NCCALCSIZE message, the first rectangle contains the new coordinates of a window that has been moved or resized,
        /// that is, it is the proposed new window coordinates. The second contains the coordinates of the window before it was moved or resized.
        /// The third contains the coordinates of the window's client area before the window was moved or resized. If the window is a child window,
        /// the coordinates are relative to the client area of the parent window. If the window is a top-level window, the coordinates are relative to the screen origin.<br></br>
        ///     <br></br>
        ///     When the window procedure returns, the first rectangle contains the coordinates of the new client rectangle resulting from the move or resize.
        /// The second rectangle contains the valid destination rectangle, and the third rectangle contains the valid source rectangle.
        /// The last two rectangles are used in conjunction with the return value of the WM_NCCALCSIZE message to determine the area of the window to be preserved.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public RECT[] rgrc;

        /// <summary>
        ///     A pointer to a WINDOWPOS structure that contains the size and position values specified in the operation that moved or resized the window.
        /// </summary>
        public WINDOWPOS lppos;

        public NCCALCSIZE_PARAMS()
        {
        }

        public NCCALCSIZE_PARAMS(RECT[] rgrc, WINDOWPOS lppos)
        {
            this.rgrc = rgrc;
            this.lppos = lppos;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPOS
    {
        public IntPtr hwnd;
        public IntPtr hwndInsertAfter;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public uint flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MARGINS
    {
        public int leftWidth;
        public int rightWidth;
        public int topHeight;
        public int bottomHeight;

        public MARGINS(int top, int right, int bottom, int left)
        {
            topHeight = top;
            rightWidth = right;
            bottomHeight = bottom;
            leftWidth = left;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DATETIMEPICKERINFO
    {
        public int cbSize;
        public RECT rcCheck;
        public int stateCheck;
        public RECT rcButton;
        public int stateButton;
        public IntPtr hwndEdit;
        public IntPtr hwndUD;
        public IntPtr hwndDropDown;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TVHITTESTINFO
    {
        public POINT pt;
        public int flags;
        public IntPtr hItem;
    }

    [Flags]
    public enum CLSCTX : uint
    {
        INPROC_SERVER = 0x1,
        INPROC_HANDLER = 0x2,
        LOCAL_SERVER = 0x4,
        INPROC_SERVER16 = 0x8,
        REMOTE_SERVER = 0x10,
        INPROC_HANDLER16 = 0x20,
        INPROC_SERVERX86 = 0x40,
        INPROC_HANDLERX86 = 0x80,
        ESERVER_HANDLER = 0x100,
        NO_CODE_DOWNLOAD = 0x400,
        NO_CUSTOM_MARSHAL = 0x1000,
        ENABLE_CODE_DOWNLOAD = 0x2000,
        NO_FAILURE_LOG = 0x4000,
        DISABLE_AAA = 0x8000,
        ENABLE_AAA = 0x10000,
        FROM_DEFAULT_CONTEXT = 0x20000,
    }
}