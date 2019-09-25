using System;
using ConwayPrototype.Core.Extensions;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Input;

namespace ConwayPrototype.Commands
{
    public class ConwayTestToPlanktonMesh : Command
    {
        static ConwayTestToPlanktonMesh _instance;
        public ConwayTestToPlanktonMesh()
        {
            _instance = this;
        }

        ///<summary>The only instance of the ConwayTestToPlanktonMesh command.</summary>
        public static ConwayTestToPlanktonMesh Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "ConwayTestToPlanktonMesh"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // get mesh
            var rc = RhinoGet.GetOneObject("Pick mesh", false, ObjectType.Mesh, out var objRef);
            if (rc != Result.Success) return rc;

            var mesh = objRef.Mesh();

            var converted = mesh.ToPlanktonMeshWithNgons().ToRhinoMeshWithNgons();

            doc.Objects.AddMesh(converted);

            doc.Views.Redraw();

            return Result.Success;
        }
    }
}