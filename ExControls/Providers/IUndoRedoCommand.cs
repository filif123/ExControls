namespace ExControls.Providers;

/// <summary>
///     Provides an interface for all commands to use in the UndoRedoProvider.
/// </summary>
public interface IUndoRedoCommand
{
    /// <summary>
    ///     Gets a name of this command.
    /// </summary>
    public string CommandName { get; }

    /// <summary>
    ///     Executes the previous command if exists.
    /// </summary>
    void Undo();

    /// <summary>
    ///     Executes the next command if exists.
    /// </summary>
    void Redo();
}