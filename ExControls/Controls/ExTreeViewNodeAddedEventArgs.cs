namespace ExControls;

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
