using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderEngine.Lighting
{
    internal struct DirectionalLight
    {
        internal DirectionalLight(float dirX, float dirY, float dirZ, float ambientR, float ambientG, float ambientB, 
                                    float diffuseR, float diffuseG, float diffuseB, float specularR, float specularG, float specularB )
        {
            DirX = dirX;
            DirY = dirY;
            DirZ = dirZ;

            AmbientR = ambientR;
            AmbientG = ambientB;
            AmbientB = ambientG;

            DiffuseR = diffuseR;
            DiffuseB = diffuseG;
            DiffuseG = diffuseB;

            SpecularR = specularR;
            SpecularB = specularB;
            SpecularG = specularG;
        }

        internal float DirX { get; private set; }
        internal float DirY { get; private set; }
        internal float DirZ { get; private set; }

        internal float AmbientR { get; private set; }
        internal float AmbientG { get; private set; }
        internal float AmbientB { get; private set; }

        internal float DiffuseR { get; private set; }
        internal float DiffuseG { get; private set; }
        internal float DiffuseB { get; private set; }

        internal float SpecularR { get; private set; }
        internal float SpecularG { get; private set; }
        internal float SpecularB { get; private set; }
    }
}
