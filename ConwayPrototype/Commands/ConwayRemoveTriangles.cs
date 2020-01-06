using System;
using ConwayPrototype.Core.Extensions;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Input;

namespace ConwayPrototype.Commands
{
    public class ConwayRemoveTriangles : Command
    {
        static ConwayRemoveTriangles _instance;
        public ConwayRemoveTriangles()
        {
            _instance = this;
        }

        ///<summary>The only instance of the ConwayRemoveTriangles command.</summary>
        public static ConwayRemoveTriangles Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "ConwayRemoveTriangles"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // get mesh
            var rc = RhinoGet.GetOneObject("Pick mesh", false, ObjectType.Mesh, out var objRef);
            if (rc != Result.Success) return rc;

            var mesh = objRef.Mesh();
            doc.Views.RedrawEnabled = false;

            var pMesh = mesh.ToPlanktonMeshWithNgons();

            for (int i = pMesh.Faces.Count - 1; i >= 0; i--)
            {
                if (pMesh.Faces.GetFaceVertices(i).Length == 3)
                {
                    pMesh.Faces.RemoveFace(i);
                }
            }

            pMesh.Compact();

            mesh = pMesh.ToRhinoMeshWithNgons();
            doc.Objects.Replace(objRef, mesh);

            doc.Views.RedrawEnabled = true;
            doc.Views.Redraw();

            return Result.Success;
        }
    }
}