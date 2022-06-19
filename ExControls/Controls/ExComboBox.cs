using System.Drawing.Design;
using System.Reflection;
using System.Runtime.InteropServices;
using ExControls.Controls;
// ReSharper disable UnusedMember.Global
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable VirtualMemberNeverOverridden.Global

namespace ExControls;

/// <summary>
///     Expanded ComboBox Control
/// </summary>
[ToolboxBitmap(typeof(ComboBox), "ComboBox.bmp")]
public class ExComboBox : ComboBox, IExControl
{
    private bool _defaultStyle;
    private bool _drawing;
    private Color _dropDownBackColor;
    private Color _dropDownSelectedRowBackColor;

    private bool _hover;
    private bool _selected;
    private bool _wasDropDown;

    private SolidBrush listbrush;
    private ComboBoxEdit _editControl;

    /// <summary>
    ///     Constructor
    /// </summary>
    public ExComboBox()
    {
        SuspendLayout();
        _defaultStyle = true;

        StyleNormal = new ExComboBoxStyle(StyleType.Normal);
        StyleHighlight = new ExComboBoxStyle(StyleType.Hover);
        StyleSelected = new ExComboBoxStyle(StyleType.Selected);
        StyleDisabled = new ExComboBoxStyle(StyleType.Disabled);

        StyleNormal.PropertyChanged += StyleOnPropertyChanged;
        StyleDisabled.PropertyChanged += StyleOnPropertyChanged;
        StyleHighlight.PropertyChanged += StyleOnPropertyChanged;
        StyleSelected.PropertyChanged += StyleOnPropertyChanged;

        _dropDownSelectedRowBackColor = SystemColors.Highlight;
        _dropDownBackColor = Color.White;

        base.DoubleBuffered = true;
        listbrush = new SolidBrush(DropDownBackColor);
        ResumeLayout();
        Invalidate();
    }

    /// <summary>
    ///     Don't use directly in the code.
    /// </summary>
    [Browsable(false)]
    public Color ActualBackColor { get; private set; }

    /// <summary>
    ///     Don't use directly in code.
    /// </summary>
    [Browsable(false)]
    public Color ActualForeColor { get; private set; }

    /// <summary>
    ///     Normal style of the Control (when is inactive).
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("Normal style of the Control (when is inactive).")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [DisplayName("StyleNormal")]
    public ExComboBoxStyle StyleNormal { get; set; }

    /// <summary>
    ///     Highlight style of the Control (when mouse is over control).
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("Highlight style of the Control (when mouse is over control).")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [DisplayName("StyleHighlight")]
    public ExComboBoxStyle StyleHighlight { get; set; }

    /// <summary>
    ///     Selected style of the Control (when control is selected).
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("Selected style of the Control (when control is selected).")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [DisplayName("StyleSelected")]
    public ExComboBoxStyle StyleSelected { get; set; }

    /// <summary>
    ///     Disabled style of the Control (when control is not Enabled).
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("Disabled style of the Control (when control is not Enabled).")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [DisplayName("StyleDisabled")]
    public ExComboBoxStyle StyleDisabled { get; set; }

