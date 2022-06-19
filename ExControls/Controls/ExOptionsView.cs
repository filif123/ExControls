﻿using ExControls.Collections;
using ExControls.Designers;
using ExControls.Editors;
using System.Drawing.Design;

// ReSharper disable UnusedMember.Global
// ReSharper disable ConvertToAutoPropertyWhenPossible
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace ExControls;

/// <summary>
/// 
/// </summary>
[Designer(typeof(ExOptionsViewDesigner))]
[DefaultEvent("SelectedPanelChanged")]
[ToolboxBitmap(typeof(ExOptionsView), "Controls\\ExOptionsView.bmp")]
public partial class ExOptionsView : UserControl, ISupportInitialize
{
    private ExOptionsPanel _selectedPanel;
    private readonly ExOptionsPanel _onSelectedPanelChangedOldSelection = null;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler SelectedPanelChanged;

    /// <summary>
    /// 
    /// </summary>
    public ExOptionsView()
    {

        // This call is required by the designer.
        InitializeComponent();

        // Add any initialization after the InitializeComponent() call.
        TreeView.AfterSelect += OnNodeSelected;
        PanelContainer.ControlAdded += OnPanelAdded;
        PanelContainer.ControlRemoved += OnPanelRemoved;

        toolStripMenu.Renderer = new NoBorderRenderer();
    }

    /// <summary>
    /// Gets the TreeView on this ExOptionsView.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [ExCategory(CategoryType.Layout)]
    public ExOptionsTreeView TreeView => treeView;

    /// <summary>
    /// Gets the ToolStrip on this ExOptionsView.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [ExCategory(CategoryType.Layout)]
    public ToolStrip ToolStripMenu => toolStripMenu;

    /// <summary>
    /// Gets or sets visibility of the SearchBox.
    /// </summary>
    [DefaultValue(true)]
    [ExCategory(CategoryType.Appearance)]
    public bool SearchBoxVisible
    {
        get => cbSearch.Visible;
        set => cbSearch.Visible = value;
    }

    /// <summary>
    /// Gets or sets visibility of the Header node name.
    /// </summary>
    [DefaultValue(true)]
    [ExCategory(CategoryType.Appearance)]
    public bool HeaderNodeNameVisible
    {
        get => labelPanelName.Visible;
        set => labelPanelName.Visible = value;
    }

    /// <summary>
    /// Gets or sets font of header panel name.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    public Font HeaderNodeNameFont
    {
        get => labelPanelName.Font;
        set => labelPanelName.Font = value;
    }

    /// <summary>
    /// Gets the panel that contains the OptionsPanels on this ExOptionsView.
    /// </summary>
    public OptionsPanelContainer PanelContainer => panelsContainer;

    internal SplitContainer SplitContainer => split;

    /// <summary>
    /// Gets the collection of ExOptionsPanel controls.
    /// </summary>
    [ExDescription("The ExOptionsPanel controls on this ExOptionsView.", true)]
    [Editor(typeof(OptionsPanelCollectionEditor), typeof(UITypeEditor))]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [ExCategory(CategoryType.Behavior)]
    public ControlCollection Panels => PanelContainer.Controls;

    /// <summary>
    /// Gets or sets the selected (displayed) ExOptionsPanel.
    /// </summary>
    [ExDescription("The selected (displayed) ExOptionsPanel.", true)]
    [ExCategory(CategoryType.Behavior)]
    public ExOptionsPanel SelectedPanel
    {
        get => _selectedPanel;
        set
        {
            _selectedPanel = value;
            OnSelectedPanelChanged();
        }
    }

    /// <inheritdoc />
    public void BeginInit()
    {
    }

    /// <inheritdoc />
    public void EndInit()
    {
        if (DesignMode) 
            return;

        // During run-time, after InitializeComponent method is done, we add the nodes for each panel to the TreeView.
        // The nodes are added during design-time when the panels are added to the control, but the order in which this
        // happens in the InitializeComponent makes this impossible during run-time. Panels are added before their nodes are set.
        TreeView.Nodes.Clear();
        foreach (ExOptionsPanel panel in Panels)
            AddNode(panel);
    }

