// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable ConvertToAutoPropertyWhenPossible
// ReSharper disable ClassNeverInstantiated.Global

using ExControls.Designers;

namespace ExControls;

/// <summary>
/// 
/// </summary>
[Designer(typeof(ExPropertyGridDesigner))]
[ToolboxBitmap(typeof(PropertyGrid),"PropertyGrid.bmp")]
public class ExPropertyGrid : PropertyGrid
{
    private readonly ToolStrip innerToolStrip;

    /// <summary>
    /// 
    /// </summary>
    public ExPropertyGrid()
    {
        innerToolStrip = Controls.OfType<ToolStrip>().FirstOrDefault();
        if (innerToolStrip is not null) return;
        var type = Type.GetType("System.Windows.Forms.PropertyGridToolStrip");
        innerToolStrip = Controls.OfType<Control>().FirstOrDefault(c => c.GetType() == type) as ToolStrip;
    }

    /// <summary>
    /// 
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Layout)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ToolStrip InnerToolStrip => innerToolStrip;
}