using ExControls.Designers;
// ReSharper disable UnusedMember.Global

namespace ExControls;

/// <summary>
/// ExColorSelector control.
/// </summary>
[Designer(typeof(ExColorSelectorDesigner))]
[DefaultProperty(nameof(SelectedColor))]
public partial class ExColorSelector : UserControl
{
    private Color _selectedColor;
    private Color _borderColor;
    private int _borderWidth;
    private BorderStyle _borderStyle;

    /// <summary>Occurs when the <see cref="SelectedColor" /> property changes.</summary>
    [ExCategory("Changed Property")]
    [ExDescription("Occurs when the SelectedColor property changes.")]
    public event EventHandler SelectedColorChanged;

    /// <summary>
    /// 
    /// </summary>
    public ExColorSelector()
    {
        InitializeComponent();
        _borderWidth = 1;
        _borderColor = Color.Black;
        _selectedColor = Color.Black;
        _borderStyle = BorderStyle.FixedSingle;
        Invalidate();
    }

    /// <summary>
    /// Gets or sets selected color.
    /// </summary>
    [ExDescription("Gets or sets selected color.", true)]
    [DefaultValue(typeof(Color), "Black")]
    public Color SelectedColor
    {
        get => _selectedColor;
        set
        {
            if (_selectedColor == value)
                return;
            _selectedColor = value;
            OnSelectedColorChanged();
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets color of border of this Control.
    /// </summary>
    [ExDescription("Gets or sets width of border of this Control.", true)]
    [DefaultValue(typeof(Color), "Black")]
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
    /// Gets or sets width of border of this Control.
    /// </summary>
    [ExDescription("Gets or sets width of border of this Control.", true)]
    [DefaultValue(1)]
    public int BorderWidth
    {
        get => _borderWidth;
        set
        {
            if (_borderWidth == value)
                return;
            _borderWidth = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets style of border of this Control.
    /// </summary>
    [ExDescription("Gets or sets style of border of this Control.", true)]
    [DefaultValue(BorderStyle.FixedSingle)]
    public new BorderStyle BorderStyle
    {
        get => _borderStyle;
        set
        {
            if (_borderStyle == value)
                return;
            _borderStyle = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets a dialog associated with this control.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [ExDescription("Gets a dialog associated with this control.", true)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public ColorDialog Dialog => colorDialog;

    private void ExColorSelector_Click(object sender, EventArgs e)
    {
        var result = colorDialog.ShowDialog();
        if (result == DialogResult.OK) 
            SelectedColor = colorDialog.Color;
    }

    private void ExColorSelector_Paint(object sender, PaintEventArgs e)
    {
        e.Graphics.Clear(SelectedColor);
        if (BorderStyle == BorderStyle.None)
            return;

        using var pen = new Pen(BorderColor, BorderWidth);
        var rec = e.ClipRectangle;
        if (BorderStyle == BorderStyle.Fixed3D) 
            e.Graphics.DrawRectangle(pen, rec);
        else
        {
            rec.Height -= BorderWidth;
            rec.Width -= BorderWidth;
            //BUG From BorderWidth = 3 it does not work
            rec.X = BorderWidth - 1;
            rec.Y = BorderWidth - 1;
            e.Graphics.DrawRectangle(pen, rec);
        }
    }

    /// <summary>Raises the <see cref="SelectedColorChanged" /> event.</summary>
    protected virtual void OnSelectedColorChanged()
    {
        SelectedColorChanged?.Invoke(this, EventArgs.Empty);
    }
}