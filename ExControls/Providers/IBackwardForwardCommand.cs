namespace ExControls.Providers;

/// <summary>
///     Provides an interface for all commands to use in the BackwardForwardProvider.
/// </summary>
public interface IBackwardForwardCommand
{
    /// <summary>
    ///     Gets a name of this command.
    /// </summary>
    public string CommandName { get; }

    /// <summary>
    ///     Executes the move.
    /// </summary>
    void Move();
}