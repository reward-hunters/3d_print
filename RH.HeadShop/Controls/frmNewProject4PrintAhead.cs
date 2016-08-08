using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using RH.HeadShop.Helpers;
using RH.HeadShop.IO;
using RH.HeadShop.Render;
using RH.HeadShop.Render.Helpers;

namespace RH.HeadShop.Controls
{
    public partial class frmNewProject4PrintAhead : Form
    {
        #region Var

        public ManType ManType
        {
            get
            {
                if (btnMale.Tag.ToString() == "1")
                    return ManType.Male;
                if (btnFemale.Tag.ToString() == "1")
                    return ManType.Female;
                if (btnChild.Tag.ToString() == "1")
                    return ManType.Child;
                return ManType.Custom;
            }
        }
        public string CustomModelPath;

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

        private FaceRecognition fcr;
        private readonly int videoCardSize;

        public int SelectedSize
        {
            get { return rb512.Checked ? 512 : (rb1024.Checked ? 1024 : 2048); }
        }
        private Pen edgePen;
        private Pen arrowPen;
        public RectangleF nextHeadRect = new RectangleF();
        public RectangleF nextHeadRectF = new RectangleF();

        public int ImageTemplateWidth;
        public int ImageTemplateHeight;
        public int ImageTemplateOffsetX;
        public int ImageTemplateOffsetY;

        public PointF MouthTransformed;
        public PointF LeftEyeTransformed;
        public PointF RightEyeTransformed;

        private float eWidth;
        public RectangleF TopEdgeTransformed;
        public RectangleF BottomEdgeTransformed;
        public Cheek LeftCheek;
        public Cheek RightCheek;

        private const int CircleRadius = 30;
        private const int HalfCircleRadius = 15;
        private const int CircleSmallRadius = 8;
        private const int HalfCircleSmallRadius = 4;

        private bool leftMousePressed;
        private Point startMousePoint;
        private RectangleF startEdgeRect;
        private Vector2 headHandPoint = Vector2.Zero;
        private Vector2 tempSelectedPoint = Vector2.Zero;
        private Vector2 tempSelectedPoint2 = Vector2.Zero;

        #endregion