    /// <summary>
    /// When the selected panel changes, display it.
    /// </summary>
    protected virtual void OnSelectedPanelChanged()
    {
        if (_onSelectedPanelChangedOldSelection != null)
            _onSelectedPanelChangedOldSelection.Visible = false;

        if (SelectedPanel != null)
        {
            // Hide every item except the selected item
            foreach (Control item in Panels)
                item.Visible = ReferenceEquals(item, SelectedPanel);
        }

        bool changed;
        if (SelectedPanel is null)
            changed = _onSelectedPanelChangedOldSelection != null;
        else
            changed = !SelectedPanel.Equals(_onSelectedPanelChangedOldSelection);

        if (DesignMode)
            labelPanelName.Text = SelectedPanel == null ? "" : SelectedPanel.Node.FullPath;

        if (changed && Created)
            SelectedPanelChanged?.Invoke(this, EventArgs.Empty);
    }

    // Change the selected panel when a node is selected
    private void OnNodeSelected(object sender, TreeViewEventArgs e)
    {
        if (e.Node is not OptionsNode node)
            return;

        SelectedPanel = node.Panel;
        labelPanelName.Text = SelectedPanel == null ? "" : node.FullPath;
    }

    private void OnPanelAdded(object sender, RestrictivePanel<ExOptionsPanel>.RestrictivePanelEventArgs<ExOptionsPanel> e)
    {
        if (!DesignMode)
            return;

        // During design mode we add the node for this panel to the treeview.
        AddNode(e.Control);
    }

    private void OnPanelRemoved(object sender, RestrictivePanel<ExOptionsPanel>.RestrictivePanelEventArgs<ExOptionsPanel> e)
    {
        if (!DesignMode)
            return;

        // During design mode we remove the node for this panel to the treeview.
        RemoveNode(e.Control);

        // TODO: Also remove child panels
    }

    private void AddNode(ExOptionsPanel panel)
    {
        if (panel.ParentNode is null)
            TreeView.Nodes.Add(panel.Node);
        else
            panel.ParentNode.Nodes.Add(panel.Node);
    }

    private void RemoveNode(ExOptionsPanel panel)
    {
        if (panel.ParentNode is null)
            TreeView.Nodes.Remove(panel.Node);
        else
            panel.ParentNode.Nodes.Remove(panel.Node);
    }

    /// <summary>
    ///     Search and select specific panel by its name (not NodeText).
    ///     If panel was not found, do nothing.
    /// </summary>
    /// <param name="name">name of panel.</param>
    public void ShowPanel(string name)
    {
        var panel = Panels.Cast<ExOptionsPanel>().FirstOrDefault(p => p.Name == name);
        if (panel != null)
            SelectedPanel = panel;
    }

    /// <inheritdoc />
    public class ExOptionsTreeView : ExTreeView
    {
        /// <inheritdoc />
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override ExTreeNodeCollection Nodes => base.Nodes;

        /// <inheritdoc />
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override AnchorStyles Anchor
        {
            get => base.Anchor;
            set => base.Anchor = value;
        }

        /// <inheritdoc />
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override DockStyle Dock
        {
            get => base.Dock;
            set => base.Dock = value;
        }

        /// <summary>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Point Location
        {
            get => base.Location;
            set => base.Location = value;
        }

        /// <summary>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Padding Margin
        {
            get => base.Margin;
            set => base.Margin = value;
        }

        /// <inheritdoc />
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override Size MaximumSize
        {
            get => base.MaximumSize;
            set => base.MaximumSize = value;
        }

        /// <inheritdoc />
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override Size MinimumSize
        {
            get => base.MinimumSize;
            set => base.MinimumSize = value;
        }

        /// <summary>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Size Size
        {
            get => base.Size;
            set => base.Size = value;
        }

        /// <inheritdoc />
        public override string ToString() => "(Collection)";
    }
}