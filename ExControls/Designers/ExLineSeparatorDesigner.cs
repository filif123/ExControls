using System.ComponentModel.Design;
using System.Drawing.Drawing2D;

namespace ExControls.Designers;

internal class ExLineSeparatorDesigner : DesignerControlBase<ExLineSeparator>
{
    private DesignerActionListCollection _actionLists;
    public override DesignerActionListCollection ActionLists => _actionLists ??= new DesignerActionListCollection {new ExLineSeparatorActionList(Host, this)};

    private class ExLineSeparatorActionList : DesignerActionListBase<ExLineSeparator>
    {
        private readonly ExLineSeparatorDesigner _designer;

        public ExLineSeparatorActionList(ExLineSeparator host, ExLineSeparatorDesigner designer) : base(host)
        {
            _designer = designer;
        }

        public LineOrientation LineOrientation
        {
            get => Host.LineOrientation;
            set
            {
                SetProperty(nameof(LineOrientation), value);
                DesignerActionService.Refresh(Host);
            }
        }

        public Color LineColor
        {
            get => Host.LineColor;
            set
            {
                SetProperty(nameof(LineColor), value);
                DesignerActionService.Refresh(Host);
            }
        }

        public DashStyle LineStyle
        {
            get => Host.LineStyle;
            set
            {
                SetProperty(nameof(LineStyle), value);
                DesignerActionService.Refresh(Host);
            }
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            var items = new DesignerActionItemCollection();
            items.Add(new DesignerActionHeaderItem("Basic"));
            items.Add(new DesignerActionPropertyItem(nameof(LineOrientation), "Line orientation:", "Basic", "Line orientation."));
            items.Add(new DesignerActionPropertyItem(nameof(LineColor), "Line color:", "Basic", "Line color."));
            items.Add(new DesignerActionPropertyItem(nameof(LineStyle), "Line style:", "Basic", "Line style."));
            return items;
        }
    }
}