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
