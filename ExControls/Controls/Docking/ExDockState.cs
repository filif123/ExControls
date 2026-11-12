namespace ExControls;

/// <summary>
/// State of an <see cref="ExDockPanel" /> in its <see cref="ExDockManager" />.
/// </summary>
public enum ExDockState
{
    /// <summary>
    /// The panel is hidden (closed by the user, by <see cref="ExDockManager.HidePanel" /> or not added to a manager);
    /// <see cref="ExDockManager.ShowPanel" /> returns it to its last place.
    /// </summary>
    Hidden,

    /// <summary>
    /// The panel is docked in a zone of the manager next to the document.
    /// </summary>
    Docked,

    /// <summary>
    /// The panel is collapsed to the strip at the edge of the manager and slides out over the document when its button is
    /// clicked.
    /// </summary>
    AutoHide,

    /// <summary>
    /// The panel is in a floating window owned by the form of the manager.
    /// </summary>
    Floating
}
