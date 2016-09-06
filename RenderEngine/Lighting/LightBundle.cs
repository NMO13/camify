using System;
using System.Collections.Generic;

namespace RenderEngine.Lighting
{
    internal class LightBundle
    {
        internal List<DirectionalLight> DirectionalLights { get; } = new List<DirectionalLight>();
        internal List<PointLight> PointLights { get; } = new List<PointLight>();

        internal LightBundle(BundleType bundleType)
        { 
            SetLights(bundleType);
        }

        private void SetLights(BundleType bundleType)
        {
            switch (bundleType)
            {
                case BundleType.Standard: CreateStandardLight(); break;
                default: throw new ArgumentException("Lighttype is not supported.");
            }
        }

        //1 directional light
        //4 point lights
        private void CreateStandardLight()
        {
            DirectionalLight directionalLight = new DirectionalLight(1, 1, -1, 0.3f, 0.3f, 0.3f, 0.5f, 0.5f, 0.5f,
                0.5f, 0.5f, 0.5f);
            DirectionalLights.Add(directionalLight);

            PointLight pointLightOne = new PointLight(0, 0, -20, 0.25f, 0.25f, 0.25f, 0.3f, 0.3f, 0.3f, 0.2f, 0.2f, 0.2f);
            PointLights.Add(pointLightOne);
        }
        internal enum BundleType
        {
            Standard
        }
    }
}
