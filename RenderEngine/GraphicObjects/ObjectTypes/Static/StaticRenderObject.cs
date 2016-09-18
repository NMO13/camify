using RenderEngine.GraphicObjects.Dynamic;
using RenderEngine.GraphicObjects.ObjectTypes;
using Shared.Geometry;

namespace RenderEngine.GraphicObjects
{
    abstract class StaticRenderObject : RenderObject, IVertexLoading
    { 
        public abstract Vertex[] LoadVertices();
    }
}
