using System.Collections;
#if NETFRAMEWORK
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
#else
using Microsoft.DotNet.DesignTools.Designers;
using Microsoft.DotNet.DesignTools.Designers.Actions;
#endif

namespace ExControls.Designers;

/// <summary>
/// Provides a ParentControlDesigner for the ExOptionsView to enhance design-time experience.
/// </summary>
internal class ExOptionsViewDesigner : DesignerParentControlBase<ExOptionsView>
{
    private readonly string[] _invisibleProperties;
    private DesignerActionListCollection _actionLists;
    private bool _newComponentInit;

    public ExOptionsViewDesigner()
    {
        _invisibleProperties = new[] { "PanelContainer",  "SplitContainer", nameof(SelectedPanel) };
        AutoResizeHandles = true;
    }

    public override DesignerActionListCollection ActionLists => _actionLists ??= new DesignerActionListCollection
    {
        new OptionsViewActionList(ControlHost, this)
    };

    private ExOptionsPanel SelectedPanel { get; set; }

    /*// Handle mouseclicks on the TreeView
    protected override void OnHostInitialized()
    {
        ControlHost.TreeView.MouseDown += OnTreeViewClicked;
#if !NETFRAMEWORK
        ControlHost.TreeView.AfterExpand += TreeViewInvalidate;
        ControlHost.TreeView.AfterCollapse += TreeViewInvalidate;
        ControlHost.TreeView.AfterSelect += TreeViewInvalidate;
#endif
    }*/

    /// <inheritdoc />
    public override void Initialize(IComponent component)
    {
        base.Initialize(component);
        EnableDesignMode(ControlHost.ToolStripMenu, "ToolStripMenu");
    }

    /// <inheritdoc />
    public override void InitializeNewComponent(IDictionary defaultValues)
    {
        base.InitializeNewComponent(defaultValues);

        _newComponentInit = true;
        OnAddPanel();
        OnAddPanel();
        _newComponentInit = false;
    }

    // The ExOptionsView can technically parent controls because this designer is a ParentControlDesigner
    // We don't want the user dragging buttons and the like on it, so disable this behavior.
    public override bool CanParent(Control control) => false;

    public override bool CanParent(ControlDesigner controlDesigner) => false;

    // When the treeview is clicked, select the host ExOptionsView (otherwise there would be no way to select it)
    private void OnTreeViewClicked(object sender, MouseEventArgs e)
    {
        MessageBox.Show("Test");
        SelectHost();
    }

    private void TreeViewInvalidate(object sender, TreeViewEventArgs e) => ControlHost.TreeView?.Invalidate();

    protected override void PreFilterProperties(IDictionary properties)
    {
        base.PreFilterProperties(properties);

        // Hide some properties
        foreach (var prop in _invisibleProperties)
            properties.Remove(prop);

        var array = new[] { nameof(SelectedPanel) };
        foreach (var t in array)
        {
            var propertyDescriptor = (PropertyDescriptor)properties[t];
            if (propertyDescriptor != null)
                properties[t] = TypeDescriptor.CreateProperty(typeof(ExOptionsViewDesigner), propertyDescriptor, Array.Empty<Attribute>());
        }
    }

/*#if !NETFRAMEWORK
    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (ControlHost.TreeView.ClientRectangle.Contains(e.Location))
        {
            var info = ControlHost.TreeView.HitTest(ControlHost.TreeView.PointToClient(e.Location));
            ControlHost.TreeView.SelectedNode = info.Node;
        }
    }
#endif*/

    protected override bool GetHitTest(Point screenCoordinates)
    {
        var point = ControlHost.PointToClient(screenCoordinates);
        
        // Allow treenode selection / expansion
        if (ControlHost.TreeView.ClientRectangle.Contains(point))
            return true;

        // Allow splitter moving
        if (ControlHost.SplitContainer.SplitterRectangle.Contains(point))
            return true;

        //Allow page scrolling
        if (ControlHost.SelectedPanel != null && ControlHost.SelectedPanel.ClientRectangle.Contains(point))
            return true;

        return base.GetHitTest(point);
    }

