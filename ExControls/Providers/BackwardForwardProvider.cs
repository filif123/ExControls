namespace ExControls.Providers;

/// <summary>
///     Provides backward-forward capability.
/// </summary>
public class BackwardForwardProvider : Component
{
    private readonly Stack<IBackwardForwardCommand> _backwardStack, _forwardStack;
    private IBackwardForwardCommand _currentCommand;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<UndoRedoStateEventArgs> BackwardForwardStateChanged;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<BackwardForwardAddedCommandEventArgs> CommandAdded;

    /// <summary>
    ///     Enables or disables manager to add commands to stacks. Dafault is false.
    /// </summary>
    [DefaultValue(false)]
    public bool ManagerEnabled { get; set; }

    /// <summary>
    ///     Represents current command.
    /// </summary>
    [Browsable(false)]
    public IBackwardForwardCommand CurrentCommand
    {
        get => _currentCommand;
        protected set => _currentCommand = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    ///     Check if there is something to undo. Use this method to decide
    /// whether your application's "Back" menu item should be enabled
    /// or disabled.
    /// </summary>
    [Browsable(false)]
    public bool CanBackward => _backwardStack.Count != 0;

    /// <summary>
    ///     Check if there is something to redo. Use this method to decide
    /// whether your application's "Forward" menu item should be enabled
    /// or disabled.
    /// </summary>
    [Browsable(false)]
    public bool CanForward => _forwardStack.Count != 0;

    /// <summary>
    ///     Get the next (or newest) undo command. This is like a "Peek"
    /// method. It does not remove the command from the undo list.
    /// </summary>
    [Browsable(false)]
    public IBackwardForwardCommand NextBackwardCommand => _backwardStack.Peek();

    /// <summary>
    ///     Get the next redo command. This is like a "Peek"
    /// method. It does not remove the command from the redo stack.
    /// </summary>
    [Browsable(false)]
    public IBackwardForwardCommand NextForwardCommand => _forwardStack.Peek();

    /// <summary>
    ///     Constructor which initializes the manager.
    /// </summary>
    public BackwardForwardProvider()
    {
        _backwardStack = new Stack<IBackwardForwardCommand>();
        _forwardStack = new Stack<IBackwardForwardCommand>();
        ManagerEnabled = false;
    }

    /// <summary>
    ///     Register a new back action command. Use this method after your
    /// application has performed an operation/command that is
    /// undoable.
    /// </summary>
    public void AddCommand(IBackwardForwardCommand cmd)
    {
        if (!ManagerEnabled)
            return;
        if (cmd is null)
            throw new ArgumentNullException(nameof(cmd));

        if(CurrentCommand is not null)
            _backwardStack.Push(CurrentCommand);
        CurrentCommand = cmd;

        if (CanForward) _forwardStack.Clear();
        OnCommandAdded(new BackwardForwardAddedCommandEventArgs(cmd));
    }

    /// <summary>
    ///     Clear the internal back/forward data structures. Use this method
    /// when your application performs an operation that cannot be undone.
    /// For example, when the user "saves" or "commits" all the changes in
    /// the application, or when a form is closed.
    /// </summary>
    public void Clear()
    {
        _backwardStack.Clear();
        _forwardStack.Clear();
        OnBackwardForwardStateChanged(new UndoRedoStateEventArgs(UndoRedoState.Clear));
    }

    /// <summary>
    ///     Perform the back operation.
    /// If an undo handler was specified, it will be used to perform the actual operation.
    /// Otherwise, the command instance is asked to perform the undo.
    /// </summary>
    public void Backward()
    {
        if (!ManagerEnabled)
            throw new InvalidOperationException("Manager is disabled");
        if (!CanBackward)
            throw new InvalidOperationException("Cannot backward because backward stack is empty");
        if (CurrentCommand is null)
            throw new InvalidOperationException("Current state is null");

        _forwardStack.Push(CurrentCommand);
        CurrentCommand = _backwardStack.Pop();
        CurrentCommand.Move();

        OnBackwardForwardStateChanged(new UndoRedoStateEventArgs(UndoRedoState.Undo));
    }

    /// <summary>
    ///     Perform the forward operation.
    /// If an undo handler was specified, it will be used to perform the actual operation.
    /// Otherwise, the command instance is asked to perform the undo.
    /// </summary>
    public void Backward(IBackwardForwardCommand finalCmd)
    {
        if (!ManagerEnabled)
            throw new InvalidOperationException("Manager is disabled");
        if (finalCmd is null)
            throw new ArgumentNullException(nameof(finalCmd));
        if (!CanBackward)
            throw new InvalidOperationException("Cannot backward because backward stack is empty");
        if (CurrentCommand is null)
            throw new InvalidOperationException("Current state is null");
        if (finalCmd == CurrentCommand)
            throw new ArgumentException("Argument cannot be CurrentCommand.");
        if (!_backwardStack.Contains(finalCmd))
            throw new ArgumentException("Argument finalCmd is not in back stack.");

        while (CurrentCommand != finalCmd)
        {
            _forwardStack.Push(CurrentCommand);
            CurrentCommand = _backwardStack.Pop();
        }

        CurrentCommand.Move();

        OnBackwardForwardStateChanged(new UndoRedoStateEventArgs(UndoRedoState.Undo));
    }

    /// <summary>
    ///     Perform the forward operation.
    /// If an undo handler was specified, it will be used to perform the actual operation.
    /// Otherwise, the command instance is asked to perform the redo.
    /// </summary>
    public void Forward()
    {
        if (!ManagerEnabled)
            throw new InvalidOperationException("Manager is disabled");
        if (!CanForward)
            throw new InvalidOperationException("Cannot forward because forward stack is empty");
        if (CurrentCommand is null)
            throw new InvalidOperationException("Current state is null");

        _backwardStack.Push(CurrentCommand);
        CurrentCommand = _forwardStack.Pop();
        CurrentCommand.Move();

        OnBackwardForwardStateChanged(new UndoRedoStateEventArgs(UndoRedoState.Redo));
    }

    /// <summary>
    ///     Perform the forward operation.
    /// If an undo handler was specified, it will be used to perform the actual operation.
    /// Otherwise, the command instance is asked to perform the redo.
    /// </summary>
    public void Forward(IBackwardForwardCommand finalCmd)
    {
        if (!ManagerEnabled)
            throw new InvalidOperationException("Manager is disabled");
        if (finalCmd is null)
            throw new ArgumentNullException(nameof(finalCmd));
        if (!CanForward)
            throw new InvalidOperationException("Cannot forward because forward stack is empty");
        if (CurrentCommand is null)
            throw new InvalidOperationException("Current state is null");
        if (finalCmd == CurrentCommand)
            throw new ArgumentException("Argument cannot be CurrentCommand.");
        if (!_forwardStack.Contains(finalCmd))
            throw new ArgumentException("Argument finalCmd is not in forward stack.");

        while (CurrentCommand != finalCmd)
        {
            _backwardStack.Push(CurrentCommand);
            CurrentCommand = _forwardStack.Pop();
        }

        CurrentCommand.Move();

        OnBackwardForwardStateChanged(new UndoRedoStateEventArgs(UndoRedoState.Redo));
    }

    /// <summary>
    ///     Get the text value of the next back command. Use this method
    /// to update the Text property of your "Back" menu item if
    /// desired. For example, the text value for a command might be "Draw Circle".
    /// </summary>
    public string GetBackwardText() => CanBackward ? NextBackwardCommand.CommandName : null;

    /// <summary>
    ///     Get the text value of the next forward command. Use this method
    /// to update the Text property of your "Forward" menu item if desired.
    /// For example, the text value for a command might be "Draw Line".
    /// </summary>
    public string GetForwardText() => CanForward ? NextForwardCommand.CommandName : null;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IBackwardForwardCommand> GetBackwardHistory() => _backwardStack;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IBackwardForwardCommand> GetForwardHistory() => _forwardStack;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    protected virtual void OnBackwardForwardStateChanged(UndoRedoStateEventArgs e) => BackwardForwardStateChanged?.Invoke(this, e);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    protected virtual void OnCommandAdded(BackwardForwardAddedCommandEventArgs e) => CommandAdded?.Invoke(this, e);
}