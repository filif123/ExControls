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

* Works on Windows 10+ with any `FormBorderStyle` except `None` (there the title bar is a plain panel). Window buttons
  and the icon follow `ControlBox`, `MinimizeBox`, `MaximizeBox`, `HelpButton`, `ShowIcon` and tool windows like the
  system caption.
* An active window shows the accent color when Windows shows it on title bars ("Show accent color on title bars and
  window borders"). `ExForm.TitleBarBackColor` and `TitleBarForeColor` take precedence. Tool strips in the title bar
  get a transparent background and the caption text color, other controls keep their colors.
* `ExForm.TitleBarBackColor` and `TitleBarForeColor` color the title bar,
  `TitleBarBorderColor` and `CornersType` work as with the system caption, `FormStyle` (Mica…) shows only in the
  system caption.
* The designer shows the form without the system caption and frame, at runtime the window has the system frame only on
  the left, right and bottom. The designed `ClientSize` is the same in both; DPI scaling scales it without the caption.
* A `MenuStrip` in the title bar collapses into one button (☰) at runtime when its items don't fit
  (`ExTitleBar.CollapseMenu`); the items move under the button, other items of the menu stay.
* Empty parts of tool strips placed directly in the title bar drag the window. In an inactive window their items
  are dimmed through `ForeColor` - a renderer that picks the text color itself won't show it.
* `ExForm.UseCompositedPainting` (`WS_EX_COMPOSITED`, on by default): an empty `TextBox` with `PlaceholderText`
  repaints such a form endlessly - use `ExTextBox.HintText`.

## Breaking changes in 2.0
* `System.Linq.Enumerable` extensions renamed to `ExEnumerable`, `IsExternalInit` polyfill is internal.
* Removed unused theming experiments (`ExStyle`, `ExStyleManager`, `IStylable`, `ExApplication`, `ExAppTheme`…) and the
  unfinished `ExFormDesigner` (replaced by `ExTitleBar`).
* `ExTools.SetTitlebarAndBorderColor(IntPtr, ...)` no longer swaps the text and border colors (wrong DWM attribute
  numbers, the `Form` overload compensated them).
* `ExForm.DarkTitleBar` and `TitleBarBackColor` no longer reset each other (the caption color takes precedence), empty
  title bar colors restore the system colors also after the form is shown.

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
