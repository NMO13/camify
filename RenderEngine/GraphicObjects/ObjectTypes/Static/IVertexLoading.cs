using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Geometry;

namespace RenderEngine.GraphicObjects.Dynamic
{
    interface IVertexLoading
    {
        Vertex[] LoadVertices();
    }
}
