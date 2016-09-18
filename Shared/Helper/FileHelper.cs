using System;
using System.Collections.Generic;
using System.IO;
using Shared.Geometry;
using Shared.Import;

namespace Shared.Helper
{
    public class FileHelper
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

        public static List<Mesh> LoadFileFromDropbox(String filepath)
        {
            MeshImporter importer = new MeshImporter();
            List<Mesh> meshes =
                importer.GenerateMeshes(GetDropboxFolderPath() +
                                        filepath);
            return meshes;
        }

        public static string GetProjectDirectory()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo currentDirectoryInfo = new DirectoryInfo(currentDirectory);
            return currentDirectoryInfo.FullName;
        }

        public static string GetShaderPath(string filename)
        {
            return Path.Combine(GetProjectDirectory(), "Shader", "Files", filename);
        }

        public static string GetMeshModelPath(string filename)
        {
            return Path.Combine(GetProjectDirectory(), "Meshmodel", "Files", filename);
        }
    }
}