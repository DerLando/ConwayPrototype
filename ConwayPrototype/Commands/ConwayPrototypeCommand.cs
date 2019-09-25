using System;
using System.Collections.Generic;
using ConwayPrototype.Core.Extensions;
using ConwayPrototype.Core.Geometry;
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
            RhinoApp.WriteLine("The {0} command is under construction.", EnglishName);

            var xTrans = Transform.Translation(new Vector3d(3, 0, 0));

            var cube = new Cube(1);
            doc.Objects.AddMesh(cube);

            var dual = cube.Dual();
            dual.Transform(xTrans);
            doc.Objects.AddMesh(dual);

            var kis = cube.Kis();
            kis.Transform(xTrans * xTrans);
            doc.Objects.AddMesh(kis);

            var ambo = cube.Ambo();
            ambo.Transform(xTrans * xTrans * xTrans);
            doc.Objects.AddMesh(ambo);

            return Result.Success;
        }
    }
}
