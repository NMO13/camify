using System;
using System.Collections.Generic;
using Shared.Geometry;

namespace GraphicsEngine.Geometry.Meshes
{
    public class DefaultMeshes
    {
        public static Mesh Box(float xExtend, float yExtend, float zExtend)
        {
            Vector3d[] defaultBoxVertices =
            {
                new Vector3d(-1 * xExtend, -1 * yExtend, -1 * zExtend),
                new Vector3d(1 * xExtend, -1 * yExtend, -1 * zExtend),
                new Vector3d(-1 * xExtend, 1 * yExtend, -1 * zExtend),
                new Vector3d(1 * xExtend, 1 * yExtend, -1 * zExtend),
                new Vector3d(-1 * xExtend, -1 * yExtend, 1 * zExtend),
                new Vector3d(1 * xExtend, -1 * yExtend, 1 * zExtend),
                new Vector3d(-1 * xExtend, 1 * yExtend, 1 * zExtend),
                new Vector3d(1 * xExtend, 1 * yExtend, 1 * zExtend)
            };
          
            int[] defaultBoxCoordinates =
	        {
	            0, 2, 3,
	            3, 1, 0,
	            4, 5, 7,
	            7, 6, 4,
	            0, 1, 5,
	            5, 4, 0,
	            1, 3, 7,
	            7, 5, 1,
	            3, 2, 6,
	            6, 7, 3,
	            2, 0, 4,
	            4, 6, 2
	        };

            var defaultBoxNormals = CalcNormals(defaultBoxVertices, defaultBoxCoordinates);
            Mesh mesh = new Mesh(defaultBoxVertices, defaultBoxCoordinates, defaultBoxNormals);
            return mesh;
        }

        private static Vector3d[] CalcNormals(Vector3d[] vertices, int[] indices)
        {
            Vector3d[] normals = new Vector3d[indices.Length];
            for(int i = 0; i < indices.Length; i+=3)
            {
                var v0 = vertices[indices[i]];
                var v1 = vertices[indices[i + 1]];
                var v2 = vertices[indices[i + 2]];

                var normal = (v1 - v0).Cross(v2 - v0);
                normals[i] = normal;
                normals[i + 1] = normal.Clone() as Vector3d;
                normals[i + 2] = normal.Clone() as Vector3d;
            }
            return normals;
        }

        public static Mesh Icosphere(int granularity, int scale)
        {
            return new IcoSphereCreator().Icosphere(granularity, scale);
        }

        private class IcoSphereCreator
        {
            private Dictionary<Int64, int> middlePointIndexCache;
            private List<Vector3d> vertices = new List<Vector3d>();
            private float _scale; 

            private struct TriangleIndices
            {
                public int v1;
                public int v2;
                public int v3;

                public TriangleIndices(int v1, int v2, int v3)
                {
                    this.v1 = v1;
                    this.v2 = v2;
                    this.v3 = v3;
                }
            }

