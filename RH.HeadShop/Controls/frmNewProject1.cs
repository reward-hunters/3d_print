using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using RH.HeadShop.IO;

namespace RH.HeadShop.Controls
{
    public partial class frmNewProject1 : Form
    {
        #region Var

        public string ProjectName
        {
            get
            {
                return textProjectName.Text;
            }
        }
        public string ProjectFolder
        {
            get
            {
                return Path.Combine(textProjectFolder.Text, textProjectName.Text);
            }
        }
        public string TemplateImage
        {
            get
            {
                return textTemplateImage.Text;
            }
        }

        public string LoadingProject
        {
            get
            {
                return textLoadProject.Text;
            }
        }

        public bool LoadProject
        {
            get
            {
                return rbSaved.Checked;
            }
        }

        public DialogResult dialogResult = DialogResult.Cancel;
        private readonly bool atStartup;

        private readonly int videoCardSize;

        public int SelectedSize
        {
            get { return rb512.Checked ? 512 : (rb1024.Checked ? 1024 : 2048); }
        }

        #endregion

        public frmNewProject1(bool atStartup)
        {
            InitializeComponent();

            this.atStartup = atStartup;
            groupLoadProject.Enabled = atStartup && !ProgramCore.PluginMode;
            rbSaved.Enabled = atStartup && !ProgramCore.PluginMode;

            rbNew.Enabled = atStartup;

            ShowInTaskbar = atStartup;

            if (ProgramCore.MainForm.CurrentProgram == frmMain.ProgramMode.HeadShop)
            {
                GL.GetInteger(GetPName.MaxTextureSize, out videoCardSize);
                rb512.Visible = rb1024.Visible = rb2048.Visible = true;
            }
            else
                rb512.Visible = rb1024.Visible = rb2048.Visible = false;

        }

        #region Form's event

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (!atStartup || rbNew.Checked)
            {
                if (pictureTemplate.Image == null)
                {
                    MessageBox.Show("Select Template Image !", "HeadShop", MessageBoxButtons.OK);
                    return;
                }
                if (string.IsNullOrEmpty(textProjectName.Text))
                {
                    MessageBox.Show("Enter Project Name !", "HeadShop", MessageBoxButtons.OK);
                    return;
                }
                if (string.IsNullOrEmpty(textProjectFolder.Text))
                {
                    MessageBox.Show("Enter Project Folder !", "HeadShop", MessageBoxButtons.OK);
                    return;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(textLoadProject.Text))
                {
                    MessageBox.Show("Select Project !", "HeadShop", MessageBoxButtons.OK);
                    return;
                }
            }

            dialogResult = DialogResult.OK;
            Close();
        }

        private void btnOpenFileDlg_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialogEx("Select template file", "Image Files|*.jpg;*.png;*.jpeg;*.bmp"))
            {
                ofd.Multiselect = false;
                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                textTemplateImage.Text = ofd.FileName;
                using (var bmp = new Bitmap(ofd.FileName))
                    pictureTemplate.Image = (Bitmap)bmp.Clone();
            }
        }
        private void btnOpenFolderDlg_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderDialogEx())
            {
                if (fbd.ShowDialog() != DialogResult.OK)
                    return;

                textProjectFolder.Text = fbd.SelectedFolder[0];
            }
        }
        private void btnLoadProject_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialogEx("Open HeadShop/HairShop project", "HeadShop projects|*.hds|HairShop projects|*.hs"))
            {
                ofd.Multiselect = false;
                if (ofd.ShowDialog(false) != DialogResult.OK)
                    return;

                textLoadProject.Text = ofd.FileName;

                var templateImagePath = Project.LoadTempaltePath(textLoadProject.Text);
                if (!string.IsNullOrEmpty(templateImagePath))
                {
                    var fi = new FileInfo(templateImagePath);
                    if (fi.Exists)
                    {
                        using (var bmp = new Bitmap(fi.FullName))
                            pictureTemplate.Image = (Bitmap)bmp.Clone();
                    }
                }
            }
        }

        #endregion

        private void rbNew_CheckedChanged(object sender, EventArgs e)
        {
            groupLoadProject.Enabled = !rbNew.Checked;
            groupBox1.Enabled = rbNew.Checked;

            if (ProgramCore.MainForm.CurrentProgram == frmMain.ProgramMode.HeadShop)
            {
                rb512.Enabled = rbNew.Checked && videoCardSize >= 512;
                rb1024.Enabled = rbNew.Checked && videoCardSize >= 1024;
                rb2048.Enabled = rbNew.Checked && videoCardSize >= 2048;
            }
        }
    }
}
