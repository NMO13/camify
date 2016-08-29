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

        internal static string SceneVertexShader { get; set; } = Path.Combine(ShaderBasePath,
            Config.SceneVertexShaderFilename);

        internal static string SceneFragmentShader { get; set; } = Path.Combine(ShaderBasePath,
            Config.SceneFragmentShaderFilename);

        private static string GetBasePath()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo currentDirectoryInfo = new DirectoryInfo(currentDirectory);
            string projectPath = currentDirectoryInfo.FullName;
            return Path.Combine(projectPath, "Shader", "Files");
        }
        public enum ShaderName
        {
            Scene,
            Mesh,
            CoordinateAxis
        }
    }
}
