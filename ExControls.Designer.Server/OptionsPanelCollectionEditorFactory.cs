using ExControls.Designer.Protocol;
using ExControls.Editors;
using Microsoft.DotNet.DesignTools.Editors;

namespace ExControls.Designer.Server;

/// <summary>
/// Registers <see cref="OptionsPanelCollectionEditor"/> in DesignToolsServer. The collection editor dialog
/// on the client asks for the editor by its name (<see cref="EditorNames.OptionsPanelCollectionEditor"/>).
/// </summary>
[ExportCollectionEditorFactory(EditorNames.OptionsPanelCollectionEditor)]
internal sealed class OptionsPanelCollectionEditorFactory : CollectionEditorFactory<OptionsPanelCollectionEditor>
{
    protected override OptionsPanelCollectionEditor CreateCollectionEditor(IServiceProvider provider, Type collectionType)
        => new(provider, collectionType);
}
