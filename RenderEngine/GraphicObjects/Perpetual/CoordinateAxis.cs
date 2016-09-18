using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using RenderEngine.Conversion;
using RenderEngine.GraphicObjects.Deformable;
using RenderEngine.Rendering;
using RenderEngine.Resources.Meshmodel;
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
            List<RenderMesh> rendermeshes = Converter.ToRenderMeshes(importer.GenerateMeshes(PerpetualMeshLibrary.CoordinateAxisMesh));
        }
    }
}
