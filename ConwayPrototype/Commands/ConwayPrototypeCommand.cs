using System.Runtime.InteropServices;
using ConwayPrototype.Core.Extensions;
using ConwayPrototype.Core.Geometry.PlatonicSolids;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;

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
            int transFactor = 3;
            int index = 0;
            Mesh seed = new Mesh();

            var rc = RhinoGet.GetInteger("Select seed value between 1 and 5", false, ref index, 1, 5);
            if (rc != Result.Success) return rc;

            switch (index)
            {
                case 1:
                    seed = new Octahedron();
                    break;
                case 2:
                    RhinoApp.WriteLine($"{index} not implemented!");
                    return Result.Failure;
                case 3:
                    seed = new Tetrahedron();
                    break;
                case 4:
                    seed = new Cube();
                    break;
                case 5:
                    RhinoApp.WriteLine($"{index} not implemented!");
                    return Result.Failure;
            }

            doc.Objects.AddMesh(seed);

            var dual = seed.Dual();
            dual.Translate(new Vector3d(transFactor, 0, 0));
            doc.Objects.AddMesh(dual);

            var kis = seed.Kis();
            kis.Translate(new Vector3d(transFactor * 2, 0, 0));
            doc.Objects.AddMesh(kis);

            var ambo = seed.Ambo();
            ambo.Translate(new Vector3d(transFactor * 3, 0, 0));
            doc.Objects.AddMesh(ambo);

            var zip = seed.Zip();
            zip.Translate(new Vector3d(transFactor * 0, transFactor * 1, 0));
            doc.Objects.AddMesh(zip);

            var join = seed.Join();
            join.Translate(new Vector3d(transFactor * 1, transFactor * 1, 0));
            doc.Objects.AddMesh(join);

            var needle = seed.Needle();
            needle.Translate(new Vector3d(transFactor * 2, transFactor * 1, 0));
            doc.Objects.AddMesh(needle);

            var truncate = seed.Truncate();
            truncate.Translate(new Vector3d(transFactor * 3, transFactor * 1, 0));
            doc.Objects.AddMesh(truncate);

            var ortho = seed.Ortho();
            ortho.Translate(new Vector3d(transFactor * 0, transFactor * 2, 0));
            doc.Objects.AddMesh(ortho);

            var expand = seed.Expand();
            expand.Translate(new Vector3d(transFactor * 1, transFactor * 2, 0));
            doc.Objects.AddMesh(expand);

            var meta = seed.Meta();
            meta.Translate(new Vector3d(transFactor * 2, transFactor * 2, 0));
            doc.Objects.AddMesh(meta);

            var bevel = seed.Bevel();
            bevel.Translate(new Vector3d(transFactor * 3, transFactor * 2, 0));
            doc.Objects.AddMesh(bevel);

            doc.Views.Redraw();

            return Result.Success;
        }
    }
}
