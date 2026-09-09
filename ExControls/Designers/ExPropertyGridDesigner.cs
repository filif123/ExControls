using System.Collections;

namespace ExControls.Designers;

internal class ExPropertyGridDesigner : DesignerControlBase<ExPropertyGrid>
{
    /// <inheritdoc />
    protected override void PreFilterProperties(IDictionary properties)
    {
        properties.Remove("AutoScroll");
        properties.Remove("AutoScrollMargin");
        properties.Remove("DockPadding");
        properties.Remove("AutoScrollMinSize");
        base.PreFilterProperties(properties);
    }

    /// <inheritdoc />
    public override void Initialize(IComponent component)
    {
        base.Initialize(component);
        if (ControlHost?.InnerToolStrip is not null)
            EnableDesignMode(ControlHost.InnerToolStrip, "InnerToolStrip");
    }
}