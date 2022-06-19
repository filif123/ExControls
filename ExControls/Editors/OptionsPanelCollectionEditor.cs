using System.ComponentModel.Design;

namespace ExControls.Editors;

/// <summary>
/// Provides a collection editor for a ControlCollection of OptionsPanels.
/// </summary>
public class OptionsPanelCollectionEditor : CollectionEditor
{
    /// <inheritdoc />
    public OptionsPanelCollectionEditor(Type type) : base(type)
    {
    }

    /// <inheritdoc />
    protected override Type CreateCollectionItemType()
    {
        // The type of the collection we are editing is Control, 
        // but we want the collection editor to edit OptionsPanels instead.
        return typeof(ExOptionsPanel);
    }

    /// <inheritdoc />
    protected override object CreateInstance(Type itemType)
    {
        if (!ReferenceEquals(itemType, typeof(ExOptionsPanel))) 
            return base.CreateInstance(itemType);

        var designerHost = (IDesignerHost)GetService(typeof(IDesignerHost));
        var view = (ExOptionsView)Context.Instance;
        var panel = ExOptionsPanel.CreatePanel(view, designerHost);

        return panel;
    }
}