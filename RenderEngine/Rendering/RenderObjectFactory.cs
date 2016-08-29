using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderEngine.GraphicObjects;
using RenderEngine.Objects;

namespace RenderEngine.Rendering
{
    class RenderObjectFactory
    {
        internal static RenderObject CreateRenderObject(ObjectType type)
        {
            switch (type)
            {
                case ObjectType.Background: return new Background();
                default: throw new ArgumentException("Object type is not supported");
            }
        }
    }
}
