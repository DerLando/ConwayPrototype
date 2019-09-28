using System;
using ConwayPrototype.UI.Views;
using Eto.Forms;
using Rhino;
using Rhino.Commands;
using Rhino.UI;
using Command = Rhino.Commands.Command;

namespace ConwayPrototype.Commands
{
    public class ConwayTestCreate : Command
    {
        static ConwayTestCreate _instance;
        public ConwayTestCreate()
        {
            _instance = this;
        }

        ///<summary>The only instance of the ConwayTestCreate command.</summary>
        public static ConwayTestCreate Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "ConwayTestCreate"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            Result rc = Result.Cancel;

            // Test for runmode and start dialog
            CreateFromSeedDialog dialog = null;
            if (mode == RunMode.Interactive)
            {
                dialog = new CreateFromSeedDialog();
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
                return rc;
            }

            doc.Objects.AddMesh(dialog.OperationResult);
            doc.Views.Redraw();

            return rc;
        }
    }
}