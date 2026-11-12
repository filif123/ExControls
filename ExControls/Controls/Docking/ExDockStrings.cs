namespace ExControls;

/// <summary>
/// Texts of the tool tips and the window menu of <see cref="ExDockManager" /> panels. The library has no UI texts of its
/// own - the defaults are English, the application sets its translation.
/// </summary>
public sealed class ExDockStrings
{
    /// <summary>
    /// Gets or sets the text of the button and menu item that hides the panel.
    /// </summary>
    public string Hide { get; set; } = "Hide";

    /// <summary>
    /// Gets or sets the text of the button and menu item that collapses the panel group to the strip at the edge.
    /// </summary>
    public string AutoHide { get; set; } = "Auto Hide";

    /// <summary>
    /// Gets or sets the text of the menu item that moves the panel group to a floating window.
    /// </summary>
    public string FloatWindow { get; set; } = "Float";

    /// <summary>
    /// Gets or sets the text of the menu item that docks a floating or collapsed panel group again.
    /// </summary>
    public string Dock { get; set; } = "Dock";

    /// <summary>
    /// Gets or sets the tool tip of the button with the window menu.
    /// </summary>
    public string Options { get; set; } = "Options";
}
