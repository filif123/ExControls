namespace ExControls;

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