    private void OnAddPanel()
    {
        // Store old items for undo
        var oldPanels = ControlHost.Panels;
        if (!_newComponentInit) RaiseComponentChanging(TypeDescriptor.GetProperties(ControlHost)["Panels"]);

        var panel = ExOptionsPanel.CreatePanel(ControlHost, DesignerHost);
        ControlHost.Panels.Add((Control)panel);

        if (!_newComponentInit) RaiseComponentChanged(TypeDescriptor.GetProperties(ControlHost)["Panels"], oldPanels, ControlHost.Panels);
        ControlHost.SelectedPanel = (ExOptionsPanel)panel;
        SelectHost();
    }

    private void OnRemovePanel()
    {
        // Store old items for undo
        var oldPanels = ControlHost.Panels;
        var panel = ControlHost.SelectedPanel;
        if (panel is null)
            return;

        RaiseComponentChanging(TypeDescriptor.GetProperties(ControlHost)["Panels"]);

        ControlHost.Panels.Remove(panel);
        DesignerHost.DestroyComponent(panel);

        RaiseComponentChanged(TypeDescriptor.GetProperties(ControlHost)["Panels"], oldPanels, ControlHost.Panels);
        SelectHost();
    }

    private sealed class OptionsViewActionList : DesignerActionListBase<ExOptionsView>
    {
        public OptionsViewActionList(ExOptionsView host, ExOptionsViewDesigner designer) : base(host)
        {
            Designer = designer;
        }

        private ExOptionsViewDesigner Designer { get; }

        private void AddPanel()
        {
            Designer.OnAddPanel();
            DesignerActionService.Refresh(Host);
        }

        private void RemovePanel()
        {
            Designer.OnRemovePanel();
            DesignerActionService.Refresh(Host);
        }

        private void EditPanels() => ExEditorServiceContext.EditValue(Designer, Component, "Panels");

        public ExOptionsPanel SelectedPanel
        {
            get => Host.SelectedPanel;
            set
            {
                SetProperty(nameof(SelectedPanel), value);
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
            var items = new DesignerActionItemCollection
            {
                new DesignerActionHeaderItem("Panels"),
                new DesignerActionMethodItem(this, nameof(AddPanel), "Add Panel", "Panels",
                    "Adds a new ExOptionsPanel to this ExOptionsView.", true),
                new DesignerActionMethodItem(this, nameof(EditPanels), "Edit Panels...", "Panels", 
                    "Edits a collection of panels in this ExOptionsView.", true),
                new DesignerActionPropertyItem(nameof(Dock), "Dock:", "", 
                    "Docks this control to a side."),
                new DesignerActionPropertyItem(nameof(SelectedPanel), "Selected Panel:", "Panels", 
                    "Gets or sets the selected ExOptionsPanel.")
            };

            if (Host.Panels.Count > 1)
                items.Add(new DesignerActionMethodItem(this, nameof(RemovePanel), "Remove Panel", "Panels", 
                    "Removes the selected ExOptionsPanel from this ExOptionsView.", true));
            
            return items;
        }
    }

    internal static partial class ComCtl32
    {
        public struct TVHITTESTINFO
        {
            public Point pt;
            public TVHT flags;
            public IntPtr hItem;
        }

        [Flags]
        public enum TVHT : uint
        {
            NOWHERE = 0x0001,
            ONITEMICON = 0x0002,
            ONITEMLABEL = 0x0004,
            ONITEM = ONITEMICON | ONITEMLABEL | ONITEMSTATEICON,
            ONITEMINDENT = 0x0008,
            ONITEMBUTTON = 0x0010,
            ONITEMRIGHT = 0x0020,
            ONITEMSTATEICON = 0x0040,
            ABOVE = 0x0100,
            BELOW = 0x0200,
            TORIGHT = 0x0400,
            TOLEFT = 0x0800
        }
    }
}