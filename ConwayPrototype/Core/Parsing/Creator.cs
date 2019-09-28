using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConwayPrototype.Core.Geometry;
using ConwayPrototype.Core.Geometry.PlatonicSolids;
using ConwayPrototype.Core.Geometry.RegularTilings;
using Rhino.Geometry;

namespace ConwayPrototype.Core.Parsing
{
    public static class Creator
    {
        public static Mesh Create(Seed s)
        {
            var mesh = new Mesh();

            switch (s)
            {
                case Seed.Dodecahedron:
                    break;
                case Seed.Cube:
                    mesh = new Cube();
                    break;
                case Seed.Icosahedron:
                    mesh = new Icosahedron();
                    break;
                case Seed.Octahedron:
                    mesh = new Octahedron();
                    break;
                case Seed.Tetrahedron:
                    mesh = new Tetrahedron();
                    break;
                case Seed.SquareTiling:
                    mesh = new Square();
                    break;
                case Seed.TriangleTiling:
                    break;
                case Seed.HexagonalTiling:
                    break;
            }

            return mesh;
        }
    }
}
