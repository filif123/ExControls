// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable EventNeverSubscribedTo.Global
namespace ExControls.Providers;

/// <summary>
///     Provides undo-redo capability.
/// </summary>
public class UndoRedoManager : Component
{
    private readonly Stack<(IUndoRedoCommand cmd, IUndoHandler handler)> _undoStack, _redoStack;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<UndoRedoStateEventArgs> UndoRedoStateChanged;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<UndoRedoAddedCommandEventArgs> CommandAdded;

    /// <summary>
    ///     Enables or disables manager to add commands to stacks. Dafault is false.
    /// </summary>
    [DefaultValue(false)]
    public bool ManagerEnabled { get; set; }

    /// <summary>
    ///     Represents last saved undo command in history.
    /// </summary>
    protected IUndoRedoCommand SavedStateCommand { get; set; }

    /// <summary>
    ///     Check if there is something to undo. Use this method to decide
    /// whether your application's "Undo" menu item should be enabled
    /// or disabled.
    /// </summary>
    [Browsable(false)]
    public bool CanUndo => _undoStack.Count > 0;

    /// <summary>
    ///     Check if there is something to redo. Use this method to decide
    /// whether your application's "Redo" menu item should be enabled
    /// or disabled.
    /// </summary>
    [Browsable(false)]
    public bool CanRedo => _redoStack.Count > 0;

    /// <summary>
    ///     Get the next (or newest) undo command. This is like a "Peek"
    /// method. It does not remove the command from the undo list.
    /// </summary>
    [Browsable(false)]
    public IUndoRedoCommand NextUndoCommand => _undoStack.Peek().cmd;

    /// <summary>
    ///     Get the next redo command. This is like a "Peek"
    /// method. It does not remove the command from the redo stack.
    /// </summary>
    [Browsable(false)]
    public IUndoRedoCommand NextRedoCommand => _redoStack.Peek().cmd;

    /// <summary>
    ///     Constructor which initializes the manager.
    /// </summary>
    public UndoRedoManager()
    {
        _undoStack = new Stack<(IUndoRedoCommand, IUndoHandler)>();
        _redoStack = new Stack<(IUndoRedoCommand, IUndoHandler)>();
        ManagerEnabled = false;
    }

    /// <summary>
    ///     Register a new undo command. Use this method after your
    /// application has performed an operation/command that is
    /// undoable.
    /// </summary>
    public void AddCommand(IUndoRedoCommand cmd)
    {
        if (!ManagerEnabled)
            return;
        if (cmd is null)
            throw new ArgumentNullException(nameof(cmd));

        _undoStack.Push((cmd, null));
        if (CanRedo) _redoStack.Clear();
        OnCommandAdded(new UndoRedoAddedCommandEventArgs(cmd, null));
    }

