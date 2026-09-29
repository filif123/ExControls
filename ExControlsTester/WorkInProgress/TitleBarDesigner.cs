using System.Windows.Forms.Design;

#if NETFRAMEWORK
using System.Windows.Forms.Design.Behavior;
#else
using Microsoft.DotNet.DesignTools.Designers;
using Microsoft.DotNet.DesignTools.Designers.Behaviors;
#endif

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