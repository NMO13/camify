using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Matrix4d = Shared.Matrix4d;

namespace RenderEngine.Rendering
{
    class RenderMesh
    {
        private Vector3d[] vertices;
        private uint[] indices;
        private int[] vao = new int[1];
        private int[] vbo = new int[1];
        private int[] ebo = new int[1];
        private Matrix4d transformationMatrix;

        public RenderMesh(Vector3d[] vertices, uint[] indices, Matrix4d transformations)
        {
            this.vertices = vertices;
            this.indices = indices;
            SetupMesh();
            SetupTransformations(transformations);
        }

        public void Draw()
        {
            // Draw mesh
            GL.BindVertexArray(vao[0]);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }

        public Matrix4d GetTransformationMatrix()
        {
            return transformationMatrix;
        }

        private void SetupTransformations(Matrix4d transformations)
        {
            //transformationMatrix.M11 = transformations.A1;
            //transformationMatrix.M12 = transformations.B1;
            //transformationMatrix.M13 = transformations.C1;
            //transformationMatrix.M14 = transformations.D1;

            //transformationMatrix.M21 = transformations.A2;
            //transformationMatrix.M22 = transformations.B2;
            //transformationMatrix.M23 = transformations.C2;
            //transformationMatrix.M24 = transformations.D2;

            //transformationMatrix.M31 = transformations.A3;
            //transformationMatrix.M32 = transformations.B3;
            //transformationMatrix.M33 = transformations.C3;
            //transformationMatrix.M34 = transformations.D3;

            //transformationMatrix.M41 = transformations.A4;
            //transformationMatrix.M42 = transformations.B4;
            //transformationMatrix.M43 = transformations.C4;
            //transformationMatrix.M44 = transformations.D4;
        }

        private void SetupMesh()
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
    }
}
