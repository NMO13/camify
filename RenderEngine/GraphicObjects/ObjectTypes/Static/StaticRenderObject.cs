using OpenTK.Graphics.OpenGL;
using RenderEngine.Resources.Shader;
using Shared.Geometry;

namespace RenderEngine.GraphicObjects.ObjectTypes.Static
{
    abstract class StaticRenderObject : RenderObject
    {
        protected override BufferUsageHint BufferUsage { get; } = BufferUsageHint.StaticDraw;
        protected override Shader Shader { get; set; }
        internal override Vertex[] Vertices { get; set; }
        internal override bool HasNormals { get; set; } = true;
    }
}
