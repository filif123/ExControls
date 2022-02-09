using ExControls.Collections;
using ExControls.Designers;
// ReSharper disable UnusedMember.Global
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace ExControls;

//--------------------------- WORK IN PROGRESS ----------------------
// Notes:
// - nefunguje spravne pridavanie prvku do TabPage (da sa umiestnit, neda sa premiestnovat a ked je vybraty, neda sa vratit naspat)
// - upravit kolekciu a editor stranok tak, aby bolo mozne upravit poradie a umiestnenie (+ pridavat a odstranovat)
// - dobrovolne: Undo/Redo support
// - odstranit staru verziu ExVerticalMenu.cs
//
//-------------------------------------------------------------------

/// <summary>
/// </summary>
[Designer(typeof(ExVerticalTabControlDesigner))]
public partial class ExVerticalTabControl : UserControl
{
    private ExVerticalTabPage selectedTab;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler SelectedTabChanged; 

    /// <summary>
    /// </summary>
    public ExVerticalTabControl()
    {
        InitializeComponent();
        TabPages = new VerticalTabPagesCollection(null);
        TabPages.PageAdded += TabPages_PageAdded;
        TabPages.PageRemoved += TabPages_PageRemoved;
    }

    /// <summary>
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public VerticalTabPagesCollection TabPages { get; }

    /// <summary>
    /// </summary>
    [DefaultValue(true)]
    public virtual bool ShowSearchBox
    {
        get => cboxSearch.Visible;
        set => cboxSearch.Visible = value;
    }

    /// <summary>
    /// </summary>
    [DefaultValue(true)]
    public virtual bool ShowPanelName
    {
        get => labelTabName.Visible;
        set => labelTabName.Visible = value;
    }

    /// <summary>
    /// </summary>
    [DefaultValue(true)]
    public virtual bool ShowToolStrip
    {
        get => toolStripMenu.Visible;
        set => toolStripMenu.Visible = value;
    }

    /// <summary>
    /// 
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ToolStrip ToolStripMenu => toolStripMenu;

    /// <summary>
    /// 
    /// </summary>
    public ExTreeView TreeView => treeView;

    /// <summary>
    /// 
    /// </summary>
    public ExVerticalTabPage SelectedTab
    {
        get => selectedTab;
        set
        {
            if (value == selectedTab)
                return;

            treeView.SelectedNode = value?.Node;
            OnSelectedTabChanged();
        }
    }

    private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
    {
        labelTabName.Text = e.Node.Text;
        HideAllTabs();
        var page = TabPages[e.Node.Index] as ExVerticalTabPage;
        if (page is not null) 
            page.Visible = true;
        selectedTab = page;
        OnSelectedTabChanged();
    }

    private void HideAllTabs()
    {
        foreach (Control control in rootPanel.Controls) 
            control.Visible = false;
    }

    private void rootPanel_ControlAdded(object sender, ControlEventArgs e)
    {
        if (e.Control is not ExVerticalTabPage)
            throw new ArgumentException("RootPanel only supports ExVerticalTabPage");
    }

    private void TabPages_PageAdded(object sender, TabPageEventArgs e)
    {
        if (DesignMode) labelTabName.Text = e.TabPage.Node.Text;

        if (e.Count == 1)
        {
            TreeView.Nodes.Insert(e.Index, e.TabPage.Node);
            rootPanel.Controls.Add(e.TabPage);
        }
        else
        {
            foreach (ExVerticalTabPage page in TabPages)
            {
                TreeView.Nodes.Add(page.Node);
                rootPanel.Controls.Add(page);
            }
        }
    }

    private void TabPages_PageRemoved(object sender, TabPageEventArgs e)
    {
        if (e.Count == 1)
        {
            TreeView.Nodes.Remove(e.TabPage.Node);
        }
        else
        {
            TreeView.Nodes.Clear();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void OnSelectedTabChanged()
    {
        SelectedTabChanged?.Invoke(this, EventArgs.Empty);
    }

    private void rootPanel_ControlRemoved(object sender, ControlEventArgs e)
    {
        TabPages.Remove(e.Control);
    }
}