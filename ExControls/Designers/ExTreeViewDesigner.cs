using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing.Design;

#if !NETFRAMEWORK
using Microsoft.DotNet.DesignTools.Designers.Actions;
#endif

namespace ExControls.Designers;

/// <summary>
///     This is the designer for tree view controls.  It inherits 
/// from the base control designer and adds live hit testing
/// capabilites for the tree view control. 
/// </summary>
internal class ExTreeViewDesigner : DesignerControlBase<ExTreeView>
{
    private Win32.TVHITTESTINFO _tvhit;
    private DesignerActionListCollection _actionLists;

    // ReSharper disable InconsistentNaming
    private const int TVM_HITTEST = 0x1100 + 17;
    private const int TVHT_ONITEMBUTTON = 0x0010;
    private const int TVHT_ONITEM = (TVHT_ONITEMICON | TVHT_ONITEMLABEL | TVHT_ONITEMSTATEICON);
    private const int TVHT_ONITEMICON = 0x0002;
    private const int TVHT_ONITEMLABEL = 0x0004;
    private const int TVHT_ONITEMSTATEICON = 0x0040;
    // ReSharper restore InconsistentNaming

    public ExTreeViewDesigner()
    {
        AutoResizeHandles = true;
    }

#if NETFRAMEWORK
    public override DesignerActionListCollection ActionLists => _actionLists ??= new DesignerActionListCollection { new ExTreeViewActionList(ControlHost, this) };
#else
    public override DesignerActionListCollection ActionLists
    {
        get
        {
            DesignerActionListCollection result;
            if ((result = _actionLists) == null)
            {
                result = InterlockedOperations.Initialize(ref _actionLists, new DesignerActionListCollection
                {
                    new ExTreeViewActionList(ControlHost, this)
                });
            }
            return result;
        }
    }
#endif


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
        _tvhit.pt.X = point.X;
        _tvhit.pt.Y = point.Y;
        fixed (Win32.TVHITTESTINFO* ptr = &_tvhit)
        {
            Win32.SendMessage(Control.Handle, TVM_HITTEST, IntPtr.Zero, new IntPtr(ptr));
        }

        return (_tvhit.flags & TVHT_ONITEMBUTTON) == TVHT_ONITEMBUTTON || (_tvhit.flags & TVHT_ONITEM) == TVHT_ONITEM;
    }

    public override void Initialize(IComponent component)
    {
        base.Initialize(component);
        Debug.Assert(ControlHost != null, "TreeView is null in ExTreeViewDesigner");
        if (ControlHost == null) return;

        ControlHost.AfterExpand += TreeViewInvalidate;
        ControlHost.AfterCollapse += TreeViewInvalidate;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && ControlHost != null)
        {
            ControlHost.AfterExpand -= TreeViewInvalidate;
            ControlHost.AfterCollapse -= TreeViewInvalidate;
        }
        base.Dispose(disposing);
    }

    private void TreeViewInvalidate(object sender, TreeViewEventArgs e)
    {
        ControlHost?.Invalidate();
    }

    /// <inheritdoc />
    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);
        if (m.Msg != (int)Win32.WM.LBUTTONDOWN) 
            return;

        var lParam = m.LParam.ToInt32();
        var hitPoint = new Point(lParam & 0xffff, lParam >> 0x10);

        if (Control.FromHandle(m.HWnd) is not ExTreeView tw) 
            return;

        var node = tw.GetNodeAt(hitPoint);
        if (node is not null)
            tw.SelectedNode = node;
        tw.Invalidate();
    }

#if NETCOREAPP3_0_OR_GREATER

    protected override void OnMouseMove(MouseEventArgs e)
    {
        Control.Cursor = Cursors.Default;
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        var nodeAt = ControlHost.GetNodeAt(e.X, e.Y);
        if (nodeAt != null)
        {
            if (nodeAt.IsExpanded)
            {
                nodeAt.Collapse();
                return;
            }
            nodeAt.Expand();
        }
    }

#endif

    private sealed class ExTreeViewActionList : DesignerActionListBase<ExTreeView>
    {
        private readonly ExTreeViewDesigner _designer;

        public ExTreeViewActionList(ExTreeView host, ExTreeViewDesigner designer) : base(host)
        {
            _designer = designer;
        }

        public void InvokeNodesDialog()
        {
#if NETFRAMEWORK
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
                //ignored
            }
#else
            _designer.InvokePropertyEditor("Nodes");
#endif
        }

        public ImageList ImageList
        {
            get => ((TreeView)Component)?.ImageList;
            set => TypeDescriptor.GetProperties(Component)["ImageList"]?.SetValue(Component, value);
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