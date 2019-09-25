using System;
using ConwayPrototype.Core.Extensions;
using ConwayPrototype.Core.Geometry;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Input;

namespace ConwayPrototype.Commands
{
    public class ConwayTestSeed : Command
    {
        static ConwayTestSeed _instance;
        public ConwayTestSeed()
        {
            _instance = this;
        }

        ///<summary>The only instance of the ConwayTestSeed command.</summary>
        public static ConwayTestSeed Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "ConwayTestSeed"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            
            // get mesh
            var rc = RhinoGet.GetOneObject("Pick mesh", false, ObjectType.Mesh, out var objRef);
            if (rc != Result.Success) return rc;

            var mesh = objRef.Mesh();
            double transFactor = mesh.GetBoundingBox(true).Diagonal.Length;

            var dual = mesh.Dual();
            dual.Translate(new Vector3d(transFactor, 0, 0));
            doc.Objects.AddMesh(dual);

            var kis = mesh.Kis();
            kis.Translate(new Vector3d(transFactor * 2, 0, 0));
            doc.Objects.AddMesh(kis);

            var ambo = mesh.Ambo();
            ambo.Translate(new Vector3d(transFactor * 3, 0, 0));
            doc.Objects.AddMesh(ambo);

            var zip = mesh.Zip();
            zip.Translate(new Vector3d(transFactor * 0, transFactor * 1, 0));
            doc.Objects.AddMesh(zip);

            var join = mesh.Join();
            join.Translate(new Vector3d(transFactor * 1, transFactor * 1, 0));
            doc.Objects.AddMesh(join);

            var needle = mesh.Needle();
            needle.Translate(new Vector3d(transFactor * 2, transFactor * 1, 0));
            doc.Objects.AddMesh(needle);

            var truncate = mesh.Truncate();
            truncate.Translate(new Vector3d(transFactor * 3, transFactor * 1, 0));
            doc.Objects.AddMesh(truncate);

            var ortho = mesh.Ortho();
            ortho.Translate(new Vector3d(transFactor * 0, transFactor * 2, 0));
            doc.Objects.AddMesh(ortho);

            var expand = mesh.Expand();
            expand.Translate(new Vector3d(transFactor * 1, transFactor * 2, 0));
            doc.Objects.AddMesh(expand);

            var meta = mesh.Meta();
            meta.Translate(new Vector3d(transFactor * 2, transFactor * 2, 0));
            doc.Objects.AddMesh(meta);

            var bevel = mesh.Bevel();
            bevel.Translate(new Vector3d(transFactor * 3, transFactor * 2, 0));
            doc.Objects.AddMesh(bevel);

            doc.Views.Redraw();

            return Result.Success;
        }
    }
}