using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using OpenTK;
using RH.HeadShop.IO;
using RH.HeadShop.Render.Helpers;
using System.Drawing.Imaging;
using Luxand;

namespace RH.HeadShop.Helpers
{
    public class OpenCvFaceRecognition
    {
        public RectangleF FaceRectRelative;
        public RectangleF nextHeadRectF = new RectangleF();
        public Vector2 LeftEyeCenter;
        public Vector2 RightEyeCenter;
        public Vector2 MouthCenter;
        public Vector4 FaceColor;

        private int angleCount = 0;

        public void Recognize(ref string path, bool needCrop)
        {
            FaceRectRelative = RectangleF.Empty;
            LeftEyeCenter = RightEyeCenter = MouthCenter = Vector2.Zero;

            var executablePath = Path.GetDirectoryName(Application.ExecutablePath);
            var faceFileName = Path.Combine(executablePath, "Haar cascades", "haarcascade_frontalface_default.xml");
            var eyeFileName = Path.Combine(executablePath, "Haar cascades", "haarcascade_eye.xml");
            var mouthFileName = Path.Combine(executablePath, "Haar cascades", "haarcascade_mcs_mouth.xml");

            var image = new Image<Bgr, byte>(path);
            var faceRectangle = Rectangle.Empty;

            var mouthRectangle = Rectangle.Empty;

            var gray = image.Convert<Gray, Byte>(); //Convert it to Grayscale

            //normalizes brightness and increases contrast of the image
            gray._EqualizeHist();

            if (needCrop)
            {
                var detector = new AdaptiveSkinDetector(1, AdaptiveSkinDetector.MorphingMethod.NONE);

                using (var skin = new Image<Gray, Byte>(image.Width, image.Height))
                {
                    var color = new Bgr(0, 0, 0);
                    var count = 0;
                    detector.Process(image, skin);
                    for (int y = 0; y < skin.Height; y++)
                    {
                        for (int x = 0; x < skin.Width; x++)
                        {
                            byte value = skin.Data[y, x, 0];
                            if (value != 0)
                            {
                                var c = image[y, x];
                                color.Red += c.Red;
                                color.Green += c.Green;
                                color.Blue += c.Blue;
                                ++count;
                            }
                        }
                    }
                    if (count > 0)
                    {
                        color.Red /= count;
                        color.Green /= count;
                        color.Blue /= count;
                        FaceColor = new Vector4((float)color.Red / 255f, (float)color.Green / 255f, (float)color.Blue / 255f, 1.0f);
                    }
                    else
                    {
                        FaceColor = new Vector4(0.72f, 0.72f, 0.72f, 1.0f);
                    }
                }
            }

            using (var face = new HaarCascade(faceFileName))
            using (var eye = new HaarCascade(eyeFileName))
            using (var mouth = new HaarCascade(mouthFileName))
            {

                //Detect the faces  from the gray scale image and store the locations as rectangle
                //The first dimensional is the channel
                //The second dimension is the index of the rectangle in the specific channel
                var facesDetected = gray.DetectHaarCascade(face, 1.1, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.FIND_BIGGEST_OBJECT, new Size(20, 20));
                if (facesDetected.Length == 0 || facesDetected[0].Length == 0)
                    faceRectangle = new Rectangle(0, 0, image.Width, image.Height);
                else
                {
                    faceRectangle = facesDetected[0][0].rect;

                    if (needCrop)       // если это создание проекта - то нужно обрезать фотку и оставить только голову
                    {
                        var newHeight = (int)(faceRectangle.Height * 0.5);
                        var newWidth = newHeight + faceRectangle.Height >= image.Height ? (int)(faceRectangle.Width * 0.2) : (int)(faceRectangle.Width * 0.3);   // если по условию - значит лицо крупно, и по ширине незачем широко оставлять
                        var newImageRect = faceRectangle;
                        newImageRect.Inflate(newWidth, newHeight);
                        if (newImageRect.Width < image.Width || newImageRect.Height < image.Height)         // если действительно лицо маленькое - делаем обрезание
                        {
                            newImageRect.X = newImageRect.X < 0 ? 0 : newImageRect.X;
                            newImageRect.Y = newImageRect.Y < 0 ? 0 : newImageRect.Y;
                            if (newImageRect.Width + newImageRect.X > image.Width)
                            {
                                var delta = (int)Math.Ceiling(((newImageRect.Width + newImageRect.X) - image.Width) * 0.5f);
                                if (newImageRect.X - delta < 0)
                                    newImageRect.Width -= delta * 2;
                                else
                                {
                                    newImageRect.Width -= delta;
                                    newImageRect.X -= delta;
                                }
                            }
                            if (newImageRect.Height + newImageRect.Y > image.Height)
                            {
                                var delta = (int)Math.Ceiling(((newImageRect.Height + newImageRect.Y) - image.Height) * 0.5f);
                                if (newImageRect.Y - delta < 0)
                                    newImageRect.Height -= delta * 2;
                                else
                                {
                                    newImageRect.Height -= delta;
                                    newImageRect.Y -= delta;
                                }
                            }

                            using (var croppedImage = ImageEx.Crop(path, newImageRect))
                            {
                                path = UserConfig.AppDataDir;
                                FolderEx.CreateDirectory(path);
                                path = Path.Combine(path, "tempHaarImage.jpg");
                                croppedImage.Save(path, ImageFormat.Jpeg);
                            }
                            Recognize(ref path, false);
                            return;
                        }
                    }
                }

                #region Рот

                //Set the region of interest on the faces
                gray.ROI = faceRectangle;
                var mouthDetected = gray.DetectHaarCascade(mouth, 1.1, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_ROUGH_SEARCH, new Size(20, 20));
                gray.ROI = Rectangle.Empty;

                if (mouthDetected.Length > 0 && mouthDetected[0].Length > 0)
                {
                    var sortedMouths = mouthDetected[0].OrderByDescending(x => x.rect.Y).ToList();
                    var hasBetterMouth = false;
                    if (sortedMouths.Count > 1)     // обрабатываем случай, когда два рта расположены близко, но нижний - неправильный.
                    {
                        var mouthRect1 = sortedMouths[0].rect;
                        var mouthRect2 = sortedMouths[1].rect;

                        if (Math.Abs(mouthRect1.Y - mouthRect2.Y) < 20)
                        {
                            var rectS1 = mouthRect1.Width * mouthRect1.Height;
                            var rectS2 = mouthRect2.Width * mouthRect2.Height;

                            if (rectS2 > rectS1)
                            {
                                hasBetterMouth = true;
                                mouthRectangle = mouthRect2;
                                mouthRectangle.Offset(faceRectangle.X, faceRectangle.Y);

                                var heightCoef = mouthRectangle.Height > 60 ? 0.28f : 0.4f;
                                //   var heightCoef = 1;
                                MouthCenter = new Vector2(mouthRectangle.X + mouthRectangle.Width * 0.5f, mouthRectangle.Y + mouthRectangle.Height * heightCoef);
                            }
                        }
                    }

                    if (!hasBetterMouth)
                    {
                        mouthRectangle = sortedMouths[0].rect;
                        mouthRectangle.Offset(faceRectangle.X, faceRectangle.Y);

                        var heightCoef = mouthRectangle.Height > 60 ? 0.28f : 0.4f;     // более пиздец точное распознавание -_-
                        //var heightCoef = 1;
                        MouthCenter = new Vector2(mouthRectangle.X + mouthRectangle.Width * 0.5f, mouthRectangle.Y + mouthRectangle.Height * heightCoef);
                    }

                    image.Draw(mouthRectangle, new Bgr(Color.Green), 2);
                }

                if (MouthCenter == Vector2.Zero)        // если не определилось - втыкаем по дефолту
                    MouthCenter = new Vector2(faceRectangle.Width * 0.5f, faceRectangle.Height / 1.5f);

                #endregion

                #region Глазки

                //Set the region of interest on the faces
                gray.ROI = faceRectangle;
                var eyesDetected = gray.DetectHaarCascade(eye, 1.1, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(30, 30));
                gray.ROI = Rectangle.Empty;

                if (eyesDetected.Length > 0 && eyesDetected[0].Length > 0)
                {
                    if (eyesDetected[0].Length == 1)        // определился один глаз
                    {
                        var eyeRect = eyesDetected[0][0].rect;
                        eyeRect.Offset(faceRectangle.X, faceRectangle.Y);
                        var center = new Vector2(eyeRect.X + eyeRect.Width * 0.5f, eyeRect.Y + eyeRect.Height * 0.5f);

                        if (center.X < MouthCenter.X)           // определяем глаз по положению рта
                        {
                            LeftEyeCenter = center;
                        }
                        else
                        {
                            RightEyeCenter = center;
                        }
                    }
                    else            // определилось несколько глаз. выбираем нужные. развлекаемся
                    {
                        var sortedEyes = eyesDetected[0].OrderBy(x => x.rect.X).ToList();
                        var j1 = 0;
                        for (var j = 0; j < sortedEyes.Count - 1; j++)
                        {
                            var rf = sortedEyes[j].rect;
                            rf.Offset(faceRectangle.X, faceRectangle.Y);
                            var center = new Vector2(rf.X + rf.Width * 0.5f, rf.Y + rf.Height * 0.5f);
                            if (Math.Abs(center.Y - MouthCenter.Y) > 20)
                            {
                                LeftEyeCenter = center;
                                j1 = j;
                                break;
                            }
                        }

                        for (var i = sortedEyes.Count - 1; i > j1; i--)
                        {
                            var rf = sortedEyes[i].rect;
                            rf.Offset(faceRectangle.X, faceRectangle.Y);
                            var center = new Vector2(rf.X + rf.Width * 0.5f, rf.Y + rf.Height * 0.5f);

                            if (Math.Abs(center.Y - LeftEyeCenter.Y) < 65 && Math.Abs(center.X - LeftEyeCenter.X) > 20) // абсолютно от балды числа .что бы уж сильно явные выпады убрать
                            {
                                RightEyeCenter = center;
                                break;
                            }
                        }
                    }
                }

                #region Глазки не определились. Через три пизды колено определяем

                if (LeftEyeCenter == Vector2.Zero)
                {
                    if (RightEyeCenter != Vector2.Zero) // определяем через правый глаз и рот
                    {
                        var delta = Math.Abs(RightEyeCenter.X - MouthCenter.X);
                        LeftEyeCenter = new Vector2(MouthCenter.X - delta, RightEyeCenter.Y);
                    }
                    else                // примерно определяем через прямоугольник лица
                        LeftEyeCenter = new Vector2(faceRectangle.X + faceRectangle.Width / 3.5f, faceRectangle.Y + faceRectangle.Height / 3f);
                }

                if (RightEyeCenter == Vector2.Zero)
                {
                    if (LeftEyeCenter != Vector2.Zero) // определяем через левый глаз и рот
                    {
                        var delta = MouthCenter.X - LeftEyeCenter.X;
                        RightEyeCenter = new Vector2(MouthCenter.X + delta, LeftEyeCenter.Y);
                    }
                    else                // примерно определяем через прямоугольник лица
                        RightEyeCenter = new Vector2(faceRectangle.X + faceRectangle.Width / 3.5f, faceRectangle.Y + faceRectangle.Height / 3f);
                }

                #endregion

                #region Поворот фотки по глазам!

                var v = new Vector2(LeftEyeCenter.X - RightEyeCenter.X, LeftEyeCenter.Y - RightEyeCenter.Y);
                v.Normalize();      // ПД !
                var xVector = new Vector2(1, 0);

                var xDiff = xVector.X - v.X;
                var yDiff = xVector.Y - v.Y;
                var angle = Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI;

                if (Math.Abs(angle) > 1 && angleCount <= 5)                // поворачиваем наклоненные головы
                {
                    ++angleCount;

                    using (var ms = new MemoryStream(File.ReadAllBytes(path))) // Don't use using!!
                    {
                        var originalImg = (Bitmap)Bitmap.FromStream(ms);

                        path = UserConfig.AppDataDir;
                        FolderEx.CreateDirectory(path);
                        path = Path.Combine(path, "tempHaarImage.jpg");

                        using (var ii = ImageEx.RotateImage(new Bitmap(originalImg), (float)-angle))
                            ii.Save(path, ImageFormat.Jpeg);
                    }

                    Recognize(ref path, false);
                    return;
                }

                #endregion

                #endregion
            }

            #region Переводим в относительные координаты

            MouthCenter = new Vector2(MouthCenter.X / (image.Width * 1f), MouthCenter.Y / (image.Height * 1f));
            LeftEyeCenter = new Vector2(LeftEyeCenter.X / (image.Width * 1f), LeftEyeCenter.Y / (image.Height * 1f));
            RightEyeCenter = new Vector2(RightEyeCenter.X / (image.Width * 1f), RightEyeCenter.Y / (image.Height * 1f));

            var leftTop = new Vector2(LeftEyeCenter.X, Math.Max(LeftEyeCenter.Y, RightEyeCenter.Y));
            var rightBottom = new Vector2(RightEyeCenter.X, MouthCenter.Y);

            FaceRectRelative = new RectangleF(leftTop.X, leftTop.Y, rightBottom.X - leftTop.X, rightBottom.Y - leftTop.Y);

            #endregion
        }
    }

