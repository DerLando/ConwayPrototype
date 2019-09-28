using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Display;
using Rhino.Geometry;

namespace ConwayPrototype.UI.Conduits
{
    public class DrawPreviewMeshConduit : DisplayConduit
    {
        private Mesh _wireFrameMesh;
        private Mesh _mesh;
        private readonly Color _wireFrameColor;
        private readonly Color _color;
        private readonly DisplayMaterial _material;
        private readonly BoundingBox _bbox;

        public DrawPreviewMeshConduit(Mesh mesh, Mesh wireFrameMesh)
        {
            _mesh = mesh;
            _wireFrameMesh = wireFrameMesh;
            _wireFrameColor = Color.LightYellow;
            _color = Color.Red;
            _material = new DisplayMaterial(_color);
            if (_mesh != null && _mesh.IsValid)
            {
                _bbox = _mesh.GetBoundingBox(true);
            }
        }

        public void SetDisplayMesh(Mesh mesh)
        {
            _mesh = mesh;
        }

        public void SetWireframeMesh(Mesh mesh)
        {
            _wireFrameMesh = mesh;
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

            e.Display.DrawMeshWires(_wireFrameMesh, _wireFrameColor);

            if (vp.DisplayMode.EnglishName.ToLower() == "wireframe")
            {
                e.Display.DrawMeshWires(_mesh, _color);
            }
            else
            {
                e.Display.DrawMeshShaded(_mesh, _material);
                e.Display.DrawMeshWires(_mesh, _wireFrameColor);
            }

        }
    }
}
