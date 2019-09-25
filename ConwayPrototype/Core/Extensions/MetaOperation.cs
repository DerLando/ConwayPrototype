using Plankton;
using PlanktonGh;
using Rhino.Geometry;

namespace ConwayPrototype.Core.Extensions
{
    public static class MetaOperation
    {
        public static Mesh Meta(this Mesh mesh)
        {
            return mesh.ToPlanktonMeshWithNgons().Meta().ToRhinoMeshWithNgons();
        }

        public static PlanktonMesh Meta(this PlanktonMesh pMesh)
        {
            return pMesh.Join().Kis();
        }
    }
}
