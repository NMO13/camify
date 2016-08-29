using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Vector3d = Shared.Geometry.Vector3d;
using Matrix4d = Shared.Geometry.Matrix4d;
using RenderEngine.Rendering;
using RenderEngine.Resources.Shader;
using Shared.Geometry;

namespace RenderEngine.Objects
{
    sealed class Background : RenderObject
    {
        private Vertex[] _sceneVertices = {
            new Vertex(-1, -1, 0),
            new Vertex(1, -1, 0),
            new Vertex(1, 1, 0),
            new Vertex(-1, 1, 0),
        };

        private uint[] _sceneIndices = {
            0, 1, 2, 3
        };

        protected override Shader Shader { get; set; }
        protected override BufferUsageHint BufferUsage => BufferUsageHint.StaticDraw;

        protected override Vertex[] Vertices => _sceneVertices;
        protected override uint[] Indices => _sceneIndices;

        //Constructor
        internal Background()
        {
            Shader = ResourceManager.Instance.GetShader(ShaderLibrary.ShaderName.Scene.ToString());
            Setup(false);
        }

        public override void Render()
        {
            GL.Viewport(0, 0, SceneModel.Instance.SceneWidth, SceneModel.Instance.SceneHeight);
            GL.Enable(EnableCap.DepthTest);
            Shader.Use();
            Shader.SetInteger("resolutionY", SceneModel.Instance.SceneHeight);

            // Draw mesh
            GL.BindVertexArray(bufferObject.Vao);
            GLCheck.Call(() => GL.DrawElements(PrimitiveType.Quads, _sceneIndices.Length, DrawElementsType.UnsignedInt, 0));
            GL.BindVertexArray(0);
        }
    }
}
