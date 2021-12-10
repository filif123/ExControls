using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using ExControls.Controls;

namespace ExControls;

/// <summary>
///     Expanded TextBox Control
/// </summary>
[ToolboxBitmap(typeof(TextBox), "TextBox.bmp")]
[Designer(typeof(ExTextBoxDesigner))]
public class ExTextBox : TextBox, IExControl
{
    private const int RGN_DIFF = 0x4;

    private Color _borderColor;
    private int _borderThickness;

    private bool _defaultStyle;
    private Color _disabledBackColor;
    private Color _disabledBorderColor;
    private Color _disabledForeColor;
    private Color _highlightColor;
    private Color _hintForeColor;
    private string _hintText;
    private bool _hover;
    private bool _selected;

    /// <summary>
    ///     Constructor
    /// </summary>
    public ExTextBox()
    {
        _defaultStyle = true;
        _borderColor = Color.DimGray;
        _highlightColor = SystemColors.Highlight;
        _borderThickness = 1;
        _disabledBorderColor = SystemColors.InactiveBorder;
        _disabledBackColor = SystemColors.Control;
        _disabledForeColor = SystemColors.GrayText;
        _hintText = null;
        _hintForeColor = SystemColors.GrayText;
        Invalidate();
    }

    /// <summary>
    ///     Color of the TextBox's border
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "Black")]
    [Description("Color of the TextBox's border.")]
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
    ///     Color of the TextBox's border when it is disabled
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "InactiveBorder")]
    [Description("Color of the TextBox's border when it is disabled.")]
    public Color DisabledBorderColor
    {
        get => _disabledBorderColor;
        set
        {
            if (_disabledBorderColor == value)
                return;
            _disabledBorderColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Background color of the TextBox's when it is disabled
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "Control")]
    [Description("Background color of the TextBox's when it is disabled.")]
    public Color DisabledBackColor
    {
        get => _disabledBackColor;
        set
        {
            if (_disabledBackColor == value)
                return;
            _disabledBackColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Foreground color of the TextBox's when it is disabled
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "GrayText")]
    [Description("Foreground color of the TextBox's when it is disabled.")]
    public Color DisabledForeColor
    {
        get => _disabledForeColor;
        set
        {
            if (_disabledForeColor == value)
                return;
            _disabledForeColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Color of the border of TextBox when mouse is over the Control
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "Highlight")]
    [Description("Color of the border of TextBox when mouse is over the Control.")]
    public Color HighlightColor
    {
        get => _highlightColor;
        set
        {
            if (_highlightColor == value)
                return;
            _highlightColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Width of the TextBox's border
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(1)]
    [Description("Width of the TextBox's border.")]
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
    ///     Hint text for TextBox
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [Description("Hint text for TextBox.")]
    public string HintText
    {
        get => _hintText;
        set
        {
            if (_hintText == value)
                return;
            _hintText = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     TextBox hint foreground color
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [Description("TextBox hint foreground color.")]
    [DefaultValue(typeof(SystemColors), "GrayText")]
    public Color HintForeColor
    {
        get => _hintForeColor;
        set
        {
            if (_hintForeColor == value)
                return;
            _hintForeColor = value;
            Invalidate();
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
            BorderStyle = value ? BorderStyle.Fixed3D : BorderStyle.FixedSingle;
            Invalidate();
        }
    }

    /// <inheritdoc />
    protected override void OnEnabledChanged(EventArgs e)
    {
        Invalidate();
        base.OnEnabledChanged(e);
    }

    /// <inheritdoc />
    protected override void OnCreateControl()
    {
        base.OnCreateControl();

        if (!DefaultStyle)
        {
            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
        }
    }

    /// <inheritdoc />
    protected override void WndProc(ref Message m)
    {
        if (!DefaultStyle && m.Msg == (int)Win32.WM.NCPAINT)
            return;

        base.WndProc(ref m);

        if (!DefaultStyle && m.Msg == (int)Win32.WM.PAINT)
        {
            using var args = new PaintEventArgs(Graphics.FromHwnd(Handle), ClientRectangle);
            OnPaint(args);
            if (!DefaultStyle)
                m.Result = IntPtr.Zero;
            return;
        }

        if (m.Msg == (int)Win32.WM.PAINT)
            DrawHint(Graphics.FromHwnd(m.HWnd));
    }

    /// <inheritdoc />
    protected override void OnPaint(PaintEventArgs e)
    {
        DrawHint(e.Graphics);

        if (!DefaultStyle)
        {
            if (!Enabled)
            {
                e.Graphics.FillRectangle(new SolidBrush(DisabledBackColor), ClientRectangle);
                TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, DisabledForeColor, DisabledBackColor, ConvertAligment());
            }

            //border
            var hdc = Win32.GetWindowDC(Handle);
            var rgn = Win32.CreateRectRgn(0, 0, Width, Height);
            var border = _hover || _selected ? HighlightColor : BorderColor;
            if (!Enabled) border = DisabledBorderColor;
            var brush = Win32.CreateSolidBrush((uint)BGRtoInt(border.R, border.G, border.B));

            Win32.CombineRgn(rgn, rgn, Win32.CreateRectRgn(BorderThickness, BorderThickness, Width - BorderThickness, Height - BorderThickness),
                RGN_DIFF);

            Win32.FillRgn(hdc, rgn, brush);

            Win32.ReleaseDC(Handle, hdc);
            Win32.DeleteObject(rgn);
            Win32.DeleteObject(brush);
        }
    }

    /// <summary>
    ///     Draws a hint text on TextBox
    /// </summary>
    /// <param name="g"></param>
    protected virtual void DrawHint(Graphics g)
    {
        if (!Focused && string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(HintText))
            TextRenderer.DrawText(g, HintText, Font, ClientRectangle, HintForeColor, BackColor, ConvertAligment());
    }

    internal TextFormatFlags ConvertAligment()
    {
        var a = TextAlign;
        return a switch
        {
            HorizontalAlignment.Center => TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter,
            HorizontalAlignment.Right => TextFormatFlags.VerticalCenter | TextFormatFlags.Right,
            _ => TextFormatFlags.VerticalCenter | TextFormatFlags.Left
        };
    }

    private static int BGRtoInt(int r, int g, int b)
    {
        return (r << 0) | (g << 8) | (b << 16);
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

    /// <summary>Raises the <see cref="IExControl.DefaultStyleChanged" /> event.</summary>
    protected virtual void OnDefaultStyleChanged()
    {
        DefaultStyleChanged?.Invoke(this, EventArgs.Empty);
    }
}

internal class ExTextBoxDesigner : ExControlDesigner
{
    private DesignerActionListCollection actionList;

    public override DesignerActionListCollection ActionLists
    {
        get { return actionList ??= new DesignerActionListCollection(new DesignerActionList[] { new ExTextBoxDesignerActionList(this) }); }
    }

    /// <summary>Gets the selection rules that indicate the movement capabilities of a component.</summary>
    /// <returns>A bitwise combination of <see cref="T:System.Windows.Forms.Design.SelectionRules" /> values.</returns>
    public override SelectionRules SelectionRules
    {
        get
        {
            if (Control is not ExTextBox control) throw new InvalidOperationException();

            if (control.Multiline) return SelectionRules.AllSizeable | SelectionRules.Moveable; // | base.SelectionRules;

            return /*base.SelectionRules |*/ SelectionRules.LeftSizeable | SelectionRules.RightSizeable | SelectionRules.Moveable;
        }
    }
}

internal class ExTextBoxDesignerActionList : ExControlDesignerActionList
{
    public ExTextBoxDesignerActionList(ControlDesigner designer) : base(designer)
    {
        Control = (ExTextBox)designer.Control;
    }

    protected new ExTextBox Control { get; }

    public bool Enabled
    {
        get => Control.Enabled;
        set => TypeDescriptor.GetProperties(Component)["Enabled"].SetValue(Component, value);
    }

    public bool ReadOnly
    {
        get => Control.ReadOnly;
        set => TypeDescriptor.GetProperties(Component)["ReadOnly"].SetValue(Component, value);
    }

    public bool Multiline
    {
        get => Control.Multiline;
        set => TypeDescriptor.GetProperties(Component)["Multiline"].SetValue(Component, value);
    }

    public char PasswordChar
    {
        get => Control.PasswordChar;
        set => TypeDescriptor.GetProperties(Component)["PasswordChar"].SetValue(Component, value);
    }

    /*public void EditItems()
    {
        var editorServiceContext = typeof(ControlDesigner).Assembly.GetTypes()
            .Where(x => x.Name == "EditorServiceContext").FirstOrDefault();
        var editValue = editorServiceContext.GetMethod("EditValue",
            System.Reflection.BindingFlags.Static |
            System.Reflection.BindingFlags.Public);
        editValue.Invoke(null, new object[] { designer, Component, "Items" });
    }*/

    public override DesignerActionItemCollection GetSortedActionItems()
    {
        var items = base.GetSortedActionItems();
        //new DesignerActionMethodItem(this, "EditItems", "Edit Items",  true),
        items.Add(new DesignerActionPropertyItem("PasswordChar", "PasswordChar", "Behavior"));
        items.Add(new DesignerActionPropertyItem("Enabled", "Enabled", "Appearance"));
        items.Add(new DesignerActionPropertyItem("ReadOnly", "ReadOnly", "Appearance"));
        items.Add(new DesignerActionPropertyItem("Multiline", "Multiline", "Appearance"));
        return items;
    }
}