namespace ExControls.Docking;

/// <summary>
/// Skupina panelov zobrazenych ako karty: je bud v zone (ukotvena alebo zbalena na liste), alebo v plavajucom okne.
/// Skryte panely ostavaju clenmi skupiny, aby sa po zobrazeni vratili na svoje miesto.
/// </summary>
internal sealed class DockGroup
{
    /// <summary>
    /// Clenovia skupiny v poradi kariet vratane skrytych.
    /// </summary>
    public List<ExDockPanel> Panels { get; } = [];

    /// <summary>
    /// Zobrazeny (aktivny) panel skupiny.
    /// </summary>
    public ExDockPanel? Active { get; set; }

    /// <summary>
    /// Podiel skupiny na dlzke zony.
    /// </summary>
    public double Weight { get; set; } = 1;

    /// <summary>
    /// Skupina je zbalena na liste pri okraji.
    /// </summary>
    public bool AutoHide { get; set; }

    /// <summary>
    /// Hrubka vysunutej zbalenej skupiny v logickych bodoch (0 = hrubka zony).
    /// </summary>
    public int AutoHideSize { get; set; }

    /// <summary>
    /// Zona skupiny; <see langword="null" />, ked je skupina v plavajucom okne.
    /// </summary>
    public DockZone? Zone { get; set; }

    /// <summary>
    /// Skupina je v plavajucom okne.
    /// </summary>
    public bool IsFloating { get; set; }

    /// <summary>
    /// Poloha plavajuceho okna na obrazovke (body zariadenia) a jeho rozmery (logicke body); prazdna = okno sa umiestni
    /// pri kurzore.
    /// </summary>
    public Rectangle FloatBounds { get; set; }

    /// <summary>
    /// Strana, kam sa skupina vrati pri ukotveni z plavajuceho okna.
    /// </summary>
    public ExDockSide HomeSide { get; set; } = ExDockSide.Right;

    /// <summary>
    /// Pohlad skupiny (vytvara ho spravca, ked sa skupina zobrazi).
    /// </summary>
    public DockGroupView? View { get; set; }

    /// <summary>
    /// Plavajuce okno skupiny (vytvara ho spravca, ked je jeho formular zobrazeny).
    /// </summary>
    public DockFloatForm? FloatForm { get; set; }

    /// <summary>
    /// Zobrazeni clenovia skupiny v poradi kariet.
    /// </summary>
    public List<ExDockPanel> VisiblePanels => Panels.Where(p => !p.IsClosed).ToList();

    /// <summary>
    /// Skupina ma aspon jeden zobrazeny panel.
    /// </summary>
    public bool HasVisiblePanels => Panels.Exists(p => !p.IsClosed);

    /// <summary>
    /// Strana skupiny: strana zony alebo, pre plavajucu skupinu, strana navratu.
    /// </summary>
    public ExDockSide Side => Zone?.Side ?? HomeSide;

    /// <summary>
    /// Opravi aktivny panel, ak je skryty alebo uz nie je clenom.
    /// </summary>
    public void FixActive()
    {
        if (Active is not null && Panels.Contains(Active) && !Active.IsClosed)
            return;

        Active = Panels.FirstOrDefault(p => !p.IsClosed);
    }
}
