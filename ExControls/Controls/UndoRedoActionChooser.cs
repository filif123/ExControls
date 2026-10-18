using ExControls.Providers;

namespace ExControls;

/// <summary>
/// List of Undo/Redo actions - selecting an action determines up to which one to undo or redo.
/// </summary>
public partial class UndoRedoActionChooser : UserControl
{
    /// <summary>
    /// The selected last action; <see langword="null" /> if the user selected nothing.
    /// </summary>
    public IUndoRedoCommand? FinalCommand { get; private set; }

    private ExBindingList<IUndoRedoCommand> Commands { get; }
    
    /// <summary>Initializes a new instance of the <see cref="UndoRedoActionChooser" /> class.</summary>
    public UndoRedoActionChooser()
    {
        InitializeComponent();

        Commands = new ExBindingList<IUndoRedoCommand>();
        Commands.ListChanged += CommandsOnListChanged;
    }

    private void CommandsOnListChanged(object? sender, ListChangedEventArgs e)
    {
        lbActions.Items.Clear();
        foreach (var command in Commands)
        {
            lbActions.Items.Add(new CommandWrapper(command));
        }
    }

    /// <summary>
    /// Clears the selection and selects the first action; call it whenever the list is shown.
    /// </summary>
    public void OnShown()
    {
        lbActions.SelectedItems.Clear();
        if (lbActions.Items.Count != 0) 
            lbActions.SelectedIndex = 0;
    }

    private void LbActions_MouseMove(object sender, MouseEventArgs e)
    {
        var index = lbActions.IndexFromPoint(e.Location);
        if (index == ListBox.NoMatches)
            return;

        lbActions.SelectedItems.Clear();

        for (var i = 0; i <= index; i++)
        {
            lbActions.SelectedIndices.Add(i);
        }
    }

    private sealed class CommandWrapper
    {
        public IUndoRedoCommand Command { get; }

        public CommandWrapper(IUndoRedoCommand command)
        {
            Command = command;
        }

        public override string ToString()
        {
            return Command.CommandName;
        }
    }

    private void LbActions_Click(object sender, EventArgs e)
    {
        FinalCommand = lbActions.SelectedItems.OfType<CommandWrapper>().LastOrDefault()?.Command;
    }
}