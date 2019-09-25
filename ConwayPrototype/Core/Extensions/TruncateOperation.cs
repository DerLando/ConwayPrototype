using Plankton;
using PlanktonGh;
using Rhino.Geometry;

namespace ConwayPrototype.Core.Extensions
{
    public static class TruncateOperation
    {
        public static Mesh Truncate(this Mesh mesh)
        {
            return mesh.ToPlanktonMeshWithNgons().Truncate().ToRhinoMeshWithNgons();
        }

        public static PlanktonMesh Truncate(this PlanktonMesh pMesh)
        {
            return pMesh.Needle().Dual();
        }
    }
}
