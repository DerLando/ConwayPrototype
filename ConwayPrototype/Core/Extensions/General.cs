using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using Eto.Forms;
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

        public static IEnumerable<Point3d> GetFaceVertices(this Mesh mesh, MeshFace face)
        {
            if (face.IsTriangle)
            {
                Point3d[] vertices = new Point3d[3];
                vertices[0] = mesh.Vertices[face.A];
                vertices[1] = mesh.Vertices[face.B];
                vertices[2] = mesh.Vertices[face.C];

                return vertices;
            }
            else
            {
                Point3d[] vertices = new Point3d[4];
                vertices[0] = mesh.Vertices[face.A];
                vertices[1] = mesh.Vertices[face.B];
                vertices[2] = mesh.Vertices[face.C];
                vertices[3] = mesh.Vertices[face.D];

                return vertices;
            }
        }

        /// <summary>
        /// Closes a given polyline by adding the first point
        /// of the polyline to the end
        /// </summary>
        /// <param name="pLine"></param>
        /// <returns></returns>
        public static bool Close(this Polyline pLine)
        {
            if(!pLine.IsClosed) pLine.Add(pLine[0]);
            return pLine.IsClosed;
        }

        public static Polyline FaceBoundary(this Mesh mesh, MeshFace face)
        {
            var boundary = new Polyline(mesh.GetFaceVertices(face));
            boundary.Close();
            return boundary;
        }

        public static Polyline NgonBoundary(this Mesh mesh, MeshNgon nGon)
        {
            return new Polyline(from index in nGon.BoundaryVertexIndexList()
                select new Point3d(mesh.Vertices[Convert.ToInt32(index)]));
        }

        public static double Circumference(this Mesh mesh, MeshNgon nGon)
        {
            return mesh.NgonBoundary(nGon).Length;
        }

        public static Polyline[] ToWireFrame(this Mesh mesh)
        {
            // find all faces NOT in nGons
            var faces = Enumerable.Range(0, mesh.Faces.Count).ToArray();

            foreach (var meshNgon in mesh.Ngons)
            {
                foreach (var index in meshNgon.FaceIndexList())
                {
                    faces[Convert.ToInt32(index)] = -1;
                }
            }

            faces = (from index in faces where index != -1 select index).ToArray();

            // empty array for circumferences
            var wireFrame = new Polyline[mesh.Ngons.Count + faces.Length];

            for (int i = 0; i < faces.Length; i++)
            {
                wireFrame[i] = mesh.FaceBoundary(mesh.Faces[faces[i]]);
            }

            for (int i = 0; i < mesh.Ngons.Count; i++)
            {
                wireFrame[i + faces.Length] = mesh.NgonBoundary(mesh.Ngons[i]);
            }

            return wireFrame;
        }

        public static IEnumerable<Color> ColorRange(int count)
        {
            // super basic for now
            int stepSize = (int)Math.Floor(255.0 / count);

            Color[] colors = new Color[count];
            for (int i = 0; i < count; i++)
            {
                colors[i] = Color.FromArgb(0, i * stepSize, i * stepSize, i * stepSize);
            }

            return colors;
        }

        public static Mesh ColorPolyhedron(this Mesh mesh)
        {
            //TODO: There seems to be no way to keep nGon Information while coloring a mesh.

            // get ngon face indices to reconstruct from later
            var ngonIndices = from ngon in mesh.Ngons
                select (from index in ngon.FaceIndexList() select Convert.ToInt32(index));

            int nGonCount = mesh.Ngons.Count;

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

                var length = Math.Round(poly.Length, (1 / tol).ToString().Length);

                if (lengthIndicesDict.ContainsKey(length))
                {
                    lengthIndicesDict[length].Add(indices);
                }
                else
                {
                    lengthIndicesDict[length] = new List<int[]>{indices};
                }
            }

            // create color range
            var colorRange = ColorRange(lengthIndicesDict.Count).ToArray();

            // empty array to hold all vertex colors
            var vertexColors = new Color[mesh.Vertices.Count];

            // extracted keys for easier reference by index
            var keys = lengthIndicesDict.Keys.ToArray();

            // iterate over all keys
            for (int i = 0; i < lengthIndicesDict.Count; i++)
            {
                // current color by index of key
                var curColor = colorRange[i];

                // iterate over face-vertex lists in key
                foreach (var vIndexList in lengthIndicesDict[keys[i]])
                {
                    foreach (var index in vIndexList)
                    {
                        // set vertex color by index stored in face-vertex list
                        vertexColors[index] = curColor;
                    }
                }
            }

            // set all colors at once
            mesh.VertexColors.SetColors(vertexColors);

            // re-set nGons
            if (false)
            {
                List<MeshNgon> nGons = new List<MeshNgon>();
                foreach (var ngonIndex in ngonIndices)
                {
                    int[] faceIndices = ngonIndex.ToArray();
                    int[] vertexIndices = new int[faceIndices.Length * 4];

                    for (int i = 0; i < faceIndices.Length; i++)
                    {
                        vertexIndices[i * 4 + 0] = mesh.Faces[faceIndices[i]].A;
                        vertexIndices[i * 4 + 1] = mesh.Faces[faceIndices[i]].B;
                        vertexIndices[i * 4 + 2] = mesh.Faces[faceIndices[i]].C;
                        vertexIndices[i * 4 + 3] = mesh.Faces[faceIndices[i]].D;

                    }

                    nGons.Add(MeshNgon.Create(vertexIndices, faceIndices));
                }

                mesh.Ngons.AddNgons(nGons);
            }

            // return
            return mesh;
        }
    }
}
