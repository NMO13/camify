using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GeometryCalculation.DataStructures;
using GraphicsEngine.Geometry.CollisionCheck;
using GraphicsEngine.HalfedgeMesh;
using Shared.Additional;
using Shared.Geometry;

namespace GraphicsEngine.Geometry.Boolean_Ops
{
    public static class BooleanModeller
    {
        public static void Subtract(DeformableObject objA, DeformableObject objB, bool invokeMulithreaded = false)
        {
            SplitResult splitResultA = new SplitResult();
            SplitResult splitResultB = new SplitResult();

            splitResultA.IsA = true;
            splitResultB.IsA = false;
            splitResultA.Other = splitResultB;
            splitResultB.Other = splitResultA;

            var bMeshNew = new HeMesh(objB.HeMesh, Vector3m.Zero());
            if (!Split(objA, objB, splitResultA, splitResultB, invokeMulithreaded))
                return;
            AlignSplitLines(objA, objB, splitResultA, splitResultB);
            RemoveDegenerateSplitLines(splitResultA);
            RemoveDegenerateSplitLines(splitResultB);
#if DEBUG
            GetSplitCurves(objA, splitResultA);
#endif
            ClassifyInside(objA, splitResultA);
            ClassifyInside(objB, splitResultB);
            RemoveInside(objA, splitResultA);
            AddOutside(objA, objB, splitResultA, splitResultB);
            objB.HeMesh = bMeshNew;
            objB.BuildBvh();
            objA.BuildBvh();
            ResetState(objA);
        }

        public static void SubtractSweptVolume(DeformableObject objA, DeformableObject objB, bool invokeMulithreaded = false)
        {
            SplitResult splitResultA = new SplitResult();
            SplitResult splitResultB = new SplitResult();

            splitResultA.IsA = true;
            splitResultB.IsA = false;
            splitResultA.Other = splitResultB;
            splitResultB.Other = splitResultA;

            if (!Split(objA, objB, splitResultA, splitResultB, invokeMulithreaded))
                return;
            AlignSplitLines(objA, objB, splitResultA, splitResultB);
            RemoveDegenerateSplitLines(splitResultA);
            RemoveDegenerateSplitLines(splitResultB);
#if DEBUG
            GetSplitCurves(objA, splitResultA); // Calculating the split curves is not necessary and should be removed in production; it is just for testing here
#endif
            ClassifyInside(objA, splitResultA);
            ClassifyInside(objB, splitResultB);
            RemoveInside(objA, splitResultA);
            AddOutside(objA, objB, splitResultA, splitResultB);
            objA.ExecutePostProcesses();
            objA.BuildBvh();
            ResetState(objA);
            //objA.ResetVertexValues(); Currently, this does not work since this could create odd faces
        }

        internal static bool Split(DeformableObject objA, DeformableObject objB, SplitResult splitResultA, SplitResult splitResultB, bool invokeMulithreaded)
        {
            IntersectionResult intersectionResultA = new IntersectionResult();
            IntersectionResult intersectionResultB = new IntersectionResult();
            // 1. find split curve
            // It is possible that the split curve of A and B are not symmetric: At coplanar faces, the split curve
            // of A -> B and B -> A may be different
            if (invokeMulithreaded)
            {
                if (objA.Intersect(objB) && new SharedSplitcurveCalculator().FindSplitcurveParallel(objA.FacePairs, objA.HeMesh, intersectionResultA, intersectionResultB))
                {
                    Parallel.Invoke(() => new FaceSplitting().Split(intersectionResultA, objA.HeMesh, splitResultA), () => new FaceSplitting().Split(intersectionResultB, objB.HeMesh, splitResultB));
                    return true;
                }
            }
            else
            {
                if (objA.Intersect(objB) && new SharedSplitcurveCalculator().FindSplitcurve(objA.FacePairs, objA.HeMesh, intersectionResultA, intersectionResultB))
                {
                    //// 2. split A's faces using split curve
                    new FaceSplitting().Split(intersectionResultA, objA.HeMesh, splitResultA);
                    //// 3. split B's faces using split curve
                    new FaceSplitting().Split(intersectionResultB, objB.HeMesh, splitResultB);
                    return true;
                }
            }


            return false;
        }

        internal static void RemoveInside(DeformableObject objA, SplitResult splitResultA)
        {
            for (int i = 0; i < splitResultA.InsideFaces.Count; i++)
            {
                var insideFace = splitResultA.InsideFaces[i];
                objA.HeMesh.RemoveFace(insideFace, true);
            }
        }

