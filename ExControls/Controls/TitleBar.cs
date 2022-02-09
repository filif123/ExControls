using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using ExControls.Designers;
using ExControls.Properties;

namespace ExControls;

//------------------- WORK IN PROGRESS -------------------

/// <summary>
/// WORK IN PROGRESS
/// </summary>
[Designer(typeof(TitleBarDesigner))]
internal partial class TitleBar : UserControl
{
    // ReSharper disable once InconsistentNaming
    private const int HT_CAPTION = 0x2;

    internal bool DeactivatedForm
    {
        get => _deactivatedForm;
        set
        {
            if (_deactivatedForm == value) return;
            _deactivatedForm = value;
            TextLabel.ForeColor = _deactivatedForm ? DeactivatedForeColor : ForeColor;
        }
    }

    private bool _closeHover, _maximizeHover, _minimizeHover, _helpHover;
    private bool _closeDrawing, _maximizeDrawing, _minimizeDrawing, _helpDrawing;

    private Color _closeButtonBackColor = Color.Empty;
    private Color _closeButtonForeColor = Color.Black;
    private bool _deactivatedForm;

    internal Color CloseButtonBackColor
    {
        get => _closeButtonBackColor;
        set
        {
            if (_closeButtonBackColor == value) return;
            _closeButtonBackColor = value;
            ButtonClose.Invalidate();
        }
    }

    internal Color CloseButtonForeColor
    {
        get => _closeButtonForeColor;
        set
        {
            if (_closeButtonForeColor == value) return;
            _closeButtonForeColor = value;
            ButtonMinimize.Invalidate();
            ButtonMaximize.Invalidate();
            ButtonHelp.Invalidate();
        }
    }

    public Form Form { get; set; }

    internal Color CloseButtonSelBackColor { get; set; } = Color.Red;
    internal Color CloseButtonSelForeColor { get; set; } = Color.White;

    internal Color ButtonBackColor { get; set; } = Color.Empty;
    internal Color ButtonForeColor { get; set; } = Color.Black;

    internal Color ButtonSelBackColor { get; set; } = Color.DarkGray;
    internal Color ButtonSelForeColor { get; set; } = Color.Black;

    public Color DeactivatedForeColor { get; set; } = Color.DimGray;
    public Color DeactivatedBackColor { get; set; } = Color.White;

    public TitleBar()
    {
        InitializeComponent();

        SetStyle(ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer | ControlStyles.UserPaint, true);
        BackColor = Color.White;
    }

    private void ButtonClose_Paint(object sender, PaintEventArgs e)
    {
        if (_closeDrawing) return;
        _closeDrawing = true;

        Color backColor;
        if (_closeHover)
            backColor = CloseButtonSelBackColor;
        else if (DeactivatedForm) 
            backColor = DeactivatedBackColor;
        else
            backColor = CloseButtonBackColor;
        
        if (backColor.IsEmpty) 
            backColor = BackColor;

        Color foreColor;
        if (_closeHover)
            foreColor = CloseButtonSelForeColor;
        else if (DeactivatedForm)
            foreColor = DeactivatedForeColor;
        else
            foreColor = CloseButtonForeColor;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        e.Graphics.Clear(backColor);
        //e.Graphics.PageUnit = GraphicsUnit.Point;
        using var pen = new Pen(foreColor, 1);
        var p1Y = e.ClipRectangle.Height / 2 - 5;
        var p1X = e.ClipRectangle.Width / 2 - 5;
        e.Graphics.DrawLine(pen, p1X, p1Y, p1X + 10, p1Y + 10);
        var p2Y = e.ClipRectangle.Height / 2 + 5;
        var p2X = e.ClipRectangle.Width / 2 - 5;
        e.Graphics.DrawLine(pen, p2X, p2Y, p2X + 10, p2Y - 10);

        _closeDrawing = false;
    }

