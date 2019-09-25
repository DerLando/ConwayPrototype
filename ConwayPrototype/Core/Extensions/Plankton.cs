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
    }
}
