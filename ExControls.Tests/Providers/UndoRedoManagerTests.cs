using ExControls.Providers;

namespace ExControls.Tests.Providers;

/// <summary>
/// Historia zmien (Spat/Znovu).
/// </summary>
[TestClass]
public class UndoRedoManagerTests
{
    private sealed class Command(string name, List<string> log) : IUndoRedoCommand
    {
        public string CommandName => name;
        public void Undo() => log.Add("undo " + name);
        public void Redo() => log.Add("redo " + name);
    }

    private sealed class Handler(List<string> log) : IUndoHandler
    {
        public void Undo(IUndoRedoCommand cmd) => log.Add("handler undo " + cmd.CommandName);
        public void Redo(IUndoRedoCommand cmd) => log.Add("handler redo " + cmd.CommandName);
    }

    [TestMethod]
    public void UndoRedo_PoradieAStav()
    {
        var log = new List<string>();
        using var manager = new UndoRedoManager { ManagerEnabled = true };
        manager.AddCommand(new Command("a", log));
        manager.AddCommand(new Command("b", log));

        Assert.AreEqual("b", manager.GetUndoText());
        manager.Undo();
        Assert.AreEqual("a", manager.GetUndoText());
        Assert.AreEqual("b", manager.GetRedoText());
        manager.Redo();

        CollectionAssert.AreEqual(new[] { "undo b", "redo b" }, log);
        Assert.IsTrue(manager.CanUndo);
        Assert.IsFalse(manager.CanRedo);
    }

    [TestMethod]
    public void AddCommand_PoSpat_ZahodiZnovu()
    {
        var log = new List<string>();
        using var manager = new UndoRedoManager { ManagerEnabled = true };
        manager.AddCommand(new Command("a", log));
        manager.Undo();

        manager.AddCommand(new Command("b", log));

        Assert.IsFalse(manager.CanRedo);
        CollectionAssert.AreEqual(new[] { "b" }, manager.GetUndoHistory().Select(c => c.CommandName).ToArray());
    }

    [TestMethod]
    public void SavedState_PoSpat_SaVratiDoUlozenehoStavu()
    {
        var log = new List<string>();
        using var manager = new UndoRedoManager { ManagerEnabled = true };
        Assert.IsTrue(manager.IsInSavedState());

        manager.AddCommand(new Command("a", log));
        manager.SetSavedState();
        manager.AddCommand(new Command("b", log));
        Assert.IsFalse(manager.IsInSavedState());

        manager.Undo();
        Assert.IsTrue(manager.IsInSavedState());
    }

    [TestMethod]
    public void Handler_VykonaSpatNamiestoPrikazu()
    {
        var log = new List<string>();
        using var manager = new UndoRedoManager { ManagerEnabled = true };
        manager.AddCommand(new Command("a", log), new Handler(log));

        manager.Undo();
        manager.Redo();

        CollectionAssert.AreEqual(new[] { "handler undo a", "handler redo a" }, log);
    }

    [TestMethod]
    public void VypnutyManazer_PrikazyIgnorujeASpatNedovoli()
    {
        using var manager = new UndoRedoManager();
        manager.AddCommand(new Command("a", new List<string>()));

        Assert.IsFalse(manager.CanUndo);
        Assert.ThrowsExactly<InvalidOperationException>(manager.Undo);
    }
}
