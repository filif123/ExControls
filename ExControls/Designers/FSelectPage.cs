using ExControls.Controls;

namespace ExControls.Designers;

public partial class FSelectPage : Form
{
    private readonly Dictionary<TreeNode, MenuPanel> pages;

    public FSelectPage(Dictionary<TreeNode, MenuPanel> pages)
    {
        InitializeComponent();
        this.pages = pages;
        lbPages.DataSource = pages.Keys.ToList();
    }

    public MenuPanel SelectedPage { get; set; }

    private void bOK_Click(object sender, EventArgs e)
    {
        if (pages.Count != 0 && lbPages.SelectedItem is TreeNode node && pages.TryGetValue(node, out var sp))
        {
            SelectedPage = sp;
        }
            
        DialogResult = DialogResult.OK;
    }
}