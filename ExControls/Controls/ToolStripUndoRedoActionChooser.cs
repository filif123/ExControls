using System.Windows.Forms.Design;

namespace ExControls;

/// <summary>
/// Drop-down with a list of Undo/Redo actions (<see cref="UndoRedoActionChooser" />).
/// </summary>
[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.ContextMenuStrip)]
public class ToolStripUndoRedoActionChooser : ToolStripDropDown
{
    /// <summary>
    /// The list of actions in the drop-down.
    /// </summary>
    public UndoRedoActionChooser Chooser { get; }

    /// <summary>Initializes a new instance of the <see cref="ToolStripUndoRedoActionChooser" /> class.</summary>
    public ToolStripUndoRedoActionChooser()
    {
        Chooser = new UndoRedoActionChooser();
        base.Items.Add(new ToolStripControlHost(Chooser));
    }
}