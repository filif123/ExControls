using System.Collections;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;
using ExControls.Designers;

namespace ExControls.Controls;

/// <summary>
/// 
/// </summary>
//TODO ble
[Designer(typeof(ExVerticalMenuDesigner2))]
public partial class ExVerticalMenu : UserControl
{
    internal readonly Dictionary<TreeNode, MenuPanel> panels = new();
    private MenuPanel _selectedPage;

    public event EventHandler<ExVerticalMenuPageEventArgs> PanelAdded;
    public event EventHandler<ExVerticalMenuPageEventArgs> PanelRemoved;

    /// <summary>
    /// 
    /// </summary>
    public ExVerticalMenu()
    {
        InitializeComponent();
    }

    //public new ControlCollection Controls => RootPanel.Controls;

    /// <summary>
    /// 
    /// </summary>
    public ExTreeNodeCollection Pages => TreeMenu.Nodes;

    public MenuPanel[] Panels => panels.Values.ToArray();

    /// <summary>
    /// 
    /// </summary>
    public MenuPanel SelectedPage
    {
        get => _selectedPage;
        set
        {
            if (_selectedPage == value)
                return;

            _selectedPage = value;

            foreach (var pair in panels)
            {
                if (pair.Value == value) 
                    TreeMenu.SelectedNode = pair.Key;
            }
        }
    }

    private void TreeMenu_AfterSelect(object sender, TreeViewEventArgs e)
    {
        if (panels.TryGetValue(e.Node, out var panel))
        {
            HideOtherPanelsAndShowThis(panel);
            _selectedPage = panel;
        }
    }

    private void TreeMenu_TreeNodeAdded(object sender, ExTreeViewNodeAddedEventArgs e)
    {
        MenuPanel panel = null;
        for (int i = e.Index; i < e.Index + e.Count; i++)
        {
            panel = new MenuPanel();
            panel.Dock = DockStyle.Fill;
            TreeMenu.Nodes[i].Tag = panel;
            panels.Add(TreeMenu.Nodes[i], panel);
            RootPanel.Controls.Add(panel);
            OnPanelAdded(new ExVerticalMenuPageEventArgs(panel));
        }
        HideOtherPanelsAndShowThis(panel);
        _selectedPage = panel;
    }

    private void TreeMenu_TreeNodeRemoved(object sender, ExTreeViewNodeRemovedEventArgs e)
    {
        if (e.Node == null)
        {
            foreach (var p in panels.Values)
            {
                RootPanel.Controls.Remove(p);
                OnPanelRemoved(new ExVerticalMenuPageEventArgs(p));
            }

            panels.Clear();
            return;
        }

        if (panels.TryGetValue(e.Node, out var panel))
        {
            RootPanel.Controls.Remove(panel);
            OnPanelRemoved(new ExVerticalMenuPageEventArgs(panel));
        }
    }

    private void HideOtherPanelsAndShowThis(MenuPanel panel)
    {
        foreach (var p in panels.Values)
        {
            if (p != panel) 
                p.Visible = false;
        }

        if (panel != null) 
            panel.Visible = true;

    }

    protected virtual void OnPanelAdded(ExVerticalMenuPageEventArgs e)
    {
        PanelAdded?.Invoke(this, e);
    }

    protected virtual void OnPanelRemoved(ExVerticalMenuPageEventArgs e)
    {
        PanelRemoved?.Invoke(this, e);
    }
}

public class ExVerticalMenuPageEventArgs
{
    public MenuPanel Panel { get; }

    /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
    public ExVerticalMenuPageEventArgs(MenuPanel panel)
    {
        Panel = panel;
    }
}

[Designer(typeof(MenuPanelDesigner))]
public class MenuPanel : Panel
{

}

internal class MenuPanelDesigner : ScrollableControlDesigner
{
    public MenuPanelDesigner() => AutoResizeHandles = true;

