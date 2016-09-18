using Shared.Helper;

namespace RenderEngine.Resources.Shader
{
    internal static class ShaderLibrary
    {
        //Scene
        internal static string SceneVertexShader { get; set; } = FileHelper.GetShaderPath(Config.SceneVertexShaderFilename);
        internal static string SceneFragmentShader { get; set; } = FileHelper.GetShaderPath(Config.SceneFragmentShaderFilename);
        //Mesh
        internal static string MeshVertexShader { get; set; } = FileHelper.GetShaderPath(Config.MeshVertexShaderFilename);
        internal static string MeshGeometryShader { get; set; } = FileHelper.GetShaderPath(Config.MeshGeometryShaderFilename);
        internal static string MeshFragmentShader { get; set; } = FileHelper.GetShaderPath(Config.MeshFragmentShaderFilename);
        //Normal visualization
        internal static string NormalDisplayVertexShader { get; set; } = FileHelper.GetShaderPath(Config.NormalVisualizationVertexShader);
        internal static string NormalDisplayGeometryShader { get; set; } = FileHelper.GetShaderPath(Config.NormalVisualizationGeometryShader);
        internal static string NormalDisplayFragmentShader { get; set; } = FileHelper.GetShaderPath(Config.NormalVisualizationFragmentShader);
        //Coordinate Axis
        internal static string CoordinateAxisVertexShader { get; set; } = FileHelper.GetShaderPath(Config.CoordinateSystemVertexShader);
        internal static string CoordinateAxisFragmentShader { get; set; } = FileHelper.GetShaderPath(Config.CoordinateSystemFragmentShader);

        //private static string GetBasePath()
        //{
        //    string currentDirectory = Directory.GetCurrentDirectory();
        //    DirectoryInfo currentDirectoryInfo = new DirectoryInfo(currentDirectory);
        //    string projectPath = currentDirectoryInfo.FullName;
        //    return Path.Combine(projectPath, "Shader", "Files");
        //}

        public enum ShaderName
        {
            Scene,
            Mesh,
            CoordinateAxis,
            NormalVisualization
        }
    }
}
