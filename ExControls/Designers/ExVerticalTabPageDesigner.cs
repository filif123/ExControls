using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;

namespace ExControls.Designers;

/// <summary>
///     This is the designer for tap page controls.  It inherits 
/// from the base control designer and adds live hit testing 
/// capabilites for the tree view control.
/// </summary>
internal class ExVerticalTabPageDesigner : PanelDesigner
{
    /// <summary>
    ///     Determines if the this designer can be parented to the specified desinger --
    /// generally this means if the control for this designer can be parented into the 
    /// given ParentControlDesigner's designer.
    /// </summary>
    /// <param name="parentDesigner"></param>
    /// <returns></returns>
    public override bool CanBeParentedTo(IDesigner parentDesigner)
    {
        return base.CanBeParentedTo(parentDesigner);
        //return parentDesigner is ExVerticalTabControlDesigner;
    }

    public override bool CanParent(Control control)
    {
        return true;
    }

    /// <summary>
    ///     Retrieves a set of rules concerning the movement capabilities of a component. 
    /// This should be one or more flags from the SelectionRules class.  If no designer 
    /// provides rules for a component, the component will not get any UI services.
    /// </summary>
    public override SelectionRules SelectionRules
    {
        get
        {
            var rules = base.SelectionRules;
            var ctl = Control;

            if (ctl.Parent is ExVerticalTabControl)
            {
                rules = SelectionRules.Locked;
                //rules &= ~SelectionRules.AllSizeable;
            }
            
            return rules;
        }
    }

    protected override ControlBodyGlyph GetControlGlyph(GlyphSelectionType selectionType)
    {
        // create a new body glyph with empty bounds.
        // this will keep incorrect tab pages from stealing drag/drop messages 
        // which are now handled by the TabControlDesigner

        //get the right cursor for this component
        OnSetCursor();

        var translatedBounds = Rectangle.Empty;

        //create our glyph, and set its cursor appropriately
        var g = new ControlBodyGlyph(translatedBounds, Cursor.Current, Control, this);

        return g;
    }
} 