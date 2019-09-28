using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace ConwayPrototype.Core.Geometry.RegularTilings
{
    public class Square : TilingBase
    {
        internal override void InitializeMesh()
        {
            CopyFrom(Mesh.CreateFromPlane(Plane, Interval, Interval, TileCount, TileCount));
        }
    }
}
