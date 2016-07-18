using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using RH.HeadEditor.Helpers;

namespace RH.HeadEditor.Data
{
    public class RenderMesh
    {
        public delegate void BeforePartDrawHandler(RenderMeshPart part);
        public event BeforePartDrawHandler OnBeforePartDraw;
        public List<BlendingInfo> BlendingInfos = new List<BlendingInfo>();

        public RenderMeshParts Parts
        {
            get;
            set;
        }
        public RectangleAABB AABB = new RectangleAABB();
        public Vector2 Scale = Vector2.One;
        public Vector2 Center = Vector2.Zero;
        private static float MORPH_SCALE = 10.0f;
        private static float MORPH_SCALE_MAX = 0.7f;
        private static float MORPH_SCALE_MIN = 0.15f;
        public float MorphScale = 5.0f;

        public float RealScale
        {
            get
            {
                return MORPH_SCALE_MIN + (MORPH_SCALE_MAX - MORPH_SCALE_MIN) * MorphScale / MORPH_SCALE;
            }
        }

        public RenderMesh()
        {
            Parts = new RenderMeshParts();
        }

        private bool morphing = false;

        public void BeginMorph()
        {
            if (morphing)
                return;
            foreach (var part in Parts)
                part.BeginMorph();
            morphing = true;
        }

        public void EndMorph()
        {
            if (!morphing)
                return;
            foreach (var part in Parts)
                part.EndMorph();
            morphing = false;
        }

        public void DoMorph(float k)
        {
            if (!morphing)
                return;
            foreach (var part in Parts)
                part.DoMorph(k);
        }

        public void ScaleWidth(float k, float centerX)
        {
            foreach (var part in Parts)
            {
                for (var i = 0; i < part.Vertices.Length; i++)
                {
                    var vertex = part.Vertices[i];
                    vertex.Position.X = centerX + (vertex.Position.X - centerX) * k;
                    vertex.OriginalPosition.X = centerX + (vertex.OriginalPosition.X - centerX) * k;
                    part.Vertices[i] = vertex;
                }
                foreach (var p in part.Points)
                {
                    var pos = p.Position;
                    pos.X = centerX + (pos.X - centerX) * k;
                    p.Position = pos;
                }
            }
        }

        public float SetSize(float diagonal)
        {
            Vector2 a = new Vector2(99999.0f, 99999.0f), b = new Vector2(-99999.0f, -99999.0f);
            foreach (var part in Parts)
                foreach (var vertex in part.Vertices)
                {
                    a.X = Math.Min(vertex.Position.X, a.X);
                    a.Y = Math.Min(vertex.Position.Y, a.Y);
                    b.X = Math.Max(vertex.Position.X, b.X);
                    b.Y = Math.Max(vertex.Position.Y, b.Y);
                }
            var d = (b - a).Length;
            if (d == 0.0f)
                return 0.0f;
            var scale = diagonal / d;
            foreach (var part in Parts)
            {
                for (var i = 0; i < part.Vertices.Length; i++)
                {
                    var vertex = part.Vertices[i];
                    vertex.OriginalPosition = vertex.Position;
                    vertex.Position.X *= scale;
                    vertex.Position.Y *= scale;
                    vertex.Position.Z *= scale;
                    part.Vertices[i] = vertex;
                }
                foreach (var p in part.Points)
                {
                    var pos = p.Position;
                    pos.X *= scale;
                    pos.Y *= scale;
                    pos.Z *= scale;
                    p.Position = pos;
                }
            }
            return 1.0f / scale;
        }

