namespace ExControls.Tests.Tools;

/// <summary>
/// Rozsirenia kolekcii - na .NET Framework aj pretazenia, ktore BCL ma az od .NET 6.
/// </summary>
[TestClass]
public class CollectionExtensionsTests
{
    [TestMethod]
    public void FindIndexAIndexOf_NajdeAleboVratiMinusJedna()
    {
        IEnumerable<string> items = new[] { "a", "b", "c" };

        Assert.AreEqual(1, items.FindIndex(i => i == "b"));
        Assert.AreEqual(-1, items.FindIndex(i => i == "x"));
        Assert.AreEqual(2, items.IndexOf("c"));
        Assert.AreEqual(-1, items.IndexOf("x"));
    }

    [TestMethod]
    public void FirstOrDefaultALastOrDefault_SPredvolenouHodnotou()
    {
        IEnumerable<string> empty = Array.Empty<string>();
        IEnumerable<string> items = new[] { "a", "bb", "c" };

        Assert.AreEqual("-", empty.FirstOrDefault("-"));
        Assert.AreEqual("-", empty.LastOrDefault("-"));
        Assert.AreEqual("bb", items.FirstOrDefault(i => i.Length == 2, "-"));
        Assert.AreEqual("-", items.LastOrDefault(i => i.Length == 3, "-"));
    }

    [TestMethod]
    public void EquatableCollection_RovnakyObsah_JeRovnaAjHash()
    {
        var a = new EquatableCollection<int>(new[] { 1, 2, 3 });
        var b = new EquatableCollection<int>(new[] { 1, 2, 3 });
        var c = new EquatableCollection<int>(new[] { 3, 2, 1 });

        Assert.IsTrue(a.Equals(b));
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        Assert.IsFalse(a.Equals(c));
    }

    [TestMethod]
    public void SortExtensions_ZoradiIListNaMieste()
    {
        IList<int> list = new List<int> { 3, 1, 2 };

        list.Sort((x, y) => y.CompareTo(x));
        CollectionAssert.AreEqual(new[] { 3, 2, 1 }, list.ToArray());

        list.Sort();
        CollectionAssert.AreEqual(new[] { 1, 2, 3 }, list.ToArray());
    }
}
