// ReSharper disable VirtualMemberNeverOverridden.Global
namespace ExControls;

/// <summary>
/// A Panel to which only controls of type T can be added.
/// </summary>
/// <typeparam name="T">The type of the control you can add to this panel.</typeparam>
[ToolboxItem(false)]
public class RestrictivePanel<T> : Panel where T : Control
{
    /// <summary>
    /// Occurs when a control is added to the panel.
    /// </summary>
    public new event EventHandler<RestrictivePanelEventArgs<T>>? ControlAdded;
    /// <summary>
    /// Occurs when a control is removed from the panel.
    /// </summary>
    public new event EventHandler<RestrictivePanelEventArgs<T>>? ControlRemoved;

    /// <inheritdoc />
    protected override void OnControlAdded(ControlEventArgs e)
    {
        if (e.Control is not T control)
        {
            throw new InvalidOperationException(
                $"Cannot add control of type '{e.Control?.GetType().Name}' to this RestrictivePanel as it accepts only controls of type '{typeof(T).Name}'!");
        }

        RaiseControlAdded(control);
        base.OnControlAdded(e);
    }

    /// <inheritdoc />
    protected override void OnControlRemoved(ControlEventArgs e)
    {
        base.OnControlRemoved(e);
        RaiseControlRemoved((T)e.Control!);
    }

    /// <summary>
    /// Raises the <see cref="ControlAdded" /> event.
    /// </summary>
    /// <param name="control">The added control.</param>
    protected virtual void RaiseControlAdded(T control)
    {
        var e = new RestrictivePanelEventArgs<T>(control);
        ControlAdded?.Invoke(this, e);
    }

    /// <summary>
    /// Raises the <see cref="ControlRemoved" /> event.
    /// </summary>
    /// <param name="control">The removed control.</param>
    protected virtual void RaiseControlRemoved(T control)
    {
        var e = new RestrictivePanelEventArgs<T>(control);
        ControlRemoved?.Invoke(this, e);
    }

    /// <summary>
    /// Provides data for the <see cref="RestrictivePanel{T}.ControlAdded" /> and <see cref="RestrictivePanel{T}.ControlRemoved" />
    /// events.
    /// </summary>
    /// <typeparam name="TC">The type of the control.</typeparam>
    public class RestrictivePanelEventArgs<TC> : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RestrictivePanelEventArgs{TC}" /> class.
        /// </summary>
        /// <param name="control">the control that was added or removed</param>
        public RestrictivePanelEventArgs(TC control)
        {
            Control = control;
        }

        /// <summary>
        /// Gets the control that was added or removed.
        /// </summary>
        public TC Control { get; }
    }
}