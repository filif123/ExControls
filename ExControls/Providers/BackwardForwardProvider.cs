// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable UnusedMember.Global
namespace ExControls.Providers;

/// <summary>
/// Provides backward-forward capability.
/// </summary>
[ToolboxBitmap(typeof(ToolboxIcons), "Toolbox.BackwardForwardProvider.ico")]
public class BackwardForwardProvider : Component
{
    private readonly Stack<IBackwardForwardCommand> _backwardStack, _forwardStack;

    /// <summary>
    /// Occurs when the provider moved backward or forward in the history, or when the history was cleared.
    /// </summary>
    public event EventHandler<UndoRedoStateEventArgs>? BackwardForwardStateChanged;

    /// <summary>
    /// Occurs when a new command was added and became the current command.
    /// </summary>
    public event EventHandler<BackwardForwardAddedCommandEventArgs>? CommandAdded;

    /// <summary>
    /// Enables or disables manager to add commands to stacks. Default is false.
    /// </summary>
    [DefaultValue(false)]
    public bool ManagerEnabled { get; set; }

    /// <summary>
    /// Gets the current command - the last added one or the one the history moved to; <see langword="null" /> before the first
    /// command is added.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IBackwardForwardCommand? CurrentCommand
    {
        get;
        protected set => field = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Check if there is something to undo. Use this method to decide
    /// whether your application's "Back" menu item should be enabled
    /// or disabled.
    /// </summary>
    [Browsable(false)]
    public bool CanBackward => _backwardStack.Count != 0;

    /// <summary>
    /// Check if there is something to redo. Use this method to decide
    /// whether your application's "Forward" menu item should be enabled
    /// or disabled.
    /// </summary>
    [Browsable(false)]
    public bool CanForward => _forwardStack.Count != 0;

    /// <summary>
    /// Get the next (or newest) undo command. This is like a "Peek"
    /// method. It does not remove the command from the undo list.
    /// </summary>
    [Browsable(false)]
    public IBackwardForwardCommand NextBackwardCommand => _backwardStack.Peek();

    /// <summary>
    /// Get the next redo command. This is like a "Peek"
    /// method. It does not remove the command from the redo stack.
    /// </summary>
    [Browsable(false)]
    public IBackwardForwardCommand NextForwardCommand => _forwardStack.Peek();

    /// <summary>
    /// Initializes a new instance of the <see cref="BackwardForwardProvider" /> class with an empty history; commands are added
    /// only after <see cref="ManagerEnabled" /> is turned on.
    /// </summary>
    public BackwardForwardProvider()
    {
        _backwardStack = new Stack<IBackwardForwardCommand>();
        _forwardStack = new Stack<IBackwardForwardCommand>();
        ManagerEnabled = false;
    }

    /// <summary>
    /// Register a new back action command. Use this method after your
    /// application has performed an operation/command that is
    /// undoable.
    /// </summary>
    /// <param name="cmd">command to add to the history</param>
    public void AddCommand(IBackwardForwardCommand cmd)
    {
        if (!ManagerEnabled)
            return;
        ArgumentNullException.ThrowIfNull(cmd);

        if(CurrentCommand is not null)
            _backwardStack.Push(CurrentCommand);
        CurrentCommand = cmd;

        if (CanForward) _forwardStack.Clear();
        OnCommandAdded(new BackwardForwardAddedCommandEventArgs(cmd));
    }

    /// <summary>
    /// Clear the internal back/forward data structures. Use this method
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
    /// Moves one step back: the previous command becomes the current one and is asked to perform the move.
    /// </summary>
    /// <exception cref="InvalidOperationException">when the manager is disabled, the backward history is empty or there is no
    /// current command.</exception>
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
    /// Moves back to <paramref name="finalCmd" />: it becomes the current command and is asked to perform the move,
    /// the skipped commands go to the forward history.
    /// </summary>
    /// <param name="finalCmd">command in the backward history to move back to</param>
    /// <exception cref="InvalidOperationException">when the manager is disabled, the backward history is empty or there is no
    /// current command.</exception>
    /// <exception cref="ArgumentNullException">when <paramref name="finalCmd" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">when <paramref name="finalCmd" /> is the current command or is not in the backward
    /// history.</exception>
    public void Backward(IBackwardForwardCommand finalCmd)
    {
        if (!ManagerEnabled)
            throw new InvalidOperationException("Manager is disabled");
        ArgumentNullException.ThrowIfNull(finalCmd);
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
    /// Moves one step forward: the next command becomes the current one and is asked to perform the move.
    /// </summary>
    /// <exception cref="InvalidOperationException">when the manager is disabled, the forward history is empty or there is no
    /// current command.</exception>
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
    /// Moves forward to <paramref name="finalCmd" />: it becomes the current command and is asked to perform the move,
    /// the skipped commands go to the backward history.
    /// </summary>
    /// <param name="finalCmd">command in the forward history to move forward to</param>
    /// <exception cref="InvalidOperationException">when the manager is disabled, the forward history is empty or there is no
    /// current command.</exception>
    /// <exception cref="ArgumentNullException">when <paramref name="finalCmd" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">when <paramref name="finalCmd" /> is the current command or is not in the forward
    /// history.</exception>
    public void Forward(IBackwardForwardCommand finalCmd)
    {
        if (!ManagerEnabled)
            throw new InvalidOperationException("Manager is disabled");
        ArgumentNullException.ThrowIfNull(finalCmd);
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
    /// Get the text value of the next back command. Use this method
    /// to update the Text property of your "Back" menu item if
    /// desired. For example, the text value for a command might be "Draw Circle".
    /// </summary>
    public string GetBackwardText() => CanBackward ? NextBackwardCommand.CommandName : "";

    /// <summary>
    /// Get the text value of the next forward command. Use this method
    /// to update the Text property of your "Forward" menu item if desired.
    /// For example, the text value for a command might be "Draw Line".
    /// </summary>
    public string GetForwardText() => CanForward ? NextForwardCommand.CommandName : "";

    /// <summary>
    /// Returns the history of commands to which the provider can move backward.
    /// </summary>
    /// <returns>The backward commands, the nearest one first.</returns>
    public IEnumerable<IBackwardForwardCommand> GetBackwardHistory() => _backwardStack;

    /// <summary>
    /// Returns the history of commands to which the provider can move forward.
    /// </summary>
    /// <returns>The forward commands, the nearest one first.</returns>
    public IEnumerable<IBackwardForwardCommand> GetForwardHistory() => _forwardStack;

    /// <summary>
    /// Raises the <see cref="BackwardForwardStateChanged" /> event.
    /// </summary>
    /// <param name="e">The event data.</param>
    protected virtual void OnBackwardForwardStateChanged(UndoRedoStateEventArgs e) => BackwardForwardStateChanged?.Invoke(this, e);

    /// <summary>
    /// Raises the <see cref="CommandAdded" /> event.
    /// </summary>
    /// <param name="e">The event data.</param>
    protected virtual void OnCommandAdded(BackwardForwardAddedCommandEventArgs e) => CommandAdded?.Invoke(this, e);
}