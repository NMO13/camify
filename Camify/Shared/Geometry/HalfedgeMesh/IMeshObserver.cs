﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEngine.HalfedgeMesh
{
    interface IMeshObserver
    {

        void VertexAdded(HeVertex v, HeMesh source);

        void FaceAdded(HeFace face, HeMesh source);

        void VertexDeleted(HeVertex v, HeMesh source);

        void FaceDeleted(HeFace face, HeMesh source);
    }
}