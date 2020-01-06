using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plankton;
using Rhino.Geometry;

namespace ConwayPrototype.Core.Extensions
{
    public static class PropellerOperation
    {
        public static Mesh Propeller(this Mesh mesh)
        {
            return mesh.ToPlanktonMeshWithNgons().Propeller().ToRhinoMeshWithNgons();
        }

        public static PlanktonMesh Propeller(this PlanktonMesh pMesh)
        {
            // face based operation
            // every face gets divided in 5
            // one large twisted face in the middle
            // and 4 smaller reconnecting the outer boundaries

            // new mesh to hold propeller result
            var propMesh = new PlanktonMesh();

            // iterate over faces
            for (int i = 0; i < pMesh.Faces.Count; i++)
            {
                var lines = MeshWorker.GetFaceLines(pMesh, i);
                var oldVertices = (from line in lines select line.From).ToArray();

                // calculate new vertices
                var newVertices = (from line in lines select line.PointAt(0.5));

                // add center face to new mesh
                var newIndices = MeshWorker.AddVertices(propMesh, newVertices);
                propMesh.Faces.AddFace(newIndices);

                // add outer faces to new mesh
                var outerIndices = MeshWorker.AddVertices(propMesh, oldVertices);

                for (int j = 0; j < newIndices.Length; j++)
                {
                    var a = outerIndices[j];
                    var b = newIndices[j];
                    var c = newIndices[(newIndices.Length - 1 + j) % newIndices.Length];

                    propMesh.Faces.AddFace(a, b, c);
                }

            }

            return propMesh;
        }
    }
}
