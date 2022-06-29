using ExControls.Collections;
using ExControls.Designers;
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ExControls;

/// <summary>
///  Expanded TreeView control.
/// </summary>
[Designer(typeof(ExTreeViewDesigner))]
[ToolboxBitmap(typeof(TreeView), "TreeView.bmp")]
[DefaultProperty(nameof(Nodes))]
[DefaultEvent(nameof(AfterSelect))]
public class ExTreeView : TreeView
{
    private readonly ExTreeNodeCollection _nodes;
    private ExTreeViewStyle _style;

    /// <summary>
    ///
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
        get => _style;
        set
        {
            if (_style == value)
                return;
            _style = value;
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
    ///     Occurs when the new <see cref="TreeNode"/> was added to <see cref="TreeNodeCollection"/>.
    /// </summary>
    public event EventHandler<ExTreeViewNodeAddedEventArgs> TreeNodeAdded
    {
        add => _nodes.TreeNodeAdded += value;
        remove => _nodes.TreeNodeAdded -= value;
    }

    /// <summary>
    ///     Occurs when the new <see cref="TreeNode"/> was added to <see cref="TreeNodeCollection"/>.
    /// </summary>
    public event EventHandler<ExTreeViewNodeRemovedEventArgs> TreeNodeRemoved
    {
        add => _nodes.TreeNodeRemoved += value;
        remove => _nodes.TreeNodeRemoved -= value;
    }

    /// <summary>
    /// 
    /// </summary>
    [ExCategory(CategoryType.Behavior)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [Localizable(true)]
    [ExDescription("TreeViewNodesDescr")]
    [MergableProperty(false)]
    public new virtual ExTreeNodeCollection Nodes => _nodes;
}

/// <summary>
/// Style of TreeView.
/// </summary>
public enum ExTreeViewStyle
{
    /// <summary>
    /// Default style.
    /// </summary>
    Default,

    /// <summary>
    /// Win10 light theme.
    /// </summary>
    Light,

    /// <summary>
    /// Win10 dark theme.
    /// </summary>
    Dark,

    /// <summary>
    /// Old style.
    /// </summary>
    Old
}

/// <summary>
/// 
/// </summary>
public class ExTreeViewNodeAddedEventArgs : EventArgs
{
    /// <summary>
    ///     Node that was added or removed.
    /// </summary>
    public TreeNode Node { get; }

    /// <summary>
    ///     Index of added node in collection.
    /// </summary>
    public int Index { get; }

    /// <summary>
    ///     Number of added nodes.
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="node"></param>
    /// <param name="index"></param>
    /// <param name="count"></param>
    public ExTreeViewNodeAddedEventArgs(TreeNode node, int index, int count)
    {
        Node = node;
        Index = index;
        Count = count;
    }
}

/// <summary>
/// 
/// </summary>
public class ExTreeViewNodeRemovedEventArgs : EventArgs
{
    /// <summary>
    ///     Node that was removed.
    /// </summary>
    public TreeNode Node { get; }

    /// <summary>
    ///     Index of removed node in collection.
    /// </summary>
    public int Index { get; }

    /// <summary>
    ///     Number of removed nodes.
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="node"></param>
    /// <param name="index"></param>
    /// <param name="count"></param>
    public ExTreeViewNodeRemovedEventArgs(TreeNode node, int index, int count)
    {
        Node = node;
        Index = index;
        Count = count;
    }
}