namespace ExControls.Docking;

/// <summary>
/// Operacie nad stromom rozlozenia zony (bez prvkov, testovatelne): vlozenie vedla skupiny alebo na okraj zony, vybratie
/// a zjednodusenie stromu.
/// </summary>
internal static class DockTree
{
    /// <summary>
    /// Skupiny podstromu v poradi (zlava doprava, zhora nadol).
    /// </summary>
    public static IEnumerable<DockGroup> Groups(DockNode node)
    {
        if (node is DockGroup group)
        {
            yield return group;
            yield break;
        }

        if (node is not DockSplit split)
            yield break;
        foreach (var child in split.Children)
            foreach (var inner in Groups(child))
                yield return inner;
    }

    /// <summary>
    /// Uzol zabera miesto: viditelna skupina, rozdelenie s viditelnym uzlom alebo dokument (vzdy).
    /// </summary>
    public static bool IsVisible(DockNode node, Func<DockGroup, bool> visible) => node switch
    {
        DockGroup group => visible(group),
        DockSplit split => split.Children.Any(c => IsVisible(c, visible)),
        _ => true
    };

    /// <summary>
    /// Vlozi uzol vedla ciela na stranu <paramref name="direction" />: v rozdeleni rovnakeho smeru ako surodenca (dostane
    /// cast podielu ciela), inak ciel nahradi nove rozdelenie s oboma uzlami.
    /// </summary>
    /// <param name="target">uzol v strome (ma rodica)</param>
    /// <param name="node">vkladany uzol bez rodica</param>
    /// <param name="direction">strana ciela, kam sa uzol vlozi</param>
    /// <param name="share">cast miesta ciela pre novy uzol (0 az 1)</param>
    public static void InsertBeside(DockNode target, DockNode node, ExDockSide direction, double share = 0.5)
    {
        var parent = target.Parent ?? throw new ArgumentException("The target has no parent split.", nameof(target));
        var sideBySide = direction is ExDockSide.Left or ExDockSide.Right;
        var before = direction is ExDockSide.Left or ExDockSide.Top;
        if (parent.SideBySide == sideBySide)
        {
            node.Weight = target.Weight * share;
            target.Weight -= node.Weight;
            var index = parent.Children.IndexOf(target);
            parent.Insert(before ? index : index + 1, node);
            return;
        }

        var split = new DockSplit(sideBySide) { Weight = target.Weight };
        parent.Replace(target, split);
        target.Weight = 1 - share;
        node.Weight = share;
        split.Add(before ? node : target);
        split.Add(before ? target : node);
    }

    /// <summary>
    /// Vlozi uzol na okraj celeho stromu korena v smere <paramref name="direction" />: pozdlz korena na jeho zaciatok alebo
    /// koniec, napriec korenom ako novy stlpec alebo riadok vedla celeho obsahu.
    /// </summary>
    /// <param name="root">koren zony</param>
    /// <param name="node">vkladany uzol bez rodica</param>
    /// <param name="direction">okraj, na ktory sa uzol vlozi</param>
    public static void InsertAtEdge(DockSplit root, DockNode node, ExDockSide direction)
    {
        var sideBySide = direction is ExDockSide.Left or ExDockSide.Right;
        var before = direction is ExDockSide.Left or ExDockSide.Top;
        var container = root;
        if (root.SideBySide != sideBySide)
        {
            // obsah korena ako jeden uzol rozdelenia napriec (ak uz take nie je jedinym uzlom korena)
            if (root.Children.Count == 1 && root.Children[0] is DockSplit only && only.SideBySide == sideBySide)
            {
                container = only;
            }
            else
            {
                container = new DockSplit(sideBySide);
                if (root.Children.Count > 0)
                {
                    var content = new DockSplit(root.SideBySide);
                    foreach (var child in root.Children.ToList())
                        content.Add(child);
                    container.Add(content);
                }

                root.Add(container);
            }
        }

        node.Weight = container.Children.Count == 0 ? 1 : container.Children.Average(c => c.Weight);
        container.Insert(before ? 0 : container.Children.Count, node);
        Normalize(root);
    }

    /// <summary>
    /// Vyberie uzol zo stromu a zjednodusi strom korena.
    /// </summary>
    public static void Remove(DockNode node)
    {
        if (node.Parent is not { } parent)
            return;
        parent.Remove(node);
        var root = parent;
        while (root.Parent is { } up)
            root = up;
        Normalize(root);
    }

    /// <summary>
    /// Zjednodusi strom: odstrani prazdne rozdelenia, rozdelenie s jednym uzlom nahradi tym uzlom a rozdelenie rovnakeho
    /// smeru ako rodic rozpusti do rodica (podiely sa prepocitaju). Koren ostava.
    /// </summary>
    public static void Normalize(DockSplit split)
    {
        var index = 0;
        while (index < split.Children.Count)
        {
            if (split.Children[index] is not DockSplit child)
            {
                index++;
                continue;
            }

            Normalize(child);
            if (child.Children.Count == 0)
            {
                split.Remove(child);
                continue;
            }

            if (child.Children.Count == 1)
            {
                // jediny uzol nahradi rozdelenie a posudi sa znova (moze byt rozdelenim smeru rodica)
                var only = child.Children[0];
                only.Weight = child.Weight;
                split.Replace(child, only);
                continue;
            }

            if (child.SideBySide == split.SideBySide)
            {
                var sum = child.Children.Sum(c => c.Weight);
                var children = child.Children.ToList();
                split.Remove(child);
                var position = index;
                foreach (var inner in children)
                {
                    inner.Weight = sum > 0 ? child.Weight * inner.Weight / sum : child.Weight / children.Count;
                    split.Insert(position++, inner);
                }

                continue;
            }

            index++;
        }
    }
}
