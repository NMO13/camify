using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooleanSubractorTests;
using GeometryCalculation.BooleanOperations;
using GeometryCalculation.DataStructures;
using GraphicsEngine;
using GraphicsEngine.Geometry;
using GraphicsEngine.Geometry.Boolean_Ops;
using GraphicsEngine.Geometry.Meshes;
using GraphicsEngine.HalfedgeMesh;
using NUnit.Framework;
using Shared.Geometry;
using Shared.Helper;

namespace BooleanSubtractorTests
{
    [TestFixture]
    class MeshSimplificationTest
    {
        private BooleanTester _bTester;

        [SetUp]
        public void Setup()
        {
            _bTester = new BooleanTester();
        }

        [Test]
        [ExpectedException]
        public void ThreePointsOnLine()
        {
            Vector3d[] defaultBoxVertices =
            {
                new Vector3d(-1, 0, 1),
                new Vector3d(0.0000000000000000000000000000000000000000000001f, 0, 0),
                new Vector3d(-1, 0, -1),
                new Vector3d(1, 0, 1),
                new Vector3d(0, 1, 0)
            };

            int[] defaultBoxCoordinates =
	        {
                0, 2, 3,
                1, 3, 2,
                4, 0, 3,
                4, 3, 1,
                4, 1, 2,
                4, 2, 0
  
	        };
            Mesh mesh = new Mesh(defaultBoxVertices, defaultBoxCoordinates, null);

            DeformableObject obj = new DeformableObject();
            obj.Initialize(mesh);

            obj.CheckSanity();
        }


        public void EdgeReduction1()
        {
            Vector3d[] defaultBoxVertices =
            {
                new Vector3d(-1, -1, 0),
                new Vector3d(1, -1, 0),
                new Vector3d(2, 0, 0),
                new Vector3d(3, 1, 0),
                new Vector3d(2, 2, 0),
                new Vector3d(1, 3, 0),
                new Vector3d(0, 2, 0),
                new Vector3d(-1, 1, 0),
                new Vector3d(1, 1, 0),
                new Vector3d(1, -1, -1),
                new Vector3d(2, 0, -1),
                new Vector3d(3, 1, -1)
            };

            int[] defaultBoxCoordinates =
            {
                0, 1, 7,
                1, 8, 7,
                1, 2, 8,
                8, 6, 7,
                2, 3, 4,
                4, 5, 6,
                4, 6, 8,
                2, 4, 8,
                1, 9, 2,
                9, 10, 2,
                10, 11, 2,
                2, 11, 3,
                1, 0, 9,
                0, 7, 9,
                9, 7, 6,
                9, 6, 5,
                9, 5, 10,
                10, 5, 11,
                11, 5, 4,
                11, 4, 3
            };

            Mesh mesh = new Mesh(defaultBoxVertices, defaultBoxCoordinates, null);
            DeformableObject obj = new DeformableObject();
            obj.Initialize(mesh);
            obj.CheckSanity();
            HeVertex v = obj.HeMesh.VertexList[8];
            HeHalfedge h0 = obj.HeMesh.HalfedgeList[12];
            HeHalfedge h1 = obj.HeMesh.HalfedgeList[13];

            int oldCount = obj.HeMesh.FaceList.Count;
            obj.HeMesh.Collapse(h1.Index);
            int newCount = obj.HeMesh.FaceList.Count;

            Assert.AreEqual(oldCount - 2, newCount);
            Assert.IsTrue(v.Index == -1);
            Assert.IsTrue(h0.Index == -1);
            Assert.IsTrue(h1.Index == -1);
        }


        //Uses same values as in EdgeReduction1
        public void EdgeReduction2()
        {
            Vector3d[] defaultBoxVertices =
            {
                new Vector3d(-1, -1, 0),
                new Vector3d(1, -1, 0),
                new Vector3d(2, 0, 0),
                new Vector3d(3, 1, 0),
                new Vector3d(2, 2, 0),
                new Vector3d(1, 3, 0),
                new Vector3d(0, 2, 0),
                new Vector3d(-1, 1, 0),
                new Vector3d(1, 1, 0),
                new Vector3d(1, -1, -1),
                new Vector3d(2, 0, -1),
                new Vector3d(3, 1, -1)
            };

            int[] defaultBoxCoordinates =
            {
                0, 1, 7,
                1, 8, 7,
                1, 2, 8,
                8, 6, 7,
                2, 3, 4,
                4, 5, 6,
                4, 6, 8,
                2, 4, 8,
                1, 9, 2,
                9, 10, 2,
                10, 11, 2,
                2, 11, 3,
                1, 0, 9,
                0, 7, 9,
                9, 7, 6,
                9, 6, 5,
                9, 5, 10,
                10, 5, 11,
                11, 5, 4,
                11, 4, 3
            };

            Mesh mesh = new Mesh(defaultBoxVertices, defaultBoxCoordinates, null);
            DeformableObject obj = new DeformableObject();
            obj.Initialize(mesh);
            obj.CheckSanity();

            int oldCount = obj.HeMesh.FaceList.Count;
            obj.HeMesh.Collapse(49);
            int newCount = obj.HeMesh.FaceList.Count;

            Assert.AreEqual(oldCount - 2, newCount);
            obj.CheckSanity();
        }


