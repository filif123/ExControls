using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ExControls
{
    /// <summary>
    ///     Expanded TabControl Control
    /// </summary>
    [ToolboxBitmap(typeof(TabControl), "TabControl.bmp")]
    public partial class ExTabControl : TabControl, IExControl
    {
        private const int TCN_FIRST = -550;
        private const int TCN_SELCHANGING = TCN_FIRST - 2;
        private const int WM_USER = 0x400;
        private const int WM_NOTIFY = 0x4E;
        private const int WM_REFLECT = WM_USER + 0x1C00;

        private Color _backcolor = Color.Empty;
        private Color _activeHeaderBackColor;
        private Color _activeHeaderForeColor;
        private Color _borderColor;
        private Color _headerBackColor;
        private Color _headerForeColor;
        private Color _highlightBackColor;
        private Color _highlightForeColor;
        
        private bool _defaultStyle;
        private int _hoverIndex = -1;
        private int _borderThickness = 1;

        /// <summary>
        ///     Occurs as a tab is being changed
        /// </summary>
        [Description("Occurs as a tab is being changed.")]
        public event EventHandler<TabPageChangeEventArgs> SelectedIndexChanging;

        /// <summary>Occurs when the <see cref="IExControl.DefaultStyle" /> property changes.</summary>
        [Category("Changed Property")]
        [Description("Occurs when the BorderColor property changes.")]
        public event EventHandler DefaultStyleChanged;

        /// <summary>
        ///     Constructor
        /// </summary>
        public ExTabControl()
        {
            InitializeComponent();
            InitInternal();
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="container"></param>
        public ExTabControl(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            InitInternal();
        }

        private void InitInternal()
        {
            _defaultStyle = true;
            _borderColor = Color.LightGray;
            _headerForeColor = Color.Black;
            _headerBackColor = Color.LightGray;
            _activeHeaderForeColor = Color.Black;
            _activeHeaderBackColor = Color.White;
            _highlightBackColor = Color.LightSteelBlue;
            _highlightForeColor = Color.White;
            _borderThickness = 1;

            Invalidate();
        }

        /// <inheritdoc />
        [Browsable(true)]
        [Category("Appearance")]
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
        ///     Color of the TabControl's border
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
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
        [Category("Appearance")]
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
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "LightGray")]
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
        [Category("Appearance")]
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
        [Category("Appearance")]
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
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "LightSteelBlue")]
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
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "White")]
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
        [Category("Appearance")]
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
            if (TabCount <= 0) return;
            //Draw a custom background for Transparent TabPages
            Rectangle r = SelectedTab.Bounds;
            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

            //Draw a border around TabPage
            r.Inflate(1, 1);
            const ButtonBorderStyle bs = ButtonBorderStyle.Solid;
            using var PaintBrush = new SolidBrush(BorderColor);
            r.Inflate(2, 2);
            ControlPaint.DrawBorder(e.Graphics, r,
                PaintBrush.Color, BorderThickness, bs,
                PaintBrush.Color, BorderThickness, bs,
                PaintBrush.Color, BorderThickness, bs,
                PaintBrush.Color, BorderThickness, bs);

            //Draw the Tabs
            for (var index = 0; index < TabCount; index++)
            {
                TabPage tp = TabPages[index];
                r = GetTabRect(index);
                if (index == SelectedIndex)
                {
                    PaintBrush.Color = BorderColor;
                    e.Graphics.FillRectangle(new SolidBrush(ActiveHeaderBackColor), r);
                    ControlPaint.DrawBorder(e.Graphics, r,
                        PaintBrush.Color, 1, bs,
                        PaintBrush.Color, 1, bs,
                        PaintBrush.Color, 1, bs,
                        ActiveHeaderBackColor, BorderThickness * 2, bs);
                }
                else if (_hoverIndex == index)
                {
                    PaintBrush.Color = BorderColor;
                    e.Graphics.FillRectangle(new SolidBrush(HighlightBackColor), r);
                    ControlPaint.DrawBorder(e.Graphics, r, PaintBrush.Color, bs);
                }
                else
                {
                    PaintBrush.Color = BorderColor;
                    e.Graphics.FillRectangle(new SolidBrush(HeaderBackColor), r);
                    ControlPaint.DrawBorder(e.Graphics, r, PaintBrush.Color, bs);
                }

                //Set up rotation for left and right aligned tabs
                if (Alignment == TabAlignment.Left || Alignment == TabAlignment.Right)
                {
                    float RotateAngle = 90;
                    if (Alignment == TabAlignment.Left) RotateAngle = 270;
                    var cp = new PointF(r.Left + (r.Width >> 1), r.Top + (r.Height >> 1));
                    e.Graphics.TranslateTransform(cp.X, cp.Y);
                    e.Graphics.RotateTransform(RotateAngle);
                    r = new Rectangle(-(r.Height >> 1), -(r.Width >> 1), r.Height, r.Width);
                }

                //Draw the Tab Text
                if (tp.Enabled)
                    TextRenderer.DrawText(e.Graphics, tp.Text, Font, r, (_hoverIndex == index && index != SelectedIndex) ? HighlightForeColor : HeaderForeColor);
                else
                    ControlPaint.DrawStringDisabled(e.Graphics, tp.Text, Font, tp.ForeColor, r, sf);

                e.Graphics.ResetTransform();
            }
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
            if (m.Msg == WM_REFLECT + WM_NOTIFY)
            {
                var hdr = (NativeMethods.NMHDR) Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.NMHDR));
                if (hdr.code == TCN_SELCHANGING)
                {
                    TabPage tp = TestTab(PointToClient(Cursor.Position));
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
        protected virtual void OnDefaultStyleChanged()
        {
            DefaultStyleChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <inheritdoc />
    public class TabPageChangeEventArgs : EventArgs
    {
        /// <summary>
        ///     Event bol zruseny
        /// </summary>
        public bool Cancel = false;

        /// <summary>
        ///     Konstruktor
        /// </summary>
        /// <param name="CurrentTab"></param>
        /// <param name="NextTab"></param>
        public TabPageChangeEventArgs(TabPage CurrentTab, TabPage NextTab)
        {
            this.CurrentTab = CurrentTab;
            this.NextTab = NextTab;
        }

        /// <summary>
        /// </summary>
        public TabPage CurrentTab { get; }


        /// <summary>
        /// </summary>
        public TabPage NextTab { get; }
    }
}