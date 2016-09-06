using OpenTK.Graphics.OpenGL;
using RenderEngine.Lighting;
using RenderEngine.Rendering;
using RenderEngine.Rendering.Scene;
using RenderEngine.Resources.Shader;
using Shared.Assets;


namespace RenderEngine.GraphicObjects.Deformable
{
    sealed class RenderMesh : RenderObject
    {
        protected override Shader Shader { get; set; }
        protected override BufferUsageHint BufferUsage => BufferUsageHint.DynamicDraw;
        internal override Vertex[] Vertices { get; set; }
        internal override bool HasNormals { get; set; }
        private LightBundle LightBundle { get; }
        private Material Material { get;  }

        internal RenderMesh(Vertex[] vertices, Material material, bool hasNormals, LightBundle lightBundle) : base(vertices, hasNormals)
        {
            Shader = ResourceManager.Instance.GetShader(ShaderLibrary.ShaderName.Mesh.ToString());
            LightBundle = lightBundle;
            Material = material;

            Vertices = vertices;
            HasNormals = hasNormals;
        }

        public override void Render()
        {
            GL.Enable(EnableCap.DepthTest);
            Shader.Use();

            DeployLightConstants();
            DeployMaterial(); 

            Shader.SetMatrix4("view", SceneModel.Instance.WorldTransformationMatrix);
            Shader.SetMatrix4("proj", SceneModel.Instance.ProjectionMatrix);

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
                Shader.SetUniform3("pointLights[" + i  + "].position", pLight.PosX, pLight.PosY, pLight.PosZ);
                Shader.SetUniform3("pointLights[" + i + "].ambient", pLight.AmbientR, pLight.AmbientG, pLight.AmbientB);
                Shader.SetUniform3("pointLights[" + i + "].diffuse", pLight.DiffuseR, pLight.DiffuseG, pLight.DiffuseB);
                Shader.SetUniform3("pointLights[" + i + "].specular", pLight.SpecularR, pLight.SpecularG, pLight.SpecularB);
            }
        }
    }
}
