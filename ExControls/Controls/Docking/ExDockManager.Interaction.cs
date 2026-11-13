using ExControls.Docking;

namespace ExControls;

// akcie z pohladov skupin: karty, tlacidla zahlavia, ponuka okna, tahanie a pustenie
public partial class ExDockManager
{
    /// <summary>
    /// Vyberie kartu skupiny, pripadne presunie fokus do panela.
    /// </summary>
    internal void SelectTab(DockGroup group, ExDockPanel? panel, bool focus)
    {
        if (panel is null || panel.Group != group || panel.IsClosed)
            return;

        if (group.Active != panel)
        {
            group.Active = panel;
            group.View?.SyncPanels();
            if (group.FloatForm is { IsDisposed: false } form)
                form.Text = panel.Text;
        }

        if (focus)
            FocusPanel(panel);
        if (group.View is { ContainsFocus: true })
            SetActivePanel(panel);
    }

    /// <summary>
    /// Pouzivatel skryva panel (tlacidlo, ponuka): <see cref="PanelClosing" /> ho moze ponechat.
    /// </summary>
    internal void ClosePanelByUser(ExDockPanel panel)
    {
        if (panel.Manager != this || panel.IsClosed || !panel.CanClose)
            return;

        var args = new ExDockPanelClosingEventArgs(panel);
        OnPanelClosing(args);
        if (!args.Cancel)
            HidePanel(panel);
    }

    /// <summary>
    /// Pouzivatel zatvara plavajuce okno: skryje jeho panely, ktore sa daju skryt.
    /// </summary>
    internal void CloseGroupByUser(DockGroup group)
    {
        foreach (var panel in group.VisiblePanels)
            ClosePanelByUser(panel);
    }

    /// <summary>
    /// Spendlik: zbali ukotvenu skupinu na listu alebo ukotvi zbalenu.
    /// </summary>
    internal void ToggleAutoHide(DockGroup group)
    {
        if (group.IsFloating || group.Panels.Count == 0)
            return;
        Change(() =>
        {
            if (_popup.Group == group)
                HidePopup();
            group.AutoHide = !group.AutoHide;
        });
    }

    /// <summary>
    /// Dvojklik na zahlavie: ukotvenu skupinu presunie do plavajuceho okna, plavajucu ukotvi na jej stranu.
    /// </summary>
    internal void ToggleFloat(DockGroup group)
    {
        if (group.Panels.Count == 0)
            return;
        if (group.IsFloating)
        {
            DockGroupHome(group);
            return;
        }

        if (group.AutoHide || !CanFloat(group))
            return;
        Change(() =>
        {
            var bounds = ScreenBoundsOf(group.View);
            DetachGroup(group);
            MakeFloating(group, bounds);
        });
    }

    /// <summary>
    /// Ukotvi plavajucu skupinu na koniec zony jej strany.
    /// </summary>
    internal void DockGroupHome(DockGroup group)
    {
        if (!group.IsFloating || group.Panels.Count == 0)
            return;
        Change(() =>
        {
            DetachGroup(group);
            PlaceAtEnd(Zone(group.HomeSide), group);
        });
    }

    private bool CanFloat(DockGroup group) => AllowFloating && group.VisiblePanels.TrueForAll(p => p.CanFloat);

    /// <summary>
    /// Ponuka okna panela (sipka v zahlavi, pravy klik na zahlavie alebo kartu).
    /// </summary>
    internal void ShowMenu(DockGroupView view, ExDockPanel panel, Point location)
    {
        if (CreateMenu(view.Group, panel) is not { } menu)
            return;

        _menuOpen = true;
        menu.Closed += (_, _) =>
        {
            _menuOpen = false;
            Post(menu.Dispose);
        };
        menu.Show(view, location);
    }

