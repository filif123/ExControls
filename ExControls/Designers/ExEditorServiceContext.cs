using System.Collections;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms.Design;

#if NETFRAMEWORK

#else
using Microsoft.DotNet.DesignTools.Designers;
using Microsoft.DotNet.DesignTools.Editors;
#endif

namespace ExControls.Designers;

internal class ExEditorServiceContext : ITypeDescriptorContext, IWindowsFormsEditorService
{
    private readonly ComponentDesigner _designer;
    private IComponentChangeService _componentChangeSvc;
    private readonly PropertyDescriptor _targetProperty;

    internal ExEditorServiceContext(ComponentDesigner designer)
    {
        _designer = designer;
    }

    internal ExEditorServiceContext(ComponentDesigner designer, PropertyDescriptor prop)
    {
        _designer = designer;
        _targetProperty = prop;
        if (prop != null) 
            return;
        prop = TypeDescriptor.GetDefaultProperty(designer.Component);
        if (prop != null && typeof(ICollection).IsAssignableFrom(prop.PropertyType))
        {
            _targetProperty = prop;
        }
    }

    internal ExEditorServiceContext(ComponentDesigner designer, PropertyDescriptor prop, string newVerbText) : this(designer, prop)
    {
        _designer.Verbs.Add(new DesignerVerb(newVerbText, OnEditItems));
    }

    /// <summary>Gets the service object of the specified type.</summary>
    /// <param name="serviceType">An object that specifies the type of service object to get.</param>
    /// <returns>A service object of type <paramref name="serviceType" />.
    /// -or-
    /// <see langword="null" /> if there is no service object of type <paramref name="serviceType" />.</returns>
    object IServiceProvider.GetService(Type serviceType)
    {
        if (serviceType == typeof(ITypeDescriptorContext) || serviceType == typeof(IWindowsFormsEditorService))
            return this;
        return _designer.Component is { Site: { } } ? _designer.Component.Site.GetService(serviceType) : null;
    }

    /// <summary>Raises the <see cref="E:System.ComponentModel.Design.IComponentChangeService.ComponentChanging" /> event.</summary>
    /// <returns>
    /// <see langword="true" /> if this object can be changed; otherwise, <see langword="false" />.</returns>
    bool ITypeDescriptorContext.OnComponentChanging()
    {
        try
        {
            ChangeService.OnComponentChanging(_designer.Component, _targetProperty);
        }
        catch (CheckoutException ex)
        {
            if (ex == CheckoutException.Canceled)
                return false;
            throw;
        }
        return true;
    }

    /// <summary>Raises the <see cref="E:System.ComponentModel.Design.IComponentChangeService.ComponentChanged" /> event.</summary>
    void ITypeDescriptorContext.OnComponentChanged()
    {
        ChangeService.OnComponentChanged(_designer.Component, _targetProperty, null, null);
    }

    /// <summary>Gets the container representing this <see cref="T:System.ComponentModel.TypeDescriptor" /> request.</summary>
    /// <returns>An <see cref="T:System.ComponentModel.IContainer" /> with the set of objects for this <see cref="T:System.ComponentModel.TypeDescriptor" />; otherwise, <see langword="null" /> if there is no container or if the <see cref="T:System.ComponentModel.TypeDescriptor" /> does not use outside objects.</returns>
    public IContainer Container => _designer.Component.Site?.Container;

    /// <summary>Gets the object that is connected with this type descriptor request.</summary>
    /// <returns>The object that invokes the method on the <see cref="T:System.ComponentModel.TypeDescriptor" />; otherwise, <see langword="null" /> if there is no object responsible for the call.</returns>
    public object Instance => _designer.Component;

    /// <summary>Gets the <see cref="T:System.ComponentModel.PropertyDescriptor" /> that is associated with the given context item.</summary>
    /// <returns>The <see cref="T:System.ComponentModel.PropertyDescriptor" /> that describes the given context item; otherwise, <see langword="null" /> if there is no <see cref="T:System.ComponentModel.PropertyDescriptor" /> responsible for the call.</returns>
    PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor => _targetProperty;

    private IComponentChangeService ChangeService =>
        _componentChangeSvc ??= (IComponentChangeService)((IServiceProvider)this).GetService(typeof(IComponentChangeService));

    /// <summary>Closes any previously opened drop down control area.</summary>
    public void CloseDropDown()
    {
        //Not used
    }

    /// <summary>Displays the specified control in a drop down area below a value field of the property grid that provides this service.</summary>
    /// <param name="control">The drop down list <see cref="T:System.Windows.Forms.Control" /> to open.</param>
    public void DropDownControl(Control control)
    {
        //Not used
    }

    /// <summary>Shows the specified <see cref="T:System.Windows.Forms.Form" />.</summary>
    /// <param name="dialog">The <see cref="T:System.Windows.Forms.Form" /> to display.</param>
    /// <returns>A <see cref="T:System.Windows.Forms.DialogResult" /> indicating the result code returned by the <see cref="T:System.Windows.Forms.Form" />.</returns>
    public DialogResult ShowDialog(Form dialog)
    {
        var service = (IUIService)((IServiceProvider)this).GetService(typeof(IUIService));
        return service?.ShowDialog(dialog) ?? dialog.ShowDialog(_designer.Component as IWin32Window);
    }

    public static object EditValue(ComponentDesigner designer, object objectToChange, string propName)
    {
        // Get PropertyDescriptor
        var descriptor = TypeDescriptor.GetProperties(objectToChange)[propName];

        // Create a Context
        var context = new ExEditorServiceContext(designer, descriptor);

        // Get Editor
        var editor = descriptor?.GetEditor(typeof(UITypeEditor)) as UITypeEditor;
        //MessageBox.Show(editor?.ToString());
        if (editor == null)
            return null;

        // Get value to edit
        var value = descriptor.GetValue(objectToChange);
        
        // Edit value
        var newValue = editor.EditValue(context, context, value);
        if (newValue != value)
        {
            try
            {
                descriptor.SetValue(objectToChange, newValue);
            }
            catch (CheckoutException)
            {
                //ignored
            }
        }
        return newValue;

    }

    private void OnEditItems(object sender, EventArgs e)
    {
        var value = _targetProperty.GetValue(_designer.Component);
        if (value == null)
            return;
#if NETFRAMEWORK //TODO
        var collectionEditor = TypeDescriptor.GetEditor(value, typeof(UITypeEditor)) as /*System.Design.*/CollectionEditor;
        collectionEditor?.EditValue(this, this, value);
#endif
    }
}