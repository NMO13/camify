using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GraphicsEngine.HalfedgeMesh;
using Shared.Geometry;

namespace GraphicsEngine.HalfedgeMeshProcessing
{
    internal class ContourGroup
    {
        internal List<Contour> Contours = new List<Contour>();
        internal List<HeFace> InsideFaces = new List<HeFace>();
        internal Vector3m Normal;
        internal int Index;

        internal void CalculateContours(List<HeHalfedge> halfedges)
        {
            if (halfedges.Count < 3)
                throw new Exception("Invalid halfedge size");

            HeHalfedge cur;
            var j = InitNewContour(halfedges, out cur);
            bool isClosed = true;

            for (int i = 0; i < halfedges.Count; i++)
            {
                isClosed = false;
                var next = halfedges[i];
                if (cur.Twin.Origin.Equals(next.Origin))
                {
                    List<HeHalfedge> nextEdges = new List<HeHalfedge>();
                    if (HasMultipleNextedges(next, halfedges, nextEdges))
                    {
                        //TODO why not GetNearestNextedge?
                        next = GetNearestNextedge(cur.Twin, nextEdges);
                        i = halfedges.FindIndex(x => x == next);
                    }
                    halfedges.RemoveAt(i);
                    Contours[j].HeList.Add(next);
                    cur = next;
                    i = -1;
                }
                if (cur.Twin.Origin.Equals(Contours[j].HeList[0].Origin) && !FindNext(cur.Twin.Origin, halfedges)) // contour is closed now => start a new one
                {
                    isClosed = true;
                    if (halfedges.Count == 0)
                        break;
                    j = InitNewContour(halfedges, out cur);
                }
            }
            if (halfedges.Count > 0)
                throw new Exception("The contours are not correctly calculated");
            if (!isClosed)
                throw new Exception("The contours are not closed");
        }

        private bool FindNext(HeVertex origin, List<HeHalfedge> halfedges)
        {
            var index = halfedges.FindIndex(x => x.Origin.Equals(origin));
            if (index >= 0)
                return true;
            return false;
        }

        private int InitNewContour(List<HeHalfedge> halfedges, out HeHalfedge first)
        {
            var curve = new Contour();
            Contours.Add(curve);
            first = halfedges.First();
            curve.HeList.Add(first);
            halfedges.RemoveAt(0);
            return Contours.Count - 1;
        }

        private HeHalfedge GetNearestNextedge(HeHalfedge cur, List<HeHalfedge> nextEdges)
        {
            foreach (var he in cur.GetHalfedgeCirculator())
            {
                foreach (var heHalfedge in nextEdges)
                {
                    if (heHalfedge == he)
                        return heHalfedge;
                }
            }
            throw new Exception("No fitting edge found");
        }

        private bool HasMultipleNextedges(HeHalfedge next, List<HeHalfedge> heSplitLines, List<HeHalfedge> nextEdges)
        {
            foreach (var heSplitLine in heSplitLines)
            {
                if (heSplitLine.Origin.Equals(next.Origin))
                    nextEdges.Add(heSplitLine);
            }
            Debug.Assert(nextEdges.Count >= 1);
            return nextEdges.Count > 1;
        }
    }
}