        public void MelaxReduction1()
        {
            Vector3d[] defaultBoxVertices =
            {
                new Vector3d(-1, -1, 0),
                new Vector3d(1, -1, 0),
                new Vector3d(2, 0, 0),
                new Vector3d(3, 1, 0),
                new Vector3d(2, 2, 0),
                new Vector3d(1, 3, 0),
                new Vector3d(0, 2, 0),
                new Vector3d(-1, 1, 0),
                new Vector3d(1, 1, 0),
                new Vector3d(-0.922f, 1.03f, -0.01f),
                new Vector3d(2, 0, -1),
                new Vector3d(3, 1, -1)
            };

            int[] defaultBoxCoordinates =
            {
                0, 1, 7,
                1, 8, 7,
                1, 2, 8,
                8, 6, 7,
                2, 3, 4,
                4, 5, 6,
                4, 6, 8,
                2, 4, 8,
                1, 9, 2,
                9, 10, 2,
                10, 11, 2,
                2, 11, 3,
                1, 0, 9,
                0, 7, 9,
                9, 7, 6,
                9, 6, 5,
                9, 5, 10,
                10, 5, 11,
                11, 5, 4,
                11, 4, 3
            };

            Mesh mesh = new Mesh(defaultBoxVertices, defaultBoxCoordinates, null);

            DeformableObject obj = new DeformableObject();
            obj.Initialize(mesh);

            //Simplifier s = new Simplifier();
            //s.AddStrategy(new MelaxSimplification(0.05f, obj.HeMesh));
        }


        public void TinyTriangleAfterSplitting()
        {
            var cube = DefaultMeshes.Box(Single.Epsilon, Single.Epsilon, Single.Epsilon);
            DeformableObject obj = new DeformableObject();
            obj.Initialize(cube);
            obj.CheckSanity();

            var cube2 = DefaultMeshes.Box(Single.Epsilon, Single.Epsilon, Single.Epsilon);
            var obj2 = new DeformableObject();
            obj2.Initialize(cube2);
            obj2.CheckSanity();
            obj2.TranslateAndBuildBvh(new Vector3m(Single.Epsilon, 0, 0));

            BooleanModeller.SubtractSweptVolume(obj, obj2);
        }

        [Test]
        public void SimpleCube()
        {
            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\MeshSimplificationTest\\SimpleCube.dae");
            Mesh mesh = meshes[0];
            DeformableObject obj = new DeformableObject();
            obj.Initialize(mesh);
            obj.AddPostProcessStep(DeformableObject.PostprocessAlgorithm.PlanarMerge);
            obj.ExecutePostProcesses();
            Assert.AreEqual(obj.HeMesh.VertexList.Count, 9);
            Assert.AreEqual(obj.HeMesh.FaceList.Count, 14);
        }

        [Test]
        public void CubeWithHole()
        {
            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\MeshSimplificationTest\\CubeWithHole.dae");
            Mesh mesh = meshes[0];
            DeformableObject obj = new DeformableObject();
            obj.Initialize(mesh);
            obj.AddPostProcessStep(DeformableObject.PostprocessAlgorithm.PlanarMerge);
            obj.ExecutePostProcesses();
            Assert.AreEqual(obj.HeMesh.VertexList.Count, 16);
            Assert.AreEqual(obj.HeMesh.FaceList.Count, 28);
        }

        [Test]
        public void CubeWithTwoHoles()
        {
            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\MeshSimplificationTest\\CubeWithTwoHoles.dae");
            Mesh mesh = meshes[0];
            DeformableObject obj = new DeformableObject();
            obj.Initialize(mesh);
            obj.AddPostProcessStep(DeformableObject.PostprocessAlgorithm.PlanarMerge);
            obj.ExecutePostProcesses();
            Assert.AreEqual(obj.HeMesh.VertexList.Count, 22);
            Assert.AreEqual(obj.HeMesh.FaceList.Count, 40);
        }

