using System.Collections.Generic;
using System.Windows.Forms.VisualStyles;
using RenderEngine.GraphicObjects;
using RenderEngine.Rendering;
using Shared.Geometry;

namespace RenderEngine.Conversion
{
    internal static class Converter
    {
        internal static List<RenderObject> ToRenderMeshes(List<Mesh> meshes)
        {
            var renderMeshes = new List<RenderObject>();
            foreach (var mesh in meshes)
            {
                var hasNormals = mesh.RenderNormals.Length > 0;
                renderMeshes.Add(RenderObjectFactory.CreateRenderObject(ObjectType.RenderMesh, mesh.RenderVertices, mesh.Material, hasNormals));
            }
            return renderMeshes;
        }
    }
}
