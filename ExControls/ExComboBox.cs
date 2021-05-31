using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
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
        private Color _dropDownBackColor;
        private bool _defaultStyle;

        private bool _hover;
        private bool _selected;
        private bool _drawing;
        private bool _wasDropDown;

        private SolidBrush listbrush;

        /// <summary>Occurs when the <see cref="IExControl.DefaultStyle" /> property changes.</summary>
        [Category("Changed Property")]
        [Description("Occurs when the BorderColor property changes.")]
        public event EventHandler DefaultStyleChanged;

        /// <summary>Occurs when the <see cref="DropDownSelectedRowBackColor" /> property changes.</summary>
        [Category("Changed Property")]
        [Description("Occurs when the DropDownSelectedRowBackColor property changes.")]
        public event EventHandler DropDownSelectedRowBackColorChanged;

        /// <summary>Occurs when the <see cref="DropDownBackColor" /> property changes.</summary>
        [Category("Changed Property")]
        [Description("Occurs when the DropDownBackColor property changes.")]
        public event EventHandler DropDownBackColorChanged;

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
            _dropDownBackColor = Color.White;

            DoubleBuffered = true;
            listbrush = new SolidBrush(DropDownBackColor);

            Invalidate();
        }

        private void StyleOnPropertyChanged(object sender, ExPropertyChangedEventArgs e)
        {
            if (!_drawing)
            {
                Invalidate();
            }
        }

        /// <summary>
        ///     Don't use directly in code.
        /// </summary>
        [Browsable(false)]
        public Color ActualBackColor { get; private set; }

        /// <summary>
        ///     Don't use directly in code.
        /// </summary>
        [Browsable(false)]
        public Color ActualForeColor { get; private set; }

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
        ///     Background color of the drop down menu.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "White")]
        [Description("Background color of the drop down menu.")]
        public Color DropDownBackColor
        {
            get => _dropDownBackColor;
            set
            {
                if (_dropDownBackColor == value)
                    return;
                _dropDownBackColor = value;
                Invalidate();
                OnDropDownBackColorChanged();
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

        public new bool Enabled
        {
            get => base.Enabled;
            set {
                if (!value && !DefaultStyle && DropDownStyle == ComboBoxStyle.DropDown)
                {
                    _wasDropDown = true;
                    DropDownStyle = ComboBoxStyle.DropDownList;
                }
                else if(value && !DefaultStyle && _wasDropDown)
                {
                    _wasDropDown = false;
                    DropDownStyle = ComboBoxStyle.DropDown;
                }

                base.Enabled = value;
            }
        }

        /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.</summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnHandleCreated(EventArgs e)
        {
            if (!DefaultStyle)
            {
                Win32.SetWindowTheme(Handle, "", "");
            }
            base.OnHandleCreated(e);
        }

        /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleDestroyed" /> event.</summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnHandleDestroyed(EventArgs e)
        {
            listbrush.Dispose();
            base.OnHandleDestroyed(e);
        }

        /// <summary>Raises the <see cref="E:System.Windows.Forms.ComboBox.SelectedIndexChanged" /> event.</summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            Invalidate();
        }

        /// <inheritdoc />
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (!DefaultStyle)
            {
                switch ((Win32.WM)m.Msg)
                {
                    case Win32.WM.PAINT:
                    {
                        using Graphics g = Graphics.FromHwnd(m.HWnd);
                        OnPaint(new PaintEventArgs(g, ClientRectangle));
                        break;
                    }
                    case Win32.WM.CTLCOLORLISTBOX:
                    {
                        listbrush.Dispose();
                        listbrush = new SolidBrush(DropDownBackColor);
                        m.Result = GetHbrush(listbrush);
                        break;
                    }
                }
            }
        }

        private IntPtr GetHbrush(Brush b)
        {
            FieldInfo field = typeof(Brush).GetField("nativeBrush", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field is not null) 
                return (IntPtr)field.GetValue(b);
            return IntPtr.Zero;
        }

        /// <inheritdoc />
        protected override void OnPaint(PaintEventArgs e)
        {
            _drawing = true;

            if (DefaultStyle)
            {
                base.OnPaint(e);
                return;
            }

            Graphics g = e.Graphics;

            Size texts = TextRenderer.MeasureText(g, Text, Font);
            var textStart = new Point(2, (int) Math.Round(ClientRectangle.Height / 2d - texts.Height / 2d));
            var dropButton = new Rectangle(Width - 20, 0, 20, Height);

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

            ActualBackColor = back;
            ActualForeColor = fore;

            if (ActualBackColor != base.BackColor) base.BackColor = ActualBackColor;
            if (ActualForeColor != base.ForeColor) base.ForeColor = ActualForeColor;

            g.Clear(back);

            using var penBorder = new Pen(border);

            if (bbfirst)
            {
                ExButtonsRenderer.DrawDropDownButton(g, dropButton, backbut, borbut, arrow);
                g.DrawRectangle(penBorder, 0, 0, Width - 1, Height - 1);
            }
            else
            {
                g.DrawRectangle(penBorder, 0, 0, Width - 1, Height - 1);
                ExButtonsRenderer.DrawDropDownButton(g, dropButton, backbut, borbut, arrow);
            }

            if (DropDownStyle == ComboBoxStyle.DropDownList)
                TextRenderer.DrawText(g, Text, Font, textStart, fore, back, RightToLeft == RightToLeft.Yes ? TextFormatFlags.Right : TextFormatFlags.Default);

            base.OnPaint(e);

            _drawing = false;
        }

        /// <inheritdoc />
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            
            if (!DefaultStyle)
            {
                SetStyle(ControlStyles.DoubleBuffer, true);
                SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
                SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                SetStyle(ControlStyles.ResizeRedraw, true);
                SetStyle(ControlStyles.Opaque, true);
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

            if (e.Index >= 0)
            {
                using Brush bBrush = ((e.State & DrawItemState.Selected) == DrawItemState.Selected) ? new SolidBrush(DropDownSelectedRowBackColor) : new SolidBrush(DropDownBackColor);

                e.Graphics.FillRectangle(bBrush, e.Bounds);
                TextRenderer.DrawText(e.Graphics, Items[e.Index].ToString(),e.Font,e.Bounds.Location, StyleNormal.ForeColor ?? e.ForeColor);
            }
            else
            {
                using var back = new SolidBrush(DropDownBackColor);
                e.Graphics.FillRectangle(back,e.Bounds);
            }
            
            e.DrawFocusRectangle();

            base.OnDrawItem(e);
        }

        /// <inheritdoc />
        protected override void OnMouseEnter(EventArgs eventargs)
        {
            if (DefaultStyle)
            {
                base.OnMouseEnter(eventargs);
                return;
            }

            if (!_hover)
            {
                _hover = true;
                Invalidate();
            }
        }

        /// <inheritdoc />
        protected override void OnMouseLeave(EventArgs eventargs)
        {
            if (DefaultStyle)
            {
                base.OnMouseLeave(eventargs);
                return;
            }

            if (_hover && !_selected)
            {
                _hover = false;
                Invalidate();
            }
        }

        /// <inheritdoc />
        protected override void OnGotFocus(EventArgs e)
        {
            if (DefaultStyle)
            {
                base.OnGotFocus(e);
                return;
            }

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
            if (DefaultStyle)
            {
                base.OnLostFocus(e);
                return;
            }

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
            if (DefaultStyle)
            {
                base.OnEnter(e);
                return;
            }

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
            if (DefaultStyle)
            {
                base.OnLeave(e);
                return;
            }

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

        /// <summary>Raises the <see cref="DropDownBackColorChanged" /> event.</summary>
        protected virtual void OnDropDownBackColorChanged()
        {
            DropDownBackColorChanged?.Invoke(this, EventArgs.Empty);
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

        /// <inheritdoc />
        public ExComboBoxStyle()
        {
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public override object Clone()
        {
            return new ExComboBoxStyle(this);
        }
    }
}
