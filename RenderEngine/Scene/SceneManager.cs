using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderEngine.Rendering;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using RenderEngine.GraphicObjects;
using RenderEngine.Resources.Shader;

namespace RenderEngine.Scene
{
    class SceneManager
    {
        private readonly Renderer _renderer = new Renderer();

        internal void Load()
        {
            InitializeGl();
            LoadShader();
            LoadGraphicObjects();
        }

        internal void Paint()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            List<IRenderable> renderMeshes = SceneModel.Instance.RenderMeshes;
            _renderer.Render(renderMeshes);
        }

        internal void Resized(int width, int height)
        {
            if(width > 0 && height > 0)
            {
                SceneModel.Instance.SceneWidth = width;
                SceneModel.Instance.SceneHeight = height;
            }
        }

        private void LoadShader()
        {
            ResourceManager.Instance.LoadShader(ShaderLibrary.SceneVertexShader, ShaderLibrary.SceneFragmentShader, null, ShaderLibrary.ShaderName.Scene.ToString());
        }

        private void LoadGraphicObjects()
        {
            RenderObject background = RenderObjectFactory.CreateRenderObject(ObjectType.Background);
            SceneModel.Instance.AddRenderObject(background);
        }

        private void InitializeGl()
        {
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
        }
    }
}
