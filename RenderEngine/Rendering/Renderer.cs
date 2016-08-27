using System.Collections.Generic;
using Shared.Geometry;

namespace RenderEngine.Rendering
{
    class Renderer
    {
        public List<RenderMesh> Mesh { private get; set; }

        public void Render()
        {
            foreach (var mesh in Mesh)
            {
                mesh.Draw();
            }
        }

        private void Draw()
        {
            
        }
    }
}
