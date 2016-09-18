﻿using System.Collections.Generic;
using RenderEngine.GraphicObjects;
using RenderEngine.GraphicObjects.ObjectTypes;
using RenderEngine.GraphicObjects.ObjectTypes.Dynamic;
using Shared.Geometry;

namespace RenderEngine.Conversion
{
    internal static class Converter
    {
        internal static List<DynamicRenderObject> ToDynamicRenderObjects(List<Mesh> meshes)
        {
            var dynamicRenderObjects = new List<DynamicRenderObject>();
            foreach (var mesh in meshes)
            {
                var container = new DynamicObjectDataContainer();
                container.Vertices = mesh.RenderVertices;
                container.Material = mesh.Material;
                container.HasNormals = mesh.RenderNormals.Length > 0;
                dynamicRenderObjects.Add(RenderObjectFactory.Instance.BuildDynamicRenderObject(container));
            }
            return dynamicRenderObjects;
        }
    }
}
