using ExControls.Docking;

namespace ExControls;

// model skupin a zon a jeho zosuladenie s prvkami (pohlady skupin, vysunuta skupina, plavajuce okna)
public partial class ExDockManager
{
    /// <summary>
    /// Farby ramu podla temy.
    /// </summary>
    internal DockColors Colors { get; private set; } = DockColors.System;

    /// <summary>
    /// Spolocne bublinove napovedy pohladov.
    /// </summary>
    internal ToolTip ToolTip => _toolTip;

    /// <summary>
    /// Zony indexovane stranou.
    /// </summary>
    internal IReadOnlyList<DockZone> Zones => _zones;

    /// <summary>
    /// Skupiny v plavajucich oknach.
    /// </summary>
    internal IReadOnlyList<DockGroup> FloatingGroups => _floating;

    /// <summary>
    /// Vysuvany panel zbalenej skupiny.
    /// </summary>
    internal DockAutoHidePopup Popup => _popup;

    /// <summary>
    /// Vsetky skupiny: zony v poradi stran a plavajuce.
    /// </summary>
    internal IEnumerable<DockGroup> AllGroups => AllZones.SelectMany(z => DockTree.Groups(z.Root)).Concat(_floating);

    /// <summary>
    /// Zony na stranach a stredova zona s dokumentom.
    /// </summary>
    internal IEnumerable<DockZone> AllZones => _zones.Append(_center);

    /// <summary>
    /// Stredova zona s dokumentom.
    /// </summary>
    internal DockZone Center => _center;

    /// <summary>
    /// Podiel miesta dokumentu pre panel ukotveny vedla neho.
    /// </summary>
    private const double DocumentShare = 0.3;

    /// <summary>
    /// Zbalene skupiny so zobrazenymi panelmi na liste strany: zo zony strany a skupiny pri dokumente s touto stranou.
    /// </summary>
    internal List<DockGroup> AutoHideGroupsOf(ExDockSide side) =>
        Zone(side).AutoHideGroups.Concat(_center.AutoHideGroups.Where(g => g.HomeSide == side)).ToList();

    /// <summary>
    /// Prevedie logicke body (96 DPI) na body zariadenia podla DPI spravcu.
    /// </summary>
    internal int ToDevice(int logical) => LogicalToDeviceUnits(logical);

    /// <summary>
    /// Prevedie body zariadenia na logicke body (96 DPI).
    /// </summary>
    internal int ToLogical(int device) => (int)Math.Round(device * 96.0 / Math.Max(DeviceDpi, 1));

    /// <summary>
    /// Vykona akciu neskor v slucke sprav (pohlad, ktory ju spustil, sa medzitym moze presunut alebo zrusit); bez handle
    /// hned.
    /// </summary>
    internal void Post(Action action)
    {
        if (IsHandleCreated && !IsDisposed)
            BeginInvoke(action);
        else if (!IsDisposed)
            action();
    }

    /// <summary>
    /// Zmena modelu: po akcii odstrani prazdne skupiny, zosuladi prvky, prepocita rozlozenie a ohlasi panely so zmenenym
    /// stavom alebo stranou.
    /// </summary>
    private void Change(Action action)
    {
        var before = _panels.Select(p => new PanelSnapshot(p, p.State, p.Side)).ToList();
        var focused = _panels.Find(p => p.ContainsFocus);
        action();
        Prune();
        SyncViews();
        PerformLayout();
        Invalidate();
        if (focused is not null && !focused.ContainsFocus)
            RestoreFocus(focused);

        foreach (var snapshot in before)
            if (snapshot.Panel.State != snapshot.State || snapshot.Panel.Side != snapshot.Side)
                OnPanelStateChanged(new ExDockPanelEventArgs(snapshot.Panel));
        foreach (var panel in _panels.Where(p => before.TrueForAll(s => s.Panel != p) && p.State != ExDockState.Hidden).ToList())
            OnPanelStateChanged(new ExDockPanelEventArgs(panel));
    }

    private readonly record struct PanelSnapshot(ExDockPanel Panel, ExDockState State, ExDockSide Side);

