using System.Runtime.InteropServices;
using ExControls.Controls;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global

namespace ExControls;

//------------------- WORK IN PROGRESS -------------------

/// <summary>
///     Expanded DateTimePicker Control. WORK IN PROGRESS
/// </summary>
[ToolboxBitmap(typeof(DateTimePicker), "DateTimePicker.bmp")]
public class ExDateTimePicker : DateTimePicker, IExControl
{
    private const int DTM_FIRST = 0x1000;
    private const int DTM_GETDATETIMEPICKERINFO = DTM_FIRST + 14;
    private Color _arrowColor;
    private Color _backColor;
    private Color _borderColor;
    private bool _defaultStyle;
    private Color _disabledBackColor;
    private Color _foreColor;
    private Color _highlightColor;

    private bool _hover;
    private bool _selected;

    //private Win32.DATETIMEPICKERINFO _dtpInfo;
    private DateTimePickerEdit _edit;

    /// <inheritdoc />
    public ExDateTimePicker()
    {
        _defaultStyle = true;
        _backColor = Color.White;
        _foreColor = Color.Black;
        _borderColor = Color.DimGray;
        _highlightColor = SystemColors.Highlight;
        _arrowColor = Color.Black;

        Invalidate();
    }

    /// <inheritdoc />
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "White")]
    public override Color BackColor
    {
        get => _backColor;
        set
        {
            if (_backColor == value)
                return;
            _backColor = value;
            Invalidate();
        }
    }

    /// <inheritdoc />
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "Black")]
    public override Color ForeColor
    {
        get => _foreColor;
        set
        {
            if (_foreColor == value)
                return;
            _foreColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Color of the border of ComboBox when mouse is over the Control.
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "Highlight")]
    [ExDescription("Color of the border of ComboBox when mouse is over the Control.")]
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
    ///     Color of the ComboBox's border.
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "DimGray")]
    [ExDescription("Color of the ComboBox's border.")]
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
    ///     Background color of the TextBox's when it is disabled.
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "Control")]
    [ExDescription("Background color of the TextBox's when it is disabled.")]
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
    ///     Color of the arrow which is in this Control in the Drop down button.
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "Black")]
    [ExDescription("Color of the arrow which is in this Control in the Drop down button.")]
    public Color ArrowColor
    {
        get => _arrowColor;
        set
        {
            if (_arrowColor == value)
                return;
            _arrowColor = value;
            Invalidate();
        }
    }

    /// <summary>Occurs when the <see cref="IExControl.DefaultStyle" /> property changes.</summary>
    [ExCategory("Changed Property")]
    [ExDescription("Occurs when the DefaultStyle property changes.")]
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

            this.SetTheme(value ? WindowsTheme.Default : WindowsTheme.None);
            Invalidate();
        }
    }

    /// <inheritdoc />
    protected override void OnCreateControl()
    {
        base.OnCreateControl();
        if (!DefaultStyle)
        {
            //SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            //SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.</summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
    protected override void OnHandleCreated(EventArgs e)
    {
        this.SetTheme(_defaultStyle ? WindowsTheme.Default : WindowsTheme.None);
        base.OnHandleCreated(e);
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleDestroyed" /> event.</summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
    protected override void OnHandleDestroyed(EventArgs e)
    {
        base.OnHandleDestroyed(e);
        _edit?.ReleaseHandle();
        _edit = null;
    }

    [DllImport("user32.dll", EntryPoint = "SendMessageW", CharSet = CharSet.Unicode)]
    private static extern IntPtr SendMessagePicker(IntPtr hWnd, int msg, IntPtr wp, out Win32.DATETIMEPICKERINFO lp);

    private Win32.DATETIMEPICKERINFO GetDatePickerInfo()
    {
        var info = new Win32.DATETIMEPICKERINFO();
        info.cbSize = Marshal.SizeOf(info);
        SendMessagePicker(Handle, DTM_GETDATETIMEPICKERINFO, IntPtr.Zero, out info);
        return info;
    }

    /// <summary>Processes Windows messages.</summary>
    /// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
    protected override void WndProc(ref Message m)
    {
        
        //if (m.GetMsg() == Win32.WM.NCPAINT && !DefaultStyle)
        {
            //using var g = Graphics.FromHwnd(Handle);
            //using var g = Graphics.FromHwnd(Handle);
            //.Clear(BackColor);
            //return;
        }

        

        base.WndProc(ref m);
        if (!DefaultStyle && m.Msg == (int)Win32.WM.NCPAINT)
        {
            using var args = new PaintEventArgs(Graphics.FromHwnd(Handle), ClientRectangle);
            OnPaint(args);
            //m.Result = IntPtr.Zero;
            //return;
        }
    }

    /// <inheritdoc />
    protected override void OnPaint(PaintEventArgs e)
    {
        //base.OnPaint(e);

        if (DefaultStyle)
            return;

        e.Graphics.Clear(BackColor);

        string sText;

        if (Format == DateTimePickerFormat.Custom) sText = $"{Value:CustomFormat}";
        else sText = Checked ? base.Text : "";

        //TextRenderer.DrawText(e.Graphics, sText, Font, new Point(0, 2), ForeColor);

        if (_hover)
        {
            using var p = new Pen(HighlightColor);
            e.Graphics.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
            e.Graphics.DrawLine(p, Width - 20, 0, Width - 20, Height);
        }
        else
        {
            using var p = new Pen(BorderColor);
            e.Graphics.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
        }

        var arrowX = ClientRectangle.Width - 14;
        var arrowY = ClientRectangle.Height / 2 - 1;

        using Brush brush = Enabled ? new SolidBrush(ArrowColor) : new SolidBrush(SystemColors.GrayText);
        Point[] arrows = { new(arrowX, arrowY), new(arrowX + 7, arrowY), new(arrowX + 3, arrowY + 4) };
        e.Graphics.FillPolygon(brush, arrows);

        var border = BorderColor;
        if (_hover || _selected) border = HighlightColor;

        ControlPaint.DrawBorder(e.Graphics, DisplayRectangle, border, ButtonBorderStyle.Solid);
    }

    /// <inheritdoc />
    protected override void OnValueChanged(EventArgs e)
    {
        base.OnValueChanged(e);
        Invalidate();
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

    private sealed class DateTimePickerEdit : NativeWindow
    {
        private ExDateTimePicker _picker;

        public DateTimePickerEdit(ExDateTimePicker picker)
        {
            _picker = picker;
        }

        /// <summary>Invokes the default window procedure associated with this window. </summary>
        /// <param name="m">A <see cref="T:System.Windows.Forms.Message" /> that is associated with the current Windows message. </param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            
            if (m.GetMsg() == Win32.WM.PAINT)
            {
                using Graphics g = Graphics.FromHwnd(m.HWnd);
                g.Clear(_picker.BackColor);
            }
        }
    }
}