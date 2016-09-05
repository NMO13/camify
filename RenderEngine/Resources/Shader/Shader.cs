﻿using System;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL;
using RenderEngine.Conversion;
using Shared.Geometry;

namespace RenderEngine
{
    class Shader
    {
        internal int ProgramId { get; }

        internal void Use()
        {
            GL.UseProgram(ProgramId);
        }
        internal Shader(string vertexShader, string fragmentShader, string geoShader)
        {
            int success;

            //vertex shader
            int vertexShaderIndex = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShaderIndex, vertexShader);
            GL.CompileShader(vertexShaderIndex);
            GL.GetShader(vertexShaderIndex, ShaderParameter.CompileStatus, out success);

            if (success == 0)
            {
                Console.WriteLine(GL.GetShaderInfoLog(vertexShaderIndex));
            }

            //fragment shader
            int fragmentShaderIndex = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShaderIndex, fragmentShader);
            GL.CompileShader(fragmentShaderIndex);
            GL.GetShader(fragmentShaderIndex, ShaderParameter.CompileStatus, out success);

            if (success == 0)
            {
                Console.WriteLine(GL.GetShaderInfoLog(fragmentShaderIndex));
            }

            //geometry shader
            int geoShaderIndex = -1;
            if (geoShader != null)
            {
                geoShaderIndex = GL.CreateShader(ShaderType.GeometryShader);
                GL.ShaderSource(geoShaderIndex, geoShader);
                GL.CompileShader(geoShaderIndex);
                GL.GetShader(geoShaderIndex, ShaderParameter.CompileStatus, out success);
            }

            // Shader Program
            string programInfoLog;
            ProgramId = GL.CreateProgram();
            GL.AttachShader(ProgramId, vertexShaderIndex);
            GL.AttachShader(ProgramId, fragmentShaderIndex);
            GL.LinkProgram(ProgramId);
            GL.GetProgramInfoLog(ProgramId, out programInfoLog);
            Debug.WriteLine(programInfoLog);

            // Delete the shaders as they're linked into our program now and no longer necessery
            GL.DeleteShader(vertexShaderIndex);
            GL.DeleteShader(fragmentShaderIndex);
            if(geoShader != null)
                GL.DeleteShader(geoShaderIndex);
        }

        internal void SetInteger(string name, int val)
        {
            GL.Uniform1(GL.GetUniformLocation(ProgramId, name), val);
        }

        internal void SetMatrix4(string name, Matrix4d matrix)
        {
            OpenTK.Matrix4 openTKMatrix = matrix.ToOpenTKMatrix4D();
            GL.UniformMatrix4(GL.GetUniformLocation(ProgramId, name), false, ref openTKMatrix);
        }

        internal void SetUniform1(string name, float x)
        {
            GL.Uniform1(GL.GetUniformLocation(ProgramId, name), x);
        }

        internal void SetUniform3(string name, float x, float y, float z)
        {
            GL.Uniform3(GL.GetUniformLocation(ProgramId, name), x, y, z);
        }
    }
}
