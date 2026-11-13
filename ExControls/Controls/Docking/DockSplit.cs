namespace ExControls.Docking;

/// <summary>
/// Rozdelenie plochy medzi uzly: vedla seba (stlpce) alebo pod sebou (riadky), podiely podla <see cref="DockNode.Weight" />.
/// Koren zony je rozdelenie pozdlz jej strany (vlavo a vpravo riadky, hore a dole stlpce).
/// </summary>
/// <param name="sideBySide">uzly vedla seba (stlpce); inak pod sebou</param>
internal sealed class DockSplit(bool sideBySide) : DockNode
{
    private readonly List<DockNode> _children = [];

    /// <summary>
    /// Uzly su vedla seba (stlpce); inak pod sebou (riadky).
    /// </summary>
    public bool SideBySide { get; } = sideBySide;

    /// <summary>
    /// Uzly v poradi zlava doprava alebo zhora nadol.
    /// </summary>
    public IReadOnlyList<DockNode> Children => _children;

    /// <summary>
    /// Vlozi uzol na poradie (orezane do rozsahu).
    /// </summary>
    public void Insert(int index, DockNode node)
    {
        node.Parent?.Remove(node);
        node.Parent = this;
        _children.Insert(Math.Max(0, Math.Min(index, _children.Count)), node);
    }

    /// <summary>
    /// Prida uzol na koniec.
    /// </summary>
    public void Add(DockNode node) => Insert(_children.Count, node);

    /// <summary>
    /// Vyberie uzol.
    /// </summary>
    public void Remove(DockNode node)
    {
        if (_children.Remove(node))
            node.Parent = null;
    }

    /// <summary>
    /// Nahradi uzol inym na rovnakom mieste.
    /// </summary>
    public void Replace(DockNode old, DockNode node)
    {
        var index = _children.IndexOf(old);
        if (index < 0)
            throw new ArgumentException("The node is not a child of the split.", nameof(old));
        node.Parent?.Remove(node);
        _children[index] = node;
        node.Parent = this;
        old.Parent = null;
    }
}
