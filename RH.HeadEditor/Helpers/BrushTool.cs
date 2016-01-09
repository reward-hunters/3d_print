using OpenTK;
using RH.HeadEditor.Data;
using System;
using System.Collections.Generic;

namespace RH.HeadEditor.Helpers
{
    public class BrushTriangle // Треугольник
    {
        public BrushPoint[] Points = new BrushPoint[3];
        public uint[] Indices = new uint[3];
        public Vector2 Min, Max;
        public bool IsProcessed;
        public Guid PartGuid;
    }

    public class BrushPoint //Физическая точка
    {
        public RenderMeshPart Part;
        public int CheckIndex;
        public List<BrushTriangle> Triangles = new List<BrushTriangle>();
        public Vector3 Position;
        public Vector3 ViewSpacePosition;
        public bool IsProcessed;
    }

    public class BrushTool
    {
        public RenderMesh renderMesh;
        public Dictionary<Guid, List<uint>> ResultIndices { get; private set; }
        public Vector3 SphereCenter = Vector3.Zero;

        private float quadRadius;
        private Matrix4 viewMatrix;
        private BrushTriangle startTriangle = null;        
        private List<BrushTriangle> triangles = new List<BrushTriangle>();
        private List<BrushPoint> points = new List<BrushPoint>();

        public float Radius = 1.5f;
        public Vector4 Color;

        public void InitializeBrush(HeadMeshesController headMeshController)
        {
            points.Clear();
            triangles.Clear();

            var tempPoins = new Dictionary<Vector3, BrushPoint>(new VectorEqualityComparer());
            foreach (var part in headMeshController.RenderMesh.Parts)
            {
                var tempIndices = new BrushPoint[part.Vertices.Length];
                for (int i = 0; i < part.Vertices.Length; ++i)
                {
                    BrushPoint bp;
                    var vertex = part.Vertices[i];
                    if (!tempPoins.TryGetValue(vertex.OriginalPosition, out bp))
                    {
                        bp = new BrushPoint
                        {
                            Position = vertex.OriginalPosition,
                            CheckIndex = i,
                            Part = part
                        };
                        tempPoins.Add(bp.Position, bp);
                        points.Add(bp);
                    }
                    tempIndices[i] = bp;
                }

                for (int i = 0; i < part.Indices.Count; i += 3)
                {
                    var triangle = new BrushTriangle
                    {
                        PartGuid = part.Guid
                    };
                    for (int j = 0; j < 3; ++j)
                    {
                        var index = part.Indices[i + j];
                        var point = tempIndices[index];
                        triangle.Points[j] = point;
                        triangle.Indices[j] = index;
                        point.Triangles.Add(triangle);
                    }
                    triangles.Add(triangle);
                }
            }
        }

        public void StartBrush(Matrix4 vm)
        {
            foreach (var point in points)
            {
                point.Position = point.Part.Vertices[point.CheckIndex].Position;
            }
            UpdateCamera(vm);
        }

        private static void FindMinMax(ref float a, ref float b, ref float c, out float min, out float max)
        {
            if (a < b)
            {
                if(b < c)
                {
                    min = a;
                    max = c;
                }
                else if(c < a)
                {
                    min = c;
                    max = b;
                } else
                {
                    min = a;
                    max = b;
                }
            }
            else
            {
                if (a < c)
                {
                    min = b;
                    max = c;
                }
                else if (c < b)
                {
                    min = c;
                    max = a;
                } else
                {
                    min = b;
                    max = a;
                }
            }
        }

        public void UpdateCamera(Matrix4 vm)
        {
            viewMatrix = vm;
            foreach (var point in points)
            {
                point.ViewSpacePosition = Vector3.Transform(point.Position, viewMatrix);
            }
            foreach (var triangle in triangles)
            {
                var p0 = triangle.Points[0].ViewSpacePosition;
                var p1 = triangle.Points[1].ViewSpacePosition;
                var p2 = triangle.Points[2].ViewSpacePosition;
                float minx, miny, maxx, maxy;
                FindMinMax(ref p0.X, ref p1.X, ref p2.X, out minx, out maxx);
                FindMinMax(ref p0.Y, ref p1.Y, ref p2.Y, out miny, out maxy);
                triangle.Min.X = minx;
                triangle.Max.X = maxx;
                triangle.Min.Y = miny;
                triangle.Max.Y = maxy;
            }
        }

        public void DrawBrush(Vector2 point)
        {
            quadRadius = Radius * Radius;
            if (!GetStartPoint(new Vector3(point.X, point.Y, 0.0f)))
                return;
            foreach (var p in points)
                p.IsProcessed = false;
            ResultIndices = new Dictionary<Guid, List<uint>>();
            ProcessTriangle(startTriangle);
        }

        private void ProcessTriangle(BrushTriangle triangle)
        {
            triangle.IsProcessed = true;
            List<uint> indices;
            if(!ResultIndices.TryGetValue(triangle.PartGuid, out indices))
            {
                indices = new List<uint>();
                ResultIndices.Add(triangle.PartGuid, indices);
            }
            indices.AddRange(triangle.Indices);
            for(int i = 0; i<3; ++i)
            {
                var point = triangle.Points[i];
                if (point.IsProcessed || ((SphereCenter - point.Position).LengthSquared > quadRadius))
                    continue;
                point.IsProcessed = true;

                foreach (var t in point.Triangles)
                {
                    if (!t.IsProcessed)
                        ProcessTriangle(t);
                }
            }
        }

        private bool GetStartPoint(Vector3 point)
        {
            startTriangle = null;
            var depth = -99999f;
            foreach (var triangle in triangles)
            {
                triangle.IsProcessed = false;
                if (triangle.Max.X < point.X || triangle.Min.X > point.X
                    || triangle.Max.Y < point.Y || triangle.Min.Y > point.Y)
                    continue;

                var p0 = triangle.Points[0].ViewSpacePosition;
                var p1 = triangle.Points[1].ViewSpacePosition;
                var p2 = triangle.Points[2].ViewSpacePosition;

                var a = p0.Xy;
                var b = p1.Xy;
                var c = p2.Xy;

                if (TexturingInfo.PointInTriangle(ref a, ref b, ref c, ref point))
                {
                    var aup = a.X - point.X;
                    var bup = b.X - point.X;
                    var cup = c.X - point.X;
                    var avp = a.Y - point.Y;
                    var bvp = b.Y - point.Y;
                    var cvp = c.Y - point.Y;

                    var f = 1.0f / ((b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X));
                    var u = (bup * cvp - bvp * cup) * f;
                    var v = (cup * avp - cvp * aup) * f;
                    var w = 1.0f - (u + v);

                    var z = u * p0.Z + v * p1.Z + w * p2.Z;
                    if (depth < z)
                    {
                        startTriangle = triangle;
                        depth = z;
                    }
                }
            }

            SphereCenter = point;
            SphereCenter.Z = depth;
            SphereCenter = Vector3.Transform(SphereCenter, viewMatrix.Inverted());

            return startTriangle != null;
        }
    }
}
