using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OpenTK;
using RH.HeadShop.Controls.PopupControl;
using RH.HeadShop.Helpers;
using RH.HeadShop.IO;
using RH.HeadShop.Render;
using RH.ImageListView;

namespace RH.HeadShop.Controls.Libraries
{
    public partial class frmMaterials : FormEx
    {
        private readonly Cursor colorPickerCursor;
        Popup complex;
        ctrlBrushesPopup brushesPopup;

        private int currentBrush = -1;
        public int CurrentBrusn
        {
            get { return currentBrush; }
            set
            {
                if (brushesPopup.CurrentBrush == currentBrush || brushesPopup.CurrentBrush == -1)
                {
                    ProgramCore.MainForm.ctrlRenderControl.Mode = Mode.None;
                    currentBrush = -1;
                }
                else
                {
                    ProgramCore.MainForm.ctrlRenderControl.Mode = Mode.Brush;
                    currentBrush = brushesPopup.CurrentBrush;   // получаем номер активной кисточки. с 5 до 300
                    ProgramCore.MainForm.ctrlRenderControl.brushTool.Radius = currentBrush / 100f;
                }

            }
        }

        public frmMaterials()
        {
            InitializeComponent();
            InitializeListView();

            using (var bitmap = new Bitmap(Properties.Resources.color_picker, new Size(24, 24)))
            {
                var ptr = bitmap.GetHicon();
                colorPickerCursor = new Cursor(ptr);
            }

            brushesPopup = new ctrlBrushesPopup(CurrentBrusn);
            complex = new Popup(brushesPopup);
            complex.Resizable = false;

            Sizeble = false;
            ProgramCore.MainForm.ctrlRenderControl.pickingController.OnSelectedMeshChanged += pickingController_OnSelectedMeshChanged;
        }

        void pickingController_OnSelectedMeshChanged()
        {
            BeginUpdate();
            try
            {
                if (ProgramCore.MainForm.ctrlRenderControl.pickingController.SelectedMeshes.Count == 0)
                {
                    panelColor.BackColor = Color.Empty;
                    teAlpha.Text = "255";

                    ctrlAngle.Angle = 0;
                    teAngle.Text = "0";
                    trackBarSize.Value = 1;

                    imageListView.ClearSelection();

                    trackBarSize.Enabled = ctrlAngle.Enabled = teAngle.Enabled = teAlpha.Enabled = btnPickColor.Enabled = false;
                }
                else
                {
                    var firstColorForPanel = ProgramCore.MainForm.ctrlRenderControl.pickingController.SelectedColor.Values.ElementAt(0);
                    var newColor = Color.FromArgb((int)(firstColorForPanel[3] * 255), (int)(firstColorForPanel[0] * 255),
                                                  (int)(firstColorForPanel[1] * 255), (int)(firstColorForPanel[2] * 255));
                    panelColor.BackColor = newColor;
                    teAlpha.Text = newColor.A.ToString(CultureInfo.InvariantCulture);

                    teAngle.Text = ((int)ProgramCore.MainForm.ctrlRenderControl.pickingController.SelectedMeshes[0].TextureAngle).ToString(CultureInfo.InvariantCulture);
                    ctrlAngle.Angle = (int)ProgramCore.MainForm.ctrlRenderControl.pickingController.SelectedMeshes[0].TextureAngle;
                    trackBarSize.Value = (int)(ProgramCore.MainForm.ctrlRenderControl.pickingController.SelectedMeshes[0].TextureSize * 10);

                    if (ProgramCore.MainForm.ctrlRenderControl.pickingController.SelectedMeshes[0].Material.Texture > 0)
                    {
                        var texturePath = ProgramCore.MainForm.ctrlRenderControl.textures.FirstOrDefault(x => x.Value.Texture == ProgramCore.MainForm.ctrlRenderControl.pickingController.SelectedMeshes[0].Material.Texture).Key;
                        foreach (var item in imageListView.Items)
                            if (item.FileName == texturePath)
                            {
                                item.Selected = true;
                                break;
                            }
                    }

                    trackBarSize.Enabled = ctrlAngle.Enabled = teAngle.Enabled = teAlpha.Enabled = btnPickColor.Enabled = true;
                }
            }
            finally
            {
                EndUpdate();
            }
        }

