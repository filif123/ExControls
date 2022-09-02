namespace ExControls.Providers;

/// <summary>
/// 
/// </summary>
public class UndoRedoStateEventArgs : EventArgs
{
    /// <inheritdoc />
    public UndoRedoStateEventArgs(UndoRedoState newState)
    {
        NewState = newState;
    }

    /// <summary>
    ///     New state of manager.
    /// </summary>
    public UndoRedoState NewState { get; }
}