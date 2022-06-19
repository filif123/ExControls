using ExControls.Designers;

namespace ExControls;

/// <summary>
/// Represents a panel that can only accept OptionPanel controls and raises an event when such controls are added or removed.
/// </summary>
[ToolboxItem(false)]
[Designer(typeof(RestrictivePanelDesigner<ExOptionsPanel>))]
public class OptionsPanelContainer : RestrictivePanel<ExOptionsPanel>
{
}