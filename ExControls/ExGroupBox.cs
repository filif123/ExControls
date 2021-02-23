using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ExControls
{
    /// <summary>
    ///     Expanded GroupBox Control
    /// </summary>
    [ToolboxBitmap(typeof(GroupBox), "GroupBox.bmp")]
    public partial class ExGroupBox : GroupBox, IExControl
    {
        private Color _borderColor;
        private bool _defaultStyle;
        private int _borderThickness;
        private DashStyle _borderStyle;

        /// <summary>Occurs when the <see cref="BorderColor" /> property changes.</summary>
        [Category("Changed Property")]
        [Description("Occurs when the BorderColor property changes.")]
        public event EventHandler BorderColorChanged;

        /// <summary>Occurs when the <see cref="BorderThickness" /> property changes.</summary>
        [Category("Changed Property")]
        [Description("Occurs when the BorderThickness property changes.")]
        public event EventHandler BorderThicknessChanged;

        /// <summary>Occurs when the <see cref="DefaultStyle" /> property changes.</summary>
        [Category("Changed Property")]
        [Description("Occurs when the DefaultStyle property changes.")]
        public event EventHandler DefaultStyleChanged;

        /// <summary>Occurs when the <see cref="BorderStyle" /> property changes.</summary>
        [Category("Changed Property")]
        [Description("Occurs when the BorderStyle property changes.")]
        public event EventHandler BorderStyleChanged;

        /// <summary>Occurs when the Line is drawing.</summary>
        [Category("Appearance")]
        [Description("Occurs when the Line is drawing.")]
        public event EventHandler<LinePenEventArgs> LineDrawing;

        /// <summary>
        ///     Constructor
        /// </summary>
        public ExGroupBox()
        {
            InitializeComponent();
            InitInternal();
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public ExGroupBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            InitInternal();
        }

        private void InitInternal()
        {
            _defaultStyle = true;
            _borderThickness = 1;
            _borderColor = Color.LightGray;
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
                Invalidate();
                OnDefaultStyleChanged();
            }
        }

        /// <summary>
        ///     Width of the GroupBox's border
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(1)]
        [Description("Width of the GroupBox's border.")]
        public int BorderThickness
        {
            get => _borderThickness;
            set
            {
                if (_borderThickness == value)
                    return;
                _borderThickness = value;
                Invalidate();
                OnBorderThicknessChanged();
            }
        }

        /// <summary>
        ///     Color of the GroupBox's border
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "LightGray")]
        [Description("Color of the GroupBox's border.")]
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                if (_borderColor == value)
                    return;
                _borderColor = value;
                Invalidate();
                OnBorderColorChanged();
            }
        }

        /// <summary>
        ///     Style of the GroupBox's border
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(DashStyle), "Solid")]
        [Description("Style of the GroupBox's border.")]
        public DashStyle BorderStyle
        {
            get => _borderStyle;
            set
            {
                if (_borderStyle == value)
                    return;
                _borderStyle = value;
                Invalidate();
            }
        }

        /// <inheritdoc />
        protected override void OnPaint(PaintEventArgs e)
        {
            if (DefaultStyle)
            {
                base.OnPaint(e);
                return;
            }

            using Brush textBrush = new SolidBrush(ForeColor);
            using Brush borderBrush = new SolidBrush(BorderColor);
            using var borderPen = new Pen(borderBrush, BorderThickness) {DashStyle = BorderStyle};
            SizeF strSize = TextRenderer.MeasureText(e.Graphics, Text, Font);
            var rect = new Rectangle(ClientRectangle.X, ClientRectangle.Y + (int)(strSize.Height / 2), ClientRectangle.Width - 1, ClientRectangle.Height - (int)(strSize.Height / 2) - 1);

            OnLineDrawing(new LinePenEventArgs(borderPen));

            // Clear text and border
            e.Graphics.Clear(BackColor);

            // Draw text
            TextRenderer.DrawText(e.Graphics, Text, Font, new Point(Padding.Left, 0), ForeColor);

            // Drawing Border
            //Left
            e.Graphics.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
            //Right
            e.Graphics.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
            //Bottom
            e.Graphics.DrawLine(borderPen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
            //Top1
            e.Graphics.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + Padding.Left, rect.Y));
            //Top2
            e.Graphics.DrawLine(borderPen, new Point(rect.X + Padding.Left + (int)strSize.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y));
        }

        /// <summary>Raises the <see cref="BorderColorChanged" /> event.</summary>
        protected virtual void OnBorderColorChanged()
        {
            BorderColorChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>Raises the <see cref="BorderThicknessChanged" /> event.</summary>
        protected virtual void OnBorderThicknessChanged()
        {
            BorderThicknessChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>Raises the <see cref="DefaultStyleChanged" /> event.</summary>
        protected virtual void OnDefaultStyleChanged()
        {
            DefaultStyleChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>Raises the <see cref="BorderStyleChanged" /> event.</summary>
        protected virtual void OnBorderStyleChanged()
        {
            BorderStyleChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>Raises the <see cref="LineDrawing" /> event.</summary>
        protected virtual void OnLineDrawing(LinePenEventArgs e)
        {
            LineDrawing?.Invoke(this, e);
        }
    }
}
