using System.Runtime.InteropServices;

namespace ExControls;

/// <summary>
/// Extended form. With <see cref="ExTitleBar" /> as a direct child it replaces the system caption by the title bar.
/// </summary>
/// <remarks>
/// Okno ma WS_EX_COMPOSITED. Prazdny <c>TextBox.PlaceholderText</c> bez zamerania v nom vyvola nekonecne prekreslovanie
/// celeho okna (text blika, WinForms ho kresli mimo WM_PAINT) - namiesto neho <see cref="ExTextBox.HintText" />.
/// </remarks>
public class ExForm : Form
{
    // okraj s automaticky skryvanym panelom uloh, ktory musi maximalizovane okno nechat volny, aby sa panel vysunul
    private const int AutoHideTaskbarMargin = 2;

    private ExTitleBar? _titleBar;
    private bool _customFrame;
    private bool _trackingNcMouse;
    private FormWindowState _lastWindowState;
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
            if (!DesignMode) 
                cp.ExStyle |= (int)Win32.WindowStylesEx.WS_EX_COMPOSITED;
            return cp;
        }
    }

    /// <inheritdoc />
    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        this.SetImmersiveDarkMode(_darkTitleBar);
        if (ExTools.IsWin11Build22000)
        {
            this.SetTitlebarAndBorderColor(_titleBarBackColor, _titleBarForeColor, _titleBarBorderColor);
            this.SetFormCorners(_cornersType);
            this.SetFormStyle(_formStyle);
        }

        UpdateFrame();
    }

    /// <summary>
    /// Gets or sets whether form's titlebar uses dark mode. Default is false.
    /// Works only in Windows 10+.
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
            if (_titleBarBackColor != Color.Empty && _darkTitleBar)
                _titleBarBackColor = Color.Empty;
        }
    }

    /// <summary>
    /// Gets or sets background color of the titlebar. Set Color.Empty for default color.
    /// Works only in Windows 11+.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
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
                this.SetTitlebarAndBorderColor(_titleBarBackColor, _titleBarForeColor, _titleBarBorderColor);
            if (_titleBarBackColor != Color.Empty && _darkTitleBar) 
                _darkTitleBar = false;
        }
    }

    /// <summary>
    /// Gets or sets foreground color of the titlebar. Set Color.Empty for default color.
    /// Works only in Windows 11+.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
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
                this.SetTitlebarAndBorderColor(_titleBarBackColor, _titleBarForeColor, _titleBarBorderColor);
        }
    }

    /// <summary>
    /// Gets or sets border color of the titlebar. Set Color.Empty for default color.
    /// Works only in Windows 11+.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
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
                this.SetTitlebarAndBorderColor(_titleBarBackColor, _titleBarForeColor, _titleBarBorderColor);
        }
    }

    /// <summary>
    /// Gets or sets type of form corner. Default is FormCornersType.Default.
    /// Works only in Windows 11+.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
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
    /// Works only in Windows 11+.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
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
    /// is a sizable top-level window and the system is Windows 10+ (<see cref="ExTools.SupportsCustomTitleBar" />).
    /// The designer always shows the system caption.
    /// </summary>
    [Browsable(false)]
    public bool UsesCustomTitleBar =>
        _titleBar is { Shown: true } && FormBorderStyle == FormBorderStyle.Sizable && TopLevel && !IsMdiChild && !DesignMode &&
        ExTools.SupportsCustomTitleBar;

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
        if (e.Control == _titleBar)
            SetTitleBar(Controls.OfType<ExTitleBar>().FirstOrDefault());
    }

    private void SetTitleBar(ExTitleBar? titleBar)
    {
        _titleBar = titleBar;
        UpdateFrame();
    }

    /// <summary>
    /// Prepocita ram okna (WM_NCCALCSIZE), ak sa zmenilo, ci okno ma vlastne zahlavie. Zahlavie ho vola pri zmene
    /// <see cref="Control.Visible" /> (skryty formular udalost VisibleChanged zahlavia nevyvola).
    /// </summary>
    internal void UpdateFrame()
    {
        if (!IsHandleCreated || UsesCustomTitleBar == _customFrame)
            return;
        Win32.SetWindowPos(Handle, IntPtr.Zero, 0, 0, 0, 0,
            Win32.SetWindowPosFlags.IgnoreMove | Win32.SetWindowPosFlags.IgnoreResize | Win32.SetWindowPosFlags.IgnoreZOrder |
            Win32.SetWindowPosFlags.DoNotActivate | Win32.SetWindowPosFlags.FrameChanged);
        _titleBar?.RefreshCaption();
    }

    /// <inheritdoc />
    protected override void OnStyleChanged(EventArgs e)
    {
        base.OnStyleChanged(e);
        // ControlBox, MinimizeBox, MaximizeBox a ShowIcon menia styl okna
        _titleBar?.RefreshCaption();
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
            case Win32.WM.SETICON:
                base.WndProc(ref m);
                _titleBar?.RefreshCaption();
                return;
        }

        base.WndProc(ref m);
    }

    private static int HitCode(Message m) => unchecked((int)m.WParam.ToInt64());

    /// <summary>
    /// Mys nad zahlavim: tlacidlo pod mysou a sledovanie odchodu mysi (WM_NCMOUSELEAVE).
    /// </summary>
    /// <returns>sprava je spracovana (mys je nad tlacidlom okna)</returns>
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
    /// Odstrani z neklientskej oblasti titulok a horny ram. Okraje vlavo, vpravo a dole (aj neviditelne na zmenu velkosti)
    /// ostavaju systemove, takze okno si necha tien, zaoblene rohy, prichytavanie a animacie.
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
    /// Maximalizovane okno nesmie zakryt cely monitor, inak sa automaticky skryvany panel uloh nevysunie.
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
    /// Rozdeli vrch klientskej oblasti na horny okraj a casti zahlavia (tahanie, tlacidla, prvky v zahlavi).
    /// </summary>
    private void WmNcHitTest(ref Message m, ExTitleBar titleBar)
    {
        base.WndProc(ref m);
        // okraje vlavo, vpravo a dole vyhodnotil system
        if (m.Result != (IntPtr)Win32.HitTest.HTCLIENT)
            return;

        var point = PointToClient(TitleBarHitTest.ScreenPoint(m.LParam));
        var resizeBorder = Win32.IsZoomed(m.HWnd) ? 0 : FrameThickness(m.HWnd);
        var hit = TitleBarHitTest.Classify(point, ClientSize.Width, resizeBorder, titleBar.Bounds, titleBar.HitTest);
        m.Result = (IntPtr)TitleBarHitTest.ToHitCode(hit);
    }

    /// <summary>
    /// Hrubka systemoveho ramu (vratane neviditelnej casti na zmenu velkosti) - rovnaka je aj horna zona na zmenu vysky.
    /// </summary>
    private int FrameThickness(IntPtr handle)
    {
        Win32.GetWindowRect(new HandleRef(this, handle), out var window);
        return PointToScreen(Point.Empty).X - window.Left;
    }
}