            public Mesh Icosphere(int recursionLevel, int scale)
            {
                _scale = scale;
                middlePointIndexCache = new Dictionary<long, int>();

                float t = (float) (1.0 + System.Math.Sqrt(5.0)) / 2.0f;
                AddVertex(new Vector3d(-1, t, 0));
                AddVertex(new Vector3d(1, t, 0));
                AddVertex(new Vector3d(-1, -t, 0));
                AddVertex(new Vector3d(1, -t, 0));

                AddVertex(new Vector3d(0, -1, t));
                AddVertex(new Vector3d(0, 1, t));
                AddVertex(new Vector3d(0, -1, -t));
                AddVertex(new Vector3d(0, 1, -t));

                AddVertex(new Vector3d(t, 0, -1));
                AddVertex(new Vector3d(t, 0, 1));
                AddVertex(new Vector3d(-t, 0, -1));
                AddVertex(new Vector3d(-t, 0, 1));

                var faces = new List<TriangleIndices>();
                faces.Add(new TriangleIndices(0, 11, 5));
                faces.Add(new TriangleIndices(0, 5, 1));
                faces.Add(new TriangleIndices(0, 1, 7));
                faces.Add(new TriangleIndices(0, 7, 10));
                faces.Add(new TriangleIndices(0, 10, 11));

                // 5 adjacent faces 
                faces.Add(new TriangleIndices(1, 5, 9));
                faces.Add(new TriangleIndices(5, 11, 4));
                faces.Add(new TriangleIndices(11, 10, 2));
                faces.Add(new TriangleIndices(10, 7, 6));
                faces.Add(new TriangleIndices(7, 1, 8));

                // 5 faces around point 3
                faces.Add(new TriangleIndices(3, 9, 4));
                faces.Add(new TriangleIndices(3, 4, 2));
                faces.Add(new TriangleIndices(3, 2, 6));
                faces.Add(new TriangleIndices(3, 6, 8));
                faces.Add(new TriangleIndices(3, 8, 9));

                // 5 adjacent faces 
                faces.Add(new TriangleIndices(4, 9, 5));
                faces.Add(new TriangleIndices(2, 4, 11));
                faces.Add(new TriangleIndices(6, 2, 10));
                faces.Add(new TriangleIndices(8, 6, 7));
                faces.Add(new TriangleIndices(9, 8, 1));


                // refine triangles
                for (int i = 0; i < recursionLevel; i++)
                {
                    var faces2 = new List<TriangleIndices>();
                    foreach (var tri in faces)
                    {
                        // replace triangle by 4 triangles
                        int a = GetMiddlePoint(tri.v1, tri.v2);
                        int b = GetMiddlePoint(tri.v2, tri.v3);
                        int c = GetMiddlePoint(tri.v3, tri.v1);

                        faces2.Add(new TriangleIndices(tri.v1, a, c));
                        faces2.Add(new TriangleIndices(tri.v2, b, a));
                        faces2.Add(new TriangleIndices(tri.v3, c, b));
                        faces2.Add(new TriangleIndices(a, b, c));
                    }
                    faces = faces2;
                }

                int[] indices = new int[faces.Count * 3];
                for(int i = 0; i < faces.Count; i++)
                {
                    indices[i * 3] = faces[i].v1;
                    indices[i * 3 + 1] = faces[i].v2;
                    indices[i * 3 + 2] = faces[i].v3;

                }
                var normals = CalcNormals(vertices.ToArray(), indices);
                Mesh mesh = new Mesh(vertices.ToArray(), indices, normals);
                return mesh;
            }

            private int GetMiddlePoint(int p1, int p2)
            {
                // first check if we have it already
                bool firstIsSmaller = p1 < p2;
                Int64 smallerIndex = firstIsSmaller ? p1 : p2;
                Int64 greaterIndex = firstIsSmaller ? p2 : p1;
                Int64 key = (smallerIndex << 32) + greaterIndex;

                int ret;
                if (middlePointIndexCache.TryGetValue(key, out ret))
                {
                    return ret;
                }

                // not in cache, calculate it
                Vector3d point1 = vertices[p1];
                Vector3d point2 = vertices[p2];
                Vector3d middle = new Vector3d(
                    (point1.X + point2.X)/2.0f,
                    (point1.Y + point2.Y)/2.0f,
                    (point1.Z + point2.Z)/2.0f);

                // add vertex makes sure point is on unit sphere
                int i = AddVertex(middle);

                // store it, return index
                middlePointIndexCache.Add(key, i);
                return i;
            }

            private int AddVertex(Vector3d p)
            {
                float length = (float) System.Math.Sqrt(p.X * p.X + p.Y * p.Y + p.Z * p.Z);
                vertices.Add(new Vector3d((p.X *_scale)/ length, (p.Y *_scale) / length, (p.Z * _scale)/ length));
                return vertices.Count - 1;
            }
        }

        public static Mesh Pyramid(float baseWidth, float baseDepth, float height)
        {
            Vector3d[] defaultBoxVertices =
            {
                new Vector3d(-1 * baseWidth, 0, 1 * baseDepth),
                new Vector3d(1 * baseWidth, 0, -1 * baseDepth),
                new Vector3d(-1 * baseWidth, 0, -1 * baseDepth),
                new Vector3d(1 * baseWidth, 0, 1 * baseDepth),
                new Vector3d(0, 1 * height, 0)
            };

            int[] defaultBoxCoordinates =
	        {
                0, 2, 3,
                1, 3, 2,
                4, 0, 3,
                4, 3, 1,
                4, 1, 2,
                4, 2, 0
  
	        };

            var normals = CalcNormals(defaultBoxVertices, defaultBoxCoordinates);
            Mesh mesh = new Mesh(defaultBoxVertices, defaultBoxCoordinates, normals);
            return mesh;
        }
    }
}