        internal static void AddOutside(DeformableObject objA, DeformableObject objB, SplitResult splitResultA, SplitResult splitResultB)
        {
            foreach (var insideFaceB in splitResultB.InsideFaces)
            {
                var v0 = insideFaceB.OuterComponent.Origin;
                var v2 = insideFaceB.OuterComponent.Next.Origin;
                var v1 = insideFaceB.OuterComponent.Next.Next.Origin;

                int index0 = -1;
                int index1 = -1;
                int index2 = -1;

                foreach (var vertexA in objA.HeMesh.VertexList)
                {
                    if (vertexA.Equals(v0))
                        index0 = vertexA.Index;
                    if (vertexA.Equals(v1))
                        index1 = vertexA.Index;
                    if (vertexA.Equals(v2))
                        index2 = vertexA.Index;
                    if (index0 != -1 && index1 != -1 && index2 != -1)
                        break;
                }

                if (index0 == -1)
                {
                    var vertex = objA.HeMesh.AddVertex(new HeVertex(v0.X, v0.Y, v0.Z));
                    index0 = vertex.Index;
                }
                if (index1 == -1)
                {
                    var vertex = objA.HeMesh.AddVertex(new HeVertex(v1.X, v1.Y, v1.Z));
                    index1 = vertex.Index;
                }
                if (index2 == -1)
                {
                    var vertex = objA.HeMesh.AddVertex(new HeVertex(v2.X, v2.Y, v2.Z));
                    index2 = vertex.Index;
                }
                
                Debug.Assert(index0 != index1 && index1 != index2);
                Debug.Assert(index0 != -1 && index1 != -1 && index2 != -1);
                objA.HeMesh.AddFace(index0, index1, index2);

            }
        }

        internal static void ResetState(DeformableObject obj)
        {
            foreach (var halfedge in obj.HeMesh.HalfedgeList)
            {
                halfedge.SetSplitline(false);
            }
            obj.FacePairs.Clear();
        }

        internal static void AlignSplitLines(DeformableObject a, DeformableObject b, SplitResult splitResultA, SplitResult splitResultB)
        {
            // take splitline of A
            // find all splitlines between A and A1
            // remove them from splitlinesA
            // find a splitline B which has A1 as starting and all splitlines until a vertex exists which has A as ending
            // Align these splitlines
            // start again

            var splitLinesA = new List<HeHalfedge>(splitResultA.Splitlines);
            var splitLinesB = new List<HeHalfedge>(splitResultB.Splitlines);
            splitResultA.Splitlines.Clear();
            splitResultB.Splitlines.Clear();

            while (splitLinesA.Count > 0)
            {
                var slA = FindNextSharedVertex(splitLinesA, splitLinesB);
                List<HeHalfedge> sharedSplitlinesA = GoUntilSharedVertex(slA, splitLinesA, splitLinesB);

                var hesB = FindHesWithSharedVertex(sharedSplitlinesA[sharedSplitlinesA.Count - 1].Twin, splitLinesB);
                List<HeHalfedge> sharedSplitlinesB = null;
                foreach (var heb in hesB)
                {
                    sharedSplitlinesB = GoUntilSharedVertex(heb, splitLinesB, splitLinesA);
                    if (sharedSplitlinesA[0].Origin.Equals(sharedSplitlinesB[sharedSplitlinesB.Count - 1].Twin.Origin))
                        if (
                            sharedSplitlinesA[sharedSplitlinesA.Count - 1].Twin.Origin.Equals(
                                sharedSplitlinesB[0].Origin))
                            break;
                }

                sharedSplitlinesA.ForEach(x => splitLinesA.Remove(x));
                sharedSplitlinesB.ForEach(x => splitLinesB.Remove(x));

                Debug.Assert(sharedSplitlinesA[0].Origin.Equals(sharedSplitlinesB[sharedSplitlinesB.Count - 1].Twin.Origin));
                Debug.Assert(sharedSplitlinesA[sharedSplitlinesA.Count - 1].Twin.Origin.Equals(sharedSplitlinesB[0].Origin));

                //Align here
                AlignSplitLine(sharedSplitlinesA, sharedSplitlinesB, a, b, splitLinesA, splitLinesB, splitResultA, splitResultB);
            }

            if(splitLinesA.Count != 0 || splitLinesB.Count != 0)
                throw new Exception();
        }

