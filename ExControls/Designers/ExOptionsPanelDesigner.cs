using System.Collections;
using System.ComponentModel.Design;
using System.Drawing.Design;
using ExControls.Editors;

#if NETFRAMEWORK
using System.Windows.Forms.Design;
#else
using Microsoft.DotNet.DesignTools.Designers;
using Microsoft.DotNet.DesignTools.Designers.Actions;
#endif

namespace ExControls.Designers;

/// <summary>
/// Provides a ParentControlDesigner for the ExOptionsPanel to enhance design-time experience.
/// </summary>
internal sealed class ExOptionsPanelDesigner : DesignerScrollableControlBase<ExOptionsPanel>
{
    private readonly string[] _invisibleProperties;
    private DesignerActionListCollection _actionLists;

    public ExOptionsPanelDesigner()
    {
        _invisibleProperties = new[]
        {
            "Anchor", "AutoSize", "AutoSizeMode", "Dock", "GenerateMember", "Location", "MaximumSize", "MinimumSize",
            "Size", "TabIndex", "TabStop", "Visible", "Owner"
        };
    }

    // We don't want to allow moving or resizing the panel
    public override SelectionRules SelectionRules => SelectionRules.Locked;

    public override DesignerActionListCollection ActionLists => _actionLists ??= new DesignerActionListCollection { new OptionsPanelActionList(ControlHost) };

    // Copied from the PanelDesigner class
    private Pen BorderPen
    {
        get
        {
            // Get a slightly lighter or darker color than the backcolor, depending on the brightness
            var color = ControlHost.BackColor.GetBrightness() < 0.5d ? ControlPaint.Light(ControlHost.BackColor) : ControlPaint.Dark(ControlHost.BackColor);

            var p = new Pen(color);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            return p;
        }
    }

    protected override void OnPaintAdornments(PaintEventArgs pe)
    {
        base.OnPaintAdornments(pe);

        // Draw a border
        OnPaintBorder(pe);
    }

    // Paint dashed border around control during design-time, copied from the PanelDesigner
    private void OnPaintBorder(PaintEventArgs pe)
    {
        if (ControlHost is not { Visible: true })
            return;
        using var p = BorderPen;
        var rect = ControlHost.ClientRectangle;
        rect.Inflate(-2, -2);
        pe.Graphics.DrawRectangle(p, rect);
    }

    protected override void PreFilterProperties(IDictionary properties)
    {
        base.PreFilterProperties(properties);

        // Hide some properties
        foreach (var prop in _invisibleProperties)
            properties.Remove(prop);
    }

    internal class OptionsPanelActionList : DesignerActionListBase<ExOptionsPanel>
    {
        public OptionsPanelActionList(ExOptionsPanel host) : base(host)
        {
        }

        [Editor(typeof(OptionsNodeEditor), typeof(UITypeEditor))]
        public OptionsNode ParentNode
        {
            get => Host.ParentNode;

            set
            {
                SetProperty(nameof(ParentNode), value);
                DesignerActionService.Refresh(Host);
            }
        }

        protected void SelectParent()
        {
            Component parent;
            var owner = Host.Owner;
            if (owner is null)
                return;

            // If this panel has a ParentNode, select the corresponding 'parent' panel.
            // If not, select the host ExOptionsView
            if (Host.ParentNode is null)
                parent = owner;
            else
            {
                parent = Host.ParentNode.Panel;
                owner.SelectedPanel = (ExOptionsPanel)parent;
            }

            // Select the new parent in the designer
            SelectionService.SetSelectedComponents(new[] { parent }, SelectionTypes.Auto);

            // Refresh the ActionList popup so that it corresponds to the newly selected parent
            DesignerActionService.Refresh(parent);
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            var items = new DesignerActionItemCollection
            {
                new DesignerActionMethodItem(this, nameof(SelectParent), "Select Parent", "", "Selects the parent ExOptionsPanel or ExOptionsView.", true),
                new DesignerActionPropertyItem(nameof(ParentNode), "Parent node:", "", "Gets or sets the parent node.")
            };
            return items;
        }
    }
}