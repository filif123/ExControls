using ExControls.Designer.Protocol;
using Microsoft.DotNet.DesignTools.Client.TypeRouting;

namespace ExControls.Designer.Client;

/// <summary>
/// Maps editor names from <c>EditorAttribute</c>s of server-side types to the client-side editors.
/// </summary>
[ExportTypeRoutingDefinitionProvider]
internal sealed class TypeRoutingProvider : TypeRoutingDefinitionProvider
{
    public override IEnumerable<TypeRoutingDefinition> GetDefinitions() =>
    [
        new(TypeRoutingKinds.Editor, EditorNames.OptionsPanelCollectionEditor, typeof(OptionsPanelCollectionEditor))
    ];
}
