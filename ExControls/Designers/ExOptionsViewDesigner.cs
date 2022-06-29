using System.Collections;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;

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
    }

    public override DesignerActionListCollection ActionLists => _actionLists ??= new DesignerActionListCollection {new OptionsViewActionList(Host, this)};

    private ExOptionsPanel SelectedPanel { get; set; }

    protected override void OnHostInitialized()
    {
        // Handle mouseclicks on the TreeView
        Host.TreeView.MouseDown += OnTreeViewClicked;
    }

    /// <inheritdoc />
    public override void Initialize(IComponent component)
    {
        base.Initialize(component);
        if (Control is ExOptionsView ctrl) 
            EnableDesignMode(ctrl.ToolStripMenu, "ToolStripMenu");
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

    public override bool CanParent(Control control)
    {
        // The ExOptionsView can technically parent controls because this designer is a ParentControlDesigner
        // We don't want the user dragging buttons and the like on it, so disable this behavior.
        return false;
    }

    public override bool CanParent(ControlDesigner controlDesigner) => false;

    private void OnTreeViewClicked(object sender, MouseEventArgs e)
    {
        // When the treeview is clicked, select the host ExOptionsView (otherwise there would be no way to select it)
        SelectHost();
    }

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

    protected override bool GetHitTest(Point pt)
    {
        pt = Host.PointToClient(pt);
        // Allow treenode selection / expansion
        if (Host.TreeView.ClientRectangle.Contains(pt))
            return true;

        // Allow splitter moving
        if (Host.SplitContainer.SplitterRectangle.Contains(pt))
            return true;

        //Allow page scrolling
        if (Host.SelectedPanel != null && Host.SelectedPanel.ClientRectangle.Contains(pt))
            return true;

        return base.GetHitTest(pt);
    }

    private void OnAddPanel()
    {
        // Store old items for undo
        var oldPanels = Host.Panels;
        if (!_newComponentInit) RaiseComponentChanging(TypeDescriptor.GetProperties(Host)["Panels"]);
        var panel = ExOptionsPanel.CreatePanel(Host, DesignerHost);
        Host.Panels.Add((Control)panel);
        if (!_newComponentInit) RaiseComponentChanged(TypeDescriptor.GetProperties(Host)["Panels"], oldPanels, Host.Panels);
        Host.SelectedPanel = (ExOptionsPanel)panel;
        SelectHost();
    }

    private void OnRemovePanel()
    {
        // Store old items for undo
        var oldPanels = Host.Panels;
        var panel = Host.SelectedPanel;
        if (panel is null)
            return;

        RaiseComponentChanging(TypeDescriptor.GetProperties(Host)["Panels"]);
        Host.Panels.Remove(panel);
        DesignerHost.DestroyComponent(panel);
        RaiseComponentChanged(TypeDescriptor.GetProperties(Host)["Panels"], oldPanels, Host.Panels);
        SelectHost();
    }

    private class OptionsViewActionList : DesignerActionListBase<ExOptionsView>
    {
        public OptionsViewActionList(ExOptionsView host, ExOptionsViewDesigner designer) : base(host)
        {
            Designer = designer;
        }

        private ExOptionsViewDesigner Designer { get; }

        protected void AddPanel()
        {
            Designer.OnAddPanel();
            DesignerActionService.Refresh(Host);
        }

        protected void RemovePanel()
        {
            Designer.OnRemovePanel();
            DesignerActionService.Refresh(Host);
        }

        protected void EditPanels()
        {
            ExEditorServiceContext.EditValue(Designer, Component, "Panels");
        }

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
            var items = new DesignerActionItemCollection();
            items.Add(new DesignerActionHeaderItem("Panels"));
            items.Add(new DesignerActionMethodItem(this, nameof(AddPanel), "Add Panel", "Panels", "Adds a new ExOptionsPanel to this ExOptionsView.", true));
            items.Add(new DesignerActionMethodItem(this, nameof(EditPanels), "Edit Panels...", "Panels", "Edits a collection of panels in this ExOptionsView.", true));
            items.Add(new DesignerActionPropertyItem(nameof(Dock), "Dock:", "", "Docks this control to a side."));
            items.Add(new DesignerActionPropertyItem(nameof(SelectedPanel), "Selected Panel:", "Panels", "Gets or sets the selected ExOptionsPanel."));
            if (Host.Panels.Count > 1)
                items.Add(new DesignerActionMethodItem(this, nameof(RemovePanel), "Remove Panel", "Panels", "Removes the selected ExOptionsPanel from this ExOptionsView.", true));
            return items;
        }
    }
}