namespace ExControls.Providers;

/// <summary>
///     Provides an interface for handlers of undo-redo commands.
/// </summary>
public interface IUndoHandler
{
    /// <summary>
    ///     Executes the previous command if exists.
    /// </summary>
    void Undo(IUndoRedoCommand cmd);

    /// <summary>
    ///     Executes the next command if exists.
    /// </summary>
    void Redo(IUndoRedoCommand cmd);
}