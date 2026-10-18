namespace ExControls.Providers;

/// <summary>
/// Provides data for the <see cref="UndoRedoManager.CommandAdded" /> event.
/// </summary>
public class UndoRedoAddedCommandEventArgs : EventArgs
{
    /// <summary>
    /// New command added to history.
    /// </summary>
    public IUndoRedoCommand NewCommand { get; }

    /// <summary>
    /// New command handler added to history. Null if undefined.
    /// </summary>
    public IUndoHandler? NewHandler { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UndoRedoAddedCommandEventArgs" /> class.
    /// </summary>
    /// <param name="newCommand">command added to the history</param>
    /// <param name="newHandler">handler of the command; <see langword="null" /> if it has none</param>
    public UndoRedoAddedCommandEventArgs(IUndoRedoCommand newCommand, IUndoHandler? newHandler)
    {
        NewCommand = newCommand;
        NewHandler = newHandler;
    }
}