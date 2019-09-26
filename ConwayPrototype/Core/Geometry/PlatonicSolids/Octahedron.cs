using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace ConwayPrototype.Core.Geometry.PlatonicSolids
{
    public class Octahedron : Polyhedron
    {
        private static readonly double[,] VertexPositionLookup =
        {
            {0, -1, 0}, {-1, 0, 0}, {0, 1, 0}, {1, 0, 0},
            {0, 0, -1}, {0, 0, 1}
        };

        private static readonly int VertexCount = 6;

        public static readonly int[,] FaceIndexLookup =
        {
            {0, 4, 3}, {3, 4, 2}, {2, 4, 1}, {1, 4, 0},
            {0, 3, 5}, {3, 2, 5}, {2, 1, 5}, {1, 0, 5}
        };

        private static readonly int FaceCount = 8;

        public Octahedron() : base() { }

        public Octahedron(Plane plane, double radius) : base(plane, radius) { }

        internal override void InitializeMesh()
        {
            // Add vertices from lookup
            for (int i = 0; i < VertexCount; i++)
            {
                Vertices.Add(VertexPositionLookup[i, 0], VertexPositionLookup[i, 1], VertexPositionLookup[i, 2]);
            }

            // Add Faces from lookup
            for (int i = 0; i < FaceCount; i++)
            {
                Faces.AddFace(FaceIndexLookup[i, 0], FaceIndexLookup[i, 1], FaceIndexLookup[i, 2]);
            }

            // Compute normals
            Normals.ComputeNormals();
        }
    }
}
