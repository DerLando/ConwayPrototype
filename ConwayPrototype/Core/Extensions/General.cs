using System;
using System.Collections.Generic;
using System.Linq;
using Rhino.Geometry;

namespace ConwayPrototype.Core.Extensions
{
    public static class General
    {
        public static MeshFace ToMeshFace(this IEnumerable<int> vertexIndices)
        {
            var iArray = vertexIndices.ToArray();

            if (iArray.Length == 3) return new MeshFace(iArray[0], iArray[1], iArray[2]);
            if (iArray.Length == 4) return new MeshFace(iArray[0], iArray[1], iArray[2], iArray[3]);

            return MeshFace.Unset;
        }

        public static double Circumference(this Mesh mesh, MeshNgon nGon)
        {
            return new Polyline(from index in nGon.BoundaryVertexIndexList() select new Point3d(mesh.Vertices[Convert.ToInt32(index)])).Length;
        }
    }
}
