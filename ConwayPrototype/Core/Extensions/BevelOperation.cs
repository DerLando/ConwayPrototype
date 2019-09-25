using Plankton;
using PlanktonGh;
using Rhino.Geometry;

namespace ConwayPrototype.Core.Extensions
{
    public static class BevelOperation
    {
        public static Mesh Bevel(this Mesh mesh)
        {
            return mesh.ToPlanktonMesh().Bevel().ToRhinoMesh();
        }

        public static PlanktonMesh Bevel(this PlanktonMesh pMesh)
        {
            return pMesh.Meta().Dual();
        }
    }
}
