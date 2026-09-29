using ExControls.Controls;
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace ExControls;

/// <summary>
/// Expanded Button Control.
/// </summary>
[ToolboxBitmap(typeof(Button), "Button.bmp")]
public class ExButton : Button, IExControl, IThemeable
{
    /// <summary>
    /// Nastavi farby a vzhlad prvku podla temy.
    /// </summary>
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
    /// Constructor
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
    /// Dont use.
    /// </summary>
    [Browsable(false)]
    [Obsolete("ExButton does not use FlatAppearance - the appearance is set by its style.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
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

    /// <summary>Raises the <see cref="System.Windows.Forms.Control.Enter" /> event.</summary>
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

    /// <summary>Raises the <see cref="System.Windows.Forms.Control.Leave" /> event.</summary>
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
/// ....
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
    /// Constructor
    /// </summary>
    /// <param name="owner"></param>
    public ExFlatButtonAppearance(ButtonBase owner) => this._owner = owner;

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
        get => _owner.FlatAppearance.BorderSize;
        set => _owner.FlatAppearance.BorderSize = value;
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

            return _owner.FlatAppearance.BorderColor;
        }
        set => _owner.FlatAppearance.BorderColor = value;
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
        get => _owner.FlatAppearance.CheckedBackColor;
        set => _owner.FlatAppearance.CheckedBackColor = value;
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
        get => _owner.FlatAppearance.MouseDownBackColor;
        set => _owner.FlatAppearance.MouseDownBackColor = value;
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
        get => _owner.FlatAppearance.MouseOverBackColor;
        set => _owner.FlatAppearance.MouseOverBackColor = value;
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
            _owner.Invalidate();
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
            _owner.Invalidate();
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