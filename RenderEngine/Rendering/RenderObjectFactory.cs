using System;
using RenderEngine.GraphicObjects;
using RenderEngine.GraphicObjects.Deformable;
using RenderEngine.GraphicObjects.Perpetual;

namespace RenderEngine.Rendering
{
    class RenderObjectFactory
    {
        internal static RenderObject CreateRenderObject(ObjectType type, Vertex[] vertices = null, int[] indices = null)
        {
            switch (type)
            {
                case ObjectType.Background: return new Background();
                case ObjectType.RenderMesh: return new RenderMesh(vertices, indices);
                default: throw new ArgumentException("Object type is not supported");
            }
        }
    }
}
