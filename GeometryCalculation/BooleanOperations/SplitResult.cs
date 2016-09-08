using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GraphicsEngine.HalfedgeMesh;
using GraphicsEngine.Math;
using Shared.Geometry.HalfedgeMesh;

namespace GraphicsEngine.Geometry.Boolean_Ops
{
    internal class SplitResult
    {
        internal List<HeFace> InsideFaces = new List<HeFace>();
        internal bool IsA = false;
        // Needs to be a hashset since the same split edge could be added twice. But we dont allow duplicates
        internal HashSet<HeHalfedge> Splitlines = new HashSet<HeHalfedge>();
        internal SplitResult Other;

        internal SplitResult()
        {
            Clear();
        }

        internal void Clear()
        {
            InsideFaces.Clear();
            Splitlines.Clear();
            Other = null;
        }
    }
}
