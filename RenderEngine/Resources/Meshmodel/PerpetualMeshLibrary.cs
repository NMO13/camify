using Shared.Helper;

namespace RenderEngine.Resources.Meshmodel
{
    class PerpetualMeshLibrary
    {
        internal static string CoordinateAxisMesh { get; set; } = FileHelper.GetMeshModelPath(Config.SceneVertexShaderFilename);
    }
}
