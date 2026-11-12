namespace ExControls;

/// <summary>
/// Data of <see cref="ExDockManager.PanelClosing" />: the user hides a panel (its close button, its window menu or the
/// floating window); setting <see cref="CancelEventArgs.Cancel" /> keeps it open.
/// </summary>
/// <param name="panel">panel being hidden</param>
public class ExDockPanelClosingEventArgs(ExDockPanel panel) : CancelEventArgs
{
    /// <summary>
    /// Gets the panel being hidden.
    /// </summary>
    public ExDockPanel Panel { get; } = panel;
}
