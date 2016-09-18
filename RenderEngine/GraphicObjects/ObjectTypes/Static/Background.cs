using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using RenderEngine.Rendering.Scene;
using RenderEngine.Resources.Shader;
using Shared.Geometry;

namespace RenderEngine.GraphicObjects.ObjectTypes.Static
{
    internal sealed class Background : StaticRenderObject
    {
        protected override Shader Shader { get; set; }
        protected override BufferUsageHint BufferUsage => BufferUsageHint.StaticDraw;
        internal override Vertex[] Vertices { get; set; }
        internal override bool HasNormals { get; set; }

        internal Background()
        {
            Vertices = LoadVertices();
            HasNormals = false;
            Shader = ResourceManager.Instance.GetShader(ShaderLibrary.ShaderName.Scene.ToString());
            Setup();
        }

        public override void Render(bool wireframe)
        {
            GL.Disable(EnableCap.DepthTest);
            Shader.Use();
            Shader.SetInteger("resolutionY", SceneModel.Instance.SceneHeight);

            DrawMesh(PrimitiveType.Quads);
        }

        public override Vertex[] LoadVertices()
        {
            return new[] {
                new Vertex(-1, -1, -1),
                new Vertex(1, -1, -1),
                new Vertex(1, 1, -1),
                new Vertex(-1, 1, -1),
            };
        }
    }
}
