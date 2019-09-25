using Plankton;
using PlanktonGh;
using Rhino.Geometry;

namespace ConwayPrototype.Core.Extensions
{
    public static class ExpandOperation
    {
        public static Mesh Expand(this Mesh mesh)
        {
            return mesh.ToPlanktonMesh().Expand().ToRhinoMesh();
        }

        public static PlanktonMesh Expand(this PlanktonMesh pMesh)
        {
            return pMesh.Ambo().Ambo();
        }
    }
}
