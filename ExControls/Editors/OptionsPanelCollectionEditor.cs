#if !NETFRAMEWORK
using Microsoft.DotNet.DesignTools.Editors;
#endif
using System.ComponentModel.Design;

namespace ExControls.Editors;

/// <summary>
/// Provides a collection editor for a ControlCollection of OptionsPanels.
/// </summary>
public class OptionsPanelCollectionEditor : CollectionEditor
{
#if NETFRAMEWORK
    /// <inheritdoc />
    public OptionsPanelCollectionEditor(Type type) : base(type)
    {
    }
#else
    /// <inheritdoc />
    public OptionsPanelCollectionEditor(IServiceProvider provider, Type type) : base(provider, type)
    {
    }
#endif

    /// <inheritdoc />
    protected override Type CreateCollectionItemType()
    {
        // The type of the collection we are editing is Control, 
        // but we want the collection editor to edit OptionsPanels instead.
        return typeof(ExOptionsPanel);
    }

    /// <inheritdoc />
    protected override object CreateInstance(Type itemType)
    {
        if (!ReferenceEquals(itemType, typeof(ExOptionsPanel))) 
            return base.CreateInstance(itemType);

        var designerHost = (IDesignerHost)GetService(typeof(IDesignerHost));
        var view = (ExOptionsView)Context.Instance;
        var panel = ExOptionsPanel.CreatePanel(view, designerHost);

        return panel;
    }

#if !NETFRAMEWORK
    private ExPanelCollectionEditorViewModel _model;

    protected override CollectionEditorViewModel BeginEditValue(ITypeDescriptorContext context, object value)
    {
        return _model ??= new ExPanelCollectionEditorViewModel(this);
    }

    protected override object EndEditValue(bool commitChange)
    {
        return _model.EditValue;
    }

    private sealed class ExPanelCollectionEditorViewModel : CollectionEditorViewModel
    {
        public ExPanelCollectionEditorViewModel(CollectionEditor editor) : base(editor)
        {
        }

        protected override void OnEditValueChanged()
        {
            throw new NotImplementedException();
        }
    }

#endif
}