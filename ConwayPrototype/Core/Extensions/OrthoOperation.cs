using Plankton;
using PlanktonGh;
using Rhino.Geometry;

namespace ConwayPrototype.Core.Extensions
{
    public static class OrthoOperation
    {
        public static Mesh Ortho(this Mesh mesh)
        {
            return mesh.ToPlanktonMesh().Ortho().ToRhinoMeshWithNgons();
        }

        public static PlanktonMesh Ortho(this PlanktonMesh pMesh)
        {
            return pMesh.Expand().Dual();
        }
    }
}
