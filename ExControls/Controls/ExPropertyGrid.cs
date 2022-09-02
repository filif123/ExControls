// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable ConvertToAutoPropertyWhenPossible
// ReSharper disable ClassNeverInstantiated.Global

using ExControls.Designers;
using ExControls.Properties;

namespace ExControls;

/// <summary>
///     Extended PropertyGrid.
/// Part of this code is from: https://www.codeproject.com/Articles/13342/Filtering-properties-in-a-PropertyGrid
/// </summary>
[Designer(typeof(ExPropertyGridDesigner))]
[ToolboxBitmap(typeof(PropertyGrid),"PropertyGrid.bmp")]
public class ExPropertyGrid : PropertyGrid, ISearchable
{
    private readonly ToolStrip _innerToolStrip;

    private ToolStripButton _buttonCategorized;
    private ToolStripButton _buttonAphabetical;
    private ToolStripSeparator _separator;
    private ToolStripButton _buttonPropertyPages;

    //Contain a reference to the collection of properties to show in the parent PropertyGrid.
    //By default, _propertyDescriptors contain all the properties of the object.
    private readonly List<PropertyDescriptor> _propertyDescriptors = new();
    
    //Contain a reference to the array of properties to display in the PropertyGrid.
    private AttributeCollection _hiddenAttributes, _browsableAttributes;
    
    //Contain references to the arrays of properties or categories to hide.
    private string[] _browsableProperties, _hiddenProperties;

    //Contain a reference to the wrapper that contains the object to be displayed into the PropertyGrid.
    private ObjectWrapper _wrapper;

    private bool _firstHideAllProperties;

    /// <summary>
    /// 
    /// </summary>
    public ExPropertyGrid()
    {
        _innerToolStrip = Controls.OfType<ToolStrip>().FirstOrDefault();
        if (_innerToolStrip is not null) return;
        var type = Type.GetType("System.Windows.Forms.PropertyGridToolStrip");
        _innerToolStrip = Controls.OfType<Control>().FirstOrDefault(c => c.GetType() == type) as ToolStrip;
    }

    /// <summary>
    ///     Gets internal ToolStrip control.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [ExCategory(CategoryType.Layout)]
    public ToolStrip InnerToolStrip => _innerToolStrip;

