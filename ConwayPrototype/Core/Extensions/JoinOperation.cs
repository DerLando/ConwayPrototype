using Plankton;
using PlanktonGh;
using Rhino.Geometry;

namespace ConwayPrototype.Core.Extensions
{
    public static class JoinOperation
    {
        public static Mesh Join(this Mesh mesh)
        {
            return mesh.ToPlanktonMesh().Join().ToRhinoMesh();
        }

        public static PlanktonMesh Join(this PlanktonMesh pMesh)
        {
            return pMesh.Ambo().Dual();
        }
    }
}
