using System;
using System.Collections.Generic;
using System.Linq;
using Rhino;
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

        public static Mesh ColorPolyhedron(this Mesh mesh)
        {
            // unweld all faces
            mesh.Unweld(0, true);

            // get tolerance
            var tol = RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;

            Dictionary<double, List<int[]>> lengthIndicesDict = new Dictionary<double, List<int[]>>();

            // iterate over faces and group by area
            foreach (var nGon in mesh.GetNgonAndFacesEnumerable())
            {
                var indices = (from index in nGon.BoundaryVertexIndexList() select Convert.ToInt32(index)).ToArray();

                var vertices = from index in indices select new Point3d(mesh.Vertices[Convert.ToInt32(index)]);

                var poly = new Polyline(vertices);

                var length = Math.Round(poly.Length, (int) (1 / tol));

                if (lengthIndicesDict.ContainsKey(length))
                {
                    lengthIndicesDict[length].Add(indices);
                }
                else
                {
                    lengthIndicesDict[length] = new List<int[]>{indices};
                }
            }
        }
    }
}