    /// <summary>
    /// Ponuka okna panela podla jeho stavu a temy; <see langword="null" />, ked v nej nie je ziadna polozka.
    /// </summary>
    internal ContextMenuStrip? CreateMenu(DockGroup group, ExDockPanel panel)
    {
        var menu = new ContextMenuStrip();
        if (group.IsFloating)
            menu.Items.Add(Strings.Dock, null, (_, _) => Post(() => DockGroupHome(group)));
        else if (AllowFloating && panel.CanFloat && !group.AutoHide)
            menu.Items.Add(Strings.FloatWindow, null, (_, _) => Post(() => FloatPanel(panel)));

        if (!group.IsFloating && panel.CanAutoHide)
        {
            var autoHide = new ToolStripMenuItem(Strings.AutoHide) { Checked = group.AutoHide };
            autoHide.Click += (_, _) => Post(() => ToggleAutoHide(group));
            menu.Items.Add(autoHide);
        }

        if (panel.CanClose)
        {
            if (menu.Items.Count > 0)
                menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(Strings.Hide, null, (_, _) => Post(() => ClosePanelByUser(panel)));
        }

        if (menu.Items.Count == 0)
        {
            menu.Dispose();
            return null;
        }

        // vzhlad ponuk programu (obsluha ContextMenuStrip v ExThemer), inak vlastny podla temy
        if (_theme is not null && !ExThemer.ApplyHandler(menu, _theme) && !_theme.UseSystemStyle)
        {
            menu.BackColor = Colors.Box;
            menu.ForeColor = Colors.BoxFore;
            menu.Renderer = new DockMenuRenderer(Colors);
        }

        return menu;
    }

    /// <summary>
    /// Zacne tahanie skupiny (zahlavie, <paramref name="panel" /> = <see langword="null" />) alebo panela (karta).
    /// </summary>
    /// <param name="view">pohlad, v ktorom tahanie zacalo</param>
    /// <param name="panel">tahany panel; <see langword="null" /> = cela skupina</param>
    /// <param name="down">miesto stlacenia mysi na obrazovke</param>
    /// <param name="screen">kurzor na obrazovke</param>
    /// <returns>tahanie alebo <see langword="null" /> (zbalenu skupinu nemozno tahat)</returns>
    internal DockDragOperation? BeginDrag(DockGroupView view, ExDockPanel? panel, Point down, Point screen)
    {
        var group = view.Group;
        if (view.Mode == DockViewMode.AutoHide || group.AutoHide)
            return null;
        if (panel is not null && group.Panels.Count(p => !p.IsClosed) <= 1)
            panel = null;

        var moving = panel is null && group.IsFloating ? group.FloatForm : null;
        var origin = moving?.Location ?? view.PointToScreen(Point.Empty);
        var size = moving?.Size ?? view.Size;
        var offset = new Point(down.X - origin.X, down.Y - origin.Y);
        // karta sa taha za zahlavie buduceho okna
        if (panel is not null)
            offset = new Point(Math.Min(offset.X, Math.Max(0, size.Width - ToDevice(20))), CaptionHeight / 2);
        var canFloat = AllowFloating && (panel?.CanFloat ?? CanFloat(group));
        var drag = new DockDragOperation(this, group, panel, moving, offset, size, canFloat);
        drag.Start(screen);
        return drag;
    }

    /// <summary>
    /// Navadzacie znacky pre kurzor: tlacidla pri okrajoch spravcu a kompas nad skupinou alebo dokumentom pod kurzorom.
    /// </summary>
    /// <param name="screen">kurzor na obrazovke</param>
    /// <param name="source">tahana skupina alebo skupina tahaneho panela</param>
    /// <param name="panel">tahany panel; <see langword="null" /> = cela skupina</param>
    /// <param name="moving">plavajuce okno, ktore sa taha s mysou</param>
    /// <param name="previous">znacky predchadzajucej polohy: kompas pod kurzorom ostava pri svojom cieli</param>
    internal DockGuideSet GuidesAt(Point screen, DockGroup source, ExDockPanel? panel, DockFloatForm? moving,
        DockGuideSet? previous = null)
    {
        var set = new DockGuideSet();
        if (!IsHandleCreated)
            return set;

        var size = ToDevice(32);
        var pad = ToDevice(4);
        var gap = ToDevice(12);

        // znacky pri okrajoch su vzdy; kompas sa od nich odsunie, ked je skupina pri okraji
        var area = RectangleToScreen(_layout.Inner);
        var outer = new List<DockGuides.OuterGuide>();
        foreach (var side in DockGuides.Sides)
        {
            var button = DockGuides.Outer(area, side, size, ToDevice(10));
            outer.Add(new DockGuides.OuterGuide(side, DockGuides.BoundsOf(DockGuides.Plate(button, pad))));
            set.Plates.Add(DockGuides.Plate(button, pad));
            set.Guides.Add(new DockGuide(button, new DockDropTarget(DockDropKind.Edge, side), side));
        }

        // kompas, na ktorom je kurzor, ostava (posunuty od okraja moze presahovat mimo svojho ciela)
        DropCandidate? candidate = null;
        var overDocument = false;
        if (previous is { CompassBounds: var last } && last.Contains(screen))
        {
            if (previous.CompassGroup is { } kept && CandidateBounds(kept) is { IsEmpty: false } keptBounds)
                candidate = new DropCandidate(kept, keptBounds);
            overDocument = previous.CompassDocument;
        }

        if (candidate is null && !overDocument)
        {
            candidate = DropCandidateAt(screen, source, panel, moving);
            overDocument = candidate is null && RectangleToScreen(_layout.Document) is { Width: > 0, Height: > 0 } over && over.Contains(screen);
        }

        if (candidate is { } found)
        {
            var group = found.Group;
            set.CompassGroup = group;
            var tab = group != source ? new DockDropTarget(DockDropKind.Tab, ExDockSide.Bottom, group) : null;
            if (group.IsFloating)
            {
                // plavajuce okno ma jednu skupinu - len karta
                var center = Shifted(DockGuides.Center(found.Bounds, size), c => DockGuides.Plate(c, pad), outer);
                set.Plates.Add(DockGuides.Plate(center, pad));
                set.CompassBounds = DockGuides.BoundsOf(DockGuides.Plate(center, pad));
                if (tab is not null)
                    set.Guides.Add(new DockGuide(center, tab, null));
            }
            else
            {
                var center = Shifted(DockGuides.Center(found.Bounds, size), c => DockGuides.CompassPlate(c, gap, pad), outer);
                AddCompass(set, center, gap, tab, side => new DockDropTarget(DockDropKind.Split, side, group));
            }
        }
        else if (overDocument)
        {
            var center = Shifted(DockGuides.Center(RectangleToScreen(_layout.Document), size), c => DockGuides.CompassPlate(c, gap, pad), outer);
            AddCompass(set, center, gap, null, side => new DockDropTarget(DockDropKind.Document, side));
            set.CompassDocument = true;
        }

        return set;
    }