        public frmNewProject4PrintAhead(bool atStartup)
        {
            InitializeComponent();

            this.atStartup = atStartup;
            groupLoadProject.Enabled = atStartup && !ProgramCore.PluginMode;
            rbSaved.Enabled = atStartup && !ProgramCore.PluginMode;

            eWidth = pictureTemplate.Width - 100;
            TopEdgeTransformed = new RectangleF(pictureTemplate.Width / 2f - eWidth / 2f, 30, eWidth, eWidth);
            BottomEdgeTransformed = new RectangleF(pictureTemplate.Width / 2f - eWidth / 2f, eWidth - eWidth / 4f, eWidth, eWidth);

            rbNew.Enabled = atStartup;
            ShowInTaskbar = atStartup;

            if (ProgramCore.MainForm.CurrentProgram == frmMain.ProgramMode.HeadShop)
            {
                GL.GetInteger(GetPName.MaxTextureSize, out videoCardSize);
                rb512.Visible = rb1024.Visible = rb2048.Visible = true;
                rb512.Enabled = rbNew.Checked && videoCardSize >= 512;
                rb1024.Enabled = rbNew.Checked && videoCardSize >= 1024;
                rb2048.Enabled = rbNew.Checked && videoCardSize >= 2048;
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

        private string templateImage;
        private void btnOpenFileDlg_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialogEx("Select template file", "Image Files|*.jpg;*.png;*.jpeg;*.bmp"))
            {
                ofd.Multiselect = false;
                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                textTemplateImage.Text = ofd.FileName;


                templateImage = ofd.FileName;
                fcr = new FaceRecognition();
                fcr.Recognize(ref templateImage, true);     // это ОЧЕНЬ! важно. потому что мы во время распознавания можем создать обрезанную фотку и использовать ее как основную в проекте.

                using (var ms = new MemoryStream(File.ReadAllBytes(templateImage))) // Don't use using!!
                {
                    var img = (Bitmap)Bitmap.FromStream(ms);
                    pictureTemplate.Image = (Bitmap)img.Clone();
                    img.Dispose();
                }

                edgePen = (Pen)DrawingTools.GreenPen.Clone();
                //edgePen.Width = 2;
                arrowPen = (Pen)DrawingTools.GreenPen.Clone();
                arrowPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                //arrowPen.Width = 2;

                nextHeadRect.Height = fcr.FaceRectRelative.Height * 3.77294f;
                var center = (fcr.FaceRectRelative.Height + fcr.FaceRectRelative.Y) / 2f * 1.4f;
                nextHeadRect.Y = center - (nextHeadRect.Height / 2f);
                nextHeadRect.Height *= 0.92f; //рисовать на месте нижней челюсти

                var leftCheekX = fcr.LeftEyeCenter.X - (fcr.RightEyeCenter.X - fcr.LeftEyeCenter.X) / 2f;
                var rightCheekX = fcr.RightEyeCenter.X + (fcr.RightEyeCenter.X - fcr.LeftEyeCenter.X) / 2f;

                LeftCheek = new Cheek(leftCheekX, center);
                RightCheek = new Cheek(rightCheekX, center);

                RecalcRealTemplateImagePosition();

                RenderTimer.Start();
                CheekTimer.Start();

                if (ProgramCore.PluginMode)
                {
                    var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    var dazPath = Path.Combine(appDataPath, @"DAZ 3D\Studio4\temp\FaceShop\", "fs3d.obj");
                    if (File.Exists(dazPath))
                    {
                        rbImportObj.Checked = true;
                        CustomModelPath = dazPath;
                    }
                    else
                        MessageBox.Show("Daz model not found.", "HeadShop", MessageBoxButtons.OK);
                }

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

        private void rbNew_CheckedChanged(object sender, EventArgs e)
        {
            groupLoadProject.Enabled = !rbNew.Checked;
            groupBox1.Enabled = btnMale.Enabled = btnFemale.Enabled = btnChild.Enabled = rbNew.Checked;

            if (rbNew.Checked)
            {
                btnMale_Click(null, EventArgs.Empty);

                if (ProgramCore.MainForm.CurrentProgram == frmMain.ProgramMode.HeadShop)
                {
                    rb512.Enabled = rbNew.Checked && videoCardSize >= 512;
                    rb1024.Enabled = rbNew.Checked && videoCardSize >= 1024;
                    rb2048.Enabled = rbNew.Checked && videoCardSize >= 2048;
                }
            }
            else
            {
                if (btnMale.Tag.ToString() == "1")
                {
                    btnMale.Image = Properties.Resources.btnMaleGray;
                    btnMale.Tag = "2";
                }
                else if (btnFemale.Tag.ToString() == "1")
                {
                    btnFemale.Image = Properties.Resources.btnFemaleGray;
                    btnFemale.Tag = "2";
                }
                else if (btnChild.Tag.ToString() == "1")
                {
                    btnChild.Image = Properties.Resources.btnChildGray;
                    btnChild.Tag = "2";
                }
            }
        }

        private void btnQuestion_MouseDown(object sender, MouseEventArgs e)
        {
            btnQuestion.Image = Properties.Resources.btnQuestionPressed;
        }
        private void btnQuestion_MouseUp(object sender, MouseEventArgs e)
        {
            ProgramCore.MainForm.ShowTutorial();
            btnQuestion.Image = Properties.Resources.btnQuestionNormal;
        }
        private void btnPlay_MouseDown(object sender, MouseEventArgs e)
        {
            btnPlay.Image = Properties.Resources.btnPlayPressed;
        }
        private void btnPlay_MouseUp(object sender, MouseEventArgs e)
        {
            ProgramCore.MainForm.ShowVideo();
            btnPlay.Image = Properties.Resources.btnPlayNormal;
        }
        private void btnInfo_MouseDown(object sender, MouseEventArgs e)
        {
            btnInfo.Image = Properties.Resources.btnInfoPressed;
        }
        private void btnInfo_MouseUp(object sender, MouseEventArgs e)
        {
            ProgramCore.MainForm.ShowSiteInfo();
            btnInfo.Image = Properties.Resources.btnInfoNormal;
        }

        private void btnMale_Click(object sender, EventArgs e)
        {
            if (btnMale.Tag.ToString() == "2")
            {
                btnMale.Tag = "1";
                btnMale.Image = Properties.Resources.btnMaleNormal;


                btnChild.Tag = btnFemale.Tag = "2";
                btnChild.Image = Properties.Resources.btnChildGray;
                btnFemale.Image = Properties.Resources.btnFemaleGray;
                rbImportObj.Checked = false;

            }
        }
        private void btnFemale_Click(object sender, EventArgs e)
        {
            if (btnFemale.Tag.ToString() == "2")
            {
                btnFemale.Tag = "1";
                btnFemale.Image = Properties.Resources.btnFemaleNormal;

                btnChild.Tag = btnMale.Tag = "2";
                btnChild.Image = Properties.Resources.btnChildGray;
                btnMale.Image = Properties.Resources.btnMaleGray;
                rbImportObj.Checked = false;
            }
        }
        private void btnChild_Click(object sender, EventArgs e)
        {
            if (btnChild.Tag.ToString() == "2")
            {
                btnChild.Tag = "1";
                btnChild.Image = Properties.Resources.btnChildNormal;

                btnMale.Tag = btnFemale.Tag = "2";
                btnMale.Image = Properties.Resources.btnMaleGray;
                btnFemale.Image = Properties.Resources.btnFemaleGray;
                rbImportObj.Checked = false;
            }
        }

        #endregion



        public void CreateProject()
        {
            #region Корректируем размер фотки

            using (var ms = new MemoryStream(File.ReadAllBytes(templateImage))) // Don't use using!!
            {
                var img = (Bitmap)Bitmap.FromStream(ms);
                var max = (float)Math.Max(img.Width, img.Height);
                if (max != SelectedSize)
                {
                    var k = SelectedSize / max;
                    var newImg = ImageEx.ResizeImage(img, new Size((int)Math.Round(img.Width * k), (int)Math.Round((img.Height * k))));

                    templateImage = UserConfig.AppDataDir;
                    FolderEx.CreateDirectory(templateImage);
                    templateImage = Path.Combine(templateImage, "tempProjectImage.jpg");

                    newImg.Save(templateImage, ImageFormat.Jpeg);
                }
            }

            #endregion

            ProgramCore.Project = new Project(ProjectName, ProjectFolder, templateImage, ManType, CustomModelPath, true, SelectedSize);

            ProgramCore.Project.FaceRectRelative = new RectangleF(LeftCheek.GetMinX(), nextHeadRect.Y, RightCheek.GetMaxX() - LeftCheek.GetMinX(), nextHeadRect.Bottom - nextHeadRect.Y);
            ProgramCore.Project.nextHeadRectF = fcr.nextHeadRectF;
            ProgramCore.Project.MouthCenter = fcr.MouthCenter;
            ProgramCore.Project.LeftEyeCenter = fcr.LeftEyeCenter;
            ProgramCore.Project.RightEyeCenter = fcr.RightEyeCenter;
            ProgramCore.Project.FaceColor = fcr.FaceColor;

            var aabb = ProgramCore.MainForm.ctrlRenderControl.InitializeShapedotsHelper(true);         // инициализация точек головы. эта инфа тоже сохранится в проект
            ProgramCore.MainForm.UpdateProjectControls(true, aabb);

            ProgramCore.Project.ToStream();
            // ProgramCore.MainForm.ctrlRenderControl.UpdateMeshProportions();

            if (ProgramCore.Project.ManType == ManType.Custom)
            {
                ProgramCore.MainForm.ctrlRenderControl.Mode = Mode.SetCustomControlPoints;
                ProgramCore.MainForm.ctrlRenderControl.InitializeCustomControlSpritesPosition();

                var exampleImgPath = Path.Combine(Application.StartupPath, "Plugin", "ControlBaseDotsExample.jpg");
                using (var ms = new MemoryStream(File.ReadAllBytes(exampleImgPath))) // Don't use using!!
                    ProgramCore.MainForm.ctrlTemplateImage.SetTemplateImage((Bitmap)Bitmap.FromStream(ms), false);          // устанавливаем картинку помощь для юзера
            }

            var projectPath = Path.Combine(ProjectFolder, string.Format("{0}.hds", ProjectName));
            ProgramCore.MainForm.mruManager.Add(projectPath);
        }



        /// <summary> Пересчитать положение прямоугольника в зависимост от размера картинки на picturetemplate </summary>
        private void RecalcRealTemplateImagePosition()
        {
            var pb = pictureTemplate;
            if (pb.Image == null)
            {
                ImageTemplateWidth = ImageTemplateHeight = 0;
                ImageTemplateOffsetX = ImageTemplateOffsetY = -1;
                MouthTransformed = PointF.Empty;
                return;
            }

            if (pb.Width / (double)pb.Height < pb.Image.Width / (double)pb.Image.Height)
            {
                ImageTemplateWidth = pb.Width;
                ImageTemplateHeight = pb.Image.Height * ImageTemplateWidth / pb.Image.Width;
            }
            else if (pb.Width / (double)pb.Height > pb.Image.Width / (double)pb.Image.Height)
            {
                ImageTemplateHeight = pb.Height;
                ImageTemplateWidth = pb.Image.Width * ImageTemplateHeight / pb.Image.Height;
            }
            else // if ((double)pb.Width / (double)pb.Height == (double)pb.Image.Width / (double)pb.Image.Height)
            {
                ImageTemplateWidth = pb.Width;
                ImageTemplateHeight = pb.Height;
            }

            ImageTemplateOffsetX = (pb.Width - ImageTemplateWidth) / 2;
            ImageTemplateOffsetY = (pb.Height - ImageTemplateHeight) / 2;

            MouthTransformed = new PointF(fcr.MouthCenter.X * ImageTemplateWidth + ImageTemplateOffsetX,
                                          fcr.MouthCenter.Y * ImageTemplateHeight + ImageTemplateOffsetY);

            LeftEyeTransformed = new PointF(fcr.LeftEyeCenter.X * ImageTemplateWidth + ImageTemplateOffsetX,
                              fcr.LeftEyeCenter.Y * ImageTemplateHeight + ImageTemplateOffsetY);
            RightEyeTransformed = new PointF(fcr.RightEyeCenter.X * ImageTemplateWidth + ImageTemplateOffsetX,
                              fcr.RightEyeCenter.Y * ImageTemplateHeight + ImageTemplateOffsetY);

            //TopEdgeTransformed.Y = (fcr.FaceRectRelative.Y * ImageTemplateHeight + ImageTemplateOffsetY - (ImageTemplateHeight * 0.40f))*1.5f;
            //BottomEdgeTransformed.Y = (fcr.FaceRectRelative.Bottom * ImageTemplateHeight + ImageTemplateOffsetY - (ImageTemplateHeight * 0.05f))*1.5f;
            //BottomEdgeTransformed.Y -= BottomEdgeTransformed.Height;

            TopEdgeTransformed.Y = nextHeadRect.Y * ImageTemplateHeight + ImageTemplateOffsetY;
            BottomEdgeTransformed.Y = (nextHeadRect.Bottom * ImageTemplateHeight + ImageTemplateOffsetY) - BottomEdgeTransformed.Height;

            //      LeftCheek.TopCheek = new RectangleF(LeftEyeTransformed.X - 10, TopEdgeTransformed.Y + (BottomEdgeTransformed.Y - TopEdgeTransformed.Y) / 3f, 100, 100);


            LeftCheek.Transform(ImageTemplateWidth, ImageTemplateHeight, ImageTemplateOffsetX, ImageTemplateOffsetY);
            RightCheek.Transform(ImageTemplateWidth, ImageTemplateHeight, ImageTemplateOffsetX, ImageTemplateOffsetY);



            //fcr.nextHeadRectF.Y = nextHeadRect.Y;
            //fcr.nextHeadRectF.Height = nextHeadRect.Height;
            //fcr.NextHeadRectInt.Y = (int)TopEdgeTransformed.Y;
            //fcr.NextHeadRectInt.Height = (int)BottomEdgeTransformed.Bottom - nextHeadRectInt.Y;

            if (TopEdgeTransformed.Y < 0)
                TopEdgeTransformed.Y = 0;

            fcr.nextHeadRectF.Y = (TopEdgeTransformed.Y - ImageTemplateOffsetY) / ImageTemplateHeight;
            fcr.nextHeadRectF.Height = (BottomEdgeTransformed.Bottom / ImageTemplateHeight) - fcr.nextHeadRectF.Y;
        }

        private float centerX(RectangleF rect)
        {
            return rect.Left + rect.Width / 2;
        }

        private void rbImportObj_CheckedChanged(object sender, EventArgs e)
        {
            if (rbImportObj.Checked)
            {
                btnFemale.Tag = btnChild.Tag = btnMale.Tag = "2";
                btnChild.Image = Properties.Resources.btnChildGray;
                btnMale.Image = Properties.Resources.btnMaleGray;
                btnFemale.Image = Properties.Resources.btnFemaleGray;

                if (!ProgramCore.PluginMode)
                {
                    using (var ofd = new OpenFileDialogEx("Select obj file", "OBJ Files|*.obj"))
                    {
                        ofd.Multiselect = false;
                        if (ofd.ShowDialog() != DialogResult.OK)
                        {
                            btnMale_Click(this, new EventArgs());
                            return;
                        }

                        //btnNext.Enabled = true;
                        CustomModelPath = ofd.FileName;
                    }
                }
            }
        }

        private void pictureTemplate_Paint(object sender, PaintEventArgs e)
        {
            if (string.IsNullOrEmpty(templateImage))
                return;

            e.Graphics.FillEllipse(DrawingTools.GreenBrushTransparent80, LeftEyeTransformed.X - HalfCircleRadius, LeftEyeTransformed.Y - HalfCircleRadius, CircleRadius, CircleRadius);
            e.Graphics.FillEllipse(DrawingTools.BlueSolidBrush, LeftEyeTransformed.X - HalfCircleSmallRadius, LeftEyeTransformed.Y - HalfCircleSmallRadius, CircleSmallRadius, CircleSmallRadius);

            e.Graphics.FillEllipse(DrawingTools.GreenBrushTransparent80, RightEyeTransformed.X - HalfCircleRadius, RightEyeTransformed.Y - HalfCircleRadius, CircleRadius, CircleRadius);
            e.Graphics.FillEllipse(DrawingTools.BlueSolidBrush, RightEyeTransformed.X - HalfCircleSmallRadius, RightEyeTransformed.Y - HalfCircleSmallRadius, CircleSmallRadius, CircleSmallRadius);


            e.Graphics.FillEllipse(DrawingTools.GreenBrushTransparent80, MouthTransformed.X - HalfCircleRadius, MouthTransformed.Y - HalfCircleRadius, CircleRadius, CircleRadius);
            e.Graphics.FillEllipse(DrawingTools.BlueSolidBrush, MouthTransformed.X - HalfCircleSmallRadius, MouthTransformed.Y - HalfCircleSmallRadius, CircleSmallRadius, CircleSmallRadius);

            e.Graphics.DrawArc(edgePen, TopEdgeTransformed, 220, 100);
            e.Graphics.DrawLine(arrowPen, centerX(TopEdgeTransformed), TopEdgeTransformed.Top, centerX(TopEdgeTransformed), TopEdgeTransformed.Top + 20);

            e.Graphics.DrawArc(edgePen, BottomEdgeTransformed, 50, 80);
            e.Graphics.DrawLine(arrowPen, centerX(BottomEdgeTransformed), BottomEdgeTransformed.Bottom, centerX(BottomEdgeTransformed), BottomEdgeTransformed.Bottom - 20);

            /*   e.Graphics.DrawArc(edgePen, LeftCheek.TopCheek, 200, 50);
               e.Graphics.DrawLine(arrowPen, centerX(LeftCheek.TopCheek), LeftCheek.TopCheek.Bottom, centerX(LeftCheek.TopCheek), LeftCheek.TopCheek.Bottom - 20);
               e.Graphics.DrawRectangle(edgePen,LeftCheek.TopCheek.X, LeftCheek.TopCheek.Y, LeftCheek.TopCheek.Width, LeftCheek.TopCheek.Height);*/

            LeftCheek.DrawLeft(e.Graphics);
            RightCheek.DrawRight(e.Graphics);

            ////e.Graphics.DrawRectangle(edgePen, 107, -48, 435, 569);
            ////ImageTemplateWidth + ImageTemplateOffsetX
            ////TopEdgeTransformed.Y = nextHeadRect.Y * ImageTemplateHeight + ImageTemplateOffsetY;
            ////BottomEdgeTransformed.Y = (nextHeadRect.Bottom * ImageTemplateHeight + ImageTemplateOffsetY) - BottomEdgeTransformed.Height;
            //float x1 = 0.07151565f;
            //float w1 = 0.8970337f-0.07151565f;
            //float y1 = 0.00f * ImageTemplateHeight + ImageTemplateOffsetY;
            //float h1 = 0.999f * ImageTemplateHeight + ImageTemplateOffsetY;
            //x1 = x1 * ImageTemplateWidth + ImageTemplateOffsetX;
            //w1 = w1 * ImageTemplateWidth + ImageTemplateOffsetX;
            ////y1 = y1 * ImageTemplateHeight + ImageTemplateOffsetY;
            ////h1 = h1 * ImageTemplateHeight + ImageTemplateOffsetY;
            //arrowPen.Width = 1;
            //e.Graphics.DrawRectangle(arrowPen, x1, y1, w1, h1);
        }
        private void pictureTemplate_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                leftMousePressed = true;

                headHandPoint.X = (ImageTemplateOffsetX + e.X) / (ImageTemplateWidth * 1f);
                headHandPoint.Y = (ImageTemplateOffsetY + e.Y) / (ImageTemplateHeight * 1f);

                if (e.X >= LeftEyeTransformed.X - HalfCircleRadius && e.X <= LeftEyeTransformed.X + HalfCircleRadius && e.Y >= LeftEyeTransformed.Y - HalfCircleRadius && e.Y <= LeftEyeTransformed.Y + HalfCircleRadius)
                {
                    currentSelection = Selection.LeftEye;
                    tempSelectedPoint = fcr.LeftEyeCenter;
                    Cursor = ProgramCore.MainForm.GrabbingCursor;
                }
                else if (e.X >= RightEyeTransformed.X - HalfCircleRadius && e.X <= RightEyeTransformed.X + HalfCircleRadius && e.Y >= RightEyeTransformed.Y - HalfCircleRadius && e.Y <= RightEyeTransformed.Y + HalfCircleRadius)
                {
                    currentSelection = Selection.RightEye;
                    tempSelectedPoint = fcr.RightEyeCenter;
                    Cursor = ProgramCore.MainForm.GrabbingCursor;
                }
                else if (e.X >= MouthTransformed.X - HalfCircleRadius && e.X <= MouthTransformed.X + HalfCircleRadius && e.Y >= MouthTransformed.Y - HalfCircleRadius && e.Y <= MouthTransformed.Y + HalfCircleRadius)
                {
                    currentSelection = Selection.Mouth;
                    tempSelectedPoint = fcr.MouthCenter;
                    Cursor = ProgramCore.MainForm.GrabbingCursor;
                }
                else
                {
                    var leftSelection = LeftCheek?.CheckGrab(e.X, e.Y, true) ?? -1;
                    var rightSelection = RightCheek?.CheckGrab(e.X, e.Y, true) ?? -1;
                    if (leftSelection != -1)
                    {
                        switch (leftSelection)
                        {
                            case 0:
                                currentSelection = Selection.LeftTopCheek;
                                tempSelectedPoint = new Vector2(LeftCheek.TopCheek.X, LeftCheek.TopCheek.Y);
                                tempSelectedPoint2 = new Vector2(RightCheek.TopCheek.X, RightCheek.TopCheek.Y);
                                break;
                            case 1:
                                currentSelection = Selection.LeftCenterCheek;
                                tempSelectedPoint = new Vector2(LeftCheek.CenterCheek.X, LeftCheek.CenterCheek.Y);
                                tempSelectedPoint2 = new Vector2(RightCheek.CenterCheek.X, RightCheek.CenterCheek.Y);
                                break;
                            case 2:
                                currentSelection = Selection.LeftBottomCheek;
                                tempSelectedPoint = new Vector2(LeftCheek.DownCheek.X, LeftCheek.DownCheek.Y);
                                tempSelectedPoint2 = new Vector2(RightCheek.DownCheek.X, RightCheek.DownCheek.Y);
                                break;
                        }
                        Cursor = ProgramCore.MainForm.GrabbingCursor;
                        startMousePoint = new Point(e.X, e.Y);

                    }
                    else if (rightSelection != -1)
                    {
                        switch (rightSelection)
                        {
                            case 0:
                                currentSelection = Selection.RightTopCheek;
                                tempSelectedPoint = new Vector2(RightCheek.TopCheek.X, RightCheek.TopCheek.Y);
                                tempSelectedPoint2 = new Vector2(LeftCheek.TopCheek.X, LeftCheek.TopCheek.Y);
                                break;
                            case 1:
                                currentSelection = Selection.RightCenterCheek;
                                tempSelectedPoint = new Vector2(RightCheek.CenterCheek.X, RightCheek.CenterCheek.Y);
                                tempSelectedPoint2 = new Vector2(LeftCheek.CenterCheek.X, LeftCheek.CenterCheek.Y);
                                break;
                            case 2:
                                currentSelection = Selection.RightBottomCheek;
                                tempSelectedPoint = new Vector2(RightCheek.DownCheek.X, RightCheek.DownCheek.Y);
                                tempSelectedPoint2 = new Vector2(LeftCheek.DownCheek.X, LeftCheek.DownCheek.Y);
                                break;
                        }
                        Cursor = ProgramCore.MainForm.GrabbingCursor;
                        startMousePoint = new Point(e.X, e.Y);
                    }
                    else if (e.X >= TopEdgeTransformed.Left && e.X <= TopEdgeTransformed.Right && e.Y >= TopEdgeTransformed.Y && e.Y <= TopEdgeTransformed.Y + 20)
                    {
                        currentSelection = Selection.TopEdge;
                        startEdgeRect = TopEdgeTransformed;
                        startMousePoint = new Point(e.X, e.Y);
                        tempSelectedPoint = new Vector2(0, nextHeadRect.Y);
                        tempSelectedPoint2 = new Vector2(0, nextHeadRect.Height);
                        Cursor = ProgramCore.MainForm.GrabbingCursor;
                    }
                    else if (e.X >= BottomEdgeTransformed.Left && e.X <= BottomEdgeTransformed.Right &&
                             e.Y >= BottomEdgeTransformed.Bottom - 20 && e.Y <= BottomEdgeTransformed.Bottom)
                    {
                        currentSelection = Selection.BottomEdge;
                        startEdgeRect = BottomEdgeTransformed;
                        startMousePoint = new Point(e.X, e.Y);
                        tempSelectedPoint = new Vector2(0, nextHeadRect.Y);
                        tempSelectedPoint2 = new Vector2(0, nextHeadRect.Height);
                        Cursor = ProgramCore.MainForm.GrabbingCursor;
                    }
                }
            }
        }
        private void pictureTemplate_MouseMove(object sender, MouseEventArgs e)
        {
            if (startMousePoint == Point.Empty)
                startMousePoint = new Point(e.X, e.Y);

            if (leftMousePressed && currentSelection != Selection.Empty)
            {
                Vector2 newPoint;
                Vector2 delta2;
                newPoint.X = (ImageTemplateOffsetX + e.X) / (ImageTemplateWidth * 1f);
                newPoint.Y = (ImageTemplateOffsetY + e.Y) / (ImageTemplateHeight * 1f);

                delta2 = newPoint - headHandPoint;
                switch (currentSelection)
                {
                    case Selection.LeftEye:

                        fcr.LeftEyeCenter = tempSelectedPoint + delta2;
                        RecalcRealTemplateImagePosition();
                        break;
                    case Selection.RightEye:
                        fcr.RightEyeCenter = tempSelectedPoint + delta2;
                        RecalcRealTemplateImagePosition();
                        break;
                    case Selection.Mouth:
                        fcr.MouthCenter = tempSelectedPoint + delta2;
                        RecalcRealTemplateImagePosition();
                        break;
                    case Selection.TopEdge:
                        nextHeadRect.Y = (tempSelectedPoint + delta2).Y;
                        nextHeadRect.Height = (tempSelectedPoint2 - delta2).Y;
                        TopEdgeTransformed.X = BottomEdgeTransformed.X = startEdgeRect.X + (e.X - startMousePoint.X);
                        RecalcRealTemplateImagePosition();
                        break;
                    case Selection.BottomEdge:
                        nextHeadRect.Height = (tempSelectedPoint2 + delta2).Y;
                        TopEdgeTransformed.X = BottomEdgeTransformed.X = startEdgeRect.X + (e.X - startMousePoint.X);
                        RecalcRealTemplateImagePosition();
                        break;
                    case Selection.LeftTopCheek:
                        var newCheekPoint = tempSelectedPoint + delta2;
                        LeftCheek.TopCheek = new PointF(newCheekPoint.X, newCheekPoint.Y);

                        newCheekPoint = new Vector2(tempSelectedPoint2.X - delta2.X, tempSelectedPoint2.Y + delta2.Y);
                        RightCheek.TopCheek = new PointF(newCheekPoint.X, newCheekPoint.Y);
                        RecalcRealTemplateImagePosition();
                        break;
                    case Selection.LeftCenterCheek:
                        newCheekPoint = tempSelectedPoint + delta2;
                        LeftCheek.CenterCheek = new PointF(newCheekPoint.X, newCheekPoint.Y);

                        newCheekPoint = new Vector2(tempSelectedPoint2.X - delta2.X, tempSelectedPoint2.Y + delta2.Y);
                        RightCheek.CenterCheek = new PointF(newCheekPoint.X, newCheekPoint.Y);
                        RecalcRealTemplateImagePosition();
                        break;
                    case Selection.LeftBottomCheek:
                        newCheekPoint = tempSelectedPoint + delta2;
                        LeftCheek.DownCheek = new PointF(newCheekPoint.X, newCheekPoint.Y);

                        newCheekPoint = new Vector2(tempSelectedPoint2.X - delta2.X, tempSelectedPoint2.Y + delta2.Y);
                        RightCheek.DownCheek = new PointF(newCheekPoint.X, newCheekPoint.Y);
                        RecalcRealTemplateImagePosition();
                        break;
                    case Selection.RightTopCheek:
                        newCheekPoint = tempSelectedPoint + delta2;
                        RightCheek.TopCheek = new PointF(newCheekPoint.X, newCheekPoint.Y);

                        newCheekPoint = new Vector2(tempSelectedPoint2.X - delta2.X, tempSelectedPoint2.Y + delta2.Y);
                        LeftCheek.TopCheek = new PointF(newCheekPoint.X, newCheekPoint.Y);
                        RecalcRealTemplateImagePosition();
                        break;
                    case Selection.RightCenterCheek:
                        newCheekPoint = tempSelectedPoint + delta2;
                        RightCheek.CenterCheek = new PointF(newCheekPoint.X, newCheekPoint.Y);

                        newCheekPoint = new Vector2(tempSelectedPoint2.X - delta2.X, tempSelectedPoint2.Y + delta2.Y);
                        LeftCheek.CenterCheek = new PointF(newCheekPoint.X, newCheekPoint.Y);
                        RecalcRealTemplateImagePosition();
                        break;
                    case Selection.RightBottomCheek:
                        newCheekPoint = tempSelectedPoint + delta2;
                        RightCheek.DownCheek = new PointF(newCheekPoint.X, newCheekPoint.Y);

                        newCheekPoint = new Vector2(tempSelectedPoint2.X - delta2.X, tempSelectedPoint2.Y + delta2.Y);
                        LeftCheek.DownCheek = new PointF(newCheekPoint.X, newCheekPoint.Y);
                        RecalcRealTemplateImagePosition();
                        break;
                }
            }
            else
            {
                if (e.X >= LeftEyeTransformed.X - HalfCircleRadius && e.X <= LeftEyeTransformed.X + HalfCircleRadius && e.Y >= LeftEyeTransformed.Y - HalfCircleRadius && e.Y <= LeftEyeTransformed.Y + HalfCircleRadius)
                    Cursor = ProgramCore.MainForm.GrabCursor;
                else if (e.X >= RightEyeTransformed.X - HalfCircleRadius && e.X <= RightEyeTransformed.X + HalfCircleRadius && e.Y >= RightEyeTransformed.Y - HalfCircleRadius && e.Y <= RightEyeTransformed.Y + HalfCircleRadius)
                    Cursor = ProgramCore.MainForm.GrabCursor;
                else if (e.X >= MouthTransformed.X - HalfCircleRadius && e.X <= MouthTransformed.X + HalfCircleRadius && e.Y >= MouthTransformed.Y - HalfCircleRadius && e.Y <= MouthTransformed.Y + HalfCircleRadius)
                    Cursor = ProgramCore.MainForm.GrabCursor;
                else if (e.X >= TopEdgeTransformed.Left && e.X <= TopEdgeTransformed.Right && e.Y >= TopEdgeTransformed.Y && e.Y <= TopEdgeTransformed.Y + 20)
                    Cursor = ProgramCore.MainForm.GrabCursor;
                else if (e.X >= BottomEdgeTransformed.Left && e.X <= BottomEdgeTransformed.Right && e.Y >= BottomEdgeTransformed.Bottom - 20 && e.Y <= BottomEdgeTransformed.Bottom)
                    Cursor = ProgramCore.MainForm.GrabCursor;
                else if (LeftCheek != null && LeftCheek.CheckGrab(e.X, e.Y, false) != -1)
                    Cursor = ProgramCore.MainForm.GrabCursor;
                else if (RightCheek != null && RightCheek.CheckGrab(e.X, e.Y, false) != -1)
                    Cursor = ProgramCore.MainForm.GrabCursor;
                else
                    Cursor = Cursors.Arrow;
            }

        }



        public enum Selection
        {
            LeftEye,
            RightEye,
            Mouth,
            TopEdge,
            BottomEdge,
            LeftTopCheek,
            LeftCenterCheek,
            LeftBottomCheek,
            RightTopCheek,
            RightCenterCheek,
            RightBottomCheek,
            Empty
        }
        private Selection currentSelection = Selection.Empty;

        private void pictureTemplate_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftMousePressed && currentSelection != Selection.Empty)
                RecalcRealTemplateImagePosition();

            startMousePoint = Point.Empty;
            currentSelection = Selection.Empty;
            leftMousePressed = false;

            headHandPoint = Vector2.Zero;
            tempSelectedPoint = Vector2.Zero;
            tempSelectedPoint2 = Vector2.Zero;
            Cursor = Cursors.Arrow;
        }

        private void RenderTimer_Tick(object sender, EventArgs e)
        {
            pictureTemplate.Refresh();
        }

        private void frmNewProject4PrintAhead_Resize(object sender, EventArgs e)
        {
            RecalcRealTemplateImagePosition();
        }

        private void rbSaved_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void CheekTimer_Tick(object sender, EventArgs e)
        {
            LeftCheek.UpdateVisibility();
        }
    }
}