        private static void AlignSplitLine(List<HeHalfedge> sharedSplitlinesA, List<HeHalfedge> sharedSplitlinesB, DeformableObject a, DeformableObject b,
            List<HeHalfedge> splitLinesA, List<HeHalfedge> splitLinesB, SplitResult splitResultA, SplitResult splitResultB)
        {
            int aIndex = 0;
            int bIndex = sharedSplitlinesB.Count - 1;
            do
            {
                var heA = sharedSplitlinesA[aIndex++];
                var heB = sharedSplitlinesB[bIndex--];
                splitResultA.Splitlines.Add(heA);
                splitResultB.Splitlines.Add(heB);

                Debug.Assert(heA.Origin.Equals(heB.Twin.Origin));
                Debug.Assert(heA.IsSplitLine);
                Debug.Assert(heB.IsSplitLine);

                if (!heA.Twin.Origin.Vector3m.Equals(heB.Origin.Vector3m))
                {
                    var lA = heA.Vector3D.LengthSquared();
                    var lB = heB.Vector3D.LengthSquared();

                    Debug.Assert(!lA.IsZero);
                    Debug.Assert(!lB.IsZero);

                    Debug.Assert(heB.IncidentFace != null);
                    Debug.Assert(heA.IncidentFace != null);

                    if (lA > lB) // split heA
                    {
                        SplitFaces(heA, heB.Origin, a, sharedSplitlinesA, aIndex-1, aIndex, splitLinesA, splitResultA);
                        //bIndex--;
                    }
                    else // lA < lB
                    {
                        SplitFaces(heB, heA.Twin.Origin, b, sharedSplitlinesB, bIndex+1, bIndex+2, splitLinesB, splitResultB);
                        bIndex++;
                        //aIndex++;
                    }
                }
            } while (aIndex < sharedSplitlinesA.Count);

            Debug.Assert(aIndex == sharedSplitlinesA.Count && bIndex == -1);
        }

        private static void SplitFaces(HeHalfedge he, HeVertex heVertex, DeformableObject obj, List<HeHalfedge> sharedSplitlines, int index0, int index1, List<HeHalfedge> splitLines, SplitResult splitResult)
        {
            var v = new HeVertex(heVertex.X, heVertex.Y, heVertex.Z);
            v = obj.HeMesh.AddVertexUnique(v, he.Origin, he.Twin.Origin);
            var faces = obj.HeMesh.SplitFaceInTwo(he, v);
            sharedSplitlines.RemoveAt(index0);
            sharedSplitlines.Insert(index0, faces[0].OuterComponent);
            sharedSplitlines.Insert(index1, faces[1].OuterComponent);
            obj.HeMesh.SplitFaceInTwo(he.Twin, v);

            splitResult.Splitlines.Remove(he);
            splitResult.Splitlines.Add(faces[0].OuterComponent);
            splitResult.Splitlines.Add(faces[1].OuterComponent);

            if (he.Twin.IsSplitLine && splitLines.Contains(he.Twin))
            {
                splitLines.Remove(he.Twin);
                splitLines.Add(faces[0].OuterComponent.Twin);
                splitLines.Add(faces[1].OuterComponent.Twin);

                splitResult.Splitlines.Remove(he.Twin);
                splitResult.Splitlines.Add(faces[0].OuterComponent.Twin);
                splitResult.Splitlines.Add(faces[1].OuterComponent.Twin);
            }
        }

        private static List<HeHalfedge> FindHesWithSharedVertex(HeHalfedge heA, List<HeHalfedge> splitLinesB)
        {
            List<HeHalfedge> list = new List<HeHalfedge>();
            foreach (var halfedge in splitLinesB)
            {
                if(halfedge.Origin.Equals(heA.Origin))
                    list.Add(halfedge);
            }
            return list;
        }

        private static List<HeHalfedge> GoUntilSharedVertex(HeHalfedge sharedVertex, List<HeHalfedge> splitLinesA, List<HeHalfedge> splitlinesB)
        {
            var list = new List<HeHalfedge>();
            list.Add(sharedVertex);
            var cur = sharedVertex;
            do
            {
                if (IsSharedVertex(cur.Twin.Origin, splitlinesB))
                    return list;
                cur = FindNextSplitEdge(cur, splitLinesA);
                list.Add(cur);
            } while (true);
        }

        private static HeHalfedge FindNextSplitEdge(HeHalfedge cur, List<HeHalfedge> splitLinesA)
        {
            foreach (var heHalfedge in splitLinesA)
            {
                // find an edge which has cur's end point as start point but we don't want the twin of cur
                if (cur.Twin.Origin.Equals(heHalfedge.Origin) && !cur.Twin.Equals(heHalfedge))
                    return heHalfedge;
            }
            throw new Exception();
        }

        private static bool IsSharedVertex(HeVertex v, List<HeHalfedge> splitlinesB)
        {
            foreach (var heHalfedge in splitlinesB)
            {
                if (heHalfedge.Origin.Equals(v) || heHalfedge.Twin.Origin.Equals(v))
                    return true;
            }
            return false;
        }

        private static HeHalfedge FindNextSharedVertex(List<HeHalfedge> splitlinesA, List<HeHalfedge> splitlinesB)
        {
            foreach (var heHalfedge in splitlinesA)
            {
                if (IsSharedVertex(heHalfedge.Origin, splitlinesB))
                    return heHalfedge;
            }
            return null;
        }

