using System.ComponentModel.Design;

namespace ExControls.Designers;

internal class ExVerticalTabPageCollectionEditor : CollectionEditor
{
    private ITypeDescriptorContext mContext;

    public ExVerticalTabPageCollectionEditor(Type type) : base(type) { }

    public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
    {
        mContext = context;
        return base.EditValue(context, provider, value);
    }
    protected override object CreateInstance(Type itemType)
    {
        if (itemType == typeof(ExVerticalTabPage))
        {
            var page = (ExVerticalTabPage) base.CreateInstance(itemType);
            if (page != null)
            {
                page.parentContext = mContext; // Each step needs a reference to its parentContext at design time
                return page;
            }
        }

        return base.CreateInstance(itemType);
    }
}