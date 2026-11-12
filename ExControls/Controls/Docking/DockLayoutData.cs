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
    /// Plavajuce okna.
    /// </summary>
    public List<FloatData> Floats { get; } = [];

    /// <summary>
    /// Zona na strane spravcu.
    /// </summary>
    internal sealed class ZoneData
    {
        public ExDockSide Side { get; set; }

        /// <summary>
        /// Hrubka zony (logicke body).
        /// </summary>
        public int Size { get; set; }

        public List<GroupData> Groups { get; } = [];
    }

    /// <summary>
    /// Skupina panelov (karty).
    /// </summary>
    internal sealed class GroupData
    {
        /// <summary>
        /// Podiel skupiny na dlzke zony.
        /// </summary>
        public double Weight { get; set; } = 1;

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
