using System.Diagnostics;
using System.Windows.Forms;
using RH.HeadShop.Helpers;
using RH.HeadShop.IO;

namespace RH.HeadShop.Controls.Tutorials.HeadShop
{
    public partial class frmShapedotsTutorial : FormEx
    {
        public frmShapedotsTutorial()
        {
            InitializeComponent();
            linkLabel1.Text = UserConfig.ByName("Tutorials")["Links", "Shapedots", "http://youtu.be/pIlrJUByJj8"];
        }

        private void frmShapedotsTutorial_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;            // this cancels the close event.
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var link = UserConfig.ByName("Tutorials")["Links", "Shapedots", "http://youtu.be/pIlrJUByJj8"];
            Process.Start(link);
        }

        private void cbShow_CheckedChanged(object sender, System.EventArgs e)
        {
            UserConfig.ByName("Options")["Tutorials", "Shapedots"] = cbShow.Checked ? "0" : "1";
        }
    }
}
