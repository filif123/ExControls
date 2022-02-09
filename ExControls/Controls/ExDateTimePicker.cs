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
            base.BackColor = value;
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
            base.ForeColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Color of the border of ComboBox when mouse is over the Control
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
    ///     Color of the ComboBox's border
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
    ///     Background color of the TextBox's when it is disabled
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
    ///     Color of the arrow which is in this Control in the Drop down button
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

    //private Win32.DATETIMEPICKERINFO dtpInfo;
    //private DTPEdit Edit;

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

            //RecreateHandle();
            Invalidate();
        }
    }

    /// <inheritdoc />
    protected override void OnCreateControl()
    {
        base.OnCreateControl();
        if (!DefaultStyle)
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.</summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        if (!DefaultStyle)
        {

            //Edit = new DTPEdit();
            //Edit.AssignHandle(dtpInfo.hwndEdit);
        }
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseClick" /> event.</summary>
    /// <param name="e">An <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
    protected override void OnMouseClick(MouseEventArgs e)
    {
        base.OnMouseClick(e);

            
    }


    [DllImport("user32.dll", EntryPoint = "SendMessageW", CharSet = CharSet.Unicode)]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, out Win32.DATETIMEPICKERINFO lp);

    /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleDestroyed" /> event.</summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
    protected override void OnHandleDestroyed(EventArgs e)
    {
        base.OnHandleDestroyed(e);
        //Edit.DestroyHandle();
    }

    /// <summary>Processes Windows messages.</summary>
    /// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);
        if (DefaultStyle)
            return;

        /*if (m.Msg == Win32.WM_PAINT)
        {
            using Graphics g = Graphics.FromHwndInternal(Handle);
            var clientRect = new Rectangle(0, 0, Width, Height);
            var buttonWidth = dtpInfo.rcButton.Width;
            var dropDownRect = new Rectangle(dtpInfo.rcButton.Left, dtpInfo.rcButton.Top, buttonWidth, clientRect.Height);
            if (RightToLeft == RightToLeft.Yes && RightToLeftLayout)
            {
                dropDownRect.X = clientRect.Width - dropDownRect.Right;
                dropDownRect.Width += 1;
            }
            var middle = new Point(dropDownRect.Left + dropDownRect.Width / 2, dropDownRect.Top + dropDownRect.Height / 2);
            var arrow = new Point[]
            {
                new(middle.X - 3, middle.Y - 2),
                new(middle.X + 4, middle.Y - 2),
                new(middle.X, middle.Y + 2)
            };

            Color borderAndButtonColor = Enabled ? BorderColor : Color.LightGray;
            Color arrorColor = BackColor;
            using (var pen = new Pen(borderAndButtonColor))
                g.DrawRectangle(pen, 0, 0,
                    clientRect.Width - 1, clientRect.Height - 1);
            using (var brush = new SolidBrush(borderAndButtonColor))
                g.FillRectangle(brush, dropDownRect);
            g.FillPolygon(Brushes.Black, arrow);
        }*/
    }

    /// <inheritdoc />
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        if (DefaultStyle)
            return;

        e.Graphics.Clear(BackColor);

        string sText;

        if (Format == DateTimePickerFormat.Custom) sText = $"{Value:CustomFormat}";
        else sText = Checked ? base.Text : "";

        TextRenderer.DrawText(e.Graphics, sText, Font, new Point(0, 2), ForeColor);

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

    /*private class DTPEdit : NativeWindow
    {
        /// <summary>Invokes the default window procedure associated with this window. </summary>
        /// <param name="m">A <see cref="T:System.Windows.Forms.Message" /> that is associated with the current Windows message. </param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == Win32.)
            {
                
            }
        }
    }*/
}