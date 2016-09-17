using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using RenderEngine.BufferObjectManagement;
using RenderEngine.Lighting;
using Shared.Geometry;

namespace RenderEngine.Rendering
{
    abstract class RenderObject : IRenderable
    {
        protected abstract Shader Shader { get; set; }
        protected abstract BufferUsageHint BufferUsage { get;}
        internal abstract Vertex[] Vertices { get; set; }
        internal abstract bool HasNormals { get; set; }

        protected BufferObjectContainer bufferObject = new BufferObjectContainer();
        public abstract void Render();

        internal RenderObject()
        {
            Setup(Vertices, HasNormals);
        }

        internal RenderObject(Vertex[] vertices, bool hasNormals)
        {
            Setup(vertices, hasNormals);
        }

        protected void Setup(Vertex[] vertices, bool hasNormals)
        {
            if(vertices == null)
                throw new ArgumentNullException("Vertices must not be null.");

            bufferObject.InitializeBuffers();
            //Bind vao, vbo and ebo
            GL.BindVertexArray(bufferObject.Vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferObject.Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Marshal.SizeOf(typeof(Vertex))), vertices, BufferUsage);

            // Vertex Positions
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Double, false, Marshal.SizeOf(typeof(Vertex)), IntPtr.Zero);
            // Is Contour Edge
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 1, VertexAttribPointerType.Float, false, Marshal.SizeOf(typeof(Vertex)), Vertex.IsContourEdgeOffset());
            // Vertex Normals
            if (hasNormals)
            {
                GL.EnableVertexAttribArray(2);
                GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Double, false, Marshal.SizeOf(typeof(Vertex)), Vertex.NormalOffset());
            }

            //Unlink buffers
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        protected void DrawMesh(PrimitiveType primitiveType = PrimitiveType.Triangles)
        {
            GL.BindVertexArray(bufferObject.Vao);
            GLCheck.Call(() => GL.DrawArrays(primitiveType, 0, Vertices.Length));
            GL.BindVertexArray(0);
        }

        //TODO alter modelmatrix rather than changing vertices
        internal void Translate(Shared.Geometry.Vector3d transformation)
        {
            var dX = transformation.X;
            var dY = transformation.Y;
            var dZ = transformation.Z;

            for(int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].X += dX;
                Vertices[i].Y += dY;
                Vertices[i].Z += dZ;
            }
            Setup(Vertices, true);
        }
    }
}
