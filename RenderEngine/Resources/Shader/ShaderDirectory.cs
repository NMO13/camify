using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderEngine.Resources.Shader
{
    static class ShaderDirectory
    {
        private static string sceneVertexShader = "sceneVertexShader.vs";
        private static string sceneFragmentShader = "sceneFragmentShader.frag";
        private static string meshVertexShader = "meshVertexShader.vs";
        private static string meshFragmentShader = "meshFragmentShader.frag";
        private static string coordinateAxisVertexShader = "coordinateAxisVertexShader.vs";
        private static string coordinateAxisFragmentShader = "coordinateAxisFragmentShader.frag";
        private static string spriteVertexShader = "spriteVertexShader.vs";
        private static string spriteFragmentShader = "spriteFragmentShader.frag";
    }

    public enum ShaderName
    {
        Scene,
        Mesh,
        CoordinateAxis
    }
}