    /// <summary>
    ///     Gets an internal Categorized button.
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Layout)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ToolStripButton ButtonCategorized => _buttonCategorized ??= InnerToolStrip.Items.Cast<ToolStripItem>().FirstOrDefault(i => i.Text == @"Categorized") 
        as ToolStripButton;

    /// <summary>
    ///     Gets an internal Alphabetical button.
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Layout)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ToolStripButton ButtonAlphabetical => _buttonAphabetical ??= InnerToolStrip.Items.Cast<ToolStripItem>().FirstOrDefault(i => i.Text == @"Alphabetical") 
        as ToolStripButton;

    /// <summary>
    ///     Gets an internal Separator.
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Layout)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ToolStripSeparator Separator => _separator ??= InnerToolStrip.Items.Cast<ToolStripItem>().FirstOrDefault(i => typeof(ToolStripSeparator) == i.GetType()) 
        as ToolStripSeparator;

    /// <summary>
    ///     Gets an internal Property Pages button.
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Layout)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ToolStripButton ButtonPropertyPages => _buttonPropertyPages ??= InnerToolStrip.Items.Cast<ToolStripItem>().FirstOrDefault(i => i.Text == @"Property Pages") 
        as ToolStripButton;

    /// <inheritdoc />
    protected override Bitmap SortByCategoryImage => Resources.PBCategory;

    /// <inheritdoc />
    protected override Bitmap SortByPropertyImage => Resources.PBAlpha;

    /// <inheritdoc />
    protected override Bitmap ShowPropertyPageImage => Resources.PBPPage;

    /// <inheritdoc />
    public bool Search(string text)
    {
        var props = SelectedTab.GetProperties(_wrapper.SelectedObject);
        if (props is null)
            return false;

        if (string.IsNullOrWhiteSpace(text))
        {
            FirstHideAllProperties = false;
            BrowsableProperties = null;
            Refresh();
            return true;
        }

        var browsable = props.Cast<PropertyDescriptor>()
                .Where(prop => prop.DisplayName.Contains(text, StringComparison.CurrentCultureIgnoreCase))
                .Select(prop => prop.Name)
                .ToList();

        FirstHideAllProperties = true;
        BrowsableProperties = browsable.ToArray();
        Refresh();
        return browsable.Count > 0;
    }

    /// <summary>
    ///     Get or set the categories to show.
    /// </summary>
    public new AttributeCollection BrowsableAttributes
    {
        get => _browsableAttributes;
        set
        {
            if (_browsableAttributes == value) return;
            _hiddenAttributes = null;
            _browsableAttributes = value;
            RefreshProperties();
        }
    }

    /// <summary>
    ///     Get or set the categories to hide.
    /// </summary>
    public AttributeCollection HiddenAttributes
    {
        get => _hiddenAttributes;
        set
        {
            if (value == _hiddenAttributes) return;
            _hiddenAttributes = value;
            _browsableAttributes = null;
            RefreshProperties();
        }
    }
    /// <summary>
    ///     Get or set the properties to show.
    /// </summary>
    /// <exception cref="ArgumentException">if one or several properties don't exist.</exception>
    public string[] BrowsableProperties
    {
        get => _browsableProperties;
        set
        {
            if (value == _browsableProperties) return;
            _browsableProperties = value;
            RefreshProperties();
            OnBrowsablePropertiesChanged();
        }
    }

    /// <summary>
    ///     Get or set the properties to hide.
    /// </summary>
    public string[] HiddenProperties
    {
        get => _hiddenProperties;
        set
        {
            if (value == _hiddenProperties) return;
            _hiddenProperties = value;
            RefreshProperties();
            OnBrowsablePropertiesChanged();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public bool FirstHideAllProperties
    {
        get => _firstHideAllProperties;
        set
        {
            if (value == _firstHideAllProperties) return;
            _firstHideAllProperties = value;
            RefreshProperties();
        }
    }

    /// <summary>
    ///     Overwrite the PropertyGrid.SelectedObject property.
    /// </summary>
    /// <remarks>
    ///     The object passed to the base PropertyGrid is the wrapper.
    /// </remarks>
    public new object SelectedObject
    {
        get => _wrapper != null ? ((ObjectWrapper)base.SelectedObject).SelectedObject : null;
        set
        {
            // Set the new object to the wrapper and create one if necessary.
            if (_wrapper == null)
            {
                _wrapper = new ObjectWrapper(value);
                RefreshProperties();
            }
            else if (_wrapper.SelectedObject != value)
            {
                var needRefresh = value.GetType() != _wrapper.SelectedObject.GetType();
                _wrapper.SelectedObject = value;
                if (needRefresh) 
                    RefreshProperties();
            }
            // Set the list of properties to the wrapper.
            _wrapper.PropertyDescriptors = _propertyDescriptors;
            // Link the wrapper to the parent PropertyGrid.
            base.SelectedObject = _wrapper;
        }
    }

    /// <summary>Called when the browsable properties have changed.</summary>
    protected virtual void OnBrowsablePropertiesChanged()
    {
    }

    /// <summary>
    ///     Build the list of the properties to be displayed in the PropertyGrid, following the filters defined the Browsable and Hidden properties.
    /// </summary>
    private void RefreshProperties()
    {
        if (_wrapper == null) 
            return;

        // Clear the list of properties to be displayed.
        _propertyDescriptors.Clear();

        // Check whether the list is filtered 
        if (_browsableAttributes is { Count: > 0 })
        {
            // Add to the list the attributes that need to be displayed.
            foreach (Attribute attribute in _browsableAttributes) 
                ShowAttribute(attribute);
        }
        else
        {
            if (!FirstHideAllProperties)
            {
                // Fill the collection with all the properties.
                _propertyDescriptors.AddRange(TypeDescriptor.GetProperties(_wrapper.SelectedObject).Cast<PropertyDescriptor>());
            }
            
            // Remove from the list the attributes that mustn't be displayed.
            if (_hiddenAttributes != null) 
                foreach (Attribute attribute in _hiddenAttributes) 
                    HideAttribute(attribute);
        }

        // Get all the properties of the SelectedObject
        var allProperties = TypeDescriptor.GetProperties(_wrapper.SelectedObject);

        // Display if necessary, some properties
        if (_browsableProperties is { Length: > 0 })
        {
            foreach (var propertyName in _browsableProperties)
            {
                try
                {
                    ShowProperty(allProperties[propertyName]);
                }
                catch (Exception)
                {
                    throw new ArgumentException(@"Property not found", propertyName);
                }
            }
        }

        // Hide if necessary, some properties
        if (_hiddenProperties is { Length: > 0 })
        {
            // Remove from the list the properties that mustn't be displayed.
            foreach (var propertyName in _hiddenProperties)
            {
                try
                {
                    // Remove from the list the property
                    HideProperty(allProperties[propertyName]);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message);
                }
            }
        }
    }
    
    /// <summary>
    ///     Allows to hide a set of properties to the parent PropertyGrid.
    /// </summary>
    /// <param name="attribute">A set of attributes that filter the original collection of properties.</param>
    /// <remarks>For better performance, include the BrowsableAttribute with true value.</remarks>
    private void HideAttribute(Attribute attribute)
    {
        var filteredOriginalPropertyDescriptors = TypeDescriptor.GetProperties(_wrapper.SelectedObject, new[] { attribute });
        if (filteredOriginalPropertyDescriptors == null || filteredOriginalPropertyDescriptors.Count == 0) 
            throw new ArgumentException(@"Attribute not found", attribute.ToString());

        foreach (PropertyDescriptor propertydescriptor in filteredOriginalPropertyDescriptors) 
            HideProperty(propertydescriptor);
    }
    /// <summary>
    ///     Add all the properties that match an attribute to the list of properties to be displayed in the PropertyGrid.
    /// </summary>
    /// <param name="attribute">The attribute to be added.</param>
    private void ShowAttribute(Attribute attribute)
    {
        var filteredOriginalPropertyDescriptors = TypeDescriptor.GetProperties(_wrapper.SelectedObject, new[] { attribute });
        if (filteredOriginalPropertyDescriptors == null || filteredOriginalPropertyDescriptors.Count == 0) 
            throw new ArgumentException(@"Attribute not found", attribute.ToString());

        foreach (PropertyDescriptor propertydescriptor in filteredOriginalPropertyDescriptors) 
            ShowProperty(propertydescriptor);
    }
    /// <summary>
    ///     Add a property to the list of properties to be displayed in the PropertyGrid.
    /// </summary>
    /// <param name="property">The property to be added.</param>
    private void ShowProperty(PropertyDescriptor property)
    {
        if (!_propertyDescriptors.Contains(property)) 
            _propertyDescriptors.Add(property);
    }
    /// <summary>
    ///     Allows to hide a property to the parent PropertyGrid.
    /// </summary>
    /// <param name="property">The name of the property to be hidden.</param>
    private void HideProperty(PropertyDescriptor property)
    {
        _propertyDescriptors.Remove(property);
    }
}