using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plankton;
using PlanktonGh;
using Rhino.Geometry;

namespace ConwayPrototype.Core.Extensions
{
    public static class DualOperation
    {
        /// <summary>
        /// Computes dual of Rhino Mesh via native Plankton.Dual() function
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        public static Mesh Dual(this Mesh mesh)
        {
            return mesh.ToPlanktonMesh().Dual().ToRhinoMesh();
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
