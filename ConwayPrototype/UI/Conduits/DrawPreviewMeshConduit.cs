using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConwayPrototype.Core.Extensions;
using Rhino.Display;
using Rhino.Geometry;

namespace ConwayPrototype.UI.Conduits
{
    public class DrawPreviewMeshConduit : DisplayConduit
    {
        private Mesh _wireFrameMesh;
        private Mesh _mesh;
        private Polyline[] _previewWireFrameMesh;
        private readonly Color _wireFrameColor;
        private readonly Color _color;
        private readonly DisplayMaterial _material;
        private readonly BoundingBox _bbox;
        private bool _shouldDrawVertexColors;

        public DrawPreviewMeshConduit(Mesh mesh, Mesh wireFrameMesh)
        {
            _mesh = mesh.ColorPolyhedron();
            _wireFrameMesh = wireFrameMesh;
            _previewWireFrameMesh = mesh.ToWireFrame();
            _wireFrameColor = Color.LightYellow;
            _color = Color.Red;
            _material = new DisplayMaterial(_color);
            _shouldDrawVertexColors = false;
            if (_mesh != null && _mesh.IsValid)
            {
                _bbox = _mesh.GetBoundingBox(true);
            }
        }

        public void SetDisplayMesh(Mesh mesh)
        {
            _previewWireFrameMesh = mesh.ToWireFrame();
            _mesh = mesh.ColorPolyhedron();
        }

        public void SetWireframeMesh(Mesh mesh)
        {
            _wireFrameMesh = mesh;
        }

        public void SetDrawMode(bool value)
        {
            _shouldDrawVertexColors = value;
        }

        protected override void CalculateBoundingBox(CalculateBoundingBoxEventArgs e)
        {
            base.CalculateBoundingBox(e);
            e.IncludeBoundingBox(_bbox);
        }

        protected override void PreDrawObject(DrawObjectEventArgs e)
        {
            base.PreDrawObject(e);
            var vp = e.Display.Viewport;

            // draw wireframe of old (input) mesh
            e.Display.DrawMeshWires(_wireFrameMesh, _wireFrameColor);

            // draw wireframe of new mes
            for (int i = 0; i < _previewWireFrameMesh.Length; i++)
            {
                e.Display.DrawPolyline(_previewWireFrameMesh[i], _wireFrameColor);
            }

            if (vp.DisplayMode.EnglishName.ToLower() == "wireframe")
            {
                e.Display.DrawMeshWires(_mesh, _color);
            }
            else
            {
                if(_shouldDrawVertexColors) e.Display.DrawMeshFalseColors(_mesh);
                else e.Display.DrawMeshShaded(_mesh, _material);

                //e.Display.DrawMeshWires(_mesh, _wireFrameColor);
            }

        }
    }
}