        public float Transform(float k, RectangleAABB aabb)
        {
            if (Parts.Count == 0)
                return 0.0f;
            AABB = aabb;
            var newWidht = aabb.Height * k;
            var scaleX = newWidht / aabb.Width;
            //Scale.X = scaleX;
            //Scale.X = 1.0f;//(float)Math.Sqrt(Math.Abs(k * AABB.Size.Y / AABB.Size.X));
            //Scale.Y = 1.0f / Scale.X;
            var centerX = (aabb.B.X + aabb.A.X) * 0.5f;
            //var count = 0.0f;
            //foreach (var part in Parts)
            //    foreach (var v in part.Vertices)
            //    {
            //        count += 1.0f;
            //        center += v.Position;
            //    }
            //center /= count;
            //Center = center.Xy;

            foreach (var part in Parts)
            {
                for (var i = 0; i < part.Vertices.Length; i++)
                {
                    var vertex = part.Vertices[i];
                    vertex.OriginalPosition = vertex.Position;
                    vertex.Position.X -= centerX;
                    vertex.Position.X *= scaleX;
                    //vertex.Position.Y *= Scale.Y;
                    vertex.Position.X += centerX;
                    part.Vertices[i] = vertex;    
                }
                foreach (var p in part.Points)
                {
                    var pos = p.Position;
                    pos.X -= centerX;
                    pos.X *= scaleX;
                    //pos.Y *= Scale.Y;
                    pos.X += centerX;
                    p.Position = pos;
                }
            }

            AABB.A = new Vector3((AABB.A.X - centerX) * scaleX + centerX, AABB.A.Y, AABB.A.Z);
            AABB.B = new Vector3((AABB.B.X - centerX) * scaleX + centerX, AABB.B.Y, AABB.B.Z);
            return scaleX;
        }

        public void SetAABB(Vector2 leye, Vector2 reye, Vector2 lip, Vector2 face)
        {
            BlendingInfos.Clear();
            var radius = Math.Abs(leye.X - reye.X) * 0.5f;
            BlendingInfos.Add(new BlendingInfo
            {
                Position = leye,
                Radius = radius
            });
            BlendingInfos.Add(new BlendingInfo
            {
                Position = reye,
                Radius = radius
            });
            BlendingInfos.Add(new BlendingInfo
            {
                Position = AABB.Center.Xy,
                Radius = (AABB.Center.Xy - AABB.B.Xy).Length
            });
            BlendingInfos.Add(new BlendingInfo
            {
                Position = lip,
                Radius = (AABB.Center.Xy - lip).Length
            });
            BlendingInfos.Add(new BlendingInfo
            {
                Position = face,
                Radius = (AABB.B.Xy - face).Length * 0.6f
            });
            foreach (var part in Parts)
                part.FillBlendingData(BlendingInfos);
        }

        public void UpdateAABB(RenderMeshPart part, ref Vector3 a, ref Vector3 b)
        {
            foreach (var vertex in part.Vertices)
            {
                a.Z = Math.Min(vertex.Position.Z, a.Z);
                b.Z = Math.Max(vertex.Position.Z, b.Z);
            }
        }

        public void AddPart(RenderMeshPart part)
        {
            Parts.Add(part);
            var a = AABB.A;
            var b = AABB.B;
            UpdateAABB(part, ref a, ref b);
            AABB.A = a;
            AABB.B = b;
        }

        public void FindFixedPoints()
        {
            var verticesDictionary = new Dictionary<Vector3, int>(new VectorEqualityComparer());
            var points = new List<List<Point3d>>();
            var edgesDictionary = new Dictionary<Line, int>(new VectorEqualityComparer());
            var triangle = new int[3];
            foreach (var part in Parts)
            {
                part.FindFixedPoints();
                var isEyelash = part.Name.ToLower().Contains("eyelash");
                for (var i = 0; i < part.Indices.Count; i += 3)
                {
                    for (var j = 0; j < 3; ++j)
                    {
                        var point = part.Points[part.Vertices[(int)part.Indices[i + j]].PointIndex];
                        if (isEyelash)
                            point.IsFixed = false;
                        int index;
                        if (!verticesDictionary.TryGetValue(point.Position, out index))
                        {
                            index = points.Count;
                            points.Add(new List<Point3d>());
                            verticesDictionary.Add(point.Position, index);
                        }
                        points[index].Add(point);
                        triangle[j] = index;
                    }
                    for (int j = 0, l = 2; j < 3; l = j, ++j)
                    {
                        var edge = new Line(triangle[j], triangle[l]);
                        if (!edgesDictionary.ContainsKey(edge))
                            edgesDictionary.Add(edge, 1);
                        else
                            edgesDictionary[edge]++;
                    }
                }
            }
            foreach (var edge in edgesDictionary.Where(e => e.Value == 1))
            {
                foreach (var point in points[edge.Key.A])
                    if(!point.IsFixed.HasValue)
                        point.IsFixed = true;
                foreach (var point in points[edge.Key.B])
                    if (!point.IsFixed.HasValue)
                        point.IsFixed = true;
            }
        }

