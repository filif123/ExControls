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
            switch (value)
            {
                case ExTreeViewStyle.Default:
                    this.SetTheme(WindowsTheme.Default);
                    break;
                case ExTreeViewStyle.Light:
                    this.SetTheme(WindowsTheme.Explorer);
                    break;
                case ExTreeViewStyle.Dark:
                    this.SetTheme(WindowsTheme.DarkExplorer);
                    break;
                case ExTreeViewStyle.Old:
                    this.SetTheme(WindowsTheme.None);
                    break;
                default:
                    return;
            }
        }
    }

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

/// <summary>
/// Windows theme of <see cref="ExTreeView" />.
/// </summary>
public enum ExTreeViewStyle
{
    /// <summary>
    /// Default theme of the tree view.
    /// </summary>
    Default,

    /// <summary>
    /// Explorer theme (as in Windows Explorer).
    /// </summary>
    Light,

    /// <summary>
    /// Dark Explorer theme.
    /// </summary>
    Dark,

    /// <summary>
    /// No theme - the classic look with plus/minus signs.
    /// </summary>
    Old
}

/// <summary>
/// Provides data for the <see cref="ExTreeView.TreeNodeAdded" /> event.
/// </summary>
public class ExTreeViewNodeAddedEventArgs : EventArgs
{
    /// <summary>
    /// Node that was added, or the first of the added nodes.
    /// </summary>
    public TreeNode Node { get; }

    /// <summary>
    /// Index of added node in collection.
    /// </summary>
    public int Index { get; }

    /// <summary>
    /// Number of added nodes.
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExTreeViewNodeAddedEventArgs" /> class.
    /// </summary>
    /// <param name="node">The added node, or the first of the added nodes.</param>
    /// <param name="index">The index of the (first) added node in the collection.</param>
    /// <param name="count">The number of added nodes.</param>
    public ExTreeViewNodeAddedEventArgs(TreeNode node, int index, int count)
    {
        Node = node;
        Index = index;
        Count = count;
    }
}

/// <summary>
/// Provides data for the <see cref="ExTreeView.TreeNodeRemoved" /> event.
/// </summary>
public class ExTreeViewNodeRemovedEventArgs : EventArgs
{
    /// <summary>
    /// Node that was removed, or <see langword="null"/> when the whole collection was cleared.
    /// </summary>
    public TreeNode? Node { get; }

    /// <summary>
    /// Index of removed node in collection.
    /// </summary>
    public int Index { get; }

    /// <summary>
    /// Number of removed nodes.
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExTreeViewNodeRemovedEventArgs" /> class.
    /// </summary>
    /// <param name="node">The removed node, or <see langword="null" /> when the whole collection was cleared.</param>
    /// <param name="index">The index of the removed node in the collection.</param>
    /// <param name="count">The number of removed nodes.</param>
    public ExTreeViewNodeRemovedEventArgs(TreeNode? node, int index, int count)
    {
        Node = node;
        Index = index;
        Count = count;
    }
}