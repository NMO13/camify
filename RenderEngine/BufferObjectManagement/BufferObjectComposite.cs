using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using Shared.Geometry;

namespace RenderEngine.BufferObjectManagement
{
    class BufferObjectComposite
    {
        internal int Vbo { get; set; }
        internal int Ebo { get; set; }
        internal int Vao { get; set; }
        internal void InitializeBuffers()
        {
            Vao = GL.GenVertexArray();
            Vbo = GL.GenBuffer();
            Ebo = GL.GenBuffer();
        }
    }
}
