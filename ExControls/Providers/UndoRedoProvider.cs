// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable EventNeverSubscribedTo.Global
namespace ExControls.Providers;

/// <summary>
///     Provides undo-redo capability.
/// </summary>
public class UndoRedoManager
{
    private readonly Stack<(ICommand cmd, IUndoHandler handler)> _undoStack, _redoStack;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<UndoRedoStateEventArgs> UndoRedoStateChanged;

    /// <summary>
    ///     Constructor which initializes the manager.
    /// </summary>
    public UndoRedoManager()
    {
        _undoStack = new Stack<(ICommand, IUndoHandler)>();
        _redoStack = new Stack<(ICommand, IUndoHandler)>();
        ManagerEnabled = false;
    }

    /// <summary>
    ///     Enables or disables manager to add commands to stacks. Dafault is false.
    /// </summary>
    public bool ManagerEnabled { get; set; }

    /// <summary>
    ///     Register a new undo command. Use this method after your
    /// application has performed an operation/command that is
    /// undoable.
    /// </summary>
    public void AddCommand(ICommand cmd)
    {
        if (!ManagerEnabled)
            return;
        if (cmd is null)
            throw new ArgumentNullException(nameof(cmd));
        _undoStack.Push((cmd, null));
        if (CanRedo) _redoStack.Clear();
    }

    /// <summary>
    ///     Register a new undo command along with an undo handler.
    /// The undo handler is used to perform the actual undo or redo
    /// operation later when requested.
    /// </summary>
    public void AddCommand(ICommand cmd, IUndoHandler handler)
    {
        if (!ManagerEnabled)
            return;
        if (cmd is null)
            throw new ArgumentNullException(nameof(cmd));
        _undoStack.Push((cmd, handler));
        if (CanRedo) _redoStack.Clear();
    }

    /// <summary>
    ///     Clear the internal undo/redo data structures. Use this method
    /// when your application performs an operation that cannot be undone.
    /// For example, when the user "saves" or "commits" all the changes in
    /// the application, or when a form is closed.
    /// </summary>
    public void Clear()
    {
        _undoStack.Clear();
        _redoStack.Clear();
        OnUndoRedoStateChanged(new UndoRedoStateEventArgs(UndoRedoState.Clear));
    }

    /// <summary>
    ///     Check if there is something to undo. Use this method to decide
    /// whether your application's "Undo" menu item should be enabled
    /// or disabled.
    /// </summary>
    public bool CanUndo => _undoStack.Count > 0;

    /// <summary>
    ///     Check if there is something to redo. Use this method to decide
    /// whether your application's "Redo" menu item should be enabled
    /// or disabled.
    /// </summary>
    public bool CanRedo => _redoStack.Count > 0;

    /// <summary>
    ///     Perform the undo operation. If an undo handler was specified, it
    /// will be used to perform the actual operation. Otherwise, the
    /// command instance is asked to perform the undo.
    /// </summary>
    public void Undo()
    {
        var cmd = _undoStack.Pop();

        if (cmd.handler != null)
            cmd.handler.Undo(cmd.cmd);
        else
            cmd.cmd.Undo();

        _redoStack.Push(cmd);
        OnUndoRedoStateChanged(new UndoRedoStateEventArgs(UndoRedoState.Undo));
    }

    /// <summary>
    ///     Perform the redo operation. If an undo handler was specified, it
    /// will be used to perform the actual operation. Otherwise, the
    /// command instance is asked to perform the redo.
    /// </summary>
    public void Redo()
    {
        var cmd = _redoStack.Pop();

        if (cmd.handler != null)
            cmd.handler.Redo(cmd.cmd);
        else
            cmd.cmd.Redo();

        _undoStack.Push(cmd);
        OnUndoRedoStateChanged(new UndoRedoStateEventArgs(UndoRedoState.Redo));
    }

    /// <summary>
    ///     Get the text value of the next undo command. Use this method
    /// to update the Text property of your "Undo" menu item if
    /// desired. For example, the text value for a command might be
    /// "Draw Circle". This allows you to change your menu item Text
    /// property to "Undo Draw Circle".
    /// </summary>
    public string GetUndoText() => _undoStack.Count != 0 ? _undoStack.Peek().cmd.CommandName : null;

    /// <summary>
    ///     Get the text value of the next redo command. Use this method
    /// to update the Text property of your "Redo" menu item if desired.
    /// For example, the text value for a command might be "Draw Line".
    /// This allows you to change your menu item text to "Redo Draw Line".
    /// </summary>
    public string GetRedoText() => _redoStack.Count != 0 ? _redoStack.Peek().cmd.CommandName : null;

    /// <summary>
    ///     Get the next (or newest) undo command. This is like a "Peek"
    /// method. It does not remove the command from the undo list.
    /// </summary>
    public ICommand GetNextUndoCommand() => _undoStack.Peek().cmd;

    /// <summary>
    ///     Get the next redo command. This is like a "Peek"
    /// method. It does not remove the command from the redo stack.
    /// </summary>
    public ICommand GetNextRedoCommand() => _redoStack.Peek().cmd;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    protected virtual void OnUndoRedoStateChanged(UndoRedoStateEventArgs e) => UndoRedoStateChanged?.Invoke(this, e);
}

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

/// <summary>
///     State of the undo-redo manager.
/// </summary>
public enum UndoRedoState
{
    /// <summary>
    ///     Undo method was invoked.
    /// </summary>
    Undo,

    /// <summary>
    ///     Redo method was invoked.
    /// </summary>
    Redo,

    /// <summary>
    ///     Manager was cleared.
    /// </summary>
    Clear
}

/// <summary>
///     Provides an interface for all commands to use in the UndoRedoProvider.
/// </summary>
public interface ICommand
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

/// <summary>
///     Provides an interface for handlers of undo-redo commands.
/// </summary>
public interface IUndoHandler
{
    /// <summary>
    ///     Executes the previous command if exists.
    /// </summary>
    void Undo(ICommand cmd);

    /// <summary>
    ///     Executes the next command if exists.
    /// </summary>
    void Redo(ICommand cmd);
}