    protected virtual void DrawBorder(Graphics graphics)
    {
        var component = (MenuPanel)Component;
        if (component is not { Visible: true })
            return;
        var borderPen = BorderPen;
        var clientRectangle = Control.ClientRectangle;
        --clientRectangle.Width;
        --clientRectangle.Height;
        graphics.DrawRectangle(borderPen, clientRectangle);
        borderPen.Dispose();
    }

    protected override void OnPaintAdornments(PaintEventArgs pe)
    {
        if (((MenuPanel)Component).BorderStyle == BorderStyle.None)
            DrawBorder(pe.Graphics);
        base.OnPaintAdornments(pe);
    }

    protected Pen BorderPen => new(Control.BackColor.GetBrightness() < 0.5 ? ControlPaint.Light(Control.BackColor) : ControlPaint.Dark(Control.BackColor))
    {
        DashStyle = DashStyle.Dash
    };

    public override SelectionRules SelectionRules
    {
        get
        {
            var selectionRules = base.SelectionRules;

            //if (Control is { Parent: ExVerticalMenu })
                selectionRules &= ~SelectionRules.AllSizeable;
            return selectionRules;
        }
    }
}

internal class ExVerticalMenuDesigner2 : ParentControlDesigner
{
    private DesignerVerbCollection verbs;
    private DesignerVerb onSelect;
    private bool addingOnInitialize;

    private ExVerticalMenu ExControl => Control as ExVerticalMenu;

    public override void Initialize(IComponent comp)
    {
        base.Initialize(comp);
        EnableDesignMode(ExControl.ToolBar, "ToolBar");
        ExControl.PanelAdded += PanelAdded;
        foreach (var panel in ExControl.Panels)
        {
            EnableDesignMode(panel, panel.Name);
        }
    }

    private void PanelAdded(object sender, ExVerticalMenuPageEventArgs e)
    {
        EnableDesignMode(e.Panel, e.Panel.Name);
    }

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Design.ControlDesigner" /> and optionally releases the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
            ExControl.PanelAdded -= PanelAdded;
        base.Dispose(disposing);
    }

    public override void InitializeNewComponent(IDictionary defaultValues)
    {
        base.InitializeNewComponent(defaultValues);

        try
        {
            addingOnInitialize = true;
            OnAdd(this, EventArgs.Empty);
            OnAdd(this, EventArgs.Empty);
        }
        finally
        {
            addingOnInitialize = false;
        }

        //MemberDescriptor member = TypeDescriptor.GetProperties(Component)["Pages"];
        //RaiseComponentChanging(member);
        //RaiseComponentChanged(member, null, null);


        //var tc = (ExVerticalMenu)Component;
        //if (tc != null) tc.TreeMenu.SelectedNode = tc.Pages[0];
    }

    public override DesignerVerbCollection Verbs
    {
        get
        {
            if (verbs == null) 
                onSelect = new DesignerVerb("Select page...", OnSelect);
            
            verbs ??= new DesignerVerbCollection
            {
                onSelect,
                new DesignerVerb("Edit pages...", OnEdit)
            };

            var ctl = (ExVerticalMenu)Control;
            onSelect.Enabled = ctl.Panels.Length > 0;

            return verbs;
        }
    }

    private void OnEdit(object sender, EventArgs e)
    {
        var menu = (ExVerticalMenu)Component;
        var s = GetService(typeof(IDesignerHost));

        var host = (IDesignerHost)GetService(typeof(IDesignerHost));
        if (host == null)
            return;

        DesignerTransaction t = null;
        try
        {
            try
            {
                t = host.CreateTransaction("Edit pages");
            }
            catch (CheckoutException ex)
            {
                if (ex == CheckoutException.Canceled)
                    return;
                throw;
            }
            MemberDescriptor member = TypeDescriptor.GetProperties(menu)["Pages"];
            var page = (MenuPanel)host.CreateComponent(typeof(MenuPanel));
            if (!addingOnInitialize)
            {
                RaiseComponentChanging(member);
            }

            string pageText = null;

            var nameProp = TypeDescriptor.GetProperties(page)["Name"];
            if (nameProp != null && nameProp.PropertyType == typeof(string))
            {
                pageText = (string)nameProp.GetValue(page);
            }

            if (pageText != null)
            {
                var textProperty = TypeDescriptor.GetProperties(page)["Text"];
                Debug.Assert(textProperty != null, "Could not find 'Text' property in Panel.");
                textProperty.SetValue(page, pageText);
            }

            var styleProp = TypeDescriptor.GetProperties(page)["UseVisualStyleBackColor"];
            if (styleProp != null && styleProp.PropertyType == typeof(bool) && !styleProp.IsReadOnly && styleProp.IsBrowsable)
            {
                styleProp.SetValue(page, true);
            }

            menu.Pages.Add(page.Name);
            menu.RootPanel.Controls.Add(page);
            // Make sure that the last tab is selected. 
            menu.SelectedPage = page;
            if (!addingOnInitialize)
                RaiseComponentChanged(member, null, null);
        }
        finally
        {
            t?.Commit();
        }
    }

    private void OnAdd(object sender, EventArgs e)
    {
        
    }

    private void OnSelect(object sender, EventArgs e)
    {
        if (Control is not ExVerticalMenu ctl || ctl.Panels.Length == 0)
            return;

        using var selectPage = new FSelectPage(ctl.panels);
        var result = selectPage.ShowDialog();
        if (result == DialogResult.OK)
        {
            ctl.SelectedPage = selectPage.SelectedPage;
        }
    }

    public override bool CanParent(Control control)
    {
        return (control is MenuPanel && !ExControl.RootPanel.Controls.Contains(control));
    }
}

