using System.Runtime.InteropServices;

namespace ExControls;

/// <summary>
/// Extended form. With <see cref="ExTitleBar" /> as a direct child it replaces the system caption by the title bar.
/// </summary>
/// <remarks>
/// The form uses WS_EX_COMPOSITED by default (<see cref="UseCompositedPainting" />). An empty unfocused
/// <c>TextBox.PlaceholderText</c> in it causes endless repainting of the whole form (the text flickers, WinForms draws it
/// outside WM_PAINT) - use <see cref="ExTextBox.HintText" /> instead.
/// <para>
/// At runtime a form with a custom title bar has the system frame only on the left, right and bottom, in the designer it has
/// neither frame nor caption - the client area (<see cref="Control.ClientSize" />, which the designer saves) is the same in both.
/// WinForms computes the form size from the window style, i.e. with the system caption; ExForm therefore recomputes it itself
/// at runtime (SetClientSizeCore, client area estimate before the window is created, size of the new window in CreateParams,
/// scaling).
/// </para>
/// </remarks>
public class ExForm : Form
{
    // okraj s automaticky skryvanym panelom uloh, ktory musi maximalizovane okno nechat volny, aby sa panel vysunul
    private const int AutoHideTaskbarMargin = 2;

    private ExTitleBar? _titleBar;
    private bool _customFrame;
    private FrameKind _sizedFor;
    private bool _clientSizeSet;
    private bool _trackingNcMouse;
    private FormWindowState _lastWindowState;
    private bool _useCompositedPainting = true;
    private bool _darkTitleBar;
    private Color _titleBarBackColor;
    private Color _titleBarForeColor;
    private Color _titleBarBorderColor;
    private FormCornersType _cornersType;
    private FormStyle _formStyle = FormStyle.Default;

    /// <inheritdoc />
    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            if (!DesignMode && _useCompositedPainting)
                cp.ExStyle |= (int)Win32.WindowStylesEx.WS_EX_COMPOSITED;
            if (DesignMode && HidesSystemCaption)
            {
                // navrhar ukaze okno bez titulku a ramu (klientska oblast = cele okno) - ako za behu pod vlastnym zahlavim;
                // rozmery z tychto stylov pocita aj WinForms
                cp.Style &= ~(int)(Win32.WindowStyles.WS_CAPTION | Win32.WindowStyles.WS_THICKFRAME);
                cp.ExStyle &= ~(int)(Win32.WindowStylesEx.WS_EX_DLGMODALFRAME | Win32.WindowStylesEx.WS_EX_WINDOWEDGE |
                                     Win32.WindowStylesEx.WS_EX_CLIENTEDGE);
            }
            else if (!IsHandleCreated && UsesCustomTitleBar && cp.Width != int.MinValue)
                KeepCreateSize(cp);

