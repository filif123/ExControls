namespace ExControls.Providers;

/// <summary>
/// Provides data for the <see cref="UndoRedoManager.UndoRedoStateChanged" /> and
/// <see cref="BackwardForwardProvider.BackwardForwardStateChanged" /> events.
/// </summary>
public class UndoRedoStateEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UndoRedoStateEventArgs" /> class.
    /// </summary>
    /// <param name="newState">new state of the manager</param>
    public UndoRedoStateEventArgs(UndoRedoState newState)
    {
        NewState = newState;
    }

    /// <summary>
    /// New state of manager.
    /// </summary>
    public UndoRedoState NewState { get; }
}