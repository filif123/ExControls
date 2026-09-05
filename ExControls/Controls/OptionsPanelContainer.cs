using ExControls.Designers;

namespace ExControls;

/// <summary>
/// Represents a panel that can only accept OptionPanel controls and raises an event when such controls are added or removed.
/// </summary>
[ToolboxItem(false)]
[Designer("ExControls.Designers.RestrictivePanelDesigner`1[[ExControls.ExOptionsPanel, ExControls]], ExControls")]
public class OptionsPanelContainer : RestrictivePanel<ExOptionsPanel>
{
}