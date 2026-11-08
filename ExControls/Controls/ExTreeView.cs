using ExControls.Collections;

namespace ExControls;

/// <summary>
/// Extended TreeView control: nodes can be hidden without removing them (<see cref="ExTreeNodeCollection" />) and the tree can
/// use the Explorer theme (<see cref="ExTreeView.Style" />).
/// </summary>
[Designer("ExControls.Designers.ExTreeViewDesigner, ExControls")]
#if NETFRAMEWORK
[ToolboxBitmap(typeof(TreeView), "TreeView.bmp")]
#else
[ToolboxBitmap(typeof(ToolboxIcons), "Toolbox.ExTreeView.ico")]
#endif
[DefaultProperty(nameof(Nodes))]
[DefaultEvent(nameof(AfterSelect))]
public class ExTreeView : TreeView
{
    private readonly ExTreeNodeCollection _nodes;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExTreeView" /> class.
    /// </summary>
    public ExTreeView()
    {
        _nodes = new ExTreeNodeCollection(base.Nodes);
    }

    /// <summary>
    /// Style of PlusMinus sign.
    /// </summary>
    [ExDescription("Style of PlusMinus sign.")]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(ExTreeViewStyle.Default)]
    public ExTreeViewStyle Style
    {
        get;
        set
        {
            if (field == value)
                return;
            field = value;
            this.SetTheme(StyleTheme);
        }
    }

    /// <summary>
    /// Tema Windows podla <see cref="Style" /> - vrati sa aj po vypnuti tmavych posuvnikov (<see cref="ExTools.SetDarkScrollBars" />).
    /// </summary>
    internal WindowsTheme StyleTheme => Style switch
    {
        ExTreeViewStyle.Light => WindowsTheme.Explorer,
        ExTreeViewStyle.Dark => WindowsTheme.DarkExplorer,
        ExTreeViewStyle.Old => WindowsTheme.None,
        _ => WindowsTheme.Default
    };

    /// <summary>
    /// Occurs when the new <see cref="TreeNode"/> was added to <see cref="TreeNodeCollection"/>.
    /// </summary>
    public event EventHandler<ExTreeViewNodeAddedEventArgs> TreeNodeAdded
    {
        add => _nodes.TreeNodeAdded += value;
        remove => _nodes.TreeNodeAdded -= value;
    }

    /// <summary>
    /// Occurs when a <see cref="TreeNode"/> was removed from <see cref="Nodes"/>.
    /// </summary>
    public event EventHandler<ExTreeViewNodeRemovedEventArgs> TreeNodeRemoved
    {
        add => _nodes.TreeNodeRemoved += value;
        remove => _nodes.TreeNodeRemoved -= value;
    }

    /// <summary>
    /// Gets the collection of root tree nodes including the hidden ones;
    /// the base <see cref="System.Windows.Forms.TreeView.Nodes" /> contains only the visible nodes.
    /// </summary>
    [ExCategory(CategoryType.Behavior)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [Localizable(true)]
    [ExDescription("TreeViewNodesDescr")]
    [MergableProperty(false)]
    public new virtual ExTreeNodeCollection Nodes => _nodes;
}
