using System;
using RenderEngine.GraphicObjects;
using RenderEngine.GraphicObjects.Deformable;
using RenderEngine.GraphicObjects.Perpetual;
using RenderEngine.Lighting;
using Shared.Assets;

namespace RenderEngine.Rendering
{
    class RenderObjectFactory
    {
        internal static RenderObject CreateRenderObject(ObjectType type, Vertex[] vertices = null, Material material = null, bool hasNormals = true, LightBundle.BundleType bundleType = LightBundle.BundleType.Standard)
        {
            switch (type)
            {
                case ObjectType.Background: return new Background();
                case ObjectType.CoordinateAxis: return new CoordinateAxis();
                case ObjectType.RenderMesh: return new RenderMesh(vertices, material, hasNormals, new LightBundle(bundleType));
                default: throw new ArgumentException("Object type is not supported");
            }
        }
    }
}
