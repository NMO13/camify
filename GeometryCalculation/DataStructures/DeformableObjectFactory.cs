﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Geometry;

namespace GeometryCalculation.DataStructures
{
    public class DeformableObjectFactory
    {
        public static DeformableObject Create(Mesh mesh, uint bvhMaxItemCount = 2)
        {
            DeformableObject obj = new DeformableObject(bvhMaxItemCount);
            obj.Initialize(mesh);
            obj.CheckSanity();
            return obj;
        }
    }
}
