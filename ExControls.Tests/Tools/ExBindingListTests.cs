using System.ComponentModel;

namespace ExControls.Tests.Tools;

/// <summary>
/// Zoznam pre naviazanie na grid s triedenim podla stlpca.
/// </summary>
[TestClass]
public class ExBindingListTests
{
    public sealed class Row
    {
        public string Name { get; set; } = "";
        public string Number { get; set; } = "";
        public int? Count { get; set; }
    }

    private static PropertyDescriptor Property(string name) => TypeDescriptor.GetProperties(typeof(Row))[name]!;

    private static ExBindingList<Row> Rows() => new(new List<Row>
    {
        new() { Name = "Beta", Number = "10", Count = 2 },
        new() { Name = "alfa", Number = "9", Count = null },
        new() { Name = "Gama", Number = "100", Count = 1 }
    });

    [TestMethod]
    public void ApplySort_CiselneTexty_TriediCiselneNieAbecedne()
    {
        var list = Rows();

        ((IBindingList)list).ApplySort(Property(nameof(Row.Number)), ListSortDirection.Ascending);

        CollectionAssert.AreEqual(new[] { "9", "10", "100" }, list.Select(r => r.Number).ToArray());
        Assert.IsTrue(((IBindingList)list).IsSorted);
    }

    [TestMethod]
    public void ApplySort_ZostupnePrazdneHodnoty_PrazdneNaKonci()
    {
        var list = Rows();

        ((IBindingList)list).ApplySort(Property(nameof(Row.Count)), ListSortDirection.Descending);

        CollectionAssert.AreEqual(new int?[] { 2, 1, null }, list.Select(r => r.Count).ToArray());
    }

    [TestMethod]
    public void ApplySort_FireEventOnSort_HlasiResetLenAkJeZapnuty()
    {
        var list = Rows();
        var events = new List<ListChangedType>();
        list.ListChanged += (_, e) => events.Add(e.ListChangedType);

        ((IBindingList)list).ApplySort(Property(nameof(Row.Name)), ListSortDirection.Ascending);
        Assert.IsEmpty(events);

        list.FireEventOnSort = true;
        ((IBindingList)list).ApplySort(Property(nameof(Row.Name)), ListSortDirection.Descending);
        CollectionAssert.AreEqual(new[] { ListChangedType.Reset }, events);
    }

    [TestMethod]
    public void ApplySort_ZoznamBezTriedenia_NezmeniPoradie()
    {
        var list = Rows();
        list.Sortable = false;

        ((IBindingList)list).ApplySort(Property(nameof(Row.Number)), ListSortDirection.Ascending);

        Assert.IsFalse(((IBindingList)list).SupportsSorting);
        CollectionAssert.AreEqual(new[] { "10", "9", "100" }, list.Select(r => r.Number).ToArray());
    }

    [TestMethod]
    public void AddRange_ViacPoloziek_JednaUdalostAUdalostiOstavajuZapnute()
    {
        var list = Rows();
        var events = new List<ListChangedEventArgs>();
        list.ListChanged += (_, e) => events.Add(e);

        list.AddRange(new[] { new Row { Name = "Delta" }, new Row { Name = "Epsilon" } });
        list.AddRange(Array.Empty<Row>());

        Assert.HasCount(5, list);
        Assert.HasCount(1, events);
        Assert.AreEqual(ListChangedType.ItemAdded, events[0].ListChangedType);
        Assert.AreEqual(3, events[0].NewIndex);
        Assert.IsTrue(list.RaiseListChangedEvents);
    }
}
