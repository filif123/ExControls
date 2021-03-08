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

    /// <summary>
    ///     
    /// </summary>
    public class ExPropertyChangedEventArgs : EventArgs
    {
        /// <summary>
        ///     New value of changed property
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        ///     Name of changed property
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        ///     Creates new instance of <see cref="ExPropertyChangedEventArgs"/>.
        /// </summary>
        /// <param name="name">name of changed property</param>
        /// <param name="value">new value of changed property</param>
        public ExPropertyChangedEventArgs(string name, object value)
        {
            PropertyName = name;
            Value = value;
        }
    }
}