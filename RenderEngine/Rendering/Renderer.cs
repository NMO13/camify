﻿using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using RenderEngine.Rendering.Scene;
using Shared.Geometry;

namespace RenderEngine.Rendering
{
    class Renderer
    {
        private readonly List<IRenderable> _renderMeshes, _perpetualMeshes;
        public Renderer(List<IRenderable> renderMeshes, List<IRenderable> perpetualMeshes)
        {
            _renderMeshes = renderMeshes;
            _perpetualMeshes = perpetualMeshes;
        }

        public void Render()
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            foreach (var perpetual in _perpetualMeshes)
            {
                perpetual.Render();
            }

            PolygonMode polyMode = SceneModel.Instance.WireframeMode ? PolygonMode.Line : PolygonMode.Fill;
            GL.PolygonMode(MaterialFace.FrontAndBack, polyMode);
            foreach (var mesh in _renderMeshes)
            {
                mesh.Render();
            }
        }
    }
}
