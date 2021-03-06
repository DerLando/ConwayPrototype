﻿using System;
using System.Collections.Generic;
using System.Linq;
using Plankton;
using PlanktonGh;
using Rhino.Geometry;

namespace ConwayPrototype.Core.Extensions
{
    public static class Plankton
    {
        /// <summary>
        /// Computes point at an half-edge for a given parameter t
        /// </summary>
        /// <param name="pMesh">PlanktonMesh to compute half-edge-point for</param>
        /// <param name="edgeIndex">Index of half-edge to compute for</param>
        /// <param name="t">Parameter along half-edge (0 is start of edge, 1 is end)</param>
        /// <returns></returns>
        public static Point3d PointAtEdge(this PlanktonMesh pMesh, int edgeIndex, double t)
        {
            // get start and end vertices defining half-edge
            var vertices = (from vertexIndex in pMesh.Halfedges.GetVertices(edgeIndex) select pMesh.Vertices[vertexIndex]).ToArray();

            // construct vector from start to end
            var dir = vertices[1].ToPoint3d() - vertices[0].ToPoint3d();

            // apply factor to dir
            dir *= t;

            // translate start point and return it
            return new Point3d(vertices[0].ToPoint3d() + dir);
        }

        public static Mesh ToRhinoMeshWithNgons(this PlanktonMesh source)
        {
            // could add different options for triangulating ngons later
            Mesh rMesh = new Mesh();
            foreach (PlanktonVertex v in source.Vertices)
            {
                rMesh.Vertices.Add(v.X, v.Y, v.Z);
            }
            for (int i = 0; i < source.Faces.Count; i++)
            {
                int[] fvs = source.Faces.GetFaceVertices(i);
                if (fvs.Length == 3)
                {
                    rMesh.Faces.AddFace(fvs[0], fvs[1], fvs[2]);
                }
                else if (fvs.Length == 4)
                {
                    rMesh.Faces.AddFace(fvs[0], fvs[1], fvs[2], fvs[3]);
                }
                else if (fvs.Length > 4)
                {
                    // triangulate about face center (fan)
                    var fc = source.Faces.GetFaceCenter(i);
                    rMesh.Vertices.Add(fc.X, fc.Y, fc.Z);

                    var faceIndices = new int[fvs.Length];

                    for (int j = 0; j < fvs.Length; j++)
                    {
                        faceIndices[j] = rMesh.Faces.AddFace(fvs[j], fvs[(j + 1) % fvs.Length], rMesh.Vertices.Count - 1);
                    }

                    rMesh.Ngons.AddNgon(MeshNgon.Create(fvs, faceIndices));
                }
            }
            rMesh.Normals.ComputeNormals();
            return rMesh;
        }

        public static PlanktonMesh ToPlanktonMeshWithNgons(this Mesh rMesh)
        {
            var pMesh = new PlanktonMesh();
            HashSet<int> topoVertexIndices = new HashSet<int>();

            // clean up input mesh
            rMesh.Vertices.CombineIdentical(true, true);
            rMesh.Vertices.CullUnused();
            rMesh.UnifyNormals();
            rMesh.Weld(Math.PI);

            foreach (var v in rMesh.TopologyVertices)
            {
                pMesh.Vertices.Add(v.X, v.Y, v.Z);
            }

            foreach (var meshNgon in rMesh.GetNgonAndFacesEnumerable())
            {
                pMesh.Faces.AddFace(from index in meshNgon.BoundaryVertexIndexList() select Convert.ToInt32(index));
            }

            return pMesh;
        }

        public static PlanktonMesh CombineIdenticalVertices(this PlanktonMesh pMesh)
        {
            // ugly hack for now
            // TODO: Replace this with a real method
            return pMesh.ToRhinoMeshWithNgons().ToPlanktonMeshWithNgons();
        }

        public static Point3d ToPoint3d(this PlanktonXYZ pPt)
        {
            return new Point3d(pPt.X, pPt.Y, pPt.Z);
        }

        public static Point3d ToPoint3d(this PlanktonVertex pV)
        {
            return new Point3d(pV.ToPoint3f());
        }

        public static int Add(this PlanktonVertexList vList, Point3d pt)
        {
            return vList.Add(pt.X, pt.Y, pt.Z);
        }
    }
}