            return cp;
        }
    }

    /// <summary>
    /// Size of the new window: on creation WinForms recomputes it from the set ClientSize with the system caption (and centers
    /// the window by it), a form with a custom title bar has its size without the caption.
    /// </summary>
    private void KeepCreateSize(CreateParams cp)
    {
        var height = cp.Height;
        cp.Width = Width;
        cp.Height = Height;
        if (height == Height || StartPosition != FormStartPosition.CenterScreen || WindowState == FormWindowState.Maximized)
            return;

        var area = Screen.FromRectangle(new Rectangle(cp.X, cp.Y, cp.Width, height)).WorkingArea;
        cp.X = Math.Max(area.X, area.X + (area.Width - cp.Width) / 2);
        cp.Y = Math.Max(area.Y, area.Y + (area.Height - cp.Height) / 2);
    }

    /// <inheritdoc />
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override ISite? Site
    {
        get => base.Site;
        set
        {
            base.Site = value;
            // v navrhari sa okno kresli bez ramu
            if (!Disposing && !IsDisposed)
                UpdateFrame();
        }
    }

    /// <inheritdoc />
    protected override void OnHandleCreated(EventArgs e)
    {
        _clientSizeSet = false;
        base.OnHandleCreated(e);
        this.SetImmersiveDarkMode(_darkTitleBar);
        if (ExTools.IsWin11Build22000)
        {
            ApplyCaptionColors();
            this.SetFormCorners(_cornersType);
            this.SetFormStyle(_formStyle);
        }

        UpdateFrame();
    }

    /// <summary>
    /// Caption, text and border colors in DWM - an empty color restores the system one.
    /// </summary>
    private void ApplyCaptionColors() => ExTools.SetCaptionColors(Handle, _titleBarBackColor, _titleBarForeColor, _titleBarBorderColor);

    /// <summary>
    /// Gets or sets whether the whole form is painted double buffered (WS_EX_COMPOSITED) - less flicker when resizing,
    /// but some controls repaint endlessly in such a window (TextBox.PlaceholderText). Default is true.
    /// </summary>
    [ExCategory(CategoryType.WindowStyle)]
    [ExDescription("Gets or sets whether the whole form is painted double buffered (WS_EX_COMPOSITED).", true)]
    [DefaultValue(true)]
    public bool UseCompositedPainting
    {
        get => _useCompositedPainting;
        set
        {
            if (_useCompositedPainting == value)
                return;
            _useCompositedPainting = value;
            if (IsHandleCreated)
                UpdateStyles();
        }
    }

    /// <summary>
    /// Gets or sets whether form's titlebar uses dark mode. Default is false.
    /// Works only in Windows 10+; <see cref="TitleBarBackColor" /> takes precedence over it.
    /// </summary>
    [ExDescription("Gets or sets whether form's titlebar uses dark mode.", true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(false)]
    public bool DarkTitleBar
    {
        get => _darkTitleBar;
        set
        {
            if (_darkTitleBar == value)
                return;
            _darkTitleBar = value;
            if (IsHandleCreated) 
                this.SetImmersiveDarkMode(_darkTitleBar);
        }
    }

    /// <summary>
    /// Gets or sets background color of the titlebar. Set Color.Empty for default color.
    /// The system caption uses it in Windows 11+, <see cref="ExTitleBar" /> in Windows 10+.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("Gets or sets the background of the caption (system caption in Windows 11+, ExTitleBar in Windows 10+). Color.Empty uses the system color.", true)]
    [DefaultValue(typeof(Color))]
    public Color TitleBarBackColor
    {
        get => _titleBarBackColor;
        set
        {
            if (_titleBarBackColor == value)
                return;
            _titleBarBackColor = value;
            if (IsHandleCreated)
                ApplyCaptionColors();
            _titleBar?.RefreshColors();
        }
    }

    /// <summary>
    /// Gets or sets foreground color of the titlebar. Set Color.Empty for default color.
    /// The system caption uses it in Windows 11+, <see cref="ExTitleBar" /> in Windows 10+ (text and window buttons).
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("Gets or sets the text color of the caption (system caption in Windows 11+, ExTitleBar in Windows 10+). Color.Empty uses the system color.", true)]
    [DefaultValue(typeof(Color))]
    public Color TitleBarForeColor
    {
        get => _titleBarForeColor;
        set
        {
            if (_titleBarForeColor == value)
                return;
            _titleBarForeColor = value;
            if (IsHandleCreated)
                ApplyCaptionColors();
            _titleBar?.RefreshColors();
        }
    }

    /// <summary>
    /// Gets or sets color of the window border. Set Color.Empty for default color.
    /// Works only in Windows 11+ (also with <see cref="ExTitleBar" />).
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("Gets or sets the color of the window border (Windows 11+). Color.Empty uses the system color.", true)]
    [DefaultValue(typeof(Color))]
    public Color TitleBarBorderColor
    {
        get => _titleBarBorderColor;
        set
        {
            if (_titleBarBorderColor == value)
                return;
            _titleBarBorderColor = value;
            if (IsHandleCreated)
                ApplyCaptionColors();
        }
    }

    /// <summary>
    /// Gets or sets type of form corner. Default is FormCornersType.Default.
    /// Works only in Windows 11+.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("Gets or sets the corners of the window (Windows 11+).", true)]
    [DefaultValue(FormCornersType.Default)]
    public FormCornersType CornersType
    {
        get => _cornersType;
        set
        {
            if (_cornersType == value)
                return;
            
            _cornersType = value;
            if (IsHandleCreated)
                this.SetFormCorners(_cornersType);
        }
    }

    /// <summary>
    /// Gets or sets system backdrop of the form. Default is FormStyle.Default.
    /// Works only in Windows 11+ and shows only in the system caption - not under <see cref="ExTitleBar" />.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("Gets or sets the system backdrop of the window (Windows 11+). It shows only in the system caption, not under ExTitleBar.", true)]
    [DefaultValue(FormStyle.Default)]
    public FormStyle FormStyle
    {
        get => _formStyle;
        set
        {
            if (_formStyle == value)
                return;
            _formStyle = value;
            if (IsHandleCreated)
                this.SetFormStyle(_formStyle);
        }
    }

    /// <summary>
    /// Gets the title bar replacing the system caption (first <see cref="ExTitleBar" /> among direct children).
    /// </summary>
    [Browsable(false)]
    public ExTitleBar? TitleBar => _titleBar;

    /// <summary>
    /// Gets whether the form shows <see cref="TitleBar" /> instead of the system caption: the title bar is visible, the form
    /// is a top-level window with a border (any <see cref="FormBorderStyle" /> except None) and the system is Windows 10+
    /// (<see cref="ExTools.SupportsCustomTitleBar" />).
    /// The designer shows such a form without the system caption and frame.
    /// </summary>
    [Browsable(false)]
    public bool UsesCustomTitleBar => HidesSystemCaption && !DesignMode;

    /// <summary>
    /// The form has no system caption: a custom title bar at runtime (<see cref="UsesCustomTitleBar" />), a form without
    /// a frame in the designer (the designer shows the form as a child window, TopLevel = false).
    /// </summary>
    private bool HidesSystemCaption =>
        _titleBar is { Shown: true } && FormBorderStyle != FormBorderStyle.None && (TopLevel || DesignMode) && !IsMdiChild &&
        ExTools.SupportsCustomTitleBar;

    private FrameKind Frame => !HidesSystemCaption ? FrameKind.System : DesignMode ? FrameKind.Designer : FrameKind.CustomTitleBar;

    /// <summary>
    /// The form size is computed with the custom title bar: before the window is created as expected, afterwards by the
    /// actual frame (WM_NCCALCSIZE).
    /// </summary>
    private bool SizedWithCustomFrame => IsHandleCreated ? _customFrame : UsesCustomTitleBar;

    /// <inheritdoc />
    protected override void OnControlAdded(ControlEventArgs e)
    {
        base.OnControlAdded(e);
        if (_titleBar is null && e.Control is ExTitleBar titleBar)
            SetTitleBar(titleBar);
    }

    /// <inheritdoc />
    protected override void OnControlRemoved(ControlEventArgs e)
    {
        base.OnControlRemoved(e);
        if (e.Control != _titleBar)
            return;

        var next = Controls.OfType<ExTitleBar>().FirstOrDefault();
        // navrhar rusi cely navrh (prvky rusi este v navrhu - zmazane zahlavie najprv z navrhu odoberie) alebo sa rusi
        // formular: ram ani rozmery sa uz nemenia, ostatne navrhare uz mozu byt zrusene
        if (Disposing || IsDisposed || e.Control is { Disposing: true, Site: not null })
            _titleBar = next;
        else
            SetTitleBar(next);
    }

    private void SetTitleBar(ExTitleBar? titleBar)
    {
        _titleBar = titleBar;
        UpdateFrame();
    }

    /// <summary>
    /// Recomputes the window frame if it changed whether the form has a custom title bar: the size before the window is
    /// created and in the designer, the frame of the created window (WM_NCCALCSIZE). The title bar calls it when
    /// <see cref="Control.Visible" /> changes (a hidden form does not raise the VisibleChanged event of the title bar).
    /// </summary>
    internal void UpdateFrame()
    {
        var frame = Frame;
        if (frame != _sizedFor)
        {
            _sizedFor = frame;
            KeepSizeForFrame();
        }

        if (IsHandleCreated && UsesCustomTitleBar != _customFrame)
            Win32.SetWindowPos(Handle, IntPtr.Zero, 0, 0, 0, 0,
                Win32.SetWindowPosFlags.IgnoreMove | Win32.SetWindowPosFlags.IgnoreResize | Win32.SetWindowPosFlags.IgnoreZOrder |
                Win32.SetWindowPosFlags.DoNotActivate | Win32.SetWindowPosFlags.FrameChanged);
        _titleBar?.RefreshCaption();
    }

    /// <summary>
    /// Size after a frame change. In the designer and before the window is created with a set ClientSize the client area is
    /// kept (as when FormBorderStyle changes) - InitializeComponent adds the title bar only after setting ClientSize.
    /// Otherwise the outer size is kept.
    /// </summary>
    private void KeepSizeForFrame()
    {
        var client = ClientSize;
        if (DesignMode)
        {
            if (IsHandleCreated)
                UpdateStyles();
            ClientSize = client;
        }
        else if (!IsHandleCreated)
        {
            if (_clientSizeSet)
                ClientSize = client;
            else
                UpdateClientSize();
        }
    }

    /// <summary>
    /// Estimates the client area from the window frame before the window is created (WinForms computes it with the system
    /// caption).
    /// </summary>
    private void UpdateClientSize()
    {
        var frame = SizeFromClientSize(Size.Empty);
        var client = new Size(Math.Max(0, Width - frame.Width), Math.Max(0, Height - frame.Height));
        if (ClientSize != client)
            UpdateBounds(Left, Top, Width, Height, client.Width, client.Height);
    }

    /// <inheritdoc />
    /// <remarks>A form with a custom title bar has the system frame only on the left, right and bottom.</remarks>
    protected override Size SizeFromClientSize(Size clientSize)
    {
        if (!SizedWithCustomFrame)
            return base.SizeFromClientSize(clientSize);

        // ram je zo vsetkych stran rovnako hruby, so systemovym titulkom ma okno navyse len titulok
        var frame = base.SizeFromClientSize(Size.Empty).Width / 2;
        return new Size(clientSize.Width + 2 * frame, clientSize.Height + frame);
    }

    /// <inheritdoc />
    protected override void SetClientSizeCore(int x, int y)
    {
        base.SetClientSizeCore(x, y);
        if (!IsHandleCreated)
            _clientSizeSet = true;
        if (!SizedWithCustomFrame)
            return;

        // WinForms k velkosti okna pripocital systemovy titulok
        var size = SizeFromClientSize(new Size(x, y));
        if (Size != size)
            Size = size;
    }

    /// <inheritdoc />
    protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
    {
        base.SetBoundsCore(x, y, width, height, specified);
        if (!IsHandleCreated && SizedWithCustomFrame)
            UpdateClientSize();
    }

    /// <inheritdoc />
    /// <remarks>A form with a custom title bar scales the client area without the system caption.</remarks>
    protected override Rectangle GetScaledBounds(Rectangle bounds, SizeF factor, BoundsSpecified specified)
    {
        var scaled = base.GetScaledBounds(bounds, factor, specified);
        if (!SizedWithCustomFrame || (specified & BoundsSpecified.Height) == 0 || GetStyle(ControlStyles.FixedHeight))
            return scaled;

        if (WindowState != FormWindowState.Normal)
            bounds = RestoreBounds;
        var frame = SizeFromClientSize(Size.Empty).Height;
        scaled.Height = (int)Math.Round((bounds.Height - frame) * factor.Height) + frame;
        return scaled;
    }

    /// <inheritdoc />
    protected override void OnStyleChanged(EventArgs e)
    {
        base.OnStyleChanged(e);
        // FormBorderStyle meni ram, ControlBox, MinimizeBox, MaximizeBox a ShowIcon tlacidla a ikonu
        UpdateFrame();
    }

    /// <inheritdoc />
    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        // maximalizacia meni znak tlacidla a vo Windows 10 horny okraj
        if (WindowState == _lastWindowState)
            return;
        _lastWindowState = WindowState;
        _titleBar?.RefreshCaption();
    }

    /// <inheritdoc />
    protected override void WndProc(ref Message m)
    {
        switch ((Win32.WM)m.Msg)
        {
            case Win32.WM.NCCALCSIZE:
                _customFrame = UsesCustomTitleBar;
                if (_customFrame)
                {
                    WmNcCalcSize(ref m);
                    return;
                }

                break;
            case Win32.WM.NCHITTEST when _customFrame && _titleBar is not null:
                WmNcHitTest(ref m, _titleBar);
                return;
            case Win32.WM.NCMOUSEMOVE when _customFrame && _titleBar is not null:
                if (WmNcMouseMove(ref m, _titleBar))
                    return;
                break;
            case Win32.WM.NCMOUSELEAVE:
                _trackingNcMouse = false;
                _titleBar?.ResetButtons();
                break;
            case Win32.WM.NCLBUTTONDOWN or Win32.WM.NCLBUTTONDBLCLK when _customFrame && _titleBar is not null &&
                                                                         TitleBarHitTest.CaptionButtonOf(HitCode(m)) is { } pressed:
                // systemova obsluha by v klientskej oblasti nakreslila klasicke tlacidlo
                _titleBar.PressButton(pressed);
                m.Result = IntPtr.Zero;
                return;
            case Win32.WM.NCLBUTTONUP when _customFrame && _titleBar is not null && TitleBarHitTest.CaptionButtonOf(HitCode(m)) is { } released:
                if (_titleBar.ReleaseButton(released))
                {
                    var command = TitleBarHitTest.SysCommandOf(released, Win32.IsZoomed(m.HWnd));
                    Win32.PostMessage(m.HWnd, (uint)Win32.WM.SYSCOMMAND, (IntPtr)command, IntPtr.Zero);
                }

                m.Result = IntPtr.Zero;
                return;
            case Win32.WM.NCACTIVATE when _titleBar is not null:
                _titleBar.SetFormActive(m.WParam != IntPtr.Zero);
                break;
            case Win32.WM.SETTINGCHANGE or Win32.WM.DWMCOLORIZATIONCOLORCHANGED:
                base.WndProc(ref m);
                _titleBar?.RefreshAccent();
                return;
            case Win32.WM.SETICON:
                base.WndProc(ref m);
                _titleBar?.RefreshCaption();
                return;
        }

        base.WndProc(ref m);
    }

    private static int HitCode(Message m) => unchecked((int)m.WParam.ToInt64());

    /// <summary>
    /// Non-client area of the window: system caption and frame, custom title bar at runtime (frame on the left, right and
    /// bottom) or designer (none).
    /// </summary>
    private enum FrameKind
    {
        System,
        CustomTitleBar,
        Designer
    }

    /// <summary>
    /// Mouse over the title bar: the window button under the mouse and tracking the mouse leave (WM_NCMOUSELEAVE).
    /// </summary>
    /// <returns>the message is handled (the mouse is over a window button)</returns>
    private bool WmNcMouseMove(ref Message m, ExTitleBar titleBar)
    {
        if (!_trackingNcMouse)
        {
            var track = new Win32.TRACKMOUSEEVENT
            {
                cbSize = (uint)Marshal.SizeOf<Win32.TRACKMOUSEEVENT>(),
                dwFlags = Win32.TrackMouseEventFlags.TME_LEAVE | Win32.TrackMouseEventFlags.TME_NONCLIENT,
                hwndTrack = m.HWnd
            };
            _trackingNcMouse = Win32.TrackMouseEvent(ref track);
        }

        var button = TitleBarHitTest.CaptionButtonOf(HitCode(m));
        titleBar.TrackButton(button);
        if (button is null)
            return false;

        // systemova obsluha by nad tlacidlom kreslila klasicke tlacidlo; rozlozenia okien (Snap Layouts) ukaze system
        // uz podla HTMAXBUTTON z WM_NCHITTEST
        m.Result = IntPtr.Zero;
        return true;
    }

    /// <summary>
    /// Removes the caption and the top frame from the non-client area. The borders on the left, right and bottom (including
    /// the invisible resize borders) stay system ones, so the window keeps its shadow, rounded corners, snapping and animations.
    /// </summary>
    private void WmNcCalcSize(ref Message m)
    {
        // lParam je obdlznik okna (pri vytvoreni okna, wParam = 0) alebo NCCALCSIZE_PARAMS, ktore nim zacina;
        // systemova obsluha ho prepise na klientsky
        var window = Marshal.PtrToStructure<Win32.RECT>(m.LParam);
        base.WndProc(ref m);
        var client = Marshal.PtrToStructure<Win32.RECT>(m.LParam);

        client.Top = window.Top;
        if (Win32.IsZoomed(m.HWnd))
        {
            // maximalizovane okno presahuje monitor o ram - zahlavie musi zacat az na okraji monitora
            client.Top += window.Bottom - client.Bottom;
            KeepAutoHideTaskbarsReachable(ref client);
        }

        Marshal.StructureToPtr(client, m.LParam, false);
        m.Result = IntPtr.Zero;
    }

    /// <summary>
    /// A maximized window must not cover the whole monitor, otherwise an auto-hide taskbar does not slide out.
    /// </summary>
    private static void KeepAutoHideTaskbarsReachable(ref Win32.RECT client)
    {
        var state = new Win32.APPBARDATA { cbSize = (uint)Marshal.SizeOf<Win32.APPBARDATA>() };
        if ((Win32.SHAppBarMessage(Win32.AppBarMessage.ABM_GETSTATE, ref state).ToUInt64() & Win32.ABS_AUTOHIDE) == 0)
            return;

        var monitor = Screen.FromRectangle(client).Bounds;
        if (HasAutoHideTaskbar(Win32.AppBarEdge.ABE_TOP, monitor))
            client.Top += AutoHideTaskbarMargin;
        if (HasAutoHideTaskbar(Win32.AppBarEdge.ABE_BOTTOM, monitor))
            client.Bottom -= AutoHideTaskbarMargin;
        if (HasAutoHideTaskbar(Win32.AppBarEdge.ABE_LEFT, monitor))
            client.Left += AutoHideTaskbarMargin;
        if (HasAutoHideTaskbar(Win32.AppBarEdge.ABE_RIGHT, monitor))
            client.Right -= AutoHideTaskbarMargin;
    }

    private static bool HasAutoHideTaskbar(Win32.AppBarEdge edge, Rectangle monitor)
    {
        var data = new Win32.APPBARDATA
        {
            cbSize = (uint)Marshal.SizeOf<Win32.APPBARDATA>(),
            uEdge = edge,
            rc = new Win32.RECT { Left = monitor.Left, Top = monitor.Top, Right = monitor.Right, Bottom = monitor.Bottom }
        };
        return Win32.SHAppBarMessage(Win32.AppBarMessage.ABM_GETAUTOHIDEBAREX, ref data) != UIntPtr.Zero;
    }

    /// <summary>
    /// Splits the top of the client area into the top border and the parts of the title bar (dragging, buttons, controls in
    /// the title bar).
    /// </summary>
    private void WmNcHitTest(ref Message m, ExTitleBar titleBar)
    {
        base.WndProc(ref m);
        // okraje vlavo, vpravo a dole vyhodnotil system
        if (m.Result != (IntPtr)Win32.HitTest.HTCLIENT)
            return;

        var point = PointToClient(TitleBarHitTest.ScreenPoint(m.LParam));
        // pevny ram sa zhora nemeni - horny okraj je zahlavie
        var sizable = FormBorderStyle is FormBorderStyle.Sizable or FormBorderStyle.SizableToolWindow;
        var resizeBorder = sizable && !Win32.IsZoomed(m.HWnd) ? FrameThickness(m.HWnd) : 0;
        var hit = TitleBarHitTest.Classify(point, ClientSize.Width, resizeBorder, titleBar.Bounds, titleBar.HitTest);
        m.Result = (IntPtr)TitleBarHitTest.ToHitCode(hit);
    }

    /// <summary>
    /// Thickness of the system frame (including the invisible resize part) - the top resize zone is just as thick.
    /// </summary>
    private int FrameThickness(IntPtr handle)
    {
        Win32.GetWindowRect(new HandleRef(this, handle), out var window);
        return PointToScreen(Point.Empty).X - window.Left;
    }
}
