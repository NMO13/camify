using OpenTK.Graphics.OpenGL;
using RenderEngine.Rendering;
using RenderEngine.Rendering.Scene;
using RenderEngine.Resources.Shader;

namespace RenderEngine.GraphicObjects.Perpetual
{
    sealed class Background : RenderObject
    {
        protected override Shader Shader { get; set; }
        protected override BufferUsageHint BufferUsage => BufferUsageHint.StaticDraw;
        internal override Vertex[] Vertices { get; set; } = {
            new Vertex(-1, -1, -1),
            new Vertex(1, -1, -1),
            new Vertex(1, 1, -1),
            new Vertex(-1, 1, -1),
        };

        internal override bool HasNormals { get; set; } = false;

        //Constructor
        internal Background()
        {
            Shader = ResourceManager.Instance.GetShader(ShaderLibrary.ShaderName.Scene.ToString());
        }

        public override void Render()
        {
            GL.Disable(EnableCap.DepthTest);
            Shader.Use();
            Shader.SetInteger("resolutionY", SceneModel.Instance.SceneHeight);

            DrawMesh(PrimitiveType.Quads);
        }
    }
}
