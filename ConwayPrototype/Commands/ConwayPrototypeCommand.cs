using ConwayPrototype.Core.Extensions;
using ConwayPrototype.Core.Geometry;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;

namespace ConwayPrototype.Commands
{
    public class ConwayPrototypeCommand : Command
    {
        public ConwayPrototypeCommand()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static ConwayPrototypeCommand Instance
        {
            get; private set;
        }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName
        {
            get { return "ConwayPrototypeCommand"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            RhinoApp.WriteLine("The {0} command is under construction.", EnglishName);

            int transFactor = 3;

            var cube = new Cube(1);
            doc.Objects.AddMesh(cube);

            var dual = cube.Dual();
            dual.Translate(new Vector3d(transFactor, 0, 0));
            doc.Objects.AddMesh(dual);

            var kis = cube.Kis();
            kis.Translate(new Vector3d(transFactor * 2, 0, 0));
            doc.Objects.AddMesh(kis);

            var ambo = cube.Ambo();
            ambo.Translate(new Vector3d(transFactor * 3, 0, 0));
            doc.Objects.AddMesh(ambo);

            var zip = cube.Zip();
            zip.Translate(new Vector3d(transFactor * 0, transFactor * 1, 0));
            doc.Objects.AddMesh(zip);

            var join = cube.Join();
            join.Translate(new Vector3d(transFactor * 1, transFactor * 1, 0));
            doc.Objects.AddMesh(join);

            var needle = cube.Needle();
            needle.Translate(new Vector3d(transFactor * 2, transFactor * 1, 0));
            doc.Objects.AddMesh(needle);

            var truncate = cube.Truncate();
            truncate.Translate(new Vector3d(transFactor * 3, transFactor * 1, 0));
            doc.Objects.AddMesh(truncate);

            var ortho = cube.Ortho();
            ortho.Translate(new Vector3d(transFactor * 0, transFactor * 2, 0));
            doc.Objects.AddMesh(ortho);

            var expand = cube.Expand();
            expand.Translate(new Vector3d(transFactor * 1, transFactor * 2, 0));
            doc.Objects.AddMesh(expand);

            var meta = cube.Meta();
            meta.Translate(new Vector3d(transFactor * 2, transFactor * 2, 0));
            doc.Objects.AddMesh(meta);

            var bevel = cube.Bevel();
            bevel.Translate(new Vector3d(transFactor * 3, transFactor * 2, 0));
            doc.Objects.AddMesh(bevel);

            doc.Views.Redraw();

            return Result.Success;
        }
    }
}
