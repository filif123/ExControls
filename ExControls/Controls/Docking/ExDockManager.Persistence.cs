using ExControls.Docking;

namespace ExControls;

// ulozenie a obnovenie rozlozenia
public partial class ExDockManager
{
    /// <summary>
    /// Saves the layout (zones and their sizes, groups, tabs, collapsed and hidden panels, floating windows) to a single-line
    /// XML string for the application settings. Panels are identified by <see cref="ExDockPanel.Key" />; sizes are in logical
    /// pixels, so the layout survives a DPI change.
    /// </summary>
    /// <returns>the layout for <see cref="LoadLayout" /></returns>
    public string SaveLayout()
    {
        var data = new DockLayoutData();
        foreach (var side in AllSides)
        {
            var zone = Zone(side);
            var zoneData = new DockLayoutData.ZoneData { Side = side, Size = zone.Size };
            zoneData.Groups.AddRange(zone.Groups.Select(ToData));
            data.Zones.Add(zoneData);
        }

        foreach (var group in _floating)
        {
            var bounds = group.FloatForm is { IsDisposed: false, WindowState: FormWindowState.Normal } form ? form.Bounds : group.FloatBounds;
            data.Floats.Add(new DockLayoutData.FloatData
            {
                Bounds = new Rectangle(bounds.X, bounds.Y, ToLogical(bounds.Width), ToLogical(bounds.Height)),
                Side = group.HomeSide,
                Group = ToData(group)
            });
        }

        return DockLayoutXml.Write(data);
    }

    /// <summary>
    /// Restores a layout saved by <see cref="SaveLayout" />. Call it after adding all panels: panels missing in the layout
    /// stay where they are, keys of panels not added are ignored. An empty string keeps the current layout.
    /// </summary>
    /// <param name="layout">saved layout; <see langword="null" /> or empty = no change</param>
    /// <exception cref="FormatException">the text is not a dock layout of a supported version (the layout does not change)</exception>
    public void LoadLayout(string? layout)
    {
        if (string.IsNullOrWhiteSpace(layout))
            return;

        var data = DockLayoutXml.Parse(layout!);
        Change(() => Apply(data));
    }

    private static DockLayoutData.GroupData ToData(DockGroup group)
    {
        var data = new DockLayoutData.GroupData
        {
            Weight = group.Weight,
            AutoHide = group.AutoHide,
            AutoHideSize = group.AutoHideSize,
            Active = group.Active?.Key
        };
        data.Panels.AddRange(group.Panels.Select(p => new DockLayoutData.PanelData(p.Key, p.IsClosed)));
        return data;
    }

    private void Apply(DockLayoutData data)
    {
        HidePopup();
        var byKey = _panels.ToDictionary(p => p.Key, StringComparer.Ordinal);
        var groups = data.Zones.SelectMany(z => z.Groups).Concat(data.Floats.Select(f => f.Group));
        foreach (var entry in groups.SelectMany(g => g.Panels))
        {
            // spomenute panely sa vyberu zo svojich skupin, ostatne ostanu na mieste
            if (!byKey.TryGetValue(entry.Key, out var panel) || panel.Group is not { } old)
                continue;
            old.Panels.Remove(panel);
            old.FixActive();
            panel.Group = null;
        }

        Prune();
        foreach (var zoneData in data.Zones)
        {
            var zone = Zone(zoneData.Side);
            zone.Size = zoneData.Size;
            var index = 0;
            foreach (var groupData in zoneData.Groups)
            {
                if (Build(groupData, byKey) is not { } group)
                    continue;
                group.Zone = zone;
                group.HomeSide = zone.Side;
                zone.Groups.Insert(index++, group);
            }
        }

        foreach (var floatData in data.Floats)
        {
            if (Build(floatData.Group, byKey) is not { } group)
                continue;
            group.HomeSide = floatData.Side;
            var bounds = floatData.Bounds;
            MakeFloating(group, new Rectangle(bounds.X, bounds.Y, ToDevice(bounds.Width), ToDevice(bounds.Height)));
        }
    }

    private static DockGroup? Build(DockLayoutData.GroupData data, Dictionary<string, ExDockPanel> panels)
    {
        var group = new DockGroup { Weight = data.Weight, AutoHide = data.AutoHide, AutoHideSize = data.AutoHideSize };
        foreach (var entry in data.Panels)
        {
            if (!panels.TryGetValue(entry.Key, out var panel) || panel.Group is not null)
                continue;
            panel.IsClosed = entry.Hidden;
            panel.Group = group;
            group.Panels.Add(panel);
        }

        if (group.Panels.Count == 0)
            return null;
        group.Active = group.Panels.Find(p => !p.IsClosed && string.Equals(p.Key, data.Active, StringComparison.Ordinal));
        group.FixActive();
        return group;
    }
}
