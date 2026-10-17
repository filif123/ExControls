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
`ExTheme` is a color palette (panels, boxes, buttons, border, highlight, marks, labels, system look, dark
scroll bars). Ex* controls implement `IThemeable` and style themselves; `ExThemer.Apply(form.Controls, theme)`
walks the controls. Controls outside the library (DataGridView, ToolStrip, Panel…) are styled by handlers the
application registers with `ExThemer.Register<T>` - the handler of the nearest registered base type is used,
a container handler themes its children by calling `ExThemer.Apply` again.

## Custom title bar
`ExTitleBar` placed directly on an `ExForm` (docked to the top) replaces the system caption, like the title bar
of JetBrains Rider or Visual Studio: the window icon (system menu) on the left, the window buttons on the right and any
controls in between - typically the main `MenuStrip`. The text of the form is drawn in the free space.

* The system frame stays: shadow, rounded corners (Windows 11), Aero Snap, Snap Layouts on the maximize button
  (Windows 11), window animations and resizing at all edges including the top one.
* The free space of the title bar and empty parts of tool strips in it drag the window (double click maximizes,
  right click opens the system menu); menu items and other controls stay clickable. Only tool strips placed directly
  in the title bar are handled this way, their items are always laid out horizontally.
* The buttons follow `ControlBox`, `MinimizeBox` and `MaximizeBox` of the form, the icon follows `ShowIcon`.
* It works on Windows 10+ with `FormBorderStyle.Sizable` (`ExForm.UsesCustomTitleBar`); otherwise the form keeps
  the system caption and the title bar is a plain panel. Hiding the title bar brings the system caption back.
* The outer size of the form stays as designed - the client area grows by the height of the removed system caption.
* Colors: `BackColor` and `ForeColor`, optionally `InactiveForeColor`, `ButtonHoverBackColor`, `ButtonPressedBackColor`,
  `CloseButtonHoverBackColor` and `TopBorderColor` (Windows 10 only, Windows 11 draws the border itself) - empty
  values are derived from `BackColor` and `ForeColor`. `ExTitleBar` implements `IThemeable` and passes the theme
  to the controls in it.
* `ExForm` has `WS_EX_COMPOSITED`: an empty `TextBox` with `PlaceholderText` repaints the whole form endlessly
  (the text blinks) - use `ExTextBox.HintText` instead.
* Designer: a new title bar is placed above other controls docked to the top; its smart tag adds the main menu.

## Breaking changes in 2.0
* `System.Linq.Enumerable` extensions renamed to `ExEnumerable` (the second `Enumerable` class caused CS0433).
* `IsExternalInit` polyfill (.NET Framework) is internal.
* Removed unused theming experiments `ExStyle`, `ExStyleManager`, `IStylable`, `ISupportsDefaultStyle`,
  `ExApplication`, `ExAppTheme` and the base class `ExStyleOld` (its members moved to `ExComboBoxStyle`).
* Removed the unfinished `ExFormDesigner` (work in progress form with custom title bar), replaced by `ExTitleBar`.
* `ExTools.SetTitlebarAndBorderColor(Form, ...)` no longer swaps the text and border colors.

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