    public class LuxandFaceRecognition
    {
        public RectangleF FaceRectRelative;
        public Vector4 FaceColor;

        public Vector2 LeftEyeCenter;
        public Vector2 RightEyeCenter;

        public Vector2 LeftMouth;
        public Vector2 RightMouth;

        public Vector2 LeftNose;
        public Vector2 RightNose;

        public Vector2 TopFace;
        public Vector2 MiddleFace1;
        public Vector2 MiddleFace2;
        public Vector2 BottomFace;

        private Vector2 RightMiddleFace1;
        private Vector2 RightMiddleFace2;

        public float GetMinX()
        {
            var minX = float.MaxValue;
            minX = Math.Min(TopFace.X, minX);
            minX = Math.Min(MiddleFace1.X, minX);
            minX = Math.Min(MiddleFace2.X, minX);
            return minX;
        }
        public float GetMaxX()
        {
            var maxX = float.MinValue;
            maxX = Math.Max(TopFace.X, maxX);
            maxX = Math.Max(RightMiddleFace1.X, maxX);
            maxX = Math.Max(RightMiddleFace2.X, maxX);
            return maxX;
        }

        public List<Vector2> FacialFeatures;
        public bool IsMale;


        private int angleCount = 0;

        public void Recognize(ref string path, bool needCrop)
        {
            FaceRectRelative = RectangleF.Empty;
            LeftEyeCenter = RightEyeCenter = LeftMouth = LeftNose = RightNose = RightMouth = Vector2.Zero;

            var executablePath = Path.GetDirectoryName(Application.ExecutablePath);
            FSDK.TPoint[] pointFeature;
            FSDK.CImage image = new FSDK.CImage(path);


            var faceRectangle = Rectangle.Empty;
            var mouthRectangle = Rectangle.Empty;

            #region Определение цвета лица

            if (needCrop)
            {
                var openCvImage = new Image<Bgr, byte>(path);
                var detector = new AdaptiveSkinDetector(1, AdaptiveSkinDetector.MorphingMethod.NONE);

                using (var skin = new Image<Gray, Byte>(image.Width, image.Height))
                {
                    var color = new Bgr(0, 0, 0);
                    var count = 0;
                    detector.Process(openCvImage, skin);
                    for (int y = 0; y < skin.Height; y++)
                    {
                        for (int x = 0; x < skin.Width; x++)
                        {
                            byte value = skin.Data[y, x, 0];
                            if (value != 0)
                            {
                                var c = openCvImage[y, x];
                                color.Red += c.Red;
                                color.Green += c.Green;
                                color.Blue += c.Blue;
                                ++count;
                            }
                        }
                    }
                    if (count > 0)
                    {
                        color.Red /= count;
                        color.Green /= count;
                        color.Blue /= count;
                        FaceColor = new Vector4((float)color.Red / 255f, (float)color.Green / 255f, (float)color.Blue / 255f, 1.0f);
                    }
                    else
                    {
                        FaceColor = new Vector4(0.72f, 0.72f, 0.72f, 1.0f);
                    }
                }
            }

            #endregion

            FSDK.TFacePosition facePosition = image.DetectFace();
            if (0 == facePosition.w)
            {
                faceRectangle = new Rectangle(0, 0, image.Width, image.Height);
                MessageBox.Show("No faces detected", "Face Detection");
            }
            else
            {
                pointFeature = image.DetectFacialFeaturesInRegion(ref facePosition);

                String AttributeValues;         // определение пола
                FSDK.DetectFacialAttributeUsingFeatures(image.ImageHandle, ref pointFeature, "Gender", out AttributeValues, 1024);
                float ConfidenceMale = 0.0f;
                float ConfidenceFemale = 0.0f;
                FSDK.GetValueConfidence(AttributeValues, "Male", ref ConfidenceMale);
                FSDK.GetValueConfidence(AttributeValues, "Female", ref ConfidenceFemale);
                IsMale = ConfidenceMale > ConfidenceFemale;


                int left = facePosition.xc - (int)(facePosition.w * 0.6f);
                //   int top = facePosition.yc - (int)(facePosition.w * 0.5f);             // верхушку определяет неправильлно. поэтому просто не будем обрезать :)
                BottomFace = new Vector2(pointFeature[11].x, pointFeature[11].y);

                faceRectangle = new Rectangle(left, 0, (int)(facePosition.w * 1.2), BottomFace.Y + 10 < image.Height ? (int)(BottomFace.Y + 15) : image.Height);
                if (needCrop)       // если это создание проекта - то нужно обрезать фотку и оставить только голову
                {
                    using (var croppedImage = ImageEx.Crop(path, faceRectangle))
                    {
                        path = UserConfig.AppDataDir;
                        FolderEx.CreateDirectory(path);
                        path = Path.Combine(path, "tempHaarImage.jpg");
                        croppedImage.Save(path, ImageFormat.Jpeg);

                        Recognize(ref path, false);
                        return;
                    }
                }

                LeftEyeCenter = new Vector2(pointFeature[0].x, pointFeature[0].y);
                RightEyeCenter = new Vector2(pointFeature[1].x, pointFeature[1].y);

                LeftMouth = new Vector2(pointFeature[3].x, pointFeature[3].y);
                RightMouth = new Vector2(pointFeature[4].x, pointFeature[4].y);

                LeftNose = new Vector2(pointFeature[45].x, pointFeature[45].y);
                RightNose = new Vector2(pointFeature[46].x, pointFeature[46].y);

                TopFace = new Vector2(pointFeature[66].x, pointFeature[66].y);
                MiddleFace1 = new Vector2(pointFeature[66].x, pointFeature[66].y);
                MiddleFace2 = new Vector2(pointFeature[5].x, pointFeature[5].y);


                RightMiddleFace1 = new Vector2(pointFeature[67].x, pointFeature[67].y);
                RightMiddleFace2 = new Vector2(pointFeature[6].x, pointFeature[6].y);

                #region Поворот фотки по глазам!

                var v = new Vector2(LeftEyeCenter.X - RightEyeCenter.X, LeftEyeCenter.Y - RightEyeCenter.Y);
                v.Normalize();      // ПД !
                var xVector = new Vector2(1, 0);

                var xDiff = xVector.X - v.X;
                var yDiff = xVector.Y - v.Y;
                var angle = Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI;

                if (Math.Abs(angle) > 1 && angleCount <= 5)                // поворачиваем наклоненные головы
                {
                    ++angleCount;

                    using (var ms = new MemoryStream(File.ReadAllBytes(path))) // Don't use using!!
                    {
                        var originalImg = (Bitmap)Bitmap.FromStream(ms);

                        path = UserConfig.AppDataDir;
                        FolderEx.CreateDirectory(path);
                        path = Path.Combine(path, "tempHaarImage.jpg");

                        using (var ii = ImageEx.RotateImage(new Bitmap(originalImg), (float)-angle))
                            ii.Save(path, ImageFormat.Jpeg);
                    }

                    Recognize(ref path, false);
                    return;
                }

                #endregion

                #region Переводим в относительные координаты

                LeftMouth = new Vector2(LeftMouth.X / (image.Width * 1f), LeftMouth.Y / (image.Height * 1f));
                RightMouth = new Vector2(RightMouth.X / (image.Width * 1f), RightMouth.Y / (image.Height * 1f));

                LeftEyeCenter = new Vector2(LeftEyeCenter.X / (image.Width * 1f), LeftEyeCenter.Y / (image.Height * 1f));
                RightEyeCenter = new Vector2(RightEyeCenter.X / (image.Width * 1f), RightEyeCenter.Y / (image.Height * 1f));

                LeftNose = new Vector2(LeftNose.X / (image.Width * 1f), LeftNose.Y / (image.Height * 1f));
                RightNose = new Vector2(RightNose.X / (image.Width * 1f), RightNose.Y / (image.Height * 1f));

                TopFace = new Vector2(TopFace.X / (image.Width * 1f), TopFace.Y / (image.Height * 1f));
                MiddleFace1 = new Vector2(MiddleFace1.X / (image.Width * 1f), MiddleFace1.Y / (image.Height * 1f));
                MiddleFace2 = new Vector2(MiddleFace2.X / (image.Width * 1f), MiddleFace2.Y / (image.Height * 1f));
                BottomFace = new Vector2(BottomFace.X / (image.Width * 1f), BottomFace.Y / (image.Height * 1f));

                RightMiddleFace1 = new Vector2(RightMiddleFace1.X / (image.Width * 1f), RightMiddleFace1.Y / (image.Height * 1f));
                RightMiddleFace2 = new Vector2(RightMiddleFace2.X / (image.Width * 1f), RightMiddleFace2.Y / (image.Height * 1f));

                FacialFeatures = new List<Vector2>();
                foreach (var point in pointFeature)
                    FacialFeatures.Add(new Vector2(point.x / (image.Width * 1f), point.y / (image.Height * 1f)));

                /*     int left = facePosition.xc - (int)(facePosition.w * 0.6f);
                     int top = facePosition.yc - (int)(facePosition.w * 0.5f);
                     var lRelative = 

                     faceRectangle = new Rectangle(left, top, (int)(facePosition.w * 1.2), (int)(facePosition.w * 1.2));
                     FaceRectRelative = 
                     */


                //      var leftTop = new Vector2(LeftEyeCenter.X, Math.Max(LeftEyeCenter.Y, RightEyeCenter.Y));
                //     var rightBottom = new Vector2(RightEyeCenter.X, LeftMouth.Y);
                // FaceRectRelative = new RectangleF(leftTop.X, leftTop.Y, rightBottom.X - leftTop.X, rightBottom.Y - leftTop.Y);

                #endregion
            }
        }
    }

