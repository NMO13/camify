using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using RenderEngine.Resources.Shader;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

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

        internal Texture LoadTextureFromFile(bool alpha, string name)
        {
            Texture tex;
            if (alpha)
            {
                tex = new Texture(PixelInternalFormat.Rgba, PixelFormat.Rgba);
            }
            else
            {
                tex = new Texture();
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

        internal Texture GetOneChannelTexture(byte[] pattern, int width, int height, string name)
        {
            Texture tex = new Texture(PixelInternalFormat.Luminance, PixelFormat.Luminance, PixelType.UnsignedByte,
                TextureMinFilter.Nearest, TextureMagFilter.Nearest);

            tex.Generate(width, height, pattern);
            _textureDict.Add(name, tex);

            return tex;
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
