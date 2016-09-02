using System.Collections.Generic;
using RenderEngine.GraphicObjects;
using RenderEngine.Rendering;
using Shared.Geometry;

namespace RenderEngine.Conversion
{
    internal static class Converter
    {
        internal static Vertex[] ToRenderVertices(this Mesh mesh)
        {
            //if(mesh.Vertices.Length != mesh.Normals.Length)
            //    throw new Exception("Vertex count and normal count do not fit.");
            Vertex[] vertices = new Vertex[mesh.Vertices.Length];
            int counter = 0;
            foreach (var vector in mesh.Vertices)
            {
                Vector3d transformedVec = mesh.ModelMatrix.Muliply(vector);
                vertices[counter].PosX = transformedVec.X;
                vertices[counter].PosY = transformedVec.Y;
                vertices[counter++].PosZ = transformedVec.Z;
            }
            return vertices;
        }

        internal static List<IRenderable> ToRenderMeshes(List<Mesh> meshes)
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
