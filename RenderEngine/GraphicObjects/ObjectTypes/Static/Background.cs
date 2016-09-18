using OpenTK.Graphics.OpenGL;
using RenderEngine.Rendering.Scene;
using RenderEngine.Resources;
using RenderEngine.Resources.Shader;
using Shared.Geometry;

namespace RenderEngine.GraphicObjects.ObjectTypes.Static
{
    internal sealed class Background : StaticRenderObject
    {
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

        private Vertex[] LoadVertices()
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
