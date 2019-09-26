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

        // plane of polyhedron on creation
        public Plane Plane { get; set; } = Plane.WorldXY;

        protected Polyhedron()
        {
            InitializeMesh();
        }

        protected Polyhedron(Plane plane, double radius)
        {
            Plane = plane;
            Radius = radius;

            InitializeMesh();
            ApplyParameters();
        }

        internal void ApplyParameters()
        {
            Transform(Rhino.Geometry.Transform.PlaneToPlane(Plane.WorldXY, Plane));
            Scale(Radius);
        }

        /// <summary>
        /// Abstract method to initialize a mesh from vertex and face lookup tables
        /// Override this in the derived classes, as it is called from the base constructor
        /// <see cref="ConwayPrototype.Core.Geometry.PlatonicSolids.Cube"/> For an example of implementation
        /// </summary>
        internal abstract void InitializeMesh();
    }
}
