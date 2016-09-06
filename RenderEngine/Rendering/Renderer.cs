using System.Collections.Generic;
using Shared.Geometry;

namespace RenderEngine.Rendering
{
    class Renderer
    {
        private readonly List<IRenderable> _renderMeshes, _perpetualMeshes;
        public Renderer(List<IRenderable> renderMeshes, List<IRenderable> perpetualMeshes)
        {
            _renderMeshes = renderMeshes;
            _perpetualMeshes = perpetualMeshes;
        }

        public void Render()
        {
            foreach (var perpetual in _perpetualMeshes)
            {
                perpetual.Render();
            }

            foreach (var mesh in _renderMeshes)
            {
                mesh.Render();
            }
        }
    }
}
