using GeometryCalculation.DataStructures;
using GraphicsEngine.Geometry;
using GraphicsEngine.Geometry.Boolean_Ops;
using GraphicsEngine.HalfedgeMesh;
using NUnit.Framework;
using Shared.Geometry;

namespace BooleanSubractorTests
{
    static class TestConfigurator
    {
        internal static bool CheckSanity = true;
        internal static bool Multithreaded = false;
    }

    class BooleanTester
    {
        internal void Test(DeformableObject objA, DeformableObject objB, bool split, int splitCurveCount,
            int resultVertexCount, int resultHalfedgeCount, int resultFaceCount)
        {
            SetObjects(objA, objB);
            var bMeshNew = new HeMesh(b.HeMesh, Vector3m.Zero());
            SplitResult splitResultA = new SplitResult();
            SplitResult splitResultB = new SplitResult();

            splitResultA.IsA = true;
            splitResultB.IsA = false;
            splitResultA.Other = splitResultB;
            splitResultB.Other = splitResultA;

            Assert.AreEqual(split, BooleanModeller.Split(a, b, splitResultA, splitResultB, TestConfigurator.Multithreaded));
            BooleanModeller.AlignSplitLines(a, b, splitResultA, splitResultB);
            BooleanModeller.RemoveDegenerateSplitLines(splitResultA);
            BooleanModeller.RemoveDegenerateSplitLines(splitResultB);
            TestSplitCurves(split, -1, splitResultA, splitResultB);
            BooleanModeller.ClassifyInside(a, splitResultA);
            BooleanModeller.ClassifyInside(b, splitResultB);
            BooleanModeller.RemoveInside(a, splitResultA);
            TestConditionSplitLine(a);
            BooleanModeller.AddOutside(a, b, splitResultA, splitResultB);
            BooleanModeller.ResetState(a);
            b.HeMesh = bMeshNew;
            CheckCountsA(resultVertexCount, resultHalfedgeCount, resultFaceCount);
            TestFramework.IsReset(a.HeMesh);
            TestFramework.IsReset(b.HeMesh);
            if (TestConfigurator.CheckSanity)
            {
                TestFramework.CheckSanity(a);
                TestFramework.CheckSanity(b);
            }
        }

        private DeformableObject a, b;

        internal void CheckCountsA(int vertexCount, int halfedgeCount, int faceCount)
        {
            Assert.AreEqual(vertexCount, a.HeMesh.VertexList.Count);
            Assert.AreEqual(halfedgeCount, a.HeMesh.HalfedgeList.Count);
            Assert.AreEqual(faceCount, a.HeMesh.FaceList.Count);
        }

        internal static void TestConditionSplitLine(DeformableObject a)
        {
            foreach (var halfedge in a.HeMesh.HalfedgeList)
            {
                if (halfedge.IsSplitLine)
                    Assert.True(halfedge.IncidentFace == null);
                else
                {
                    Assert.True(halfedge.IncidentFace != null);
                }
            }
        }

        internal void TestSplitCurves(bool split, int curveCount, SplitResult splitResultA, SplitResult splitResultB)
        {
            if (split)
            {
                var sca = BooleanModeller.GetSplitCurves(a, splitResultA);
                var scb = BooleanModeller.GetSplitCurves(b, splitResultB);
            }
        }

        internal void SetObjects(DeformableObject objA, DeformableObject objB)
        {
            this.a = objA;
            this.b = objB;
            a.CheckSanity();
            b.CheckSanity();
        }   
    }
}
