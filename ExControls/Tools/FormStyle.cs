// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable InconsistentNaming

namespace ExControls;

/// <summary>
/// Defines form's type of style. Works only in Windows 11+.
/// </summary>
public enum FormStyle
{
    /// <summary>
    /// Auto style. Does not work in build lower than 22523.
    /// </summary>
    Auto,

    /// <summary>
    /// Default style. Does not work in build lower than 22000.
    /// </summary>
    Default,

    /// <summary>
    /// Mica style. Does not work in build lower than 22000.
    /// </summary>
    Mica,

    /// <summary>
    /// Acrylic style. Does not work in build lower than 22523.
    /// </summary>
    Acrylic,

    /// <summary>
    /// Tabbed style. Does not work in build lower than 22523. 
    /// </summary>
    Tabbed
}
