using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConwayPrototype.Core.Parsing;
using ConwayPrototype.UI.Conduits;
using Eto.Drawing;
using Eto.Forms;
using Rhino;
using Rhino.Display;
using Rhino.Geometry;
using Rhino.UI.Controls;

namespace ConwayPrototype.UI.Views
{
    public class OperateOnMeshDialog : Dialog<DialogResult>
    {

        // mesh
        private Mesh _mesh;
        private Operator _operator;
        private DrawPreviewMeshConduit _conduit;
        public Mesh OperationResult { get; private set; }

        // controls
        private Button btn_OK =  new Button{Text = "OK"};
        private Button btn_Preview = new Button{Text = "Preview"};
        private TextBox tB_OperationInput = new TextBox();
        private Label lbl_OperationInput = new Label{Text = "Command", VerticalAlignment = VerticalAlignment.Center};
        private Label lbl_AvailableOperators = new Label{Text = $"Currently Available Operators:\n {Tokenizer.PossibleTokens}"};

        public OperateOnMeshDialog(Mesh mesh)
        {
            // initialize fields
            _mesh = mesh;
            _operator = new Operator(mesh);
            _conduit = new DrawPreviewMeshConduit(_operator.GetMesh(), _mesh);
            _conduit.Enabled = true;

            RhinoDoc.ActiveDoc.Views.Redraw();

            // Initial settings
            Padding = new Padding(5);
            Resizable = false;
            Result = DialogResult.Cancel;
            Title = "Conway";
            WindowStyle = WindowStyle.Default;

            // initialize event handlers
            btn_OK.Click += ON_btn_OK_Clicked;
            btn_Preview.Click += On_btn_Preview_Clicked;
            tB_OperationInput.TextChanged += tB_OperationInput_TextChanged;

            // initialize layout
            var layout = new DynamicLayout();

            layout.Add(lbl_AvailableOperators);
            layout.AddSeparateRow(new Control[] {lbl_OperationInput, tB_OperationInput});
            layout.Add(btn_OK);

            Content = layout;
        }

        private void tB_OperationInput_TextChanged(object sender, EventArgs e)
        {
            _operator = new Operator(_mesh);
            _operator.Operate(Tokenizer.Tokenize(tB_OperationInput.Text));
            _conduit.Enabled = false;
            _conduit.SetDisplayMesh(_operator.GetMesh());
            _conduit.Enabled = true;

            RhinoDoc.ActiveDoc.Views.Redraw();
        }

        private void On_btn_Preview_Clicked(object sender, EventArgs e)
        {
            _operator = new Operator(_mesh);
            _operator.Operate(Tokenizer.Tokenize(tB_OperationInput.Text));
            _conduit.Enabled = false;
            _conduit.SetDisplayMesh(_operator.GetMesh());
            _conduit.Enabled = true;

            RhinoDoc.ActiveDoc.Views.Redraw();
        }

        private void ON_btn_OK_Clicked(object sender, EventArgs e)
        {
            OperationResult = _operator.GetMesh();
            Close(DialogResult.Ok);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _conduit.Enabled = false;

            // save initial mesh if nothing has been done
            if(OperationResult is null) OperationResult = _mesh;

            base.OnClosing(e);
        }
    }
}
