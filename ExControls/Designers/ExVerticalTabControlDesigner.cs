using System.Collections;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms.Design;

namespace ExControls.Designers;

/// <summary>
/// 
/// </summary>
internal class ExVerticalTabControlDesigner : ParentControlDesigner
{
    private DesignerVerbCollection _verbs;
    private bool addingOnInit;

    private IDesignerHost designerHost;

    private ExVerticalTabControl TabControl => Component as ExVerticalTabControl;

    /// <inheritdoc />
    public override DesignerVerbCollection Verbs
    {
        get 
        { 
            return _verbs ??= new DesignerVerbCollection(new[]
            {
                new DesignerVerb("Add Tab", OnAddTab),
                new DesignerVerb("Remove Tab", OnRemoveTab),
                new DesignerVerb("Select Tab...", OnSelectTab)
            });
        }
    }

    private void OnAddTab(object sender, EventArgs e)
    {
        var host = (IDesignerHost) GetService(typeof(IDesignerHost));
        if(host == null) return;
        var transaction = host.CreateTransaction("Add TabPage");
        try
        {
            MemberDescriptor member = TypeDescriptor.GetProperties(TabControl)["TabPages"];
            var page = (ExVerticalTabPage) host.CreateComponent(typeof(ExVerticalTabPage));

            if (!addingOnInit) RaiseComponentChanging(member);

            string text = null;
            var nameDescriptor = TypeDescriptor.GetProperties(page)["Name"];
            if (nameDescriptor != null && nameDescriptor.PropertyType == typeof(string))
                text = (string) nameDescriptor.GetValue(page);
            if (text != null)
            {
                var textDescriptor = TypeDescriptor.GetProperties(page)["Text"];
                textDescriptor?.SetValue(page, text);
            }

            TabControl.TabPages.Add(page);
            TabControl.SelectedTab = page;
            page.Node.Name = page.Name;
            page.Node.Text = page.Name;

            if (!addingOnInit) RaiseComponentChanged(member, null, null);
        }
        finally
        {
            transaction?.Commit();
        }
    }

    private void OnSelectTab(object sender, EventArgs e)
    {
        if (Control is not ExVerticalTabControl ctl || ctl.TabPages.Count == 0)
            return;

        using var selectPage = new FSelectPage(TabControl);
        selectPage.ShowDialog();
    }

    private void OnRemoveTab(object sender, EventArgs e)
    {
        var host = (IDesignerHost) GetService(typeof(IDesignerHost));
        if(host == null) return;
        var transaction = host.CreateTransaction("Remove TabPage");
        try
        {
            MemberDescriptor member = TypeDescriptor.GetProperties(TabControl)["TabPages"];

            var page = TabControl.SelectedTab;
            if (page is null) return;

            RaiseComponentChanging(member);
            TabControl.TabPages.Remove(page);
            host.DestroyComponent(page);

            RaiseComponentChanged(member, null, null);
        }
        finally
        {
            transaction?.Commit();
        }
    }

    /// <inheritdoc />
    public override void Initialize(IComponent component)
    {
        base.Initialize(component);
        AutoResizeHandles = true;
        EnableDesignMode(TabControl.ToolStripMenu, "ToolStripMenu");
        //EnableDesignMode(TabControl.TreeView, "TreeView");
        foreach (var pageO in TabControl.TabPages)
        {
            if (pageO is not ExVerticalTabPage page) continue;
            TypeDescriptor.AddAttributes(page, new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content));
            EnableDesignMode(page, page.Name);
        }
        designerHost = (IDesignerHost)component.Site.GetService(typeof(IDesignerHost));
    }

    /// <inheritdoc />
    public override void InitializeNewComponent(IDictionary defaultValues)
    {
        base.InitializeNewComponent(defaultValues);
        addingOnInit = true;
        OnAddTab(this, EventArgs.Empty);
        OnAddTab(this, EventArgs.Empty);
        addingOnInit = false;
    }

    public override void InitializeExistingComponent(IDictionary defaultValues)
    {
        base.InitializeExistingComponent(defaultValues);
        foreach (var pageO in TabControl.TabPages)
        {
            if (pageO is not ExVerticalTabPage page) continue;
            TypeDescriptor.AddAttributes(page, new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content));
            EnableDesignMode(page, page.Name);
        }
    }

    /// <inheritdoc />
    protected override IComponent[] CreateToolCore(ToolboxItem tool, int x, int y, int width, int height, bool hasLocation, bool hasSize)
    {
        if (designerHost != null)
        {
            var toInvoke = (ExVerticalTabPageDesigner)designerHost.GetDesigner(TabControl.SelectedTab);
            InvokeCreateTool(toInvoke, tool);
        }

        return null;
    }

    /// <inheritdoc />
    protected override void WndProc(ref Message m) {
        base.WndProc(ref m);
        if (m.Msg == (int) Win32.WM.LBUTTONDOWN){
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

    /// <inheritdoc />
    public override ICollection AssociatedComponents => TabControl.rootPanel.Controls.Cast<Control>().ToList();

    /// <inheritdoc />
    public override bool CanParent(Control control)
    {
        return false;
        //return control is ExVerticalTabControl tc && !tc.TabPages.Contains(control);
    }

    /*/// <inheritdoc />
    public override int NumberOfInternalControlDesigners()
    {
        return TabControl.TabPages.Count;
    }

    public override ControlDesigner InternalControlDesigner(int internalControlIndex)
    {
        switch (internalControlIndex)
        {
            case 0:
                return designerHost.GetDesigner(panel) as ControlDesigner;
            default:
                return null;
        }
    }*/


    /// <inheritdoc />
    protected override Control GetParentForComponent(IComponent component)
    {
        return TabControl.SelectedTab;
    }

    protected override bool CanAddComponent(IComponent component)
    {
        return false;
    }
}