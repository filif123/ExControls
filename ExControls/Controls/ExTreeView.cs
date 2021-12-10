using System.Collections;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms.Design;

namespace ExControls.Controls;

/// <summary>
///  Expanded TreeView control.
/// </summary>
[Designer(typeof(ExTreeViewDesigner))]
[ToolboxBitmap(typeof(TreeView), "TreeView.bmp")]
public class ExTreeView : TreeView
{
    /// <summary>
    /// 
    /// </summary>
    private ExTreeNodeCollection _nodes { get; }

    /// <summary>
    ///
    /// </summary>
    public ExTreeView()
    {
        _nodes = new ExTreeNodeCollection(base.Nodes);
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
    [Description("TreeViewNodesDescr")]
    [MergableProperty(false)]
    public new ExTreeNodeCollection Nodes => _nodes;
}

/// <summary>
/// 
/// </summary>
public class ExTreeViewNodeAddedEventArgs : EventArgs
{
    public TreeNode Node { get; }
    public int Index { get; }
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
    public TreeNode Node { get; }
    public int Index { get; }
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

/// <summary>
/// 
/// </summary>
[Editor("System.Windows.Forms.Design.TreeNodeCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
public class ExTreeNodeCollection : IList
{
    private readonly TreeNodeCollection _collection;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collection"></param>
    public ExTreeNodeCollection(TreeNodeCollection collection)
    {
        _collection = collection;
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
    public TreeNode Add(string text)
    {
        var node = _collection.Add(text);
        OnTreeNodeAdded(new ExTreeViewNodeAddedEventArgs(node, _collection.Count - 1, 1));
        return node;
    }

    /// <summary>Creates a new tree node with the specified key and text, and adds it to the collection.</summary>
    /// <param name="key">The name of the tree node.</param>
    /// <param name="text">The text to display in the tree node.</param>
    /// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that was added to the collection.</returns>
    public TreeNode Add(string key, string text)
    {
        var node = _collection.Add(key, text);
        OnTreeNodeAdded(new ExTreeViewNodeAddedEventArgs(node, _collection.Count - 1, 1));
        return node;
    }

    /// <summary>Creates a tree node with the specified key, text, and image, and adds it to the collection.</summary>
    /// <param name="key">The name of the tree node.</param>
    /// <param name="text">The text to display in the tree node.</param>
    /// <param name="imageIndex">The index of the image to display in the tree node.</param>
    /// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that was added to the collection.</returns>
    public TreeNode Add(string key, string text, int imageIndex)
    {
        var node = _collection.Add(key, text, imageIndex);
        OnTreeNodeAdded(new ExTreeViewNodeAddedEventArgs(node, _collection.Count - 1, 1));
        return node;
    }

    /// <summary>Creates a tree node with the specified key, text, and image, and adds it to the collection.</summary>
    /// <param name="key">The name of the tree node.</param>
    /// <param name="text">The text to display in the tree node.</param>
    /// <param name="imageKey">The image to display in the tree node.</param>
    /// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that was added to the collection.</returns>
    public TreeNode Add(string key, string text, string imageKey)
    {
        var node = _collection.Add(key, text, imageKey);
        OnTreeNodeAdded(new ExTreeViewNodeAddedEventArgs(node, _collection.Count - 1, 1));
        return node;
    }

    /// <summary>Creates a tree node with the specified key, text, and images, and adds it to the collection.</summary>
    /// <param name="key">The name of the tree node.</param>
    /// <param name="text">The text to display in the tree node.</param>
    /// <param name="imageIndex">The index of the image to display in the tree node.</param>
    /// <param name="selectedImageIndex">The index of the image to be displayed in the tree node when it is in a selected state.</param>
    /// <returns>The tree node that was added to the collection.</returns>
    public TreeNode Add(string key, string text, int imageIndex, int selectedImageIndex)
    {
        var node = _collection.Add(key, text, imageIndex, selectedImageIndex);
        OnTreeNodeAdded(new ExTreeViewNodeAddedEventArgs(node, _collection.Count - 1, 1));
        return node;
    }

    /// <summary>Creates a tree node with the specified key, text, and images, and adds it to the collection.</summary>
    /// <param name="key">The name of the tree node.</param>
    /// <param name="text">The text to display in the tree node.</param>
    /// <param name="imageKey">The key of the image to display in the tree node.</param>
    /// <param name="selectedImageKey">The key of the image to display when the node is in a selected state.</param>
    /// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that was added to the collection.</returns>
    public TreeNode Add(string key, string text, string imageKey, string selectedImageKey)
    {
        var node = _collection.Add(key, text, imageKey, selectedImageKey);
        OnTreeNodeAdded(new ExTreeViewNodeAddedEventArgs(node, _collection.Count - 1, 1));
        return node;
    }

    /// <summary>Adds a previously created tree node to the end of the tree node collection.</summary>
    /// <param name="node">The <see cref="T:System.Windows.Forms.TreeNode" /> to add to the collection.</param>
    /// <returns>The zero-based index value of the <see cref="T:System.Windows.Forms.TreeNode" /> added to the tree node collection.</returns>
    /// <exception cref="T:System.ArgumentException">The <paramref name="node" /> is currently assigned to another <see cref="T:System.Windows.Forms.TreeView" />.</exception>
    public int Add(TreeNode node)
    {
        var index = _collection.Add(node);
        OnTreeNodeAdded(new ExTreeViewNodeAddedEventArgs(node, _collection.Count - 1, 1));
        return index;
    }

    /// <summary>Adds an array of previously created tree nodes to the collection.</summary>
    /// <param name="nodes">An array of <see cref="T:System.Windows.Forms.TreeNode" /> objects representing the tree nodes to add to the collection.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="nodes" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="nodes" /> is the child of another <see cref="T:System.Windows.Forms.TreeView" />.</exception>
    public void AddRange(TreeNode[] nodes)
    {
        if (nodes == null)
            throw new ArgumentNullException(nameof(nodes));
        if (nodes.Length == 0)
            return;

        int index = _collection.Count;
        _collection.AddRange(nodes);
        OnTreeNodeAdded(new ExTreeViewNodeAddedEventArgs(nodes[0], index, nodes.Length));
    }

    /// <summary>Inserts an existing tree node into the tree node collection at the specified location.</summary>
    /// <param name="index">The indexed location within the collection to insert the tree node.</param>
    /// <param name="node">The <see cref="T:System.Windows.Forms.TreeNode" /> to insert into the collection.</param>
    /// <exception cref="T:System.ArgumentException">The <paramref name="node" /> is currently assigned to another <see cref="T:System.Windows.Forms.TreeView" />.</exception>
    public void Insert(int index, TreeNode node)
    {
        _collection.Insert(index, node);
        OnTreeNodeAdded(new ExTreeViewNodeAddedEventArgs(node, index, 1));
    }

    /// <summary>Creates a tree node with the specified text and inserts it at the specified index.</summary>
    /// <param name="index">The location within the collection to insert the node.</param>
    /// <param name="text">The text to display in the tree node.</param>
    /// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that was inserted in the collection.</returns>
    public TreeNode Insert(int index, string text)
    {
        var node = _collection.Insert(index, text);
        OnTreeNodeAdded(new ExTreeViewNodeAddedEventArgs(node, index, 1));
        return node;
    }

    /// <summary>Creates a tree node with the specified text and key, and inserts it into the collection.</summary>
    /// <param name="index">The location within the collection to insert the node.</param>
    /// <param name="key">The name of the tree node.</param>
    /// <param name="text">The text to display in the tree node.</param>
    /// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that was inserted in the collection.</returns>
    public TreeNode Insert(int index, string key, string text)
    {
        var node = _collection.Insert(index, key, text);
        OnTreeNodeAdded(new ExTreeViewNodeAddedEventArgs(node, index, 1));
        return node;
    }

    /// <summary>Creates a tree node with the specified key, text, and image, and inserts it into the collection at the specified index.</summary>
    /// <param name="index">The location within the collection to insert the node.</param>
    /// <param name="key">The name of the tree node.</param>
    /// <param name="text">The text to display in the tree node.</param>
    /// <param name="imageIndex">The index of the image to display in the tree node.</param>
    /// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that was inserted in the collection.</returns>
    public TreeNode Insert(int index, string key, string text, int imageIndex)
    {
        var node = _collection.Insert(index, key, text, imageIndex);
        OnTreeNodeAdded(new ExTreeViewNodeAddedEventArgs(node, index, 1));
        return node;
    }

    /// <summary>Creates a tree node with the specified key, text, and image, and inserts it into the collection at the specified index.</summary>
    /// <param name="index">The location within the collection to insert the node.</param>
    /// <param name="key">The name of the tree node.</param>
    /// <param name="text">The text to display in the tree node.</param>
    /// <param name="imageKey">The key of the image to display in the tree node.</param>
    /// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that was inserted in the collection.</returns>
    public TreeNode Insert(int index, string key, string text, string imageKey)
    {
        var node = _collection.Insert(index, key, text, imageKey);
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
    public TreeNode Insert(int index, string key, string text, int imageIndex, int selectedImageIndex)
    {
        var node = _collection.Insert(index, key, text, imageIndex, selectedImageIndex);
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
    public TreeNode Insert(int index, string key, string text, string imageKey, string selectedImageKey)
    {
        var node = _collection.Insert(index, key, text, imageKey, selectedImageKey);
        OnTreeNodeAdded(new ExTreeViewNodeAddedEventArgs(node, index, 1));
        return node;
    }

    /// <summary>Removes all tree nodes from the collection.</summary>
    public void Clear()
    {
        var count = _collection.Count;
        _collection.Clear();
        OnTreeNodeRemoved(new ExTreeViewNodeRemovedEventArgs(null, 0, count));
    }

    /// <summary>Removes a tree node from the tree node collection at a specified index.</summary>
    /// <param name="index">The index of the <see cref="T:System.Windows.Forms.TreeNode" /> to remove.</param>
    public void RemoveAt(int index)
    {
        var node = _collection[index];
        _collection.RemoveAt(index);
        OnTreeNodeRemoved(new ExTreeViewNodeRemovedEventArgs(node, 0, 1));
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
        get => _collection[index];
        set => _collection[index] = value is TreeNode node ? node : throw new ArgumentException(nameof(value));
    }

    /// <summary>Gets or sets the element at the specified index.</summary>
    /// <param name="index">The zero-based index of the element to get or set.</param>
    /// <returns>The element at the specified index.</returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.IList" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.IList" /> is read-only.</exception>
    public virtual TreeNode this[int index]
    {
        get => _collection[index];
        set => _collection[index] = value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    public virtual TreeNode this[string key] => _collection[key];

    /// <summary>Gets a value indicating whether the <see cref="T:System.Collections.IList" /> is read-only.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Collections.IList" /> is read-only; otherwise, <see langword="false" />.</returns>
    public bool IsReadOnly => _collection.IsReadOnly;

    /// <summary>Gets a value indicating whether the <see cref="T:System.Collections.IList" /> has a fixed size.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Collections.IList" /> has a fixed size; otherwise, <see langword="false" />.</returns>
    public bool IsFixedSize => false;

    /// <summary>Removes the tree node with the specified key from the collection.</summary>
    /// <param name="key">The name of the tree node to remove from the collection.</param>
    public void RemoveByKey(string key)
    {
        var node = _collection[key];
        _collection.RemoveByKey(key);
        OnTreeNodeRemoved(new ExTreeViewNodeRemovedEventArgs(node, 0, 1));
    }
    /// <summary>Adds an item to the <see cref="T:System.Collections.IList" />.</summary>
     /// <param name="value">The object to add to the <see cref="T:System.Collections.IList" />.</param>
     /// <returns>The position into which the new element was inserted, or -1 to indicate that the item was not inserted into the collection.</returns>
     /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList" /> is read-only.
     /// -or-
     /// The <see cref="T:System.Collections.IList" /> has a fixed size.</exception>
    public int Add(object value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));
        return value is TreeNode node ? Add(node) : Add(value.ToString()).Index;
    }

    /// <summary>Determines the index of a specific item in the <see cref="T:System.Collections.IList" />.</summary>
    /// <param name="value">The object to locate in the <see cref="T:System.Collections.IList" />.</param>
    /// <returns>The index of <paramref name="value" /> if found in the list; otherwise, -1.</returns>
    int IList.IndexOf(object value)
    {
        return value is TreeNode node ? _collection.IndexOf(node) : -1;
    }

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
    public void Remove(TreeNode node) => node.Remove();

    /// <summary>Determines whether the <see cref="T:System.Collections.IList" /> contains a specific value.</summary>
    /// <param name="value">The object to locate in the <see cref="T:System.Collections.IList" />.</param>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Object" /> is found in the <see cref="T:System.Collections.IList" />; otherwise, <see langword="false" />.</returns>

    bool IList.Contains(object value) => value is TreeNode tn && _collection.Contains(tn);

    /// <summary>Returns an enumerator that iterates through a collection.</summary>
    /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
    public IEnumerator GetEnumerator()
    {
        return _collection.GetEnumerator();
    }

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
    public void CopyTo(Array array, int index)
    {
        _collection.CopyTo(array, index);
    }

    /// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.ICollection" />.</summary>
    /// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection" />.</returns>
    public int Count => _collection.Count;

    /// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</summary>
    /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</returns>
    public object SyncRoot => this;

    /// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).</summary>
    /// <returns>
    /// <see langword="true" /> if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, <see langword="false" />.</returns>
    public bool IsSynchronized => false;
}

/// 
///  
///      This is the designer for tree view controls.  It inherits 
///      from the base control designer and adds live hit testing
///      capabilites for the tree view control. 
/// 
internal class ExTreeViewDesigner : ControlDesigner
{
    private Win32.TVHITTESTINFO tvhit;
    private DesignerActionListCollection _actionLists;
    private ExTreeView treeView;

    private const int TVM_HITTEST = 0x1100 + 17;
    private const int TVHT_ONITEMBUTTON = 0x0010;
    private const int TVHT_ONITEM = (TVHT_ONITEMICON | TVHT_ONITEMLABEL | TVHT_ONITEMSTATEICON);
    private const int TVHT_ONITEMICON = 0x0002;
    private const int TVHT_ONITEMLABEL = 0x0004;
    private const int TVHT_ONITEMSTATEICON = 0x0040;

    public ExTreeViewDesigner()
    {
        AutoResizeHandles = true;
    }

    /// 
    ///      Disposes of this object. 
    /// 
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (treeView != null)
            {
                treeView.AfterExpand -= TreeViewInvalidate;
                treeView.AfterCollapse -= TreeViewInvalidate;
                treeView.AfterSelect -= TreeViewInvalidate;
                treeView = null;
            }
        }

        base.Dispose(disposing);
    }

    /// 
    /// 
    ///    Allows your component to support a design time user interface. A TabStrip
    ///       control, for example, has a design time user interface that allows the user 
    ///       to click the tabs to change tabs. To implement this, TabStrip returns
    ///       true whenever the given point is within its tabs. 
    ///  
    protected override unsafe bool GetHitTest(Point point)
    {
        point = Control.PointToClient(point);
        tvhit.pt.X = point.X;
        tvhit.pt.Y = point.Y;
        fixed (Win32.TVHITTESTINFO* ptr = &tvhit)
        {
            Win32.SendMessage(Control.Handle, TVM_HITTEST, IntPtr.Zero, new IntPtr(ptr));
        }
        
        return (tvhit.flags & TVHT_ONITEMBUTTON) == TVHT_ONITEMBUTTON || (tvhit.flags & TVHT_ONITEM) == TVHT_ONITEM;
    }

    public override void Initialize(IComponent component)
    {
        base.Initialize(component);
        treeView = component as ExTreeView;
        Debug.Assert(treeView != null, "TreeView is null in ExTreeViewDesigner");

        if (treeView == null) return;

        treeView.AfterExpand += TreeViewInvalidate;
        treeView.AfterCollapse += TreeViewInvalidate;
        treeView.AfterSelect += TreeViewInvalidate;
    }

    private void TreeViewInvalidate(object sender, TreeViewEventArgs e)
    {
        treeView?.Invalidate();
    }

    public override DesignerActionListCollection ActionLists => _actionLists ??= new DesignerActionListCollection { new TreeViewActionList(this) };
}

internal class TreeViewActionList : DesignerActionList
{
    private readonly ExTreeViewDesigner _designer;

    public TreeViewActionList(ExTreeViewDesigner designer) : base(designer.Component)
    {
        _designer = designer;
    }

    public void InvokeNodesDialog()
    {
        //EditorServiceContext.EditValue(_designer, Component, "Nodes");

        var property = TypeDescriptor.GetProperties(Component)["Nodes"];
        var editorServiceContext = new ExEditorServiceContext(_designer, property);
        var editor = property.GetEditor(typeof(UITypeEditor)) as UITypeEditor;
        object obj1 = property.GetValue(Component);
        object obj2 = editor?.EditValue(editorServiceContext, editorServiceContext, obj1);

        if (obj2 == obj1) 
            return;

        try
        {
            property.SetValue(Component, obj2);
        }
        catch (CheckoutException)
        {
        }
    }

    public ImageList ImageList
    {
        get => ((TreeView)Component).ImageList;
        set => TypeDescriptor.GetProperties(Component)["ImageList"].SetValue(Component, value);
    }

    public override DesignerActionItemCollection GetSortedActionItems()
    {
        var items = new DesignerActionItemCollection
        {
            new DesignerActionMethodItem(
                this, 
                "InvokeNodesDialog",
                "Edit Nodes...", //SR.GetString(SR.InvokeNodesDialogDisplayName)
                "Properties", //SR.GetString(SR.PropertiesCategoryName)
                "Edit Nodes...", //SR.GetString(SR.InvokeNodesDialogDescription)
                true),
            new DesignerActionPropertyItem(
                "ImageList",
                "Image list", //SR.GetString(SR.ImageListDisplayName)
                "Properties", //SR.GetString(SR.PropertiesCategoryName)
                "Image list") //SR.GetString(SR.ImageListDescription)
        };
        return items;
    }
}

internal class ExEditorServiceContext : ITypeDescriptorContext, IWindowsFormsEditorService
{
    private readonly ComponentDesigner _designer;
    private IComponentChangeService _componentChangeSvc;

    internal ExEditorServiceContext(ComponentDesigner designer, PropertyDescriptor prop)
    {
        _designer = designer;
        PropertyDescriptor = prop;
        if (prop != null)
            return;
        prop = TypeDescriptor.GetDefaultProperty(designer.Component);
        if (prop == null || !typeof(ICollection).IsAssignableFrom(prop.PropertyType))
            return;
        PropertyDescriptor = prop;
    }

    /// <summary>Gets the service object of the specified type.</summary>
    /// <param name="serviceType">An object that specifies the type of service object to get.</param>
    /// <returns>A service object of type <paramref name="serviceType" />.
    /// -or-
    /// <see langword="null" /> if there is no service object of type <paramref name="serviceType" />.</returns>
    object IServiceProvider.GetService(Type serviceType)
    {
        if (serviceType == typeof(ITypeDescriptorContext) || serviceType == typeof(IWindowsFormsEditorService))
            return this;
        return _designer.Component is { Site: { } } ? _designer.Component.Site.GetService(serviceType) : null;
    }

    /// <summary>Raises the <see cref="E:System.ComponentModel.Design.IComponentChangeService.ComponentChanging" /> event.</summary>
    /// <returns>
    /// <see langword="true" /> if this object can be changed; otherwise, <see langword="false" />.</returns>
    bool ITypeDescriptorContext.OnComponentChanging()
    {
        try
        {
            ChangeService.OnComponentChanging(_designer.Component, PropertyDescriptor);
        }
        catch (CheckoutException ex)
        {
            if (ex == CheckoutException.Canceled)
                return false;
            throw;
        }
        return true;
    }

    /// <summary>Raises the <see cref="E:System.ComponentModel.Design.IComponentChangeService.ComponentChanged" /> event.</summary>
    void ITypeDescriptorContext.OnComponentChanged()
    {
        ChangeService.OnComponentChanged(_designer.Component, PropertyDescriptor, null, null);
    }

    /// <summary>Gets the container representing this <see cref="T:System.ComponentModel.TypeDescriptor" /> request.</summary>
    /// <returns>An <see cref="T:System.ComponentModel.IContainer" /> with the set of objects for this <see cref="T:System.ComponentModel.TypeDescriptor" />; otherwise, <see langword="null" /> if there is no container or if the <see cref="T:System.ComponentModel.TypeDescriptor" /> does not use outside objects.</returns>
    public IContainer Container => _designer.Component.Site?.Container;

    /// <summary>Gets the object that is connected with this type descriptor request.</summary>
    /// <returns>The object that invokes the method on the <see cref="T:System.ComponentModel.TypeDescriptor" />; otherwise, <see langword="null" /> if there is no object responsible for the call.</returns>
    public object Instance => _designer.Component;

    /// <summary>Gets the <see cref="T:System.ComponentModel.PropertyDescriptor" /> that is associated with the given context item.</summary>
    /// <returns>The <see cref="T:System.ComponentModel.PropertyDescriptor" /> that describes the given context item; otherwise, <see langword="null" /> if there is no <see cref="T:System.ComponentModel.PropertyDescriptor" /> responsible for the call.</returns>
    public PropertyDescriptor PropertyDescriptor { get; }

    private IComponentChangeService ChangeService =>
        _componentChangeSvc ??= (IComponentChangeService)((IServiceProvider)this).GetService(typeof(IComponentChangeService));

    /// <summary>Closes any previously opened drop down control area.</summary>
    public void CloseDropDown()
    {
    }

    /// <summary>Displays the specified control in a drop down area below a value field of the property grid that provides this service.</summary>
    /// <param name="control">The drop down list <see cref="T:System.Windows.Forms.Control" /> to open.</param>
    public void DropDownControl(Control control)
    {
    }

    /// <summary>Shows the specified <see cref="T:System.Windows.Forms.Form" />.</summary>
    /// <param name="dialog">The <see cref="T:System.Windows.Forms.Form" /> to display.</param>
    /// <returns>A <see cref="T:System.Windows.Forms.DialogResult" /> indicating the result code returned by the <see cref="T:System.Windows.Forms.Form" />.</returns>
    public DialogResult ShowDialog(Form dialog)
    {
        var service = (IUIService)((IServiceProvider)this).GetService(typeof(IUIService));
        return service?.ShowDialog(dialog) ?? dialog.ShowDialog(_designer.Component as IWin32Window);
    }
}