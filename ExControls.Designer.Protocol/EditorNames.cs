namespace ExControls.Designer.Protocol;

/// <summary>
/// Names of type editors routed between the client (Visual Studio) and the server (DesignToolsServer).
/// </summary>
public static class EditorNames
{
    /// <summary>
    /// Collection editor of <c>ExOptionsView.Panels</c>. Must match the <c>EditorAttribute</c> on that property.
    /// </summary>
    public const string OptionsPanelCollectionEditor = nameof(OptionsPanelCollectionEditor);
}
