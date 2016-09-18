using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using RenderEngine.GraphicObjects;
using RenderEngine.Rendering.Scene;

namespace RenderEngine.Rendering
{
    class Renderer
    {
        internal AnimationManager AnimationManager = new AnimationManager();
        public void Render()
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            foreach (var staticObject in SceneModel.Instance.StaticRenderObjects)
            {
                staticObject.Render(false);
            }

            AnimationManager.Animate();

            var dynamicObjects = SceneModel.Instance.DynamicRenderObjects;
            foreach (var mesh in dynamicObjects)
            {
                mesh.Render(SceneModel.Instance.WireframeMode);
            }
        }
    }
}
