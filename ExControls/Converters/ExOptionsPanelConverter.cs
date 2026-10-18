using System.ComponentModel.Design.Serialization;

namespace ExControls.Converters;

/// <summary>
/// Type converter for references to <see cref="ExOptionsPanel" />: the property grid offers the panels of the owning
/// <see cref="ExOptionsView" /> and the designer serializes a panel as <c>new ExOptionsPanel(owner)</c>.
/// </summary>
public class ExOptionsPanelConverter : ReferenceConverter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExOptionsPanelConverter"/> class.
    /// </summary>
    public ExOptionsPanelConverter() : base(typeof(ExOptionsPanel))
    {
    }

    /// <inheritdoc />
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        => ReferenceEquals(destinationType, typeof(InstanceDescriptor)) || base.CanConvertTo(context, destinationType);

    /// <inheritdoc />
    public override object? ConvertTo(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object? value, Type destinationType)
    {
        if (ReferenceEquals(destinationType, typeof(InstanceDescriptor)))
        {
            var type = typeof(ExOptionsPanel);

            // Get the parameterless constructor of the OptionsNode type
            var constructorInfo = type.GetConstructor(new []{ typeof(ExOptionsView) });

            var panel = (ExOptionsPanel) value!;

            // Return a new InstanceDescriptor for it (this creates the "new ExOptionsPanel(ExOptionsView owner)" code in InitializeComponent).
            return new InstanceDescriptor(constructorInfo, new object[]{ panel.Owner! }, false);
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }

    /// <inheritdoc />
    public override bool GetStandardValuesSupported(ITypeDescriptorContext? context) => GetOwnerView(context) is not null;

    /// <inheritdoc />
    public override bool GetStandardValuesExclusive(ITypeDescriptorContext? context) => true;

    /// <inheritdoc />
    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext? context)
    {
        var view = GetOwnerView(context);
        var panels = view is null ? Array.Empty<ExOptionsPanel>() : view.Panels.Cast<ExOptionsPanel>().ToArray();
        return new StandardValuesCollection(panels);
    }
    
    private static ExOptionsView? GetOwnerView(ITypeDescriptorContext? context) => context?.Instance switch
    {
        ExOptionsView view => view,
        ExOptionsPanel panel => panel.Owner,
        { } instance => instance.GetType().GetProperty("Host")?.GetValue(instance) as ExOptionsView,
        _ => null
    };
}