        [Test]
        public void CubeWithThreeHoles()
        {
            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\MeshSimplificationTest\\CubeWithThreeHoles.dae");
            Mesh mesh = meshes[0];
            DeformableObject obj = new DeformableObject();
            obj.Initialize(mesh);
            obj.AddPostProcessStep(DeformableObject.PostprocessAlgorithm.PlanarMerge);
            obj.ExecutePostProcesses();
            Assert.AreEqual(obj.HeMesh.VertexList.Count, 27);
            Assert.AreEqual(obj.HeMesh.FaceList.Count, 50);
        }

        [Test]
        public void StraightEdgeReduction1()
        {
            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\MeshSimplificationTest\\StraightEdgeReduction1.dae");
            Mesh mesh = meshes[0];
            DeformableObject obj = new DeformableObject();
            obj.Initialize(mesh);
            obj.AddPostProcessStep(DeformableObject.PostprocessAlgorithm.ContourCalculator);
            obj.AddPostProcessStep(DeformableObject.PostprocessAlgorithm.StraightEdgeReduction);
            obj.ExecutePostProcesses();
            Assert.AreEqual(obj.HeMesh.VertexList.Count, 5);
            Assert.AreEqual(obj.HeMesh.FaceList.Count, 6);
        }

        [Test]
        public void StraightEdgeReduction2()
        {
            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\MeshSimplificationTest\\CubeWithTwoHoles.dae");
            Mesh mesh = meshes[0];
            DeformableObject obj = new DeformableObject();
            obj.Initialize(mesh);
            obj.AddPostProcessStep(DeformableObject.PostprocessAlgorithm.StraightEdgeReduction);
            obj.ExecutePostProcesses();
            Assert.AreEqual(obj.HeMesh.VertexList.Count, 22);
            Assert.AreEqual(obj.HeMesh.FaceList.Count, 40);
        }

        [Test]
        public void StraightEdgeReduction3()
        {
            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\MeshSimplificationTest\\StraightEdgeReduction3.dae");
            Mesh mesh = meshes[0];
            DeformableObject obj = new DeformableObject();
            obj.Initialize(mesh);
            obj.AddPostProcessStep(DeformableObject.PostprocessAlgorithm.StraightEdgeReduction);
            obj.ExecutePostProcesses();
            Assert.AreEqual(obj.HeMesh.VertexList.Count, 24);
            Assert.AreEqual(obj.HeMesh.FaceList.Count, 44);
        }

        [Test]
        public void StraightEdgeReduction4()
        {
            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\ContourCalculationTest\\ComplexObject1.dae");
            Mesh mesh = meshes[0];
            DeformableObject obj = new DeformableObject();
            obj.Initialize(mesh);
            obj.AddPostProcessStep(DeformableObject.PostprocessAlgorithm.StraightEdgeReduction);
            obj.ExecutePostProcesses();
            Assert.AreEqual(obj.HeMesh.VertexList.Count, 56);
            Assert.AreEqual(obj.HeMesh.FaceList.Count, 108);
        }

        [Test]
        public void Combination1()
        {
            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\MeshSimplificationTest\\MergableFaces.dae");
            Mesh mesh = meshes[0];
            DeformableObject obj = new DeformableObject();
            obj.Initialize(mesh);
            obj.AddPostProcessStep(DeformableObject.PostprocessAlgorithm.PlanarMerge);
            obj.AddPostProcessStep(DeformableObject.PostprocessAlgorithm.StraightEdgeReduction);
            obj.ExecutePostProcesses();

            Assert.AreEqual(obj.ContourGroupManager.ContourGroups.Count, 6);

            foreach (var contourGroup in obj.ContourGroupManager.ContourGroups)
            {
                Assert.AreEqual(contourGroup.Contours.Count, 1);
                Assert.AreEqual(contourGroup.Contours[0].HeList.Count, 4);
            }

        }

        [Test]
        public void Combination1Reversed()
        {
            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\MeshSimplificationTest\\MergableFaces.dae");
            Mesh mesh = meshes[0];
            DeformableObject obj = new DeformableObject();
            obj.Initialize(mesh);
            obj.AddPostProcessStep(DeformableObject.PostprocessAlgorithm.StraightEdgeReduction);
            obj.AddPostProcessStep(DeformableObject.PostprocessAlgorithm.PlanarMerge);
            obj.ExecutePostProcesses();

            Assert.AreEqual(obj.ContourGroupManager.ContourGroups.Count, 6);

            foreach (var contourGroup in obj.ContourGroupManager.ContourGroups)
            {
                Assert.AreEqual(contourGroup.Contours.Count, 1);
                Assert.AreEqual(contourGroup.Contours[0].HeList.Count, 4);
            }
        }
    }
}
