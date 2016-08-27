using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Vector3d = Shared.Geometry.Vector3d;
using Matrix4d = Shared.Geometry.Matrix4d;

namespace RenderEngine.Rendering
{
    class RenderObject : IRenderable
    {
        private int[] vao = new int[1];
        private int[] vbo = new int[1];
        private int[] ebo = new int[1];
        private Vertex[] vertices;
        private int[] indices;

        public RenderObject(Vector3d[] vertices, int[] indices, Matrix4d transformations)
        {
            //this.vertices = vertices;
            this.indices = indices;
        }

        public void Setup()
        {
            GL.GenVertexArrays(1, out vao[0]);
            GL.GenBuffers(1, out vbo[0]);
            GL.GenBuffers(1, out ebo[0]);

            //Bind vao, vbo and ebo
            GL.BindVertexArray(vao[0]);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Marshal.SizeOf(typeof(Vector3d))), vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo[0]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Length * sizeof(int)), indices, BufferUsageHint.StaticDraw);

            // Vertex Positions
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf(typeof(Vector3d)), 0);
            // Vertex Normals
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf(typeof(Vector3d)), Marshal.OffsetOf(typeof(Vector3d), "normal"));

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        public void Render()
        {
            // Draw mesh
            GL.BindVertexArray(vao[0]);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }
    }
}
