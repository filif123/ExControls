using System.ComponentModel.Design.Serialization;

namespace ExControls.Converters;

/// <summary>
/// Provides a type converter to convert OptionsNodes to InstanceDescriptors, 
/// used to allow the designer to create new OptionsNode objects, so it can serialize OptionsNodes to the designer file.
/// </summary>
public class OptionsNodeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) 
        => ReferenceEquals(destinationType, typeof(InstanceDescriptor)) || base.CanConvertTo(context, destinationType);

    /// <inheritdoc />
    public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
    {
        if (ReferenceEquals(destinationType, typeof(InstanceDescriptor)))
        {
            var type = typeof(OptionsNode);

            // Get the parameterless constructor of the OptionsNode type
            var constructorInfo = type.GetConstructor(Type.EmptyTypes);

            // Return a new InstanceDescriptor for it (this creates the "new OptionsNode()" code in InitializeComponent).
            return new InstanceDescriptor(constructorInfo, null, false);
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}