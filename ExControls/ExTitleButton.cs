using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ExControls
{
    /// <summary>
    ///     Button for Title of ExForm
    /// </summary>
    public partial class ExTitleButton : UserControl
    {
        private Color _highlightBackColor;
        private Color _highlightForeColor;
        private bool _autoButtonColors;

        /// <summary>
        ///     Constructor
        /// </summary>
        public ExTitleButton()
        {
            InitializeComponent();

            BackColor = Color.Transparent;
            AutoButtonColors = true;
        }

        public bool Hover { get; private set; }

        /// <summary>
        ///     Gets or sets whether Button sets ForeColor and HighlightForeColor according to BackColor of the parent and HighlightBackColor darken according to BackColor
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(true)]
        [Description("Gets or sets whether Button sets ForeColor and HighlightForeColor according to BackColor of the parent and HighlightBackColor darken according to BackColor.")]
        public bool AutoButtonColors
        {
            get => _autoButtonColors;
            set
            {
                if (_autoButtonColors == value)
                    return;
                _autoButtonColors = value;

                Invalidate();
            }
        }

        /// <summary>
        ///     Background color of Button when mouse is over the Control
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(SystemColors), "Highlight")]
        [Description("Background color of Button when mouse is over the Control.")]
        public Color HighlightBackColor
        {
            get => _highlightBackColor;
            set
            {
                if (_highlightBackColor == value)
                    return;
                _highlightBackColor = value;
                Invalidate();
            }
        }

        /// <summary>
        ///     Foreground color of Button when mouse is over the Control
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(SystemColors), "Highlight")]
        [Description("Foreground color of Button when mouse is over the Control.")]
        public Color HighlightForeColor
        {
            get => _highlightForeColor;
            set
            {
                if (_highlightForeColor == value)
                    return;
                _highlightForeColor = value;
                Invalidate();
            }
        }

        /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseEnter" /> event.</summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            if (!Hover)
            {
                Hover = true;
                Invalidate();
            }
        }

        /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave" /> event.</summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (Hover)
            {
                Hover = false;
                Invalidate();
            }
        }
    }
}
