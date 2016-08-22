using Shared;
using System;
using System.Collections.Generic;

namespace DataManagement
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
    }
}