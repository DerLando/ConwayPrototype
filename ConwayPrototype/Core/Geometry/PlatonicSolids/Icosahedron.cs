using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace ConwayPrototype.Core.Geometry.PlatonicSolids
{
    public class Icosahedron : Polyhedron
    {
        private static readonly double[,] VertexPositionLookup =
        {
            {-Phi, 0, -1}, {0, -1, -Phi}, {Phi, 0, -1}, {0, 1, -Phi}, {-1, Phi, 0}, {-1, -Phi, 0},
            {0, -1, Phi}, {1, -Phi, 0}, {1, Phi, 0}, {0, 1, Phi}, {-Phi, 0, 1}, {Phi, 0, 1}
        };

        private static readonly int VertexCount = 12;

        public static readonly int[,] FaceIndexLookup =
        {
            {0, 2, 1}, {0, 3, 2}, {0, 4, 3}, {0, 5, 4}, {0, 1, 5},
            {5, 1, 6}, {1, 7, 6}, {2, 7, 1}, {2, 8, 7}, {3, 8, 2}, {3, 9, 8}, {3, 4, 9}, {4, 10, 9}, {4, 5, 10}, {5, 6, 10},
            {11, 6, 7}, {11, 7, 8}, {11, 8, 9}, {11, 9, 10}, {11, 10, 6}
        };

        private static readonly int FaceCount = 20;

        public Icosahedron() : base() { }

        public Icosahedron(Plane plane, double radius) : base(plane, radius) { }

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
