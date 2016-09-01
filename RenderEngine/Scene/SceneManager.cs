using System.Collections.Generic;
using RenderEngine.Rendering;
using OpenTK.Graphics.OpenGL;
using RenderEngine.Camera;
using RenderEngine.GraphicObjects;
using RenderEngine.Resources.Shader;
using Shared.Geometry;
using Matrix4d = Shared.Geometry.Matrix4d;

namespace RenderEngine.Scene
{
    class SceneManager
    {
        private readonly Renderer _renderer = new Renderer();

        internal void Load(int width, int height)
        {
            InitializeGl();
            Resized(width, height);
            LoadShader();
            LoadPerpetualObjects();
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

                double r = (width / (double)height);
                SceneModel.Instance.ProjectionMatrix = Matrix4d.CreatePerspectiveFieldOfView((float)(0.45 * (1.5 / r)), (float)r,
                    1f, 2000);
                GL.Viewport(0, 0, width, height);
            }
        }

        internal void AdjustCamera()
        {
            SceneModel.Instance.LookAtMatrix = Matrix4d.LookAt(Objective.CameraPos, Objective.CameraFront + Objective.CameraPos, Objective.CameraUp); //Camera/View transformation
            Matrix4d zoom = Matrix4d.CreateTranslation(new Vector3d(0, 0, Objective.CurZoom));
            SceneModel.Instance.WorldTransformationMatrix = SceneModel.Instance.LookAtMatrix * zoom;
        }

        private void LoadShader()
        {
            ResourceManager.Instance.LoadShader(ShaderLibrary.SceneVertexShader, ShaderLibrary.SceneFragmentShader, null, ShaderLibrary.ShaderName.Scene.ToString());
            ResourceManager.Instance.LoadShader(ShaderLibrary.MeshVertexShader, ShaderLibrary.MeshFragmentShader, null, ShaderLibrary.ShaderName.Mesh.ToString());
        }

        private void LoadPerpetualObjects()
        {
            RenderObject background = RenderObjectFactory.CreateRenderObject(ObjectType.Background);
            SceneModel.Instance.AddRenderObject(background);
        }

        private void InitializeGl()
        {
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            GL.Viewport(0, 0, SceneModel.Instance.SceneWidth, SceneModel.Instance.SceneHeight);
        }
    }
}
