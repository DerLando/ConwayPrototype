using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace ConwayPrototype.Core.Geometry.RegularTilings
{
    public abstract class TilingBase : Mesh
    {
        public Plane Plane { get; set; } = Plane.WorldXY;
        public int TileCount { get; set; } = 10;
        public double SpanLength { get; set; } = 0.2;

        public Interval Interval => new Interval(-SpanLength * TileCount / 2.0, SpanLength * TileCount / 2.0);

        protected TilingBase()
        {
            InitializeMesh();
        }

        protected TilingBase(Plane plane, int tileCount, double spanLength)
        {
            Plane = plane;
            TileCount = tileCount;
            SpanLength = spanLength;

            InitializeMesh();
        }

        internal abstract void InitializeMesh();
    }
}
