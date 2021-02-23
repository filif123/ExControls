using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;

namespace ExControls
{
    public partial class ExForm : Form
    {
        private bool _autoForeColor;
        private Color _titleBarBackColor;
        private bool _autoButtonColors;
        private int _borderThickness;

        /// <inheritdoc />
        public ExForm()
        {
            InitializeComponent();

            _autoForeColor = true;
            _titleBarBackColor = SystemColors.Control;
            Icon = ApplicationIcon;
            ToolStripLeft.Renderer = new CustomToolStripProfessionalRenderer();
            Invalidate(true);
        }

        /// <summary>
        ///     Gets or sets whether Button sets ForeColor and HighlightForeColor according to BackColor of the parent and HighlightBackColor darken according to BackColor
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(true)]
        [Description("Gets or sets whether Button sets ForeColor and HighlightForeColor according to BackColor of the parent and HighlightBackColor darken according to BackColor.")]
        public bool AutoButtonColors
        {
            get => _autoButtonColors;
            set
            {
                if (_autoButtonColors == value)
                    return;
                _autoButtonColors = value;

                if (value)
                {
                    foreach (Control control in RightMenu.Controls)
                    {
                        if (control is ExTitleButton etb)
                        {
                            etb.HighlightBackColor = ControlPaint.Dark(etb.BackColor);
                            etb.HighlightForeColor = ExUtils.GetContrastForeColor(etb.HighlightBackColor);
                            etb.ForeColor = ExUtils.GetContrastForeColor(etb.BackColor);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Gets or sets whether Button sets ForeColor and HighlightForeColor according to BackColor of the parent and HighlightBackColor darken according to BackColor
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(true)]
        [Description("Gets or sets whether Button sets ForeColor and HighlightForeColor according to BackColor of the parent and HighlightBackColor darken according to BackColor.")]
        public bool AutoForeColor
        {
            get => _autoForeColor;
            set
            {
                if (_autoForeColor == value)
                    return;

                _autoForeColor = value;

                if (value) 
                    TitleName.ForeColor = ExUtils.GetContrastForeColor(FormTitle.BackColor);

                Invalidate();
            }
        }

        /// <summary>
        ///     Background color of the TitleBar
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(SystemColors), "Control")]
        [Description("Background color of the TitleBar.")]
        public Color TitleBarBackColor
        {
            get => _titleBarBackColor;
            set
            {
                if (_titleBarBackColor == value)
                    return;
                
                if (AutoForeColor)
                {
                    TitleName.ForeColor = ExUtils.GetContrastForeColor(_titleBarBackColor); 
                }

                _titleBarBackColor = value;
                FormTitle.BackColor = _titleBarBackColor;
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

        /// <summary>Gets or sets the text associated with this control.</summary>
        /// <returns>The text associated with this control.</returns>
        [Browsable(true)]
        public override string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;
                TitleName.Text = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(true)]
        public new Icon Icon
        {
            get => base.Icon;
            set
            {
                base.Icon = value;
                AppIcon.Image = value.ToBitmap();
            }
        }

        internal static Icon ApplicationIcon => Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);

        /// <inheritdoc />
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        private void FormTitle_DoubleClick(object sender, EventArgs e)
        {
            ChangeWindowState();
        }



        private void ChangeWindowState()
        {
            if (WindowState == FormWindowState.Normal) WindowState = FormWindowState.Maximized;
            if (WindowState == FormWindowState.Maximized) WindowState = FormWindowState.Normal;
        }

        private void RightMenu_ControlAdded(object sender, ControlEventArgs e)
        {
            if (e.Control is ExTitleButton {AutoButtonColors: true} etb)
            {
                etb.HighlightBackColor = ControlPaint.Dark(etb.BackColor);
                etb.HighlightForeColor = ExUtils.GetContrastForeColor(etb.HighlightBackColor);
                etb.ForeColor = ExUtils.GetContrastForeColor(etb.BackColor);
            }
        }

        /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.</summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, 
                TitleBarBackColor, BorderThickness, ButtonBorderStyle.Solid,
                Color.White, 0, ButtonBorderStyle.None,
                TitleBarBackColor, BorderThickness, ButtonBorderStyle.Solid, 
                TitleBarBackColor, BorderThickness, ButtonBorderStyle.Solid);
        }

        private void TitleButtonExit_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(TitleButtonExit.Hover ? TitleButtonExit.HighlightBackColor : TitleButtonExit.BackColor);
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            var middle = new Point(e.ClipRectangle.Width / 2, e.ClipRectangle.Height / 2);

            Color color = TitleButtonExit.ForeColor;
            if (TitleButtonExit.Hover) color = TitleButtonExit.HighlightForeColor;
            var pen = new Pen(new SolidBrush(color));

            e.Graphics.DrawLine(pen, middle.X - 5, middle.Y - 5, middle.X + 5, middle.Y + 5);
            e.Graphics.DrawLine(pen, middle.X - 5, middle.Y + 5, middle.X + 5, middle.Y - 5);
        }

        internal class CustomToolStripProfessionalRenderer : ToolStripProfessionalRenderer
        {
            public CustomToolStripProfessionalRenderer() : base(new ProfessionalColorTable())
            {

            }

            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                // Don't draw a border
            }
        }

        private void ToolStripLeft_ItemAdded(object sender, ToolStripItemEventArgs e)
        {

        }
    }
}
