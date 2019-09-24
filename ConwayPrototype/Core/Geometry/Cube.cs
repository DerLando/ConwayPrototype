using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayPrototype.Core.Geometry
{
    public class Cube : Polyhedron
    {
        private static readonly double[,] VertexPositionLookup =
            {{-1, -1, -1}, {1, -1, -1}, {1, 1, -1}, {-1, 1, -1},
                {-1, -1, 1}, {1, -1, 1}, {1, 1, 1}, {-1, 1, 1}};

        private static readonly int VertexCount = 8;

        public static readonly int[,] FaceIndexLookup =
            {{0, 1, 2, 3}, {1, 5, 6, 2}, {5, 4, 7, 6}, {4, 0, 3, 7}, {0, 1, 5, 4}, {3, 2, 6, 7}};

        private static readonly int FaceCount = 6;

        public Cube(double radius)
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

            // Compact
            Compact();
        }
    }
}
