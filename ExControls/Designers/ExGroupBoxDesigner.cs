using System.ComponentModel.Design;
using System.Drawing.Design;

namespace ExControls.Designers;

internal class ExGroupBoxDesigner : DesignerParentControlBase<ExGroupBox>
{
    private DesignerActionListCollection _actionLists;
    public override DesignerActionListCollection ActionLists => _actionLists ??= new DesignerActionListCollection {new ExGroupBoxActionList(Host, this)};

    private class ExGroupBoxActionList : DesignerActionListBase<ExGroupBox>
    {
        private readonly ExGroupBoxDesigner _designer;

        public ExGroupBoxActionList(ExGroupBox host, ExGroupBoxDesigner designer) : base(host)
        {
            _designer = designer;
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