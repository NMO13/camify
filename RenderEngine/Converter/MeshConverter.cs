using System.Collections.Generic;
using RenderEngine.Rendering;
using Shared.Geometry;

namespace RenderEngine.Converter
{
    class MeshConverter
    {
        public static List<RenderObject> ToRenderMeshes(List<Mesh> meshes)
        {
            var renderMeshes = new List<RenderObject>();
            foreach (var mesh in meshes)
            {
                RenderObject renderMesh = new RenderObject(mesh.Vertices, mesh.Indices, mesh.ModelMatrix);
                renderMeshes.Add(renderMesh);
            }

            return renderMeshes;
        }
    }
}
