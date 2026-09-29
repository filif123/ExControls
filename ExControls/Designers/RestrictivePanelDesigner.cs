namespace ExControls.Designers;

internal sealed class RestrictivePanelDesigner<T> : DesignerParentControlBase<RestrictivePanel<T>> where T : Control
{
    public override bool CanParent(Control control) => control is T;
}