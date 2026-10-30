using System.ComponentModel.Design;
using System.Drawing.Design;
using ExControls.Converters;
using ExControls.Editors;

namespace ExControls;

/// <summary>
/// Represents a panel in the ExOptionsView containing one set of options.
/// </summary>
[ToolboxItem(false)]
[Designer("ExControls.Designers.ExOptionsPanelDesigner, ExControls")]
[TypeConverter(typeof(ExOptionsPanelConverter))]
public class ExOptionsPanel : Panel
{
    private OptionsNode? _node;

    private readonly List<Control> _linkCollection;

    // original background colors of the controls highlighted by the search; Color.Empty = inherited from the parent
    private readonly Dictionary<Control, Color> _highlighted = new();

    /// <summary>
    /// Occurs when the <see cref="ParentNode" /> property is set.
    /// </summary>
    public event EventHandler? ParentNodeChanged;

    /// <summary>
    /// Occurs when another panel sets its <see cref="ParentNode" /> to the node of this panel.
    /// </summary>
    public event EventHandler? ChildrenChanged;

    /// <summary>
    /// Occurs when the <see cref="NodeText" /> property changes.
    /// </summary>
    public event EventHandler? NodeTextChanged;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExOptionsPanel" /> class that fills its parent; the owning view is looked
    /// up among the parent controls.
    /// </summary>
    public ExOptionsPanel()
    {
        Size = ClientSize;
        Location = Point.Empty;
        base.Dock = DockStyle.Fill;
        base.DoubleBuffered = true;

        _linkCollection = new List<Control>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExOptionsPanel" /> class that belongs to the specified view.
    /// </summary>
    /// <param name="owner">The view to which the panel belongs.</param>
    public ExOptionsPanel(ExOptionsView owner) : this()
    {
        Owner = owner;
    }

    #region  Properties

    /// <summary>
    /// Gets the <see cref="ExOptionsView" /> to which this panel belongs.
    /// </summary>
    /// <value>
    /// The owning view, looked up among the parent controls if it was not set;
    /// <see langword="null" /> if the panel is not placed in a view.
    /// </value>
    public ExOptionsView? Owner
    {
        // Just in case the Owner has not been set, let's try to find it ourselves
        get => (field ??= FindOwner())!;
        private set;
    }

    /// <summary>
    /// Gets or sets the node corresponding to this panel. Should not be used during run-time.
    /// </summary>
    [ExCategory("Nodes")]
    [ExDescription("The OptionsNode that corresponds to this panel.")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public OptionsNode Node
    {
        get => _node!;
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
    [ExCategory("Nodes")]
    [ExDescription("The parent node for the node corresponding to this panel. Set to create child option panels.")]
    [Editor(typeof(OptionsNodeEditor), typeof(UITypeEditor))]
    public OptionsNode? ParentNode
    {
        get;
        set
        {
            if (value != null && ReferenceEquals(value, Node))
                throw new InvalidOperationException("ParentNode cannot be the same as the current Node!");

            if (value is not null && value.Panel is not null && value.Panel.ParentNode == value)
                value.Panel.ParentNode = null;

            field = value;
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
    /// Gets or sets whether the panel shows links to its child panels.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [ExDescription("", true)]
    [DefaultValue(false)]
    public bool GenerateLinksToChildren
    {
        get;
        set
        {
            if (value == field)
                return;
            field = value;
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
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
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
    /// Generates link to panel if GenerateLinksToChildren is true.
    /// </summary>
    public virtual void GenerateLinks()
    {
        RemoveLinks(); //remove all previous links

        var location = new Point(5, 5);
        var spacing = GetFontHeight(Font) + 10;

        SuspendLayout();
        foreach (var child in Owner!.Panels.Cast<ExOptionsPanel>().Where(p => p.ParentNode?.Panel == this))
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
    /// Removes links in panel if GenerateLinksToChildren is true.
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
    private ExOptionsView? FindOwner()
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
#pragma warning disable CA1822 // Mark members as static
    
    private bool ShouldSerializeNode() => true;


    private bool ShouldSerializeParentNode() => true;
    
#pragma warning restore CA1822 // Mark members as static

    #endregion

    /// <summary>
    /// Determines whether a control in the panel (including nested controls) or the node text contains the specified text.
    /// </summary>
    /// <param name="text">The text to search for.</param>
    /// <returns><see langword="true" /> if the text was found; otherwise, <see langword="false" />.</returns>
    public bool Search(string text)
    {
        foreach (Control ctl in Controls)
        {
            if (ctl is ISearchable searchable && searchable.Search(text))
                return true;

            if (Search(text, ctl))
                return true;
        }
        return NodeText.Contains(text, StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool Search(string text, Control control)
    {
        if (TextMatches(control, text))
            return true;
        return control.Controls.Cast<Control>().Any(ctrl => Search(text, ctrl));
    }

    /// <summary>
    /// Highlights the labels, check boxes and radio buttons in the panel (including nested controls) whose text contains the
    /// specified text and removes the previous highlight. An empty text only removes the highlight.
    /// </summary>
    /// <param name="text">The text to search for.</param>
    /// <param name="color">The background color of the highlighted controls.</param>
    /// <returns>The number of highlighted controls.</returns>
    public int HighlightMatches(string? text, Color color)
    {
        ClearHighlight();
        if (text is null || string.IsNullOrWhiteSpace(text))
            return 0;

        foreach (var control in Descendants(this).Where(c => (c is Label or CheckBox or RadioButton) && TextMatches(c, text)))
        {
            // a color inherited from the parent is restored as inherited (Color.Empty), not as a fixed color
            var inherited = TypeDescriptor.GetProperties(control)[nameof(BackColor)]?.ShouldSerializeValue(control) != true;
            _highlighted[control] = inherited ? Color.Empty : control.BackColor;
            control.BackColor = color;
        }

        return _highlighted.Count;
    }

    private void ClearHighlight()
    {
        foreach (var pair in _highlighted.Where(pair => !pair.Key.IsDisposed))
            pair.Key.BackColor = pair.Value;
        _highlighted.Clear();
    }

    // mnemonic ampersands are not part of the visible text
    private static bool TextMatches(Control control, string text) =>
        control.Text.Replace("&", "").Contains(text, StringComparison.CurrentCultureIgnoreCase);

    private static IEnumerable<Control> Descendants(Control parent)
    {
        foreach (Control child in parent.Controls)
        {
            yield return child;
            foreach (var nested in Descendants(child))
                yield return nested;
        }
    }

    /// <summary>
    /// Raises the <see cref="ParentNodeChanged" /> event.
    /// </summary>
    protected virtual void OnParentNodeChanged()
    {
        ParentNodeChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Raises the <see cref="ChildrenChanged" /> event and regenerates the links if <see cref="GenerateLinksToChildren" /> is
    /// <see langword="true" />.
    /// </summary>
    protected virtual void OnChildrenChanged()
    {
        ChildrenChanged?.Invoke(this, EventArgs.Empty);
        if (GenerateLinksToChildren) GenerateLinks();
    }

    /// <summary>
    /// Raises the <see cref="NodeTextChanged" /> event.
    /// </summary>
    protected virtual void OnNodeTextChanged()
    {
        NodeTextChanged?.Invoke(this, EventArgs.Empty);
    }
}