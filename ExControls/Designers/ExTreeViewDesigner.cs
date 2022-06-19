using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing.Design;

namespace ExControls.Designers;

/// <summary>
///     This is the designer for tree view controls.  It inherits 
/// from the base control designer and adds live hit testing
/// capabilites for the tree view control. 
/// </summary>
internal class ExTreeViewDesigner : DesignerControlBase<ExTreeView>
{
    private Win32.TVHITTESTINFO tvhit;
    private DesignerActionListCollection _actionLists;

    // ReSharper disable InconsistentNaming
    private const int TVM_HITTEST = 0x1100 + 17;
    private const int TVHT_ONITEMBUTTON = 0x0010;
    private const int TVHT_ONITEM = (TVHT_ONITEMICON | TVHT_ONITEMLABEL | TVHT_ONITEMSTATEICON);
    private const int TVHT_ONITEMICON = 0x0002;
    private const int TVHT_ONITEMLABEL = 0x0004;
    private const int TVHT_ONITEMSTATEICON = 0x0040;
    // ReSharper restore InconsistentNaming

    public override DesignerActionListCollection ActionLists => _actionLists ??= new DesignerActionListCollection { new ExTreeViewActionList(Host, this) };

    /// <summary>
    ///     Allows your component to support a design time user interface. A TabStrip
    /// control, for example, has a design time user interface that allows the user 
    /// to click the tabs to change tabs. To implement this, TabStrip returns
    /// true whenever the given point is within its tabs. 
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    protected override unsafe bool GetHitTest(Point point)
    {
        point = Control.PointToClient(point);
        tvhit.pt.X = point.X;
        tvhit.pt.Y = point.Y;
        fixed (Win32.TVHITTESTINFO* ptr = &tvhit)
        {
            Win32.SendMessage(Control.Handle, TVM_HITTEST, IntPtr.Zero, new IntPtr(ptr));
        }

        return (tvhit.flags & TVHT_ONITEMBUTTON) == TVHT_ONITEMBUTTON || (tvhit.flags & TVHT_ONITEM) == TVHT_ONITEM;
    }

    public override void Initialize(IComponent component)
    {
        base.Initialize(component);
        Debug.Assert(Host != null, "TreeView is null in ExTreeViewDesigner");
        if (Host == null) return;

        Host.AfterExpand += TreeViewInvalidate;
        Host.AfterCollapse += TreeViewInvalidate;
        Host.AfterSelect += TreeViewInvalidate;
    }

    private void TreeViewInvalidate(object sender, TreeViewEventArgs e)
    {
        Host?.Invalidate();
    }

    /// <inheritdoc />
    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);
        if (m.Msg == (int)Win32.WM.LBUTTONDOWN)
        {
            var lParam = m.LParam.ToInt32();
            var hitPoint = new Point(lParam & 0xffff, lParam >> 0x10);
            if (Control.FromHandle(m.HWnd) is ExTreeView tw)
            {
                var node = tw.GetNodeAt(hitPoint);
                if (node is not null)
                    tw.SelectedNode = node;
                tw.Invalidate();
            }
        }
    }

    private class ExTreeViewActionList : DesignerActionListBase<ExTreeView>
    {
        private readonly ExTreeViewDesigner _designer;

        public ExTreeViewActionList(ExTreeView host, ExTreeViewDesigner designer) : base(host)
        {
            _designer = designer;
        }

        public void InvokeNodesDialog()
        {
            var property = TypeDescriptor.GetProperties(Component)["Nodes"];
            var editorServiceContext = new ExEditorServiceContext(_designer, property);
            var editor = property.GetEditor(typeof(UITypeEditor)) as UITypeEditor;
            var value = property.GetValue(Component);
            var editedValue = editor?.EditValue(editorServiceContext, editorServiceContext, value);

            if (editedValue == value)
                return;

            try
            {
                property.SetValue(Component, editedValue);
            }
            catch (CheckoutException)
            {
            }
        }

        public ImageList ImageList
        {
            get => ((TreeView)Component).ImageList;
            set => TypeDescriptor.GetProperties(Component)["ImageList"].SetValue(Component, value);
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            var items = new DesignerActionItemCollection
            {
                new DesignerActionMethodItem(this, nameof(InvokeNodesDialog), "Edit Nodes...", "Properties", "Edit Nodes...", true),
                new DesignerActionPropertyItem(nameof(ImageList), "Image list", "Properties", "Image list")
            };
            return items;
        }
    }
}