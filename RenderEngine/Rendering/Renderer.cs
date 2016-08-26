using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

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
