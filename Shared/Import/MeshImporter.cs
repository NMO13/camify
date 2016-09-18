using System;
using System.Collections.Generic;
using System.Linq;
using Assimp;
using Assimp.Configs;
using Shared.Geometry;
using Mesh = Shared.Geometry.Mesh;

namespace Shared.Import
{
    public class MeshImporter
    {
        private static MeshImporter _instance;
        public static MeshImporter Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MeshImporter();
                }
                return _instance;
            }
        }

        private List<Mesh> _meshes;
        public List<Mesh> GenerateMeshes(string path)
        {
            _meshes = new List<Mesh>();
            AssimpImporter importer = new AssimpImporter();
            ExcludeComponent exclusion = ExcludeComponent.Animations | ExcludeComponent.Boneweights |
                                         ExcludeComponent.Cameras | ExcludeComponent.Lights |
                                         ExcludeComponent.TexCoords | ExcludeComponent.Textures |
                                         ExcludeComponent.Normals;
            importer.SetConfig(new RemoveComponentConfig(exclusion));

            var scene = importer.ImportFile(path, PostProcessSteps.JoinIdenticalVertices | PostProcessSteps.RemoveComponent);
            ProcessNode(scene.RootNode, scene);
            importer.Dispose();
            return _meshes;
        }

        private void ProcessNode(Node node, Assimp.Scene scene)
        {
            for (int i = 0; i < node.MeshCount; i++)
            {
                // The node object only contains indices to index the actual objects in the scene. 
                // The scene contains all the data, node is just to keep stuff organized (like relations between nodes).
                Assimp.Mesh m = scene.Meshes[node.MeshIndices[i]];
                _meshes.Add(ProcessMesh(m, node, scene.Materials[m.MaterialIndex]));
            }

            // After we've processed all of the _meshes (if any) we then recursively process each of the children nodes
            if (node.HasChildren)
            {
                for (int i = 0; i < node.Children.Length; i++)
                {
                    ProcessNode(node.Children[i], scene);
                }
            }
        }

        private Mesh ProcessMesh(Assimp.Mesh mesh, Node node, Assimp.Material material)
        {
            // Data to fill
            List<Vector3d> vertices = new List<Vector3d>();
            List<int> indices = new List<int>();

            var modelMatrix = Convert(node.Transform);
            for (var i = 0; i < mesh.VertexCount; i++)
            {
                Vector3d vertex = new Vector3d(mesh.Vertices[i].X, mesh.Vertices[i].Y, mesh.Vertices[i].Z); // Positions
                vertex = modelMatrix * vertex;
                vertices.Add(vertex);
            }

            // Now walk through each of the mesh's faces and retrieve the corresponding vertex indices.
            for (int i = 0; i < mesh.FaceCount; i++)
            {
                Face face = mesh.Faces[i];
                // Retrieve all indices of the face and store them in the indices vector
                for (int j = 0; j < face.IndexCount; j++)
                    indices.Add((int)face.Indices[j]);
            }

            //node.Transform
            Mesh m = new Mesh(vertices.ToArray(), indices.ToArray(), modelMatrix);
            return m;
        }
        private Matrix4d Convert(Matrix4x4 transformations)
        {
            Matrix4d transformationMatrix = new Matrix4d();

            transformationMatrix.M11 = transformations.A1;
            transformationMatrix.M12 = transformations.A2;
            transformationMatrix.M13 = transformations.A3;
            transformationMatrix.M14 = transformations.A4;

            transformationMatrix.M21 = transformations.B1;
            transformationMatrix.M22 = transformations.B2;
            transformationMatrix.M23 = transformations.B3;
            transformationMatrix.M24 = transformations.B4;

            transformationMatrix.M31 = transformations.C1;
            transformationMatrix.M32 = transformations.C2;
            transformationMatrix.M33 = transformations.C3;
            transformationMatrix.M34 = transformations.C4;

            transformationMatrix.M41 = transformations.D1;
            transformationMatrix.M42 = transformations.D2;
            transformationMatrix.M43 = transformations.D3;
            transformationMatrix.M44 = transformations.D4;

            return transformationMatrix;
        }


    }
}