    /// <summary>
    /// Vyberie zdroj presunu: celu skupinu (vyberie ju z jej miesta) alebo jeden panel (nova skupina s nim).
    /// </summary>
    private DockGroup Take(DockGroup source, ExDockPanel? panel)
    {
        if (panel is null || source.Panels.Count == 1)
        {
            DetachGroup(source);
            return source;
        }

        source.Panels.Remove(panel);
        source.FixActive();
        var group = new DockGroup { HomeSide = source.Side, AutoHideSize = source.AutoHideSize };
        group.Panels.Add(panel);
        panel.Group = group;
        panel.IsClosed = false;
        group.Active = panel;
        return group;
    }

    /// <summary>
    /// Vyberie skupinu zo zony alebo plavajuceho okna (skupina zostane bez miesta, kym sa nevlozi inam).
    /// </summary>
    private void DetachGroup(DockGroup group)
    {
        if (_popup.Group == group)
            HidePopup();
        if (group.Zone is { } zone)
        {
            group.HomeSide = group.Side;
            DockTree.Remove(group);
            group.Zone = null;
        }

        if (group.IsFloating)
        {
            group.IsFloating = false;
            _floating.Remove(group);
            CloseFloatForm(group);
        }

        group.AutoHide = false;
        if (group.View is { } view)
            view.Parent = null;
    }

    /// <summary>
    /// Vlozi skupinu na koniec zony pozdlz jej strany (vlavo a vpravo pod ostatne, hore a dole vpravo od nich).
    /// </summary>
    private static void PlaceAtEnd(DockZone zone, DockGroup group) =>
        PlaceAtEdge(zone, group, DockLayoutEngine.IsHorizontalSide(zone.Side) ? ExDockSide.Bottom : ExDockSide.Right);

    /// <summary>
    /// Vlozi skupinu na okraj celej zony: pozdlz strany na zaciatok alebo koniec, napriec stranou ako novy stlpec alebo
    /// riadok (vonkajsi okraj alebo pri dokumente).
    /// </summary>
    private static void PlaceAtEdge(DockZone zone, DockGroup group, ExDockSide direction)
    {
        DockTree.InsertAtEdge(zone.Root, group, direction);
        Placed(zone, group);
    }

    /// <summary>
    /// Rozdeli cielovu skupinu a skupinu vlozi na stranu <paramref name="direction" /> od nej.
    /// </summary>
    private static void PlaceBeside(DockGroup target, DockGroup group, ExDockSide direction)
    {
        DockTree.InsertBeside(target, group, direction);
        Placed(target.Zone!, group, target.Side);
    }

    /// <summary>
    /// Rozdeli dokument a skupinu vlozi na stranu <paramref name="direction" /> od neho (ako Visual Studio).
    /// </summary>
    private void PlaceByDocument(DockGroup group, ExDockSide direction)
    {
        DockTree.InsertBeside(_center.Document!, group, direction, DocumentShare);
        Placed(_center, group, direction);
    }

    // skupina vlozena do stromu zony je ukotvena; pri dokumente si pamata stranu (lista auto-hide, navrat)
    private static void Placed(DockZone zone, DockGroup group, ExDockSide? side = null)
    {
        group.Zone = zone;
        group.HomeSide = zone.IsCenter ? side ?? group.HomeSide : zone.Side;
        group.IsFloating = false;
        group.AutoHide = false;
    }

    private void MakeFloating(DockGroup group, Rectangle bounds)
    {
        group.Zone = null;
        group.AutoHide = false;
        group.IsFloating = true;
        group.FloatBounds = bounds;
        if (!_floating.Contains(group))
            _floating.Add(group);
    }

    /// <summary>
    /// Presunie panel alebo vsetkych clenov skupiny (aj skrytych) medzi karty cielovej skupiny.
    /// </summary>
    private static void MoveIntoGroup(DockGroup source, ExDockPanel? panel, DockGroup target)
    {
        if (source == target)
            return;

        var active = panel ?? source.Active;
        var moving = panel is null ? source.Panels.ToList() : [panel];
        foreach (var member in moving)
        {
            source.Panels.Remove(member);
            target.Panels.Add(member);
            member.Group = target;
        }

        source.FixActive();
        if (active is { IsClosed: false })
            target.Active = active;
        target.FixActive();
    }

    /// <summary>
    /// Odstrani skupiny bez clenov.
    /// </summary>
    private void Prune()
    {
        foreach (var group in AllGroups.Where(g => g.Panels.Count == 0).ToList())
            DiscardGroup(group);
    }

