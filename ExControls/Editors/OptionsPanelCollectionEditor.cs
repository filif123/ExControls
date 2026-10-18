#if !NETFRAMEWORK
using Microsoft.DotNet.DesignTools.Editors;
#endif
using System.ComponentModel.Design;

namespace ExControls.Editors;

/// <summary>
/// Provides a collection editor for a ControlCollection of OptionsPanels.
/// </summary>
/// <remarks>
/// On .NET this is the server-side part of the editor; it is registered in DesignToolsServer
/// and shown in Visual Studio by the ExControls.Designer package (see ExControls.Designer.targets).
/// </remarks>
public class OptionsPanelCollectionEditor : CollectionEditor
{
#if NETFRAMEWORK
    /// <summary>
    /// Initializes a new instance of the <see cref="OptionsPanelCollectionEditor" /> class.
    /// </summary>
    /// <param name="type">type of the edited collection</param>
    public OptionsPanelCollectionEditor(Type type) : base(type)
    {
    }
#else
    /// <summary>
    /// Initializes a new instance of the <see cref="OptionsPanelCollectionEditor" /> class.
    /// </summary>
    /// <param name="provider">service provider of the design tools server</param>
    /// <param name="type">type of the edited collection</param>
    public OptionsPanelCollectionEditor(IServiceProvider provider, Type type) : base(provider, type)
    {
    }
#endif

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

#if NETFRAMEWORK
        var designerHost = (IDesignerHost)GetService(typeof(IDesignerHost))!;
#else
        var designerHost = GetService<IDesignerHost>()!;
#endif
        var view = (ExOptionsView)Context!.Instance!;
        var panel = ExOptionsPanel.CreatePanel(view, designerHost);

        return panel;
    }
}