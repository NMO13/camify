using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using RenderEngine.BufferObjectManagement;
using RenderEngine.ErrorHandling;
using RenderEngine.Rendering;
using RenderEngine.Resources.Shader;
using Shared.Geometry;

namespace RenderEngine.GraphicObjects.ObjectTypes
{
    abstract class RenderObject : IRenderable
    {
        protected abstract Shader Shader { get; set; }
        protected abstract BufferUsageHint BufferUsage { get; }
        internal abstract Vertex[] Vertices { get; set; }
        internal abstract bool HasNormals { get; set; }

        private readonly BufferObjectContainer _bufferObject = new BufferObjectContainer();
        public abstract void Render(bool wireframe);

        protected void Setup()
        {
            if (Vertices == null)
                throw new ArgumentNullException("Vertices must not be null.");

            //Bind vao, vbo and ebo
            GLCheck.Call(() => GL.BindVertexArray(_bufferObject.Vao));

            GL.BindBuffer(BufferTarget.ArrayBuffer, _bufferObject.Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Vertices.Length * Marshal.SizeOf(typeof(Vertex))), Vertices, BufferUsage);

            // Vertex Positions
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Double, false, Marshal.SizeOf(typeof(Vertex)), IntPtr.Zero);
            // Is Contour Edge
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 1, VertexAttribPointerType.Float, false, Marshal.SizeOf(typeof(Vertex)), Vertex.IsContourEdgeOffset());
            // Vertex Normals
            if (HasNormals)
            {
                GL.EnableVertexAttribArray(2);
                GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Double, false, Marshal.SizeOf(typeof(Vertex)), Vertex.NormalOffset());
            }

            //Unlink buffers
            GLCheck.Call(() => GL.BindBuffer(BufferTarget.ArrayBuffer, 0));
            GLCheck.Call(() => GL.BindVertexArray(0));
            GLCheck.Call(() => GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0));
        }
        protected void DrawMesh(PrimitiveType primitiveType = PrimitiveType.Triangles)
        {
            GL.BindVertexArray(_bufferObject.Vao);
            GLCheck.Call(() => GL.DrawArrays(primitiveType, 0, Vertices.Length));
            GL.BindVertexArray(0);
        }
    }
}
