using System.Drawing.Design;
using System.Windows.Forms.Design;
using ExControls.Designers;

namespace ExControls.Editors;

/// <summary>
/// Provides a UITypeEditor for properties of type OptionsNode that displays a dropdown with OptionsNodes to select from in the property grid.
/// </summary>
public class OptionsNodeEditor : UITypeEditor
{
    private IWindowsFormsEditorService _editorService;

    /// <inheritdoc />
    // Tell the property grid we want to show a dropdown.
    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) => UITypeEditorEditStyle.DropDown;

    /// <inheritdoc />
    public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
    {
        if (provider != null)
            _editorService = (IWindowsFormsEditorService) provider.GetService(typeof(IWindowsFormsEditorService));

        if (_editorService == null)
            return value ?? base.EditValue(context, provider, null);

        // Create the list of other nodes and show it in the dropdown
        var list = CreateNodesList(context);
        list.SelectedIndexChanged += (_, _) => _editorService.CloseDropDown();
        _editorService.DropDownControl(list);

        // Return the selected node when done
        if (list.Text == @"(none)" || list.SelectedItem is null)
            value = null;
        else
            value = list.SelectedItem;

        // If something went wrong return the base value
        return value ?? base.EditValue(context, provider, null);
    }

    // Recursively finds all OptionsNodes in the current ExOptionsView, except the current node, to show in a ListBox
    private static ListBox CreateNodesList(ITypeDescriptorContext context)
    {
        // Get the ExOptionsPanel that hosts the property so we can then get the node and the host ExOptionsView
        var panel = GetPanel(context);
        var view = panel.Owner;

        // Recursively add all nodes to a list
        var nodes = new List<TreeNode>();
        foreach (var n in view.TreeView.Nodes)
            AddNodes((TreeNode)n, nodes);
        var list = new ListBox();

        // Add a (none) option
        list.Items.Add("(none)");

        // Add all nodes except the current node to the ListBox
        foreach (var n in nodes.Where(n => !ReferenceEquals(n, panel.Node)))
            list.Items.Add(n);

        return list;
    }

    private static void AddNodes(TreeNode parentNode, ICollection<TreeNode> nodes)
    {
        nodes.Add(parentNode);
        foreach (var n in parentNode.Nodes)
            AddNodes((TreeNode)n, nodes);
    }

    private static ExOptionsPanel GetPanel(ITypeDescriptorContext context)
    {
        // There are two different scenarios possible here:
        // 1. The property is being edited from a PropertyGrid.
        // In this case, the context.Instance property is the ExOptionsPanel we are editing, easy!
        // 2. The property is being edited from the ActionList / Smart Tag panel
        // In this case, the context.Instance property is the OptionsPanelActionList class instead.
        // Not as easy but not hard either: we just cast it to the OptionsPanelActionList class and use its ControlHost property, done!


        // Simply try option 1 first:
        if (context.Instance is not ExOptionsPanel panel)
        {
            // If that failed, it is being edited from the ActionList / Smart Tag panel:
            var actionList = (ExOptionsPanelDesigner.OptionsPanelActionList)context.Instance;
            panel = actionList.Host;
        }

        return panel;
    }
}