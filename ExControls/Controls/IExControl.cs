namespace ExControls.Controls;

/// <summary>
/// Control of this library that can be drawn in a custom style besides the default Windows style.
/// </summary>
public interface IExControl
{
    /// <summary>
    /// Gets or sets whether the control uses the default Windows style; <see langword="false" /> = the custom style drawn by the
    /// control with its own colors.
    /// </summary>
    public bool DefaultStyle { get; set; }

    /// <summary>Occurs when the <see cref="DefaultStyle" /> property changed.</summary>
    public event EventHandler DefaultStyleChanged;
}

/// <summary>
/// Checkable control (<see cref="ExCheckBox" />, <see cref="ExRadioButton" />) whose box and text are laid out by
/// <see cref="ExButtonRenderer.GetBoxAndTextRectangle" />.
/// </summary>
public interface ICheckableExControl
{
    /// <summary>
    /// Gets or sets the alignment of the text.
    /// </summary>
    public ContentAlignment TextAlign { get; set; }

    /// <summary>
    /// Gets or sets the alignment of the check box or radio button mark.
    /// </summary>
    public ContentAlignment CheckAlign { get; set; }

    /// <summary>
    /// Gets the client area of the control.
    /// </summary>
    public Rectangle ClientRectangle { get; }

    /// <summary>
    /// Gets or sets whether the control is laid out from right to left.
    /// </summary>
    public RightToLeft RightToLeft { get; set; }

    /// <summary>
    /// Gets or sets the text of the control.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets the font of the text.
    /// </summary>
    public Font Font { get; set; }
}

/// <summary>Notifies clients that a property value has changed.</summary>
public interface IExNotifyPropertyChanged
{
    /// <summary>Occurs when a property value changed.</summary>
    event EventHandler<ExPropertyChangedEventArgs> PropertyChanged;
}