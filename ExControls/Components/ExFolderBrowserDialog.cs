// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global

using System.Drawing.Design;
using System.Reflection;
using System.Security.Permissions;

namespace ExControls;

//source: https://stackoverflow.com/questions/4136477/trying-to-open-a-file-dialog-using-the-new-ifiledialog-and-ifileopendialog-inter
#if NETFRAMEWORK

/// <summary>
///  Represents a common dialog box that allows the user to specify options for
///  selecting a folder. This class cannot be inherited.
/// </summary>
[DefaultEvent(nameof(HelpRequest))]
[DefaultProperty(nameof(SelectedPath))]
[Designer("System.Windows.Forms.Design.FolderBrowserDialogDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
[ToolboxBitmap(typeof(FolderBrowserDialog), "FolderBrowserDialog.bmp")]
public sealed class ExFolderBrowserDialog : CommonDialog
{
    private string _initialDirectory;
    private string _description;
    private Environment.SpecialFolder _rootFolder;
    private string _selectedPath;
    private bool _selectedPathNeedsCheck;

    /// <summary>Initializes a new instance of the <see cref="ExFolderBrowserDialog" /> class.</summary>
    public ExFolderBrowserDialog() => Reset();

    /// <summary>
    ///  Gets or sets the initial directory displayed by the folder browser dialog.
    /// </summary>
    public string InitialDirectory
    {
        get => _initialDirectory;
        set => _initialDirectory = value ?? "";
    }

    /// <summary>
    /// Occurs when the user clicks the Help button on a common dialog box.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new event EventHandler HelpRequest
    {
        add => base.HelpRequest += value;
        remove => base.HelpRequest -= value;
    }

    /// <summary>
    ///  Gets or sets a description to show above the folders. Here you can provide
    ///  instructions for selecting a folder.
    /// </summary>
    [Browsable(true)]
    [DefaultValue("")]
    [Localizable(true)]
    [ExCategory(CategoryType.Appearance)]
    public string Description
    {
        get => _description;
        set => _description = value ?? "";
    }

    /// <summary>
    ///  Determines if the 'New Folder' button should be exposed.
    ///  This property has no effect if the Vista style dialog is used; in that case, the New Folder button is always shown.
    /// </summary>
    [Browsable(true)]
    [DefaultValue(true)]
    [Localizable(false)]
    [ExCategory(CategoryType.Appearance)]
    public bool ShowNewFolderButton { get; set; }

    /// <summary>
    ///  Gets/sets the root node of the directory tree.
    /// </summary>
    [Browsable(true)]
    [DefaultValue(Environment.SpecialFolder.Desktop)]
    [Localizable(false)]
    [ExCategory(CategoryType.Appearance)]
    [TypeConverter("System.Windows.Forms.SpecialFolderEnumConverter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
    public Environment.SpecialFolder RootFolder
    {
        get => _rootFolder;
        set
        {
            if (!Enum.IsDefined(typeof(Environment.SpecialFolder), value))
            {
                throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(Environment.SpecialFolder));
            }

            _rootFolder = value;
        }
    }

    /// <summary>
    ///  Gets the directory path of the folder the user picked.
    ///  Sets the directory path of the initial folder shown in the dialog box.
    /// </summary>
    [Browsable(true)]
    [DefaultValue("")]
    [Editor("System.Windows.Forms.Design.SelectedPathEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
    [Localizable(true)]
    [ExCategory(CategoryType.Behavior)]
    public string SelectedPath
    {
        get
        {
            if (string.IsNullOrEmpty(_selectedPath) || !_selectedPathNeedsCheck)
                return _selectedPath;
            new FileIOPermission(FileIOPermissionAccess.PathDiscovery, _selectedPath).Demand();
            return _selectedPath;
        }
        set
        {
            _selectedPath = value ?? string.Empty;
            _selectedPathNeedsCheck = false;
        }
    }

    /// <summary>
    /// Resets properties to their default values.
    /// </summary>
    public override void Reset()
    {
        _description = "";
        _initialDirectory = Environment.CurrentDirectory;
        _selectedPath = "";
        _rootFolder = Environment.SpecialFolder.Desktop;
        _selectedPathNeedsCheck = false;
    }

    /// <summary>When overridden in a derived class, specifies a common dialog box.</summary>
    /// <param name="hwndOwner">A value that represents the window handle of the owner window for the common dialog box.</param>
    /// <returns>
    /// <see langword="true" /> if the dialog box was successfully run; otherwise, <see langword="false" />.</returns>
    protected override bool RunDialog(IntPtr hwndOwner)
    {
        var version = Environment.OSVersion.Version.Major >= 6;
        var result = version
            ? VistaDialog.Show(hwndOwner, InitialDirectory, Description)
            : ShowXpDialog(hwndOwner, InitialDirectory, Description, ShowNewFolderButton, RootFolder);

        SelectedPath = result.FileName;
        return result.Result;
    }

    private static ShowDialogResult ShowXpDialog(IntPtr ownerHandle, string initialDirectory, string title, bool showNewFolderBtn, Environment.SpecialFolder rootFolder)
    {
        var folderBrowserDialog = new FolderBrowserDialog
        {
            Description = title,
            SelectedPath = initialDirectory,
            ShowNewFolderButton = showNewFolderBtn,
            RootFolder = rootFolder
        };

        var dialogResult = new ShowDialogResult();

        if (folderBrowserDialog.ShowDialog(new WindowWrapper(ownerHandle)) == DialogResult.OK)
        {
            dialogResult.Result = true;
            dialogResult.FileName = folderBrowserDialog.SelectedPath;
        }

        return dialogResult;
    }

    private struct ShowDialogResult
    {
        public bool Result { get; set; }
        public string FileName { get; set; }
    }

    private static class VistaDialog
    {
        private const string FOLDERS_FILTER = "Folders|\n";
        private const BindingFlags FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private static readonly Assembly WindowsFormsAssembly = typeof(FileDialog).Assembly;
        private static readonly Type FileDialogType = WindowsFormsAssembly.GetType("System.Windows.Forms.FileDialogNative+IFileDialog");
        private static readonly MethodInfo CreateVistaDialogMethodInfo = typeof(OpenFileDialog).GetMethod("CreateVistaDialog", FLAGS);
        private static readonly MethodInfo OnBeforeVistaDialogMethodInfo = typeof(OpenFileDialog).GetMethod("OnBeforeVistaDialog", FLAGS);
        private static readonly MethodInfo GetOptionsMethodInfo = typeof(FileDialog).GetMethod("GetOptions", FLAGS);
        private static readonly MethodInfo SetOptionsMethodInfo = FileDialogType.GetMethod("SetOptions", FLAGS);
        private static readonly MethodInfo AdviseMethodInfo = FileDialogType.GetMethod("Advise");
        private static readonly MethodInfo UnadviseMethodInfo = FileDialogType.GetMethod("Unadvise");
        private static readonly MethodInfo ShowMethodInfo = FileDialogType.GetMethod("Show");

        private static readonly uint FosPickFoldersBitFlag = (uint)WindowsFormsAssembly
            .GetType("System.Windows.Forms.FileDialogNative+FOS")
            .GetField("FOS_PICKFOLDERS")
            .GetValue(null);

        private static readonly ConstructorInfo VistaDialogEventsConstructorInfo = WindowsFormsAssembly
            .GetType("System.Windows.Forms.FileDialog+VistaDialogEvents")
            .GetConstructor(FLAGS, null, new[] { typeof(FileDialog) }, null);

        public static ShowDialogResult Show(IntPtr ownerHandle, string initialDirectory, string title)
        {
            var openFileDialog = new OpenFileDialog
            {
                AddExtension = false,
                CheckFileExists = false,
                DereferenceLinks = true,
                Filter = FOLDERS_FILTER,
                InitialDirectory = initialDirectory,
                Multiselect = false,
                Title = title,
            };

            var options = (uint)GetOptionsMethodInfo.Invoke(openFileDialog, new object[] { });
            var fileDialog = CreateVistaDialogMethodInfo.Invoke(openFileDialog, new object[] { });

            OnBeforeVistaDialogMethodInfo.Invoke(openFileDialog, new[] { fileDialog });
            SetOptionsMethodInfo.Invoke(fileDialog, new object[] { options | FosPickFoldersBitFlag });

            var adviseParametersWithOutputConnectionToken = new[] { VistaDialogEventsConstructorInfo.Invoke(new object[] { openFileDialog }), 0U };
            AdviseMethodInfo.Invoke(fileDialog, adviseParametersWithOutputConnectionToken);

            try
            {
                var retVal = (int)ShowMethodInfo.Invoke(fileDialog, new object[] { ownerHandle });
                return new ShowDialogResult
                {
                    Result = retVal == 0,
                    FileName = openFileDialog.FileName
                };
            }
            finally
            {
                UnadviseMethodInfo.Invoke(fileDialog, new[] { adviseParametersWithOutputConnectionToken[1] });
            }
        }
    }

    // Wrap an IWin32Window around an IntPtr
    private sealed class WindowWrapper : IWin32Window
    {
        public WindowWrapper(IntPtr handle)
        {
            Handle = handle;
        }

        public IntPtr Handle { get; }
    }
}
#endif