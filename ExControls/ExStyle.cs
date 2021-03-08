using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace ExControls
{
    /// <summary>
    ///     Class for definition styles for Control
    /// </summary>
    public class ExStyle : IExNotifyPropertyChanged, ICloneable
    {
        private Color? _backColor;
        private Color? _foreColor;
        private Color? _borderColor;

        /// <summary>
        ///     Constructor for designer
        /// </summary>
        public ExStyle()
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="copy"></param>
        protected ExStyle(ExStyle copy)
        {
            BackColor = copy.BackColor;
            ForeColor = copy.ForeColor;
            BorderColor = copy.BorderColor;
        }

        /// <summary>
        ///     Foreground color of the Control
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "White")]
        [Description("Foreground color of the Control.")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        public virtual Color? BackColor
        {
            get => _backColor;
            set
            {
                if (_backColor == value)
                    return;
                _backColor = value;
                OnPropertyChanged(new ExPropertyChangedEventArgs(nameof(BackColor), value));
            }
        }

        /// <summary>
        ///     Background color of the Control
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("Background color of the Control.")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        public virtual Color? ForeColor
        {
            get => _foreColor;
            set
            {
                if (_foreColor == value)
                    return;

                _foreColor = value;
                OnPropertyChanged(new ExPropertyChangedEventArgs(nameof(ForeColor), value));
            }
        }

        /// <summary>
        ///     Color of the Controls's border
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "DimGray")]
        [Description("Color of the Controls's border.")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        public virtual Color? BorderColor
        {
            get => _borderColor;
            set
            {
                if (_borderColor == value)
                    return;

                _borderColor = value;
                OnPropertyChanged(new ExPropertyChangedEventArgs(nameof(BorderColor), BorderColor));
            }
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return "(Collection)";
        }

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public virtual object Clone()
        {
            return new ExStyle(this);
        }

        /// <summary>Occurs when a property value changes.</summary>
        public event EventHandler<ExPropertyChangedEventArgs> PropertyChanged;

        /// <summary>
        ///     Raises the <see cref="PropertyChanged" /> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPropertyChanged(ExPropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}
