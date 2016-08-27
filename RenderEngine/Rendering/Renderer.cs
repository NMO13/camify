using System.Collections.Generic;
using Shared.Geometry;

namespace RenderEngine.Rendering
{
    class Renderer
    { 
        public void Render(List<IRenderable> meshes)
        {
            foreach (var mesh in meshes)
            {
                mesh.Render();
            }
        }
    }
}
