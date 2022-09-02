namespace ExControls.Providers;

/// <summary>
/// 
/// </summary>
public class UndoRedoAddedCommandEventArgs : EventArgs
{
    /// <summary>
    ///     New command added to history.
    /// </summary>
    public IUndoRedoCommand NewCommand { get; }

    /// <summary>
    ///     New command handler added to history. Null if undefined.
    /// </summary>
    public IUndoHandler NewHandler { get; }

    /// <inheritdoc />
    public UndoRedoAddedCommandEventArgs(IUndoRedoCommand newCommand, IUndoHandler newHandler)
    {
        NewCommand = newCommand;
        NewHandler = newHandler;
    }
}