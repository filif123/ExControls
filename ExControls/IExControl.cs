using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExControls
{
    public interface IExControl
    {
        /// <summary>
        ///     Default style of the Control
        /// </summary>
        public bool DefaultStyle { get; set; }

        /// <summary>Occurs when the <see cref="DefaultStyle" /> property changed.</summary>
        public event EventHandler DefaultStyleChanged;
    }

    public interface ICheckableExControl
    {
        public ContentAlignment TextAlign { get; set; }
        public ContentAlignment CheckAlign { get; set; }
        public Rectangle ClientRectangle { get; }
        public RightToLeft RightToLeft { get; set; }
        public string Text { get; set; }
        public Font Font { get; set; }
    }

    /// <summary>Notifies clients that a property value has changed.</summary>
    public interface IExNotifyPropertyChanged
    {
        /// <summary>Occurs when a property value changed.</summary>
        event EventHandler<ExPropertyChangedEventArgs> PropertyChanged;
    }
}
