using System;
using System.Collections;
using System.Collections.Generic;
using Assimp.Configs;
using GraphicsEngine.HalfedgeMesh;
using RenderEngine;
using Shared.Assets;

namespace Shared.Geometry
{
    public class Mesh
    {
        public Mesh(Vector3d[] vertices, int[] indices, Matrix4d modelmatrix)
        {
            Vertices = vertices;
            Indices = indices;
            ModelMatrix = modelmatrix;
        }

        public Mesh(HeMesh heMesh)
        {
            RenderNormals = new Vector3d[1];

            heMesh.VertexList.Compact();
            Vertices = new Vector3d[heMesh.VertexList.Count];
            int i = 0;
            foreach (var heVertex in heMesh.VertexList)
            {
                Vertices[i++] = heVertex.Vector3d;
            }
            List<int> indices = new List<int>();
            RenderVertices = new Vertex[heMesh.HalfedgeList.Count];
            i = 0;
            foreach (var heFace in heMesh.FaceList)
            {
                indices.Add(heFace.V0.Index);
                indices.Add(heFace.V1.Index);
                indices.Add(heFace.V2.Index);

                // current normal algorithm
                Vector3d normal = heFace.OuterComponent.Normal.Vector3d.Unit();

                var vertex = new Vertex((float)Vertices[heFace.V0.Index].X, (float)Vertices[heFace.V0.Index].Y, (float)Vertices[heFace.V0.Index].Z, (float)normal.X, (float)normal.Y, (float)normal.Z);
                RenderVertices[i++] = vertex;

                vertex = new Vertex((float)Vertices[heFace.V1.Index].X, (float)Vertices[heFace.V1.Index].Y, (float)Vertices[heFace.V1.Index].Z, (float)normal.X, (float)normal.Y, (float)normal.Z);
                RenderVertices[i++] = vertex;

                vertex = new Vertex((float)Vertices[heFace.V2.Index].X, (float)Vertices[heFace.V2.Index].Y, (float)Vertices[heFace.V2.Index].Z, (float)normal.X, (float)normal.Y, (float)normal.Z);
                RenderVertices[i++] = vertex;
            }
            Indices = indices.ToArray();
            //TODO replace with smooth normal calculation
            // TODO calculate contour edges and save them in list
            //CalculateContour();


        }

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

        public List<Vector3d> RenderNormals2 = new List<Vector3d>(); 
        public Vector3d[] Vertices { get;}
        public int[] Indices;
        public List<Edge> Edges = new List<Edge>(); 
        public List<Face> Faces = new List<Face>();

        public Vertex[] RenderVertices { get; private set; }

        public Matrix4d ModelMatrix { get; set; }
        public Material Material = new Material(MaterialType.Silver);

        public class Face
        {
            public int Index = -1;
            public int Edge0;
            public int Edge1;
            public int Edge2;
        }

        public class Edge
        {
            public int Index = -1;
            public int TwinEdge;
            public int IncidentFace;
            public int Vertex;
        }

    }
}
