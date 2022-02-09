using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;

namespace ExControls.Designers;

internal class TitleBarDesigner : ParentControlDesigner
{
    public override void Initialize(IComponent component)
    {
        base.Initialize(component);
        var bar = (TitleBar) component;
        //EnableDesignMode(bar.PanelCustom, "CustomPanel");
    }

    protected override bool DrawGrid => false;

    public override GlyphCollection GetGlyphs(GlyphSelectionType selectionType)
    {
        return new GlyphCollection();
        //return base.GetGlyphs(selectionType);
    }

    protected override void OnPaintAdornments(PaintEventArgs pe)
    {
        //base.OnPaintAdornments(pe);
    }

    public override SelectionRules SelectionRules => SelectionRules.Locked;
}