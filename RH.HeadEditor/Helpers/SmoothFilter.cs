using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RH.HeadEditor.Helpers
{
    public class SmoothFilter
    {
        public static Vector3[] laplacianFilter(Vector3[] sv, int[] t)
        {
            Vector3[] wv = new Vector3[sv.Length];
            List<Vector3> adjacentVertices = new List<Vector3>();

            float dx = 0.0f;
            float dy = 0.0f;
            float dz = 0.0f;

            for (int vi = 0; vi < sv.Length; vi++)
            {
                // Find the sv neighboring vertices
                adjacentVertices = MeshUtils.findAdjacentNeighbors(sv, t, sv[vi]);

                if (adjacentVertices.Count != 0)
                {
                    dx = 0.0f;
                    dy = 0.0f;
                    dz = 0.0f;

                    //Debug.Log("Vertex Index Length = "+vertexIndexes.Length);
                    // Add the vertices and divide by the number of vertices
                    for (int j = 0; j < adjacentVertices.Count; j++)
                    {
                        dx += adjacentVertices[j].X;
                        dy += adjacentVertices[j].Y;
                        dz += adjacentVertices[j].Z;
                    }

                    wv[vi].X = dx / adjacentVertices.Count;
                    wv[vi].Y = dy / adjacentVertices.Count;
                    wv[vi].Z = dz / adjacentVertices.Count;
                }
            }

            return wv;
        }

        /*
            HC (Humphrey’s Classes) Smooth Algorithm - Reduces Shrinkage of Laplacian Smoother

            Where sv - original points
                    pv - previous points,
                    alpha [0..1] influences previous points pv, e.g. 0
                    beta  [0..1] e.g. > 0.5
        */
        public static Vector3[] hcFilter(Vector3[] sv, Vector3[] pv, int[] t, float alpha, float beta)
        {
            Vector3[] wv = new Vector3[sv.Length];
            Vector3[] bv = new Vector3[sv.Length];



            // Perform Laplacian Smooth
            wv = laplacianFilter(sv, t);

            // Compute Differences
            for (int i = 0; i < wv.Length; i++)
            {
                bv[i].X = wv[i].X - (alpha * sv[i].X + (1 - alpha) * sv[i].X);
                bv[i].Y = wv[i].Y - (alpha * sv[i].Y + (1 - alpha) * sv[i].Y);
                bv[i].Z = wv[i].Z - (alpha * sv[i].Z + (1 - alpha) * sv[i].Z);
            }

            List<int> adjacentIndexes = new List<int>();

            float dx = 0.0f;
            float dy = 0.0f;
            float dz = 0.0f;

            for (int j = 0; j < bv.Length; j++)
            {
                adjacentIndexes.Clear();

                // Find the bv neighboring vertices
                adjacentIndexes = MeshUtils.findAdjacentNeighborIndexes(sv, t, sv[j]);

                dx = 0.0f;
                dy = 0.0f;
                dz = 0.0f;

                for (int k = 0; k < adjacentIndexes.Count; k++)
                {
                    dx += bv[adjacentIndexes[k]].X;
                    dy += bv[adjacentIndexes[k]].Y;
                    dz += bv[adjacentIndexes[k]].Z;

                }

                wv[j].X -= beta * bv[j].X + ((1 - beta) / adjacentIndexes.Count) * dx;
                wv[j].Y -= beta * bv[j].Y + ((1 - beta) / adjacentIndexes.Count) * dy;
                wv[j].Z -= beta * bv[j].Z + ((1 - beta) / adjacentIndexes.Count) * dz;
            }

            return wv;
        }
    }
}
