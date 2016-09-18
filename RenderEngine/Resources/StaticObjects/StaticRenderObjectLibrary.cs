using Shared.Helper;

namespace RenderEngine.Resources.StaticObjects
{
    class StaticRenderObjectLibrary
    {
        internal static string CoordinateSystemMesh { get; set; } = FileHelper.GetMeshModelPath(Config.CoordinateSystemMesh);
    }
}
