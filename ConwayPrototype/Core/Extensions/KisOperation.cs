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
    /// <summary>
    /// Kis raises a pyramid on each face
    /// </summary>
    public static class KisOperation
    {
        public static Mesh Kis(this Mesh mesh, int n=0)
        {
            return mesh.ToPlanktonMesh().Kis(n).ToRhinoMesh();
        }

        /// <summary>
        /// compute kis of PlanktonMesh using the native Plankton function Faces.Stellate()
        /// </summary>
        /// <param name="pMesh"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static PlanktonMesh Kis(this PlanktonMesh pMesh, int n=0)
        {
            for (var i = pMesh.Faces.Count - 1; i >= 0; i--)
            {
                if (pMesh.Faces.GetFaceVertices(i).Length == n || n == 0)
                {
                    pMesh.Faces.Stellate(i);
                }
            }
            return pMesh;
        }
    }
}
