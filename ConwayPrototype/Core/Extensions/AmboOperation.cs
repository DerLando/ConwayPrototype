using System.Linq;
using Plankton;
using PlanktonGh;
using Rhino.Geometry;

namespace ConwayPrototype.Core.Extensions
{
    public static class AmboOperation
    {
        public static Mesh Ambo(this Mesh mesh)
        {
            return mesh.ToPlanktonMesh().Ambo().ToRhinoMeshWithNgons();
        }

        public static PlanktonMesh Ambo(this PlanktonMesh pMesh)
        {
            // create new Plankton mesh, to hold ambo vertices and faces
            PlanktonMesh apMesh = new PlanktonMesh();

            // iterate over all half-edges of original mesh
            for (int i = 0; i < pMesh.Halfedges.Count; i++)
            {
                // test if already used half-edge
                if (pMesh.Halfedges[i].StartVertex > pMesh.Halfedges.EndVertex(i)) continue;

                // get mid-point of current half-edge
                var c = pMesh.PointAtEdge(i, 0.5);

                // add it to new mesh
                apMesh.Vertices.Add(c.X, c.Y, c.Z);
            }

            // iterate over all faces of original mesh
            // to create faces corresponding to them
            for (int i = 0; i < pMesh.Faces.Count; i++)
            {
                // get half-edge indices corresponding to current face
                var heIndices = pMesh.Faces.GetHalfedges(i);

                // each half-edge index in the original mesh directly corresponds to a vertex index in the new mesh
                // so we can assign a new face by the found half-edge indices (divided by two)
                apMesh.Faces.AddFace(from index in heIndices select index / 2);
            }

            // iterate over all vertices of original mesh
            // to create faces corresponding to their connectivity
            for (int i = 0; i < pMesh.Vertices.Count; i++)
            {
                // get half-edge indices corresponding to current vertex
                var heIndices = pMesh.Vertices.GetHalfedges(i);

                // we can use the same knowledge about connectivity like with the faces
                apMesh.Faces.AddFace(from index in heIndices.Reverse() select index / 2);
            }

            return apMesh;
        }
    }
}
