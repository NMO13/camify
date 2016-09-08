using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Geometry.HalfedgeMesh;

namespace GraphicsEngine.HalfedgeMesh.Simplification
{
    internal class CollapseList
    {
        private Dictionary<HeFace, HeFace> Faces = new Dictionary<HeFace, HeFace>(); 
        void AddHalfedge()
        {
            
        }

        internal IEnumerable<HeFace> ToList()
        {
            return Faces.Keys;
        }
    }
}
