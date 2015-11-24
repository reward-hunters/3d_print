using System;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using RH.HeadShop.Helpers;
using RH.HeadShop.Render.Helpers;

namespace RH.HeadShop.Controls
{
    public partial class frmNewProfilePict2 : UserControlEx
    {
        #region Var

        private int ImageTemplateWidth;
        private int ImageTemplateHeight;
        private int ImageTemplateOffsetX;
        private int ImageTemplateOffsetY;

        /// <summary> Позиции глаза и рта, трансформированые для отображения на текущей картинки (смасштабированной) </summary>
        private PointF MouthTransformed;
        private PointF EyeTransformed;

        /// <summary> Позиции глаза и рта в относительных координатах </summary>
        public Vector2 MouthRelative;
        public Vector2 EyeRelative;

        private bool leftMousePressed;
        private Point startMousePoint;
        private Vector2 headHandPoint = Vector2.Zero;
        private Vector2 tempSelectedPoint = Vector2.Zero;
        private enum Selection
        {
            Eye,
            Mouth,
            Empty
        }
        private Selection currentSelection = Selection.Empty;

        /// <summary> Путь до основного изображения. Путь до повернутого и обрезаного изображения </summary>
        private string originalPath;
        public string RotatedPath;

        #endregion

        public frmNewProfilePict2(string originalImagePath, string rotatedImagePath, Vector2 mouthRelative, Vector2 eyeRelative)
        {
            InitializeComponent();
            textName.Text = originalImagePath;

            originalPath = originalImagePath;
            RotatedPath = rotatedImagePath;

            MouthRelative = mouthRelative;
            EyeRelative = eyeRelative;

            using (var bmp = new Bitmap(rotatedImagePath))
                pictureTemplate.Image = (Bitmap)bmp.Clone();

            RecalcRealTemplateImagePosition();
            RenderTimer.Start();
        }

        #region Form's event

        private void frmNewProject2_Resize(object sender, EventArgs e)
        {
            RecalcRealTemplateImagePosition();
        }

        private void pictureTemplate_Paint(object sender, PaintEventArgs e)
        {
            if (!EyeTransformed.IsEmpty)
            {
                e.Graphics.FillEllipse(DrawingTools.GreenBrushTransparent80, EyeTransformed.X - 10, EyeTransformed.Y - 10, 20, 20);
                e.Graphics.FillEllipse(DrawingTools.BlueSolidBrush, EyeTransformed.X - 2, EyeTransformed.Y - 2, 4, 4);
            }

            if (!MouthTransformed.IsEmpty)
            {
                e.Graphics.FillEllipse(DrawingTools.GreenBrushTransparent80, MouthTransformed.X - 10, MouthTransformed.Y - 10, 20, 20);
                e.Graphics.FillEllipse(DrawingTools.BlueSolidBrush, MouthTransformed.X - 2, MouthTransformed.Y - 2, 4, 4);
            }

        }
        private void pictureTemplate_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                leftMousePressed = true;

                headHandPoint.X = (ImageTemplateOffsetX + e.X) / (ImageTemplateWidth * 1f);
                headHandPoint.Y = (ImageTemplateOffsetY + e.Y) / (ImageTemplateHeight * 1f);

                if (e.X >= EyeTransformed.X - 10 && e.X <= EyeTransformed.X + 10 && e.Y >= EyeTransformed.Y - 10 && e.Y <= EyeTransformed.Y + 10)
                {
                    currentSelection = Selection.Eye;
                    tempSelectedPoint = EyeRelative;
                }
                else if (e.X >= MouthTransformed.X - 10 && e.X <= MouthTransformed.X + 10 && e.Y >= MouthTransformed.Y - 10 && e.Y <= MouthTransformed.Y + 10)
                {
                    currentSelection = Selection.Mouth;
                    tempSelectedPoint = MouthRelative;
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
                    case Selection.Eye:
                        EyeRelative = tempSelectedPoint + delta2;
                        RecalcRealTemplateImagePosition();
                        break;

                    case Selection.Mouth:
                        MouthRelative = tempSelectedPoint + delta2;
                        RecalcRealTemplateImagePosition();
                        break;

                }
            }

        }
        private void pictureTemplate_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftMousePressed && currentSelection != Selection.Empty)
            {
                RecalcRealTemplateImagePosition();
                //TODO: ДЕЛАЕМ ПОВОРОТ И ОБРЕЗКУ ФОТКИ!
            }

            startMousePoint = Point.Empty;
            currentSelection = Selection.Empty;
            leftMousePressed = false;

            headHandPoint = Vector2.Zero;
            tempSelectedPoint = Vector2.Zero;
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

        #endregion

        #region Supported void's

        private void RenderTimer_Tick(object sender, EventArgs e)
        {
            pictureTemplate.Refresh();
        }
        /// <summary> Пересчитать положение прямоугольника в зависимост от размера картинки на picturetemplate </summary>
        private void RecalcRealTemplateImagePosition()
        {
            var pb = pictureTemplate;
            if (pb.Image == null)
            {
                ImageTemplateWidth = ImageTemplateHeight = 0;
                ImageTemplateOffsetX = ImageTemplateOffsetY = -1;
                MouthTransformed = EyeTransformed = PointF.Empty;
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
            else
            {
                ImageTemplateWidth = pb.Width;
                ImageTemplateHeight = pb.Height;
            }

            ImageTemplateOffsetX = (pb.Width - ImageTemplateWidth) / 2;
            ImageTemplateOffsetY = (pb.Height - ImageTemplateHeight) / 2;

            MouthTransformed = new PointF(MouthRelative.X * ImageTemplateWidth + ImageTemplateOffsetX,
                                          MouthRelative.Y * ImageTemplateHeight + ImageTemplateOffsetY);

            EyeTransformed = new PointF(EyeRelative.X * ImageTemplateWidth + ImageTemplateOffsetX,
                              EyeRelative.Y * ImageTemplateHeight + ImageTemplateOffsetY);

        }

        #endregion

    }
}
