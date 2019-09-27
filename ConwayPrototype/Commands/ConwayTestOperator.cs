using System;
using ConwayPrototype.Core.Extensions;
using ConwayPrototype.UI.Conduits;
using ConwayPrototype.UI.Views;
using Eto.Forms;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Input;
using Rhino.UI;
using Command = Rhino.Commands.Command;

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

            // extract attributes from picked mesh
            var attributes = objRef.Object().Attributes;

            // delete picked mesh
            doc.Objects.Delete(objRef, true);

            // redraw
            doc.Views.Redraw();

            // Test for runmode and start dialog
            OperateOnMeshDialog dialog = null;
            if (mode == RunMode.Interactive)
            {
                dialog = new OperateOnMeshDialog(mesh);
                dialog.RestorePosition();
                var dialog_rc = dialog.ShowSemiModal(doc, RhinoEtoApp.MainWindow);
                dialog.SavePosition();
                if (dialog_rc == DialogResult.Ok)
                {
                    rc = Result.Success;
                }
            }

            else
            {
                RhinoApp.WriteLine($"Scriptable version of {EnglishName} not implemented.");
                doc.Objects.AddMesh(mesh, attributes);
                return rc;
            }

            doc.Objects.AddMesh(dialog.OperationResult, attributes);
            doc.Views.Redraw();

            return rc;
        }
    }
}