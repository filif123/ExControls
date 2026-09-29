using System.Windows.Forms.Design;

namespace ExControls;

/// <summary>
/// Rozbalovacia ponuka so zoznamom akcii Spat/Znovu (<see cref="UndoRedoActionChooser" />).
/// </summary>
[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.ContextMenuStrip)]
public class ToolStripUndoRedoActionChooser : ToolStripDropDown
{
    /// <summary>
    /// Zoznam akcii v ponuke.
    /// </summary>
    public UndoRedoActionChooser Chooser { get; }

    /// <summary>Initializes a new instance of the <see cref="ToolStripUndoRedoActionChooser" /> class.</summary>
    public ToolStripUndoRedoActionChooser()
    {
        Chooser = new UndoRedoActionChooser();
        base.Items.Add(new ToolStripControlHost(Chooser));
    }
}