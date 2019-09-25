using Plankton;
using PlanktonGh;
using Rhino.Geometry;

namespace ConwayPrototype.Core.Extensions
{
    public static class NeedleOperation
    {
        public static Mesh Needle(this Mesh mesh)
        {
            return mesh.ToPlanktonMeshWithNgons().Needle().ToRhinoMeshWithNgons();
        }

        public static PlanktonMesh Needle(this PlanktonMesh pMesh)
        {
            return pMesh.Dual().Kis();
        }
    }
}
