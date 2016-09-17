using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using RenderEngine.GraphicObjects.Deformable;
using RenderEngine.Rendering.Scene;
using Shared.Geometry;

namespace RenderEngine.Rendering
{
    class Renderer
    {
        private readonly List<RenderMesh> _renderMeshes; 
        private readonly List<IRenderable>  _perpetualMeshes;
        internal AnimationManager AnimationManager = new AnimationManager();
        public Renderer(List<RenderMesh> renderMeshes, List<IRenderable> perpetualMeshes)
        {
            _renderMeshes = renderMeshes;
            _perpetualMeshes = perpetualMeshes;
        }

        public void Render()
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            foreach (var perpetual in _perpetualMeshes)
            {
                perpetual.Render(false);
            }

            AnimationManager.Animate();

            var renderMeshes = SceneModel.Instance.RenderMeshes;
            foreach (var mesh in renderMeshes)
            {
                mesh.Render(SceneModel.Instance.WireframeMode);
            }
        }
    }
}
