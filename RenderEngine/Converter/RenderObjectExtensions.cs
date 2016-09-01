using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RenderEngine;
using RenderEngine.Converter;
using Shared.Geometry;

namespace Shared.Helper
{
    internal static class RenderObjectExtensions
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
    }
}
