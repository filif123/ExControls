using System.ComponentModel;
using System.Windows.Forms;

namespace ExControls
{
    /// <summary>
    ///     Expanded ComboBox Control for ToolStrip
    /// </summary>
    public partial class ToolStripExComboBox : ToolStripControlHost
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public ToolStripExComboBox() : base(new ExComboBox())
        {
        }

        /// <summary>
        ///     Object ComboBox, type <see cref="ExComboBox"/>
        /// </summary>
        [Browsable(true)]
        public ExComboBox ComboBox => Control as ExComboBox;
    }
}
