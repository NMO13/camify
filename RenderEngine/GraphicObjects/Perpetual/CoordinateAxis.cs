using System;
using OpenTK.Graphics.OpenGL;
using RenderEngine.Rendering;
using Shared.Geometry;
using Shared.Import;

namespace RenderEngine.GraphicObjects.Perpetual
{
    class CoordinateAxis : RenderObject
    {
        protected override Shader Shader { get; set; }
        protected override BufferUsageHint BufferUsage { get; }
        internal override Vertex[] Vertices { get; set; }
        internal override bool HasNormals { get; set; }
        public override void Render(bool wireframe)
        {
            MeshImporter importer = new MeshImporter();
           // importer.GenerateMeshes(Config.)
            throw new NotImplementedException();
        }
    }
}