    private void ButtonMaximize_Paint(object sender, PaintEventArgs e)
    {
        if (_maximizeDrawing) return;
        _maximizeDrawing = true;

        
        Color backColor;
        if (_maximizeHover)
            backColor = ButtonSelBackColor;
        else if (DeactivatedForm) 
            backColor = DeactivatedBackColor;
        else
            backColor = ButtonBackColor;
        
        if (backColor.IsEmpty) 
            backColor = BackColor;

        Color foreColor;
        if (_maximizeHover)
            foreColor = ButtonSelForeColor;
        else if (DeactivatedForm)
            foreColor = DeactivatedForeColor;
        else
            foreColor = ButtonForeColor;

        var form = Form as ExForm;
        e.Graphics.Clear(backColor);
        //e.Graphics.PageUnit = GraphicsUnit.Point;
        using var pen = new Pen(foreColor, 1);

        if (form == null || form.WindowState == FormWindowState.Normal)
        {
            var py = e.ClipRectangle.Height / 2 - 5;
            var px = e.ClipRectangle.Width / 2 - 5;
            e.Graphics.DrawRectangle(pen, px, py, 9, 9);
        }
        else if(form.WindowState == FormWindowState.Maximized)
        {
            var py = e.ClipRectangle.Height / 2 - 5;
            var px = e.ClipRectangle.Width / 2 - 5;
            e.Graphics.DrawRectangle(pen, px + 2, py, 9, 9);
            using var b = new SolidBrush(backColor);
            e.Graphics.FillRectangle(b, px, py + 2, 10, 10);
            e.Graphics.DrawRectangle(pen, px, py + 2, 9, 9);
        }
        
        _maximizeDrawing = false;
    }

    private void ButtonMinimize_Paint(object sender, PaintEventArgs e)
    {
        if (_minimizeDrawing) return;
        _minimizeDrawing = true;

        Color backColor;
        if (_minimizeHover)
            backColor = ButtonSelBackColor;
        else if (DeactivatedForm) 
            backColor = DeactivatedBackColor;
        else
            backColor = ButtonBackColor;
        
        if (backColor.IsEmpty) 
            backColor = BackColor;

        Color foreColor;
        if (_minimizeHover)
            foreColor = ButtonSelForeColor;
        else if (DeactivatedForm)
            foreColor = DeactivatedForeColor;
        else
            foreColor = ButtonForeColor;

        e.Graphics.Clear(backColor);
        //e.Graphics.PageUnit = GraphicsUnit.Point;
        using var pen = new Pen(foreColor, 1);
        var py = e.ClipRectangle.Height / 2;
        var px = e.ClipRectangle.Width / 2 - 5;
        e.Graphics.DrawLine(pen, px, py, px + 10, py);

        _minimizeDrawing = false;
    }

    private void ButtonHelp_Paint(object sender, PaintEventArgs e)
    {
        if (_helpDrawing) return;
        _helpDrawing = true;

        Color backColor;
        if (_helpHover)
            backColor = ButtonSelBackColor;
        else if (DeactivatedForm) 
            backColor = DeactivatedBackColor;
        else
            backColor = ButtonBackColor;
        
        if (backColor.IsEmpty) 
            backColor = BackColor;

        e.Graphics.Clear(backColor);

        var py = e.ClipRectangle.Height / 2 - 5;
        var px = e.ClipRectangle.Width / 2 - 3;

        using var bmp = Resources.qm;
        // Set the image attribute's color mappings
        var colorMap = new ColorMap[1];
        colorMap[0] = new ColorMap {OldColor = Color.Black, NewColor = backColor};
        //colorMap[1] = new ColorMap {OldColor = Color.White, NewColor = _minimizeHover ? ButtonSelForeColor : ButtonForeColor};
        var attr = new ImageAttributes();
        attr.SetRemapTable(colorMap);
        // Draw using the color map
        var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
        e.Graphics.DrawImage(bmp, rect, px, py, rect.Width, rect.Height, GraphicsUnit.Point, attr);
        //e.Graphics.DrawImage(bmp,px,py,rect,GraphicsUnit.Pixel);

        /*e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using var pen = new Pen(_minimizeHover ? ButtonSelForeColor : ButtonForeColor, 2);
        var py = e.ClipRectangle.Height / 2 - 5;
        var px = e.ClipRectangle.Width / 2 - 3;
        e.Graphics.DrawLine(pen, px, py + 1, px, py);
        e.Graphics.DrawLine(pen, px + 1, py, px + 5, py);
        e.Graphics.DrawLine(pen, px + 6, py + 1, px + 6, py + 4);
        e.Graphics.DrawLine(pen, px + 6, py + 4, px + 3, py + 7);
        e.Graphics.DrawLine(pen, px + 3, py + 7, px + 3, py + 9);

        e.Graphics.DrawLine(pen, px + 3, py + 11, px + 3, py + 12);*/

        _helpDrawing = false;
    }

