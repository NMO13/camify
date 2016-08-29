using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using RenderEngine.BufferObjectManagement;

namespace RenderEngine.Rendering
{
    abstract class RenderObject : IRenderable
    {
        protected abstract Shader Shader { get; set; }
        protected abstract BufferUsageHint BufferUsage { get;}
        protected abstract Vertex[] Vertices { get; }
        protected abstract uint[] Indices { get; }

        protected BufferObjectComposite bufferObject = new BufferObjectComposite();
        public abstract void Render();

        protected void Setup(bool hasNormals)
        {
            if(Vertices == null)
                throw new ArgumentNullException("Vertices must not be null.");
            
            bufferObject.InitializeBuffers();
            //Bind vao, vbo and ebo
            GL.BindVertexArray(bufferObject.Vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferObject.Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Vertices.Length * Marshal.SizeOf(typeof(Vertex))), Vertices, BufferUsage);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, bufferObject.Ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(Indices.Length * sizeof(int)), Indices, BufferUsage);

            // Vertex Positions
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Double, false, Marshal.SizeOf(typeof(Vertex)), IntPtr.Zero);
            // Vertex Normals
            if (hasNormals)
            {
                GL.EnableVertexAttribArray(1);
                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Double, false, Marshal.SizeOf(typeof(Vertex)), Vertex.NormalOffset());
            }

            //Unlink buffers
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }
    }
}
