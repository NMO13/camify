using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Assets
{
    public class Material
    {
        public float AmbientR;
        public float AmbientG;
        public float AmbientB;

        public float DiffuseR;
        public float DiffuseG;
        public float DiffuseB;

        public float SpecularR;
        public float SpecularG;
        public float SpecularB;

        public float Shininess;

        internal Material(MaterialType type)
        {
            switch (type)
            {
                case MaterialType.Silver: SetToSilver(); break;
                case MaterialType.Gold: SetToGold(); break;
            }
        }

        private void SetToSilver()
        {
            AmbientR = MaterialConstants.SilverAmbientR;
            AmbientG = MaterialConstants.SilverAmbientG;
            AmbientB = MaterialConstants.SilverAmbientB;

            DiffuseR = MaterialConstants.SilverDiffuseR;
            DiffuseG= MaterialConstants.SilverDiffuseG;
            DiffuseB = MaterialConstants.SilverDiffuseB;

            SpecularR = MaterialConstants.SilverSpecularR;
            SpecularG = MaterialConstants.SilverSpecularG;
            SpecularB = MaterialConstants.SilverSpecularB;

            Shininess = MaterialConstants.Shininess;
        }
        private void SetToGold()
        {
            throw new NotImplementedException();
        }
    }

    enum MaterialType
    {
        Silver,
        Gold
    }
}
