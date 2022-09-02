using System.ComponentModel.Design;
#if NETFRAMEWORK

#else
using Microsoft.DotNet.DesignTools.Designers.Actions;
#endif

namespace ExControls.Designers;

internal class ExColorSelectorDesigner : DesignerControlBase<ExColorSelector>
{
    private DesignerActionListCollection _actionLists;
    public override DesignerActionListCollection ActionLists => _actionLists ??= new DesignerActionListCollection {new ExColorSelectorActionList(ControlHost)};

    private sealed class ExColorSelectorActionList : DesignerActionListBase<ExColorSelector>
    {
        public ExColorSelectorActionList(ExColorSelector host) : base(host)
        {
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
            items.Add(new DesignerActionPropertyItem(nameof(BorderStyle), "Border style:", "Appearance"));
            if (BorderStyle != BorderStyle.None)
            {
                items.Add(new DesignerActionPropertyItem(nameof(BorderColor), "Border color:", "Appearance"));
                items.Add(new DesignerActionPropertyItem(nameof(BorderWidth), "Border width:", "Appearance"));
            }
            
            return items;
        }
    }
}