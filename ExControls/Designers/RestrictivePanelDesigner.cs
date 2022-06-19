namespace ExControls.Designers;

internal class RestrictivePanelDesigner<T> : DesignerParentControlBase<RestrictivePanel<T>> where T : Control
{
    public override bool CanParent(Control control) => control is T;
}