using Rhino.Geometry;

namespace ConwayPrototype.Core.Geometry.PlatonicSolids
{
    public class Tetrahedron : PolyhedronBase
    {
        private static readonly double[,] VertexPositionLookup =
            {{1, 1, 1}, {1, -1, -1}, {-1, 1, -1}, {-1, -1, 1}};

        private static readonly int VertexCount = 4;

        public static readonly int[,] FaceIndexLookup =
            {{0, 1, 2}, {0, 2, 3}, {0, 3, 1}, {1, 3, 2}};

        private static readonly int FaceCount = 4;

        public Tetrahedron() : base() { }

        public Tetrahedron(Plane plane, double radius) : base(plane, radius) { }

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