internal class ExVerticalMenuDesigner : ParentControlDesigner
{
    private bool tabControlSelected;    //used for HitTest logic 
    private DesignerVerbCollection verbs;
    private DesignerVerb removeVerb;
    private bool disableDrawGrid;

    private const int HTCLIENT = 1;

    // Shadow SelectedIndex property so we separate the persisted value shown in the 
    // properties window from the visible selected page the user is currently working on.
    private bool addingOnInitialize;  //used so that we can fire the ComponentChangedEvent only aftger adding (both) the tabPages during
                                      // InitializeNewComponent. 

    private bool forwardOnDrag;     //should we forward the OnDrag call to the TabPageDesigner 

    /// 
    ///  
    ///     The TabControl will not re-parent any controls that are within it's lasso at
    ///     creation time.
    /// 
    protected override bool AllowControlLasso => false;

    protected override bool DrawGrid => !disableDrawGrid && base.DrawGrid;

    /// 
    /// 
    ///     Overridden to disallow SnapLines during drag operations if we are dragging TabPages around
    ///  
    public override bool ParticipatesWithSnapLines
    {
        get
        {
            if (!forwardOnDrag)
            {
                return false;
            }

            var pageDesigner = GetSelectedTabPageDesigner();
            return pageDesigner == null || pageDesigner.ParticipatesWithSnapLines;
        }
    }

    /// 
    /// 
    ///     Accessor method for the SelectedIndex property on TabControl.  We shadow 
    ///     this property at design time.
    ///  
    private Panel SelectedPage { get; set; } = null;

    /// 
    ///  
    ///     Returns the design-time verbs supported by the component associated with
    ///     the customizer. The verbs returned by this method are typically displayed
    ///     in a right-click menu by the design-time environment. The return value may
    ///     be null if the component has no design-time verbs. When a user selects one 
    ///     of the verbs, the performVerb() method is invoked with the the
    ///     corresponding DesignerVerb object. 
    ///     NOTE: A design-time environment will typically provide a "Properties..." 
    ///     entry on a component's right-click menu. The getVerbs() method should
    ///     therefore not include such an entry in the returned list of verbs. 
    /// 
    public override DesignerVerbCollection Verbs
    {
        get
        {
            if (verbs == null)
            {

                removeVerb = new DesignerVerb("Remove MenuItem", OnRemove);

                verbs = new DesignerVerbCollection
                {
                    new DesignerVerb("Add MenuItem", OnAdd),
                    removeVerb
                };
            }
            if (Control != null)
            {
                removeVerb.Enabled = Control.Controls.Count > 0;
            }
            return verbs;
        }
    }

