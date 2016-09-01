using System.Collections.Generic;
using RenderEngine.GraphicObjects;
using RenderEngine.GraphicObjects.Deformable;
using RenderEngine.Rendering;
using Shared.Geometry;
using Shared.Helper;

namespace RenderEngine.Converter
{
    class MeshConverter
    {
        public static List<IRenderable> ToRenderMeshes(List<Mesh> meshes)
        {
            var renderMeshes = new List<IRenderable>();
            foreach (var mesh in meshes)
            {
                Vertex[] vertices = mesh.ToRenderVertices();
                int[] indices = mesh.Indices;
                renderMeshes.Add(RenderObjectFactory.CreateRenderObject(ObjectType.RenderMesh, vertices, indices));
            }
            return renderMeshes;
        }
    }
}
