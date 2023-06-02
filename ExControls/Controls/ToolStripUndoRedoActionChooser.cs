using System.Windows.Forms.Design;

namespace ExControls;

[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.ContextMenuStrip)]
public class ToolStripUndoRedoActionChooser : ToolStripDropDown
{
    public UndoRedoActionChooser Chooser { get; }

    public ToolStripUndoRedoActionChooser()
    {
        Chooser = new UndoRedoActionChooser();
        base.Items.Add(new ToolStripControlHost(Chooser));
    }
}