    /// <summary>
    ///     Color of the selected row in drop down menu
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "Highlight")]
    [ExDescription("Color of the selected row in drop down menu.")]
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
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "White")]
    [ExDescription("Background color of the drop down menu.")]
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
            if (value != DrawMode.Normal) _defaultStyle = false;

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

    /// <summary>
    /// </summary>
    public new bool Enabled
    {
        get => base.Enabled;
        set
        {
            switch (value)
            {
                case false when !DefaultStyle && DropDownStyle == ComboBoxStyle.DropDown:
                    _wasDropDown = true;
                    DropDownStyle = ComboBoxStyle.DropDownList;
                    break;
                case true when !DefaultStyle && _wasDropDown:
                    _wasDropDown = false;
                    DropDownStyle = ComboBoxStyle.DropDown;
                    break;
            }

            base.Enabled = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public bool UseDarkScrollBar { get; set; }

    /// <summary>Occurs when the <see cref="IExControl.DefaultStyle" /> property changes.</summary>
    [ExCategory("Changed Property")]
    [ExDescription("Occurs when the BorderColor property changes.")]
    public event EventHandler DefaultStyleChanged;


    /// <inheritdoc />
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(true)]
    [ExDescription("Default style of the Control.")]
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

    /// <summary>Occurs when the <see cref="DropDownSelectedRowBackColor" /> property changes.</summary>
    [ExCategory("Changed Property")]
    [ExDescription("Occurs when the DropDownSelectedRowBackColor property changes.")]
    public event EventHandler DropDownSelectedRowBackColorChanged;

    /// <summary>Occurs when the <see cref="DropDownBackColor" /> property changes.</summary>
    [ExCategory("Changed Property")]
    [ExDescription("Occurs when the DropDownBackColor property changes.")]
    public event EventHandler DropDownBackColorChanged;

    private void StyleOnPropertyChanged(object sender, ExPropertyChangedEventArgs e)
    {
        if (!_drawing) Invalidate();
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.</summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
    protected override void OnHandleCreated(EventArgs e)
    {
        if (!DefaultStyle) Win32.SetWindowTheme(Handle, "", "");
        var info = GetComboboxinfo();
        _editControl = new ComboBoxEdit(this);
        _editControl.AssignHandle(info.hwndEdit);
        base.OnHandleCreated(e);
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleDestroyed" /> event.</summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
    protected override void OnHandleDestroyed(EventArgs e)
    {
        listbrush.Dispose();
        _editControl.ReleaseHandle();
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
            switch ((Win32.WM)m.Msg)
            {
                case Win32.WM.PAINT:
                {
                    using var g = Graphics.FromHwnd(m.HWnd);
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

    private Win32.COMBOBOXINFO GetComboboxinfo()
    {
        var info = new Win32.COMBOBOXINFO();
        info.cbSize = Marshal.SizeOf(info);
        SendMessageCb(Handle, 0x164, IntPtr.Zero, out info);
        return info;
    }

    /// <inheritdoc />
    protected override void OnDropDown(EventArgs e)
    {
        // Install wrapper
        base.OnDropDown(e);

        if (!UseDarkScrollBar)
            return;

        // Retrieve handle to dropdown list
        var info = GetComboboxinfo();
        ExTools.SetTheme(info.hwndList,WindowsTheme.DarkExplorer);
    }

    [DllImport("user32.dll", EntryPoint = "SendMessageW", CharSet = CharSet.Unicode)]
    private static extern IntPtr SendMessageCb(IntPtr hWnd, int msg, IntPtr wp, out Win32.COMBOBOXINFO lp);

    private static IntPtr GetHbrush(Brush b)
    {
        var field = typeof(Brush).GetField("nativeBrush", BindingFlags.NonPublic | BindingFlags.Instance);
        if (field is not null)
            return (IntPtr)field.GetValue(b);
        return IntPtr.Zero;
    }

    /// <inheritdoc />
    protected override void OnPaint(PaintEventArgs e)
    {
        if (DefaultStyle)
        {
            base.OnPaint(e);
            return;
        }

        _drawing = true;

        var g = e.Graphics;

        var size = TextRenderer.MeasureText(g, Text, Font);
        var textStart = new Point(2, (int)Math.Round(ClientRectangle.Height / 2d - size.Height / 2d));
        var dropButton = new Rectangle(Width - 20, 0, 20, Height);

        var back = StyleNormal.BackColor ??= Color.White;
        var fore = StyleNormal.ForeColor ??= Color.Black;
        var border = StyleNormal.BorderColor ??= Color.DimGray;
        var arrow = StyleNormal.ArrowColor ??= Color.Black;
        var backbut = StyleNormal.ButtonBackColor ??= back;
        var borbut = StyleNormal.ButtonBorderColor ??= back;
        var bbfirst = StyleNormal.ButtonRenderFirst ??= true;

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
            ExButtonRenderer.DrawDropDownButton(g, dropButton, backbut, borbut, arrow);
            g.DrawRectangle(penBorder, 0, 0, Width - 1, Height - 1);
        }
        else
        {
            g.DrawRectangle(penBorder, 0, 0, Width - 1, Height - 1);
            ExButtonRenderer.DrawDropDownButton(g, dropButton, backbut, borbut, arrow);
        }

        if (DropDownStyle == ComboBoxStyle.DropDownList)
            TextRenderer.DrawText(g, Text, Font, textStart, fore, back,
                RightToLeft == RightToLeft.Yes ? TextFormatFlags.Right : TextFormatFlags.Default);

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
            using Brush bBrush = (e.State & DrawItemState.Selected) == DrawItemState.Selected
                ? new SolidBrush(DropDownSelectedRowBackColor)
                : new SolidBrush(DropDownBackColor);

            e.Graphics.FillRectangle(bBrush, e.Bounds);
            TextRenderer.DrawText(e.Graphics, Items[e.Index].ToString(), e.Font, e.Bounds.Location, StyleNormal.ForeColor ?? e.ForeColor);
        }
        else
        {
            using var back = new SolidBrush(DropDownBackColor);
            e.Graphics.FillRectangle(back, e.Bounds);
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

    private class ComboBoxEdit : NativeWindow
    {
        private readonly ExComboBox parent;

        public ComboBoxEdit(ExComboBox parent)
        {
            this.parent = parent;
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (parent.DefaultStyle || m.Msg != (int) Win32.WM.PAINT) 
                return;
            if (parent.Enabled || parent.DropDownStyle != ComboBoxStyle.DropDown) 
                return;

            var g = Graphics.FromHwnd(Handle);
            var backColor = parent.StyleDisabled.BackColor ?? SystemColors.Control;
            var foreColor = parent.StyleDisabled.ForeColor ?? SystemColors.GrayText;
            Win32.GetWindowRect(new HandleRef(this, Handle), out var rec);
            var recc = new Rectangle(0, 0, rec.Width, rec.Height);
            g.Clear(backColor);
            TextRenderer.DrawText(g, parent.Text, parent.Font, recc, foreColor, backColor, ExTextBox.ConvertAligment(HorizontalAlignment.Left));
            m.Result = IntPtr.Zero;
        }
    }
}

/// <summary>
///     Class for definition styles for ExComboBox
/// </summary>
public class ExComboBoxStyle : ExStyleOld
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
    public ExComboBoxStyle(StyleType type) : base(type)
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
    [ExCategory(CategoryType.Appearance)]
    [NotifyParentProperty(true)]
    [DefaultValue(typeof(Color), "Black")]
    [ExDescription("Color of the arrow which is in this Control as the dropdown button.")]
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
    [ExCategory(CategoryType.Appearance)]
    [NotifyParentProperty(true)]
    [DefaultValue(typeof(Color), "White")]
    [ExDescription("Background color of the dropdown button.")]
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
    [ExCategory(CategoryType.Appearance)]
    [NotifyParentProperty(true)]
    [DefaultValue(typeof(Color), "DimGray")]
    [ExDescription("Border color of the dropdown button.")]
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
    [ExCategory(CategoryType.Appearance)]
    [NotifyParentProperty(true)]
    [ExDescription("Gets or sets whether DropDown button has to be rendered first.")]
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
    public override object Clone() => new ExComboBoxStyle(this);
}