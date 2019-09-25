using Plankton;
using PlanktonGh;
using Rhino.Geometry;

namespace ConwayPrototype.Core.Extensions
{
    public static class BevelOperation
    {
        public static Mesh Bevel(this Mesh mesh)
        {
            return mesh.ToPlanktonMeshWithNgons().Bevel().ToRhinoMeshWithNgons();
        }

        public static PlanktonMesh Bevel(this PlanktonMesh pMesh)
        {
            return pMesh.Meta().Dual();
        }
    }
}