        #region Supported void's

        private void InitializeListView()
        {
            imageListView.AllowDuplicateFileNames = true;
            imageListView.SetRenderer(new ImageListViewRenderers.DefaultRenderer());

            imageListView.Columns.Add(ColumnType.Name);
            imageListView.Columns.Add(ColumnType.FileSize);
            imageListView.ThumbnailSize = new Size(64, 64);

            imageListView.Items.Clear();
            imageListView.SuspendLayout();
            try
            {
                var directoryPath = Path.Combine(Application.StartupPath, "Libraries", "Materials");
                var di = new DirectoryInfo(directoryPath);
                if (!di.Exists)
                    return;

                foreach (var p in di.GetFiles("*.jpg"))
                {
                    if (p.FullName.Contains("_alpha."))
                        continue;
                    imageListView.Items.Add(p.FullName);
                }
            }
            finally
            {
                imageListView.ResumeLayout();
            }
        }

        private void UpdateTexture()
        {
            if (ProgramCore.MainForm.ctrlRenderControl.pickingController.SelectedMeshes == null)
                return;

            var radAngle = (float)(Math.PI * ctrlAngle.Angle / 180f);
            var size = trackBarSize.Value / 10f;

            foreach (var mesh in ProgramCore.MainForm.ctrlRenderControl.pickingController.SelectedMeshes)
            {
                mesh.TextureSize = size;
                mesh.TextureAngle = ctrlAngle.Angle;
                mesh.UpdateTextureCoordinates(radAngle, size);
            }
        }

        #endregion

        #region Form's event

        private void frmMaterials_Activated(object sender, EventArgs e)
        {
            ProgramCore.MainForm.ctrlRenderControl.StagesDeactivate(1);
        }
        private void frmMaterials_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;            // this cancels the close event.
        }

