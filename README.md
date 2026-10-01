# ExControls
Extended controls for Windows Forms.

This library features extensions over some of the classic controls, most of which expand their ability to customize visuals and colors. The ExControls library was originally created as part of the GVDEditor program for the dark mode of the graphical interface.

These controls have been extended:
* ComboBox (also its variant in ToolStrip and DataGridView)
* DateTimePicker (partial)
* GroupBox
* CheckBox (also its variant in DataGridView)
* CheckedListBox
* MaskedTextBox
* NumericUpDown
* RadioButton
* TabControl
* TextBox

These controls have been added:
* LineSeparator

These components have been extended:
* FolderBrowserDialog

## Themes
`ExTheme` is a color palette (panels, boxes, buttons, border, highlight, marks, labels, system look, dark
scroll bars). Ex* controls implement `IThemeable` and style themselves; `ExThemer.Apply(form.Controls, theme)`
walks the controls. Controls outside the library (DataGridView, ToolStrip, Panel…) are styled by handlers the
application registers with `ExThemer.Register<T>` - the handler of the nearest registered base type is used,
a container handler themes its children by calling `ExThemer.Apply` again.

## Breaking changes in 2.0
* `System.Linq.Enumerable` extensions renamed to `ExEnumerable` (the second `Enumerable` class caused CS0433).
* `IsExternalInit` polyfill (.NET Framework) is internal.
* Removed unused theming experiments `ExStyle`, `ExStyleManager`, `IStylable`, `ISupportsDefaultStyle`,
  `ExApplication`, `ExAppTheme` and the base class `ExStyleOld` (its members moved to `ExComboBoxStyle`).
* Removed the unfinished `ExFormDesigner` (work in progress form with custom title bar, kept in `ExControlsTester/WorkInProgress`).

## How to install?
```
dotnet add package ExControls
```

## Designer extension (.NET)
The .NET WinForms designer runs out of process and loads custom type editors only from NuGet packages.
`ExControls.Designer.Package` builds `artifacts/packages/ExControls.Designer.<version>.nupkg`
(client part for Visual Studio, server part for DesignToolsServer); projects that design forms with
ExControls import `ExControls.Designer.targets` next to their `ExControls` project reference.

After changing `ExControls.Designer.Client/Server/Protocol` or the `Version` of `ExControls`
(the server part is compiled against it), increase `ExControlsDesignerVersion`
in `ExControls.Designer.targets`, rebuild the package and restart Visual Studio -
NuGet and the designer cache packages by version.
