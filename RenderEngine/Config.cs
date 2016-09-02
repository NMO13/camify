using System.Runtime.CompilerServices;
using Shared.Geometry;

namespace RenderEngine
{
    public struct Config
    {
        ///############# Shader ###########///
        internal static readonly string SceneVertexShaderFilename = "sceneShader.vs";
        internal static readonly string SceneFragmentShaderFilename = "sceneShader.frag";

        internal static readonly string MeshVertexShaderFilename = "mesh.vs";
        internal static readonly string MeshFragmentShaderFilename = "mesh.frag";

        ///############# Camera ###########///
        internal static readonly float GranularityZoom = 10f;
        internal static readonly float MinZoom = -1800f;
        internal static readonly float MaxZoom = 700f;
        internal static readonly float InitialZoom = -400f;
        internal static readonly float PitchX = 0;
        internal static readonly float PitchY = -45f;
        internal static readonly float PitchZ = -30f;
    }
}
