using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConwayPrototype.Core.Extensions;
using ConwayPrototype.Core.Geometry;
using ConwayPrototype.Core.Parsing;
using ConwayPrototype.UI.Conduits;
using Eto.Drawing;
using Eto.Forms;
using Rhino;
using Rhino.Geometry;

namespace ConwayPrototype.UI.Views
{
    public class CreateFromSeedDialog : Dialog<DialogResult>
    {
        // mesh
        private Mesh _mesh;
        private Operator _operator;
        private DrawPreviewMeshConduit _conduit;
        public Mesh OperationResult { get; private set; }

        // controls
        private Button btn_OK = new Button { Text = "OK" };
        private Button btn_Zoom = new Button {Text = "Zoom"};
        private TextBox tB_OperationInput = new TextBox();
        private Label lbl_OperationInput = new Label { Text = "Command", VerticalAlignment = VerticalAlignment.Center };
        private Label lbl_AvailableOperators = new Label { Text = $"Currently Available Operators:\n {Tokenizer.PossibleTokens}" };
        private Label lbl_Seeds = new Label{Text = "Available Seeds", VerticalAlignment = VerticalAlignment.Center};
        private ComboBox cB_Seeds = new ComboBox{DataStore = Enum.GetNames(typeof(Seed)) };
        private CheckBox cB_DrawVertexColors = new CheckBox{Checked = false};
        private Label lbl_DrawVertexColors = new Label
            {Text = "Topology View", VerticalAlignment = VerticalAlignment.Center};

        public CreateFromSeedDialog()
        {
            // initialize fields
            _mesh = new Mesh();
            _operator = new Operator(_mesh);
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
            btn_Zoom.Click += btn_Zoom_Clicked;
            tB_OperationInput.TextChanged += tB_OperationInput_TextChanged;
            cB_Seeds.SelectedIndexChanged += cB_Seeds_SelectedIndexChanged;
            cB_Seeds.SelectedIndex = 1;
            cB_DrawVertexColors.CheckedChanged += cB_DrawVertexColors_CheckedChanged;

            // initialize layout
            var layout = new DynamicLayout();

            layout.AddSeparateRow(lbl_AvailableOperators);
            layout.AddRow(new Control[] {lbl_Seeds, cB_Seeds});
            layout.AddRow(new Control[] {lbl_DrawVertexColors, cB_DrawVertexColors});
            layout.AddRow(new Control[] {lbl_OperationInput, tB_OperationInput});
            layout.AddRow(new Control[] {btn_Zoom, btn_OK});

            Content = layout;
        }

        private void cB_DrawVertexColors_CheckedChanged(object sender, EventArgs e)
        {
            bool value = cB_DrawVertexColors.Checked.Value;
            _conduit.SetDrawMode(value);

            RhinoDoc.ActiveDoc.Views.Redraw();
        }

        private void btn_Zoom_Clicked(object sender, EventArgs e)
        {
            RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.ZoomBoundingBox(_mesh.GetBoundingBox(true));
        }

        private void cB_Seeds_SelectedIndexChanged(object sender, EventArgs e)
        {
            // change internal seed mesh
            _mesh = Creator.Create((Seed) Enum.Parse(typeof(Seed), (string) cB_Seeds.SelectedValue));

            // update wireframe mesh of conduit
            _conduit.SetWireframeMesh(_mesh);

            // raise textChanged event to re-run operations
            tB_OperationInput_TextChanged(sender, e);
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

        private void ON_btn_OK_Clicked(object sender, EventArgs e)
        {
            OperationResult = _operator.GetMesh().ColorPolyhedron();
            Close(DialogResult.Ok);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _conduit.Enabled = false;

            // save initial mesh if nothing has been done
            if (OperationResult is null) OperationResult = _mesh;

            base.OnClosing(e);
        }

    }
}
