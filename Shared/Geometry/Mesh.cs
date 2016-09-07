using System;
using RenderEngine;
using Shared.Assets;

namespace Shared.Geometry
{
    public class Mesh
    {
        public Mesh(Vector3d[] vertices, int[] indices, Vector3d[] renderNormals)
        {
            if (vertices == null || indices == null)
                throw new ArgumentException("Vertices or indices not set");
            if (renderNormals != null)
                if (indices.Length != renderNormals.Length)
                    throw new ArgumentException("RenderNormals array must have same size as renderNormals index array");
            if (indices.Length % 3 != 0)
                throw new ArgumentException("Index array length is not valid");
            Vertices = vertices;
            Indices = indices;
            RenderNormals = renderNormals ?? new Vector3d[0];
            ModelMatrix = Matrix4d.Identity;
            CreateRenderVertices();
        }

        private void CreateRenderVertices()
        {
            RenderVertices = new Vertex[Indices.Length];
            for(int i = 0; i < Indices.Length; i++)
            {
                var vertex = Vertices[Indices[i]];
                Vertex renderVertex;
                if(RenderNormals == null || RenderNormals.Length == 0)
                    renderVertex = new Vertex((float)vertex.X, (float)vertex.Y, (float)vertex.Z);
                else
                {
                    renderVertex = new Vertex((float)vertex.X, (float)vertex.Y, (float)vertex.Z, (float)RenderNormals[i].X, (float)RenderNormals[i].Y, (float)RenderNormals[i].Z);
                }
                RenderVertices[i] = renderVertex;
            }
        }

        public Vector3d[] RenderNormals { get; }
        public Vector3d[] Vertices { get;}
        public int[] Indices;

        public Vertex[] RenderVertices { get; private set; }

        public Matrix4d ModelMatrix { get; set; }
        public Material Material = new Material(MaterialType.Gold);

    }
}
