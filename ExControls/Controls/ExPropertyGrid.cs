namespace ExControls.Controls;

/// <summary>
/// 
/// </summary>
public class ExPropertyGrid : PropertyGrid
{
    private readonly ToolStrip innerToolStrip;

    /// <summary>
    /// 
    /// </summary>
    public ExPropertyGrid()
    {
        innerToolStrip = Controls.OfType<ToolStrip>().FirstOrDefault();
    }

    /// <summary>
    /// 
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Layout)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public ToolStripItemCollection InnerToolStripItems => innerToolStrip.Items; //BUG doesnt work
}