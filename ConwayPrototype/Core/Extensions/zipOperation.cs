using Plankton;
using PlanktonGh;
using Rhino.Geometry;

namespace ConwayPrototype.Core.Extensions
{
    public static class ZipOperation
    {
        public static Mesh Zip(this Mesh mesh)
        {
            return mesh.ToPlanktonMesh().Zip().ToRhinoMesh();
        }

        public static PlanktonMesh Zip(this PlanktonMesh pMesh)
        {
            return pMesh.Kis().Dual();
        }
    }
}
