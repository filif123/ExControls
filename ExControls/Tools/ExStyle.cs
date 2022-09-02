using System.Runtime.CompilerServices;

namespace ExControls;

//TODO: WIP --------------

/// <summary>
///     Defines style of control. This class is abstract.
/// </summary>
public record ExStyle : INotifyPropertyChanged
{
    private bool _initializing;
    private Color _backColor;
    private Color _foreColor;
    private Color _borderColor;

    /// <inheritdoc />
    public event PropertyChangedEventHandler PropertyChanged;

    public ExStyle()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public ExStyle(Control control)
    {
        Control = control;
    }

    /// <summary>
    ///     Copy constructor.
    /// </summary>
    /// <param name="original"></param>
    protected ExStyle(ExStyle original)
    {
        Parent = original.Parent;
        BackColor = original._backColor;
        ForeColor = original._foreColor;
        BorderColor = original._borderColor;
    }

    /// <summary>
    ///     Gets the editing control.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Control Control { get; }

    /// <summary>
    /// Gets a parent style.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ExStyle Parent { get; set; }

    /// <summary>
    /// Gets or sets background color of the control.
    /// </summary>
    [DefaultValue(typeof(Color), "Empty")]
    public Color BackColor
    {
        get => GetValue(_backColor, Parent._backColor);
        set => SetField(ref _backColor, value);
    }

    /// <summary>
    /// Gets or sets foreground color of the control.
    /// </summary>
    [DefaultValue(typeof(Color), "Empty")]
    public Color ForeColor
    {
        get => GetValue(_foreColor, Parent._foreColor);
        set => SetField(ref _foreColor, value);
    }

    /// <summary>
    /// Gets or sets border color of the control.
    /// </summary>
    [DefaultValue(typeof(Color), "Empty")]
    public Color BorderColor
    {
        get => GetValue(_borderColor, Parent._borderColor);
        set => SetField(ref _borderColor, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="propertyName"></param>
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        if (!_initializing) 
            Control.Invalidate();
    }

    /// <summary>
    ///     Sets the field value and calls OnPropertyChanged().
    /// </summary>
    /// <param name="field"></param>
    /// <param name="value"></param>
    /// <param name="propertyName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) 
            return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="field"></param>
    /// <param name="parentField"></param>
    /// <returns></returns>
    protected Color GetValue(Color field, Color parentField) => field == Color.Empty && Parent is not null ? parentField : field;

    /// <summary>
    /// 
    /// </summary>
    public void BeginInit()
    {
        _initializing = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public void Endinit()
    {
        _initializing = false;
        Control.Invalidate();
    }
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="TS"></typeparam>
public sealed class ExStyleManager<TS> where TS : ExStyle, new()
{
    private bool _defaultStyle;
    private readonly IStylable<TS> _control;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<EventArgs> DefaultStyleChanged;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<PropertyChangedEventArgs> StyleNormalChanged;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<PropertyChangedEventArgs> StyleDisabledChanged;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<PropertyChangedEventArgs> StyleSelectedChanged;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<PropertyChangedEventArgs> StyleHoverChanged;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<PropertyChangedEventArgs> StyleReadOnlyChanged;

    /// <summary>
    ///     Gets or sets
    /// </summary>
    [DefaultValue(true)]
    public bool DefaultStyle
    {
        get => _defaultStyle;
        set
        {
            if (value == _defaultStyle)
                return;
            _defaultStyle = value;
            OnDefaultStyleChanged();
            _control?.Invalidate();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [ExDescription("Normal style of the control.", true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public TS StyleNormal { get; }

    /// <summary>
    /// 
    /// </summary>
    [ExDescription("Disabled style of the control.", true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public TS StyleDisabled { get; }

    /// <summary>
    /// 
    /// </summary>
    [ExDescription("Selected style of the control.", true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public TS StyleSelected { get; }

    /// <summary>
    /// 
    /// </summary>
    [ExDescription("Hover style of the control.", true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public TS StyleHover { get; }

    /// <summary>
    /// 
    /// </summary>
    [ExDescription("Read only style of the control.", true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public TS StyleReadOnly { get; }

    /// <summary>
    /// 
    /// </summary>
    public ExStyleManager(IStylable<TS> control)
    {
        _control = control;
        var template = control.GetTemplateStyleManager();

        StyleNormal = new TS { Parent = template.StyleNormal };
        StyleDisabled = new TS { Parent = template.StyleDisabled };
        StyleSelected = new TS { Parent = template.StyleSelected };
        StyleHover = new TS { Parent = template.StyleHover };
        StyleReadOnly = new TS { Parent = template.StyleReadOnly };

        StyleNormal.PropertyChanged += (_, args) => StyleNormalChanged?.Invoke(this, args);
        StyleDisabled.PropertyChanged += (_, args) => StyleDisabledChanged?.Invoke(this, args);
        StyleSelected.PropertyChanged += (_, args) => StyleSelectedChanged?.Invoke(this, args);
        StyleHover.PropertyChanged += (_, args) => StyleHoverChanged?.Invoke(this, args);
        StyleReadOnly.PropertyChanged += (_, args) => StyleReadOnlyChanged?.Invoke(this, args);
    }

    internal ExStyleManager()
    {
        StyleNormal = new TS { Parent = null };
        StyleDisabled = new TS { Parent = StyleNormal };
        StyleSelected = new TS { Parent = StyleNormal };
        StyleHover = new TS { Parent = StyleNormal };
        StyleReadOnly = new TS { Parent = StyleNormal };

        StyleNormal.PropertyChanged += (_, args) => StyleNormalChanged?.Invoke(this, args);
        StyleDisabled.PropertyChanged += (_, args) => StyleDisabledChanged?.Invoke(this, args);
        StyleSelected.PropertyChanged += (_, args) => StyleSelectedChanged?.Invoke(this, args);
        StyleHover.PropertyChanged += (_, args) => StyleHoverChanged?.Invoke(this, args);
        StyleReadOnly.PropertyChanged += (_, args) => StyleReadOnlyChanged?.Invoke(this, args);
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnDefaultStyleChanged()
    {
        DefaultStyleChanged?.Invoke(this, EventArgs.Empty);
    }

    private bool ShouldSerializeStyleNormal() => StyleNormal != StyleNormal.Parent;
    private bool ShouldSerializeStyleDisabled() => StyleDisabled != StyleDisabled.Parent;
    private bool ShouldSerializeStyleSelected() => StyleSelected != StyleSelected.Parent;
    private bool ShouldSerializeStyleHover() => StyleHover != StyleHover.Parent;
    private bool ShouldSerializeStyleReadOnly() => StyleReadOnly != StyleReadOnly.Parent;
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IStylable<T> where T : ExStyle, new()
{
    /// <summary>
    /// 
    /// </summary>
    public ExStyleManager<T> StyleManager { get; }

    /// <summary>
    ///     Gets a static style template of this control.
    /// </summary>
    /// <returns></returns>
    public ExStyleManager<T> GetTemplateStyleManager();

    public void Invalidate();
}

/// <summary>
/// 
/// </summary>
public interface ISupportsDefaultStyle
{
    /// <summary>
    ///     Default style of the Control.
    /// </summary>
    public bool DefaultStyle { get; set; }

    /// <summary>Occurs when the <see cref="DefaultStyle" /> property changed.</summary>
    public event EventHandler DefaultStyleChanged;
}