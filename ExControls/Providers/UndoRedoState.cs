namespace ExControls.Providers;

/// <summary>
///     State of the undo-redo manager.
/// </summary>
public enum UndoRedoState
{
    /// <summary>
    ///     Undo/Back method was invoked.
    /// </summary>
    Undo,

    /// <summary>
    ///     Redo/Forward method was invoked.
    /// </summary>
    Redo,

    /// <summary>
    ///     Manager was cleared.
    /// </summary>
    Clear
}