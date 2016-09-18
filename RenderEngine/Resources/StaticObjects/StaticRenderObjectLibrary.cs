using Shared.Helper;

namespace RenderEngine.Resources.Meshmodel
{
    class StaticRenderObjectLibrary
    {
        internal static string CoordinateAxisMesh { get; set; } = FileHelper.GetMeshModelPath(Config.SceneVertexShaderFilename);
    }
}
