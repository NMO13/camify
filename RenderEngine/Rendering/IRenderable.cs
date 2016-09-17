using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderEngine.Rendering
{
    interface IRenderable
    {
        void Render(bool wireframe);
    }
}
