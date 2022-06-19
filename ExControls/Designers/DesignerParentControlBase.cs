using System.ComponentModel.Design;
using System.Windows.Forms.Design;

namespace ExControls.Designers;

/// <summary>
/// Provides a common base class for ParentControlDesigners.
/// </summary>
/// <typeparam name="T">The Control to design.</typeparam>
internal class DesignerParentControlBase<T> : ParentControlDesigner where T : Control
{
    private ISelectionService _selectionService;
    private T _host;
    private IDesignerHost _designerHost;

    protected DesignerParentControlBase()
    {
        AutoResizeHandles = true;
    }

    /// <summary>
    /// Returns the Control this designer is designing as type T.
    /// </summary>
    protected T Host
    {
        get
        {
            if (_host is not null) return _host;
            _host = (T)Control;
            OnHostInitialized();

            return _host;
        }
    }

    /// <summary>
    /// Creates and returns the ISelectionService used to select components in the designer.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
    public ISelectionService SelectionService => _selectionService ??= (ISelectionService)GetService(typeof(ISelectionService));

    /// <summary>
    /// Creates and returns the IDesignerHost service.
    /// </summary>
    public IDesignerHost DesignerHost => _designerHost ??= (IDesignerHost)GetService(typeof(IDesignerHost));

    protected virtual void OnHostInitialized()
    {
    }

    protected void SelectHost()
    {
        SelectionService.SetSelectedComponents(new Component[] { Host });
    }
}