    /// <summary>
    ///     Register a new undo command along with an undo handler.
    /// The undo handler is used to perform the actual undo or redo
    /// operation later when requested.
    /// </summary>
    public void AddCommand(IUndoRedoCommand cmd, IUndoHandler handler)
    {
        if (!ManagerEnabled)
            return;
        if (cmd is null)
            throw new ArgumentNullException(nameof(cmd));

        _undoStack.Push((cmd, handler));
        if (CanRedo) 
            _redoStack.Clear();
        OnCommandAdded(new UndoRedoAddedCommandEventArgs(cmd, handler));
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
    ///     Sets current state as saved.
    /// </summary>
    public void SetSavedState() => SavedStateCommand = _undoStack.Count == 0 ? null : NextUndoCommand;

    /// <summary>
    ///     Returns whether the manager is in saved state.
    /// </summary>
    /// <returns></returns>
    public bool IsInSavedState() => ReferenceEquals(_undoStack.Count == 0 ? null : NextUndoCommand, SavedStateCommand);

    /// <summary>
    ///     Perform the undo operation.
    /// If an undo handler was specified, it will be used to perform the actual operation.
    /// Otherwise, the command instance is asked to perform the undo.
    /// </summary>
    public void Undo()
    {
        if (!ManagerEnabled)
            throw new InvalidOperationException("Manager is disabled");
        if (!CanUndo)
            throw new InvalidOperationException("Cannot undo because undo stack is empty");

        var cmd = _undoStack.Pop();

        if (cmd.handler != null)
            cmd.handler.Undo(cmd.cmd);
        else
            cmd.cmd.Undo();

        _redoStack.Push(cmd);
        OnUndoRedoStateChanged(new UndoRedoStateEventArgs(UndoRedoState.Undo));
    }

    /// <summary>
    ///     Perform the undo operation.
    /// If an undo handler was specified, it will be used to perform the actual operation.
    /// Otherwise, the command instance is asked to perform the undo.
    /// </summary>
    public void Undo(IUndoRedoCommand finalCmd)
    {
        if (!ManagerEnabled)
            throw new InvalidOperationException("Manager is disabled");
        if (!CanUndo)
            throw new InvalidOperationException("Cannot undo because undo stack is empty");
        if (_undoStack.All(x => x.cmd != finalCmd)) //if finalCmd is not in undo stack
            throw new ArgumentException("Argument finalCmd is not in undo stack.");

        while (NextUndoCommand != finalCmd)
        {
            Undo();
        }
    }

    /// <summary>
    ///     Perform the redo operation.
    /// If an undo handler was specified, it will be used to perform the actual operation.
    /// Otherwise, the command instance is asked to perform the redo.
    /// </summary>
    public void Redo()
    {
        if (!ManagerEnabled)
            throw new InvalidOperationException("Manager is disabled");
        if (!CanRedo)
            throw new InvalidOperationException("Cannot redo because redo stack is empty");

        var cmd = _redoStack.Pop();

        if (cmd.handler != null)
            cmd.handler.Redo(cmd.cmd);
        else
            cmd.cmd.Redo();

        _undoStack.Push(cmd);
        OnUndoRedoStateChanged(new UndoRedoStateEventArgs(UndoRedoState.Redo));
    }

    /// <summary>
    ///     Perform the redo operation.
    /// If an undo handler was specified, it will be used to perform the actual operation.
    /// Otherwise, the command instance is asked to perform the redo.
    /// </summary>
    public void Redo(IUndoRedoCommand finalCmd)
    {
        if (!ManagerEnabled)
            throw new InvalidOperationException("Manager is disabled");
        if (!CanRedo)
            throw new InvalidOperationException("Cannot redo because redo stack is empty");
        if (_redoStack.All(x => x.cmd != finalCmd)) //if finalCmd is not in redo stack
            throw new ArgumentException("Argument finalCmd is not in redo stack.");

        while (NextRedoCommand != finalCmd)
        {
            Redo();
        }
    }

    /// <summary>
    ///     Get the text value of the next undo command. Use this method
    /// to update the Text property of your "Undo" menu item if
    /// desired. For example, the text value for a command might be
    /// "Draw Circle". This allows you to change your menu item Text
    /// property to "Undo Draw Circle".
    /// </summary>
    public string GetUndoText() => CanUndo ? NextUndoCommand.CommandName : null;

    /// <summary>
    ///     Get the text value of the next redo command. Use this method
    /// to update the Text property of your "Redo" menu item if desired.
    /// For example, the text value for a command might be "Draw Line".
    /// This allows you to change your menu item text to "Redo Draw Line".
    /// </summary>
    public string GetRedoText() => CanRedo ? NextRedoCommand.CommandName : null;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IUndoRedoCommand> GetUndoHistory()
    {
        return _undoStack.Select(x => x.cmd);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IUndoRedoCommand> GetRedoHistory()
    {
        return _redoStack.Select(x => x.cmd);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    protected virtual void OnUndoRedoStateChanged(UndoRedoStateEventArgs e) => UndoRedoStateChanged?.Invoke(this, e);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    protected virtual void OnCommandAdded(UndoRedoAddedCommandEventArgs e) => CommandAdded?.Invoke(this, e);
}