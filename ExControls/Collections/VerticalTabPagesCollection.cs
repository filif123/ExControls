using System.Collections;
using System.Drawing.Design;
using ExControls.Designers;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

// ReSharper disable UnusedMember.Global
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace ExControls.Collections;

/// <summary>
///     Collection of VerticalTabPages.
/// </summary>
[Editor(typeof(ExVerticalTabPageCollectionEditor), typeof(UITypeEditor))]
[TypeConverter(typeof(ExVerticalTabPageConverter))]
public class VerticalTabPagesCollection : IList
{
    private readonly List<ExVerticalTabPage> _tabs = new();

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<TabPageEventArgs> PageAdded;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<TabPageEventArgs> PageRemoved;

    /// <summary>
    /// 
    /// </summary>
    public VerticalTabPagesCollection(ExVerticalTabPage owner)
    {
        Owner = owner;
    }

    /// <summary>
    /// 
    /// </summary>
    public ExVerticalTabPage Owner { get; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsRoot => Owner == null;

    /// <inheritdoc />
    public int Count => _tabs.Count;

    /// <inheritdoc />
    public object SyncRoot => new ();

    /// <inheritdoc />
    public bool IsSynchronized => false;

    /// <inheritdoc />
    public bool IsReadOnly => false;

    /// <inheritdoc />
    public bool IsFixedSize => false;

    /// <inheritdoc />
    public object this[int index]
    {
        get => _tabs[index];
        set
        {
            if (value is not ExVerticalTabPage page)
                return;

            _tabs[index] = page;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public ExVerticalTabPage this[TreeNode node]
    {
        get => node == null ? null : _tabs[node.Index];
        set => _tabs[node.Index] = value;
    }

    /// <inheritdoc />
    public int Add(object item)
    {
        if (item is not ExVerticalTabPage page)
            throw new ArgumentException("Collection must contains items of type ExVerticalTabPage only.");

        if (_tabs.Contains(item))
            return -1;

        _tabs.Add(page);
        OnPageAdded(new TabPageEventArgs(page, 1, Count - 1));

        return Count - 1;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    public int Add(ExVerticalTabPage page)
    {
        return Add(page as object);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="items"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void AddRange(ExVerticalTabPage[] items)
    {
        if (items == null) 
            throw new ArgumentNullException(nameof(items));

        var count = items.Length;
        if (count == 0) return;
        _tabs.AddRange(items);
        OnPageAdded(new TabPageEventArgs(items[0], count, Count - 1));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="items"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void AddRange(VerticalTabPagesCollection items)
    {
        if (items == null) 
            throw new ArgumentNullException(nameof(items));

        var count = items.Count; 
        if (count == 0) return;

        foreach (ExVerticalTabPage page in items) 
            _tabs.Add(page);

        OnPageAdded(new TabPageEventArgs(items[0] as ExVerticalTabPage, count, Count - 1));
    }

    /// <inheritdoc />
    public void Clear()
    {
        var count = _tabs.Count;
        _tabs.Clear();
        OnPageRemoved(new TabPageEventArgs(count == 0 ? null : _tabs[0], count, 0));
    }

    /// <inheritdoc />
    public bool Contains(object item) => _tabs.Contains(item);

    /// <inheritdoc />
    public void CopyTo(Array array, int arrayIndex)
    {
        _tabs.CopyTo(array as ExVerticalTabPage[] ?? Array.Empty<ExVerticalTabPage>(), arrayIndex);
    }

    /// <inheritdoc />
    public void Remove(object item)
    {
        if (item is not ExVerticalTabPage page)
            return;
        var i = _tabs.IndexOf(page);

        if (i == -1) return;

        _tabs.RemoveAt(i);
        OnPageRemoved(new TabPageEventArgs(page, 1, i));
    }

    /// <inheritdoc />
    public int IndexOf(object item) => _tabs.IndexOf(item as ExVerticalTabPage);

    /// <inheritdoc />
    public void Insert(int index, object item)
    {
        if (item is not ExVerticalTabPage page)
            throw new ArgumentException("Collection must contains items of type ExVerticalTabPage only.");

        if (_tabs.Contains(item))
            return;

        page.Node.Name = page.Name;
        page.Node.Text = page.Text;
        _tabs.Insert(index, page);
        OnPageAdded(new TabPageEventArgs(page, 1, index));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    public void Insert(int index, ExVerticalTabPage item)
    {
        Insert(index, item as object);
    }

    /// <inheritdoc />
    public void RemoveAt(int index)
    {
        _tabs.RemoveAt(index);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerator GetEnumerator()
    {
        return _tabs.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    protected virtual void OnPageAdded(TabPageEventArgs e)
    {
        PageAdded?.Invoke(this, e);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    protected virtual void OnPageRemoved(TabPageEventArgs e)
    {
        PageRemoved?.Invoke(this, e);
    }
}

/// <summary>
/// 
/// </summary>
public class TabPageEventArgs : EventArgs
{
    /// <summary>
    /// 
    /// </summary>
    public ExVerticalTabPage TabPage { get; }

    /// <summary>
    /// 
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// 
    /// </summary>
    public int Index { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tabPage"></param>
    /// <param name="count"></param>
    /// <param name="index"></param>
    public TabPageEventArgs(ExVerticalTabPage tabPage, int count, int index)
    {
        TabPage = tabPage;
        Count = count;
        Index = index;
    }
}