using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Assets
{
    class MaterialFactory
    {
        internal static Material CreateDefaultMaterial(MaterialType type)
        {
            switch (type)
            {
                case MaterialType.Silver: return new Material(type);
                default: throw new Exception("Material type is not supported");
            }
        }
    }
}
