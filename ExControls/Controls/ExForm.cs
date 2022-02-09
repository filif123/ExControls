using System.Runtime.InteropServices;
using ExControls.Designers;
// ReSharper disable ConvertToAutoProperty

// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace ExControls;

//------------------- WORK IN PROGRESS -------------------

/// <summary>
/// WORK IN PROGRESS
/// </summary>
[Designer(typeof(ExFormDesigner))]
public partial class ExForm
{
    private bool _customMenu;
    private bool _maximizing;

    private Color _titleBarBackColor;
    private Color _titleBarForeColor;
    private Color _titleBarCloseButtonSelBackColor;
    private Color _titleBarCloseButtonSelForeColor;
    private Color _titleBarButtonBackColor;
    private Color _titleBarButtonForeColor;
    private Color _titleBarButtonSelBackColor;
    private Color _titleBarButtonSelForeColor;
    private Color _titleBarCloseButtonBackColor;
    private Color _titleBarCloseButtonForeColor;
    private Color _titleBarDeactivatedForeColor = Color.DimGray;
    private Color _titleBarDeactivatedBackColor;

    /*[Browsable(false)] 
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    internal Panel PanelRoot => panelRoot;*/

    /// <summary>
    /// 
    /// </summary>
    public Color BorderColor { get; set; } = Color.Black;

