#if NETFRAMEWORK
using System.Windows.Forms.Design;
#else
using Microsoft.DotNet.DesignTools.Designers;
#endif

namespace ExControls.Designers;

/// <summary>
/// 
/// </summary>
public class ExFormDesigner : DocumentDesigner
{
    private ExFormTest FormControl => Control as ExFormTest;

    /// <inheritdoc />
    public override void Initialize(IComponent component)
    {
        base.Initialize(component);
        var form = (ExFormTest) component;
        /*if (GetService(typeof(INestedContainer)) is INestedContainer service)
        {
            service.Remove(_bar);
        }*/
        EnableDesignMode(form.CustomPanel, "CustomPanel");
        //EnableDesignMode(form.PanelRoot,"Panel");
        var dropDown = new ToolStripDropDown();
        //dropDown.Close(Too);
    }

    /*protected override Control GetParentForComponent(IComponent component)
    {
        return FormControl.PanelRoot;
    }*/
}