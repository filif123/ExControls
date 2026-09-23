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

## How to install?
```
dotnet add package ExControls
```

## Designer extension (.NET)
The .NET WinForms designer runs out of process and loads custom type editors only from NuGet packages.
`ExControls.Designer.Package` builds `artifacts/packages/ExControls.Designer.<version>.nupkg`
(client part for Visual Studio, server part for DesignToolsServer); projects that design forms with
ExControls import `ExControls.Designer.targets` next to their `ExControls` project reference.

After changing `ExControls.Designer.Client/Server/Protocol`, increase `ExControlsDesignerVersion`
in `ExControls.Designer.targets`, rebuild the package and restart Visual Studio -
NuGet and the designer cache packages by version.
