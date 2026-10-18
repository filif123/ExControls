namespace ExControls;

/// <summary>
/// A control that styles itself by a theme (including its child controls).
/// </summary>
public interface IThemeable
{
    /// <summary>
    /// Sets the colors and appearance of the control by the theme.
    /// </summary>
    void ApplyTheme(ExTheme theme);
}
