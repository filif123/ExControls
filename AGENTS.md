# ExControls – agent guidelines

WinForms control library with dark mode support (the `ExControls` package on nuget.org, MIT license). The INISSTools programs use it through `ToolsCore`; Code comments are in Slovak without diacritics.

## Structure and rules

- Projects: `ExControls` (the library), `ExControls.Tests`, `ExControlsTester` (demo app), the out-of-process Visual Studio designer extension `ExControls.Designer.{Client,Server,Protocol,Package}` and `ExControls.Designer.targets` (delivers the extension to projects that use a `ProjectReference`).
- Targets net48 + net8 + net10 (net48-only code lives in `Compat/`); `ArgumentNullException.ThrowIfNull`, `[NotNull]` and `[AllowNull]` come from the polyfill in `Compat/` on net48 (not `#pragma`). `ExControls.Tests` runs on both net48 and net10. Win32 interop contains only the declarations that are used (`[DllImport]` because of net48).
- Theming: Ex* controls implement `IThemeable` (`Theming/`: `ExTheme`, `ExThemer`); a new Ex* control = `ApplyTheme`, a new standard control type = `ExThemer.Register<T>` (ToolsCore registers the handlers for standard controls), never a new `switch` branch. `ExMessageBox` dialogs get their icons through the `ExMessageBox.IconProvider` hook.
- `WFO1000` – mark public properties of custom controls with `[Browsable(false)] [DesignerSerializationVisibility(Hidden)]`.
- Toolbox icons are custom `.ico` files in `ExControls/Toolbox` referenced via `[ToolboxBitmap(typeof(ToolboxIcons), "Toolbox.<Name>.ico")]` – for .NET projects VS looks for the icon only in the control's own assembly, and a reference to a WinForms icon (`typeof(Button), "Button.bmp"`) shows the default one, so controls derived from WinForms controls keep it only under `#if NETFRAMEWORK`. Each icon has a single 16×16 image without semi-transparent pixels and, like the WinForms icons, a 1 px #F6F6F6 halo between dark pixels and transparency (multiple sizes = no image, semi-transparent pixels = pink edges, a line without the halo falls apart into dots in VS). A new toolbox control without an icon fails `ToolboxBitmapTests`; icon origins are listed in `THIRD-PARTY-NOTICES.md`.
- Out-of-process designer: an exception thrown in an attribute drops all attributes of that member; clicks in the designer arrive in `OnMouseDown`; editors live in the `ExControls.Designer` package (bump its version on every change); `MoveLockedAssembly` must not run during a design-time build.
- The library has no logging and no program UI texts – it reports errors through exceptions.

## Publishing to nuget.org

Tag `v<Version>` → `.github/workflows/nuget.yml` (checks `<Version>` and `AssemblyVersion`, runs the tests, builds the package with `.snupkg` symbols, Trusted Publishing via `NuGet/login` – no API key, the nuget.org policy belongs to owner `filif123`); a published version can never be replaced, only unlisted. The package is produced only by `dotnet pack` (not by every build): it contains the XML documentation (from obj – it is not copied to the output of programs using a `ProjectReference`) and the designer extension in `lib/<tfm>/Design/WinForms` (the `BeforePack` target restores and builds it – the Server references ExControls, an inner build would create a cycle); `ProjectReference` consumers still get the extension through `ExControls.Designer.targets`.

## Commands

```bash
dotnet build ExControls.slnx -nologo -v q
dotnet test ExControls.Tests/ExControls.Tests.csproj    # net48 and net10
dotnet pack ExControls/ExControls.csproj -c Release     # package into artifacts/
```

## Common rules

- `Directory.Build.props`: `TreatWarningsAsErrors`, `Nullable`, `ImplicitUsings`, `AnalysisLevel latest-recommended` – the build must not produce a single warning; intentionally disabled rules live only there (`NoWarn` with a reason). Fix warnings in code; an exceptional suppression goes directly on the reported member (`[SuppressMessage(..., Justification = ...)]`).
- Do not launch the GUI from Bash – the user verifies it manually.
- Tests use MSTest, methods are named `Area_WhatItVerifies` (in Slovak, e.g. `Oblast_CoOveruje`).
- Scripts: save generators as files (a bash heredoc mangles `\\` and `\n`); no diacritics in `.ps1` files.

## Commits (Conventional Commits)

Commit only when asked. Messages follow [Conventional Commits](https://www.conventionalcommits.org/): `<type>(<scope>): <description>`.

- Type: `feat`, `fix`, `refactor`, `perf`, `docs`, `test`, `build` (projects, packages), `ci` (workflows), `style`, `chore`, `revert`.
- Scope is optional, lowercase, named after the control or area (e.g. `button`, `options-view`, `theming`, `designer`, `toolbox`).
- Description in English, imperative mood, lowercase, no trailing period, at most 72 characters; class names in backticks. An optional body after a blank line explains why. A public API change that breaks existing code gets `!` and a `BREAKING CHANGE: …` footer (nuget.org package – bump the major version).
- Example: ``fix(theming): repaint `ExDropDownButton` arrow in dark mode``.
