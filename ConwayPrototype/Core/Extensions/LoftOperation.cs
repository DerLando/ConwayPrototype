using Plankton;
using Rhino.Geometry;

namespace ConwayPrototype.Core.Extensions
{
    public static class LoftOperation
    {
        public static Mesh Loft(this Mesh mesh)
        {
            return mesh.ToPlanktonMeshWithNgons().Loft().ToRhinoMeshWithNgons();
        }

        public static PlanktonMesh Loft(this PlanktonMesh pMesh)
        {
            var lMesh = new PlanktonMesh();

            // iterate over faces
            for (int i = 0; i < pMesh.Faces.Count; i++)
            {
                // get face center
                var center = pMesh.Faces.GetFaceCenter(i).ToPoint3d();

                // calculate mid-Point between each face vertex and face center
                var vIndices = pMesh.Faces.GetFaceVertices(i);
                foreach (var index in vIndices)
                {
                    var v = pMesh.Vertices[index].ToPoint3d();
                }
            }
        }
    }
}
