// ReSharper disable VirtualMemberNeverOverridden.Global
namespace ExControls;

/// <summary>
/// A Panel to which only controls of type T can be added.
/// </summary>
/// <typeparam name="T">The type of the control you can add to this panel.</typeparam>
/// <remarks></remarks>
[ToolboxItem(false)]
public class RestrictivePanel<T> : Panel where T : Control
{
    /// <summary>
    /// 
    /// </summary>
    public new event EventHandler<RestrictivePanelEventArgs<T>> ControlAdded;
    /// <summary>
    /// 
    /// </summary>
    public new event EventHandler<RestrictivePanelEventArgs<T>> ControlRemoved;

    /// <inheritdoc />
    protected override void OnControlAdded(ControlEventArgs e)
    {
        if (e.Control is not T control)
        {
            throw new InvalidOperationException(
                $"Cannot add control of type '{e.Control.GetType().Name}' to this RestrictivePanel as it accepts only controls of type '{typeof(T).Name}'!");
        }

        RaiseControlAdded(control);
        base.OnControlAdded(e);
    }

    /// <inheritdoc />
    protected override void OnControlRemoved(ControlEventArgs e)
    {
        base.OnControlRemoved(e);
        RaiseControlRemoved((T)e.Control);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="control"></param>
    protected virtual void RaiseControlAdded(T control)
    {
        var e = new RestrictivePanelEventArgs<T>(control);
        ControlAdded?.Invoke(this, e);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="control"></param>
    protected virtual void RaiseControlRemoved(T control)
    {
        var e = new RestrictivePanelEventArgs<T>(control);
        ControlRemoved?.Invoke(this, e);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TC"></typeparam>
    public class RestrictivePanelEventArgs<TC> : EventArgs
    {
        /// <inheritdoc />
        public RestrictivePanelEventArgs(TC control)
        {
            Control = control;
        }

        /// <summary>
        /// 
        /// </summary>
        public TC Control { get; }
    }
}