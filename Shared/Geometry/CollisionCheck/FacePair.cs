using System.Diagnostics;
using Shared.Geometry.HalfedgeMesh;

namespace Shared.Geometry.CollisionCheck
{
    public class FacePair
    {
        public FacePair(HeFace a, HeFace b)
        {
            Debug.Assert(a != null && b != null);
            A = a;
            B = b;
        }
        public HeFace A;
        public HeFace B;
    }
}
