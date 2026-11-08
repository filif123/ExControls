// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace ExControls;

/// <summary>
/// Border and background colors of an <see cref="ExButton" /> in the flat style, including the colors for the mouse and focus states.
/// </summary>
[TypeConverter(typeof(ExpandableObjectConverter))]
public class ExFlatButtonAppearance
{
    private readonly ButtonBase _owner;
    private Color _mouseDownBorderColor = Color.Empty;
    private Color _mouseOverBorderColor = Color.Empty;
    private Color _focusBorderColor = Color.Empty;

    private Color _tmpBeforeHoverColor = Color.Empty;
    private Color _tmpBeforeClickColor = Color.Empty;
    private Color _tmpBeforeFocusColor = Color.Empty;
    private bool _inHoverMode;
    private bool _inClickMode;
    private bool _inFocusMode;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExFlatButtonAppearance" /> class.
    /// </summary>
    /// <param name="owner">The button whose appearance is configured.</param>
    public ExFlatButtonAppearance(ButtonBase owner) => this._owner = owner;

    /// <summary>
    /// Gets or sets the width of the button border in pixels.
    /// </summary>
    [Browsable(true)]
    [NotifyParentProperty(true)]
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("ButtonBorderSizeDescr")]
    [EditorBrowsable(EditorBrowsableState.Always)]
    [DefaultValue(1)]
    public int BorderSize
    {
        get => _owner.FlatAppearance.BorderSize;
        set => _owner.FlatAppearance.BorderSize = value;
    }

    /// <summary>
    /// Gets or sets the border color of the button.
    /// While the hover, pressed or focus border color is shown, the original border color is returned.
    /// </summary>
    [Browsable(true)]
    [NotifyParentProperty(true)]
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("ButtonBorderColorDescr")]
    [EditorBrowsable(EditorBrowsableState.Always)]
    [DefaultValue(typeof(Color), "")]
    public Color BorderColor
    {
        get
        {
            if (_inFocusMode)
                return _tmpBeforeFocusColor;
            if (_inHoverMode)
                return _tmpBeforeHoverColor;
            if (_inClickMode)
                return _tmpBeforeClickColor;

            return _owner.FlatAppearance.BorderColor;
        }
        set => _owner.FlatAppearance.BorderColor = value;
    }

    /// <summary>
    /// Gets or sets the background color of the button when it is checked.
    /// </summary>
    [Browsable(true)]
    [NotifyParentProperty(true)]
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("ButtonCheckedBackColorDescr")]
    [EditorBrowsable(EditorBrowsableState.Always)]
    [DefaultValue(typeof(Color), "")]
    public Color CheckedBackColor
    {
        get => _owner.FlatAppearance.CheckedBackColor;
        set => _owner.FlatAppearance.CheckedBackColor = value;
    }

    /// <summary>
    /// Gets or sets the background color of the button while the mouse button is pressed over it.
    /// </summary>
    [Browsable(true)]
    [NotifyParentProperty(true)]
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("ButtonMouseDownBackColorDescr")]
    [EditorBrowsable(EditorBrowsableState.Always)]
    [DefaultValue(typeof(Color), "")]
    public Color MouseDownBackColor
    {
        get => _owner.FlatAppearance.MouseDownBackColor;
        set => _owner.FlatAppearance.MouseDownBackColor = value;
    }

    /// <summary>
    /// Gets or sets the background color of the button when the mouse pointer is over it.
    /// </summary>
    [Browsable(true)]
    [NotifyParentProperty(true)]
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("ButtonMouseOverBackColorDescr")]
    [EditorBrowsable(EditorBrowsableState.Always)]
    [DefaultValue(typeof(Color), "")]
    public Color MouseOverBackColor
    {
        get => _owner.FlatAppearance.MouseOverBackColor;
        set => _owner.FlatAppearance.MouseOverBackColor = value;
    }

    /// <summary>
    /// Gets or sets the border color of the button while the mouse button is pressed over it.
    /// <see cref="Color.Empty" /> keeps the border color unchanged.
    /// </summary>
    [Browsable(true)]
    [NotifyParentProperty(true)]
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("ButtonMouseDownBackColorDescr")]
    [EditorBrowsable(EditorBrowsableState.Always)]
    [DefaultValue(typeof(Color), "")]
    public Color MouseDownBorderColor
    {
        get => _mouseDownBorderColor;
        set
        {
            if (_mouseDownBorderColor == value)
                return;
            _mouseDownBorderColor = value;
            _owner.Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the border color of the button when the mouse pointer is over it.
    /// <see cref="Color.Empty" /> keeps the border color unchanged.
    /// </summary>
    [Browsable(true)]
    [NotifyParentProperty(true)]
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("ButtonMouseOverBackColorDescr")]
    [EditorBrowsable(EditorBrowsableState.Always)]
    [DefaultValue(typeof(Color), "")]
    public Color MouseOverBorderColor
    {
        get => _mouseOverBorderColor;
        set
        {
            if (_mouseOverBorderColor == value)
                return;
            _mouseOverBorderColor = value;
            _owner.Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the border color of the button when it has focus.
    /// <see cref="Color.Empty" /> keeps the border color unchanged.
    /// </summary>
    [Browsable(true)]
    [NotifyParentProperty(true)]
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("ButtonMouseOverBackColorDescr")]
    [EditorBrowsable(EditorBrowsableState.Always)]
    [DefaultValue(typeof(Color), "")]
    public Color FocusBorderColor
    {
        get => _focusBorderColor;
        set
        {
            if (_focusBorderColor == value)
                return;
            _focusBorderColor = value;
            _owner.Invalidate();
        }
    }

    internal void SetTmpBeforeHoverColor(Color tmpBeforeHoverColor)
    {
        _tmpBeforeHoverColor = tmpBeforeHoverColor;
        _inHoverMode = true;
    }

    internal void UnsetTmpBeforeHoverColor()
    {
        _tmpBeforeHoverColor = Color.Empty;
        _inHoverMode = false;
    }

    internal void SetTmpBeforeClickColor(Color tmpBeforeClickColor)
    {
        _tmpBeforeClickColor = tmpBeforeClickColor;
        _inClickMode = true;
    }

    internal void UnsetTmpBeforeClickColor()
    {
        _tmpBeforeClickColor = Color.Empty;
        _inClickMode = false;
    }

    internal void SetTmpBeforeFocusColor(Color tmpBeforeFocusColor)
    {
        _tmpBeforeFocusColor = tmpBeforeFocusColor;
        _inFocusMode = true;
    }

    internal void UnsetTmpBeforeFocusColor()
    {
        _tmpBeforeFocusColor = Color.Empty;
        _inFocusMode = false;
    }

    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => "";
}
