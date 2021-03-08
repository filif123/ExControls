using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace ExControls
{
    /// <summary>
    ///     Present the Windows Vista-style open file dialog to select a folder. Fall back for older Windows Versions <br></br>
    ///     Source: (edited)<br></br>
    ///     https://stackoverflow.com/questions/4136477/trying-to-open-a-file-dialog-using-the-new-ifiledialog-and-ifileopendialog-inter
    /// </summary>
    [ToolboxBitmap(typeof(FolderBrowserDialog), "FolderBrowserDialog.bmp")]
    public partial class ExFolderBrowserDialog : Component
    {
        private const string DEFAULT_TITLE = "Select a folder";

        private string _initialDirectory;
        private string _title;

        /// <summary>
        ///     Constructor
        /// </summary>
        public ExFolderBrowserDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="container"></param>
        public ExFolderBrowserDialog(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        ///     Pociatocny priecinok
        /// </summary>
        public string InitialDirectory
        {
            get => string.IsNullOrEmpty(_initialDirectory) ? Environment.CurrentDirectory : _initialDirectory;
            set => _initialDirectory = value;
        }

        /// <summary>
        ///     Text zobrazujuci sa na zahlavi okna
        /// </summary>
        [DefaultValue(DEFAULT_TITLE)]
        public string Title
        {
            get => _title ?? DEFAULT_TITLE;
            set => _title = value;
        }

        /// <summary>
        ///     Gets a path to file
        /// </summary>
        [Browsable(false)]
        public string FileName { get; private set; } = "";

        /// <summary>
        ///     Show folder browser dialog.
        /// </summary>
        /// <returns></returns>
        public bool Show()
        {
            return Show(IntPtr.Zero);
        }

        /// <summary>
        ///     Show folder browser dialog with specific owner.
        /// </summary>
        /// <param name="hWndOwner">Handle of the control or window to be the parent of the file dialog</param>
        /// <returns><see langword="true"/> if the user clicks OK</returns>
        public bool Show(IntPtr hWndOwner)
        {
            var version = Environment.OSVersion.Version.Major >= 6;
            ShowDialogResult result = version ? 
                VistaDialog.Show(hWndOwner, InitialDirectory, Title) :
                ShowXpDialog(hWndOwner, InitialDirectory, Title);

            FileName = result.FileName;
            return result.Result;
        }

        private static ShowDialogResult ShowXpDialog(IntPtr ownerHandle, string initialDirectory, string title)
        {
            var folderBrowserDialog = new FolderBrowserDialog
            {
                Description = title,
                SelectedPath = initialDirectory,
                ShowNewFolderButton = false
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
            private const string c_foldersFilter = "Folders|\n";

            private const BindingFlags c_flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            private static readonly Assembly s_windowsFormsAssembly = typeof(FileDialog).Assembly;

            private static readonly Type s_iFileDialogType = s_windowsFormsAssembly.GetType("System.Windows.Forms.FileDialogNative+IFileDialog");

            private static readonly MethodInfo s_createVistaDialogMethodInfo = typeof(OpenFileDialog).GetMethod("CreateVistaDialog", c_flags);

            private static readonly MethodInfo s_onBeforeVistaDialogMethodInfo = typeof(OpenFileDialog).GetMethod("OnBeforeVistaDialog", c_flags);

            private static readonly MethodInfo s_getOptionsMethodInfo = typeof(FileDialog).GetMethod("GetOptions", c_flags);

            private static readonly MethodInfo s_setOptionsMethodInfo = s_iFileDialogType.GetMethod("SetOptions", c_flags);

            private static readonly uint s_fosPickFoldersBitFlag = (uint)s_windowsFormsAssembly
                .GetType("System.Windows.Forms.FileDialogNative+FOS")
                .GetField("FOS_PICKFOLDERS")
                .GetValue(null);

            private static readonly ConstructorInfo s_vistaDialogEventsConstructorInfo = s_windowsFormsAssembly
                .GetType("System.Windows.Forms.FileDialog+VistaDialogEvents")
                .GetConstructor(c_flags, null, new[] { typeof(FileDialog) }, null);

            private static readonly MethodInfo s_adviseMethodInfo = s_iFileDialogType.GetMethod("Advise");
            private static readonly MethodInfo s_unAdviseMethodInfo = s_iFileDialogType.GetMethod("Unadvise");
            private static readonly MethodInfo s_showMethodInfo = s_iFileDialogType.GetMethod("Show");

            public static ShowDialogResult Show(IntPtr ownerHandle, string initialDirectory, string title)
            {
                var openFileDialog = new OpenFileDialog
                {
                    AddExtension = false,
                    CheckFileExists = false,
                    DereferenceLinks = true,
                    Filter = c_foldersFilter,
                    InitialDirectory = initialDirectory,
                    Multiselect = false,
                    Title = title
                };

                object iFileDialog = s_createVistaDialogMethodInfo.Invoke(openFileDialog, new object[] { });
                s_onBeforeVistaDialogMethodInfo.Invoke(openFileDialog, new[] { iFileDialog });
                s_setOptionsMethodInfo.Invoke(iFileDialog,
                    new object[] {(uint) s_getOptionsMethodInfo.Invoke(openFileDialog, new object[] { }) | s_fosPickFoldersBitFlag});
                var adviseParametersWithOutputConnectionToken = new[] {s_vistaDialogEventsConstructorInfo.Invoke(new object[] {openFileDialog}), 0U};
                s_adviseMethodInfo.Invoke(iFileDialog, adviseParametersWithOutputConnectionToken);

                try
                {
                    var retVal = (int)s_showMethodInfo.Invoke(iFileDialog, new object[] { ownerHandle });
                    return new ShowDialogResult
                    {
                        Result = retVal == 0,
                        FileName = openFileDialog.FileName
                    };
                }
                finally
                {
                    s_unAdviseMethodInfo.Invoke(iFileDialog, new[] { adviseParametersWithOutputConnectionToken[1] });
                }
            }
        }

        // Wrap an IWin32Window around an IntPtr
        private class WindowWrapper : IWin32Window
        {
            public WindowWrapper(IntPtr handle)
            {
                Handle = handle;
            }

            public IntPtr Handle { get; }
        }
    }
}
