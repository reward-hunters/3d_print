using System;
using System.Drawing;
using System.Drawing.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Bitmap = System.Drawing.Bitmap;
using Brushes = System.Drawing.Brushes;
using Font = System.Drawing.Font;
using Rectangle = System.Drawing.Rectangle;

namespace RH.HeadShop.Render.Helpers
{
    public class TextRenderHelper
    {
        private Font FontValue;
        private string LabelValue;
        private bool NeedToCalculateSize, NeedToRenderTexture;
        private Texture Texture;
        private int CalculatedWidth, CalculatedHeight;

        public Font Font
        {
            get
            {
                return FontValue;
            }

            set
            {
                FontValue = value;
                NeedToCalculateSize = true;
                NeedToRenderTexture = true;
            }
        }

        public string Label
        {
            get
            {
                return LabelValue;
            }

            set
            {
                if (value != LabelValue)
                {
                    LabelValue = value;
                    NeedToCalculateSize = true;
                    NeedToRenderTexture = true;
                }
            }
        }

        public int Width
        {
            get
            {
                if (NeedToCalculateSize)
                {
                    CalculateSize();
                }
                return CalculatedWidth;
            }
        }

        public int Height
        {
            get
            {
                if (NeedToCalculateSize)
                {
                    CalculateSize();
                }
                return CalculatedHeight;
            }
        }

        public Color4 Color = Color4.Black;

        public TextRenderHelper(Font font)
        {
            Font = font;
        }

        public TextRenderHelper(Font font, Color4 color)
        {
            Font = font;
            Color = color;
        }

        public TextRenderHelper(Font font, string label)
        {
            Font = font;
            Label = label;
        }

        public TextRenderHelper(Font font, Color4 color, string label)
        {
            Font = font;
            Color = color;
            Label = label;
        }

        private void CalculateSize()
        {
            using (var bitmap = new Bitmap(1, 1))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    var measures = graphics.MeasureString(Label, Font);
                    CalculatedWidth = (int)Math.Ceiling(measures.Width);
                    CalculatedHeight = (int)Math.Ceiling(measures.Height);
                }
            }
            NeedToCalculateSize = false;
        }

        public float Scale = 1.0f;
        public Vector2 Position = Vector2.Zero;

        public void Render()
        {
            if (string.IsNullOrEmpty(Label))
            {
                return;
            }

            if (NeedToRenderTexture)
            {
                using (var bitmap = new Bitmap(Width, Height))
                {
                    var rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.Clear(System.Drawing.Color.Transparent);
                        graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                        graphics.DrawString(Label, Font, Brushes.White, rectangle);

                        if (null != Texture)
                        {
                            Texture.Dispose();
                        }

                        Texture = new Texture(bitmap);
                    }
                }
                NeedToRenderTexture = false;
            }

            Texture.Bind();

            GL.Disable(EnableCap.Texture2D);
            GL.Color4(Color4.White);

            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(0, 0);
            GL.Vertex2(Width * Scale, 0);
            GL.Vertex2(Width * Scale, Height * Scale);
            GL.Vertex2(0, Height * Scale);
            GL.End();

            GL.Enable(EnableCap.Texture2D);
            GL.Color4(Color);

            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord2(0, (float)Texture.Height / Texture.PotHeight);
            GL.Vertex2(0, 0);

            GL.TexCoord2((float)Texture.Width / Texture.PotWidth, (float)Texture.Height / Texture.PotHeight);
            GL.Vertex2(Width * Scale, 0);

            GL.TexCoord2((float)Texture.Width / Texture.PotWidth, 0); 
            GL.Vertex2(Width * Scale, Height * Scale);

            GL.TexCoord2(0, 0); 
            GL.Vertex2(0, Height * Scale);

            GL.End();
        }
    }
}
