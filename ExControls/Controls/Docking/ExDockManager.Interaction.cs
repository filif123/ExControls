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
            InsertGroup(Zone(group.HomeSide), group, int.MaxValue);
        });
    }

    private bool CanFloat(DockGroup group) => AllowFloating && group.VisiblePanels.TrueForAll(p => p.CanFloat);

    /// <summary>
    /// Ponuka okna panela (sipka v zahlavi, pravy klik na zahlavie alebo kartu).
    /// </summary>
    internal void ShowMenu(DockGroupView view, ExDockPanel panel, Point location)
    {
        var group = view.Group;
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
            return;
        }

        if (_theme is not null)
            ExThemer.Apply(menu, _theme);
        _menuOpen = true;
        menu.Closed += (_, _) =>
        {
            _menuOpen = false;
            Post(menu.Dispose);
        };
        menu.Show(view, location);
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
    /// Navadzacie znacky pre kurzor: vonkajsie pri okrajoch spravcu a kosostvorec nad skupinou pod kurzorom.
    /// </summary>
    internal List<DockGuide> GuidesAt(Point screen, DockGroup source, ExDockPanel? panel, DockFloatForm? moving)
    {
        var guides = new List<DockGuide>();
        if (!IsHandleCreated)
            return guides;

        var size = ToDevice(32);
        var area = RectangleToScreen(_layout.Inner);
        foreach (var side in AllSides)
            guides.Add(new DockGuide(DockGuides.Outer(area, side, size, ToDevice(8)), new DockDropTarget(DockDropKind.Edge, side), side));

        if (DropCandidateAt(screen, source, panel, moving) is not { } candidate)
            return guides;

        var center = DockGuides.Center(candidate.Bounds, size);
        if (candidate.Group != source)
            guides.Add(new DockGuide(center, new DockDropTarget(DockDropKind.Tab, ExDockSide.Bottom, candidate.Group), null));
        if (candidate.Group.Zone is { } zone)
            foreach (var direction in DockGuides.SplitDirections(zone.Side))
                guides.Add(new DockGuide(DockGuides.Arrow(center, direction, ToDevice(2)),
                    new DockDropTarget(DockDropKind.Split, direction, candidate.Group), direction));
        return guides;
    }

    /// <summary>
    /// Skupina pod kurzorom, ku ktorej mozno pripojit tahany zdroj (nie ona sama, ani presuvane okno).
    /// </summary>
    private DropCandidate? DropCandidateAt(Point screen, DockGroup source, ExDockPanel? panel, DockFloatForm? moving)
    {
        bool Allowed(DockGroup group) => group != source || (panel is not null && source.Panels.Count(p => !p.IsClosed) > 1);

        foreach (var group in _floating)
            if (group.FloatForm is { Visible: true } form && form != moving && group.View is { } view && view.Parent == form
                && view.RectangleToScreen(view.ClientRectangle) is var bounds && bounds.Contains(screen) && Allowed(group))
                return new DropCandidate(group, bounds);

        foreach (var zone in _zones)
            foreach (var group in zone.DockedGroups)
                if (group.View is { Visible: true } view && view.Parent == this
                    && view.RectangleToScreen(view.ClientRectangle) is var bounds && bounds.Contains(screen) && Allowed(group))
                    return new DropCandidate(group, bounds);
        return null;
    }

    private readonly record struct DropCandidate(DockGroup Group, Rectangle Bounds);

    /// <summary>
    /// Nahlad miesta, kam sa zdroj po pusteni presunie (obrazovka).
    /// </summary>
    internal Rectangle PreviewBounds(DockDropTarget target, DockGroup source, ExDockPanel? panel, Rectangle floatBounds)
    {
        switch (target.Kind)
        {
            case DockDropKind.Float:
                return floatBounds;
            case DockDropKind.Tab:
            case DockDropKind.Split:
                return target.Group?.View is { IsHandleCreated: true } view
                    ? DockGuides.Preview(view.RectangleToScreen(view.ClientRectangle), target)
                    : Rectangle.Empty;
            default:
                // rozlozenie s novou skupinou na konci zony (bez presuvanej skupiny)
                var input = BuildInput();
                var zone = Zone(target.Side);
                var weights = zone.DockedGroups.Where(g => panel is not null || g != source).Select(g => g.Weight).ToList();
                weights.Add(weights.Count == 0 ? 1 : weights.Average());
                input.Weights[(int)target.Side] = weights.ToArray();
                var groups = DockLayoutEngine.Compute(input).Groups[(int)target.Side];
                return groups.Length == 0 ? Rectangle.Empty : RectangleToScreen(groups[groups.Length - 1]);
        }
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
                case DockDropKind.Split when target.Group is { Zone: { } zone } group:
                    if (group == source && (panel is null || source.Panels.Count == 1))
                        return;
                    var split = Take(source, panel);
                    split.Weight = group.Weight / 2;
                    group.Weight -= split.Weight;
                    var index = zone.Groups.IndexOf(group);
                    InsertGroup(zone, split, target.Before ? index : index + 1, true);
                    break;
                case DockDropKind.Edge:
                    InsertGroup(Zone(target.Side), Take(source, panel), int.MaxValue);
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
