namespace ExControls;

/// <summary>
/// Part of the custom title bar of a form at a point - determines what Windows does with the mouse (<see cref="ExTitleBar.HitTest" />).
/// </summary>
public enum TitleBarHit
{
    /// <summary>
    /// A control in the title bar (menu item, tool strip button) - it handles the mouse itself.
    /// </summary>
    Client,

    /// <summary>
    /// Free space of the title bar - dragging the window, double click to maximize, system menu on right click.
    /// </summary>
    Caption,

    /// <summary>
    /// Window icon - system menu.
    /// </summary>
    SystemMenu,

    /// <summary>
    /// Minimize button.
    /// </summary>
    MinimizeButton,

    /// <summary>
    /// Maximize and restore button - in Windows 11 the system offers Snap Layouts over it.
    /// </summary>
    MaximizeButton,

    /// <summary>
    /// Close button.
    /// </summary>
    CloseButton,

    /// <summary>
    /// Top border of the window - resizing the height.
    /// </summary>
    ResizeTop,

    /// <summary>
    /// Top left corner of the window - resizing.
    /// </summary>
    ResizeTopLeft,

    /// <summary>
    /// Top right corner of the window - resizing.
    /// </summary>
    ResizeTopRight,

    /// <summary>
    /// Help button (<see cref="Form.HelpButton" />, only without minimize and maximize) - context help.
    /// </summary>
    HelpButton
}
