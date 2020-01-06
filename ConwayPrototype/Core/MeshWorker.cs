using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConwayPrototype.Core.Extensions;
using Plankton;
using Rhino.Geometry;

namespace ConwayPrototype.Core
{
    public static class MeshWorker
    {
        public static IEnumerable<Point3d> GetFaceVertices(PlanktonMesh pMesh, int faceIndex)
        {
            return (from index in pMesh.Faces.GetFaceVertices(faceIndex) select pMesh.Vertices[index].ToPoint3d()).ToArray();;
        }

        public static IEnumerable<Line> GetFaceLines(PlanktonMesh pMesh, int faceIndex)
        {
            var edges = pMesh.Halfedges.GetFaceCirculator(pMesh.Faces[faceIndex].FirstHalfedge).ToArray();
            var lines = new Line[edges.Length];

            for (int i = 0; i < edges.Length; i++)
            {
                var from = pMesh.Vertices[pMesh.Halfedges[edges[i]].StartVertex].ToPoint3d();
                var to = pMesh.Vertices[pMesh.Halfedges.EndVertex(edges[i])].ToPoint3d();

                lines[i] = new Line(from, to);
            }

            return lines;
        }

        public static int[] AddVertices(PlanktonMesh pMesh, IEnumerable<Point3d> positions)
        {
            List<int> indices = new List<int>();

            foreach (var position in positions)
            {
                indices.Add(pMesh.Vertices.Add(position.X, position.Y, position.Z));
            }

            return indices.ToArray();
        }
    }
}