    /// <summary>
    /// Skupina ako ciel na obrazovke: plavajuca v okne, ukotvena podla rozlozenia; prazdne, ked sa nezobrazuje.
    /// </summary>
    private Rectangle CandidateBounds(DockGroup group)
    {
        if (group.Panels.Count == 0 || group.View is not { } view)
            return Rectangle.Empty;
        if (group.IsFloating)
            return group.FloatForm is { Visible: true } form && view.Parent == form ? view.RectangleToScreen(view.ClientRectangle) : Rectangle.Empty;
        return view.Parent == this && _layout.BoundsOf(group) is { Width: > 0, Height: > 0 } bounds ? RectangleToScreen(bounds) : Rectangle.Empty;
    }

    private Rectangle Shifted(Rectangle center, Func<Rectangle, Point[]> plate, List<DockGuides.OuterGuide> outer)
    {
        var shift = DockGuides.AvoidOuter(DockGuides.BoundsOf(plate(center)), outer, ToDevice(4));
        center.Offset(shift);
        return center;
    }

    // kompas: podlozka, stred (karta, ak je) a styri sipky
    private void AddCompass(DockGuideSet set, Rectangle center, int gap, DockDropTarget? tab, Func<ExDockSide, DockDropTarget> arrow)
    {
        set.Plates.Add(DockGuides.CompassPlate(center, gap, ToDevice(4)));
        set.CompassBounds = DockGuides.BoundsOf(set.Plates[set.Plates.Count - 1]);
        if (tab is not null)
            set.Guides.Add(new DockGuide(center, tab, null));
        foreach (var side in DockGuides.Sides)
            set.Guides.Add(new DockGuide(DockGuides.Arrow(center, side, gap), arrow(side), side));
    }

    /// <summary>
    /// Skupina pod kurzorom, ku ktorej mozno pripojit tahany zdroj (nie ona sama, ani presuvane okno).
    /// </summary>
    private DropCandidate? DropCandidateAt(Point screen, DockGroup source, ExDockPanel? panel, DockFloatForm? moving)
    {
        bool Allowed(DockGroup group) => group != source || (panel is not null && source.Panels.Count(p => !p.IsClosed) > 1);

        foreach (var group in _floating)
            if (group != source && group.FloatForm is { Visible: true } form && form != moving && group.View is { } view
                && view.Parent == form && view.RectangleToScreen(view.ClientRectangle) is var bounds && bounds.Contains(screen))
                return new DropCandidate(group, bounds);

        foreach (var zone in AllZones)
            foreach (var group in zone.DockedGroups)
                if (group.View is { } view && view.Parent == this && _layout.BoundsOf(group) is { Width: > 0, Height: > 0 } layout
                    && RectangleToScreen(layout) is var bounds && bounds.Contains(screen) && Allowed(group))
                    return new DropCandidate(group, bounds);
        return null;
    }

