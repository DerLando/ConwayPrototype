using System.Linq;
using Plankton;
using Rhino.Geometry;

namespace ConwayPrototype.Core.Extensions
{
    public static class LoftOperation
    {
        public static Mesh Loft(this Mesh mesh)
        {
            return mesh.ToPlanktonMeshWithNgons().Loft().ToRhinoMeshWithNgons();
        }

        /// <summary>
        /// Loft operation:
        /// This insets each face by 1/4 of its diagonal and adds a center face
        /// </summary>
        /// <param name="pMesh"></param>
        /// <returns></returns>
        public static PlanktonMesh Loft(this PlanktonMesh pMesh)
        {
            var lMesh = new PlanktonMesh();

            // iterate over faces
            for (int i = 0; i < pMesh.Faces.Count; i++)
            {
                // get face center
                var center = pMesh.Faces.GetFaceCenter(i).ToPoint3d();

                // get vertices for face
                var vIndices = pMesh.Faces.GetFaceVertices(i);
                var vertices = (from index in vIndices select pMesh.Vertices[index].ToPoint3d()).ToArray();

                // get face vertex count
                int vertexCount = vIndices.Length;

                // calculate mid-Point between each face vertex and face center
                Point3d[] midPoints = new Point3d[vertexCount];
                for (int j = 0; j < vertexCount; j++)
                {
                    midPoints[j] = new Line(center, vertices[j]).PointAt(0.5);
                }

                // Add original face vertices to new mesh
                for (int j = 0; j < vertexCount; j++)
                {
                    lMesh.Vertices.Add(vertices[j]);
                }

                // Add new face vertices to mesh
                for (int j = 0; j < vertexCount; j++)
                {
                    lMesh.Vertices.Add(midPoints[j]);
                }

                // Add new inset faces
                int allVerticesCount = lMesh.Vertices.Count;
                int startIndex = allVerticesCount - vertexCount * 2;
                for (int j = 0; j < vertexCount; j++)
                {
                    int a = j + startIndex;
                    int b = (j + 1) % vertexCount + startIndex;
                    int c = (j + 1) % vertexCount + startIndex + vertexCount;
                    int d = j + startIndex + vertexCount;
                    lMesh.Faces.AddFace(a, b, c, d);
                }

                // Add center face of inset
                lMesh.Faces.AddFace(Enumerable.Range(startIndex + vertexCount, vertexCount));
            }

            // combine identical vertices,
            // this is necessary because we added vertices by face
            // ugly hack :(
            lMesh = lMesh.ToRhinoMeshWithNgons().ToPlanktonMeshWithNgons();

            return lMesh;
        }
    }
}
