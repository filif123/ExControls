using System.ComponentModel.Design;
using System.Drawing.Design;
using ExControls.Designers;
using ExControls.Editors;

namespace ExControls;

/// <summary>
/// Represents a panel in the ExOptionsView containing one set of options.
/// </summary>
[ToolboxItem(false)]
[Designer(typeof(ExOptionsPanelDesigner))]
public class ExOptionsPanel : Panel
{
    private ExOptionsView _owner;
    private OptionsNode _node;
    private OptionsNode _parentNode;

    /// <inheritdoc />
    public ExOptionsPanel()
    {
        base.Dock = DockStyle.Fill;
        base.DoubleBuffered = true;
    }

    #region  Properties

    /// <summary>
    /// Gets the ExOptionsView to which this panel belongs
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
    public ExOptionsView Owner
    {
        // Just in case the Owner has not been set, let's try to find it ourselves
        get => _owner ??= FindOwner();
        private set => _owner = value;
    }

    /// <summary>
    /// Gets or sets the node corresponding to this panel. Should not be used during run-time.
    /// </summary>
    [Category("Nodes")]
    [Description("The OptionsNode that corresponds to this panel.")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public OptionsNode Node
    {
        get => _node;
        set
        {
            _node = value;
            if (_node != null)
                value.Panel = this;
        }
    }

    /// <summary>
    /// Gets or sets the parent OptionsNode. Set this to create child option panels.
    /// </summary>
    [Category("Nodes")]
    [Description("The parent node for the node corresponding to this panel. Set to create child option panels.")]
    [Editor(typeof(OptionsNodeEditor), typeof(UITypeEditor))]
    public OptionsNode ParentNode
    {
        get => _parentNode;
        set
        {
            if (value != null && ReferenceEquals(value, Node))
                throw new Exception("ParentNode cannot be the same as the current Node!");
            if (value != null && value.Panel.ParentNode == value) //TODO ? 
                value.Panel.ParentNode = null;

            _parentNode = value;

            // Remove and Add the node again during design-time to 'refresh' the nodes
            if (DesignMode && Owner != null)
            {
                Owner.PanelContainer.Controls.Remove(this);
                Owner.PanelContainer.Controls.Add(this);
            }
        }
    }

    /// <summary>
    /// Gets or sets the text of the node.
    /// </summary>
    [Category("Nodes")]
    [Description("The text displayed in the node.")]
    public string NodeText
    {
        get => Node.Text;
        set => Node.Text = value;
    }

    #endregion

    #region  Methods 

    internal static object CreatePanel(ExOptionsView owner, IDesignerHost designerHost)
    {
        // Use the IDesignerHost to create the panel component so that it is editable (and selectable) during design-time
        var panel = (ExOptionsPanel) designerHost.CreateComponent(typeof(ExOptionsPanel));

        // Also set some properties
        panel.Owner = owner;
        panel.Dock = DockStyle.Fill;

        // Create node
        var node = new OptionsNode
        {
            Panel = panel
        };
        panel.Node = node;
        return panel;
    }

    // Keep walking up the tree of parent controls until we reach an ExOptionsView.
    private ExOptionsView FindOwner()
    {
        var parent = Parent;
        while (parent != null)
        {
            parent = parent.Parent;
            if (parent is ExOptionsView view)
            {
                // Found it
                return view;
            }
        }

        return null;
    }

    // Tell the designer to serialize the Node and ParentNode properties to the designer file.
    private bool ShouldSerializeNode() => true;

    private bool ShouldSerializeParentNode() => true;

    #endregion

    /// <summary>
    ///     Detects if there is a control in the panel with the specified text.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public bool Search(string text)
    {
        if (NodeText.Contains(text))
            return true;
        
        foreach (Control ctrl in Controls)
        {
            if (Search(text, ctrl))
                return true;
        }
        return false;
    }

    private static bool Search(string text, Control control)
    {
        if (control.Text.Contains(text))
            return true;
        foreach (Control ctrl in control.Controls)
        {
            if (Search(text, ctrl))
                return true;
        }
        return false;
    }
}