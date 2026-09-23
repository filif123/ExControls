using ExControls.Designer.Protocol;
using Microsoft.DotNet.DesignTools.Client.Editors;

namespace ExControls.Designer.Client;

/// <summary>
/// Shows the Visual Studio collection editor dialog for <c>ExOptionsView.Panels</c>.
/// The dialog is driven by the server-side editor with the same name (creating panels as components, etc.).
/// </summary>
internal sealed class OptionsPanelCollectionEditor : CollectionEditor
{
    public OptionsPanelCollectionEditor(Type collectionType) : base(collectionType)
    {
    }

    protected override string Name => EditorNames.OptionsPanelCollectionEditor;
}
