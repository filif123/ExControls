using System.Collections;
using System.Drawing.Design;
// ReSharper disable UnusedMember.Global
// ReSharper disable VirtualMemberNeverOverridden.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace ExControls.Collections;

/// <summary>
/// 
/// </summary>
[Editor("System.Windows.Forms.Design.TreeNodeCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
public class ExTreeNodeCollection : IList
{
    private readonly List<TreeNode> _nodes;
    private readonly TreeNodeCollection _visibleNodes;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nodes"></param>
    public ExTreeNodeCollection(TreeNodeCollection nodes)
    {
        _nodes = nodes.Cast<TreeNode>().ToList();
        _visibleNodes = nodes;
    }

    /// <summary>
    ///     Occurs when the new <see cref="TreeNode"/> was added to <see cref="TreeNodeCollection"/>.
    /// </summary>
    public event EventHandler<ExTreeViewNodeAddedEventArgs> TreeNodeAdded;

    /// <summary>
    ///     Occurs when the new <see cref="TreeNode"/> was added to <see cref="TreeNodeCollection"/>.
    /// </summary>
    public event EventHandler<ExTreeViewNodeRemovedEventArgs> TreeNodeRemoved;

    /// <summary>Adds a new tree node with the specified label text to the end of the current tree node collection.</summary>
    /// <param name="text">The label text displayed by the <see cref="T:System.Windows.Forms.TreeNode" />.</param>
    /// <returns>A <see cref="T:System.Windows.Forms.TreeNode" /> that represents the tree node being added to the collection.</returns>
    public virtual TreeNode Add(string text)
    {
        var node = _visibleNodes.Add(text);
        _nodes.Add(node);
        OnTreeNodeAdded(new ExTreeViewNodeAddedEventArgs(node, _nodes.Count - 1, 1));
        return node;
    }

    /// <summary>Creates a new tree node with the specified key and text, and adds it to the collection.</summary>
    /// <param name="key">The name of the tree node.</param>
    /// <param name="text">The text to display in the tree node.</param>
    /// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that was added to the collection.</returns>
    public virtual TreeNode Add(string key, string text)
    {
        var node = _visibleNodes.Add(key, text);
        _nodes.Add(node);
        OnTreeNodeAdded(new ExTreeViewNodeAddedEventArgs(node, _nodes.Count - 1, 1));
        return node;
    }

    /// <summary>Creates a tree node with the specified key, text, and image, and adds it to the collection.</summary>
    /// <param name="key">The name of the tree node.</param>
    /// <param name="text">The text to display in the tree node.</param>
    /// <param name="imageIndex">The index of the image to display in the tree node.</param>
    /// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that was added to the collection.</returns>
    public virtual TreeNode Add(string key, string text, int imageIndex)
    {
        var node = _visibleNodes.Add(key, text, imageIndex);
        _nodes.Add(node);
        OnTreeNodeAdded(new ExTreeViewNodeAddedEventArgs(node, _nodes.Count - 1, 1));
        return node;
    }

    /// <summary>Creates a tree node with the specified key, text, and image, and adds it to the collection.</summary>
    /// <param name="key">The name of the tree node.</param>
    /// <param name="text">The text to display in the tree node.</param>
    /// <param name="imageKey">The image to display in the tree node.</param>
    /// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that was added to the collection.</returns>
    public virtual TreeNode Add(string key, string text, string imageKey)
    {
        var node = _visibleNodes.Add(key, text, imageKey);
        _nodes.Add(node);
        OnTreeNodeAdded(new ExTreeViewNodeAddedEventArgs(node, _nodes.Count - 1, 1));
        return node;
    }

    /// <summary>Creates a tree node with the specified key, text, and images, and adds it to the collection.</summary>
    /// <param name="key">The name of the tree node.</param>
    /// <param name="text">The text to display in the tree node.</param>
    /// <param name="imageIndex">The index of the image to display in the tree node.</param>
    /// <param name="selectedImageIndex">The index of the image to be displayed in the tree node when it is in a selected state.</param>
    /// <returns>The tree node that was added to the collection.</returns>
    public virtual TreeNode Add(string key, string text, int imageIndex, int selectedImageIndex)
    {
        var node = _visibleNodes.Add(key, text, imageIndex, selectedImageIndex);
        _nodes.Add(node);
        OnTreeNodeAdded(new ExTreeViewNodeAddedEventArgs(node, _nodes.Count - 1, 1));
        return node;
    }

    /// <summary>Creates a tree node with the specified key, text, and images, and adds it to the collection.</summary>
    /// <param name="key">The name of the tree node.</param>
    /// <param name="text">The text to display in the tree node.</param>
    /// <param name="imageKey">The key of the image to display in the tree node.</param>
    /// <param name="selectedImageKey">The key of the image to display when the node is in a selected state.</param>
    /// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that was added to the collection.</returns>
    public virtual TreeNode Add(string key, string text, string imageKey, string selectedImageKey)
    {
        var node = _visibleNodes.Add(key, text, imageKey, selectedImageKey);
        _nodes.Add(node);
        OnTreeNodeAdded(new ExTreeViewNodeAddedEventArgs(node, _nodes.Count - 1, 1));
        return node;
    }

    /// <summary>Adds a previously created tree node to the end of the tree node collection.</summary>
    /// <param name="node">The <see cref="T:System.Windows.Forms.TreeNode" /> to add to the collection.</param>
    /// <returns>The zero-based index value of the <see cref="T:System.Windows.Forms.TreeNode" /> added to the tree node collection.</returns>
    /// <exception cref="T:System.ArgumentException">The <paramref name="node" /> is currently assigned to another <see cref="T:System.Windows.Forms.TreeView" />.</exception>
    public virtual int Add(TreeNode node)
    {
        var index = _visibleNodes.Add(node);
        _nodes.Add(node);
        OnTreeNodeAdded(new ExTreeViewNodeAddedEventArgs(node, _nodes.Count - 1, 1));
        return index;
    }

    /// <summary>Adds an array of previously created tree nodes to the collection.</summary>
    /// <param name="nodes">An array of <see cref="T:System.Windows.Forms.TreeNode" /> objects representing the tree nodes to add to the collection.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="nodes" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="nodes" /> is the child of another <see cref="T:System.Windows.Forms.TreeView" />.</exception>
    public virtual void AddRange(TreeNode[] nodes)
    {
        if (nodes == null)
            throw new ArgumentNullException(nameof(nodes));
        if (nodes.Length == 0)
            return;

        int index = _nodes.Count;
        _nodes.AddRange(nodes);
        _visibleNodes.AddRange(nodes);
        OnTreeNodeAdded(new ExTreeViewNodeAddedEventArgs(nodes[0], index, nodes.Length));
    }

    /// <summary>Inserts an existing tree node into the tree node collection at the specified location.</summary>
    /// <param name="index">The indexed location within the collection to insert the tree node.</param>
    /// <param name="node">The <see cref="T:System.Windows.Forms.TreeNode" /> to insert into the collection.</param>
    /// <exception cref="T:System.ArgumentException">The <paramref name="node" /> is currently assigned to another <see cref="T:System.Windows.Forms.TreeView" />.</exception>
    public virtual void Insert(int index, TreeNode node)
    {
        _nodes.Insert(index, node);
        if (index < _visibleNodes.Count)
            _visibleNodes.Insert(index, node);
        else
            _visibleNodes.Add(node);
        OnTreeNodeAdded(new ExTreeViewNodeAddedEventArgs(node, index, 1));
    }

    /// <summary>Creates a tree node with the specified text and inserts it at the specified index.</summary>
    /// <param name="index">The location within the collection to insert the node.</param>
    /// <param name="text">The text to display in the tree node.</param>
    /// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that was inserted in the collection.</returns>
    public virtual TreeNode Insert(int index, string text)
    {
        var node = new TreeNode(text);
        _nodes.Insert(index, node);
        if (index < _visibleNodes.Count)
            _visibleNodes.Insert(index, node);
        else
            _visibleNodes.Add(node);
        OnTreeNodeAdded(new ExTreeViewNodeAddedEventArgs(node, index, 1));
        return node;
    }

    /// <summary>Creates a tree node with the specified text and key, and inserts it into the collection.</summary>
    /// <param name="index">The location within the collection to insert the node.</param>
    /// <param name="key">The name of the tree node.</param>
    /// <param name="text">The text to display in the tree node.</param>
    /// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that was inserted in the collection.</returns>
    public virtual TreeNode Insert(int index, string key, string text)
    {
        var node = new TreeNode(text) {Name = key};
        _nodes.Insert(index, node);
        if (index < _visibleNodes.Count)
            _visibleNodes.Insert(index, node);
        else
            _visibleNodes.Add(node);
        OnTreeNodeAdded(new ExTreeViewNodeAddedEventArgs(node, index, 1));
        return node;
    }

    /// <summary>Creates a tree node with the specified key, text, and image, and inserts it into the collection at the specified index.</summary>
    /// <param name="index">The location within the collection to insert the node.</param>
    /// <param name="key">The name of the tree node.</param>
    /// <param name="text">The text to display in the tree node.</param>
    /// <param name="imageIndex">The index of the image to display in the tree node.</param>
    /// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that was inserted in the collection.</returns>
    public virtual TreeNode Insert(int index, string key, string text, int imageIndex)
    {
        var node = new TreeNode(text) {Name = key, ImageIndex = imageIndex};
        _nodes.Insert(index, node);
        if (index < _visibleNodes.Count)
            _visibleNodes.Insert(index, node);
        else
            _visibleNodes.Add(node);
        OnTreeNodeAdded(new ExTreeViewNodeAddedEventArgs(node, index, 1));
        return node;
    }

    /// <summary>Creates a tree node with the specified key, text, and image, and inserts it into the collection at the specified index.</summary>
    /// <param name="index">The location within the collection to insert the node.</param>
    /// <param name="key">The name of the tree node.</param>
    /// <param name="text">The text to display in the tree node.</param>
    /// <param name="imageKey">The key of the image to display in the tree node.</param>
    /// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that was inserted in the collection.</returns>
    public virtual TreeNode Insert(int index, string key, string text, string imageKey)
    {
        var node = new TreeNode(text) {Name = key, ImageKey = imageKey};
        _nodes.Insert(index, node);
        if (index < _visibleNodes.Count)
            _visibleNodes.Insert(index, node);
        else
            _visibleNodes.Add(node);
        OnTreeNodeAdded(new ExTreeViewNodeAddedEventArgs(node, index, 1));
        return node;
    }

    /// <summary>Creates a tree node with the specified key, text, and images, and inserts it into the collection at the specified index.</summary>
    /// <param name="index">The location within the collection to insert the node.</param>
    /// <param name="key">The name of the tree node.</param>
    /// <param name="text">The text to display in the tree node.</param>
    /// <param name="imageIndex">The index of the image to display in the tree node.</param>
    /// <param name="selectedImageIndex">The index of the image to display in the tree node when it is in a selected state.</param>
    /// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that was inserted in the collection.</returns>
    public virtual TreeNode Insert(int index, string key, string text, int imageIndex, int selectedImageIndex)
    {
        var node = new TreeNode(text, imageIndex, selectedImageIndex){Name = key};
        _nodes.Insert(index, node);
        if (index < _visibleNodes.Count)
            _visibleNodes.Insert(index, node);
        else
            _visibleNodes.Add(node);
        OnTreeNodeAdded(new ExTreeViewNodeAddedEventArgs(node, index, 1));
        return node;
    }

    /// <summary>Creates a tree node with the specified key, text, and images, and inserts it into the collection at the specified index.</summary>
    /// <param name="index">The location within the collection to insert the node.</param>
    /// <param name="key">The name of the tree node.</param>
    /// <param name="text">The text to display in the tree node.</param>
    /// <param name="imageKey">The key of the image to display in the tree node.</param>
    /// <param name="selectedImageKey">The key of the image to display in the tree node when it is in a selected state.</param>
    /// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that was inserted in the collection.</returns>
    public virtual TreeNode Insert(int index, string key, string text, string imageKey, string selectedImageKey)
    {
        var node = new TreeNode(text){Name = key, ImageKey = imageKey, SelectedImageKey = selectedImageKey};
        _nodes.Insert(index, node);
        if (index < _visibleNodes.Count)
            _visibleNodes.Insert(index, node);
        else
            _visibleNodes.Add(node);
        OnTreeNodeAdded(new ExTreeViewNodeAddedEventArgs(node, index, 1));
        return node;
    }

    /// <summary>Removes all tree nodes from the collection.</summary>
    public virtual void Clear()
    {
        var count = _nodes.Count;
        _nodes.Clear();
        _visibleNodes.Clear();
        OnTreeNodeRemoved(new ExTreeViewNodeRemovedEventArgs(null, 0, count));
    }

    /// <summary>Removes a tree node from the tree node collection at a specified index.</summary>
    /// <param name="index">The index of the <see cref="T:System.Windows.Forms.TreeNode" /> to remove.</param>
    public virtual void RemoveAt(int index)
    {
        var node = _nodes[index];
        _nodes.RemoveAt(index);
        _visibleNodes.RemoveAt(index);
        OnTreeNodeRemoved(new ExTreeViewNodeRemovedEventArgs(node, 0, 1));
    }

    /// <summary>
    ///     Sets the visibility of the tree node.
    /// </summary>
    /// <param name="node">treenode</param>
    /// <param name="visible">whether node should be visible</param>
    public void SetVisibility(TreeNode node, bool visible)
    {
        if (GetVisibility(node) == visible)
            return;
        
        var index = _nodes.IndexOf(node);
        if (index == -1)
            return;
        
        if (visible)
        {
            if (index >= _visibleNodes.Count)
                _visibleNodes.Add(node);
            else
                _visibleNodes.Insert(index, node);
        }
        else
        {
            _visibleNodes.Remove(node);
        }
    }

    /// <summary>
    ///     Sets the visibility of the tree node by index.
    /// </summary>
    /// <param name="index">index of treenode</param>
    /// <param name="visible">whether node should be visible</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void SetVisibility(int index, bool visible)
    {
        if (GetVisibility(index) == visible)
            return;
        
        if (index < 0 || index >= _nodes.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        
        var node = _nodes[index];

        if (visible)
        {
            if (index >= _visibleNodes.Count)
                _visibleNodes.Add(node);
            else
                _visibleNodes.Insert(index, node);
        }
        else
        {
            _visibleNodes.Remove(node);
        }
    }

    /// <summary>
    ///     Sets the visibility of the tree node by key.
    /// </summary>
    /// <param name="key">key of treenode</param>
    /// <param name="visible">whether node should be visible</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void SetVisibility(string key, bool visible)
    {
        if (GetVisibility(key) == visible)
            return;
        
        var node = _nodes.FirstOrDefault(n => n.Name.Equals(key,StringComparison.OrdinalIgnoreCase));
        var index = _nodes.IndexOf(node);
        if (index == -1 || node is null)
            throw new ArgumentOutOfRangeException(nameof(key));

        if (visible)
        {
            if (index >= _visibleNodes.Count)
                _visibleNodes.Add(node);
            else
                _visibleNodes.Insert(index, node);
        }
        else
        {
            _visibleNodes.Remove(node);
        }
    }

    /// <summary>
    ///     Gets the visibility of the tree node by node.
    /// </summary>
    /// <param name="node">tree node</param>
    /// <returns></returns>
    public bool GetVisibility(TreeNode node) => _visibleNodes.Contains(node);

    /// <summary>
    ///    Gets the visibility of the tree node by index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool GetVisibility(int index)
    {
        if (index < 0 || index >= _nodes.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        var node = _nodes[index];
        return _visibleNodes.Contains(node);
    }

    /// <summary>
    ///     Gets the visibility of the tree node by key.
    /// </summary>
    /// <param name="key">tree node key</param>
    /// <returns></returns>
    public bool GetVisibility(string key) => _visibleNodes.ContainsKey(key);

    /// <summary>
    ///     Sets the visibility of all tree nodes.
    /// </summary>
    /// <param name="visible">whether node should be visible</param>
    public void SetVisibilityForAll(bool visible)
    {
        if (visible)
        {
            _visibleNodes.Clear();
            _visibleNodes.AddRange(_nodes.Cast<TreeNode>().ToArray());
        }
        else
            _visibleNodes.Clear();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    protected virtual void OnTreeNodeAdded(ExTreeViewNodeAddedEventArgs e) => TreeNodeAdded?.Invoke(this, e);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    protected virtual void OnTreeNodeRemoved(ExTreeViewNodeRemovedEventArgs e) => TreeNodeRemoved?.Invoke(this, e);

    object IList.this[int index]
    {
        get => _visibleNodes[index];
        set => _visibleNodes[index] = value as TreeNode ?? throw new ArgumentException(nameof(value));
    }

    /// <summary>Gets or sets the element at the specified index.</summary>
    /// <param name="index">The zero-based index of the element to get or set.</param>
    /// <returns>The element at the specified index.</returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.IList" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.IList" /> is read-only.</exception>
    public virtual TreeNode this[int index]
    {
        get => _visibleNodes[index];
        set => _visibleNodes[index] = value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    public virtual TreeNode this[string key] => _visibleNodes[key];

    /// <summary>Gets a value indicating whether the <see cref="T:System.Collections.IList" /> is read-only.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Collections.IList" /> is read-only; otherwise, <see langword="false" />.</returns>
    public virtual bool IsReadOnly => false;

    /// <summary>Gets a value indicating whether the <see cref="T:System.Collections.IList" /> has a fixed size.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Collections.IList" /> has a fixed size; otherwise, <see langword="false" />.</returns>
    public virtual bool IsFixedSize => false;

    /// <summary>Removes the tree node with the specified key from the collection.</summary>
    /// <param name="key">The name of the tree node to remove from the collection.</param>
    public virtual void RemoveByKey(string key)
    {
        var node = _nodes.FirstOrDefault(n => n.Name.Equals(key,StringComparison.OrdinalIgnoreCase));
        _nodes.Remove(node);
        _visibleNodes.RemoveByKey(key);
        OnTreeNodeRemoved(new ExTreeViewNodeRemovedEventArgs(node, 0, 1));
    }
    /// <summary>Adds an item to the <see cref="T:System.Collections.IList" />.</summary>
    /// <param name="value">The object to add to the <see cref="T:System.Collections.IList" />.</param>
    /// <returns>The position into which the new element was inserted, or -1 to indicate that the item was not inserted into the collection.</returns>
    /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList" /> is read-only.
    /// -or-
    /// The <see cref="T:System.Collections.IList" /> has a fixed size.</exception>
    public virtual int Add(object value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));
        return value is TreeNode node ? Add(node) : Add(value.ToString()).Index;
    }

    /// <summary>Determines the index of a specific item in the <see cref="T:System.Collections.IList" />.</summary>
    /// <param name="value">The object to locate in the <see cref="T:System.Collections.IList" />.</param>
    /// <returns>The index of <paramref name="value" /> if found in the list; otherwise, -1.</returns>
    int IList.IndexOf(object value) => value is TreeNode node ? _visibleNodes.IndexOf(node) : -1;

    /// <summary>Inserts an item to the <see cref="T:System.Collections.IList" /> at the specified index.</summary>
    /// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted.</param>
    /// <param name="value">The object to insert into the <see cref="T:System.Collections.IList" />.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.IList" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList" /> is read-only.
    /// -or-
    /// The <see cref="T:System.Collections.IList" /> has a fixed size.</exception>
    /// <exception cref="T:System.NullReferenceException">
    /// <paramref name="value" /> is null reference in the <see cref="T:System.Collections.IList" />.</exception>
    void IList.Insert(int index, object value)
    {
        if (value is not TreeNode node)
            throw new ArgumentException(nameof(value));
        Insert(index, node);
    }

    /// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList" />.</summary>
    /// <param name="value">The object to remove from the <see cref="T:System.Collections.IList" />.</param>
    /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList" /> is read-only.
    /// -or-
    /// The <see cref="T:System.Collections.IList" /> has a fixed size.</exception>
    void IList.Remove(object value)
    {
        if (value is not TreeNode node)
            return;
        Remove(node);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="node"></param>
    public virtual void Remove(TreeNode node) => node.Remove();

    /// <summary>Determines whether the <see cref="T:System.Collections.IList" /> contains a specific value.</summary>
    /// <param name="value">The object to locate in the <see cref="T:System.Collections.IList" />.</param>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Object" /> is found in the <see cref="T:System.Collections.IList" />; otherwise, <see langword="false" />.</returns>

    bool IList.Contains(object value) => value is TreeNode tn && _visibleNodes.Contains(tn);

    /// <summary>Returns an enumerator that iterates through a collection.</summary>
    /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
    public IEnumerator GetEnumerator() => _visibleNodes.GetEnumerator();

    /// <summary>Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
    /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
    /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is less than zero.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///         <paramref name="array" /> is multidimensional.
    /// -or-
    /// The number of elements in the source <see cref="T:System.Collections.ICollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.
    /// -or-
    /// The type of the source <see cref="T:System.Collections.ICollection" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
    public void CopyTo(Array array, int index) => _visibleNodes.CopyTo(array, index);

    /// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.ICollection" />.</summary>
    /// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection" />.</returns>
    public int Count => _visibleNodes.Count;

    /// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</summary>
    /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</returns>
    public object SyncRoot => this;

    /// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).</summary>
    /// <returns>
    /// <see langword="true" /> if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, <see langword="false" />.</returns>
    public bool IsSynchronized => false;
}