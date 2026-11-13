Extended controls for Windows Forms.

This library features extensions over some of the classic controls, most of which expand their ability to customize visuals and colors. The ExControls library was originally created as part of the GVDEditor program for the dark mode of the graphical interface.

These controls have been extended:

- ComboBox (also its variant in ToolStrip and DataGridView)
- DateTimePicker (partial)
- GroupBox
- CheckBox (also its variant in DataGridView)
- CheckedListBox
- MaskedTextBox
- NumericUpDown
- RadioButton
- TabControl
- TextBox

These controls have been added:

- LineSeparator
- TitleBar (custom title bar of `ExForm` with the main menu)
- DockManager (dockable tool panels like Visual Studio or Rider)

These components have been extended:

- FolderBrowserDialog

## Themes

`ExTheme` is a color palette. Ex* controls implement `IThemeable` and style themselves;
`ExThemer.Apply(form.Controls, theme)` walks the controls. Controls outside the library (DataGridView, ToolStrip,
Panel…) are styled by handlers registered with `ExThemer.Register<T>`.

## Dock manager

`ExDockManager` lays out a window into the main content (`Document`) and tool panels (`ExDockPanel`) docked to the
left, right, top or bottom. Left and right zones span the full height, top and bottom ones lie between them; a zone
splits into nested rows and columns of groups, more panels in one group are tabs. The user resizes zones and groups with
splitters, hides a panel (×), collapses its group to the strip at the edge (pin, the panel slides out on a click) and
drags the caption (whole group) or a tab (one panel) with docking guides like in Visual Studio: a compass over a group
adds a tab (center) or splits the group (arrows), a compass over the document splits the document (the panel takes
only the height or width of the document, not the whole side), the guides at the edges add a column or row at the outer
edge of the zone; out of the guides it becomes a floating window (`AllowFloating`). The guides at the edges stay
visible, a compass near an edge moves away from them and stays while the cursor is on it. Panels are identified by
`Key`; the layout is saved as a single-line XML string with sizes in logical pixels:

```csharp
var dock = new ExDockManager { Dock = DockStyle.Fill, Strings = new ExDockStrings { Hide = "Skryť" /* ... */ } };
Controls.Remove(content);
dock.Document.Controls.Add(content);          // main content of the window
Controls.Add(dock);
dock.BringToFront();                          // Fill after the docked tool strips

var errors = new ExDockPanel("errors", "Error List");
errors.Controls.Add(new ErrorListView { Dock = DockStyle.Fill });
dock.AddPanel(errors, ExDockSide.Bottom);     // AddPanel(panel, tabWith) adds a tab
dock.AddPanel(output, errors, ExDockSide.Right); // splits the group of errors, output on its right
dock.SetZoneSize(ExDockSide.Bottom, 180);
dock.HidePanel(errors);                       // shown later by ShowPanel(errors)
dock.LoadLayout(settings.DockLayout);         // after adding all panels; empty = keep, invalid = FormatException
// ...
settings.DockLayout = dock.SaveLayout();
```

The manager owns its panels (disposing it disposes the hidden ones too), applies a theme to the document and to all
panels (`IThemeable`) and raises `PanelStateChanged` (e.g. for a checked menu item), `PanelClosing` (the user hides a
panel, can be cancelled) and `ActivePanelChanged`. `ShowPanel`, `HidePanel`, `DockPanel`, `FloatPanel` and
`SetAutoHide` change the layout from code. The manager is created in code (it is not in the toolbox).

## Custom title bar

`ExTitleBar` placed directly on an `ExForm` (docked to the top) replaces the system caption: the window icon on the left,
the window buttons on the right and controls such as the main `MenuStrip` in between (in a narrow window it collapses
into ☰). The system frame stays (shadow,
Aero Snap, Snap Layouts, animations); the free space of the title bar and empty parts of its tool strips drag the window.
It works on Windows 10+ with any `FormBorderStyle` except `None`; `TitleBarBackColor` and `TitleBarForeColor` of the
form color the title bar (an active window otherwise follows the Windows accent color on title bars), `FormStyle`
(Mica…) shows only in the system caption. The designer shows
the form without the system caption and frame, so the designed `ClientSize` matches the runtime window. Do not use
`TextBox.PlaceholderText` on an `ExForm` with `UseCompositedPainting` (`WS_EX_COMPOSITED` repaints it endlessly) -
use `ExTextBox.HintText`.

## Visual Studio designer

In .NET projects the package brings its own designer extension (collection editor of `ExOptionsView.Panels`),
Visual Studio loads it automatically.

## Breaking changes in 2.0

- `System.Linq.Enumerable` extensions renamed to `ExEnumerable`.
- `IsExternalInit` polyfill (.NET Framework) is internal.
- Removed unused `ExStyle`, `ExStyleManager`, `IStylable`, `ISupportsDefaultStyle`, `ExApplication`, `ExAppTheme`,
  `ExStyleOld` (members moved to `ExComboBoxStyle`) and the unfinished `ExFormDesigner` (replaced by `ExTitleBar`).
- `ExTools.SetTitlebarAndBorderColor(IntPtr, ...)` no longer swaps the text and border colors (wrong DWM attribute
  numbers, the `Form` overload compensated them).
- `ExForm.DarkTitleBar` and `TitleBarBackColor` no longer reset each other (the caption color takes precedence), empty
  title bar colors restore the system colors also after the form is shown.

## License

MIT. Toolbox icons come from .NET Windows Forms and Fluent UI System Icons (both MIT), see THIRD-PARTY-NOTICES.md
in the package.
