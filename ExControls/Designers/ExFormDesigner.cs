using System.Windows.Forms.Design;

namespace ExControls.Designers;

/// <summary>
/// 
/// </summary>
public class ExFormDesigner : DocumentDesigner
{
    private ExForm FormControl => Control as ExForm;

    /// <inheritdoc />
    public override void Initialize(IComponent component)
    {
        base.Initialize(component);
        var form = (ExForm) component;
        /*if (GetService(typeof(INestedContainer)) is INestedContainer service)
        {
            service.Remove(_bar);
        }*/
        EnableDesignMode(form.CustomPanel, "CustomPanel");
        //EnableDesignMode(form.PanelRoot,"Panel");
    }

    /*protected override Control GetParentForComponent(IComponent component)
    {
        return FormControl.PanelRoot;
    }*/
}