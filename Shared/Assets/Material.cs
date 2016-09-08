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

        public Material(MaterialType type)
        {
            switch (type)
            {
                case MaterialType.Silver: SetToSilver(); break;
                case MaterialType.Gold: SetToGold(); break;
                default: throw new Exception("Material type is not supported");
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

            Shininess = MaterialConstants.SilverShininess;
        }
        private void SetToGold()
        {
            AmbientR = MaterialConstants.GoldAmbientR;
            AmbientG = MaterialConstants.GoldAmbientG;
            AmbientB = MaterialConstants.GoldAmbientB;

            DiffuseR = MaterialConstants.GoldDiffuseR;
            DiffuseG = MaterialConstants.GoldDiffuseG;
            DiffuseB = MaterialConstants.GoldDiffuseB;

            SpecularR = MaterialConstants.GoldSpecularR;
            SpecularG = MaterialConstants.GoldSpecularG;
            SpecularB = MaterialConstants.GoldSpecularB;

            Shininess = MaterialConstants.GoldShininess;
        }
    }

    public enum MaterialType
    {
        Silver,
        Gold
    }
}
