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

These components have been extended:

- FolderBrowserDialog

## Themes

`ExTheme` is a color palette. Ex* controls implement `IThemeable` and style themselves;
`ExThemer.Apply(form.Controls, theme)` walks the controls. Controls outside the library (DataGridView, ToolStrip,
Panel…) are styled by handlers registered with `ExThemer.Register<T>`.

## Breaking changes in 2.0

- `System.Linq.Enumerable` extensions renamed to `ExEnumerable`.
- `IsExternalInit` polyfill (.NET Framework) is internal.
- Removed unused `ExStyle`, `ExStyleManager`, `IStylable`, `ISupportsDefaultStyle`, `ExApplication`, `ExAppTheme`,
  `ExStyleOld` (members moved to `ExComboBoxStyle`) and the unfinished `ExFormDesigner`.
