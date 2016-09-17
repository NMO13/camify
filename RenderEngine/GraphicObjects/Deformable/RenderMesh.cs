using OpenTK.Graphics.OpenGL;
using RenderEngine.Camera;
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

        private Shader NormalVisualizationShader { get; } =
            ResourceManager.Instance.GetShader(ShaderLibrary.ShaderName.NormalVisualization.ToString());

        private byte[] bayerMatrix =  {
            0, 32,  8, 40,  2, 34, 10, 42,   /* 8x8 Bayer ordered dithering  */
            48, 16, 56, 24, 50, 18, 58, 26,  /* pattern.  Each input pixel   */
            12, 44,  4, 36, 14, 46,  6, 38,  /* is scaled to the 0..63 range */
            60, 28, 52, 20, 62, 30, 54, 22,  /* before looking in this table */
            3, 35, 11, 43,  1, 33,  9, 41,   /* to determine the action.     */
            51, 19, 59, 27, 49, 17, 57, 25,
            15, 47,  7, 39, 13, 45,  5, 37,
            63, 31, 55, 23, 61, 29, 53, 21 };

        internal RenderMesh(Vertex[] vertices, Material material, bool hasNormals, LightBundle lightBundle) : base(vertices, hasNormals)
        {
            Shader = ResourceManager.Instance.GetShader(ShaderLibrary.ShaderName.Mesh.ToString());
            LightBundle = lightBundle;
            Material = material;

            Vertices = vertices;
            HasNormals = hasNormals;
        }

        public override void Render(bool wireframe)
        {
            PolygonMode polyMode = wireframe ? PolygonMode.Line : PolygonMode.Fill;
            GL.PolygonMode(MaterialFace.FrontAndBack, polyMode);
            GL.Enable(EnableCap.DepthTest);
            Shader.Use();

            GLCheck.Call(DeployLightConstants);
            GLCheck.Call(DeployMaterial);

            Shader.SetMatrix4("view", SceneModel.Instance.WorldTransformationMatrix);
            Shader.SetMatrix4("proj", SceneModel.Instance.ProjectionMatrix);
            Shader.SetUniform2("win_scale", SceneModel.Instance.SceneWidth, SceneModel.Instance.SceneHeight);

            Texture bayerTexture = ResourceManager.Instance.GetTexture(bayerMatrix, 8, 8, "bayerTex");
            bayerTexture.Bind();
            DrawMesh();

            if (SceneModel.Instance.ShowNormals)
            {
                NormalVisualizationShader.Use();
                NormalVisualizationShader.SetMatrix4("view", SceneModel.Instance.WorldTransformationMatrix);
                NormalVisualizationShader.SetMatrix4("proj", SceneModel.Instance.ProjectionMatrix);
                NormalVisualizationShader.SetUniform1("magnitude", Config.Magnitude);
                DrawMesh();
            }
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
                Shader.SetUniform3("pointLights[" + i  + "].position", pLight.PosX, pLight.PosY, zPosition);
                Shader.SetUniform3("pointLights[" + i + "].ambient", pLight.AmbientR, pLight.AmbientG, pLight.AmbientB);
                Shader.SetUniform3("pointLights[" + i + "].diffuse", pLight.DiffuseR, pLight.DiffuseG, pLight.DiffuseB);
                Shader.SetUniform3("pointLights[" + i + "].specular", pLight.SpecularR, pLight.SpecularG, pLight.SpecularB);
            }
        }
    }
}
