using ExControls.Collections;

namespace ExControls.Tests.Collections;

/// <summary>
/// Uzly stromu so skryvanim (vyhladavanie v ExOptionsView) - skryte uzly ostavaju v kolekcii a vracaju sa na svoje miesto.
/// </summary>
[TestClass]
public class ExTreeNodeCollectionTests
{
    private TreeView _tree = null!;
    private ExTreeNodeCollection _nodes = null!;

    [TestInitialize]
    public void Init()
    {
        _tree = new TreeView();
        _nodes = new ExTreeNodeCollection(_tree.Nodes);
        _nodes.Add("a", "A");
        _nodes.Add("b", "B").Nodes.Add("b1", "B1");
        _nodes.Add("c", "C");
    }

    [TestCleanup]
    public void Cleanup() => _tree.Dispose();

    private string[] Visible() => _tree.Nodes.Cast<TreeNode>().Select(n => n.Name).ToArray();

    [TestMethod]
    public void SetVisibility_SkrytieAZobrazenie_UzolSaVratiNaSvojeMiesto()
    {
        _nodes.SetVisibility("b", false);
        CollectionAssert.AreEqual(new[] { "a", "c" }, Visible());

        _nodes.SetVisibility("b", true);
        CollectionAssert.AreEqual(new[] { "a", "b", "c" }, Visible());
    }

    [TestMethod]
    public void SetVisibility_ViacSkrytychPredUzlom_ZachovaPoradie()
    {
        _nodes.SetVisibility(0, false);
        _nodes.SetVisibility(1, false);
        CollectionAssert.AreEqual(new[] { "c" }, Visible());

        _nodes.SetVisibility(1, true);
        CollectionAssert.AreEqual(new[] { "b", "c" }, Visible());

        _nodes.SetVisibilityForAll(true);
        CollectionAssert.AreEqual(new[] { "a", "b", "c" }, Visible());
    }

    [TestMethod]
    public void SetVisibility_VnorenyUzolPodlaKluca_ZobraziJehoVetvu()
    {
        _nodes.SetVisibility("b", false);

        _nodes.SetVisibility("b1", true);

        CollectionAssert.AreEqual(new[] { "a", "b", "c" }, Visible());
        Assert.IsTrue(_nodes.ContainsVisible(_tree.Nodes.Find("b1", true).Single(), true));
    }

    [TestMethod]
    public void RemoveAt_PriSkrytomUzle_OdstraniSpravnyUzol()
    {
        _nodes.SetVisibility("a", false);

        _nodes.RemoveAt(2);

        CollectionAssert.AreEqual(new[] { "b" }, Visible());
        _nodes.SetVisibilityForAll(true);
        CollectionAssert.AreEqual(new[] { "a", "b" }, Visible());
    }

    [TestMethod]
    public void Remove_OdstranenyUzol_SaUzNevrati()
    {
        var b = _nodes["b"]!;

        _nodes.Remove(b);
        _nodes.SetVisibilityForAll(true);

        CollectionAssert.AreEqual(new[] { "a", "c" }, Visible());
        Assert.IsFalse(_nodes.Contains(b, false));
    }

    [TestMethod]
    public void Insert_PriSkrytomUzle_VlozeniePodlaPoradiaVsetkychUzlov()
    {
        _nodes.SetVisibility("a", false);

        _nodes.Insert(2, "x", "X");

        CollectionAssert.AreEqual(new[] { "b", "x", "c" }, Visible());
        _nodes.SetVisibilityForAll(true);
        CollectionAssert.AreEqual(new[] { "a", "b", "x", "c" }, Visible());
    }

    [TestMethod]
    public void AddAClear_HlasiUdalosti()
    {
        var added = 0;
        var removed = 0;
        _nodes.TreeNodeAdded += (_, _) => added++;
        _nodes.TreeNodeRemoved += (_, _) => removed++;

        _nodes.AddRange(new[] { new TreeNode("d") { Name = "d" }, new TreeNode("e") { Name = "e" } });
        _nodes.Clear();

        Assert.AreEqual(1, added);
        Assert.AreEqual(1, removed);
        Assert.AreEqual(0, _nodes.Count);
    }
}
