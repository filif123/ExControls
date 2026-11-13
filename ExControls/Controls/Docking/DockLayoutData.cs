namespace ExControls.Docking;

/// <summary>
/// Ulozene rozlozenie spravcu panelov bez prvkov (zapisuje a cita ho <see cref="DockLayoutXml" />). Rozmery su v logickych
/// bodoch (96 DPI), poloha plavajuceho okna v bodoch obrazovky.
/// </summary>
internal sealed class DockLayoutData
{
    /// <summary>
    /// Zony v poradi zapisu.
    /// </summary>
    public List<ZoneData> Zones { get; } = [];

    /// <summary>
    /// Uzly stredoveho stromu s dokumentom (<see cref="DocumentData" />); <see langword="null" />, ked sa neulozil (pri
    /// dokumente nie su ziadne skupiny).
    /// </summary>
    public List<NodeData>? Center { get; set; }

    /// <summary>
    /// Plavajuce okna.
    /// </summary>
    public List<FloatData> Floats { get; } = [];

    /// <summary>
    /// Vsetky skupiny rozlozenia (zony do hlbky, stred, potom plavajuce okna).
    /// </summary>
    public IEnumerable<GroupData> AllGroups =>
        Zones.SelectMany(z => z.Nodes).Concat(Center ?? []).SelectMany(GroupsOf).Concat(Floats.Select(f => f.Group));

    private static IEnumerable<GroupData> GroupsOf(NodeData node) => node switch
    {
        SplitData split => split.Children.SelectMany(GroupsOf),
        GroupData group => [group],
        _ => []
    };

    /// <summary>
    /// Zona na strane spravcu: uzly korena (pozdlz strany).
    /// </summary>
    internal sealed class ZoneData
    {
        public ExDockSide Side { get; set; }

        /// <summary>
        /// Hrubka zony (logicke body).
        /// </summary>
        public int Size { get; set; }

        public List<NodeData> Nodes { get; } = [];
    }

    /// <summary>
    /// Uzol stromu zony.
    /// </summary>
    internal abstract class NodeData
    {
        /// <summary>
        /// Podiel uzla na dlzke rodicovskeho rozdelenia.
        /// </summary>
        public double Weight { get; set; } = 1;
    }

    /// <summary>
    /// Rozdelenie: uzly vedla seba (stlpce) alebo pod sebou (riadky).
    /// </summary>
    internal sealed class SplitData : NodeData
    {
        public bool SideBySide { get; set; }

        public List<NodeData> Children { get; } = [];
    }

    /// <summary>
    /// Dokument v strome stredu.
    /// </summary>
    internal sealed class DocumentData : NodeData;

    /// <summary>
    /// Skupina panelov (karty).
    /// </summary>
    internal sealed class GroupData : NodeData
    {
        /// <summary>
        /// Strana skupiny pri dokumente (lista auto-hide); <see langword="null" /> v zone na strane.
        /// </summary>
        public ExDockSide? Side { get; set; }

        /// <summary>
        /// Skupina je zbalena na liste pri okraji.
        /// </summary>
        public bool AutoHide { get; set; }

        /// <summary>
        /// Hrubka vysunutej zbalenej skupiny (logicke body, 0 = hrubka zony).
        /// </summary>
        public int AutoHideSize { get; set; }

        /// <summary>
        /// Kluc aktivneho panela.
        /// </summary>
        public string? Active { get; set; }

        public List<PanelData> Panels { get; } = [];
    }

    /// <summary>
    /// Panel skupiny.
    /// </summary>
    /// <param name="Key">kluc panela</param>
    /// <param name="Hidden">panel je skryty (po zobrazeni sa vrati do skupiny)</param>
    internal sealed record PanelData(string Key, bool Hidden);

    /// <summary>
    /// Plavajuce okno so skupinou.
    /// </summary>
    internal sealed class FloatData
    {
        /// <summary>
        /// Poloha okna na obrazovke (body zariadenia) a jeho rozmery (logicke body).
        /// </summary>
        public Rectangle Bounds { get; set; }

        /// <summary>
        /// Strana, kam sa skupina vrati pri ukotveni.
        /// </summary>
        public ExDockSide Side { get; set; } = ExDockSide.Right;

        public GroupData Group { get; set; } = new();
    }
}