    private void DiscardGroup(DockGroup group)
    {
        if (_popup.Group == group)
            HidePopup();
        CloseFloatForm(group);
        if (group.View is { } view)
        {
            // panely patria spravcovi, nie pohladu - nesmu sa zrusit s nim
            foreach (var panel in view.Controls.OfType<ExDockPanel>().ToList())
                view.Controls.Remove(panel);
            view.Parent = null;
            view.Dispose();
            group.View = null;
        }

        DockTree.Remove(group);
        group.Zone = null;
        _floating.Remove(group);
    }

    private DockGroupView EnsureView(DockGroup group) => group.View ??= new DockGroupView(this, group);

    /// <summary>
    /// Zosuladi prvky s modelom: ukotvene skupiny su detmi spravcu, zbalene len vo vysunutom paneli, plavajuce
    /// v oknach (ked je formular spravcu zobrazeny).
    /// </summary>
    private void SyncViews()
    {
        if (_popup.Group is { } shown && (!shown.AutoHide || shown.Zone is null || !shown.HasVisiblePanels))
            HidePopup();

        SuspendLayout();
        foreach (var group in AllGroups.ToList())
        {
            group.FixActive();
            if (group.IsFloating)
            {
                SyncFloating(group);
            }
            else
            {
                CloseFloatForm(group);
                if (group.AutoHide)
                {
                    if (group.View is { } view && view.Parent is not null && view.Parent != _popup)
                        view.Parent = null;
                }
                else if (group.HasVisiblePanels)
                {
                    var view = EnsureView(group);
                    view.Mode = DockViewMode.Docked;
                    view.Dock = DockStyle.None;
                    if (view.Parent != this)
                        Controls.Add(view);
                }
                else if (group.View is { Parent: not null } hidden)
                {
                    hidden.Parent = null;
                }
            }

            group.View?.SyncPanels();
        }

        _popup.BringToFront();
        ResumeLayout(false);
    }

    /// <summary>
    /// Plavajuce okna sa vytvaraju az pri zobrazenom formulari spravcu (vlastnik okna).
    /// </summary>
    private bool CanShowFloating => IsHandleCreated && !DesignMode && FindForm() is { IsHandleCreated: true, Visible: true };

    private void SyncFloating(DockGroup group)
    {
        if (!group.HasVisiblePanels)
        {
            CloseFloatForm(group);
            return;
        }

        var view = EnsureView(group);
        if (!CanShowFloating)
        {
            if (view.Parent is not DockFloatForm)
                view.Parent = null;
            view.Mode = DockViewMode.Floating;
            return;
        }

        if (group.FloatForm is { IsDisposed: false } existing)
        {
            existing.Attach(view);
            return;
        }

        var form = new DockFloatForm(this, group);
        group.FloatForm = form;
        form.Bounds = ValidFloatBounds(group.FloatBounds);
        form.Attach(view);
        form.Show(FindForm());
    }

    private static void CloseFloatForm(DockGroup group)
    {
        if (group.FloatForm is not { } form)
            return;
        group.FloatForm = null;
        if (!form.IsDisposed)
            form.CloseByManager();
    }

    /// <summary>
    /// Poloha plavajuceho okna: ulozena, inak pri spravcovi; okno mimo obrazoviek sa presunie nad formular.
    /// </summary>
    private Rectangle ValidFloatBounds(Rectangle bounds)
    {
        var owner = FindForm()?.Bounds ?? RectangleToScreen(ClientRectangle);
        if (bounds.Width <= 0 || bounds.Height <= 0)
        {
            var size = new Size(ToDevice(300), ToDevice(220));
            bounds = new Rectangle(owner.X + (owner.Width - size.Width) / 2, owner.Y + (owner.Height - size.Height) / 2, size.Width, size.Height);
        }

        if (!Screen.AllScreens.Any(s => s.WorkingArea.IntersectsWith(bounds)))
            bounds.Location = new Point(owner.X + (owner.Width - bounds.Width) / 2, owner.Y + (owner.Height - bounds.Height) / 2);
        return bounds;
    }

    private static Rectangle ScreenBoundsOf(Control? control) =>
        control is { Parent: not null, IsHandleCreated: true, Visible: true } ? control.RectangleToScreen(control.ClientRectangle) : Rectangle.Empty;

