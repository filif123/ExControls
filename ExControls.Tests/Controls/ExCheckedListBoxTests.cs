namespace ExControls.Tests.Controls;

/// <summary>
/// Highlighting the item under the mouse in the custom style: only the previous and the new item are repainted (repainting
/// the whole multi-column list flickered) and the MouseMove and MouseLeave events are raised as in the default style.
/// </summary>
[STATestClass]
public class ExCheckedListBoxTests
{
    private static TestList CreateList()
    {
        var list = new TestList { DefaultStyle = false, MultiColumn = true, ColumnWidth = 100, Size = new Size(300, 100) };
        list.Items.AddRange(["A", "B", "C", "D", "E", "F", "G", "H"]);
        list.CreateControl();
        return list;
    }

    [TestMethod]
    public void PrechodMysou_PrekresliLenPovodnuANovuPolozku()
    {
        using var list = CreateList();
        var first = list.GetItemRectangle(0);
        var second = list.GetItemRectangle(1);
        list.RaiseMouseMove(new Point(first.X + 2, first.Y + 2));
        var invalidated = new List<Rectangle>();
        list.Invalidated += (_, e) => invalidated.Add(e.InvalidRect);

        list.RaiseMouseMove(new Point(second.X + 2, second.Y + 2));

        CollectionAssert.AreEquivalent(new[] { first, second }, invalidated);
    }

    [TestMethod]
    public void VlastnyStyl_UdalostiMysiSaVyvolaju()
    {
        using var list = CreateList();
        var moved = 0;
        var left = 0;
        list.MouseMove += (_, _) => moved++;
        list.MouseLeave += (_, _) => left++;

        list.RaiseMouseMove(new Point(2, 2));
        list.RaiseMouseLeave();

        Assert.AreEqual((1, 1), (moved, left));
    }

    private sealed class TestList : ExCheckedListBox
    {
        public void RaiseMouseMove(Point location) => OnMouseMove(new MouseEventArgs(MouseButtons.None, 0, location.X, location.Y, 0));

        public void RaiseMouseLeave() => OnMouseLeave(EventArgs.Empty);
    }
}
