# Rozpracované: okno s vlastným záhlavím

`ExFormTest`, `TitleBar` a ich návrhári (`ExFormDesigner`, `TitleBarDesigner`) boli v knižnici ExControls
označené ako WORK IN PROGRESS. Z knižnice (a NuGet balíka) sú vyradené, kód je tu len ako archív –
**nekompiluje sa** (`Compile Remove` v `ExControlsTester.csproj`).

Pri dokončení ich treba vrátiť do knižnice: používajú interné `Win32` API a obrázok `Resources.qm`
z ExControls, návrhári na .NET závisia od `Microsoft.WinForms.Designer.SDK`. Verejné vlastnosti
musia mať `[Browsable(false)] [DesignerSerializationVisibility(Hidden)]` alebo `ShouldSerialize*` (WFO1000).

V etape 6 (2026-09-28) sa z knižnice odstránili aj nepoužité Win32 deklarácie, na ktoré tento kód odkazuje
(`WINDOWPLACEMENT`, `NCCALCSIZE_PARAMS`, `ShowWindowCommands`, `GetWindowPlacement`, `AdjustWindowRectEx`, väčšina
`WM`…) – pri dokončení ich treba doplniť späť do `Tools/Win32.*.cs`.