    private void ButtonMaximize_Click(object sender, EventArgs e)
    {
        Form.WindowState = Form.WindowState == FormWindowState.Normal ? FormWindowState.Maximized : FormWindowState.Normal;
        ButtonMaximize.Invalidate();
    }

    private void ButtonMinimize_Click(object sender, EventArgs e)
    {
        Form.WindowState = FormWindowState.Minimized;
        ButtonMinimize.Invalidate();
        //Win32.AnimateWindow(Form.Handle, 300, Win32.AW_VER_POSITIVE | Win32.AW_SLIDE);
    }

    private void ButtonHelp_Click(object sender, EventArgs e)
    {
        Win32.SendMessage(Form.Handle, Win32.WM.SYSCOMMAND, (IntPtr) Win32.SC_CONTEXTHELP, IntPtr.Zero);
    }

    private void ButtonClose_Click(object sender, EventArgs e)
    {
        Form.Close();
    }

    private void ButtonClose_MouseEnter(object sender, EventArgs e)
    {
        if (!_closeHover)
        {
            _closeHover = true;
            ButtonClose.Invalidate();
        }
    }

    private void ButtonClose_MouseLeave(object sender, EventArgs e)
    {
        if (_closeHover)
        {
            _closeHover = false;
            ButtonClose.Invalidate();
        }
    }
    
    private void ButtonMaximize_MouseEnter(object sender, EventArgs e)
    {
        if (!_maximizeHover)
        {
            _maximizeHover = true;
            ButtonMaximize.Invalidate();
        }
    }

    private void ButtonMaximize_MouseLeave(object sender, EventArgs e)
    {
        if (_maximizeHover)
        {
            _maximizeHover = false;
            ButtonMaximize.Invalidate();
        }
    }

    private void ButtonMinimize_MouseEnter(object sender, EventArgs e)
    {
        if (!_minimizeHover)
        {
            _minimizeHover = true;
            ButtonMinimize.Invalidate();
        }
    }

    private void ButtonMinimize_MouseLeave(object sender, EventArgs e)
    {
        if (_minimizeHover)
        {
            _minimizeHover = false;
            ButtonMinimize.Invalidate();
        }
    }

    private void ButtonHelp_MouseEnter(object sender, EventArgs e)
    {
        if (!_helpHover)
        {
            _helpHover = true;
            ButtonHelp.Invalidate();
        }
    }

    private void ButtonHelp_MouseLeave(object sender, EventArgs e)
    {
        if (_helpHover)
        {
            _helpHover = false;
            ButtonHelp.Invalidate();
        }
    }

    private void TablePanel_MouseDown(object sender, MouseEventArgs e)
    {
        switch (e.Button)
        {
            case MouseButtons.Left:
                Win32.ReleaseCapture();
                Win32.SendMessage(Form.Handle, Win32.WM.NCLBUTTONDOWN, (IntPtr) HT_CAPTION, IntPtr.Zero);
                break;
            case MouseButtons.Right:
            {
                var p = MousePosition.X + (MousePosition.Y * 0x10000);
                Win32.SendMessage(Form.Handle, Win32.WM.POPUPSYSTEMMENU, (IntPtr)0, (IntPtr)p);
                break;
            }
        }
    }

    private void TablePanel_Paint(object sender, PaintEventArgs e)
    {

    }

    private void BoxIcon_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right) ShowSystemContextMenu();
    }

    private void TextLabel_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right) ShowSystemContextMenu();
    }

    private void ShowSystemContextMenu()
    {
        var p = MousePosition.X + (MousePosition.Y * 0x10000);
        Win32.SendMessage(Form.Handle, Win32.WM.POPUPSYSTEMMENU, (IntPtr)0, (IntPtr)p);
    }

    private void TablePanel_DoubleClick(object sender, EventArgs e)
    {
        Form.WindowState = Form.WindowState switch
        {
            FormWindowState.Maximized => FormWindowState.Normal,
            FormWindowState.Normal => FormWindowState.Maximized,
            _ => Form.WindowState
        };
    }

    public void EnableCustomMenu(bool enable)
    {
        /*if (enable)
        {
            TablePanel.SetColumn(TextLabel, 2);
            TablePanel.SetColumn(PanelCustom, 1);
        }
        else
        {
            TablePanel.SetColumn(TextLabel, 1);
            TablePanel.SetColumn(PanelCustom, 2);
            PanelCustom.Controls.Clear();
        }*/
    }
}