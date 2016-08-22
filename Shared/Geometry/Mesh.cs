using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class Mesh
    {
        public Mesh(Vector3d[] vertices, int[] indices, Vector3d[] normals, int[] normalIndices)
        {
            if (vertices == null || indices == null)
                throw new ArgumentException("Vertices or indices not set");
            if (normals != null && normalIndices != null)
                if (indices.Length != normalIndices.Length)
                    throw new ArgumentException("Normals array must have same size as normals index array");
            if (indices.Length % 3 != 0)
                throw new ArgumentException("Index array length is not valid");
            Vertices = vertices;
            Indices = indices;
            Normals = normals ?? new Vector3d[0];
            NormalIndices = normalIndices ?? new int[0];
        }
        public Vector3d[] Normals { get; private set; }
        public Vector3d[] Vertices { get; private set; }
        public int[] Indices;
        public int[] NormalIndices { get; private set; }

        public Matrix4d ModelMatrix { get; set; }
        public Material material = new Material();

    }

    public class Material
    {
        public float ambientR;
        public float ambientG;
        public float ambientB;

        public float diffuseR;
        public float diffuseG;
        public float diffuseB;

        public float specularR;
        public float specularG;
        public float specularB;

        public float shininess;

        public bool isSet;
    }
}
