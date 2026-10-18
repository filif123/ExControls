using System.Collections;
#if NETFRAMEWORK
using System.ComponentModel.Design;
#else
using Microsoft.DotNet.DesignTools.Designers.Actions;
#endif

namespace ExControls.Designers;

/// <summary>
/// Designer of <see cref="ExTitleBar" />: the title bar belongs directly onto the form and the form has only one, a new one
/// goes to the top of the form (above the other controls docked to the top) and the smart tag adds a main menu to it.
/// </summary>
internal sealed class ExTitleBarDesigner : DesignerParentControlBase<ExTitleBar>
{
    private DesignerActionListCollection? _actionLists;

    public override DesignerActionListCollection ActionLists => _actionLists ??= new DesignerActionListCollection
    {
        new TitleBarActionList(ControlHost, this)
    };

    /// <inheritdoc />
    public override void InitializeNewComponent(IDictionary? defaultValues)
    {
        base.InitializeNewComponent(defaultValues);
        // ukotvene prvky sa ukladaju od posledneho v poradi - zahlavie na konci je navrchu okna;
        // druhe zahlavie (ak ho navrhar predsa vlozil) nesmie vytlacit prve
        if (IsOnlyTitleBar(ControlHost, ControlHost.Parent))
            ControlHost.SendToBack();
    }

    /// <inheritdoc />
    /// <remarks>A title bar inside a title bar makes no sense - only a title bar directly on the form replaces the system
    /// caption.</remarks>
    public override bool CanParent(Control control) => control is not ExTitleBar && base.CanParent(control);

    /// <inheritdoc />
    /// <remarks>Only directly onto a form that has no title bar yet.</remarks>
    public override bool CanBeParentedTo(System.ComponentModel.Design.IDesigner parentDesigner) =>
        parentDesigner.Component is Form form && IsOnlyTitleBar(ControlHost, form) && base.CanBeParentedTo(parentDesigner);

    /// <summary>
    /// The parent has no other title bar.
    /// </summary>
    private static bool IsOnlyTitleBar(ExTitleBar titleBar, Control? parent) =>
        parent is null || !parent.Controls.OfType<ExTitleBar>().Any(other => other != titleBar);

    /// <summary>
    /// Adds a new menu docked left into the title bar and sets it as the main menu of the form (Alt key).
    /// </summary>
    internal void AddMainMenu()
    {
        if (ControlHost.FindForm() is not { } form)
            return;

        using var transaction = DesignerHost.CreateTransaction("Add main menu");
        var menu = (MenuStrip)DesignerHost.CreateComponent(typeof(MenuStrip));
        var controls = TypeDescriptor.GetProperties(ControlHost)["Controls"];
        RaiseComponentChanging(controls);
        ControlHost.Controls.Add(menu);
        RaiseComponentChanged(controls, null, null);

        TypeDescriptor.GetProperties(menu)[nameof(MenuStrip.Dock)]?.SetValue(menu, DockStyle.Left);
        TypeDescriptor.GetProperties(menu)[nameof(MenuStrip.GripStyle)]?.SetValue(menu, ToolStripGripStyle.Hidden);
        TypeDescriptor.GetProperties(form)[nameof(Form.MainMenuStrip)]?.SetValue(form, menu);
        transaction.Commit();

        SelectionService.SetSelectedComponents(new Component[] { menu });
    }

    private sealed class TitleBarActionList(ExTitleBar host, ExTitleBarDesigner designer) : DesignerActionListBase<ExTitleBar>(host)
    {
        private const string Category = "Title bar";

        public bool ShowTitle
        {
            get => Host.ShowTitle;
            set
            {
                SetProperty(nameof(ShowTitle), value);
                DesignerActionService.Refresh(Host);
            }
        }

        public void AddMainMenu()
        {
            designer.AddMainMenu();
            DesignerActionService.Refresh(Host);
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            var items = new DesignerActionItemCollection
            {
                new DesignerActionHeaderItem(Category),
                new DesignerActionPropertyItem(nameof(ShowTitle), "Show form text", Category,
                    "Shows the text of the form in the free space of the title bar.")
            };

            var form = Host.FindForm();
            if (form is not ExForm exForm)
                items.Add(new DesignerActionTextItem("The form must derive from ExForm to replace the system caption.", Category));
            else if (exForm.TitleBar != Host)
                items.Add(new DesignerActionTextItem("The form already has a title bar, this one is not used. Delete it.", Category));
            else if (form.MainMenuStrip is null)
                items.Add(new DesignerActionMethodItem(this, nameof(AddMainMenu), "Add main menu", Category,
                    "Adds a MenuStrip into the title bar and sets it as the main menu of the form.", true));
            else if (form.MainMenuStrip.Parent != Host)
                items.Add(new DesignerActionTextItem("Drag the main menu into the title bar to show it in the caption.", Category));

            return items;
        }
    }
}
