using System;
using ConwayPrototype.Core.Extensions;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Input;

namespace ConwayPrototype.Commands
{
    public class ConwayTestOperator : Command
    {
        static ConwayTestOperator _instance;
        public ConwayTestOperator()
        {
            _instance = this;
        }

        ///<summary>The only instance of the ConwayTestOperator command.</summary>
        public static ConwayTestOperator Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "ConwayTestOperator"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // get mesh
            var rc = RhinoGet.GetOneObject("Pick mesh", false, ObjectType.Mesh, out var objRef);
            if (rc != Result.Success) return rc;

            var mesh = objRef.Mesh();

            var converted = mesh.Loft();

            doc.Objects.AddMesh(converted);

            doc.Views.Redraw();

            return Result.Success;
        }
    }
}