        internal static void ClassifyInside(DeformableObject obj, SplitResult splitResult)
        {
            Stack<HeHalfedge> stack = new Stack<HeHalfedge>();
            var splitlines = splitResult.Splitlines.ToList();

            while (splitlines.Count > 0) // for each split curve
            {
                stack.Push(splitlines[0]);

                while (stack.Count > 0)
                {
                    var he = stack.Pop();
                    if (!he.IncidentFace.DynamicProperties.ExistsKey(PropertyConstants.Marked))
                    {
                        splitResult.InsideFaces.Add(he.IncidentFace);
                        he.IncidentFace.DynamicProperties.AddProperty(PropertyConstants.Marked, true);
                    }

                    CheckHe(he, stack, splitlines);
                    CheckHe(he.Next, stack, splitlines);
                    CheckHe(he.Next.Next, stack, splitlines);
                }
            }
        }

        private static void CheckHe(HeHalfedge he, Stack<HeHalfedge> stack, List<HeHalfedge> splitlines)
        {
            if (!he.IsSplitLine)
            {
                if (!he.Twin.IncidentFace.DynamicProperties.ExistsKey(PropertyConstants.Marked))
                    stack.Push(he.Twin);
            }
            else
            {
                splitlines.Remove(he);
            }
        }

        internal static List<Splitcurve> GetSplitCurves(DeformableObject obj, SplitResult splitResult)
        {
            List<Splitcurve> Splitcurves = new List<Splitcurve>();
            var heSplitLines = new List<HeHalfedge>(splitResult.Splitlines);
            if (heSplitLines.Count == 0)
                return Splitcurves;

            HeHalfedge cur;
            var j = InitNewCurve(heSplitLines, out cur, Splitcurves);
            bool isClosed = true;

            for (int i = 0; i < heSplitLines.Count; i++)
            {
                isClosed = false;
                var next = heSplitLines[i];
                if (cur.Next.Origin.Equals(next.Origin))
                {
                    List<HeHalfedge> nextEdges = new List<HeHalfedge>();
                    if (HasMultipleNextedges(next, heSplitLines, nextEdges))
                    {
                        next = GetNearestNextedge(cur.Next, nextEdges);
                        i = heSplitLines.FindIndex(x => x == next);
                    }
                    heSplitLines.RemoveAt(i);
                    Splitcurves[j].splitLines.Add(next);
                    cur = next;
                    i = -1;
                }
                if (cur.Next.Origin.Equals(Splitcurves[j].splitLines[0].Origin)) // split curve is closed now => start a new one
                {
                    isClosed = true;
                    if (heSplitLines.Count == 0)
                        break;
                    j = InitNewCurve(heSplitLines, out cur, Splitcurves);
                }
            }
            TestIfCurvesValid(Splitcurves);
            if (heSplitLines.Count > 0)
                throw new Exception("The split curves are not correct");
            if (!isClosed)
                throw new Exception("Split curves are not closed");
            return Splitcurves;
        }

        private static void TestIfCurvesValid(List<Splitcurve> splitcurves)
        {
            foreach (var splitcurve in splitcurves)
            {
                if (splitcurve.splitLines.Count < 3)
                {
                    throw new Exception("Split curve has too few lines");
                }
            }
        }

        internal static void RemoveDegenerateSplitLines(SplitResult splitResult)
        {
            var splitlines = new List<HeHalfedge>(splitResult.Splitlines);
            List<HeHalfedge> degenerates = new List<HeHalfedge>();
            foreach (var splitLine in splitlines)
            {
                Debug.Assert(splitLine.IsSplitLine);
                if (splitLine.Twin.IsSplitLine && splitlines.Find(x => x.Index == splitLine.Twin.Index) != null)                {
                    degenerates.Add(splitLine);
                }
            }
            Debug.Assert(degenerates.Count % 2 == 0);

            // we remove all "degenerate split lines" because RemoveInside() would remove wrong faces if we kept them
            foreach (var se in degenerates)
            {
                splitResult.Splitlines.Remove(se);
                se.SetSplitline(false);
            }
        }

        private static int InitNewCurve(List<HeHalfedge> splitLines, out HeHalfedge first, List<Splitcurve> splitcurves)
        {
            var curve = new Splitcurve();
            splitcurves.Add(curve);
            first = splitLines.First();
            curve.splitLines.Add(first);
            splitLines.RemoveAt(0);
            return splitcurves.Count - 1;
        }
        private static HeHalfedge GetNearestNextedge(HeHalfedge cur, List<HeHalfedge> nextEdges)
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

        private static bool HasMultipleNextedges(HeHalfedge next, List<HeHalfedge> heSplitLines, List<HeHalfedge> nextEdges)
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

    internal class Splitcurve
    {
        internal List<HeHalfedge> splitLines = new List<HeHalfedge>();
    }
}
