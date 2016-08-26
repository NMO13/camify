using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsEngine.HalfedgeMesh;

namespace GraphicsEngine.Geometry.CollisionCheck
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
