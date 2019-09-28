using System;
using Rhino.Geometry;

namespace ConwayPrototype.Core.Geometry.PlatonicSolids
{
    /// <summary>
    /// abstract base class for platonic solids
    /// </summary>
    public abstract class PolyhedronBase : Mesh
    {
        // radius of polyhedron on creation
        public double Radius { get; set; } = 1;

        // plane of polyhedron on creation
        public Plane Plane { get; set; } = Plane.WorldXY;

        internal static double Phi = 0.61803398874989484820458683436;

        protected PolyhedronBase()
        {
            InitializeMesh();
        }

        protected PolyhedronBase(Plane plane, double radius)
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
