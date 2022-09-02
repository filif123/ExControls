using System.ComponentModel.Design.Serialization;

namespace ExControls.Converters;

/// <summary>
/// 
/// </summary>
public class ExOptionsPanelConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) 
        => ReferenceEquals(destinationType, typeof(InstanceDescriptor)) || base.CanConvertTo(context, destinationType);

    /// <inheritdoc />
    public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
    {
        if (ReferenceEquals(destinationType, typeof(InstanceDescriptor)))
        {
            var type = typeof(ExOptionsPanel);

            // Get the parameterless constructor of the OptionsNode type
            var constructorInfo = type.GetConstructor(new []{ typeof(ExOptionsView) });

            var panel = (ExOptionsPanel) value;

            // Return a new InstanceDescriptor for it (this creates the "new ExOptionsPanel(ExOptionsView owner)" code in InitializeComponent).
            return new InstanceDescriptor(constructorInfo, new object[]{ panel.Owner }, false);
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}