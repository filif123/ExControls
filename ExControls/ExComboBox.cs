using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace ExControls
{
    /// <summary>
    ///     Expanded ComboBox Control
    /// </summary>
    [ToolboxBitmap(typeof(ComboBox),"ComboBox.bmp")]
    public partial class ExComboBox : ComboBox, IExControl
    {
        private Color _dropDownSelectedRowBackColor;
        private bool _defaultStyle;

        private bool _hover;
        private bool _selected;
        private bool _drawing;

        /// <summary>Occurs when the <see cref="IExControl.DefaultStyle" /> property changes.</summary>
        [Category("Changed Property")]
        [Description("Occurs when the BorderColor property changes.")]
        public event EventHandler DefaultStyleChanged;

        /// <summary>Occurs when the <see cref="DropDownSelectedRowBackColor" /> property changes.</summary>
        [Category("Changed Property")]
        [Description("Occurs when the DropDownSelectedRowBackColor property changes.")]
        public event EventHandler DropDownSelectedRowBackColorChanged;

        /// <summary>
        ///     Constructor
        /// </summary>
        public ExComboBox()
        {
            InitializeComponent();
            InitInternal();
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="container"></param>
        public ExComboBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            InitInternal();
        }

        private void InitInternal()
        {
            _defaultStyle = true;

            StyleNormal = new ExComboBoxStyle();
            StyleHighlight = new ExComboBoxStyle();
            StyleSelected = new ExComboBoxStyle();
            StyleDisabled = new ExComboBoxStyle();

            StyleNormal.PropertyChanged += StyleOnPropertyChanged;
            StyleDisabled.PropertyChanged += StyleOnPropertyChanged;
            StyleHighlight.PropertyChanged += StyleOnPropertyChanged;
            StyleSelected.PropertyChanged += StyleOnPropertyChanged;

            _dropDownSelectedRowBackColor = SystemColors.Highlight;

            DoubleBuffered = true;
            Invalidate();
        }

        private void StyleOnPropertyChanged(object sender, ExPropertyChangedEventArgs e)
        {
            if (!_drawing) Invalidate();
        }

        /// <inheritdoc/>
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
                DrawMode = value ? DrawMode.Normal : DrawMode.OwnerDrawFixed;
                Invalidate();
                OnDefaultStyleChanged();
            }
        }

        /// <summary>
        ///     Normal style of the Control (when is inactive).
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("Normal style of the Control (when is inactive).")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ExComboBoxStyle StyleNormal { get; set; }

        /// <summary>
        /// Highlight style of the Control (when mouse is over control).
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("Highlight style of the Control (when mouse is over control).")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ExComboBoxStyle StyleHighlight { get; set; }

        /// <summary>
        ///     Selected style of the Control (when control is selected).
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("Selected style of the Control (when control is selected).")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ExComboBoxStyle StyleSelected { get; set; }

        /// <summary>
        ///     Disabled style of the Control (when control is not Enabled).
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("Disabled style of the Control (when control is not Enabled).")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ExComboBoxStyle StyleDisabled { get; set; }

        /// <summary>
        ///     Color of the selected row in drop down menu
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(SystemColors), "Highlight")]
        [Description("Color of the selected row in drop down menu.")]
        public Color DropDownSelectedRowBackColor
        {
            get => _dropDownSelectedRowBackColor;
            set
            {
                if (_dropDownSelectedRowBackColor == value)
                    return;
                _dropDownSelectedRowBackColor = value; 
                Invalidate();
                OnDropDownSelectedRowBackColorChanged();
            }
        }

        /// <summary>
        ///     Draw mode of ComboBox
        /// </summary>
        public new DrawMode DrawMode
        {
            get => base.DrawMode;
            set
            {
                if (value != DrawMode.Normal)
                {
                    _defaultStyle = false;
                }

                base.DrawMode = value;
            }
        }

        /// <inheritdoc />
        [Browsable(false)]
        public override Color BackColor
        {
            get => base.BackColor;
            set 
            {
                StyleNormal.BackColor = value;
                base.BackColor = value;
            }
        }

        /// <inheritdoc />
        [Browsable(false)]
        public override Color ForeColor
        {
            get => base.ForeColor;
            set
            {
                StyleNormal.ForeColor = value;
                base.ForeColor = value;
            }
        }

        /// <inheritdoc />
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 15 && !DefaultStyle)
            {
                using Graphics g = Graphics.FromHwnd(m.HWnd);
                OnPaint(new PaintEventArgs(g, ClientRectangle));
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _drawing = true;

            Size texts = TextRenderer.MeasureText(e.Graphics, Text, Font);
            Point textStart = new Point(2, (int) Math.Round(ClientRectangle.Height / 2d - texts.Height / 2d));
            Rectangle dropButton = new Rectangle(Width - 20, 0, 20, Height);

            Color back = StyleNormal.BackColor ??= Color.White; 
            Color fore = StyleNormal.ForeColor ??= Color.Black; 
            Color border = StyleNormal.BorderColor ??= Color.DimGray;
            Color arrow = StyleNormal.ArrowColor ??= Color.Black;
            Color backbut = StyleNormal.ButtonBackColor ??= back;
            Color borbut = StyleNormal.ButtonBorderColor ??= back;
            bool bbfirst = StyleNormal.ButtonRenderFirst ??= true;

            if (!Enabled)
            {
                StyleDisabled.BackColor = back = StyleDisabled.BackColor ?? Parent.BackColor;
                StyleDisabled.ForeColor = fore = StyleDisabled.ForeColor ?? SystemColors.GrayText;
                if (StyleDisabled.BorderColor.HasValue) border = StyleDisabled.BorderColor.Value;
                StyleDisabled.ArrowColor = arrow = StyleDisabled.ArrowColor ?? SystemColors.GrayText;
                StyleDisabled.ButtonBackColor = backbut = StyleDisabled.ButtonBackColor ?? back;
                StyleDisabled.ButtonBorderColor = borbut = StyleDisabled.ButtonBorderColor ?? back;
                if (StyleDisabled.ButtonRenderFirst.HasValue) bbfirst = StyleDisabled.ButtonRenderFirst.Value;
            }
            else if (_selected)
            {
                if (StyleSelected.BackColor.HasValue) back = StyleSelected.BackColor.Value;
                if (StyleSelected.ForeColor.HasValue) fore = StyleSelected.ForeColor.Value;
                StyleSelected.BorderColor = border = StyleSelected.BorderColor ?? SystemColors.Highlight;
                if (StyleSelected.ArrowColor.HasValue) arrow = StyleSelected.ArrowColor.Value;
                if (StyleSelected.ButtonBackColor.HasValue) backbut = StyleSelected.ButtonBackColor.Value;
                if (StyleSelected.ButtonBorderColor.HasValue) borbut = StyleSelected.ButtonBorderColor.Value;
                if (StyleSelected.ButtonRenderFirst.HasValue) bbfirst = StyleSelected.ButtonRenderFirst.Value;
            }
            else if (_hover)
            {
                if (StyleHighlight.BackColor.HasValue) back = StyleHighlight.BackColor.Value;
                if (StyleHighlight.ForeColor.HasValue) fore = StyleHighlight.ForeColor.Value;
                StyleHighlight.BorderColor = border = StyleHighlight.BorderColor ?? SystemColors.Highlight;
                if (StyleHighlight.ArrowColor.HasValue) arrow = StyleHighlight.ArrowColor.Value;
                if (StyleHighlight.ButtonBackColor.HasValue) backbut = StyleHighlight.ButtonBackColor.Value;
                if (StyleHighlight.ButtonBorderColor.HasValue) borbut = StyleHighlight.ButtonBorderColor.Value;
                if (StyleHighlight.ButtonRenderFirst.HasValue) bbfirst = StyleHighlight.ButtonRenderFirst.Value;
            }

            e.Graphics.Clear(back);

            using var penBorder = new Pen(border);

            if (bbfirst)
            {
                ExButtonsRenderer.DrawDropDownButton(e.Graphics, dropButton, backbut, borbut, arrow);
                e.Graphics.DrawRectangle(penBorder, 0, 0, Width - 1, Height - 1);
            }
            else
            {
                e.Graphics.DrawRectangle(penBorder, 0, 0, Width - 1, Height - 1);
                ExButtonsRenderer.DrawDropDownButton(e.Graphics, dropButton, backbut, borbut, arrow);
            }

            if (DropDownStyle == ComboBoxStyle.DropDownList)
                TextRenderer.DrawText(e.Graphics, Text, Font, textStart, fore, back, RightToLeft == RightToLeft.Yes ? TextFormatFlags.Right : TextFormatFlags.Default);
            
            _drawing = false;
        }

        /// <inheritdoc />
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            
            if (!DefaultStyle)
            {
                SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            }
        }

        /// <inheritdoc />
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (DefaultStyle)
            {
                base.OnDrawItem(e);
                return;
            }
            
            e.DrawBackground();

            if (e.Index >= 0)
            {
                Brush bBrush = ((e.State & DrawItemState.Selected) == DrawItemState.Selected) ? new SolidBrush(DropDownSelectedRowBackColor) : new SolidBrush(StyleNormal.BackColor ?? e.BackColor);

                e.Graphics.FillRectangle(bBrush, e.Bounds);
                TextRenderer.DrawText(e.Graphics, Items[e.Index].ToString(),e.Font,e.Bounds.Location, StyleNormal.ForeColor ?? e.ForeColor);
            }
            
            e.DrawFocusRectangle();

            base.OnDrawItem(e);
        }

        /// <inheritdoc />
        protected override void OnMouseEnter(EventArgs eventargs)
        {
            base.OnMouseEnter(eventargs);
            if (DefaultStyle)
                return;

            if (!_hover)
            {
                _hover = true;
                Invalidate();
            }
        }

        /// <inheritdoc />
        protected override void OnMouseLeave(EventArgs eventargs)
        {
            base.OnMouseLeave(eventargs);
            if (DefaultStyle)
                return;

            if (_hover && !_selected)
            {
                _hover = false;
                Invalidate();
            }
        }

        /// <inheritdoc />
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if (DefaultStyle)
                return;

            if (!_selected)
            {
                _hover = true;
                _selected = true;
                Invalidate();
            }
        }

        /// <inheritdoc />
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (DefaultStyle)
                return;

            if (_selected)
            {
                _hover = false;
                _selected = false;
                Invalidate();
            }
        }

        /// <inheritdoc />
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            if (DefaultStyle)
                return;

            if (!_selected)
            {
                _hover = true;
                _selected = true;
                Invalidate();
            }
        }

        /// <inheritdoc />
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            if (DefaultStyle)
                return;

            if (_selected)
            {
                _hover = false;
                _selected = false;
                Invalidate();
            }
        }

        /// <summary>Raises the <see cref="DropDownSelectedRowBackColorChanged" /> event.</summary>
        protected virtual void OnDropDownSelectedRowBackColorChanged()
        {
            DropDownSelectedRowBackColorChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>Raises the <see cref="IExControl.DefaultStyleChanged" /> event.</summary>
        protected virtual void OnDefaultStyleChanged()
        {
            DefaultStyleChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    ///     Class for definition styles for ExComboBox
    /// </summary>
    public class ExComboBoxStyle : ExStyle
    {
        private Color? _arrowColor;
        private Color? _buttonBackColor;
        private Color? _buttonBorderColor;
        private bool? _buttonRenderFirst;

        public ExComboBoxStyle()
        {
        }

        protected ExComboBoxStyle(ExComboBoxStyle copy) : base(copy)
        {
            ArrowColor = copy.ArrowColor;
            ButtonBackColor = copy.ButtonBackColor;
            ButtonBorderColor = copy.ButtonBorderColor;
            ButtonRenderFirst = copy.ButtonRenderFirst;
        }

        /// <summary>
        ///     Color of the arrow which is in this Control as the dropdown button
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [NotifyParentProperty(true)]
        [DefaultValue(typeof(Color), "Black")]
        [Description("Color of the arrow which is in this Control as the dropdown button.")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        public Color? ArrowColor
        {
            get => _arrowColor;
            set
            {
                if (_arrowColor == value)
                    return;
                _arrowColor = value;
                OnPropertyChanged(new ExPropertyChangedEventArgs(nameof(ArrowColor), value));
            }
        }

        /// <summary>
        ///     Background color of the dropdown button
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [NotifyParentProperty(true)]
        [DefaultValue(typeof(Color), "White")]
        [Description("Background color of the dropdown button.")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        public Color? ButtonBackColor
        {
            get => _buttonBackColor;
            set
            {
                if (_buttonBackColor == value)
                    return;
                _buttonBackColor = value;
                OnPropertyChanged(new ExPropertyChangedEventArgs(nameof(ButtonBackColor), value));
            }
        }

        /// <summary>
        ///     Border color of the dropdown button
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [NotifyParentProperty(true)]
        [DefaultValue(typeof(Color), "DimGray")]
        [Description("Border color of the dropdown button.")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        public Color? ButtonBorderColor
        {
            get => _buttonBorderColor;
            set
            {
                if (_buttonBorderColor == value)
                    return;
                _buttonBorderColor = value;
                OnPropertyChanged(new ExPropertyChangedEventArgs(nameof(ButtonBorderColor), value));
            }
        }

        /// <summary>
        ///     Gets or sets whether DropDown button has to draw first
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("Appearance")]
        [NotifyParentProperty(true)]
        [Description("Gets or sets whether DropDown button has to be rendered first.")]
        public bool? ButtonRenderFirst
        {
            get => _buttonRenderFirst;
            set
            {
                if (_buttonRenderFirst == value)
                    return;
                _buttonRenderFirst = value;
                OnPropertyChanged(new ExPropertyChangedEventArgs(nameof(ButtonRenderFirst), value));
            }
        }

        public override object Clone()
        {
            return new ExComboBoxStyle(this);
        }
    }
}
