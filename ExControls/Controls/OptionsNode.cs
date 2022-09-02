using ExControls.Converters;

namespace ExControls;

/// <summary>
/// Represents a TreeNode in the TreeView of an ExOptionsView.
/// </summary>
[TypeConverter(typeof(OptionsNodeConverter))]
public class OptionsNode : TreeNode
{
    private ExOptionsPanel _panel;

    /// <summary>
    /// Gets the ExOptionsPanel corresponding to this node.
    /// </summary>
    [Browsable(false)]
    public ExOptionsPanel Panel
    {
        get => _panel;
        internal set
        {
            _panel = value;
            Text = value.Name;
        }
    }

    /// <inheritdoc />
    public override string ToString() => Text;
}