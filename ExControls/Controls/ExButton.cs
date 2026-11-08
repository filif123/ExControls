using ExControls.Controls;
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace ExControls;

/// <summary>
/// Extended Button control: in the custom style (<see cref="ExButton.DefaultStyle" /> = <see langword="false" />) it is flat with
/// border and background colors for the hover, pressed and focus states (<see cref="ExButton.ExFlatAppearance" />).
/// </summary>
#if NETFRAMEWORK
[ToolboxBitmap(typeof(Button), "Button.bmp")]
#else
[ToolboxBitmap(typeof(ToolboxIcons), "Toolbox.ExButton.ico")]
#endif
public class ExButton : Button, IExControl, IThemeable
{
    /// <inheritdoc />
    public virtual void ApplyTheme(ExTheme theme)
    {
        ExThemer.KeepDefaults(this, theme, nameof(BackColor), nameof(ForeColor), nameof(UseVisualStyleBackColor));
        ExThemer.KeepDefaults(ExFlatAppearance, theme, nameof(ExFlatAppearance.BorderColor), nameof(ExFlatAppearance.MouseOverBackColor),
            nameof(ExFlatAppearance.MouseDownBackColor), nameof(ExFlatAppearance.FocusBorderColor));
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

    // sila textu neaktivneho tlacidla - rovnaka ako pri sipke ExDropDownButton
    private const int DisabledForeAlpha = 110;

    private bool _defaultStyle;
    // FlatStyle z navrhara alebo kodu - plati, ked ma tlacidlo predvoleny vzhlad
    private FlatStyle _flatStyle = FlatStyle.Standard;
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
            base.FlatStyle = _defaultStyle ? _flatStyle : FlatStyle.Flat;

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
            _flatStyle = value;
            base.FlatStyle = _defaultStyle ? value : FlatStyle.Flat;
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

    /// <summary>
    /// Text color of a disabled button in the custom style: <see cref="Control.ForeColor" /> faded towards <see cref="Control.BackColor" />.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color DisabledForeColor
    {
        get
        {
            int Fade(int fore, int back) => back + (fore - back) * DisabledForeAlpha / 255;
            return Color.FromArgb(Fade(ForeColor.R, BackColor.R), Fade(ForeColor.G, BackColor.G), Fade(ForeColor.B, BackColor.B));
        }
    }

    /// <inheritdoc />
    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent);
        if (_defaultStyle || Enabled || Text.Length == 0 || Image is not null || BackgroundImage is not null)
            return;

        // WinForms berie farbu neaktivneho textu z tmavsieho odtiena pozadia - na tmavom pozadi by bol takmer cierny
        var area = Rectangle.Inflate(ClientRectangle, -ExFlatAppearance.BorderSize, -ExFlatAppearance.BorderSize);
        using (var brush = new SolidBrush(BackColor))
            pevent.Graphics.FillRectangle(brush, area);

        var text = new Rectangle(area.X + Padding.Left, area.Y + Padding.Top, area.Width - Padding.Horizontal, area.Height - Padding.Vertical);
        TextRenderer.DrawText(pevent.Graphics, Text, Font, text, DisabledForeColor, TextFlags());
    }

    private TextFormatFlags TextFlags()
    {
        var flags = TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl;
        if (!UseMnemonic)
            flags |= TextFormatFlags.NoPrefix;
        else if (!ShowKeyboardCues)
            flags |= TextFormatFlags.HidePrefix;

        const ContentAlignment right = ContentAlignment.TopRight | ContentAlignment.MiddleRight | ContentAlignment.BottomRight;
        const ContentAlignment center = ContentAlignment.TopCenter | ContentAlignment.MiddleCenter | ContentAlignment.BottomCenter;
        const ContentAlignment bottom = ContentAlignment.BottomLeft | ContentAlignment.BottomCenter | ContentAlignment.BottomRight;
        const ContentAlignment middle = ContentAlignment.MiddleLeft | ContentAlignment.MiddleCenter | ContentAlignment.MiddleRight;
        flags |= (TextAlign & right) != 0 ? TextFormatFlags.Right : (TextAlign & center) != 0 ? TextFormatFlags.HorizontalCenter : TextFormatFlags.Left;
        flags |= (TextAlign & bottom) != 0 ? TextFormatFlags.Bottom : (TextAlign & middle) != 0 ? TextFormatFlags.VerticalCenter : TextFormatFlags.Top;
        return flags;
    }

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
