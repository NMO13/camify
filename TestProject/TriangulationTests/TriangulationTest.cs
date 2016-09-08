using System.Collections.Generic;
using System.Linq;
using GraphicsEngine.Geometry.Triangulation;
using NUnit.Framework;
using Shared.Geometry;

namespace TestProject.TriangulationTests
{
    [TestFixture]
    class TriangulationTest
    {
        private void TestPoints(List<Vector3m> points, List<Vector3m> testPoints)
        {
            foreach (var testPoint in testPoints)
            {
                Assert.IsTrue(points.Contains(testPoint));            
            }
        }

        private void TestFaces(List<Vector3m> points, Vector3m normal)
        {
            Assert.AreEqual(points.Count % 3, 0);
            for (int i = 0; i < points.Count; i += 3)
            {
                var a = new Vector3m(points[i].X, points[i].Y, points[i].Z);
                var b = new Vector3m(points[i+1].X, points[i+1].Y, points[i+1].Z);
                var c = new Vector3m(points[i+2].X, points[i+2].Y, points[i+2].Z);
                var res = (b - a).Cross(c - a);
                Assert.IsTrue(res.X.Sign == normal.X.Sign && res.Y.Sign == normal.Y.Sign && res.Z.Sign == normal.Z.Sign);
                
            }
        }

        [Test]
        public void EarClipping1()
        {
            Vector3m[] points = {new Vector3m(0, 0, 0), new Vector3m(1, 0, 0), new Vector3m(0, 1, 0)};
            EarClipping earClipping = new EarClipping();
            earClipping.SetPoints(new List<Vector3m>(points));
            earClipping.Triangulate();
            var res = earClipping.Result;
            Assert.AreEqual(res.Count, 3);
            TestPoints(points.ToList(), res);
            TestFaces(res, (points[1] - points[0]).Cross(points[2] - points[0]));
        }

        [Test]
        public void EarClipping2()
        {
            Vector3m[] points = { new Vector3m(0, 0, 0), new Vector3m(1, 0, 0), new Vector3m(1, 1, 0), new Vector3m(0, 1, 0) };
            EarClipping earClipping = new EarClipping();
            earClipping.SetPoints(new List<Vector3m>(points));
            earClipping.Triangulate();
            var res = earClipping.Result;
            Assert.AreEqual(res.Count, 6);
            TestPoints(points.ToList(), res);
            TestFaces(res, (points[1] - points[0]).Cross(points[2] - points[0]));
        }

        [Test]
        [ExpectedException]
        public void EarClipping3()
        {
            EarClipping earClipping = new EarClipping();
            earClipping.Triangulate();
        }

        [Test]
        public void EarClipping4()
        {
            Vector3m[] points = { new Vector3m(0, 0, 0), new Vector3m(1, -5, 0), new Vector3m(2, -1, 0), new Vector3m(-1, 3, 0), new Vector3m(-2, 0, 0), new Vector3m(-1, -2, 0) };
            EarClipping earClipping = new EarClipping();
            earClipping.SetPoints(new List<Vector3m>(points));
            earClipping.Triangulate();
            var res = earClipping.Result;
            Assert.AreEqual(res.Count, 12);
            TestPoints(points.ToList(), res);
            TestFaces(res, (points[1] - points[0]).Cross(points[2] - points[0]));
        }

        [Test]
        public void EarClipping5()
        {
            Vector3m[] points = { new Vector3m(2, 3, 0), new Vector3m(0, 0, 0), new Vector3m(1, -2, 0), new Vector3m(2, 0, 0), new Vector3m(3, -1, 0), new Vector3m(4, 0, 0) };
            EarClipping earClipping = new EarClipping();
            earClipping.SetPoints(new List<Vector3m>(points));
            earClipping.Triangulate();
            var res = earClipping.Result;
            Assert.AreEqual(res.Count, 12);
            TestPoints(points.ToList(), res);
            TestFaces(res, (points[1] - points[0]).Cross(points[2] - points[0]));
        }

        [Test]
        public void EarClipping6()
        {
            Vector3m[] points = { new Vector3m(3, 0, 0), new Vector3m(5, 0, 0), new Vector3m(3, 2, 0), new Vector3m(0, 0, 0) };
            EarClipping earClipping = new EarClipping();
            earClipping.SetPoints(new List<Vector3m>(points));
            earClipping.Triangulate();
            var res = earClipping.Result;
            Assert.AreEqual(res.Count, 6);
            TestPoints(points.ToList(), res);
            TestFaces(res, (points[1] - points[0]).Cross(points[2] - points[0]));
        }