    /// 
    /// 
    ///     Called when the designer is intialized.  This allows the designer to provide some 
    ///     meaningful default values in the control.
    ///  
    public override void InitializeNewComponent(IDictionary defaultValues)
    {
        base.InitializeNewComponent(defaultValues);

        // Add 2 tab pages
        // member is OK to be null...
        //
        try
        {
            addingOnInitialize = true;
            OnAdd(this, EventArgs.Empty);
            OnAdd(this, EventArgs.Empty);
        }
        finally
        {
            addingOnInitialize = false;
        }

        MemberDescriptor member = TypeDescriptor.GetProperties(Component)["Controls"];
        RaiseComponentChanging(member);
        RaiseComponentChanged(member, null, null);


        var tc = (ExVerticalMenu)Component;
        if (tc != null)
        { //always Select the First Tab on Initialising the component...
            tc.TreeMenu.SelectedNode = tc.Pages[0];
        }
    }

    /// 
    ///  
    ///     Determines if the this designer can parent to the specified desinger --
    ///     generally this means if the control for this designer can parent the
    ///     given ControlDesigner's designer.
    ///  
    public override bool CanParent(Control control)
    {
        // If the tabcontrol already contains the control we are dropping then don't allow the drop. 
        // I.e. we don't want to allow local drag-drop for tabcontrols. 
        return Control is ExVerticalMenu menu && (control is Panel && !menu.Panels.Contains(control));
    }

    private void CheckVerbStatus()
    {
        if (removeVerb != null && Control is ExVerticalMenu menu) 
            removeVerb.Enabled = menu.Panels.Length > 0;
    }

    /// 
    ///  
    ///      This is the worker method of all CreateTool methods.  It is the only one
    ///      that can be overridden.
    /// 
    protected override IComponent[] CreateToolCore(ToolboxItem tool, int x, int y, int width, int height, bool hasLocation, bool hasSize)
    {
        var tc = (ExVerticalMenu)Control;

        //VSWhidbey #409457 
        if (tc.SelectedPage == null)
        {
            throw new ArgumentException("Invalid MenuItem page type");
        }
        var host = (IDesignerHost)GetService(typeof(IDesignerHost));
        if (host == null) 
            return null;

        var selectedTabPageDesigner = (TabPageDesigner)host.GetDesigner(tc.SelectedPage); //TODO vracia null
        InvokeCreateTool(selectedTabPageDesigner, tool);
        // InvokeCreate Tool will do the necessary hookups. 
        return null;
    }


    /// 
    /// 
    ///     Disposes of this designer. 
    /// 
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            var svc = (ISelectionService)GetService(typeof(ISelectionService));
            if (svc != null) 
                svc.SelectionChanged -= OnSelectionChanged;


            var cs = (IComponentChangeService)GetService(typeof(IComponentChangeService));
            if (cs != null) 
                cs.ComponentChanged -= OnComponentChanged;

