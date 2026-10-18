using System.ComponentModel.Design;
#if NETFRAMEWORK
using System.Windows.Forms.Design;
#else
using Microsoft.DotNet.DesignTools.Designers;
#endif

namespace ExControls.Designers;

/// <summary>
/// Provides a common base class for ScrollableControlDesigners.
/// </summary>
/// <typeparam name="T">The Control to design.</typeparam>
internal class DesignerScrollableControlBase<T> : ScrollableControlDesigner where T : Control
{
    private T? _host;
    private IDesignerHost? _designerHost;

    protected DesignerScrollableControlBase()
    {
        AutoResizeHandles = true;
    }

    /// <summary>
    /// Returns the Control this designer is designing as type T.
    /// </summary>
    protected T ControlHost
    {
        get
        {
            if (_host is not null) return _host;
            _host = (T)Control;
            OnHostInitialized();

            return _host;
        }
    }



    private ISelectionService? _selectionService;

    /// <summary>
    /// Gets the selection service used to select components in the designer.
    /// </summary>
    /// <value>The selection service, obtained from the designer services on first access.</value>
#if NETFRAMEWORK
    public ISelectionService SelectionService => (_selectionService ??= (ISelectionService?)GetService(typeof(ISelectionService)))!;
#else
    public new ISelectionService SelectionService => (_selectionService ??= GetService<ISelectionService>())!;
#endif

    /// <summary>
    /// Gets the designer host service.
    /// </summary>
#if NETFRAMEWORK
    public IDesignerHost DesignerHost => (_designerHost ??= (IDesignerHost?)GetService(typeof(IDesignerHost)))!;
#else
    public IDesignerHost DesignerHost => (_designerHost ??= GetService<IDesignerHost>())!;
#endif

    protected virtual void OnHostInitialized()
    {
    }

    protected void SelectHost()
    {
        SelectionService.SetSelectedComponents(new Component[] { ControlHost });
    }
}