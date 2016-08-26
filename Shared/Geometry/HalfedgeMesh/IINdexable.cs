using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEngine.HalfedgeMesh
{
    public interface IIndexable
    {
        int Index { get; set; }
    }
}
