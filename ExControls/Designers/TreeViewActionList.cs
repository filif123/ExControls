using System.ComponentModel.Design;
using System.Drawing.Design;

namespace ExControls.Designers;

internal class TreeViewActionList : DesignerActionList
{
    private readonly ExTreeViewDesigner _designer;

    public TreeViewActionList(ExTreeViewDesigner designer) : base(designer.Component)
    {
        _designer = designer;
    }

    public void InvokeNodesDialog()
    {
        var property = TypeDescriptor.GetProperties(Component)["Nodes"];
        var editorServiceContext = new ExEditorServiceContext(_designer, property);
        var editor = property.GetEditor(typeof(UITypeEditor)) as UITypeEditor;
        var value = property.GetValue(Component);
        var editedValue = editor?.EditValue(editorServiceContext, editorServiceContext, value);

        if (editedValue == value) 
            return;

        try
        {
            property.SetValue(Component, editedValue);
        }
        catch (CheckoutException)
        {
        }
    }

    public ImageList ImageList
    {
        get => ((TreeView)Component).ImageList;
        set => TypeDescriptor.GetProperties(Component)["ImageList"].SetValue(Component, value);
    }

    public override DesignerActionItemCollection GetSortedActionItems()
    {
        var items = new DesignerActionItemCollection
        {
            new DesignerActionMethodItem(this, nameof(InvokeNodesDialog), "Edit Nodes...", "Properties", "Edit Nodes...", true),
            new DesignerActionPropertyItem(nameof(ImageList), "Image list", "Properties", "Image list")
        };
        return items;
    }
}