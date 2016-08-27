using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderEngine.Resources.Shader;

namespace RenderEngine
{
    class ResourceManager
    {
        private Dictionary<string, Shader> _shaderDict = new Dictionary<string, Shader>(); 
        private Dictionary<string, Texture> _textureDict = new Dictionary<string, Texture>();
        internal Shader GetShader(string name)
        {
            return _shaderDict[name];
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

        internal void LoadShader(string vertexShaderPath, string fragShaderPath, string name)
        {
            Shader shader = LoadShaderFromFile(vertexShaderPath, fragShaderPath);
            _shaderDict.Add(name, shader);
        }

        internal void Clear()
        {
            
        }

        private Texture LoadTextureFromFile(string path, bool alpha)
        {
            throw new NotImplementedException();
        }

        private Shader LoadShaderFromFile(string vertexShaderPath, string fragShaderPath, string geoShaderPath)
        {
            string vertexShader, fragmentShader;
            using (var streamReader = new StreamReader(vertexShaderPath, Encoding.UTF8))
            {
                vertexShader = streamReader.ReadToEnd();
            }

            using (var streamReader = new StreamReader(fragShaderPath, Encoding.UTF8))
            {
                fragmentShader = streamReader.ReadToEnd();
            }

            return null;
        }
    }
}
