#if NETFRAMEWORK
using System.ComponentModel.Design;
#else
using Microsoft.DotNet.DesignTools.Designers.Actions;
#endif
namespace ExControls.Designers;

internal class ExGroupBoxDesigner : DesignerParentControlBase<ExGroupBox>
{
    private DesignerActionListCollection _actionLists;
    public override DesignerActionListCollection ActionLists => _actionLists ??= new DesignerActionListCollection { new ExGroupBoxActionList(ControlHost) };

    private sealed class ExGroupBoxActionList : DesignerActionListBase<ExGroupBox>
    {

        public ExGroupBoxActionList(ExGroupBox host) : base(host)
        {
        }

        public string Text
        {
            get => Host.Text;
            set
            {
                SetProperty(nameof(Text), value);
                DesignerActionService.Refresh(Host);
            }
        }

        public bool DefaultStyle
        {
            get => Host.DefaultStyle;
            set
            {
                SetProperty(nameof(DefaultStyle), value);
                DesignerActionService.Refresh(Host);
            }
        }

        public DockStyle Dock
        {
            get => Host.Dock;
            set
            {
                SetProperty(nameof(Dock), value);
                DesignerActionService.Refresh(Host);
            }
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            var items = new DesignerActionItemCollection();
            items.Add(new DesignerActionHeaderItem("Basic"));
            items.Add(new DesignerActionPropertyItem(nameof(Text), "Header text:", "Basic"));
            items.Add(new DesignerActionPropertyItem(nameof(DefaultStyle), "Default style", "Basic"));
            items.Add(new DesignerActionPropertyItem(nameof(Dock), "Dock:", string.Empty));
            return items;
        }
    }
}