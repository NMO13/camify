﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using RenderEngine.Rendering;

namespace RenderEngine.Objects
{
    sealed class CoordinateAxis : RenderObject
    {
        protected override Shader Shader { get; set; }
        protected override BufferUsageHint BufferUsage { get; }
        protected override Vertex[] Vertices { get; }
        protected override uint[] Indices { get; }

        public override void Render()
        {
            throw new NotImplementedException();
        }
    }
}
