using System.Collections;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using ExControls.Collections;

namespace ExControls.Designers;

/// <summary>Provides a type converter to convert <see cref="ExVerticalTabPageConverter" /> objects to and from various other representations.</summary>
public class ExVerticalTabPageConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {
        return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
    }

    /// <inheritdoc />
    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
        if (destinationType == null) 
            throw new ArgumentNullException(nameof(destinationType));

        if (destinationType != typeof(InstanceDescriptor))
            return base.ConvertTo(context, culture, value, destinationType);

        if (value is ExVerticalTabPage page)
        {
            MemberInfo constructor;
            object[] arguments;
            if (page.Node.ImageIndex == -1 || page.Node.SelectedImageIndex == -1)
            {
                if (page.Children.Count == 0)
                {
                    constructor = typeof(ExVerticalTabPage).GetConstructor(new[] { typeof(string) });
                    arguments = new object[] { page.Text };
                }
                else
                {
                    constructor = typeof(ExVerticalTabPage).GetConstructor(new[] { typeof(string), typeof(ExVerticalTabPage[]) });
                    var array = new ExVerticalTabPage[page.Children.Count];
                    page.Children.CopyTo(array, 0);
                    arguments = new object[] { page.Text, array };
                }
            }
            else if (page.Children.Count == 0)
            {
                constructor = typeof(ExVerticalTabPage).GetConstructor(new[] { typeof(string), typeof(int), typeof(int) });
                arguments = new object[] { page.Text, page.Node.ImageIndex, page.Node.SelectedImageIndex };
            }
            else
            {
                constructor = typeof(ExVerticalTabPage).GetConstructor(new[] { typeof(string), typeof(int), typeof(int), typeof(ExVerticalTabPage[]) });
                var array = new ExVerticalTabPage[page.Children.Count];
                page.Children.CopyTo(array, 0);
                arguments = new object[] { page.Text, page.Node.ImageIndex, page.Node.SelectedImageIndex, array };
            }

            return constructor != null ? 
                new InstanceDescriptor(constructor, arguments, false) : 
                base.ConvertTo(context, culture, value, destinationType);
        }

        /*if (value is VerticalTabPagesCollection)
        {
            Type valueType = value.GetType();
            ConstructorInfo ci = valueType.GetConstructor(Type.EmptyTypes);
            return new InstanceDescriptor(ci, null, false);
        }*/

        return base.ConvertTo(context, culture, value, destinationType);
    }
}