    public class Cheek
    {
        public PointF TopCheek;
        public PointF TopCheekTransformed;
        public bool TopVisible = true;
        public bool TopTouched;

        public PointF CenterCheek;
        public PointF CenterCheekTransformed;
        public bool CenterVisible = true;
        public bool CenterTouched;

        public PointF DownCheek;
        public PointF DownCheekTransformed;
        public bool DownVisible = true;
        public bool DownTouched;

        private const int HalfCircleRadius = 15;
        private const int CircleSmallRadius = 8;
        private const int HalfCircleSmallRadius = 4;

        public Cheek(float xPos, float centerY)
        {
            CenterCheek = new PointF(xPos, centerY);
            TopCheek = new PointF(xPos, centerY - centerY / 2f);
            DownCheek = new PointF(xPos, centerY + centerY / 2f);
        }

        public void Transform(float imageWidth, float imageHeight, float offsetX, float offsetY)
        {
            TopCheekTransformed = new PointF(TopCheek.X * imageWidth + offsetX, TopCheek.Y * imageHeight + offsetY);

            CenterCheekTransformed = new PointF(CenterCheek.X * imageWidth + offsetX, CenterCheek.Y * imageHeight + offsetY);

            DownCheekTransformed = new PointF(DownCheek.X * imageWidth + offsetX, DownCheek.Y * imageHeight + offsetY);
        }

