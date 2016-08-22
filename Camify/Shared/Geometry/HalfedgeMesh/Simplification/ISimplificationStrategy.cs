using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEngine.HalfedgeMesh.Simplification
{
    interface ISimplificationStrategy
    { 
        void Apply();
    }
}
