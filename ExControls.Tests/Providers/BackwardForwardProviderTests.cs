using ExControls.Providers;

namespace ExControls.Tests.Providers;

/// <summary>
/// Navigacia spat/dopredu po predchadzajucich vyberoch.
/// </summary>
[TestClass]
public sealed class BackwardForwardProviderTests : IDisposable
{
    private sealed class Position(string name, List<string> log) : IBackwardForwardCommand
    {
        public string CommandName => name;
        public void Move() => log.Add(name);
    }

    private readonly List<string> _log = new();
    private BackwardForwardProvider _provider = null!;
    private Position[] _positions = null!;

    [TestInitialize]
    public void Init()
    {
        _provider = new BackwardForwardProvider { ManagerEnabled = true };
        _positions = new[] { new Position("a", _log), new Position("b", _log), new Position("c", _log) };
        foreach (var position in _positions)
            _provider.AddCommand(position);
    }

    // MSTest vola Dispose po kazdom teste
    public void Dispose() => _provider.Dispose();

    [TestMethod]
    public void BackwardForward_PresunieSaNaPredchadzajucuADalsiuPoziciu()
    {
        Assert.AreSame(_positions[2], _provider.CurrentCommand);
        _provider.Backward();
        _provider.Backward();
        Assert.IsFalse(_provider.CanBackward);
        _provider.Forward();

        Assert.AreSame(_positions[1], _provider.CurrentCommand);
        CollectionAssert.AreEqual(new[] { "b", "a", "b" }, _log);
    }

    [TestMethod]
    public void BackwardNaPoziciu_PreskociMedzipoziciuAPohneSaRaz()
    {
        _provider.Backward(_positions[0]);

        Assert.AreSame(_positions[0], _provider.CurrentCommand);
        CollectionAssert.AreEqual(new[] { "a" }, _log);
        Assert.AreEqual("b", _provider.GetForwardText());

        _provider.Forward(_positions[2]);
        Assert.AreSame(_positions[2], _provider.CurrentCommand);
        Assert.IsFalse(_provider.CanForward);
    }

    [TestMethod]
    public void AddCommand_PoNavrate_ZahodiDopredu()
    {
        _provider.Backward();

        _provider.AddCommand(new Position("d", _log));

        Assert.IsFalse(_provider.CanForward);
        Assert.AreEqual("b", _provider.GetBackwardText());
    }

    [TestMethod]
    public void BackwardNaAktualnuPoziciu_Vynimka()
    {
        Assert.ThrowsExactly<ArgumentException>(() => _provider.Backward(_positions[2]));
    }
}
