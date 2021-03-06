﻿using Rhino.Geometry;

namespace ConwayPrototype.Core.Geometry.PlatonicSolids
{
    public class Cube : PolyhedronBase
    {
        private static readonly double[,] VertexPositionLookup =
            {{-1, -1, -1}, {-1, 1, -1}, {1, 1, -1}, {1, -1, -1},
                {-1, -1, 1}, {-1, 1, 1}, {1, 1, 1}, {1, -1, 1} };

        private static readonly int VertexCount = 8;

        public static readonly int[,] FaceIndexLookup =
            {{0, 1, 2, 3}, {3, 2, 6, 7}, {7, 6, 5, 4}, {4, 5, 1, 0}, {0, 3, 7, 4}, {1, 5, 6, 2}};

        private static readonly int FaceCount = 6;

        public Cube() : base() { }

        public Cube(Plane plane, double radius) : base(plane, radius) { }

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
                Faces.AddFace(FaceIndexLookup[i, 0], FaceIndexLookup[i, 1], FaceIndexLookup[i, 2],
                    FaceIndexLookup[i, 3]);
            }

            // Compute normals
            Normals.ComputeNormals();
        }
    }
}