        [Test]
        [ExpectedException]
        public void EarClipping7()
        {
            Vector3m[] points = { new Vector3m(3, 0, 0), new Vector3m(5, 0, 0), new Vector3m(7, 0, 0) };
            EarClipping earClipping = new EarClipping();
            earClipping.SetPoints(new List<Vector3m>(points));
            earClipping.Triangulate();
        }


        [Test]
        public void EarClipping8()
        {
            Vector3m[] points = { new Vector3m(2, -2, 0), new Vector3m(2, 0, 0), new Vector3m(1, 0, 0), new Vector3m(0, 0, 0), new Vector3m(-1, 0, 0), new Vector3m(-2, 0, 0) };
            EarClipping earClipping = new EarClipping();
            earClipping.SetPoints(new List<Vector3m>(points));
            earClipping.Triangulate();
            var res = earClipping.Result;
            Assert.AreEqual(res.Count, 12);
            TestPoints(points.ToList(), res);
            TestFaces(res, (points[1] - points[0]).Cross(points[2] - points[0]));
        }

        [Test]
        public void EarClipping9()
        {
            Vector3m[] points = { new Vector3m(-1, 1, 0), new Vector3m(-1, -1, 0), new Vector3m(1, -1, 0), new Vector3m(1, 1, 0), new Vector3m(0, 1, 0), new Vector3m(0.25, 0.75, 0), new Vector3m(-0.25, 0.75, 0), new Vector3m(0, 1, 0) };
            List<List<Vector3m>> holes = new List<List<Vector3m>>();

            EarClipping earClipping = new EarClipping();
            var pointList = points.ToList();
            earClipping.SetPoints(pointList, holes);
            earClipping.Triangulate();
            var res = earClipping.Result;
            Assert.AreEqual(res.Count, 18);
            Assert.AreEqual(pointList.Count, 8);
            TestFaces(res, (points[1] - points[0]).Cross(points[2] - points[0]));
        }

        [Test]
        public void EarClippingWHole()
        {
            Vector3m[] points = { new Vector3m(0, 0, 0), new Vector3m(4, 0, 0), new Vector3m(4, 4, 0), new Vector3m(0, 4, 0) };
            List<List<Vector3m>> holes = new List<List<Vector3m>>();
            Vector3m[] hole = { new Vector3m(1, 1, 0), new Vector3m(1, 3, 0), new Vector3m(3, 3, 0), new Vector3m(3, 1, 0) };
            holes.Add(hole.ToList());

            EarClipping earClipping = new EarClipping();
            var pointList = points.ToList();
            earClipping.SetPoints(pointList, holes);
            earClipping.Triangulate();
            var res = earClipping.Result;
            Assert.AreEqual(res.Count, 24);
            Assert.AreEqual(pointList.Count, 4);
            Assert.AreEqual(holes[0].Count, 4);
            TestFaces(res, (points[1] - points[0]).Cross(points[2] - points[0]));
        }

        [Test]
        public void EarClippingW2Holes2()
        {
            Vector3m[] points =
            {
                new Vector3m(0, 0, 0), new Vector3m(10, 0, 0), new Vector3m(10, 4, 0), new Vector3m(8, 4, 0),
                new Vector3m(8, 2, 0), new Vector3m(6, 2, 0), new Vector3m(6, 4, 0), new Vector3m(3, 4, 0),
                new Vector3m(3, 6, 0), new Vector3m(0, 6, 0)
            };

            List<List<Vector3m>> holes = new List<List<Vector3m>>();
            Vector3m[] hole = { new Vector3m(1, 3, 0), new Vector3m(1, 4, 0), new Vector3m(2, 4, 0), new Vector3m(2, 3, 0)};
            holes.Add(hole.ToList());
            Vector3m[] hole2 = { new Vector3m(1, 1, 0), new Vector3m(1, 2, 0), new Vector3m(2, 2, 0), new Vector3m(2, 1, 0) };
            holes.Add(hole2.ToList());

            EarClipping earClipping = new EarClipping();
            var pointList = points.ToList();
            earClipping.SetPoints(pointList, holes);
            earClipping.Triangulate();
            var res = earClipping.Result;
            Assert.AreEqual(res.Count, 60);
            Assert.AreEqual(pointList.Count, 10);
            Assert.AreEqual(holes[0].Count, 4);
            Assert.AreEqual(holes[1].Count, 4);
            TestFaces(res, (points[1] - points[0]).Cross(points[2] - points[0]));
        }