    /// <summary>
    /// 
    /// </summary>
    public Color TitleBarDeactivatedForeColor
    {
        get => _titleBarDeactivatedForeColor;
        set
        {
            if (_titleBarDeactivatedForeColor == value) return;
            _titleBarDeactivatedForeColor = value;
            TitleBar.DeactivatedForeColor = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public Color TitleBarDeactivatedBackColor
    {
        get => _titleBarDeactivatedBackColor;
        set
        {
            if (_titleBarDeactivatedBackColor == value) return;
            _titleBarDeactivatedBackColor = value;
            TitleBar.DeactivatedBackColor = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public Color TitleBarBackColor
    {
        get => _titleBarBackColor;
        set
        {
            if (_titleBarBackColor == value) return;
            _titleBarBackColor = value;
            TitleBar.BackColor = value;
            TitleBar.Invalidate(true);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public Color TitleBarForeColor
    {
        get => _titleBarForeColor;
        set
        {
            if (_titleBarForeColor == value) return;
            _titleBarForeColor = value;
            TitleBar.ForeColor = value;
            TitleBar.Invalidate(true);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public Color TitleBarCloseButtonSelBackColor
    {
        get => _titleBarCloseButtonSelBackColor;
        set
        {
            if (_titleBarCloseButtonSelBackColor == value) return;
            _titleBarCloseButtonSelBackColor = value;
            TitleBar.CloseButtonSelBackColor = value;
            TitleBar.Invalidate(true);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public Color TitleBarCloseButtonSelForeColor
    {
        get => _titleBarCloseButtonSelForeColor;
        set
        {
            if (_titleBarCloseButtonSelForeColor == value) return;
            _titleBarCloseButtonSelForeColor = value;
            TitleBar.CloseButtonSelForeColor = value;
            TitleBar.Invalidate(true);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public Color TitleBarCloseButtonBackColor
    {
        get => _titleBarCloseButtonBackColor;
        set
        {
            if (_titleBarCloseButtonBackColor == value) return;
            _titleBarCloseButtonBackColor = value;
            TitleBar.CloseButtonBackColor = value;
            TitleBar.Invalidate(true);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public Color TitleBarCloseButtonForeColor
    {
        get => _titleBarCloseButtonForeColor;
        set
        {
            if (_titleBarCloseButtonForeColor == value) return;
            _titleBarCloseButtonForeColor = value;
            TitleBar.CloseButtonForeColor = value;
            TitleBar.Invalidate(true);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public Color TitleBarButtonBackColor
    {
        get => _titleBarButtonBackColor;
        set
        {
            if (_titleBarButtonBackColor == value) return;
            _titleBarButtonBackColor = value;
            TitleBar.ButtonBackColor = value;
            TitleBar.Invalidate(true);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public Color TitleBarButtonForeColor
    {
        get => _titleBarButtonForeColor;
        set
        {
            if (_titleBarButtonForeColor == value) return;
            _titleBarButtonForeColor = value;
            TitleBar.ButtonForeColor = value;
            TitleBar.Invalidate(true);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public Color TitleBarButtonSelBackColor
    {
        get => _titleBarButtonSelBackColor;
        set
        {
            if (_titleBarButtonSelBackColor == value) return;
            _titleBarButtonSelBackColor = value;
            TitleBar.ButtonSelBackColor = value;
            TitleBar.Invalidate(true);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public Color TitleBarButtonSelForeColor
    {
        get => _titleBarButtonSelForeColor;
        set
        {
            if (_titleBarButtonSelForeColor == value) return;
            _titleBarButtonSelForeColor = value;
            TitleBar.ButtonSelForeColor = value;
            TitleBar.Invalidate(true);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public ExForm()
    {
        InitializeComponent();
        TitleBar.Form = this;
        TitleBar.BoxIcon.Image = base.Icon.ToBitmap();
        TitleBar.TextLabel.Text = base.Text;
        TitleBar.ButtonHelp.Visible = false;
        TitleBar.ButtonMaximize.Click += ButtonMaximize_Click;

        TitleBarBackColor = Color.White;
        TitleBarForeColor = Color.Black;
        TitleBarCloseButtonSelBackColor = Color.Red;
        TitleBarCloseButtonSelForeColor = Color.White;
        TitleBarButtonBackColor = Color.White;
        TitleBarButtonForeColor = Color.Black;
        TitleBarButtonSelBackColor = Color.Gainsboro;
        TitleBarButtonSelForeColor = Color.Black;
        TitleBarCloseButtonBackColor = Color.White;
        TitleBarCloseButtonForeColor = Color.Black;

        SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
    }

    private void ButtonMaximize_Click(object sender, EventArgs e)
    {
        //_maximizing = true;
    }

    /// <inheritdoc />
    protected override void OnPaintBackground(PaintEventArgs e)
    {
        base.OnPaintBackground(e);

        //Rectangle rc = new Rectangle(ClientSize.Width - cGrip, ClientSize.Height - cGrip, cGrip, cGrip);
        //ControlPaint.DrawSizeGrip(e.Graphics, BackColor, rc);
        //rc = new Rectangle(0, 0, ClientSize.Width, cCaption);
        //e.Graphics.FillRectangle(Brushes.DarkBlue, rc);

        if (WindowState == FormWindowState.Maximized)
            return;
        //ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, BorderColor, ButtonBorderStyle.Solid);
    }

    /// <summary>
    /// 
    /// </summary>
    public new bool ShowIcon
    {
        get => base.ShowIcon;
        set
        {
            base.ShowIcon = value;
            TitleBar.BoxIcon.Visible = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public new Icon Icon
    {
        get => base.Icon;
        set
        {
            base.Icon = value;
            TitleBar.BoxIcon.Image = value.ToBitmap();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public bool CustomMenu
    {
        get => _customMenu;
        set
        {
            if (_customMenu == value) return;
            _customMenu = value;
            TitleBar.EnableCustomMenu(_customMenu);
        }
    }


    /// <inheritdoc />
    public override string Text
    {
        get => base.Text;
        set
        {
            base.Text = value;
            TitleBar.TextLabel.Text = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public new bool HelpButton
    {
        get => base.HelpButton;
        set
        {
            base.HelpButton = value;
            TitleBar.ButtonHelp.Visible = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public new bool MaximizeBox
    {
        get => base.MaximizeBox;
        set
        {
            base.MaximizeBox = value;
            TitleBar.ButtonMaximize.Visible = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public new bool MinimizeBox
    {
        get => base.MinimizeBox;
        set
        {
            base.MinimizeBox = value;
            TitleBar.ButtonMinimize.Visible = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public new bool ControlBox
    {
        get => base.ControlBox;
        set
        {
            base.ControlBox = value;
            TitleBar.ButtonClose.Visible = value;
            TitleBar.ButtonMinimize.Visible = value;
            TitleBar.ButtonMaximize.Visible = value;
            TitleBar.ButtonHelp.Visible = value;
        }
    }

    /// <inheritdoc />
    public override Color BackColor
    {
        get => base.BackColor;
        set
        {
            base.BackColor = value;
            TitleBar.Invalidate(true);
        }
    }

    /// <inheritdoc />
    public override Color ForeColor
    {
        get => base.ForeColor;
        set
        {
            base.ForeColor = value;
            TitleBar.Invalidate(true);
        }
    }


    /// <inheritdoc />
    protected override CreateParams CreateParams
    {
        get
        {
            var p = base.CreateParams;
            //p.Style |= (int) Win32.WindowStyles.WS_SYSMENU;
            //if (MaximizeBox) p.Style |= (int) Win32.WindowStyles.WS_MAXIMIZEBOX;
            //if (MinimizeBox) p.Style |= (int) Win32.WindowStyles.WS_MINIMIZEBOX;
            //p.Style |= 0x40000;
            //p.Style &= ~0x00C00000; //WS_CAPTION;
            //p.ClassStyle |= (int) Win32.ClassStyles.CS_DROPSHADOW;
            p.Style &= ~0x02000000;  // Turn off WS_CLIPCHILDREN
            //p.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED 
            return p;
        }
    }

    /// <inheritdoc />
    protected override void OnLayout(LayoutEventArgs levent)
    {
        base.OnLayout(levent);
        TitleBar.Invalidate(true);
    }

    /// <inheritdoc />
    protected override void WndProc(ref Message m) {
        /*if (DesignMode)
        {
            base.WndProc(ref m);
            return;
        }*/

        //base.WndProc(ref m);

        //if (m.Msg == (int)Win32.WM.NCHITTEST && (int)m.Result == (int) Win32.HitTestValues.HTCLIENT)
            //m.Result = (IntPtr)Win32.HitTestValues.HTCAPTION;

        var orig = WindowState;
        switch ((Win32.WM) m.Msg)
        {
            case Win32.WM.NCCALCSIZE when m.WParam != IntPtr.Zero:
            {
                base.WndProc(ref m);
                Win32.WINDOWPLACEMENT wPos = default;
                wPos.Length = Marshal.SizeOf<Win32.WINDOWPLACEMENT>();
                Win32.GetWindowPlacement(m.HWnd, ref wPos);

                if (wPos.ShowCmd == Win32.ShowWindowCommands.ShowMaximized) 
                    return;

                var style = (Win32.WindowStyles) Win32.GetWindowLongPtr(m.HWnd, Win32.GWL_STYLE);
                style &=  ~Win32.WindowStyles.WS_CAPTION;
                style |= Win32.WindowStyles.WS_SIZEFRAME;
                //style &= ~Win32.WindowStyles.WS_SYSMENU;
                //Win32.SetWindowLong(m.HWnd, Win32.GWL_STYLE, (uint)style);

                //var exstyle = (Win32.WindowStylesEx) Win32.GetWindowLongPtr(m.HWnd, Win32.GWL_EXSTYLE);

                Win32.RECT border = default;
                Win32.AdjustWindowRectEx(ref border, (uint)style, false, 0);
                
                border.Left *= -1;
                border.Top *= -1;
                var ps = new Win32.NCCALCSIZE_PARAMS();
                Marshal.PtrToStructure(m.LParam, ps);
                ps.rgrc[0].Top += 1;
                ps.rgrc[0].Left += border.Left;
                ps.rgrc[0].Right -= border.Right;
                ps.rgrc[0].Bottom -= border.Bottom;
                m.Result = IntPtr.Zero;
                return;
            }
            case Win32.WM.NCACTIVATE:
            {
                base.WndProc(ref m);
                //m.LParam = (IntPtr) (-1);
                return;
            }
            /*default:
                base.WndProc(ref m);
                break;*/
            /*case Win32.WM.NCHITTEST when m.Result == (IntPtr) 1:
                base.WndProc(ref m);
                m.Result = new IntPtr(2);
                return;*/
        }

        base.WndProc(ref m);
        if (WindowState != orig)
            OnFormWindowStateChanged();
    }

    protected virtual void OnFormWindowStateChanged()
    {
        Padding = WindowState == FormWindowState.Maximized ? new Padding(6,6,0,0) : new Padding(1);
    }

    /// <inheritdoc />
    protected override void OnActivated(EventArgs e)
    {
        base.OnActivated(e);
        if (!TitleBar.DeactivatedForm) return;
        TitleBar.DeactivatedForm = false;
        TitleBar.Invalidate(true);
    }

    /// <inheritdoc />
    protected override void OnDeactivate(EventArgs e)
    {
        base.OnDeactivate(e);
        if (TitleBar.DeactivatedForm) return;
        TitleBar.DeactivatedForm = true;
        TitleBar.Invalidate(true);
    }
}