    private readonly record struct DropCandidate(DockGroup Group, Rectangle Bounds);

    /// <summary>
    /// Nahlad miesta, kam sa zdroj po pusteni presunie (obrazovka).
    /// </summary>
    internal Rectangle PreviewBounds(DockDropTarget target, Rectangle floatBounds) => target.Kind switch
    {
        DockDropKind.Float => floatBounds,
        DockDropKind.Tab or DockDropKind.Split => target.Group?.View is { IsHandleCreated: true } view
            ? DockGuides.Preview(view.RectangleToScreen(view.ClientRectangle), target)
            : Rectangle.Empty,
        DockDropKind.Document => RectangleToScreen(DocumentPreview(target.Side)),
        _ => RectangleToScreen(ZonePreview(target.Side))
    };

    /// <summary>
    /// Miesto skupiny ukotvenej vedla dokumentu v klientskej oblasti: cast dokumentu na danej strane.
    /// </summary>
    internal Rectangle DocumentPreview(ExDockSide side)
    {
        var document = _layout.Document;
        return DockLayoutEngine.Take(ref document, side, (int)(DockLayoutEngine.Thickness(document, side) * DocumentShare));
    }

    /// <summary>
    /// Miesto novej skupiny na vonkajsom okraji zony v klientskej oblasti: v prazdnej zone cela zona, inak jej cast podla
    /// poctu stlpcov alebo riadkov napriec zonou.
    /// </summary>
    internal Rectangle ZonePreview(ExDockSide side)
    {
        var zone = Zone(side);
        var bounds = _layout.Zones[(int)side];
        if (bounds.IsEmpty)
        {
            // rozlozenie s jednou skupinou v prazdnej zone
            var probe = new DockGroup();
            var root = new DockSplit(zone.Root.SideBySide);
            root.Add(probe);
            var input = BuildInput();
            input.Roots[(int)side] = root;
            input.Visible = g => g == probe || DockZone.IsDocked(g);
            return DockLayoutEngine.Compute(input).Zones[(int)side];
        }

        var across = zone.Root.Children.Where(c => DockTree.IsVisible(c, DockZone.IsDocked)).ToList();
        var count = across.Count == 1 && across[0] is DockSplit only && only.SideBySide != zone.Root.SideBySide
            ? only.Children.Count(c => DockTree.IsVisible(c, DockZone.IsDocked))
            : 1;
        var thickness = DockLayoutEngine.Thickness(bounds, side) / (count + 1);
        return DockLayoutEngine.Take(ref bounds, side, thickness);
    }

    /// <summary>
    /// Presunie skupinu alebo panel na ciel (po pusteni mysi).
    /// </summary>
    /// <param name="source">tahana skupina alebo skupina tahaneho panela</param>
    /// <param name="panel">tahany panel; <see langword="null" /> = cela skupina</param>
    /// <param name="target">ciel</param>
    /// <param name="floatBounds">plavajuce okno pre <see cref="DockDropKind.Float" /> (obrazovka)</param>
    internal void Drop(DockGroup source, ExDockPanel? panel, DockDropTarget target, Rectangle floatBounds)
    {
        // medzitym zmenene rozlozenie (napr. nacitane) - tahanie sa zahodi
        if (source.Panels.Count == 0 || (panel is not null && panel.Group != source))
            return;
        if (target.Group is { } targetGroup && (targetGroup.Panels.Count == 0 || !AllGroups.Contains(targetGroup)))
            return;

        Change(() =>
        {
            switch (target.Kind)
            {
                case DockDropKind.Tab when target.Group is { } group:
                    MoveIntoGroup(source, panel, group);
                    break;
                case DockDropKind.Split when target.Group is { Zone: not null } group:
                    if (group == source && (panel is null || source.Panels.Count == 1))
                        return;
                    PlaceBeside(group, Take(source, panel), target.Side);
                    break;
                case DockDropKind.Edge:
                    PlaceAtEdge(Zone(target.Side), Take(source, panel), target.Side);
                    break;
                case DockDropKind.Document:
                    PlaceByDocument(Take(source, panel), target.Side);
                    break;
                case DockDropKind.Float:
                    if (source.IsFloating && (panel is null || source.Panels.Count == 1))
                    {
                        source.FloatBounds = floatBounds;
                        return;
                    }

                    MakeFloating(Take(source, panel), floatBounds);
                    break;
            }
        });
    }
}
