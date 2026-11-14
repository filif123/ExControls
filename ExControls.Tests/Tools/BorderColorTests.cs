namespace ExControls.Tests.Tools;

/// <summary>
/// The border of a standard control in its own color (<see cref="ExTools.SetBorderColor" />): it is drawn over the system
/// 1 px frame (also in DrawToBitmap), stays after the handle is recreated and <see langword="null" /> gives the system border back.
/// </summary>
[STATestClass]
public class BorderColorTests
{
    private static Color Corner(Control control)
    {
        using var bitmap = new Bitmap(control.Width, control.Height);
        control.DrawToBitmap(bitmap, new Rectangle(0, 0, control.Width, control.Height));
        return bitmap.GetPixel(0, control.Height / 2);
    }

    private static TestList CreateList()
    {
        var list = new TestList { BorderStyle = BorderStyle.FixedSingle, Size = new Size(120, 80) };
        list.Items.AddRange(["A", "B"]);
        list.CreateControl();
        return list;
    }

    [TestMethod]
    public void Okraj_FarbaPodlaNastavenia()
    {
        using var list = CreateList();

        list.SetBorderColor(Color.Red);

        Assert.AreEqual(Color.Red.ToArgb(), Corner(list).ToArgb());
    }

    [TestMethod]
    public void Okraj_OstavaPoNovomHandle()
    {
        using var list = CreateList();
        list.SetBorderColor(Color.Red);

        list.Recreate();

        Assert.AreEqual(Color.Red.ToArgb(), Corner(list).ToArgb());
    }

    [TestMethod]
    public void Okraj_NullVratiSystemovy()
    {
        using var list = CreateList();
        var system = Corner(list);
        list.SetBorderColor(Color.Red);
        list.SetBorderColor(Color.Lime);

        list.SetBorderColor(null);

        Assert.AreEqual(system.ToArgb(), Corner(list).ToArgb());
    }

    private sealed class TestList : ListBox
    {
        public void Recreate() => RecreateHandle();
    }
}