            if (Control is ExVerticalMenu menu)
            {
                menu.TreeMenu.AfterSelect -= OnTabSelectedIndexChanged;
                menu.GotFocus -= OnGotFocus;
                menu.ControlAdded -= OnControlAdded;
            }

        }

        base.Dispose(disposing);
    }

    /// 
    ///  
    ///     Allows your component to support a design time user interface.  A TabStrip 
    ///     control, for example, has a design time user interface that allows the user
    ///     to click the tabs to change tabs.  To implement this, TabStrip returns 
    ///     true whenever the given point is within its tabs.
    /// 
    protected override bool GetHitTest(Point point)
    {
        var tc = (ExVerticalMenu)Control;

        // tabControlSelected tells us if a tab page or the tab control itself is selected. 
        // If the tab control is selected, then we need to return true from here - so we can switch back and forth 
        // between tabs.  If we're not currently selected, we want to select the tab control
        // so return false. 
        if (!tabControlSelected) 
            return false;
        var hitTest = Control.PointToClient(point);
        return !tc.DisplayRectangle.Contains(hitTest);
    }

    /// 
    ///  
    ///     Given a component, this retrieves the tab page that it's parented to, or
    ///     null if it's not parented to any tab page.
    /// 
    internal static Panel GetTabPageOfComponent(object comp)
    {
        if (comp is not Control c)
            return null;

        while (c != null && c is not Panel)
        {
            c = c.Parent;
        }
        return (Panel)c;
    }

    ///  
    /// 
    ///     Called by the host when we're first initialized. 
    /// 
    public override void Initialize(IComponent component)
    {
        base.Initialize(component);

        AutoResizeHandles = true;
        var control = component as ExVerticalMenu;
        Debug.Assert(control != null, "Component must be a tab control, it is a: " + component.GetType().FullName);

        var svc = (ISelectionService)GetService(typeof(ISelectionService));
        if (svc != null)
        {
            svc.SelectionChanged += OnSelectionChanged;
        }

        var cs = (IComponentChangeService)GetService(typeof(IComponentChangeService));
        if (cs != null)
        {
            cs.ComponentChanged += OnComponentChanged;
        }

        control.TreeMenu.AfterSelect += OnTabSelectedIndexChanged;
        control.GotFocus += OnGotFocus;
        control.ControlAdded += OnControlAdded;
    }


    /// 
    /// 
    ///      Called in response to a verb to add a tab.  This adds a new 
    ///      tab with a default name.
    ///  
    private void OnAdd(object sender, EventArgs eevent)
    {
        var menu = (ExVerticalMenu)Component;


        var host = (IDesignerHost)GetService(typeof(IDesignerHost));
        if (host == null) 
            return;

        DesignerTransaction t = null;
        try
        {
            try
            {
                t = host.CreateTransaction("Add MenuItem");
            }
            catch (CheckoutException ex)
            {
                if (ex == CheckoutException.Canceled)
                    return;
                throw;
            }
            MemberDescriptor member = TypeDescriptor.GetProperties(menu)["Pages"];
            var page = (Panel)host.CreateComponent(typeof(Panel));
            if (!addingOnInitialize)
            {
                RaiseComponentChanging(member);
            }

            // NOTE:  We also modify padding of TabPages added through the TabPageCollectionEditor.
            //        If you need to change the default Padding, change it there as well. 
            page.Padding = new Padding(3);

            string pageText = null;

            var nameProp = TypeDescriptor.GetProperties(page)["Name"];
            if (nameProp != null && nameProp.PropertyType == typeof(string))
            {
                pageText = (string)nameProp.GetValue(page);
            }

            if (pageText != null)
            {
                var textProperty = TypeDescriptor.GetProperties(page)["Text"];
                Debug.Assert(textProperty != null, "Could not find 'Text' property in Panel.");
                textProperty.SetValue(page, pageText);
            }

            var styleProp = TypeDescriptor.GetProperties(page)["UseVisualStyleBackColor"];
            if (styleProp != null && styleProp.PropertyType == typeof(bool) && !styleProp.IsReadOnly && styleProp.IsBrowsable)
            {
                styleProp.SetValue(page, true);
            }

            menu.Pages.Add(page);
            // Make sure that the last tab is selected. 
            menu.TreeMenu.SelectedNode = menu.TreeMenu.Nodes[menu.TreeMenu.Nodes.Count - 1];
            if (!addingOnInitialize) 
                RaiseComponentChanged(member, null, null);
        }
        finally
        {
            t?.Commit();
        }
    }


    private void OnComponentChanged(object sender, ComponentChangedEventArgs e)
    {
        CheckVerbStatus();
    }


    private void OnGotFocus(object sender, EventArgs e)
    {
        var eventSvc = (IEventHandlerService)GetService(typeof(IEventHandlerService));
        if (eventSvc == null) return;

        var focusWnd = eventSvc.FocusWindow;
        focusWnd?.Focus();
    }

    ///  
    /// 
    ///      This is called in response to a verb to remove a tab.  It removes
    ///      the current tab.
    ///  
    private void OnRemove(object sender, EventArgs eevent)
    {
        var tc = (ExVerticalMenu)Component;

        // if the control is null, or there are not tab pages, get out!...
        // 
        if (tc == null || tc.TreeMenu.Nodes.Count == 0)
        {
            return;
        }

        // member is OK to be null...
        // 
        MemberDescriptor member = TypeDescriptor.GetProperties(Component)["Pages"];

        var tp = tc.SelectedPage;

        // destroy the page
        //
        var host = (IDesignerHost)GetService(typeof(IDesignerHost));
        if (host == null) return;

        DesignerTransaction t = null;
        try
        {
            try
            {
                t = host.CreateTransaction("Remove MenuItem");
                RaiseComponentChanging(member);
            }
            catch (CheckoutException ex)
            {
                if (ex == CheckoutException.Canceled)
                {
                    return;
                }
                throw;
            }

            host.DestroyComponent(tp);

            RaiseComponentChanged(member, null, null);
        }
        finally
        {
            t?.Commit();
        }
    }


    protected override void OnPaintAdornments(PaintEventArgs pe)
    {
        try
        {
            disableDrawGrid = true;

            // we don't want to do this for the tab control designer
            // because you can't drag anything onto it anyway. 
            // so we will always return false for draw grid.
            base.OnPaintAdornments(pe);

        }
        finally
        {
            disableDrawGrid = false;
        }
    }

    private void OnControlAdded(object sender, ControlEventArgs e)
    {
        if (e.Control is { IsHandleCreated: false })
        {
            var hwnd = e.Control.Handle;
        } //TODO ?
    }

    ///  
    /// 
    ///      Called when the current selection changes.  Here we check to 
    ///      see if the newly selected component is one of our tabs.  If it 
    ///      is, we make sure that the tab is the currently visible tab.
    ///  
    private void OnSelectionChanged(object sender, EventArgs e)
    {
        var svc = (ISelectionService)GetService(typeof(ISelectionService));

        tabControlSelected = false;//this is for HitTest purposes 

        if (svc == null) return;

        var selComponents = svc.GetSelectedComponents();

        var tabControl = (ExVerticalMenu)Component;

        foreach (object comp in selComponents)
        {

            if (comp == tabControl) 
                tabControlSelected = true; //this is for HitTest purposes

            var page = GetTabPageOfComponent(comp);

            if (page == null || page.Parent != tabControl) 
                continue;

            tabControlSelected = false; //this is for HitTest purposes
            //tabControl.SelectedPage = page;
            //TODO uncomment
            //var selMgr = (SelectionManager)GetService(typeof(SelectionManager));
            //selMgr.Refresh();
            break;
        }
    }

    /// 
    ///  
    ///      Called when the selected tab changes.  This accesses the design
    ///      time selection service to surface the new tab as the current 
    ///      selection. 
    /// 
    private void OnTabSelectedIndexChanged(object sender, EventArgs e)
    {

        // if this was called as a result of a prop change, don't set the
        // selection to the control (causes flicker)
        // 
        // Attempt to select the tab control
        var svc = (ISelectionService)GetService(typeof(ISelectionService));
        if (svc == null) return;
        var selComponents = svc.GetSelectedComponents();

        var tabControl = (ExVerticalMenu)Component;
        bool selectedComponentOnTab = false;

        foreach (object comp in selComponents)
        {
            var page = GetTabPageOfComponent(comp);
            if (page != null && page.Parent == tabControl && page == tabControl.SelectedPage)
            {
                selectedComponentOnTab = true;
                break;
            }
        }

        if (!selectedComponentOnTab)
        {
            svc.SetSelectedComponents(new object[] { Component });
        }
    }

    protected override void PreFilterProperties(IDictionary properties)
    {
        base.PreFilterProperties(properties);

        // Handle shadowed properties
        string[] shadowProps = { "SelectedNode" };

        Attribute[] empty = Array.Empty<Attribute>();

        foreach (var t in shadowProps)
        {
            var prop = (PropertyDescriptor)properties[t];
            if (prop != null)
            {
                properties[t] = TypeDescriptor.CreateProperty(typeof(ExTreeViewDesigner), prop, empty);
            }
        }
    }

    private TabPageDesigner GetSelectedTabPageDesigner()
    {
        TabPageDesigner pageDesigner = null;
        var selectedTab = ((ExVerticalMenu)Component).SelectedPage;
        if (selectedTab == null) 
            return null;

        var host = (IDesignerHost)GetService(typeof(IDesignerHost));
        if (host != null)
        {
            pageDesigner = host.GetDesigner(selectedTab) as TabPageDesigner;
        }
        return pageDesigner;
    }

    ///  
    /// 
    ///      Called in response to a drag enter for OLE drag and drop.  This method is overriden 
    ///      so that we can forward the OnDragEnter event to the currently selected TabPage. 
    ///
    //TODO uncomment
    protected override void OnDragEnter(DragEventArgs de)
    {

        /*// Check what we are dragging... If we are just dragging tab pages,
        // then we do not want to forward the OnDragXXX

        forwardOnDrag = false;
        
        var data = de.Data;//as DropSourceBehavior.BehaviorDataObject;
        if (data != null)
        {
            int primaryIndex = -1;
            var dragControls = data.GetSortedDragControls(ref primaryIndex);
            if (dragControls != null)
            {
                foreach (var t in dragControls)
                {
                    if (t is not (System.Windows.Forms.Control or TabPage)) 
                        continue;

                    forwardOnDrag = true;
                    break;
                }
            }
        }
        else
        {
            // We must be dragging something off the toolbox, so forward the drag to the right tabpage.
            forwardOnDrag = true;
        }

        if (forwardOnDrag)
        {
            var pageDesigner = GetSelectedTabPageDesigner();
            if (pageDesigner != null)
            {
                pageDesigner.OnDragEnterInternal(de);
            }
        }
        else
        {
            base.OnDragEnter(de);
        }*/
    }

    /// 
    /// 
    ///      Called in response to a drag enter for OLE drag and drop.  This method is overriden 
    ///      so that we can forward the OnDragEnter event to the currently selected TabPage.
    ///  
    protected override void OnDragDrop(DragEventArgs de)
    {
        if (forwardOnDrag)
        {
            var pageDesigner = GetSelectedTabPageDesigner();
            pageDesigner?.OnDragDropInternal(de);
        }
        else
        {
            base.OnDragDrop(de);
        }

        forwardOnDrag = false;
    }

    /// 
    ///  
    ///      Called in response to a drag leave for OLE drag and drop.  This method is overriden
    ///      so that we can forward the OnDragLeave event to the currently selected TabPage. 
    ///  
    protected override void OnDragLeave(EventArgs e)
    {
        if (forwardOnDrag)
        {
            var pageDesigner = GetSelectedTabPageDesigner();
            pageDesigner?.OnDragLeaveInternal(e);
        }
        else
        {
            base.OnDragLeave(e);
        }

        forwardOnDrag = false;
    }

    ///  
    /// 
    ///      Called in response to a drag over for OLE drag and drop.  This method is overriden 
    ///      so that we can forward the OnDragOver event to the currently selected TabPage. 
    /// 
    protected override void OnDragOver(DragEventArgs de)
    {
        if (forwardOnDrag)
        {
            //Need to make sure that we are over a valid area.
            //VSWhidbey# 354139. Now that all dragging/dropping is done via
            //the behavior service and adorner window, we have to do our own 
            //validation, and cannot rely on the OS to do it for us.
            var tc = (ExVerticalMenu)Control;
            var dropPoint = Control.PointToClient(new Point(de.X, de.Y));
            if (!tc.DisplayRectangle.Contains(dropPoint))
            {
                de.Effect = DragDropEffects.None;
                return;
            }

            var pageDesigner = GetSelectedTabPageDesigner();
            pageDesigner?.OnDragOverInternal(de);
        }
        else
        {
            base.OnDragOver(de);
        }
    }

    /// 
    ///  
    ///      Called in response to a give feedback for OLE drag and drop.  This method is overriden 
    ///      so that we can forward the OnGiveFeedback event to the currently selected TabPage.
    ///  
    protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
    {
        if (forwardOnDrag)
        {
            var pageDesigner = GetSelectedTabPageDesigner();
            pageDesigner?.OnGiveFeedbackInternal(e);
        }
        else
        {
            base.OnGiveFeedback(e);
        }
    }

    ///  
    /// 
    ///      Overrides control designer's wnd proc to handle HTTRANSPARENT. 
    ///  
    protected override void WndProc(ref Message m)
    {
        switch ((Win32.WM)m.Msg)
        {
            case Win32.WM.NCHITTEST:
                // The tab control always fires HTTRANSPARENT in empty areas, which
                // causes the message to go to our parent.  We want
                // the tab control's designer to get these messages, however, 
                // so change this.
                // 
                base.WndProc(ref m);
                if ((int)m.Result == -1) //HTTRANSPARENT
                {
                    m.Result = (IntPtr)HTCLIENT;
                }
                break;
            case Win32.WM.CONTEXTMENU:
                // We handle this in addition to a right mouse button. 
                // Why?  Because we often eat the right mouse button, so
                // it may never generate a WM_CONTEXTMENU.  However, the 
                // system may generate one in response to an F-10. 
                //
                int x = Win32.SignedLOWORD((int)m.LParam);
                int y = Win32.SignedHIWORD((int)m.LParam);
                if (x == -1 && y == -1)
                {
                    // for shift-F10
                    var p = Cursor.Position;
                    x = p.X;
                    y = p.Y;
                }
                OnContextMenu(x, y);
                break;
            case Win32.WM.HSCROLL:
            case Win32.WM.VSCROLL:
                //We do this so that we can update the areas covered by glyphs correctly. VSWhidbey# 187405.
                //We just invalidate the area corresponding to the ClientRectangle in the adornerwindow. 
                BehaviorService?.Invalidate(BehaviorService.ControlRectInAdornerWindow(Control));
                base.WndProc(ref m);
                break;
            default:
                base.WndProc(ref m);
                break;
        }
    }
}