    /// <summary>
    /// Vysunie zbalenu skupinu s panelom.
    /// </summary>
    internal void ShowPopup(DockGroup group, ExDockPanel panel, bool focus)
    {
        if (group.Zone is null || panel.Group != group || panel.IsClosed)
            return;

        group.Active = panel;
        var view = EnsureView(group);
        _popup.Attach(group, view, group.Side);
        view.SyncPanels();
        LayoutPopup();
        _popup.Visible = true;
        _popup.BringToFront();
        _popupTimer.Start();
        Invalidate();
        if (focus)
            FocusPanel(panel);
    }

    /// <summary>
    /// Zasunie vysunutu skupinu.
    /// </summary>
    internal void HidePopup()
    {
        _popupTimer.Stop();
        if (_popup.Group is null && !_popup.Visible)
            return;
        var focused = _popup.ContainsFocus;
        _popup.Visible = false;
        _popup.Detach();
        Invalidate();
        if (focused)
            FocusDocument();
    }

    private bool _menuOpen;

    private void PopupTimer_Tick(object? sender, EventArgs e)
    {
        if (!_popup.Visible)
        {
            _popupTimer.Stop();
            return;
        }

        // zasunie sa, ked fokus a mys odisli (ako Visual Studio)
        if (_popup.ContainsFocus || _popup.IsResizing || _menuOpen)
            return;
        var mouse = PointToClient(MousePosition);
        if (_popup.Bounds.Contains(mouse) || _stripButtons.Exists(b => b.Group == _popup.Group && b.Bounds.Contains(mouse)))
            return;
        HidePopup();
    }

    /// <summary>
    /// Presunie fokus do panela (na jeho posledny aktivny prvok alebo prvy prvok s fokusom).
    /// </summary>
    internal static void FocusPanel(ExDockPanel panel)
    {
        if (!panel.IsHandleCreated || !panel.Visible || panel.ContainsFocus)
            return;
        if (panel.ActiveControl is { CanFocus: true } active)
        {
            active.Focus();
            return;
        }

        if (!panel.SelectNextControl(null, true, true, true, false))
            panel.Focus();
    }

    /// <summary>
    /// Panel s fokusom sa presunul alebo zmizol: presunuty dostane fokus spat (plavajuce okno sa aktivuje), inak ho
    /// dostane dokument.
    /// </summary>
    private void RestoreFocus(ExDockPanel panel)
    {
        switch (panel.State)
        {
            case ExDockState.Docked:
                FocusPanel(panel);
                break;
            case ExDockState.Floating when panel.Group?.FloatForm is { IsDisposed: false } form:
                form.Activate();
                FocusPanel(panel);
                break;
            default:
                FocusDocument();
                break;
        }
    }

    private void FocusDocument()
    {
        if (_document.IsHandleCreated && _document.Visible && !_document.ContainsFocus)
            _document.SelectNextControl(null, true, true, true, false);
    }

    private void SetActivePanel(ExDockPanel? panel)
    {
        if (_activePanel == panel)
            return;
        _activePanel = panel;
        OnActivePanelChanged(EventArgs.Empty);
    }

    /// <summary>
    /// Fokus vstupil do pohladu skupiny.
    /// </summary>
    internal void OnViewEnter(DockGroupView view) => SetActivePanel(view.Group.Active);

    /// <summary>
    /// Panel zmenil nazov alebo obrazok: prekresli zahlavie, karty a listu.
    /// </summary>
    internal void OnPanelAppearanceChanged(ExDockPanel panel)
    {
        if (panel.Group is not { } group)
            return;
        group.View?.PerformLayout();
        group.View?.Invalidate();
        if (group.FloatForm is { IsDisposed: false } form && group.Active == panel)
            form.Text = panel.Text;
        if (group.AutoHide)
            PerformLayout();
    }

    /// <inheritdoc />
    protected override void OnVisibleChanged(EventArgs e)
    {
        base.OnVisibleChanged(e);
        // plavajuce okna z nacitaneho rozlozenia sa ukazu, az ked je formular zobrazeny
        if (Visible && _floating.Count > 0)
            Post(() =>
            {
                SuspendLayout();
                foreach (var group in _floating.ToList())
                    SyncFloating(group);
                ResumeLayout(false);
            });
    }
}
