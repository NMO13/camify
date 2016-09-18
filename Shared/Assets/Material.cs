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
                case MaterialType.Red: SetToRed(); break;
                case MaterialType.Green: SetToGreen(); break;
                case MaterialType.Blue: SetToBlue(); break;
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

        private void SetToRed()
        {
            AmbientR = MaterialConstants.RedAmbientR;
            AmbientG = MaterialConstants.RedAmbientG;
            AmbientB = MaterialConstants.RedAmbientB;

            DiffuseR = MaterialConstants.RedDiffuseR;
            DiffuseG = MaterialConstants.RedDiffuseG;
            DiffuseB = MaterialConstants.RedDiffuseB;

            SpecularR = MaterialConstants.RedSpecularR;
            SpecularG = MaterialConstants.RedSpecularG;
            SpecularB = MaterialConstants.RedSpecularB;

            Shininess = MaterialConstants.RedShininess;
        }

        private void SetToGreen()
        {
            AmbientR = MaterialConstants.GreenAmbientR;
            AmbientG = MaterialConstants.GreenAmbientG;
            AmbientB = MaterialConstants.GreenAmbientB;

            DiffuseR = MaterialConstants.GreenDiffuseR;
            DiffuseG = MaterialConstants.GreenDiffuseG;
            DiffuseB = MaterialConstants.GreenDiffuseB;

            SpecularR = MaterialConstants.GreenSpecularR;
            SpecularG = MaterialConstants.GreenSpecularG;
            SpecularB = MaterialConstants.GreenSpecularB;

            Shininess = MaterialConstants.GreenShininess;
        }

        private void SetToBlue()
        {
            AmbientR = MaterialConstants.BlueAmbientR;
            AmbientG = MaterialConstants.BlueAmbientG;
            AmbientB = MaterialConstants.BlueAmbientB;

            DiffuseR = MaterialConstants.BlueDiffuseR;
            DiffuseG = MaterialConstants.BlueDiffuseG;
            DiffuseB = MaterialConstants.BlueDiffuseB;

            SpecularR = MaterialConstants.BlueSpecularR;
            SpecularG = MaterialConstants.BlueSpecularG;
            SpecularB = MaterialConstants.BlueSpecularB;

            Shininess = MaterialConstants.BlueShininess;
        }
    }

    public enum MaterialType
    {
        Silver,
        Gold,
        Red, 
        Green,
        Blue
    }
}
