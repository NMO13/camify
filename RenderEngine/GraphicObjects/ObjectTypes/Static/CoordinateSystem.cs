using System;
using OpenTK.Graphics.OpenGL;
using Shared.Geometry;

namespace RenderEngine.GraphicObjects.ObjectTypes.Static
{
    internal sealed class CoordinateSystem : StaticRenderObject
    {
        public override Vertex[] LoadVertices()
        {
            throw new NotImplementedException();
        }
   
        protected override Shader Shader { get; set; }
        protected override BufferUsageHint BufferUsage { get; }
        internal override Vertex[] Vertices { get; set; }
        internal override bool HasNormals { get; set; }

        public override void Render(bool wireframe)
        {
            throw new NotImplementedException();
        }
    }
}
