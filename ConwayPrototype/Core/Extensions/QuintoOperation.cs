using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plankton;
using Rhino.Geometry;

namespace ConwayPrototype.Core.Extensions
{
    public static class QuintoOperation
    {
        public static Mesh Quinto(this Mesh mesh)
        {
            return mesh.ToPlanktonMeshWithNgons().Quinto().ToRhinoMeshWithNgons();
        }

        public static PlanktonMesh Quinto(this PlanktonMesh pMesh)
        {
            // new mesh to hold quinto result
            var qMesh = new PlanktonMesh();

            // iterate over faces
            for (int i = 0; i < pMesh.Faces.Count; i++)
            {
                // get face vertices
                var oldVertices =
                    (from index in pMesh.Faces.GetFaceVertices(i) select pMesh.Vertices[index].ToPoint3d()).ToArray();
                var oldVertexCount = oldVertices.Length;

                // empty array to hold all new face vertices
                var vertexCount = oldVertices.Length * 3;
                var vertices = new Point3d[vertexCount];

                // add old vertices
                for (int j = 0; j < oldVertexCount; j++)
                {
                    vertices[j] = oldVertices[j];
                }

                // get mid points of face edges
                for (int j = 0; j < oldVertexCount; j++)
                {
                    int upperIndex = (j + 1) % oldVertexCount;
                    var midPoint = new Line(oldVertices[j], oldVertices[upperIndex]).PointAt(0.5);
                    
                    // add to vertex array
                    vertices[j + oldVertexCount] = midPoint;
                }

                // get face center
                var ptCenter = pMesh.Faces.GetFaceCenter(i).ToPoint3d();

                // get mid point from edge mid-point to face center
                for (int j = 0; j < oldVertexCount; j++)
                {
                    var midPoint = new Line(vertices[j + oldVertexCount], ptCenter).PointAt(0.5);

                    // add to vertex array
                    vertices[j + 2 * oldVertexCount] = midPoint;
                }

                // Add vertices to new mesh
                for (int j = 0; j < vertexCount; j++)
                {
                    qMesh.Vertices.Add(vertices[j]);
                }

                // Add faces
                int startIndex = qMesh.Vertices.Count - vertexCount;
                for (int j = 0; j < oldVertexCount; j++)
                {
                    int a = startIndex + j;
                    int b = startIndex + j + oldVertexCount;
                    int c = startIndex + j + 2 * oldVertexCount;

                    // don't know a sexy math formula for this behaviour right now :/
                    int d;
                    int e;
                    if (j == 0)
                    {
                        d = startIndex + vertexCount - 1;
                        e = startIndex + 2 * oldVertexCount - 1;
                    }
                    else
                    {
                        d = startIndex + j + 2 * oldVertexCount - 1;
                        e = startIndex + j + oldVertexCount - 1;
                    }


                    qMesh.Faces.AddFace(new[] {a, b, c, d, e});
                }

                // Add center face of inset
                qMesh.Faces.AddFace(Enumerable.Range(startIndex + 2 * oldVertexCount, oldVertexCount));

            }

            // remove identical vertices which exist because we added by face
            qMesh = qMesh.CombineIdenticalVertices();

            // return
            return qMesh;

        }
    }
}
