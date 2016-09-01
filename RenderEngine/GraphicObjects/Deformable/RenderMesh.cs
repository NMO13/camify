using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using RenderEngine.Rendering;
using RenderEngine.Resources.Shader;
using RenderEngine.Scene;
using Shared.Geometry;

namespace RenderEngine.GraphicObjects.Deformable
{
    sealed class RenderMesh : RenderObject
    {
        protected override Shader Shader { get; set; }
        protected override BufferUsageHint BufferUsage => BufferUsageHint.DynamicDraw;
        internal override Vertex[] Vertices { get; set; }
        internal override int[] Indices { get; set; }
        internal override bool HasNormals { get; } = false;

        internal RenderMesh(Vertex[] vertices, int[] indices) : base(vertices, indices)
        {
            Indices = indices;
            Shader = ResourceManager.Instance.GetShader(ShaderLibrary.ShaderName.Mesh.ToString());
        }

        public override void Render()
        {
            GL.Enable(EnableCap.DepthTest);
            Shader.Use();

            Shader.SetMatrixMatrix4("view", SceneModel.Instance.WorldTransformationMatrix);
            Shader.SetMatrixMatrix4("proj", SceneModel.Instance.ProjectionMatrix);

            DrawMesh(PrimitiveType.Triangles);
        }
    }
}
