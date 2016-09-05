using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderEngine.Lighting
{
    internal struct PointLight
    {
        internal PointLight(float posX, float posY, float posZ, float ambientR, float ambientB, float ambientG,
                            float diffuseR, float diffuseG, float diffuseB, float specularR, float specularB, float specularG)
        {
            PosX = posX;
            PosY = posY;
            PosZ = posZ;

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
        internal float PosX { get; private set; }
        internal float PosY { get; private set; }
        internal float PosZ { get; private set; }

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
