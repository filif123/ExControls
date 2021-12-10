using System.Runtime.InteropServices;
using ExControls.Controls;

namespace ExControls;

/// <summary>
///     Expanded TabControl Control
/// </summary>
[ToolboxBitmap(typeof(TabControl), "TabControl.bmp")]
public class ExTabControl : TabControl, IExControl
{
    private const int TCN_FIRST = -550;
    private const int TCN_SELCHANGING = TCN_FIRST - 2;
    private const int WM_REFLECT = (int)(Win32.WM.USER + 0x1C00);
    private Color _activeHeaderBackColor;
    private Color _activeHeaderForeColor;

    private Color _backcolor = Color.Empty;
    private Color _borderColor;
    private int _borderThickness;

    private bool _defaultStyle;
    private Color _headerBackColor;
    private Color _headerForeColor;
    private Color _highlightBackColor;
    private Color _highlightForeColor;
    private int _hoverIndex = -1;

    /// <summary>
    ///     Constructor
    /// </summary>
    public ExTabControl()
    {
        _defaultStyle = true;
        _borderColor = Color.LightGray;
        _headerForeColor = Color.Black;
        _headerBackColor = SystemColors.Control;
        _activeHeaderForeColor = Color.Black;
        _activeHeaderBackColor = Color.White;
        _highlightBackColor = SystemColors.GradientInactiveCaption;
        _highlightForeColor = Color.Black;
        _borderThickness = 1;

        Invalidate();
    }

