using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsEngine.Geometry;
using GraphicsEngine.HalfedgeMesh;

namespace GraphicsEngine.HalfedgeMeshProcessing
{
    internal interface IPostProcess : IMeshObserver
    {
        void Execute(DeformableObject obj);
    }
}
