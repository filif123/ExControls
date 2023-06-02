namespace ExControls;

/// <inheritdoc />
public class ExListBox : ListBox
{
    private const int RGN_DIFF = 0x4;
    
    private bool _defaultStyle;
    private Color _selectedRowBackColor;
    private Color _borderColor;
    private Color _disabledBackColor;
    private Color _disabledForeColor;
    private Color _disabledBorderColor;
    private int _borderThickness;

    /// <inheritdoc />
    public ExListBox()
    {
        _defaultStyle = true;
        _borderColor = Color.DimGray;
        _borderThickness = 1;
        _disabledBorderColor = SystemColors.InactiveBorder;
        _disabledBackColor = SystemColors.Control;
        _disabledForeColor = SystemColors.GrayText;
        _selectedRowBackColor = SystemColors.Highlight;
        Invalidate();
    }

    /// <inheritdoc />
    protected override void OnCreateControl()
    {
        base.OnCreateControl();
        if (!DefaultStyle)
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Opaque, true);
        }
    }

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
            DrawMode = !_defaultStyle ? DrawMode.OwnerDrawFixed : DrawMode;
            Invalidate();
        }
    }

    /// <summary>
    ///     Color of the selected row.
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "Highlight")]
    [ExDescription("Color of the selected row.")]
    public Color SelectedRowBackColor
    {
        get => _selectedRowBackColor;
        set
        {
            if (_selectedRowBackColor == value)
                return;
            _selectedRowBackColor = value;
            Invalidate();
            OnSelectedRowBackColorChanged();
        }
    }

    /// <summary>
    ///     Color of the ListBox's border.
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "DimGray")]
    [ExDescription("Color of the ListBox's border.")]
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
    ///     Color of the ListBox's border when it is disabled.
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "InactiveBorder")]
    [ExDescription("Color of the ListBox's border when it is disabled.")]
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
    ///     Background color of the ListBox's when it is disabled.
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "Control")]
    [ExDescription("Background color of the ListBox's when it is disabled.")]
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
    ///     Foreground color of the ListBox's when it is disabled
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "GrayText")]
    [ExDescription("Foreground color of the ListBox's when it is disabled.")]
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
    ///     Width of the ListBox's border.
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(1)]
    [ExDescription("Width of the ListBox's border.")]
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

    /// <summary>Occurs when the <see cref="SelectedRowBackColor" /> property changes.</summary>
    [ExCategory("Changed Property")]
    [ExDescription("Occurs when the SelectedRowBackColor property changes.",true)]
    public event EventHandler SelectedRowBackColorChanged;

    /// <summary>Raises the <see cref="SelectedRowBackColorChanged" /> event.</summary>
    protected virtual void OnSelectedRowBackColorChanged()
    {
        SelectedRowBackColorChanged?.Invoke(this, EventArgs.Empty);
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
            using Brush bBrush = e.State.HasFlag(DrawItemState.Selected)
                ? new SolidBrush(SelectedRowBackColor)
                : new SolidBrush(BackColor);

            e.Graphics.FillRectangle(bBrush, e.Bounds);
            TextRenderer.DrawText(e.Graphics, GetItemText(Items[e.Index]), e.Font, e.Bounds.Location, e.ForeColor);
        }

        e.DrawFocusRectangle();

        base.OnDrawItem(e);
    }

    /// <inheritdoc />
    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);
        if (!DefaultStyle && m.GetMsg() == Win32.WM.NCPAINT)
        {
            DrawBorder(new PaintEventArgs(Graphics.FromHwnd(Handle), ClientRectangle));
        }
    }

    private void DrawBorder(PaintEventArgs e)
    {
        if (DefaultStyle)
            return;

        //border
        var hdc = Win32.GetWindowDC(Handle);
        var rgn = Win32.CreateRectRgn(0, 0, Width, Height);
        var border = !Enabled ? DisabledBorderColor : BorderColor;
        var brush = Win32.CreateSolidBrush(Win32.RGBtoInt(border));

        Win32.CombineRgn(rgn, rgn, Win32.CreateRectRgn(BorderThickness, BorderThickness, Width - BorderThickness, Height - BorderThickness), RGN_DIFF);

        Win32.FillRgn(hdc, rgn, brush);

        Win32.ReleaseDC(Handle, hdc);
        Win32.DeleteObject(rgn);
        Win32.DeleteObject(brush);
    }
}