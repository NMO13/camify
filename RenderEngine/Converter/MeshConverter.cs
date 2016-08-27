using System.Collections.Generic;
using RenderEngine.Rendering;
using Shared.Geometry;

namespace RenderEngine.Converter
{
    class MeshConverter
    {
        public static List<RenderMesh> ToRenderMeshes(List<Mesh> meshes)
        {
            var renderMeshes = new List<RenderMesh>();
            foreach (var mesh in meshes)
            { 
                RenderMesh renderMesh = new RenderMesh(mesh.Vertices, mesh.Indices, mesh.ModelMatrix);
                renderMeshes.Add(renderMesh);
            }

            return renderMeshes;
        }
    }
}
