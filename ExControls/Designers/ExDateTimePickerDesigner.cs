#if NETFRAMEWORK
using System.Windows.Forms.Design;
#else
using Microsoft.DotNet.DesignTools.Designers;
#endif

namespace ExControls.Designers;

/// <summary>
///     Designer of <see cref="ExDateTimePicker" />: the height is fixed (computed from the font), so only the
///     left/right grab handles are shown - same as the designer of the native DateTimePicker.
/// </summary>
internal sealed class ExDateTimePickerDesigner : DesignerControlBase<ExDateTimePicker>
{
    /// <inheritdoc />
    public override SelectionRules SelectionRules => SelectionRules.LeftSizeable | SelectionRules.RightSizeable | SelectionRules.Moveable;
}
