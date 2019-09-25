using System.Collections.Generic;
using System.Linq;
using Plankton;
using PlanktonGh;
using Rhino.Geometry;

namespace ConwayPrototype.Core.Extensions
{
    public static class Plankton
    {
        /// <summary>
        /// Computes point at an half-edge for a given parameter t
        /// </summary>
        /// <param name="pMesh">PlanktonMesh to compute half-edge-point for</param>
        /// <param name="edgeIndex">Index of half-edge to compute for</param>
        /// <param name="t">Parameter along half-edge (0 is start of edge, 1 is end)</param>
        /// <returns></returns>
        public static Point3d PointAtEdge(this PlanktonMesh pMesh, int edgeIndex, double t)
        {
            // get start and end vertices defining half-edge
            var vertices = (from vertexIndex in pMesh.Halfedges.GetVertices(edgeIndex) select pMesh.Vertices[vertexIndex]).ToArray();

            // construct vector from start to end
            var dir = vertices[1].ToPoint3d() - vertices[0].ToPoint3d();

            // apply factor to dir
            dir *= t;

            // translate start point and return it
            return new Point3d(vertices[0].ToPoint3d() + dir);
        }

        public static Mesh ToRhinoMeshWithNgons(this PlanktonMesh source)
        {
            // could add different options for triangulating ngons later
            Mesh rMesh = new Mesh();
            foreach (PlanktonVertex v in source.Vertices)
            {
                rMesh.Vertices.Add(v.X, v.Y, v.Z);
            }
            for (int i = 0; i < source.Faces.Count; i++)
            {
                int[] fvs = source.Faces.GetFaceVertices(i);
                if (fvs.Length == 3)
                {
                    rMesh.Faces.AddFace(fvs[0], fvs[1], fvs[2]);
                }
                else if (fvs.Length == 4)
                {
                    rMesh.Faces.AddFace(fvs[0], fvs[1], fvs[2], fvs[3]);
                }
                else if (fvs.Length > 4)
                {
                    // triangulate about face center (fan)
                    var fc = source.Faces.GetFaceCenter(i);
                    rMesh.Vertices.Add(fc.X, fc.Y, fc.Z);

                    var faceIndices = new int[fvs.Length];

                    for (int j = 0; j < fvs.Length; j++)
                    {
                        faceIndices[j] = rMesh.Faces.AddFace(fvs[j], fvs[(j + 1) % fvs.Length], rMesh.Vertices.Count - 1);
                    }

                    rMesh.Ngons.AddNgon(MeshNgon.Create(fvs, faceIndices));
                }
            }
            rMesh.Normals.ComputeNormals();
            return rMesh;
        }
    }
}
