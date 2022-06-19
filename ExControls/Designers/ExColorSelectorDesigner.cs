using System.ComponentModel.Design;

namespace ExControls.Designers;

internal class ExColorSelectorDesigner : DesignerControlBase<ExColorSelector>
{
    private DesignerActionListCollection _actionLists;
    public override DesignerActionListCollection ActionLists => _actionLists ??= new DesignerActionListCollection {new ExColorSelectorActionList(Host, this)};

    private class ExColorSelectorActionList : DesignerActionListBase<ExColorSelector>
    {
        private readonly ExColorSelectorDesigner _designer;

        public ExColorSelectorActionList(ExColorSelector host, ExColorSelectorDesigner designer) : base(host)
        {
            _designer = designer;
        }

        public Color SelectedColor
        {
            get => Host.SelectedColor;
            set
            {
                SetProperty(nameof(SelectedColor), value);
                DesignerActionService.Refresh(Host);
            }
        }

        public Color BorderColor
        {
            get => Host.BorderColor;
            set
            {
                SetProperty(nameof(BorderColor), value);
                DesignerActionService.Refresh(Host);
            }
        }

        public int BorderWidth
        {
            get => Host.BorderWidth;
            set
            {
                SetProperty(nameof(BorderWidth), value);
                DesignerActionService.Refresh(Host);
            }
        }

        public BorderStyle BorderStyle
        {
            get => Host.BorderStyle;
            set
            {
                SetProperty(nameof(BorderStyle), value);
                DesignerActionService.Refresh(Host);
            }
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            var items = new DesignerActionItemCollection();
            items.Add(new DesignerActionHeaderItem("Basic"));
            items.Add(new DesignerActionHeaderItem("Appearance"));
            items.Add(new DesignerActionPropertyItem(nameof(SelectedColor), "Selected color:", "Basic"));
            items.Add(new DesignerActionPropertyItem(nameof(BorderStyle), "Border color:", "Appearance"));
            if (BorderStyle != BorderStyle.None)
            {
                items.Add(new DesignerActionPropertyItem(nameof(BorderColor), "Border color:", "Appearance"));
                items.Add(new DesignerActionPropertyItem(nameof(BorderWidth), "Border width:", "Appearance"));
            }
            
            return items;
        }
    }
}