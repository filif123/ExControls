namespace ExControls.Controls;

/// <summary>
///     Every Control in this library implements this interface.
/// </summary>
public interface IExControl
{
    /// <summary>
    ///     Default style of the Control.
    /// </summary>
    public bool DefaultStyle { get; set; }

    /// <summary>Occurs when the <see cref="DefaultStyle" /> property changed.</summary>
    public event EventHandler DefaultStyleChanged;
}

/// <summary>
///     This interface is implemented by RadioButton and Checkbox as checkable controls.
/// </summary>
public interface ICheckableExControl
{
    /// <summary>
    ///     Text aligment.
    /// </summary>
    public ContentAlignment TextAlign { get; set; }

    /// <summary>
    ///     CheckBox/RadioButton aligment.
    /// </summary>
    public ContentAlignment CheckAlign { get; set; }

    /// <summary>
    ///     Bounds of this control.
    /// </summary>
    public Rectangle ClientRectangle { get; }

    /// <summary>
    ///     Right-to-left aligment.
    /// </summary>
    public RightToLeft RightToLeft { get; set; }

    /// <summary>
    ///     Text of the Control.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    ///     Font of the Text.
    /// </summary>
    public Font Font { get; set; }
}

/// <summary>Notifies clients that a property value has changed.</summary>
public interface IExNotifyPropertyChanged
{
    /// <summary>Occurs when a property value changed.</summary>
    event EventHandler<ExPropertyChangedEventArgs> PropertyChanged;
}