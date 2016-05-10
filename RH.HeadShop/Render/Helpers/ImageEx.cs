using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace RH.HeadShop.Render.Helpers
{
    public static class ImageEx
    {
        /// <summary> Обрезать изображение </summary>
        public static Bitmap Crop(Bitmap img, Rectangle cropArea)
        {
            return img.Clone(cropArea, img.PixelFormat);
        }
        public static Bitmap Crop(Image img, Rectangle cropArea)
        {
            var bmpImage = new Bitmap(img);
            return Crop(bmpImage, cropArea);
        }
        public static Bitmap Crop(string imgPath, Rectangle cropArea)
        {
            var bmpImage = new Bitmap(imgPath);
            return Crop(bmpImage, cropArea);
        }

        /// <summary> Вставить изображение в определенную область </summary>
        public static void CopyRegionIntoImage(Bitmap srcBitmap, ref Bitmap destBitmap, Rectangle destRegion)
        {
            using (var grD = Graphics.FromImage(destBitmap))
            {
                var srcRect = new Rectangle(0, 0, srcBitmap.Width, srcBitmap.Height);
                grD.DrawImage(srcBitmap, destRegion, srcRect, GraphicsUnit.Pixel);
            }
        }

        public static Bitmap RotateImage2(Bitmap bmpSrc, float theta)
        {
            Matrix mRotate = new Matrix();
            mRotate.Translate(bmpSrc.Width / -2, bmpSrc.Height / -2, MatrixOrder.Append);
            mRotate.RotateAt(theta, new System.Drawing.Point(0, 0), MatrixOrder.Append);
            using (GraphicsPath gp = new GraphicsPath())
            {  // transform image points by rotation matrix
                gp.AddPolygon(new System.Drawing.Point[] { new System.Drawing.Point(0, 0), new System.Drawing.Point(bmpSrc.Width, 0), new System.Drawing.Point(0, bmpSrc.Height) });
                gp.Transform(mRotate);
                System.Drawing.PointF[] pts = gp.PathPoints;

                // create destination bitmap sized to contain rotated source image
                Bitmap bmpDest = new Bitmap(bmpSrc.Width, bmpSrc.Height);

                using (Graphics gDest = Graphics.FromImage(bmpDest))
                {  // draw source into dest
                    Matrix mDest = new Matrix();
                    mDest.Translate(bmpDest.Width / 2, bmpDest.Height / 2, MatrixOrder.Append);
                    gDest.Transform = mDest;
                    gDest.DrawImage(bmpSrc, pts);
                    return bmpDest;
                }
            }
        }

        /// <summary> Поворот изображения на произвольный угол </summary>
        public static Bitmap RotateImage(Bitmap img, float angle)
        {
            var rotatedImage = new Bitmap(img.Width, img.Height, img.PixelFormat);
            using (var g = Graphics.FromImage(rotatedImage))
            {
                g.Clear(Color.White);                
                g.TranslateTransform(img.Width * 0.5f, img.Height * 0.5f); //set the rotation point as the center into the matrix
                g.RotateTransform(angle); //rotate
                g.TranslateTransform(-img.Width * 0.5f, -img.Height * 0.5f); //restore rotation point into the matrix
                g.DrawImage(img, new Point(0, 0)); //draw the image on the new bitmap
            }

            return rotatedImage;
        }

        public static Image ResizeImage(Image imgToResize, Size size)
        {
            var sourceWidth = imgToResize.Width;
            var sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = (size.Width / (float)sourceWidth);
            nPercentH = (size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            var destWidth = (int)Math.Round(sourceWidth * nPercent);
            var destHeight = (int)Math.Round(sourceHeight * nPercent);

            var b = new Bitmap(destWidth, destHeight);
            var g = Graphics.FromImage(b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return b;
        }
    }
}
