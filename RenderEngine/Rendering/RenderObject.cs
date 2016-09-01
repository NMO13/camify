using System;
using System.Diagnostics;
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
        internal abstract Vertex[] Vertices { get; set; }
        internal abstract int[] Indices { get; set; }

        internal abstract bool HasNormals { get; }

        protected BufferObjectComposite bufferObject = new BufferObjectComposite();
        public abstract void Render();

        internal RenderObject()
        {
            Setup(Vertices, Indices, HasNormals);
        }

        internal RenderObject(Vertex[] vertices, int[] indices)
        {
            Setup(vertices, indices, HasNormals);
        }

        protected void Setup(Vertex[] vertices, int[] indices, bool hasNormals)
        {
            if(vertices == null)
                throw new ArgumentNullException("Vertices must not be null.");
            if(indices == null)
                throw new ArgumentNullException("Indices must not be null.");

            bufferObject.InitializeBuffers();
            //Bind vao, vbo and ebo
            GL.BindVertexArray(bufferObject.Vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferObject.Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Marshal.SizeOf(typeof(Vertex))), vertices, BufferUsage);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, bufferObject.Ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Length * sizeof(int)), indices, BufferUsage);

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

        protected void DrawMesh(PrimitiveType primitiveType)
        {
            GL.BindVertexArray(bufferObject.Vao);
            GLCheck.Call(() => GL.DrawElements(primitiveType, Indices.Length, DrawElementsType.UnsignedInt, 0));
            GL.BindVertexArray(0);
        }
    }
}
