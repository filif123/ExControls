using ExControls.Docking;

namespace ExControls;

// ulozenie a obnovenie rozlozenia
public partial class ExDockManager
{
    /// <summary>
    /// Saves the layout (zones and their sizes, nested rows and columns of groups, tabs, collapsed and hidden panels, floating
    /// windows) to a single-line
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
            zoneData.Nodes.AddRange(zone.Root.Children.Select(ToData));
            data.Zones.Add(zoneData);
        }

        // stred len so skupinami pri dokumente
        if (DockTree.Groups(_center.Root).Any())
            data.Center = _center.Root.Children.Select(ToData).ToList();

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

    private static DockLayoutData.NodeData ToData(DockNode node)
    {
        if (node is DockDocumentNode)
            return new DockLayoutData.DocumentData { Weight = node.Weight };

        if (node is DockSplit split)
        {
            var splitData = new DockLayoutData.SplitData { Weight = split.Weight, SideBySide = split.SideBySide };
            splitData.Children.AddRange(split.Children.Select(ToData));
            return splitData;
        }

        return ToData((DockGroup)node);
    }

    private static DockLayoutData.GroupData ToData(DockGroup group)
    {
        var data = new DockLayoutData.GroupData
        {
            Weight = group.Weight,
            AutoHide = group.AutoHide,
            AutoHideSize = group.AutoHideSize,
            Active = group.Active?.Key,
            Side = group.Zone is { IsCenter: true } ? group.HomeSide : null
        };
        data.Panels.AddRange(group.Panels.Select(p => new DockLayoutData.PanelData(p.Key, p.IsClosed)));
        return data;
    }

    private void Apply(DockLayoutData data)
    {
        HidePopup();
        var byKey = _panels.ToDictionary(p => p.Key, StringComparer.Ordinal);
        foreach (var entry in data.AllGroups.SelectMany(g => g.Panels))
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
            // uzly z rozlozenia pred uzlami panelov, ktore v nom nie su
            var index = 0;
            foreach (var nodeData in zoneData.Nodes)
                if (Build(nodeData, byKey) is { } node)
                    zone.Root.Insert(index++, node);
            foreach (var group in DockTree.Groups(zone.Root))
            {
                group.Zone = zone;
                group.HomeSide = zone.Side;
            }

            DockTree.Normalize(zone.Root);
        }

        if (data.Center is { } centerData)
            ApplyCenter(centerData, byKey);

        foreach (var floatData in data.Floats)
        {
            if (Build(floatData.Group, byKey) is not { } group)
                continue;
            group.HomeSide = floatData.Side;
            var bounds = floatData.Bounds;
            MakeFloating(group, new Rectangle(bounds.X, bounds.Y, ToDevice(bounds.Width), ToDevice(bounds.Height)));
        }
    }

    /// <summary>
    /// Stred z rozlozenia: dokument na ulozene miesto, skupiny pri nom so svojou stranou.
    /// </summary>
    private void ApplyCenter(List<DockLayoutData.NodeData> nodes, Dictionary<string, ExDockPanel> panels)
    {
        var document = _center.Document!;
        DockTree.Remove(document);
        var index = 0;
        foreach (var nodeData in nodes)
            if (Build(nodeData, panels, document) is { } node)
                _center.Root.Insert(index++, node);
        if (document.Parent is null)
            _center.Root.Add(document);

        foreach (var group in DockTree.Groups(_center.Root))
            group.Zone = _center;
        DockTree.Normalize(_center.Root);
    }

    // uzol stromu z rozlozenia; bez pridanych panelov nevznikne, dokument len v strede
    private static DockNode? Build(DockLayoutData.NodeData data, Dictionary<string, ExDockPanel> panels, DockDocumentNode? document = null)
    {
        switch (data)
        {
            case DockLayoutData.DocumentData:
                if (document is null || document.Parent is not null)
                    return null;
                document.Weight = data.Weight;
                return document;
            case DockLayoutData.GroupData groupData:
                if (Build(groupData, panels) is not { } group)
                    return null;
                if (groupData.Side is { } side)
                    group.HomeSide = side;
                return group;
        }

        var splitData = (DockLayoutData.SplitData)data;
        var split = new DockSplit(splitData.SideBySide) { Weight = splitData.Weight };
        foreach (var child in splitData.Children)
            if (Build(child, panels, document) is { } node)
                split.Add(node);
        return split.Children.Count > 0 ? split : null;
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
