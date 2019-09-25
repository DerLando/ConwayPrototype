using Plankton;
using PlanktonGh;
using Rhino.Geometry;

namespace ConwayPrototype.Core.Extensions
{
    /// <summary>
    /// DualOperation replaces all Faces with a Vertex at the face center
    /// and all Vertices with a Face using the Vertex connectivity
    /// </summary>
    public static class DualOperation
    {
        /// <summary>
        /// Computes dual of Rhino Mesh via native Plankton.Dual() function
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        public static Mesh Dual(this Mesh mesh)
        {
            return mesh.ToPlanktonMesh().Dual().ToRhinoMeshWithNgons();
        }

        /// <summary>
        /// Same as `Dual`, but returns a PlanktonMesh, useful for chaining operators
        /// </summary>
        /// <param name="pMesh"></param>
        /// <returns></returns>
        public static PlanktonMesh Dual(this PlanktonMesh pMesh)
        {
            return pMesh.Dual();
        }
    }
}
