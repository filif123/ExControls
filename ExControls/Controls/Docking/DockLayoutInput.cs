namespace ExControls.Docking;

/// <summary>
/// Vstup vypoctu rozlozenia <see cref="DockLayoutEngine" /> v bodoch zariadenia; polia su indexovane stranou
/// (<see cref="ExDockSide" />).
/// </summary>
internal sealed class DockLayoutInput
{
    /// <summary>
    /// Klientska oblast spravcu.
    /// </summary>
    public Rectangle Client { get; set; }

    /// <summary>
    /// Hrubka rozdelovaca medzi zonou a dokumentom aj medzi skupinami.
    /// </summary>
    public int Splitter { get; set; }

    /// <summary>
    /// Najmensia sirka aj vyska dokumentu - zony sa zuzia, aby sa dokument zmestil.
    /// </summary>
    public int MinDocument { get; set; }

    /// <summary>
    /// Najmensia hrubka zony (ak sa vobec zmesti).
    /// </summary>
    public int MinZone { get; set; }

    /// <summary>
    /// Hrubka listy auto-hide pri okraji (0 = strana listu nema).
    /// </summary>
    public int[] Strips { get; } = new int[4];

    /// <summary>
    /// Pozadovana hrubka zony.
    /// </summary>
    public int[] ZoneSizes { get; } = new int[4];

    /// <summary>
    /// Vahy ukotvenych skupin zony v poradi (prazdne = zona sa nezobrazi).
    /// </summary>
    public double[][] Weights { get; } = [[], [], [], []];
}