        [Test]
        public void EarClippingW2Holes3()
        {
            Vector3m[] points =
            {
                new Vector3m(0, 0, 0), new Vector3m(10, 0, 0), new Vector3m(10, 4, 0), new Vector3m(8, 4, 0),
                new Vector3m(8, 2, 0), new Vector3m(6, 2, 0), new Vector3m(6, 4, 0), new Vector3m(3, 4, 0),
                new Vector3m(3, 6, 0), new Vector3m(0, 6, 0)
            };

            List<List<Vector3m>> holes = new List<List<Vector3m>>();
            Vector3m[] hole = { new Vector3m(2, 4, 0), new Vector3m(2, 3, 0), new Vector3m(1, 3, 0), new Vector3m(1, 4, 0) };
            holes.Add(hole.ToList());
            Vector3m[] hole2 = { new Vector3m(2, 1, 0), new Vector3m(1, 1, 0), new Vector3m(1, 2, 0), new Vector3m(2, 2, 0) };
            holes.Add(hole2.ToList());

            EarClipping earClipping = new EarClipping();
            var pointList = points.ToList();
            earClipping.SetPoints(pointList, holes);
            earClipping.Triangulate();
            var res = earClipping.Result;
            Assert.AreEqual(res.Count, 60);
            Assert.AreEqual(pointList.Count, 10);
            Assert.AreEqual(holes[0].Count, 4);
            Assert.AreEqual(holes[1].Count, 4);
            TestFaces(res, (points[1] - points[0]).Cross(points[2] - points[0]));
        }

        [Test]
        public void EarClippingW2Holes4()
        {
            Vector3m[] points =
            {
                new Vector3m(-4, -4, 0), new Vector3m(4, -4, 0), new Vector3m(4, 4, 0), new Vector3m(-4, 4, 0)
            };

            List<List<Vector3m>> holes = new List<List<Vector3m>>();
            Vector3m[] hole =
            {
                new Vector3m(-3, -2, 0), new Vector3m(-3, 2, 0), new Vector3m(0, -1, 0), new Vector3m(3, 2, 0),
                new Vector3m(3, -2, 0)
            };
            holes.Add(hole.ToList());
            Vector3m[] hole2 =
            {
                new Vector3m(0, 0, 0), new Vector3m(-1, 1, 0), new Vector3m(1, 1, 0)
            };
            holes.Add(hole2.ToList());

            EarClipping earClipping = new EarClipping();
            var pointList = points.ToList();
            earClipping.SetPoints(pointList, holes);
            earClipping.Triangulate();
            var res = earClipping.Result;
            Assert.AreEqual(res.Count, 42);
            Assert.AreEqual(pointList.Count, 4);
            Assert.AreEqual(holes[0].Count, 5);
            Assert.AreEqual(holes[1].Count, 3);
            TestFaces(res, (points[1] - points[0]).Cross(points[2] - points[0]));
        }

        [Test]
        public void NonConvexRegion1()
        {
            Vector3m[] points =
            {
                new Vector3m(0, 0, 0), new Vector3m(5, 1, 0), new Vector3m(3, 3, 0), new Vector3m(5, 6, 0),
                new Vector3m(5, 4, 0), new Vector3m(7, 5, 0), new Vector3m(6, 3, 0), new Vector3m(10, 1, 0),
                new Vector3m(7, 7, 0), new Vector3m(0, 8, 0)
            };

            List<List<Vector3m>> holes = new List<List<Vector3m>>();
            Vector3m[] hole = { new Vector3m(1, 5, 0), new Vector3m(1, 6, 0), new Vector3m(3, 6, 0) };
            holes.Add(hole.ToList());

            EarClipping earClipping = new EarClipping();
            var pointList = points.ToList();
            earClipping.SetPoints(pointList, holes);
            earClipping.Triangulate();
            var res = earClipping.Result;
            Assert.AreEqual(res.Count, 39);
            TestFaces(res, (points[1] - points[0]).Cross(points[2] - points[0]));
        }

