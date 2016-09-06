using System;
using System.Collections.Generic;
using System.Linq;
using Assimp;
using Assimp.Configs;
using Mesh = Shared.Geometry.Mesh;
using Shared.Geometry;

namespace DataManagement
{
    public class MeshImporter
    {
        private List<Mesh> meshes = new List<Mesh>();
        public List<Mesh> GenerateMeshes(String path)
        {
            AssimpImporter importer = new AssimpImporter();
            importer.SetConfig(new RemoveComponentConfig(ExcludeComponent.Animations | ExcludeComponent.Boneweights |
                                ExcludeComponent.Cameras | ExcludeComponent.Lights |
                                ExcludeComponent.TexCoords | ExcludeComponent.Textures));

            var scene = importer.ImportFile(path, PostProcessSteps.JoinIdenticalVertices | PostProcessSteps.RemoveComponent);
            ProcessNode(scene.RootNode, scene);
            importer.Dispose();
            return meshes;
        }

        private void ProcessNode(Node node, Assimp.Scene scene)
        {
            for (int i = 0; i < node.MeshCount; i++)
            {
                // The node object only contains indices to index the actual objects in the scene. 
                // The scene contains all the data, node is just to keep stuff organized (like relations between nodes).
                Assimp.Mesh m = scene.Meshes[node.MeshIndices[i]];
                meshes.Add(ProcessMesh(m, node, scene.Materials[m.MaterialIndex]));
            }

            // After we've processed all of the meshes (if any) we then recursively process each of the children nodes
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
            List<int> vertexIndices = new List<int>();
            Dictionary<int, int> vertexMapping = new Dictionary<int, int>();
            HashSet<Vector3d> vertices = new HashSet<Vector3d>();

            VertexMapping(mesh, vertices, vertexMapping);

            Vector3d[] normals = null;
            if (mesh.HasNormals)
                normals = new Vector3d[mesh.FaceCount * 3];
            // Now walk through each of the mesh's faces and retrieve the corresponding vertex indices.
            for (int i = 0; i < mesh.FaceCount; i++)
            {
                Face face = mesh.Faces[i];
                // Retrieve all indices of the face and store them in the indices vector
                if (face.IndexCount != 3)
                    throw new Exception("A face must have exactly three vertices");
                
                for (int j = 0; j < face.IndexCount; j++)
                {
                    int index = (int)face.Indices[j];
                    vertexIndices.Add(vertexMapping[index]);
                    if (mesh.HasNormals)
                        normals[i * 3 + j] = new Vector3d(mesh.Normals[index].X, mesh.Normals[index].Y, mesh.Normals[index].Z);
                }
            }

            Mesh m = new Mesh(vertices.ToArray(), vertexIndices.ToArray(), normals);
            if (!material.Name.Contains("DefaultMaterial"))
            {
                SetMaterialProps(m, material);
            }
            m.ModelMatrix = Convert(node.Transform);
            return m;
        }

        private void NormalMapping(Assimp.Mesh mesh, HashSet<Vector3d> normals, Dictionary<int, int> normalMapping)
        {
            for (int i = 0; i < mesh.Normals.Length; i++)
            {
                Vector3d normal = new Vector3d(mesh.Vertices[i].X, mesh.Vertices[i].Y, mesh.Vertices[i].Z); // Positions
                normals.Add(normal);
            }
            List<Vector3d> vertsList = normals.ToList();
            for (var i = 0; i < mesh.VertexCount; i++)
            {
                Vector3d vertex = new Vector3d(mesh.Vertices[i].X, mesh.Vertices[i].Y, mesh.Vertices[i].Z); // Positions
                int index = vertsList.FindIndex(x => x.Equals(vertex));
                normalMapping.Add(i, index);
            }
        }

        private void VertexMapping(Assimp.Mesh mesh, HashSet<Vector3d> vertices, Dictionary<int, int> vertexMapping)
        {
            for (var i = 0; i < mesh.VertexCount; i++)
            {
                Vector3d vertex = new Vector3d(mesh.Vertices[i].X, mesh.Vertices[i].Y, mesh.Vertices[i].Z); // Positions
                vertices.Add(vertex);
            }
            List<Vector3d> vertsList = vertices.ToList();
            for (var i = 0; i < mesh.VertexCount; i++)
            {
                Vector3d vertex = new Vector3d(mesh.Vertices[i].X, mesh.Vertices[i].Y, mesh.Vertices[i].Z); // Positions
                int index = vertsList.FindIndex(x => x.Equals(vertex));
                vertexMapping.Add(i, index);
            }
        }

        private void SetMaterialProps(Mesh mesh, Assimp.Material material)
        {
            mesh.Material.AmbientR = material.ColorAmbient.R;
            mesh.Material.AmbientG = material.ColorAmbient.G;
            mesh.Material.AmbientB = material.ColorAmbient.B;

            mesh.Material.DiffuseR = material.ColorDiffuse.R;
            mesh.Material.DiffuseG = material.ColorDiffuse.G;
            mesh.Material.DiffuseB = material.ColorDiffuse.B;

            mesh.Material.SpecularR = material.ColorSpecular.R;
            mesh.Material.SpecularG = material.ColorSpecular.G;
            mesh.Material.SpecularB = material.ColorSpecular.B;

            mesh.Material.Shininess = material.Shininess;
            mesh.Material.IsSet = true;
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
