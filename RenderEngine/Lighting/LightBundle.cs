using System;
using System.Collections.Generic;

namespace RenderEngine.Lighting
{
    internal class LightBundle
    {
        internal List<DirectionalLight> DirectionalLights { get; } = new List<DirectionalLight>();
        internal List<PointLight> PointLights { get; } = new List<PointLight>();

        internal LightBundle()
        {
            SetLights(BundleType.Standard);
        }

        internal LightBundle(BundleType bundleType)
        { 
            SetLights(bundleType);
        }

        private void SetLights(BundleType bundleType)
        {
            switch (bundleType)
            {
                case BundleType.Standard: CreateStandardLight(); break;
                case BundleType.CoordinateAxis: CreateCoordinateAxisLight(); break;
                default: throw new ArgumentException("Lighttype is not supported.");
            }
        }

        private void CreateCoordinateAxisLight()
        {
            throw new NotImplementedException();
        }

        //1 directional light
        //4 point lights
        private void CreateStandardLight()
        {
            DirectionalLight directionalLight = new DirectionalLight(1f, 1f, -1f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f,
                0.5f, 0.5f, 0.5f);
            DirectionalLights.Add(directionalLight);

            PointLight pointLightOne = new PointLight(0, 200, 0, 0.5f, 0.5f, 0.5f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f);
            PointLights.Add(pointLightOne);

            PointLight pointLightTwo = new PointLight(200, 0, 200, 0.5f, 0.5f, 0.5f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f);
            PointLights.Add(pointLightTwo);
        }
        internal enum BundleType
        {
            Standard,
            CoordinateAxis
        }
    }
}
