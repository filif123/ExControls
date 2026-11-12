namespace ExControls;

/// <summary>
/// Data of an event about an <see cref="ExDockPanel" />.
/// </summary>
/// <param name="panel">panel the event is about</param>
public class ExDockPanelEventArgs(ExDockPanel panel) : EventArgs
{
    /// <summary>
    /// Gets the panel the event is about.
    /// </summary>
    public ExDockPanel Panel { get; } = panel;
}
