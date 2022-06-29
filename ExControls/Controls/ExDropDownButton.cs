namespace ExControls;

/// <summary>
///     Extended button control with drop down menu.
/// </summary>
public class ExDropDownButton : ExButton
{
    private Color _arrowColor;
    private Color _disabledArrowColor;

    /// <inheritdoc />
    public ExDropDownButton()
    {
        _arrowColor = Color.Black;
        _disabledArrowColor = Color.Gray;
    }
    
    /// <summary>
    ///     Drop down menu.
    /// </summary>
    [DefaultValue(null)]
    public ContextMenuStrip DropDownMenu { get; set; }

    /// <summary>
    ///    Gets or sets a value indicating whether the drop down menu should be showing under cursor.
    /// </summary>
    [DefaultValue(false)]
    public bool ShowMenuUnderCursor { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("ArrowColorDescr")]
    [DefaultValue(typeof(Color), "Black")]
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

    /// <summary>
    /// 
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("DisabledArrowColorDescr")]
    [DefaultValue(typeof(Color), "Gray")]
    public Color DisabledArrowColor
    {
        get => _disabledArrowColor;
        set
        {
            if (_disabledArrowColor == value)
                return;
            _disabledArrowColor = value;
            Invalidate();
        }
    }

    /// <inheritdoc />
    protected override void OnMouseDown(MouseEventArgs mevent)
    {
        base.OnMouseDown(mevent);

        if (DropDownMenu != null && mevent.Button == MouseButtons.Left)
        {
            var menuLocation = ShowMenuUnderCursor ? mevent.Location : new Point(0, Height);
            DropDownMenu.Show(this, menuLocation);
        }
    }

    /// <inheritdoc />
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        if (DropDownMenu == null) 
            return;
        
        var arrowX = ClientRectangle.Width - 14;
        var arrowY = ClientRectangle.Height / 2 - 1;

        using var brush = Enabled ? new SolidBrush(ArrowColor) : new SolidBrush(DisabledArrowColor);
        var arrows = new Point[] { new(arrowX, arrowY), new(arrowX + 7, arrowY), new(arrowX + 3, arrowY + 4) };
        e.Graphics.FillPolygon(brush, arrows);
    }
}