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

These components have been extended:

- FolderBrowserDialog

## Themes

`ExTheme` is a color palette. Ex* controls implement `IThemeable` and style themselves;
`ExThemer.Apply(form.Controls, theme)` walks the controls. Controls outside the library (DataGridView, ToolStrip,
Panel…) are styled by handlers registered with `ExThemer.Register<T>`.

## Custom title bar

`ExTitleBar` placed directly on an `ExForm` (docked to the top) replaces the system caption: the window icon on the left,
the window buttons on the right and controls such as the main `MenuStrip` in between. The system frame stays (shadow,
Aero Snap, Snap Layouts, animations); the free space of the title bar and empty parts of its tool strips drag the window.
It works on Windows 10+ with `FormBorderStyle.Sizable`, otherwise the form keeps the system caption. Do not use
`TextBox.PlaceholderText` on an `ExForm` (`WS_EX_COMPOSITED` repaints it endlessly) - use `ExTextBox.HintText`.

## Breaking changes in 2.0

- `System.Linq.Enumerable` extensions renamed to `ExEnumerable`.
- `IsExternalInit` polyfill (.NET Framework) is internal.
- Removed unused `ExStyle`, `ExStyleManager`, `IStylable`, `ISupportsDefaultStyle`, `ExApplication`, `ExAppTheme`,
  `ExStyleOld` (members moved to `ExComboBoxStyle`) and the unfinished `ExFormDesigner` (replaced by `ExTitleBar`).
- `ExTools.SetTitlebarAndBorderColor(Form, ...)` no longer swaps the text and border colors.