        [Test]
        public void NonConvexRegion2()
        {
            Vector3m[] points =
            {
                new Vector3m(0, 0, 0), new Vector3m(9, 0, 0), new Vector3m(9, 6, 0), new Vector3m(7, 3, 0),
                new Vector3m(6, 5, 0), new Vector3m(4, 4, 0), new Vector3m(6, 7, 0), new Vector3m(0, 7, 0)
            };

            List<List<Vector3m>> holes = new List<List<Vector3m>>();
            Vector3m[] hole = { new Vector3m(2, 2, 0), new Vector3m(2, 3, 0), new Vector3m(4, 2, 0) };
            holes.Add(hole.ToList());

            EarClipping earClipping = new EarClipping();
            var pointList = points.ToList();
            earClipping.SetPoints(pointList, holes);
            earClipping.Triangulate();
            var res = earClipping.Result;
            Assert.AreEqual(res.Count, 33);
            TestFaces(res, (points[1] - points[0]).Cross(points[2] - points[0]));
        }

        [Test]
        public void NonConvexRegion3()
        {
            Vector3m[] points =
            {
                new Vector3m(0, 0, 0), new Vector3m(9, 0, 0), new Vector3m(9, 6, 0), new Vector3m(8, 4, 0),
                new Vector3m(8, 6, 0), new Vector3m(7, 3, 0), new Vector3m(6, 5, 0), new Vector3m(4, 4, 0),
                new Vector3m(6, 7, 0), new Vector3m(0, 7, 0)
            };

            List<List<Vector3m>> holes = new List<List<Vector3m>>();
            Vector3m[] hole = { new Vector3m(4, 2, 0), new Vector3m(4, 3, 0), new Vector3m(5, 2, 0) };
            holes.Add(hole.ToList());

            Vector3m[] hole2 = { new Vector3m(2, 2, 0), new Vector3m(1, 2, 0), new Vector3m(1, 3, 0) };
            holes.Add(hole2.ToList());

            EarClipping earClipping = new EarClipping();
            var pointList = points.ToList();
            earClipping.SetPoints(pointList, holes);
            earClipping.Triangulate();
            var res = earClipping.Result;
            Assert.AreEqual(res.Count, 54);
            TestFaces(res, (points[1] - points[0]).Cross(points[2] - points[0]));
        }

        [Test]
        public void MIPTriangleReversed()
        {
            Vector3m[] points =
            {
                new Vector3m(-2, 3, 0), new Vector3m(1, -5, 0), new Vector3m(5, 1, 0), new Vector3m(5, -1, 0), new Vector3m(6, 1, 0),
                new Vector3m(6, -2, 0), new Vector3m(7, 0, 0), new Vector3m(9, -2, 0), new Vector3m(8, 3, 0)
            };

            List<List<Vector3m>> holes = new List<List<Vector3m>>();
            Vector3m[] hole = { new Vector3m(0, -1, 0), new Vector3m(0, 2, 0), new Vector3m(2, 2, 0) };

            holes.Add(hole.ToList());
            EarClipping earClipping = new EarClipping();
            var pointList = points.ToList();
            earClipping.SetPoints(pointList, holes);
            earClipping.Triangulate();
            var res = earClipping.Result;
            Assert.AreEqual(res.Count, 36);
            TestFaces(res, (points[1] - points[0]).Cross(points[2] - points[0]));
        }

        [Test]
        public void Bug1()
        {
            Vector3m[] points =
            {
                new Vector3m(200, 200, 0), new Vector3m(-200, 200, 0), new Vector3m(-200, -200, 0), new Vector3m(200, -200, 0)
            };

            List<List<Vector3m>> holes = new List<List<Vector3m>>();
            Vector3m[] hole = { new Vector3m(136.692260742188, 145.482269287109, 0), new Vector3m(20.0013608932495, 28.7913694381714, 0), new Vector3m(-96.6895389556885, 145.482269287109, 0) };


            Vector3m[] hole2 =
            {
                new Vector3m(185.483631134033, 96.6909122467041, 0),
                new Vector3m(185.483631134033, -165.482269287109, 0),
                new Vector3m(-145.480911254883, -165.482269287109, 0),
                new Vector3m(-145.480911254883, 96.6909198760986, 0),
                new Vector3m(-3.62770080566406, -45.1622905731201, 0),
                new Vector3m(-2.00111961364746, -46.5963306427002, 0),
                new Vector3m(-0.287700653076172, -47.9253902435303, 0),
                new Vector3m(1.50579071044922, -49.1442604064941, 0),
                new Vector3m(3.37227058410645, -50.2480907440186, 0),
                new Vector3m(5.30438041687012, -51.2325496673584, 0),
                new Vector3m(7.29448986053467, -52.09375, 0),
                new Vector3m(9.33473968505859, -52.8283004760742, 0),
                new Vector3m(11.4170970916748, -53.4332809448242, 0),
                new Vector3m(13.533332824707, -53.9063186645508, 0),
                new Vector3m(15.6750922203064, -54.245548248291, 0),
                new Vector3m(17.8339250087738, -54.4496116638184, 0),
                new Vector3m(20.0013114210451, -54.5177307128906, 0),
                new Vector3m(22.1686990261078, -54.4496116638184, 0),
                new Vector3m(24.3275308609009, -54.2455596923828, 0),
                new Vector3m(26.4692912101746, -53.9063415527344, 0),
                new Vector3m(28.5855255126953, -53.4333000183105, 0),
                new Vector3m(30.6678800582886, -52.8283309936523, 0),
                new Vector3m(32.70814037323, -52.0937881469727, 0),
                new Vector3m(34.6982498168945, -51.2326107025146, 0),
                new Vector3m(36.6303806304932, -50.248140335083, 0),
                new Vector3m(38.4968490600586, -49.1443099975586, 0),
                new Vector3m(40.290340423584, -47.9254608154297, 0),
                new Vector3m(42.0037593841553, -46.5963802337646, 0),
                new Vector3m(43.6303405761719, -45.1623706817627, 0),
                new Vector3m(45.1636791229248, -43.6290397644043, 0)
            };
            holes.Add(hole2.ToList());
            holes.Add(hole.ToList());
            EarClipping earClipping = new EarClipping();
            var pointList = points.ToList();
            earClipping.SetPoints(pointList, holes);
            earClipping.Triangulate();
            var res = earClipping.Result;
            Assert.AreEqual(res.Count, 117);
            TestFaces(res, (points[1] - points[0]).Cross(points[2] - points[0]));
        }

