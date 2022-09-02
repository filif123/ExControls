namespace ExControls.Providers;

/// <summary>
///    Provides data for the <see cref="ExControls.Providers.BackwardForwardProvider.AddedCommand"/> event.
/// </summary>
/// <seealso cref="System.EventArgs" />
public class BackwardForwardAddedCommandEventArgs : EventArgs
{
    /// <summary>
    ///     New command added to history.
    /// </summary>
    public IBackwardForwardCommand NewCommand { get; }

    /// <inheritdoc />
    public BackwardForwardAddedCommandEventArgs(IBackwardForwardCommand newCommand)
    {
        NewCommand = newCommand;
    }
}