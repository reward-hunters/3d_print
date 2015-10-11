using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using OpenTK;
using RH.HeadShop.Helpers;
using RH.HeadShop.IO;
using RH.HeadShop.Render.Helpers;

namespace RH.HeadShop.Controls
{
    public partial class frmNewProject2 : Form
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

        public DialogResult dialogResult = DialogResult.Cancel;

        public int ImageTemplateWidth;
        public int ImageTemplateHeight;
        public int ImageTemplateOffsetX;
        public int ImageTemplateOffsetY;

        public PointF MouthTransformed;
        public PointF LeftEyeTransformed;
        public PointF RightEyeTransformed;

        private Pen edgePen;
        private Pen arrowPen;
        private const float eWidth = 300;
        public RectangleF TopEdgeTransformed = new RectangleF(400 - eWidth / 2, 30, eWidth, eWidth);
        public RectangleF BottomEdgeTransformed = new RectangleF(400 - eWidth / 2, 400 - eWidth, eWidth, eWidth);

        private bool leftMousePressed;
        private Point startMousePoint;
        private RectangleF startEdgeRect;

        private Vector2 headHandPoint = Vector2.Zero;
        private Vector2 tempSelectedPoint = Vector2.Zero;
        private Vector2 tempSelectedPoint2 = Vector2.Zero;
        public RectangleF nextHeadRect = new RectangleF();
        public RectangleF nextHeadRectF = new RectangleF();
        private enum Selection
        {
            LeftEye,
            RightEye,
            Mouth,
            TopEdge,
            BottomEdge,
            Empty
        }
        private Selection currentSelection = Selection.Empty;

        private readonly FaceRecognition fcr;

        #endregion

