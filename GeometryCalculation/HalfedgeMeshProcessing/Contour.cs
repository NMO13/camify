using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsEngine.HalfedgeMesh;

namespace GraphicsEngine.HalfedgeMeshProcessing
{
    class Contour
    {
        internal List<HeHalfedge> HeList = new List<HeHalfedge>();

        internal bool Merge(HeHalfedge cl0, HeHalfedge cl1, HeHalfedge newLine)
        {
            Debug.Assert(HeList.Count >= 3);
            for (int i = 0; i < HeList.Count; i++)
            {
                var contourLine = HeList[i];
                if (contourLine == cl0)
                {
                    Debug.Assert(HeList[(i + 1) % HeList.Count] == cl1);
                    HeList.RemoveAt(i);
                    if (i == HeList.Count)
                        i = 0;
                    HeList.RemoveAt(i);
                    HeList.Insert(i, newLine);
                    Debug.Assert(HeList.Count >= 3);
                    return true;
                }
            }
            return false;
        }
    }
}