    /// <summary>
    ///     Color of the TabControl's border
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "LightGray")]
    [Description("Color of the TabControl's border.")]
    public Color BorderColor
    {
        get => _borderColor;
        set
        {
            if (_borderColor == value)
                return;
            _borderColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Foreground color of the Tab header
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "Black")]
    [Description("Foreground color of the Tab header.")]
    public Color HeaderForeColor
    {
        get => _headerForeColor;
        set
        {
            if (_headerForeColor == value)
                return;
            _headerForeColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Background color of the Tab header
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "Control")]
    [Description("Background color of the Tab header.")]
    public Color HeaderBackColor
    {
        get => _headerBackColor;
        set
        {
            if (_headerBackColor == value)
                return;
            _headerBackColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Foreground color of the active Tab header
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "White")]
    [Description("Background color of the active Tab header.")]
    public Color ActiveHeaderBackColor
    {
        get => _activeHeaderBackColor;
        set
        {
            if (_activeHeaderBackColor == value)
                return;
            _activeHeaderBackColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Foreground color of the active Tab header
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "Black")]
    [Description("Foreground color of the active Tab header.")]
    public Color ActiveHeaderForeColor
    {
        get => _activeHeaderForeColor;
        set
        {
            if (_activeHeaderForeColor == value)
                return;
            _activeHeaderForeColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Background color of the hovered Tab header
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "GradientInactiveCaption")]
    [Description("Background color of the hovered Tab header.")]
    public Color HighlightBackColor
    {
        get => _highlightBackColor;
        set
        {
            if (_highlightBackColor == value)
                return;
            _highlightBackColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Foreground color of the hovered Tab header
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "Black")]
    [Description("Foreground color of the hovered Tab header.")]
    public Color HighlightForeColor
    {
        get => _highlightForeColor;
        set
        {
            if (_highlightForeColor == value)
                return;
            _highlightForeColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Width of the TabControl's border
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(1)]
    [Description("Width of the TabControl's border.")]
    public int BorderThickness
    {
        get => _borderThickness;
        set
        {
            if (_borderThickness == value)
                return;
            _borderThickness = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     The background color used to display text and graphics in a control
    /// </summary>
    [Browsable(true)]
    [Description("The background color used to display text and graphics in a control.")]
    public override Color BackColor
    {
        get
        {
            if (_backcolor == Color.Empty)
                return Parent?.BackColor ?? DefaultBackColor;

            return _backcolor;
        }
        set
        {
            if (_backcolor == value) return;
            _backcolor = value;
            Invalidate();
            base.OnBackColorChanged(EventArgs.Empty);
        }
    }

    /// <summary>Occurs when the <see cref="IExControl.DefaultStyle" /> property changes.</summary>
    [ExCategory("Changed Property")]
    [Description("Occurs when the BorderColor property changes.")]
    public event EventHandler DefaultStyleChanged;

    /// <inheritdoc />
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(true)]
    [Description("Default style of the Control.")]
    public bool DefaultStyle
    {
        get => _defaultStyle;
        set
        {
            if (_defaultStyle == value)
                return;
            _defaultStyle = value;
            DrawMode = value ? TabDrawMode.Normal : TabDrawMode.OwnerDrawFixed;
            Invalidate();
        }
    }

    /// <summary>
    ///     Occurs as a tab is being changed
    /// </summary>
    [Description("Occurs as a tab is being changed.")]
    public event EventHandler<TabPageChangeEventArgs> SelectedIndexChanging;

    /// <inheritdoc />
    protected override void OnCreateControl()
    {
        base.OnCreateControl();
        if (!DefaultStyle)
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }
    }

    /// <inheritdoc />
    public override void ResetBackColor()
    {
        _backcolor = Color.Empty;
        Invalidate();
    }

    /// <inheritdoc />
    protected override void OnParentBackColorChanged(EventArgs e)
    {
        base.OnParentBackColorChanged(e);
        Invalidate();
    }

    /// <inheritdoc />
    protected override void OnSelectedIndexChanged(EventArgs e)
    {
        base.OnSelectedIndexChanged(e);
        Invalidate();
    }

    /// <inheritdoc />
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        if (DefaultStyle)
            return;

        e.Graphics.Clear(BackColor);
        if (TabCount <= 0 || SelectedTab == null) return;

        //Draw a custom background for Transparent TabPages
        var border = SelectedTab.Bounds;
        border.Y++;
        border.Inflate(3, 3);

        //Draw a border around TabPage
        const ButtonBorderStyle bs = ButtonBorderStyle.Solid;
        using var paintBrush = new SolidBrush(BorderColor);
        ControlPaint.DrawBorder(e.Graphics, border,
            paintBrush.Color, BorderThickness, bs,
            paintBrush.Color, BorderThickness, bs,
            paintBrush.Color, BorderThickness, bs,
            paintBrush.Color, BorderThickness, bs);

        //Draw the Tabs
        for (var index = 0; index < TabCount; index++)
        {
            var tp = TabPages[index];
            var page = border;
            page.Inflate(-1, -1);
            e.Graphics.FillRectangle(new SolidBrush(tp.BackColor == Color.Transparent ? ActiveHeaderBackColor : tp.BackColor), page);

            var r = GetTabRect(index);
            if (Protrudes(ClientRectangle, r))
                continue;
            r.Inflate(1, 1);
            r.Width--;
            r.Height--;
            r.Y++;
            if (index == SelectedIndex)
                continue;

            if (_hoverIndex == index)
            {
                paintBrush.Color = BorderColor;
                e.Graphics.FillRectangle(new SolidBrush(HighlightBackColor), r);
                ControlPaint.DrawBorder(e.Graphics, r, paintBrush.Color, bs);
            }
            else
            {
                paintBrush.Color = BorderColor;
                e.Graphics.FillRectangle(new SolidBrush(HeaderBackColor), r);
                ControlPaint.DrawBorder(e.Graphics, r, paintBrush.Color, bs);
            }

            Draw(index, r, tp);
        }

        var ra = GetTabRect(SelectedIndex);
        ra.Inflate(1, 0);
        ra.Width--;
        ra.Height += 3;
        ra.Y -= 2;
        paintBrush.Color = BorderColor;
        e.Graphics.FillRectangle(new SolidBrush(ActiveHeaderBackColor), ra);
        ControlPaint.DrawBorder(e.Graphics, ra,
            paintBrush.Color, 1, bs,
            paintBrush.Color, 1, bs,
            paintBrush.Color, 1, bs,
            paintBrush.Color, 0, bs);

        Draw(SelectedIndex, ra, SelectedTab);


        void Draw(int index, Rectangle r, TabPage tp)
        {
            //Set up rotation for left and right aligned tabs
            if (Alignment is TabAlignment.Left or TabAlignment.Right)
            {
                float RotateAngle = 90;
                if (Alignment == TabAlignment.Left) RotateAngle = 270;
                var cp = new PointF(r.Left + (r.Width >> 1), r.Top + (r.Height >> 1));
                e.Graphics.TranslateTransform(cp.X, cp.Y);
                e.Graphics.RotateTransform(RotateAngle);
                r = new Rectangle(-(r.Height >> 1), -(r.Width >> 1), r.Height, r.Width);
            }

            //Draw the Tab Text
            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            if (tp.Enabled)
            {
                //TextRenderer.DrawText(e.Graphics, tp.Text, Font, r,
                //_hoverIndex == index && index != SelectedIndex ? HighlightForeColor : HeaderForeColor);
                using var brush = new SolidBrush(_hoverIndex == index && index != SelectedIndex ? HighlightForeColor : HeaderForeColor);
                e.Graphics.DrawString(tp.Text,Font,brush,r,sf);
            }
            else
                ControlPaint.DrawStringDisabled(e.Graphics, tp.Text, Font, tp.ForeColor, r, sf);

            e.Graphics.ResetTransform();
        }
    }

    private static bool Protrudes(Rectangle rec1, Rectangle rec2)
    {
        return !rec1.Contains(rec2) && rec1.IntersectsWith(rec2);
    }

    /// <inheritdoc />
    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (DefaultStyle)
        {
            base.OnMouseMove(e);
            return;
        }

        for (var i = 0; i < TabCount; i++)
            if (GetTabRect(i).Contains(e.Location))
            {
                if (_hoverIndex != i)
                {
                    _hoverIndex = i;
                    Invalidate();
                }

                return;
            }

        if (_hoverIndex != -1)
        {
            _hoverIndex = -1;
            Invalidate();
        }
    }

    /// <inheritdoc />
    protected override void OnMouseLeave(EventArgs e)
    {
        if (DefaultStyle)
        {
            base.OnMouseLeave(e);
            return;
        }

        if (_hoverIndex != -1)
        {
            _hoverIndex = -1;
            Invalidate();
        }
    }

    /// <inheritdoc />
    protected override void WndProc(ref Message m)
    {
        if (m.Msg == (int)(WM_REFLECT + Win32.WM.NOTIFY))
        {
            var hdr = (Win32.NMHDR)Marshal.PtrToStructure(m.LParam, typeof(Win32.NMHDR));
            if (hdr.code == TCN_SELCHANGING)
            {
                var tp = TestTab(PointToClient(Cursor.Position));
                if (tp != null)
                {
                    var e = new TabPageChangeEventArgs(SelectedTab, tp);
                    SelectedIndexChanging?.Invoke(this, e);
                    if (e.Cancel || tp.Enabled == false)
                    {
                        m.Result = new IntPtr(1);
                        return;
                    }
                }
            }
        }

        base.WndProc(ref m);
    }

    private TabPage TestTab(Point pt)
    {
        for (var index = 0; index <= TabCount - 1; index++)
            if (GetTabRect(index).Contains(pt.X, pt.Y))
                return TabPages[index];
        return null;
    }

    /// <summary>Raises the <see cref="IExControl.DefaultStyleChanged" /> event.</summary>
    protected virtual void OnDefaultStyleChanged() => DefaultStyleChanged?.Invoke(this, EventArgs.Empty);
}

/// <summary>
///     Event arguments for the <see cref="ExTabControl.SelectedIndexChanging"/> event.
/// </summary>
public class TabPageChangeEventArgs : EventArgs
{
    /// <summary>
    ///     Gets or sets whether the event should be canceled.
    /// </summary>
    public bool Cancel { get; set; }

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="currentTab">current Tab</param>
    /// <param name="newTab">new tab</param>
    public TabPageChangeEventArgs(TabPage currentTab, TabPage newTab)
    {
        CurrentTab = currentTab;
        NewTab = newTab;
    }

    /// <summary>
    ///     Gets the current selected Tab.
    /// </summary>
    public TabPage CurrentTab { get; }


    /// <summary>
    ///     Gets the Tab that is to become current.
    /// </summary>
    public TabPage NewTab { get; }
}