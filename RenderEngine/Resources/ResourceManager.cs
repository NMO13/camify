using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using RenderEngine.Resources.Shader;

namespace RenderEngine
{
    class ResourceManager
    {
        private readonly Dictionary<string, Shader> _shaderDict = new Dictionary<string, Shader>(); 
        private readonly Dictionary<string, Texture> _textureDict = new Dictionary<string, Texture>();
        private static ResourceManager _instance;

        internal static ResourceManager Instance
        {
            get
            {
                if(_instance == null)
                    _instance = new ResourceManager();
                return _instance;
            }
        }
        private ResourceManager() { }
        internal Shader GetShader(string name)
        {
            return _shaderDict[name];
        }

        internal void LoadShader(string vertexShaderPath, string fragShaderPath, string geoShaderPath, string name)
        {
            Shader shader = LoadShaderFromFile(vertexShaderPath, fragShaderPath, geoShaderPath);
            _shaderDict.Add(name, shader);
        }

        internal Texture GetTexture(string name)
        {
            return _textureDict[name];
        }

        internal Texture LoadTexture(string path, bool alpha, string name)
        {
            Texture tex = LoadTextureFromFile(path, alpha);
            _textureDict.Add(name, tex);
            return tex;
        }

        internal void Clear()
        {
            //TODO: Call this when application is destroyed
            foreach (KeyValuePair<string, Shader> entry in _shaderDict)
            {
                GL.DeleteProgram(entry.Value.ProgramId);
            }

            foreach (KeyValuePair<string, Texture> entry in _textureDict)
            {
                GL.DeleteTexture(entry.Value.TextureId);   
            }
        }

        private Texture LoadTextureFromFile(string path, bool alpha)
        {
            throw new NotImplementedException();
        }

        private Shader LoadShaderFromFile(string vertexShaderPath, string fragShaderPath, string geoShaderPath)
        {
            string strVertexShader = null, strFragmentShader = null, strGeoShader = null;
            using (var streamReader = new StreamReader(vertexShaderPath, Encoding.UTF8))
            {
                strVertexShader = streamReader.ReadToEnd();
            }

            using (var streamReader = new StreamReader(fragShaderPath, Encoding.UTF8))
            {
                strFragmentShader = streamReader.ReadToEnd();
            }

            if (geoShaderPath != null)
            {
                using (var streamReader = new StreamReader(geoShaderPath, Encoding.UTF8))
                {
                    strGeoShader = streamReader.ReadToEnd();
                }
            }

            return new Shader(strVertexShader, strFragmentShader, strGeoShader);
        }
    }
}