        public int CheckGrab(float x, float y, bool mousePressed)
        {
            if (x >= TopCheekTransformed.X - HalfCircleSmallRadius && x <= TopCheekTransformed.X + HalfCircleSmallRadius &&
                y >= TopCheekTransformed.Y - HalfCircleSmallRadius && y <= TopCheekTransformed.Y + HalfCircleSmallRadius)
            {
                if (mousePressed)
                {
                    TopVisible = true;
                    TopTouched = true;
                }
                return 0;
            }

            if (x >= CenterCheekTransformed.X - HalfCircleSmallRadius &&
                x <= CenterCheekTransformed.X + HalfCircleSmallRadius &&
                y >= CenterCheekTransformed.Y - HalfCircleSmallRadius &&
                y <= CenterCheekTransformed.Y + HalfCircleSmallRadius)
            {
                if (mousePressed)
                {
                    CenterVisible = true;
                    CenterTouched = true;
                }
                return 1;
            }

            if (x >= DownCheekTransformed.X - HalfCircleSmallRadius &&
                x <= DownCheekTransformed.X + HalfCircleSmallRadius &&
                y >= DownCheekTransformed.Y - HalfCircleSmallRadius &&
                y <= DownCheekTransformed.Y + HalfCircleSmallRadius)
            {
                if (mousePressed)
                {
                    DownVisible = true;
                    DownTouched = true;
                }
                return 2;
            }

            return -1;
        }

