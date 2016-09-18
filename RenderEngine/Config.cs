namespace RenderEngine
{
    public struct Config
    {
        ///############# Shader ###########///
        internal static readonly string SceneVertexShaderFilename = "sceneShader.vs";
        internal static readonly string SceneFragmentShaderFilename = "sceneShader.frag";

        internal static readonly string MeshVertexShaderFilename = "mesh.vs";
        internal static readonly string MeshGeometryShaderFilename = "mesh.gs";
        internal static readonly string MeshFragmentShaderFilename = "mesh.frag";

        internal static readonly string NormalVisualizationVertexShader = "normalDisplay.vs";
        internal static readonly string NormalVisualizationGeometryShader = "normalDisplay.gs";
        internal static readonly string NormalVisualizationFragmentShader = "normalDisplay.frag";

        internal static readonly string CoordinateAxisVertexShader = "coordinateAxis.vs";
        internal static readonly string CoordinateAxisFragmentShader = "coordinateAxis.frag";

        ///############# Camera ###########///
        internal static readonly float GranularityZoom = 10f;
        internal static readonly float MinZoom = -1800f;
        internal static readonly float MaxZoom = 700f;
        internal static readonly float InitialZoom = -10f;
        internal static readonly float PitchX = 0;
        internal static readonly float PitchY = 0f;
        internal static readonly float PitchZ = 0f;

        ///############# Normal visualization ###########///
        internal static readonly float Magnitude = 0.4f;

        ///############# CoordinateAxis ###########///
        internal static readonly string CoordinateAxisMesh = "coordinate-system.dae";
    }
}
