using System.ComponentModel.Design;
using System.Drawing.Design;
using ExControls.Converters;
using ExControls.Designers;
using ExControls.Editors;

namespace ExControls;

/// <summary>
///     Represents a panel in the ExOptionsView containing one set of options.
/// </summary>
[ToolboxItem(false)]
[Designer(typeof(ExOptionsPanelDesigner))]
[TypeConverter(typeof(ExOptionsPanelConverter))]
public class ExOptionsPanel : Panel
{
    private ExOptionsView _owner;
    private OptionsNode _node;
    private OptionsNode _parentNode;
    private bool _generateLinksToChildren;

    private readonly List<Control> _linkCollection;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler ParentNodeChanged;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler ChildrenChanged;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler NodeTextChanged;

    /// <inheritdoc />
    public ExOptionsPanel()
    {
        Size = ClientSize;
        Location = Point.Empty;
        base.Dock = DockStyle.Fill;
        base.DoubleBuffered = true;

        _linkCollection = new List<Control>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="owner"></param>
    public ExOptionsPanel(ExOptionsView owner) : this()
    {
        _owner = owner;
    }

    #region  Properties

    /// <summary>
    ///     Gets the ExOptionsView to which this panel belongs
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
    ///     Gets or sets the node corresponding to this panel. Should not be used during run-time.
    /// </summary>
    [ExCategory("Nodes")]
    [ExDescription("The OptionsNode that corresponds to this panel.")]
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
    ///     Gets or sets the parent OptionsNode. Set this to create child option panels.
    /// </summary>
    [ExCategory("Nodes")]
    [ExDescription("The parent node for the node corresponding to this panel. Set to create child option panels.")]
    [Editor(typeof(OptionsNodeEditor), typeof(UITypeEditor))]
    public OptionsNode ParentNode
    {
        get => _parentNode;
        set
        {
            if (value != null && ReferenceEquals(value, Node))
                throw new InvalidOperationException("ParentNode cannot be the same as the current Node!");

            if (value is not null && value.Panel is not null && value.Panel.ParentNode == value) 
                value.Panel.ParentNode = null;

            _parentNode = value;
            OnParentNodeChanged();

            value?.Panel?.OnChildrenChanged();

            if (GenerateLinksToChildren) 
                GenerateLinks();

            // Remove and Add the node again during design-time to 'refresh' the nodes
            if (DesignMode && Owner != null)
            {
                Owner.PanelContainer.SuspendLayout();
                Owner.PanelContainer.Controls.Remove(this);
                Owner.PanelContainer.Controls.Add(this);
                Owner.PanelContainer.ResumeLayout();
            }
        }
    }

    /// <summary>
    ///     Gets or sets
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("",true)]
    [DefaultValue(false)]
    public bool GenerateLinksToChildren
    {
        get => _generateLinksToChildren;
        set
        {
            if (value == _generateLinksToChildren)
                return;
            _generateLinksToChildren = value;
            if (value)
                GenerateLinks();
            else
                RemoveLinks();
        }
    }

    /// <summary>
    /// Gets or sets the text of the node.
    /// </summary>
    [ExCategory("Nodes")]
    [Description("The text displayed in the node.")]
    public string NodeText
    {
        get => Node.Text;
        set
        {
            if (Node.Text == value)
                return;
            Node.Text = value;
            OnNodeTextChanged();
        }
    }

    #endregion

    #region  Methods

    /// <summary>
    ///     Generates link to panel if GenerateLinksToChildren is true.
    /// </summary>
    public virtual void GenerateLinks()
    {
        RemoveLinks(); //remove all previous links

        var location = new Point(5, 5);
        var spacing = GetFontHeight(Font) + 10;

        SuspendLayout();
        foreach (var child in Owner.Panels.Cast<ExOptionsPanel>().Where(p => p.ParentNode?.Panel == this))
        {
            var linkLabel = new LinkLabel();
            linkLabel.Text = child.NodeText;
            linkLabel.Location = location;
            linkLabel.LinkColor = Owner.LinkToChildrenForeColor;
            linkLabel.LinkClicked += (_, _) => Owner.SelectedPanel = child;
            child.NodeTextChanged += (_, _) => linkLabel.Text = child.NodeText;

            _linkCollection.Add(linkLabel);
            Controls.Add(linkLabel);
            location.Y += spacing;
        }
        ResumeLayout(true);
    }

    private int GetFontHeight(Font fnt)
    {
        using var g = CreateGraphics();
        var size = g.MeasureString("ATQj", fnt, 495);
        return (int) Math.Ceiling(size.Height);
    }

    /// <summary>
    ///     Removes links in panel if GenerateLinksToChildren is true.
    /// </summary>
    public virtual void RemoveLinks()
    {
        foreach (var link in _linkCollection) 
            Controls.Remove(link);

        _linkCollection.Clear();
    }

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
                return view; // Found it
        }

        return null;
    }

    // Tell the designer to serialize the Node and ParentNode properties to the designer file.
#pragma warning disable S1144 // Unused private types or members should be removed
#pragma warning disable S3400 // Methods should not return constants

    private bool ShouldSerializeNode() => true;

    private bool ShouldSerializeParentNode() => true;

#pragma warning restore S3400 // Methods should not return constants
#pragma warning restore S1144 // Unused private types or members should be removed

    #endregion

    /// <summary>
    ///     Detects if there is a control in the panel with the specified text.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public bool Search(string text)
    {
        foreach (Control ctl in Controls)
        {
            if (ctl is ISearchable searchable && searchable.Search(text))
                return true;

            if (Search(text, ctl))
                return true;
        }
        return NodeText.Contains(text);
    }

    private static bool Search(string text, Control control)
    {
        if (control.Text.Contains(text, StringComparison.CurrentCultureIgnoreCase))
            return true;
        return control.Controls.Cast<Control>().Any(ctrl => Search(text, ctrl));
    }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void OnParentNodeChanged()
    {
        ParentNodeChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void OnChildrenChanged()
    {
        ChildrenChanged?.Invoke(this, EventArgs.Empty);
        if (GenerateLinksToChildren) GenerateLinks();
    }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void OnNodeTextChanged()
    {
        NodeTextChanged?.Invoke(this, EventArgs.Empty);
    }
}