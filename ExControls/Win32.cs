using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ExControls
{
    internal static class Win32
    {
        #region Constants

        public const int WM_CHANGEUISTATE = 295;
        public const int WM_MOUSEACTIVATE = 33;
        public const int WM_MOUSEFIRST = 512;
        public const int WM_MOUSELAST = 521;
        public const int WM_MOUSEHOVER = 673;
        public const int WM_MOUSELEAVE = 675;
        public const int WM_NCPAINT = 135;
        public const int WM_PAINT = 15;
        public const int WM_ERASEBKGND = 20;
        public const int WM_KILLFOCUS = 8;
        public const int WM_SETFOCUS = 7;
        public const int EM_GETRECT = 178;
        public const int WM_CTLCOLOREDIT = 307;
        public const int WM_CTLCOLORSTATIC = 312;
        public const int WM_CTLCOLORLISTBOX = 308;

        #endregion

        #region Methods

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(IntPtr hwnd, uint wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string lclassName, string windowTitle);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern bool FillRgn(IntPtr hdc, IntPtr hrgn, IntPtr hbr);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CombineRgn(IntPtr hrgnDst, IntPtr hrgnDst1, IntPtr hrgnDst2, int mode);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateSolidBrush(uint crColor);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(POINT Point);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
        internal static extern IntPtr GetFocus();

        [DllImport("uxtheme", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(IntPtr hWnd, string textSubAppName, string textSubIdList);

        [DllImport("user32", ExactSpelling = true, SetLastError = true)]
        public static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, [In, Out] ref POINT rect, int cPoints);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern uint SetBkColor(IntPtr hdc, int crColor);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern uint SetTextColor(IntPtr hdc, int crColor);

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        #endregion

        #region Structs

        [StructLayout(LayoutKind.Sequential)]
        public struct NMHDR
        {
            public IntPtr HWND;
            public uint idFrom;
            public int code;

            public override string ToString()
            {
                return $"Hwnd: {HWND}, ControlID: {idFrom}, Code: {code}";
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public int x;
            public int y;
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

            public RECT(Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

            public int X
            {
                get => Left;
                set { Right -= (Left - value); Left = value; }
            }

            public int Y
            {
                get => Top;
                set { Bottom -= (Top - value); Top = value; }
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
                set { X = value.X; Y = value.Y; }
            }

            public Size Size
            {
                get => new(Width, Height);
                set { Width = value.Width; Height = value.Height; }
            }

            public static implicit operator Rectangle(RECT r)
            {
                return new(r.Left, r.Top, r.Width, r.Height);
            }

            public static implicit operator RECT(Rectangle r)
            {
                return new(r);
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

            public override string ToString()
            {
                return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
            }
        }

        #endregion
    }
}
