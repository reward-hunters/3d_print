using System.Diagnostics;
using System.Windows.Forms;
using RH.HeadShop.Helpers;
using RH.HeadShop.IO;

namespace RH.HeadShop.Controls.Tutorials
{
    public partial class frmLineToolTutorial : Form
    {
        public frmLineToolTutorial()
        {
            InitializeComponent();
            linkLabel1.Text = UserConfig.ByName("Tutorials")["Links", "LineTool", "https://www.youtube.com/watch?v=c7YbRsm8m9I"];
        }

        private void frmStartTutorial_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;            // this cancels the close event.
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var link = UserConfig.ByName("Tutorials")["Links", "LineTool", "https://www.youtube.com/watch?v=c7YbRsm8m9I"];
            Process.Start(link);
        }

        private void cbShow_CheckedChanged(object sender, System.EventArgs e)
        {
            UserConfig.ByName("Options")["Tutorials", "LineTool"] = cbShow.Checked ? "0" : "1";
        }
    }
}
