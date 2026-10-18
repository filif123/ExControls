using ExControls.Controls;
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace ExControls;

/// <summary>
/// Extended Button control: in the custom style (<see cref="ExButton.DefaultStyle" /> = <see langword="false" />) it is flat with
/// border and background colors for the hover, pressed and focus states (<see cref="ExButton.ExFlatAppearance" />).
/// </summary>
[ToolboxBitmap(typeof(Button), "Button.bmp")]
public class ExButton : Button, IExControl, IThemeable
{
    /// <inheritdoc />
    public void ApplyTheme(ExTheme theme)
    {
        DefaultStyle = theme.UseSystemStyle;
        if (theme.UseSystemStyle)
            return;

        BackColor = theme.ButtonBackColor;
        ForeColor = theme.ButtonForeColor;
        ExFlatAppearance.BorderColor = theme.BorderColor;
        ExFlatAppearance.MouseOverBackColor = theme.HighlightBackColor;
        ExFlatAppearance.MouseDownBackColor = theme.HighlightBackColor;
        ExFlatAppearance.FocusBorderColor = theme.HighlightBackColor;
    }

    private bool _defaultStyle;
    private ExFlatButtonAppearance? _appearance;
    private Color _tmpBeforeHoverColor = Color.Empty;
    private Color _tmpBeforeClickColor = Color.Empty;
    private Color _tmpBeforeFocusColor = Color.Empty;
    private bool _focused;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExButton" /> class.
    /// </summary>
    public ExButton()
    {
        _defaultStyle = true;
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
            if (!_defaultStyle) 
                FlatStyle = FlatStyle.Flat;

            OnDefaultStyleChanged();
        }
    }

    /// <summary>
    /// Gets or sets the flat style appearance of the button. When <see cref="DefaultStyle" /> is <see langword="false" />,
    /// the style is always <see cref="System.Windows.Forms.FlatStyle.Flat" />.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public new FlatStyle FlatStyle
    {
        get => base.FlatStyle;
        set
        {
            if (!_defaultStyle && value != FlatStyle.Flat)
                value = FlatStyle.Flat;
            base.FlatStyle = value;
        }
    }

    /// <summary>
    /// Do not use: the appearance of the button is set by its style and <see cref="ExFlatAppearance" />.
    /// </summary>
    [Browsable(false)]
    [Obsolete("ExButton does not use FlatAppearance - the appearance is set by its style.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public new FlatButtonAppearance FlatAppearance => base.FlatAppearance;

    /// <summary>
    /// Gets the border and background colors of the button for the mouse and focus states,
    /// used when <see cref="DefaultStyle" /> is <see langword="false" />.
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("ButtonFlatAppearance")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ExFlatButtonAppearance ExFlatAppearance
    {
        get
        {
            _appearance ??= new ExFlatButtonAppearance(this);
            return _appearance;
        }
    }

    /// <summary>Occurs when the <see cref="IExControl.DefaultStyle" /> property changes.</summary>
    [ExCategory("Changed Property")]
    [ExDescription("Occurs when the BorderColor property changes.")]
    public event EventHandler? DefaultStyleChanged;

    /// <summary>Raises the <see cref="IExControl.DefaultStyleChanged" /> event.</summary>
    protected virtual void OnDefaultStyleChanged() => DefaultStyleChanged?.Invoke(this, EventArgs.Empty);

    /// <summary>Raises the <see cref="System.Windows.Forms.Control.OnMouseDown(System.Windows.Forms.MouseEventArgs)" /> event.</summary>
    /// <param name="mevent">A <see cref="System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
    protected override void OnMouseDown(MouseEventArgs mevent)
    {
        base.OnMouseDown(mevent);
        if (_defaultStyle || ExFlatAppearance.MouseDownBorderColor == Color.Empty)
            return;
        _tmpBeforeClickColor = ExFlatAppearance.BorderColor;
        ExFlatAppearance.SetTmpBeforeClickColor(_tmpBeforeClickColor);
        ExFlatAppearance.BorderColor = ExFlatAppearance.MouseDownBorderColor;
    }

    /// <summary>Raises the <see cref="System.Windows.Forms.ButtonBase.OnMouseUp(System.Windows.Forms.MouseEventArgs)" /> event.</summary>
    /// <param name="mevent">A <see cref="System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
    protected override void OnMouseUp(MouseEventArgs mevent)
    {
        base.OnMouseUp(mevent);
        if (_defaultStyle || ExFlatAppearance.MouseDownBorderColor == Color.Empty)
            return;
        ExFlatAppearance.BorderColor = _tmpBeforeClickColor;
        ExFlatAppearance.UnsetTmpBeforeClickColor();
        _tmpBeforeClickColor = Color.Empty;
        if (_focused)
            SetFocusColor();
    }

    /// <summary>Raises the <see cref="System.Windows.Forms.Control.MouseEnter" /> event.</summary>
    /// <param name="e">An <see cref="System.EventArgs" /> that contains the event data.</param>
    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        if (_defaultStyle || ExFlatAppearance.MouseOverBorderColor == Color.Empty)
            return;
        _tmpBeforeHoverColor = ExFlatAppearance.BorderColor;
        ExFlatAppearance.SetTmpBeforeHoverColor(_tmpBeforeHoverColor);
        ExFlatAppearance.BorderColor = ExFlatAppearance.MouseOverBorderColor;
    }

    /// <summary>Raises the <see cref="System.Windows.Forms.Control.MouseLeave" /> event.</summary>
    /// <param name="e">An <see cref="System.EventArgs" /> that contains the event data.</param>
    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        if (_defaultStyle || ExFlatAppearance.MouseOverBorderColor == Color.Empty)
            return;
        ExFlatAppearance.BorderColor = _tmpBeforeHoverColor;
        ExFlatAppearance.UnsetTmpBeforeHoverColor();
        _tmpBeforeHoverColor = Color.Empty;
        if (_focused) 
            SetFocusColor();
    }

    /// <summary>Raises the <see cref="System.Windows.Forms.Control.GotFocus" /> event.</summary>
    /// <param name="e">An <see cref="System.EventArgs" /> that contains the event data.</param>
    protected override void OnGotFocus(EventArgs e)
    {
        base.OnGotFocus(e);
        _focused = true;
        if (_defaultStyle || ExFlatAppearance.FocusBorderColor == Color.Empty)
            return;
        SetFocusColor();
    }

    /// <summary>Raises the <see cref="System.Windows.Forms.ButtonBase.OnLostFocus(System.EventArgs)" /> event.</summary>
    /// <param name="e">An <see cref="System.EventArgs" /> that contains the event data.</param>
    protected override void OnLostFocus(EventArgs e)
    {
        base.OnLostFocus(e);
        _focused = false;
        if (_defaultStyle || ExFlatAppearance.FocusBorderColor == Color.Empty)
            return;
        UnsetFocusColor();
    }

    private void SetFocusColor()
    {
        _tmpBeforeFocusColor = ExFlatAppearance.BorderColor;
        ExFlatAppearance.SetTmpBeforeFocusColor(_tmpBeforeFocusColor);
        ExFlatAppearance.BorderColor = ExFlatAppearance.FocusBorderColor;
    }

    private void UnsetFocusColor()
    {
        ExFlatAppearance.BorderColor = _tmpBeforeFocusColor;
        ExFlatAppearance.UnsetTmpBeforeFocusColor();
        _tmpBeforeFocusColor = Color.Empty;
    }
}


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