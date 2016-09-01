using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using RenderEngine.Rendering;

namespace RenderEngine.GraphicObjects.Perpetual
{
    sealed class CoordinateAxis : RenderObject
    {
        protected override Shader Shader { get; set; }
        protected override BufferUsageHint BufferUsage { get; }
        internal override Vertex[] Vertices { get; set; }
        internal override int[] Indices { get; set; }
        internal override bool HasNormals { get; } = true;

        public override void Render()
        {
            throw new NotImplementedException();
        }
    }
}
