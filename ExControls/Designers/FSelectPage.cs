using ExControls.Controls;

namespace ExControls.Designers;

public partial class FSelectPage : Form
{
    private readonly ExVerticalTabControl tabControl;

    public FSelectPage(ExVerticalTabControl tabControl)
    {
        InitializeComponent();
        this.tabControl = tabControl;
        lbPages.DataSource = tabControl.TabPages;
        lbPages.SelectedItem = tabControl.SelectedTab;
    }

    private void bOK_Click(object sender, EventArgs e)
    {
        if (tabControl.TabPages.Count != 0)
        {
            tabControl.SelectedTab = lbPages.SelectedItem as ExVerticalTabPage;
        }
            
        DialogResult = DialogResult.OK;
    }
}