        public frmNewProject2(string projectPath, string templateImagePath, FaceRecognition fcr)
        {
            InitializeComponent();
            this.fcr = fcr;

            edgePen = (Pen)DrawingTools.GreenPen.Clone();
            //edgePen.Width = 2;
            arrowPen = (Pen)DrawingTools.GreenPen.Clone();
            arrowPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            //arrowPen.Width = 2;

            nextHeadRect.Height = fcr.FaceRectRelative.Height * 3.77294f;
            float center = (fcr.FaceRectRelative.Height + fcr.FaceRectRelative.Y) / 2f * 1.4f;
            nextHeadRect.Y = center - (nextHeadRect.Height / 2f);
            nextHeadRect.Height *= 0.92f; //рисовать на месте нижней челюсти
            
            textName.Text = projectPath;
            if (!string.IsNullOrEmpty(templateImagePath))
            {
                using (var bmp = new Bitmap(templateImagePath))
                    pictureTemplate.Image = (Bitmap)bmp.Clone();
            }
            RecalcRealTemplateImagePosition();
            RenderTimer.Start();

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

        #region Form's event

        private void RenderTimer_Tick(object sender, EventArgs e)
        {
            pictureTemplate.Refresh();
        }
        private void frmNewProject2_Resize(object sender, EventArgs e)
        {
            RecalcRealTemplateImagePosition();
        }
        private void frmNewProject2_Load(object sender, EventArgs e)
        {
            if (UserConfig.ByName("Options")["Tutorials", "Recognize", "1"] == "1")
                ProgramCore.MainForm.frmTutRecognize.ShowDialog(this);
        }

        private float centerX(RectangleF rect)
        {
            return rect.Left + rect.Width / 2;
        }
        private float centerY(RectangleF rect)
        {
            return rect.Top + rect.Height / 2;
        }

        private void pictureTemplate_Paint(object sender, PaintEventArgs e)

        {
            e.Graphics.FillEllipse(DrawingTools.GreenBrushTransparent80, LeftEyeTransformed.X - 10, LeftEyeTransformed.Y - 10, 20, 20);
            e.Graphics.FillEllipse(DrawingTools.BlueSolidBrush, LeftEyeTransformed.X - 2, LeftEyeTransformed.Y - 2, 4, 4);

            e.Graphics.FillEllipse(DrawingTools.GreenBrushTransparent80, RightEyeTransformed.X - 10, RightEyeTransformed.Y - 10, 20, 20);
            e.Graphics.FillEllipse(DrawingTools.BlueSolidBrush, RightEyeTransformed.X - 2, RightEyeTransformed.Y - 2, 4, 4);


            e.Graphics.FillEllipse(DrawingTools.GreenBrushTransparent80, MouthTransformed.X - 10, MouthTransformed.Y - 10, 20, 20);
            e.Graphics.FillEllipse(DrawingTools.BlueSolidBrush, MouthTransformed.X - 2, MouthTransformed.Y - 2, 4, 4);

            e.Graphics.DrawArc(edgePen, TopEdgeTransformed, 220, 100);
            e.Graphics.DrawLine(arrowPen, centerX(TopEdgeTransformed), TopEdgeTransformed.Top, centerX(TopEdgeTransformed), TopEdgeTransformed.Top + 20);

            e.Graphics.DrawArc(edgePen, BottomEdgeTransformed, 50, 80);
            e.Graphics.DrawLine(arrowPen, centerX(BottomEdgeTransformed), BottomEdgeTransformed.Bottom, centerX(BottomEdgeTransformed), BottomEdgeTransformed.Bottom - 20);

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

                if (e.X >= LeftEyeTransformed.X - 10 && e.X <= LeftEyeTransformed.X + 10 && e.Y >= LeftEyeTransformed.Y - 10 && e.Y <= LeftEyeTransformed.Y + 10)
                {
                    currentSelection = Selection.LeftEye;
                    tempSelectedPoint = fcr.LeftEyeCenter;
                }
                else if (e.X >= RightEyeTransformed.X - 10 && e.X <= RightEyeTransformed.X + 10 && e.Y >= RightEyeTransformed.Y - 10 && e.Y <= RightEyeTransformed.Y + 10)
                {
                    currentSelection = Selection.RightEye;
                    tempSelectedPoint = fcr.RightEyeCenter;
                }
                else if (e.X >= MouthTransformed.X - 10 && e.X <= MouthTransformed.X + 10 && e.Y >= MouthTransformed.Y - 10 && e.Y <= MouthTransformed.Y + 10)
                {
                    currentSelection = Selection.Mouth;
                    tempSelectedPoint = fcr.MouthCenter;
                }
                else if (e.X >= TopEdgeTransformed.Left && e.X <= TopEdgeTransformed.Right  && e.Y >= TopEdgeTransformed.Y && e.Y <= TopEdgeTransformed.Y + 20)
                {
                    currentSelection = Selection.TopEdge;
                    startEdgeRect = TopEdgeTransformed;
                    startMousePoint = new Point(e.X, e.Y);
                    tempSelectedPoint = new Vector2(0, nextHeadRect.Y);
                    tempSelectedPoint2 = new Vector2(0, nextHeadRect.Height);
                }
                else if (e.X >= BottomEdgeTransformed.Left && e.X <= BottomEdgeTransformed.Right && e.Y >= BottomEdgeTransformed.Bottom - 20 && e.Y <= BottomEdgeTransformed.Bottom)
                {
                    currentSelection = Selection.BottomEdge;
                    startEdgeRect = BottomEdgeTransformed;
                    startMousePoint = new Point(e.X, e.Y);
                    tempSelectedPoint = new Vector2(0, nextHeadRect.Y);
                    tempSelectedPoint2 = new Vector2(0, nextHeadRect.Height);
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
                }
            }

        }
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
                btnNext.Enabled = true;
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
                btnNext.Enabled = true;
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
                btnNext.Enabled = true;
            }
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
                            btnNext.Enabled = false;
                            return;
                        }

                        btnNext.Enabled = true;
                        CustomModelPath = ofd.FileName;
                    }
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

        private void btnInfo_MouseDown(object sender, MouseEventArgs e)
        {
            btnInfo.Image = Properties.Resources.btnInfoPressed;
        }
        private void btnInfo_MouseUp(object sender, MouseEventArgs e)
        {
            ProgramCore.MainForm.ShowSiteInfo();
            btnInfo.Image = Properties.Resources.btnInfoNormal;
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

        private void btnNext_Click(object sender, EventArgs e)
        {
            dialogResult = DialogResult.OK;
            Close();
        }

        #endregion

        #region Supported void's

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

            //fcr.nextHeadRectF.Y = nextHeadRect.Y;
            //fcr.nextHeadRectF.Height = nextHeadRect.Height;
            //fcr.NextHeadRectInt.Y = (int)TopEdgeTransformed.Y;
            //fcr.NextHeadRectInt.Height = (int)BottomEdgeTransformed.Bottom - nextHeadRectInt.Y;

            if (TopEdgeTransformed.Y < 0)
                TopEdgeTransformed.Y = 0;

            fcr.nextHeadRectF.Y = (TopEdgeTransformed.Y - ImageTemplateOffsetY) / ImageTemplateHeight;
            fcr.nextHeadRectF.Height = (BottomEdgeTransformed.Bottom / ImageTemplateHeight) - fcr.nextHeadRectF.Y;

        }

        #endregion

    }
}
