using System.ComponentModel.Design;
#if NETFRAMEWORK
using System.ComponentModel.Design;
#else
using Microsoft.DotNet.DesignTools.Designers.Actions;
#endif

namespace ExControls.Designers;

internal class ExCheckBoxDesigner : DesignerControlBase<ExCheckBox>
{
    private DesignerActionListCollection _actionLists;
    public override DesignerActionListCollection ActionLists => _actionLists ??= new DesignerActionListCollection {new ExCheckBoxActionList(ControlHost)};

    private sealed class ExCheckBoxActionList : DesignerActionListBase<ExCheckBox>
    {

        public ExCheckBoxActionList(ExCheckBox host) : base(host)
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

        public bool Checked
        {
            get => Host.Checked;
            set
            {
                SetProperty(nameof(Checked), value);
                DesignerActionService.Refresh(Host);
            }
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            var items = new DesignerActionItemCollection();
            items.Add(new DesignerActionHeaderItem("Basic"));
            items.Add(new DesignerActionPropertyItem(nameof(Text), "Text:", "Basic"));
            items.Add(new DesignerActionPropertyItem(nameof(DefaultStyle), "Default style", "Basic"));
            items.Add(new DesignerActionPropertyItem(nameof(Checked), "Checked", "Basic"));
            return items;
        }
    }
}