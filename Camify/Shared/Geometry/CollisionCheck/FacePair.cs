using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsEngine.HalfedgeMesh;

namespace GraphicsEngine.Geometry.CollisionCheck
{
    class FacePair
    {
        internal FacePair(HeFace a, HeFace b)
        {
            Debug.Assert(a != null && b != null);
            A = a;
            B = b;
        }
        internal HeFace A;
        internal HeFace B;
    }
}