internal class TabPageDesigner : PanelDesigner
{
    public override bool CanBeParentedTo(IDesigner parentDesigner) => parentDesigner is { Component: ExVerticalMenu };

    public override SelectionRules SelectionRules
    {
        get
        {
            var selectionRules = base.SelectionRules;
            var control = Control;
            if (control is { Parent: ExVerticalMenu })
                selectionRules &= ~SelectionRules.AllSizeable;
            return selectionRules;
        }
    }

    internal void OnDragDropInternal(DragEventArgs de) => OnDragDrop(de);

    internal void OnDragEnterInternal(DragEventArgs de) => OnDragEnter(de);

    internal void OnDragLeaveInternal(EventArgs e) => OnDragLeave(e);

    internal void OnDragOverInternal(DragEventArgs e) => OnDragOver(e);

    internal void OnGiveFeedbackInternal(GiveFeedbackEventArgs e) => OnGiveFeedback(e);

    protected override ControlBodyGlyph GetControlGlyph(GlyphSelectionType selectionType)
    {
        OnSetCursor();
        return new ControlBodyGlyph(Rectangle.Empty, Cursor.Current, Control, this);
    }
}

internal class PanelDesigner : ScrollableControlDesigner
{
    
}

internal interface IEventHandlerService
{
    event EventHandler EventHandlerChanged;

    Control FocusWindow { get; }

    object GetHandler(Type handlerType);

    void PopHandler(object handler);

    void PushHandler(object handler);
}