        public void UpdateVisibility()
        {
            if (!TopTouched)
                TopVisible = !TopVisible;
            if (!CenterTouched)
                CenterVisible = !CenterVisible;
            if (!DownTouched)
                DownVisible = !DownVisible;
        }

        public float GetMaxX()
        {
            var max = float.MinValue;
            max = Math.Max(TopCheek.X, max);
            max = Math.Max(CenterCheek.X, max);
            max = Math.Max(DownCheek.X, max);
            return max;
        }
        public float GetMinX()
        {
            var min = float.MaxValue;
            min = Math.Min(TopCheek.X, min);
            min = Math.Min(CenterCheek.X, min);
            min = Math.Min(DownCheek.X, min);
            return min;
        }

        Pen arrowsPen = new Pen(Color.DarkOliveGreen, 2);
        public void DrawLeft(Graphics g)
        {
            if (TopVisible)
            {
                g.FillRectangle(Brushes.DarkOliveGreen, TopCheekTransformed.X - HalfCircleSmallRadius,
                    TopCheekTransformed.Y - HalfCircleSmallRadius, CircleSmallRadius, CircleSmallRadius);
                g.DrawLine(arrowsPen, TopCheekTransformed.X, TopCheekTransformed.Y,
                    TopCheekTransformed.X + HalfCircleRadius, TopCheekTransformed.Y + HalfCircleRadius);
            }

            if (CenterVisible)
            {
                g.FillRectangle(Brushes.DarkOliveGreen, CenterCheekTransformed.X - HalfCircleSmallRadius,
                    CenterCheekTransformed.Y - HalfCircleSmallRadius, CircleSmallRadius, CircleSmallRadius);
                g.DrawLine(arrowsPen, CenterCheekTransformed.X, CenterCheekTransformed.Y,
                    CenterCheekTransformed.X + HalfCircleRadius, CenterCheekTransformed.Y);
            }

            if (DownVisible)
            {
                g.FillRectangle(Brushes.DarkOliveGreen, DownCheekTransformed.X - HalfCircleSmallRadius,
                    DownCheekTransformed.Y - HalfCircleSmallRadius, CircleSmallRadius, CircleSmallRadius);
                g.DrawLine(arrowsPen, DownCheekTransformed.X, DownCheekTransformed.Y,
                    DownCheekTransformed.X + HalfCircleRadius, DownCheekTransformed.Y - HalfCircleRadius);
            }
        }
        public void DrawRight(Graphics g)
        {
            if (TopVisible)
            {
                g.FillRectangle(Brushes.DarkOliveGreen, TopCheekTransformed.X - HalfCircleSmallRadius,
                    TopCheekTransformed.Y - HalfCircleSmallRadius, CircleSmallRadius, CircleSmallRadius);
                g.DrawLine(arrowsPen, TopCheekTransformed.X, TopCheekTransformed.Y,
                    TopCheekTransformed.X - HalfCircleRadius, TopCheekTransformed.Y + HalfCircleRadius);
            }

            if (CenterVisible)
            {
                g.FillRectangle(Brushes.DarkOliveGreen, CenterCheekTransformed.X - HalfCircleSmallRadius,
                    CenterCheekTransformed.Y - HalfCircleSmallRadius, CircleSmallRadius, CircleSmallRadius);
                g.DrawLine(arrowsPen, CenterCheekTransformed.X, CenterCheekTransformed.Y,
                    CenterCheekTransformed.X - HalfCircleRadius, CenterCheekTransformed.Y);
            }

            if (DownVisible)
            {
                g.FillRectangle(Brushes.DarkOliveGreen, DownCheekTransformed.X - HalfCircleSmallRadius,
                    DownCheekTransformed.Y - HalfCircleSmallRadius, CircleSmallRadius, CircleSmallRadius);
                g.DrawLine(arrowsPen, DownCheekTransformed.X, DownCheekTransformed.Y,
                    DownCheekTransformed.X - HalfCircleRadius, DownCheekTransformed.Y - HalfCircleRadius);
            }
        }
    }
}
