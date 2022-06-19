using System.ComponentModel.Design;

namespace ExControls.Designers;

internal class ExCheckBoxDesigner : DesignerControlBase<ExCheckBox>
{
    private DesignerActionListCollection _actionLists;
    public override DesignerActionListCollection ActionLists => _actionLists ??= new DesignerActionListCollection {new ExCheckBoxActionList(Host, this)};

    private class ExCheckBoxActionList : DesignerActionListBase<ExCheckBox>
    {
        private readonly ExCheckBoxDesigner _designer;

        public ExCheckBoxActionList(ExCheckBox host, ExCheckBoxDesigner designer) : base(host)
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