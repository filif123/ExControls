namespace ExControls;

/// <summary>
///     Expanded Label Control.
/// </summary>
[ToolboxBitmap(typeof(Label), "Label.bmp")]
public class ExLabel : Label
{
    private Color _disabledForeColor;

    /// <summary>
    /// 
    /// </summary>
    public ExLabel()
    {
        _disabledForeColor = Color.DimGray;
    }

    /// <summary>
    ///     Color of the CheckBox's text and box when the Control is disabled
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "DimGray")]
    [ExDescription("Color of the CheckBox's text and box when the Control is disabled.")]
    public Color DisabledForeColor
    {
        get => _disabledForeColor;
        set
        {
            if (_disabledForeColor == value)
                return;
            _disabledForeColor = value;
            Invalidate();
            OnDisabledForeColorChanged();
        }
    }

    /// <summary>Occurs when the <see cref="DisabledForeColor" /> property changes.</summary>
    [ExCategory("Changed Property")]
    [ExDescription("Occurs when the DisabledForeColor property changes.")]
    public event EventHandler DisabledForeColorChanged;

    /// <summary>Raises the <see cref="DisabledForeColorChanged" /> event.</summary>
    protected virtual void OnDisabledForeColorChanged()
    {
        DisabledForeColorChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <inheritdoc />
    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(BackColor);
        TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, Enabled ? ForeColor : DisabledForeColor, BackColor);
    }
}