namespace ExControls.Controls;

/// <summary>Notifies clients that a property value has changed.</summary>
public interface IExNotifyPropertyChanged
{
    /// <summary>Occurs when a property value changed.</summary>
    event EventHandler<ExPropertyChangedEventArgs> PropertyChanged;
}
