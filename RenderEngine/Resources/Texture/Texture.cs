using System;
using OpenTK.Graphics.OpenGL;

namespace RenderEngine
{
    class Texture
    {
        private int TextureId { get; set; }
        private int _width, _height;
        private PixelInternalFormat _pixelInternalFormat;
        private PixelFormat _pixelFormat;
        private PixelType _pixelType;
        private TextureMinFilter _filterMin;
        private TextureMagFilter _filterMag;

        internal Texture(PixelInternalFormat internalFormat = PixelInternalFormat.Rgb, PixelFormat pixelFormat = PixelFormat.Rgb, 
            PixelType pixelType = PixelType.UnsignedByte, TextureMinFilter filterMin = TextureMinFilter.Linear, TextureMagFilter filterMag = TextureMagFilter.Linear)
        {
            TextureId = GL.GenTexture();
            _pixelInternalFormat = internalFormat;
            _pixelFormat = pixelFormat;
            _pixelType = pixelType;
            _filterMin = filterMin;
            _filterMag = filterMag;
        }

        internal void Generate(int width, int height, byte[] data)
        {
            _width = width;
            _height = height;

            //Create texture
            GL.BindTexture(TextureTarget.Texture2D, TextureId);
            GLCheck.Call(() => GL.TexImage2D(TextureTarget.Texture2D, 0, _pixelInternalFormat, _width, _height, 0, _pixelFormat, _pixelType, data));
            //Set texture wrap and filter modes
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)_filterMin);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)_filterMag);
            //Unbind
            GLCheck.Call(() => GL.BindTexture(TextureTarget.Texture2D, 0));

        }

        internal void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, TextureId);
        }

        internal void Delete()
        {
            GL.DeleteTexture(TextureId);
        }
    }
}
