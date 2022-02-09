using System.Collections;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;
using ExControls.Collections;
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


        var tc = (ExVerticalMenu)Component;
        if (tc != null) tc.TreeMenu.SelectedNode = tc.Pages[0];
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
                new DesignerVerb("Add page", OnAdd)
            };

            var ctl = (ExVerticalMenu)Control;
            onSelect.Enabled = ctl.Panels.Length > 0;

            return verbs;
        }
    }

    private void OnEdit(object sender, EventArgs e)
    {
        
    }

    private void OnAdd(object sender, EventArgs e)
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
                t = host.CreateTransaction("Add page");
            }
            catch (CheckoutException ex)
            {
                if (ex == CheckoutException.Canceled)
                    return;
                throw;
            }
            MemberDescriptor member = TypeDescriptor.GetProperties(menu)["Pages"];
            var page = (MenuPanel)host.CreateComponent(typeof(MenuPanel), "panel" + ExControl.Pages.Count);
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

    private void OnSelect(object sender, EventArgs e)
    {
        /*if (Control is not ExVerticalMenu ctl || ctl.Panels.Length == 0)
            return;

        using var selectPage = new FSelectPage(ctl.panels, ctl.SelectedPage);
        var result = selectPage.ShowDialog();
        if (result == DialogResult.OK)
        {
            ctl.SelectedPage = selectPage.SelectedPage;
        }*/
    }

    public override bool CanParent(Control control)
    {
        return (control is MenuPanel && !ExControl.RootPanel.Controls.Contains(control));
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