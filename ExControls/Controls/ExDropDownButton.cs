namespace ExControls;

/// <summary>
/// Extended button control with drop down menu.
/// </summary>
public class ExDropDownButton : ExButton
{
    private Color _arrowColor;
    private Color _disabledArrowColor;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExDropDownButton" /> class without a menu.
    /// </summary>
    public ExDropDownButton()
    {
        _arrowColor = Color.Black;
        _disabledArrowColor = Color.Gray;
    }
    
    /// <summary>
    /// Gets or sets the menu shown when the button is clicked with the left mouse button - under the button, or under the cursor
    /// with <see cref="ShowMenuUnderCursor" />.
    /// </summary>
    [DefaultValue(null)]
    public ContextMenuStrip? DropDownMenu { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the drop down menu should be showing under cursor.
    /// </summary>
    [DefaultValue(false)]
    public bool ShowMenuUnderCursor { get; set; }

    /// <summary>
    /// Gets or sets the color of the drop-down arrow when the button is enabled.
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
    /// Gets or sets the color of the drop-down arrow when the button is disabled.
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
    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent);

        if (DropDownMenu == null) 
            return;
        
        var arrowX = ClientRectangle.Width - 14;
        var arrowY = ClientRectangle.Height / 2 - 1;

        using var brush = Enabled ? new SolidBrush(ArrowColor) : new SolidBrush(DisabledArrowColor);
        var arrows = new Point[] { new(arrowX, arrowY), new(arrowX + 7, arrowY), new(arrowX + 3, arrowY + 4) };
        pevent.Graphics.FillPolygon(brush, arrows);
    }
}