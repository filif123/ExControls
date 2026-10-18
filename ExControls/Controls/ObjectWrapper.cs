namespace ExControls;

/// <summary>Wrapper of the object shown by <see cref="ExPropertyGrid" /> that exposes only the selected properties
/// (<see cref="PropertyDescriptors" />).</summary>
internal sealed class ObjectWrapper : ICustomTypeDescriptor
{
    /// <summary>Initializes a new instance of the <see cref="ObjectWrapper" /> class for the specified object.</summary>
    /// <param name="obj">A reference to the selected object that will be linked to the parent PropertyGrid.</param>
    internal ObjectWrapper(object? obj)
    {
        SelectedObject = obj;
    }

    /// <summary>Gets or sets a reference to the selected object that will be linked to the parent PropertyGrid.</summary>
    public object? SelectedObject { get; set; }

    /// <summary>
    /// Gets or sets the properties to show in the parent PropertyGrid.
    /// By default, PropertyDescriptors contain all the properties of the object.
    /// </summary>
    public List<PropertyDescriptor> PropertyDescriptors { get; set; } = new();

    #region ICustomTypeDescriptor Members

    public PropertyDescriptorCollection GetProperties(Attribute[]? attributes)
    {
        return GetProperties();
    }

    public PropertyDescriptorCollection GetProperties()
    {
        return new PropertyDescriptorCollection(PropertyDescriptors.ToArray(), true);
    }

    /// <summary>Returns the attributes of <see cref="SelectedObject" />.</summary>
    /// <returns>attributes of the selected object</returns>
    public AttributeCollection GetAttributes()
    {
        return TypeDescriptor.GetAttributes(SelectedObject!, true);
    }
    /// <summary>Returns the class name of <see cref="SelectedObject" />.</summary>
    /// <returns>class name of the selected object</returns>
    public string? GetClassName()
    {
        return TypeDescriptor.GetClassName(SelectedObject!, true);
    }
    /// <summary>Returns the component name of <see cref="SelectedObject" />.</summary>
    /// <returns>name of the selected object; <see langword="null" /> if it has none</returns>
    public string? GetComponentName()
    {
        return TypeDescriptor.GetComponentName(SelectedObject!, true);
    }

    /// <summary>Returns the type converter of <see cref="SelectedObject" />.</summary>
    /// <returns>type converter of the selected object</returns>
    public TypeConverter GetConverter()
    {
        return TypeDescriptor.GetConverter(SelectedObject!, true);
    }

    /// <summary>Returns the default event of <see cref="SelectedObject" />.</summary>
    /// <returns>default event of the selected object; <see langword="null" /> if it has none</returns>
    public EventDescriptor? GetDefaultEvent()
    {
        return TypeDescriptor.GetDefaultEvent(SelectedObject!, true);
    }

    /// <summary>Returns the default property of <see cref="SelectedObject" />.</summary>
    /// <returns>default property of the selected object; <see langword="null" /> if it has none</returns>
    public PropertyDescriptor? GetDefaultProperty()
    {
        return TypeDescriptor.GetDefaultProperty(SelectedObject!, true);
    }

    /// <summary>Returns an editor of the specified type for the wrapper.</summary>
    /// <param name="editorBaseType">base type of the requested editor</param>
    /// <returns>the editor; <see langword="null" /> if there is none</returns>
    public object? GetEditor(Type editorBaseType)
    {
        return TypeDescriptor.GetEditor(this, editorBaseType, true);
    }

    public EventDescriptorCollection GetEvents(Attribute[]? attributes)
    {
        return TypeDescriptor.GetEvents(SelectedObject!, attributes, true);
    }

    public EventDescriptorCollection GetEvents()
    {
        return TypeDescriptor.GetEvents(SelectedObject!, true);
    }

    public object? GetPropertyOwner(PropertyDescriptor? pd)
    {
        return SelectedObject;
    }

    #endregion
}