        public void Draw(bool debug)
        {
            GL.Color3(1.0f, 1.0f, 1.0f);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.NormalArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);
            GL.EnableClientState(ArrayCap.ColorArray);

            foreach (var part in Parts)
            {
                if (OnBeforePartDraw != null)
                    OnBeforePartDraw(part);

                GL.BindBuffer(BufferTarget.ArrayBuffer, part.VertexBuffer);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, part.IndexBuffer);

                GL.VertexPointer(3, VertexPointerType.Float, Vertex3d.Stride, new IntPtr(0));
                GL.NormalPointer(NormalPointerType.Float, Vertex3d.Stride, new IntPtr(Vector3.SizeInBytes));
                // GL.TexCoordPointer(2, TexCoordPointerType.Float, Vertex3d.Stride, new IntPtr(2 * Vector3.SizeInBytes + Vector2.SizeInBytes + Vector4.SizeInBytes));
                GL.TexCoordPointer(2, TexCoordPointerType.Float, Vertex3d.Stride, new IntPtr(2 * Vector3.SizeInBytes));
                GL.ColorPointer(4, ColorPointerType.Float, Vertex3d.Stride, new IntPtr(2 * Vector3.SizeInBytes + Vector2.SizeInBytes));

                GL.DrawRangeElements(PrimitiveType.Triangles, 0, part.CountIndices, part.CountIndices, DrawElementsType.UnsignedInt, new IntPtr(0));
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.NormalArray);
            GL.DisableClientState(ArrayCap.TextureCoordArray);
            GL.DisableClientState(ArrayCap.ColorArray);

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.DepthTest);

            if (debug)
                OpenGlHelper.DrawAABB(AABB.A, AABB.B);
        }

        public void DrawToTexture(IEnumerable<RenderMeshPart> parts)
        {
            GL.Color3(1.0f, 1.0f, 1.0f);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.NormalArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);

            foreach (var part in parts)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, part.VertexBuffer);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, part.IndexBuffer);

                GL.VertexPointer(2, VertexPointerType.Float, Vertex3d.Stride, new IntPtr(2 * Vector3.SizeInBytes)); //Как позицию используем основные текстурные координаты
                GL.NormalPointer(NormalPointerType.Float, Vertex3d.Stride, new IntPtr(0));//Как нормаль используем позиции (координата Z потребуется для вычисления смешивания 
                GL.TexCoordPointer(3, TexCoordPointerType.Float, Vertex3d.Stride, new IntPtr(2 * Vector3.SizeInBytes + Vector2.SizeInBytes + Vector4.SizeInBytes));//Как текстурные координаты берем дополнительные текстурные координаты

                GL.DrawRangeElements(PrimitiveType.Triangles, 0, part.CountIndices, part.CountIndices, DrawElementsType.UnsignedInt, new IntPtr(0));
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.NormalArray);
            GL.DisableClientState(ArrayCap.TextureCoordArray);
        }

        public void Destroy()
        {
            foreach (var part in Parts)
            {
                part.Destroy();
            }
        }

        public void Save(string path)
        {
            using (var bw = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
            {
                AABB.ToStream(bw);
                Scale.ToStream(bw);
                Center.ToStream(bw);
                bw.Write(MorphScale);

                bw.Write(Parts.Count);
                foreach (var part in Parts)
                    part.ToStream(bw);
            }
        }

        public void Load(string path)
        {
            if (!File.Exists(path))
                return;

            Parts.Clear();
            using (var br = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                AABB = RectangleAABB.FromStream(br);
                Scale = Vector2Ex.FromStream(br);
                Center = Vector2Ex.FromStream(br);
                MorphScale = br.ReadSingle();

                var cnt = br.ReadInt32();
                for (var i = 0; i < cnt; i++)
                    Parts.Add(RenderMeshPart.FromStream(br));
            }
        }
    }
}
