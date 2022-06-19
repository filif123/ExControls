using System.ComponentModel.Design;

namespace ExControls.Designers;

internal abstract class DesignerActionListBase<T> : DesignerActionList where T : Control
{
    private DesignerActionUIService _designerActionService;
    private ISelectionService _selectionService;

    protected DesignerActionListBase(T host) : base(host)
    {
        Host = host;
    }

    public T Host { get; }

    protected ISelectionService SelectionService => _selectionService ??= (ISelectionService)GetService(typeof(ISelectionService));

    protected DesignerActionUIService DesignerActionService => _designerActionService ??= (DesignerActionUIService)GetService(typeof(DesignerActionUIService));

    protected void SetProperty(string propertyName, object value)
    {
        // Sets a property value using a PropertyDescriptor
        // This is 'safer' then manually setting the property the regular way from a DesignerActionList
        // because it does not circumvent the design-time services such as undo and property grid refreshing

        var prop = TypeDescriptor.GetProperties(Host)[propertyName];
        prop.SetValue(Host, value);
    }
}