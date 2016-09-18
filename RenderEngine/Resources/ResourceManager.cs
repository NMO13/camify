using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace RenderEngine.Resources
{
    class ResourceManager
    {
        private readonly Dictionary<string, Shader.Shader> _shaderDict = new Dictionary<string, Shader.Shader>(); 
        private readonly Dictionary<string, Texture.Texture> _textureDict = new Dictionary<string, Texture.Texture>();
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
        internal Shader.Shader GetShader(string name)
        {
            return _shaderDict[name];
        }

        internal void LoadShader(string vertexShaderPath, string fragShaderPath, string geoShaderPath, string name)
        {
            Shader.Shader shader = LoadShaderFromFile(vertexShaderPath, fragShaderPath, geoShaderPath);
            _shaderDict.Add(name, shader);
        }

        internal Texture.Texture LoadTextureFromFile(bool alpha, string name)
        {
            Texture.Texture tex;
            if (alpha)
            {
                tex = new Texture.Texture(PixelInternalFormat.Rgba, PixelFormat.Rgba);
            }
            else
            {
                tex = new Texture.Texture();
            }

            Bitmap bitmap = new Bitmap(name);
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            if(bmpData == null)
                throw new Exception("Bitmap could not be loaded.");

            byte[] byteArray = new byte[Math.Abs(bmpData.Stride * bmpData.Height)];
            Marshal.Copy(bmpData.Scan0, byteArray, 0, byteArray.Length);

            tex.Generate(bmpData.Width, bmpData.Height, byteArray);
            bitmap.UnlockBits(bmpData);
            bitmap.Dispose();

            _textureDict.Add(name, tex);
            return tex;
        }

        internal Texture.Texture GetTexture(byte[] pattern, int width, int height, string name)
        {
            if (!_textureDict.ContainsKey(name))
            {
                return CreateOneChannelTexture(pattern, width, height, name);
            }

            return _textureDict[name];
        }

        private Texture.Texture CreateOneChannelTexture(byte[] pattern, int width, int height, string name)
        {
            Texture.Texture tex = new Texture.Texture(PixelInternalFormat.Luminance, PixelFormat.Luminance, PixelType.UnsignedByte,
                    TextureMinFilter.Nearest, TextureMagFilter.Nearest);

            tex.Generate(width, height, pattern);
            _textureDict.Add(name, tex);
            return tex;
        }

        private Shader.Shader LoadShaderFromFile(string vertexShaderPath, string fragShaderPath, string geoShaderPath)
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

            return new Shader.Shader(strVertexShader, strFragmentShader, strGeoShader);
        }
    }
}
