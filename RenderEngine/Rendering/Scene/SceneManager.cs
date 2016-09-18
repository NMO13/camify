using System.Collections.Generic;
using GraphicsEngine.Rotation;
using OpenTK.Graphics.OpenGL;
using RenderEngine.Camera;
using RenderEngine.GraphicObjects;
using RenderEngine.GraphicObjects.ObjectTypes;
using RenderEngine.Resources.Shader;
using Shared.Geometry;
using Matrix4d = Shared.Geometry.Matrix4d;

namespace RenderEngine.Rendering.Scene
{
    class SceneManager
    {
        private readonly Renderer _renderer = new Renderer();
        internal WorldRotator WorldRotator = new WorldRotator();

        internal void Load(int width, int height)
        {
            InitializeGl();
            Resized(width, height);
            LoadShader();
            LoadStaticObjects();
        }

        internal void Paint()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            _renderer.Render();
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
                WorldRotator.SetBounds(width, height);
            }
        }

        internal void AdjustCamera()
        {
            SceneModel.Instance.LookAtMatrix = Matrix4d.LookAt(Objective.CameraPos, Objective.CameraFront + Objective.CameraPos, Objective.CameraUp); //Camera/View transformation
            Matrix4d zoom = Matrix4d.CreateTranslation(new Vector3d(0, 0, Objective.CurZoom));
            SceneModel.Instance.WorldTransformationMatrix = Matrix4d.Mult(SceneModel.Instance.LookAtMatrix, zoom);

            //Rotate camera
            Matrix4d test = WorldRotator.GetRotationMatrix();
            Matrix4d rotation = Matrix4d.Mult(Objective.InitialPitch, test);
            SceneModel.Instance.WorldTransformationMatrix = Matrix4d.Mult(rotation,
                SceneModel.Instance.WorldTransformationMatrix);
        }

        private void LoadShader()
        {
            ResourceManager.Instance.LoadShader(ShaderLibrary.SceneVertexShader, ShaderLibrary.SceneFragmentShader, null, ShaderLibrary.ShaderName.Scene.ToString());
            ResourceManager.Instance.LoadShader(ShaderLibrary.MeshVertexShader, ShaderLibrary.MeshFragmentShader, ShaderLibrary.MeshGeometryShader, ShaderLibrary.ShaderName.Mesh.ToString());
            ResourceManager.Instance.LoadShader(ShaderLibrary.NormalDisplayVertexShader, ShaderLibrary.NormalDisplayFragmentShader, ShaderLibrary.NormalDisplayGeometryShader, ShaderLibrary.ShaderName.NormalVisualization.ToString());
        }

        private void LoadStaticObjects()
        {
            StaticRenderObject background = RenderObjectFactory.Instance.BuildStaticRenderObject(ObjectType.Background);
            SceneModel.Instance.AddStaticObject(background);

            //RenderObject coordinateAxis = RenderObjectFactory.CreateRenderObject(ObjectType.);
            //SceneModel.Instance.AddPerpetualObject(coordinateAxis);
        }

        private void InitializeGl()
        {
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            GL.Viewport(0, 0, SceneModel.Instance.SceneWidth, SceneModel.Instance.SceneHeight);
        }
    }
}