        private void btnPickColor_Click(object sender, EventArgs e)
        {
            ProgramCore.MainForm.ctrlRenderControl.brushTool.Color = new Vector3(panelColor.BackColor.R / 255f, panelColor.BackColor.G / 255f, panelColor.BackColor.B / 255f);
            for (var i = 0; i < ProgramCore.MainForm.ctrlRenderControl.pickingController.SelectedColor.Keys.Count; i++)
            {
                var key = ProgramCore.MainForm.ctrlRenderControl.pickingController.SelectedColor.Keys.ElementAt(i);
                ProgramCore.MainForm.ctrlRenderControl.pickingController.SelectedColor[key] = new Vector4(panelColor.BackColor.R / 255f, panelColor.BackColor.G / 255f, panelColor.BackColor.B / 255f, panelColor.BackColor.A / 255f);
            }
        }
        private void panelColor_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            using (var cd = new ColorDialog())
            {
                cd.FullOpen = true;
                if (cd.ShowDialog() != DialogResult.OK)
                    return;
                panelColor.BackColor = cd.Color;
            }

        }
        private void imageListView_DoubleClick(object sender, EventArgs e)
        {
            if (ProgramCore.MainForm.ctrlRenderControl.pickingController.SelectedMeshes == null || imageListView.SelectedItems.Count == 0)
                return;

            var sel = imageListView.SelectedItems[0];
            var transparentPath = Path.Combine(Path.GetDirectoryName(sel.FileName), Path.GetFileNameWithoutExtension(sel.FileName) + "_alpha" + Path.GetExtension(sel.FileName));
            transparentPath = File.Exists(transparentPath) ? transparentPath : string.Empty;

            foreach (var mesh in ProgramCore.MainForm.ctrlRenderControl.pickingController.SelectedMeshes)
            {
                mesh.Material.DiffuseTextureMap = sel.FileName;
                mesh.Material.TransparentTextureMap = transparentPath;
            }
        }

        private void teAlpha_Validating(object sender, CancelEventArgs e)
        {
            var textEdit = (TextBox)sender;
            var regex = new Regex("^([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])$");
            errorProvider1.SetError(textEdit, !regex.IsMatch(textEdit.Text) ? "Please enter only number between (0; 255)" : "");
        }
        private void teAlpha_TextChanged(object sender, EventArgs e)
        {
            if (IsUpdating)
                return;

            for (var i = 0; i < ProgramCore.MainForm.ctrlRenderControl.pickingController.SelectedColor.Keys.Count; i++)
            {
                var key = ProgramCore.MainForm.ctrlRenderControl.pickingController.SelectedColor.Keys.ElementAt(i);
                var color = ProgramCore.MainForm.ctrlRenderControl.pickingController.SelectedColor[key];
                ProgramCore.MainForm.ctrlRenderControl.pickingController.SelectedColor[key] = new Vector4(color[0], color[1], color[2], Helpers.StringConverter.ToFloat(teAlpha.Text, 255) / 255f);
            }
        }

        private void ctrlAngle_AngleChanged()
        {
            if (IsUpdating)
                return;

            BeginUpdate();

            UpdateTexture();
            teAngle.Text = ctrlAngle.Angle.ToString(CultureInfo.InvariantCulture);

            EndUpdate();
        }
        private void teAngle_Validating(object sender, CancelEventArgs e)
        {
            var textEdit = (TextBox)sender;
            var regex = new Regex("^(0?[0-9]?[0-9]|[1-2][0-9][0-9]|3[0-5][0-9]|360)$");
            errorProvider1.SetError(textEdit, !regex.IsMatch(textEdit.Text) ? "Please enter only number between (0; 360)" : "");
        }
        private void teAngle_TextChanged(object sender, EventArgs e)
        {
            if (IsUpdating)
                return;

            var value = Helpers.StringConverter.ToInt(teAngle.Text, 0);
            ctrlAngle.Angle = value;
        }

        private void trackBarSize_Scroll(object sender, EventArgs e)
        {
            UpdateTexture();
        }

        private void btnAddNewMaterial_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialogEx("Select new material..", "Image files|*.jpg"))
            {
                ofd.Multiselect = false;
                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                var directoryPath = Path.Combine(Application.StartupPath, "Libraries", "Materials");
                var oldFileName = Path.GetFileNameWithoutExtension(ofd.FileName);
                var newFileName = oldFileName;
                var filePath = Path.Combine(directoryPath, newFileName + ".jpg");
                var index = 0;
                while (File.Exists(filePath))
                {
                    newFileName = oldFileName + string.Format("_{0}", index);
                    filePath = Path.Combine(directoryPath, newFileName + ".jpg");
                    ++index;
                }

                File.Copy(ofd.FileName, filePath, false);
                InitializeListView();
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (var sel in imageListView.SelectedItems)
            {
                var path = Path.Combine(sel.FilePath, sel.FileName);
                var fi = new FileInfo(path);
                if (fi.Exists)
                {
                    fi.Attributes = FileAttributes.Normal;
                    fi.Delete();
                }
            }
            InitializeListView();
        }

        #endregion

        private void btnColorPicker_MouseDown(object sender, MouseEventArgs e)
        {
            if (IsUpdating)
                return;
            BeginUpdate();
            if (btnColorPicker.Tag.ToString() == "2")       // режим снятие цвета
            {
                btnColorPicker.Tag = "1";
                btnColorPicker.BackColor = SystemColors.ControlDarkDark;
                btnColorPicker.ForeColor = Color.White;

                ChangeCursors(colorPickerCursor);
                ProgramCore.MainForm.ctrlRenderControl.Mode = Mode.ColorPicker;
            }
            else
            {
                btnColorPicker.Tag = "2";
                btnColorPicker.BackColor = SystemColors.Control;
                btnColorPicker.ForeColor = Color.Black;

                ChangeCursors(DefaultCursor);
                ProgramCore.MainForm.ctrlRenderControl.Mode = Mode.None;
            }
            EndUpdate();
        }

        private void ChangeCursors(Cursor cursor)
        {
            Cursor = cursor;
            ProgramCore.MainForm.ctrlTemplateImage.Cursor = cursor;
            ProgramCore.MainForm.ctrlRenderControl.Cursor = cursor;
        }

        public void SetColorFromPicker(Color color)
        {
            panelColor.BackColor = color;
            ProgramCore.MainForm.ctrlRenderControl.brushTool.Color = new Vector3(panelColor.BackColor.R / 255f, panelColor.BackColor.G / 255f, panelColor.BackColor.B / 255f);
        }

        private void pbBrush_MouseDown(object sender, MouseEventArgs e)
        {
            complex.Show(sender as PictureBox);
        }
    }
}
