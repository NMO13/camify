using System.Collections.Generic;
using RenderEngine.Objects;
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
                //RenderObject renderMesh = new RenderMesh();
                //renderMeshes.Add(renderMesh);
            }

            return renderMeshes;
        }
    }
}
