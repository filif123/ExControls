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
* TitleBar (custom title bar of `ExForm` with the main menu, see below)

These components have been extended:
* FolderBrowserDialog

## Themes
`ExTheme` is a color palette. Ex* controls implement `IThemeable` and style themselves, `ExThemer.Apply(form.Controls, theme)`
applies the theme to a form. Other controls (DataGridView, ToolStrip…) are styled by handlers the application registers
with `ExThemer.Register<T>`.

## Custom title bar
`ExTitleBar` placed directly on an `ExForm` replaces the system caption like in JetBrains Rider or Visual Studio: window
icon, form text, window buttons and controls such as the main `MenuStrip` in between. The system frame stays (shadow,
Aero Snap, Snap Layouts, animations).

* Works on Windows 10+ with `FormBorderStyle.Sizable`, otherwise the form keeps the system caption.
* The outer size of the form stays as designed, the client area grows by the height of the system caption.
* Empty parts of tool strips placed directly in the title bar drag the window. In an inactive window their items
  are dimmed through `ForeColor` - a renderer that picks the text color itself won't show it.
* `ExForm.UseCompositedPainting` (`WS_EX_COMPOSITED`, on by default): an empty `TextBox` with `PlaceholderText`
  repaints such a form endlessly - use `ExTextBox.HintText`.

## Breaking changes in 2.0
* `System.Linq.Enumerable` extensions renamed to `ExEnumerable`, `IsExternalInit` polyfill is internal.
* Removed unused theming experiments (`ExStyle`, `ExStyleManager`, `IStylable`, `ExApplication`, `ExAppTheme`…) and the
  unfinished `ExFormDesigner` (replaced by `ExTitleBar`).
* `ExTools.SetTitlebarAndBorderColor(Form, ...)` no longer swaps the text and border colors.

## How to install?
```
dotnet add package ExControls
```

## Development

### Designer extension
The .NET WinForms designer runs out of process and loads custom type editors only from NuGet packages.
`ExControls.Designer.Package` builds `artifacts/packages/ExControls.Designer.<version>.nupkg`; projects that design
forms with ExControls import `ExControls.Designer.targets` next to their `ExControls` project reference.

### After changing ExControls
The designer loads `ExControls.dll` from the build output: build the solution and open the form again (if the designer
still shows the old controls, restart Visual Studio).

Increase `ExControlsDesignerVersion` in `ExControls.Designer.targets`, build the solution and restart Visual Studio when
* the version of ExControls changes (`<Version>` in `ExControls.csproj` and `AssemblyVersion` in `Properties/AssemblyInfo.cs`
  always together) - the server part of the package is bound to it,
* `ExControls.Designer.Client`, `Server` or `Protocol` change, or the ExControls code they use (`ExControls.Editors`,
  `ExOptionsView` panels).

NuGet and Visual Studio cache the package by version, a package rebuilt with the same version is ignored. An outdated
package shows "Failed to launch the design tools server process" on every form.
