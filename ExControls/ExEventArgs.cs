using System;
using System.Drawing;

namespace ExControls
{
    /// <inheritdoc />
    public class LinePenEventArgs : EventArgs
    {
        /// <summary>
        ///     Pen of the line
        /// </summary>
        public Pen Pen { get; }

        internal LinePenEventArgs( Pen pen)
        {
            Pen = pen;
        }
    }

    public class ExPropertyChangedEventArgs : EventArgs
    {
        public object Value { get; set; }
        public string PropertyName { get; }

        public ExPropertyChangedEventArgs(string name, object value)
        {
            PropertyName = name;
            Value = value;
        }
    }
}