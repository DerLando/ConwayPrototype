using System;
using System.Drawing;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Input;

namespace ConwayPrototype.Commands
{
    public class ConwayMarkMeshTopology : Command
    {
        static ConwayMarkMeshTopology _instance;
        public ConwayMarkMeshTopology()
        {
            _instance = this;
        }

        ///<summary>The only instance of the ConwayMarkMeshTopology command.</summary>
        public static ConwayMarkMeshTopology Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "ConwayMarkMeshTopology"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // get mesh
            var rc = RhinoGet.GetOneObject("Pick mesh", false, ObjectType.Mesh, out var objRef);
            if (rc != Result.Success) return rc;

            var mesh = objRef.Mesh();
            doc.Views.RedrawEnabled = false;

            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                doc.Objects.AddTextDot(i.ToString(), mesh.Vertices[i]);
            }

            var faceAttributes = new ObjectAttributes
            {
                ObjectColor = Color.Blue, ColorSource = ObjectColorSource.ColorFromObject
            };

            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                doc.Objects.AddTextDot(i.ToString(), mesh.Faces.GetFaceCenter(i), faceAttributes);
            }

            doc.Views.RedrawEnabled = true;
            doc.Views.Redraw();

            return Result.Success;
        }
    }
}