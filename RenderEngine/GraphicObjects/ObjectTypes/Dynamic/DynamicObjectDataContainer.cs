using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderEngine.Lighting;
using Shared.Assets;
using Shared.Geometry;

namespace RenderEngine.GraphicObjects.ObjectTypes.Dynamic
{
    class DynamicObjectDataContainer
    {
        internal Vertex[] Vertices { get; set; }
        internal Material Material { get; set; } = new Material(MaterialType.Silver);
        internal  bool HasNormals { get; set; }
        internal LightBundle LightBundle { get; set; } = new LightBundle();
    }
}
