﻿using System.ComponentModel.Design;

#if NETFRAMEWORK
using System.Windows.Forms.Design;
#else
using Microsoft.DotNet.DesignTools.Designers;
#endif

namespace ExControls.Designers;

/// <summary>
/// Provides a common base class for ControlDesigners.
/// </summary>
/// <typeparam name="T">The Control to design.</typeparam>
internal class DesignerControlBase<T> : ControlDesigner where T : Control
{
    private T _host;
    private IDesignerHost _designerHost;

    protected DesignerControlBase()
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

#if NETFRAMEWORK
    private ISelectionService _selectionService;

    /// <summary>
    /// Creates and returns the ISelectionService used to select components in the designer.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
    public ISelectionService SelectionService => _selectionService ??= (ISelectionService)GetService(typeof(ISelectionService));
#endif

    /// <summary>
    /// Creates and returns the IDesignerHost service.
    /// </summary>
    public IDesignerHost DesignerHost => _designerHost ??= (IDesignerHost)GetService(typeof(IDesignerHost));

    protected virtual void OnHostInitialized()
    {
    }

    protected void SelectHost()
    {
        SelectionService.SetSelectedComponents(new Component[] { ControlHost });
    }
}