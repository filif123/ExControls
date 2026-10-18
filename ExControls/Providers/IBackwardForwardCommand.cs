namespace ExControls.Providers;

/// <summary>
/// Provides an interface for all commands to use in the BackwardForwardProvider.
/// </summary>
public interface IBackwardForwardCommand
{
    /// <summary>
    /// Gets a name of this command.
    /// </summary>
    public string CommandName { get; }

    /// <summary>
    /// Returns the application to the state remembered by this command (for example selects the remembered item); the provider
    /// calls it when it moves back or forward to the command.
    /// </summary>
    void Move();
}