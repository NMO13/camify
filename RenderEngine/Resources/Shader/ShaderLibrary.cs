using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderEngine.Resources.Shader
{
    internal static class ShaderLibrary
    {
        private static readonly string ShaderBasePath = GetBasePath();

        //Scene
        internal static string SceneVertexShader { get; set; } = GetFullPath(Config.SceneVertexShaderFilename);

        internal static string SceneFragmentShader { get; set; } = GetFullPath(Config.SceneFragmentShaderFilename);

        //Mesh
        internal static string MeshVertexShader { get; set; } = GetFullPath(Config.MeshVertexShaderFilename);

        internal static string MeshGeometryShader { get; set; } = GetFullPath(Config.MeshGeometryShaderFilename);

        internal static string MeshFragmentShader { get; set; } = GetFullPath(Config.MeshFragmentShaderFilename);

        //Normal visualization
        internal static string NormalDisplayVertexShader { get; set; } = GetFullPath(Config.NormalVisualizationVertexShader);
        internal static string NormalDisplayGeometryShader { get; set; } = GetFullPath(Config.NormalVisualizationGeometryShader);
        internal static string NormalDisplayFragmentShader { get; set; } = GetFullPath(Config.NormalVisualizationFragmentShader);

        private static string GetBasePath()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo currentDirectoryInfo = new DirectoryInfo(currentDirectory);
            string projectPath = currentDirectoryInfo.FullName;
            return Path.Combine(projectPath, "Shader", "Files");
        }

        private static string GetFullPath(string shaderPath)
        {
            return Path.Combine(ShaderBasePath, shaderPath);
        }
        public enum ShaderName
        {
            Scene,
            Mesh,
            CoordinateAxis,
            ShaderVisualization
        }
    }
}
