using ExControls.Controls;
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace ExControls;

/// <summary>
///     Expanded Button Control.
/// </summary>
[ToolboxBitmap(typeof(Button), "Button.bmp")]
public class ExButton : Button, IExControl
{
    private bool _defaultStyle;
    private ExFlatButtonAppearance appearance;
    private Color tmpBeforeHoverColor = Color.Empty;
    private Color tmpBeforeClickColor = Color.Empty;
    private Color tmpBeforeFocusColor = Color.Empty;
    private bool focused;

    /// <summary>
    ///     Constructor
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
    /// 
    /// </summary>
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
    ///     Dont use.
    /// </summary>
    [Browsable(false)]
    [Obsolete]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("ButtonFlatAppearance")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public new FlatButtonAppearance FlatAppearance => base.FlatAppearance;

    /// <summary>
    /// 
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("ButtonFlatAppearance")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ExFlatButtonAppearance ExFlatAppearance
    {
        get
        {
            appearance ??= new ExFlatButtonAppearance(this);
            return appearance;
        }
    }

    /// <summary>Occurs when the <see cref="IExControl.DefaultStyle" /> property changes.</summary>
    [ExCategory("Changed Property")]
    [ExDescription("Occurs when the BorderColor property changes.")]
    public event EventHandler DefaultStyleChanged;

    /// <summary>Raises the <see cref="IExControl.DefaultStyleChanged" /> event.</summary>
    protected virtual void OnDefaultStyleChanged() => DefaultStyleChanged?.Invoke(this, EventArgs.Empty);

    /// <summary>Raises the <see cref="M:System.Windows.Forms.Control.OnMouseDown(System.Windows.Forms.MouseEventArgs)" /> event.</summary>
    /// <param name="mevent">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
    protected override void OnMouseDown(MouseEventArgs mevent)
    {
        base.OnMouseDown(mevent);
        if (_defaultStyle || appearance.MouseDownBorderColor == Color.Empty)
            return;
        tmpBeforeClickColor = appearance.BorderColor;
        appearance.SetTmpBeforeClickColor(tmpBeforeClickColor);
        appearance.BorderColor = appearance.MouseDownBorderColor;
    }

    /// <summary>Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnMouseUp(System.Windows.Forms.MouseEventArgs)" /> event.</summary>
    /// <param name="mevent">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
    protected override void OnMouseUp(MouseEventArgs mevent)
    {
        base.OnMouseUp(mevent);
        if (_defaultStyle || appearance.MouseDownBorderColor == Color.Empty)
            return;
        appearance.BorderColor = tmpBeforeClickColor;
        appearance.UnsetTmpBeforeClickColor();
        tmpBeforeClickColor = Color.Empty;
        if (focused)
            SetFocusColor();
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Enter" /> event.</summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        if (_defaultStyle || appearance.MouseOverBorderColor == Color.Empty)
            return;
        tmpBeforeHoverColor = appearance.BorderColor;
        appearance.SetTmpBeforeHoverColor(tmpBeforeHoverColor);
        appearance.BorderColor = appearance.MouseOverBorderColor;
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Leave" /> event.</summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        if (_defaultStyle || appearance.MouseOverBorderColor == Color.Empty)
            return;
        appearance.BorderColor = tmpBeforeHoverColor;
        appearance.UnsetTmpBeforeHoverColor();
        tmpBeforeHoverColor = Color.Empty;
        if (focused) 
            SetFocusColor();
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.GotFocus" /> event.</summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
    protected override void OnGotFocus(EventArgs e)
    {
        base.OnGotFocus(e);
        focused = true;
        if (_defaultStyle || appearance.FocusBorderColor == Color.Empty)
            return;
        SetFocusColor();
    }

    /// <summary>Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnLostFocus(System.EventArgs)" /> event.</summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
    protected override void OnLostFocus(EventArgs e)
    {
        base.OnLostFocus(e);
        focused = false;
        if (_defaultStyle || appearance.FocusBorderColor == Color.Empty)
            return;
        UnsetFocusColor();
    }

    private void SetFocusColor()
    {
        tmpBeforeFocusColor = appearance.BorderColor;
        appearance.SetTmpBeforeFocusColor(tmpBeforeFocusColor);
        appearance.BorderColor = appearance.FocusBorderColor;
    }

    private void UnsetFocusColor()
    {
        appearance.BorderColor = tmpBeforeFocusColor;
        appearance.UnsetTmpBeforeFocusColor();
        tmpBeforeFocusColor = Color.Empty;
    }
}


/// <summary>
///     ....
/// </summary>
[TypeConverter(typeof(ExpandableObjectConverter))]
public class ExFlatButtonAppearance
{
    private readonly ButtonBase owner;
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
    ///     Constructor
    /// </summary>
    /// <param name="owner"></param>
    public ExFlatButtonAppearance(ButtonBase owner) => this.owner = owner;

    /// <summary>
    /// ...
    /// </summary>
    [Browsable(true)]
    [NotifyParentProperty(true)]
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("ButtonBorderSizeDescr")]
    [EditorBrowsable(EditorBrowsableState.Always)]
    [DefaultValue(1)]
    public int BorderSize
    {
        get => owner.FlatAppearance.BorderSize;
        set => owner.FlatAppearance.BorderSize = value;
    }

    /// <summary>
    /// 
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

            return owner.FlatAppearance.BorderColor;
        }
        set => owner.FlatAppearance.BorderColor = value;
    }

    /// <summary>
    /// 
    /// </summary>
    [Browsable(true)]
    [NotifyParentProperty(true)]
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("ButtonCheckedBackColorDescr")]
    [EditorBrowsable(EditorBrowsableState.Always)]
    [DefaultValue(typeof(Color), "")]
    public Color CheckedBackColor
    {
        get => owner.FlatAppearance.CheckedBackColor;
        set => owner.FlatAppearance.CheckedBackColor = value;
    }

    /// <summary>
    /// 
    /// </summary>
    [Browsable(true)]
    [NotifyParentProperty(true)]
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("ButtonMouseDownBackColorDescr")]
    [EditorBrowsable(EditorBrowsableState.Always)]
    [DefaultValue(typeof(Color), "")]
    public Color MouseDownBackColor
    {
        get => owner.FlatAppearance.MouseDownBackColor;
        set => owner.FlatAppearance.MouseDownBackColor = value;
    }

    /// <summary>
    /// 
    /// </summary>
    [Browsable(true)]
    [NotifyParentProperty(true)]
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("ButtonMouseOverBackColorDescr")]
    [EditorBrowsable(EditorBrowsableState.Always)]
    [DefaultValue(typeof(Color), "")]
    public Color MouseOverBackColor
    {
        get => owner.FlatAppearance.MouseOverBackColor;
        set => owner.FlatAppearance.MouseOverBackColor = value;
    }

    /// <summary>
    /// 
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
            owner.Invalidate();
        }
    }

    /// <summary>
    /// 
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
            owner.Invalidate();
        }
    }

    /// <summary>
    /// 
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
            owner.Invalidate();
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
        _inClickMode = true;
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