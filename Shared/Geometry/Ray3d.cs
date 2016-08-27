using GraphicsEngine.Math;
using Shared;

namespace Shared.Geometry
{
    internal class Ray3d
    {
        internal Vector3d P0 { get; set; }
        internal Vector3d P1 { get; set; }

        internal Ray3d(Vector3d p0, Vector3d p1)
        {
            P0 = p0;
            P1 = p1;
        }

        internal Ray3d() : this(Vector3d.Zero(), Vector3d.Zero())
        {
            
        }

        public Vector3d Origin
        {
            get { return P0; }
        }

        public Vector3d Direction
        {
            get
            {
                return P1 - P0;
            }
        }
    }
}
