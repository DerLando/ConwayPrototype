using Rhino.Geometry;

namespace ConwayPrototype.Core.Geometry
{
    /// <summary>
    /// abstract base class for platonic solids
    /// </summary>
    public abstract class Polyhedron : Mesh
    {
        // radius of polyhedron on creation
        public double Radius { get; set; } = 1;
    }
}
