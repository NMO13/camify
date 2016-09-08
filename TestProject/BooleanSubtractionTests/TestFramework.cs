using System;
using System.Collections.Generic;
using GeometryCalculation.DataStructures;
using GraphicsEngine.HalfedgeMesh;
using NUnit.Framework;
using Shared.Geometry;
using Shared.Import;

namespace TestProject.BooleanSubtractionTests
{
    static class TestFramework
    {
        internal static String GetDropboxFolderPath()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string dbPath = System.IO.Path.Combine(appDataPath, "Dropbox\\host.db");
            string[] lines = System.IO.File.ReadAllLines(dbPath);
            byte[] dbBase64Text = Convert.FromBase64String(lines[1]);
            string folderPath = System.Text.ASCIIEncoding.ASCII.GetString(dbBase64Text);
            return folderPath;
        }

        internal static List<Mesh> LoadFile(String file)
        {
            MeshImporter importer = new MeshImporter();
            List<Mesh> meshes = importer.GenerateMeshes(file);
            return meshes;
        }

        //internal static List<Mesh> LoadMeshesFromDropboxFile(String filename)
        //{
        //    var dbFolderpath = GetDropboxFolderPath();
        //    return LoadFile(dbFolderpath + "\\" + filename);
        //}

        internal static void CheckSanity(DeformableObject a)
        {
            a.CheckSanity();
            AreVerticesUnique(a);
            AreFacesUnique(a);
            a.GetMesh();
        }

        internal static void IsReset(HeMesh mesh)
        {
            foreach (var halfedge in mesh.HalfedgeList)
            {
                Assert.IsTrue(halfedge.IsSplitLine == false);
            }

            foreach (var face in mesh.FaceList)
            {
                Assert.AreEqual(face.DynamicProperties.Count, 0);
            }
        }

        private static void AreFacesUnique(DeformableObject obj)
        {
            var faceList = obj.HeMesh.FaceList;
            for (int i = 0; i < faceList.Count; i++)
            {
                if (faceList[i] == null)
                    continue;
                for (int j = i + 1; j < faceList.Count; j++)
                {
                    if (faceList[j] == null)
                        continue;
                    Assert.False(faceList[i].Equals(faceList[j]));
                }
            }
        }

        internal static void AreVerticesUnique(DeformableObject obj)
        {
            var vList = obj.HeMesh.VertexList;

            for (int i = 0; i < vList.Count; i++)
            {
                if (vList[i] == null)
                    continue;
                for (int j = i + 1; j < vList.Count; j++)
                {
                    if (vList[j] == null)
                        continue;
                    Assert.False(vList[i].Equals(vList[j]));
                }
            }
        }
    }
}
