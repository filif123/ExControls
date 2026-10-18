namespace ExControls.Providers;

/// <summary>
/// Provides data for the <see cref="BackwardForwardProvider.CommandAdded"/> event.
/// </summary>
/// <seealso cref="System.EventArgs" />
public class BackwardForwardAddedCommandEventArgs : EventArgs
{
    /// <summary>
    /// New command added to history.
    /// </summary>
    public IBackwardForwardCommand NewCommand { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BackwardForwardAddedCommandEventArgs" /> class.
    /// </summary>
    /// <param name="newCommand">command added to the history</param>
    public BackwardForwardAddedCommandEventArgs(IBackwardForwardCommand newCommand)
    {
        NewCommand = newCommand;
    }
}