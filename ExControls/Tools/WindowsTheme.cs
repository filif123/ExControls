// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable InconsistentNaming

namespace ExControls;

/// <summary>
/// Specify theme for Windows controls.
/// </summary>
public enum WindowsTheme
{
    /// <summary>
    /// No style will be used.
    /// </summary>
    None,

    /// <summary>
    /// Default style will be used.
    /// </summary>
    Default,

    /// <summary>
    /// Style of Windows Explorer in current Windows version.
    /// </summary>
    Explorer,

    /// <summary>
    /// Style of Windows Explorer (dark mode) in current Windows version. Works only in Windows 10.
    /// </summary>
    DarkExplorer,

    /// <summary>
    /// Other style will be used.
    /// </summary>
    Other
}
