using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using RenderEngine.Camera;
using RenderEngine.Conversion;
using RenderEngine.ErrorHandling;
using RenderEngine.GraphicObjects.ObjectTypes.Dynamic;
using RenderEngine.Lighting;
using RenderEngine.Rendering.Scene;
using RenderEngine.Resources;
using RenderEngine.Resources.Shader;
using RenderEngine.Resources.StaticObjects;
using Shared.Assets;
using Shared.Geometry;
using Shared.Geometry.HalfedgeMesh;
using Shared.Import;

namespace RenderEngine.GraphicObjects.ObjectTypes.Static
{
    internal sealed class CoordinateSystemPart : StaticRenderObject
    {
        private Material Material { get; }
        private LightBundle LightBundle { get; }

        internal CoordinateSystemPart(Mesh mesh, Material material)
        {
            Vertices = LoadVertices(mesh);
            LightBundle = new LightBundle();
            Material =  material;
            Shader = ResourceManager.Instance.GetShader(ShaderLibrary.ShaderName.CoordinateAxis.ToString());
            Setup();
        }

        private Vertex[] LoadVertices(Mesh mesh)
        {
            HeMesh heMesh = new HeMesh(mesh);
            var meshNew = new Mesh(heMesh);
            return meshNew.RenderVertices;
        }

        public override void Render(bool wireframe)
        {
            GL.Viewport(0, 0, 90, 90);
            GL.Enable(EnableCap.DepthTest);
            Shader.Use();

            GLCheck.Call(DeployLightConstants);
            GLCheck.Call(DeployMaterial);

            Matrix4d view = Matrix4d.Mult(SceneModel.Instance.LookAtMatrix,
                Matrix4d.CreateTranslation(0, 0, -40));
            view = Matrix4d.Mult(SceneModel.Instance.RotationMatrix, view);
            Shader.SetMatrix4("view", view);

            Matrix4d proj = Matrix4d.CreatePerspectiveFieldOfView((float) (0.45*1.5), 1, 1, 2000);
            Shader.SetMatrix4("proj", proj);

            DrawMesh();
        }

        private void DeployMaterial()
        {
            Shader.SetUniform1("material.shininess", Material.Shininess);
            Shader.SetUniform3("material.ambient", Material.AmbientR, Material.AmbientG, Material.AmbientB);
            Shader.SetUniform3("material.diffuse", Material.DiffuseR, Material.DiffuseG, Material.DiffuseB);
            Shader.SetUniform3("material.specular", Material.SpecularR, Material.SpecularG, Material.SpecularB);

        }

        private void DeployLightConstants()
        {
            foreach (var dirLight in LightBundle.DirectionalLights)
            {
                Shader.SetUniform3("dirLight.direction", dirLight.DirX, dirLight.DirY, dirLight.DirZ);
                Shader.SetUniform3("dirLight.ambient", dirLight.AmbientR, dirLight.AmbientG, dirLight.AmbientB);
                Shader.SetUniform3("dirLight.diffuse", dirLight.DiffuseR, dirLight.DiffuseG, dirLight.DiffuseB);
                Shader.SetUniform3("dirLight.specular", dirLight.SpecularR, dirLight.SpecularG, dirLight.SpecularB);
            }

            for (int i = 0; i < LightBundle.PointLights.Count; i++)
            {
                Shader.SetInteger("numPointLights", LightBundle.PointLights.Count);

                PointLight pLight = LightBundle.PointLights[i];
                float zPosition = pLight.PosZ + Objective.CurZoom;
                Shader.SetUniform3("pointLights[" + i + "].position", pLight.PosX, pLight.PosY, zPosition);
                Shader.SetUniform3("pointLights[" + i + "].ambient", pLight.AmbientR, pLight.AmbientG, pLight.AmbientB);
                Shader.SetUniform3("pointLights[" + i + "].diffuse", pLight.DiffuseR, pLight.DiffuseG, pLight.DiffuseB);
                Shader.SetUniform3("pointLights[" + i + "].specular", pLight.SpecularR, pLight.SpecularG, pLight.SpecularB);
            }
        }

    }
}
