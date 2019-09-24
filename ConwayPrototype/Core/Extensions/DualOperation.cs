using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace ConwayPrototype.Core.Extensions
{
    public static class DualOperation
    {
        public static Mesh Dual(this Mesh mesh)
        {
            // construct new replacement mesh
            Mesh replacement = new Mesh();

            // get face centers
            var centers = from index in Enumerable.Range(0, mesh.Faces.Count) select mesh.Faces.GetFaceCenter(index);

            // face centers are new vertices
            replacement.Vertices.AddVertices(centers);

            // get vertex connectivity
            //TODO: Connectivity needs better ordering
            var connectivity = from index in Enumerable.Range(0, mesh.Vertices.Count)
                select mesh.Vertices.GetConnectedVertices(index);

            // Add faces from connectivity
            replacement.Faces.AddFaces(from c in connectivity select c.ToMeshFace());

            // Compute normals
            replacement.Normals.ComputeNormals();

            // Compact
            replacement.Compact();

            // return replacement
            return replacement;
        }
    }
}