        [Test]
        public void PointOnEdge()
        {
            Vector3m[] points =
            {
                new Vector3m(0, 3, 0), new Vector3m(1, 0, 0), new Vector3m(2, 3, 0), new Vector3m(3, 0, 0), new Vector3m(4, 3, 0)
            };

            EarClipping earClipping = new EarClipping();
            var pointList = points.ToList();
            earClipping.SetPoints(pointList);
            earClipping.Triangulate();
            var res = earClipping.Result;
            Assert.AreEqual(res.Count, 6);
            TestFaces(res, (points[1] - points[0]).Cross(points[2] - points[0]));
        }

        [Test]
        public void PointPointHole()
        {
            Vector3m[] points =
            {
                new Vector3m(-1, -1, 0), new Vector3m(1, -1, 0), new Vector3m(1, 1, 0), new Vector3m(-1, 1, 0)
            };

            List<List<Vector3m>> holes = new List<List<Vector3m>>();
            Vector3m[] hole = { new Vector3m(-0.75, 0.25, 0), new Vector3m(-0.75, 0.75, 0), new Vector3m(-0.5, 0.75, 0) };
            holes.Add(hole.ToList());

            Vector3m[] hole2 =
            {
                new Vector3m(0.5, 0.25, 0), new Vector3m(0.25, 0.75, 0), new Vector3m(0.25, 0.25, 0),
                new Vector3m(0, 0.25, 0), new Vector3m(0.25, 0.75, 0), new Vector3m(0.75, 0.5, 0),
            };
            holes.Add(hole2.ToList());

            EarClipping earClipping = new EarClipping();
            var pointList = points.ToList();
            earClipping.SetPoints(pointList, holes);
            earClipping.Triangulate();
            var res = earClipping.Result;
            Assert.AreEqual(res.Count, 45);
            TestFaces(res, (points[1] - points[0]).Cross(points[2] - points[0]));
        }

        [Test]
        public void Polygon3D()
        {
            Vector3m[] points =
            {
                new Vector3m(0, 0, 0), new Vector3m(5, 0, 0), new Vector3m(5, 5, 5), new Vector3m(3, 3, 3), new Vector3m(2, 6, 6), new Vector3m(1, 3, 3), new Vector3m(0, 5, 5)
            };

            List<List<Vector3m>> holes = new List<List<Vector3m>>();
            Vector3m[] hole = { new Vector3m(2, 3.5, 3.5), new Vector3m(1.5, 3.5, 3.5), new Vector3m(2, 4, 4) };
            holes.Add(hole.ToList());

            EarClipping earClipping = new EarClipping();
            var pointList = points.ToList();
            earClipping.SetPoints(pointList, holes);
            earClipping.Triangulate();
            var res = earClipping.Result;
            Assert.AreEqual(res.Count, 30);
            TestFaces(res, (points[1] - points[0]).Cross(points[2] - points[0]));
        }
    }
}
