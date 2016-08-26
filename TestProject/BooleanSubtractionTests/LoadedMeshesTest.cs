using System;
using System.Collections.Generic;
using DataManagement;
using GraphicsEngine;
using GraphicsEngine.Geometry;
using NUnit.Framework;
using Shared;

namespace BooleanSubractorTests
{
    [TestFixture]
    class LoadedMeshesTest
    {
        private BooleanTester _bTester;

        [SetUp]
        public void Setup()
        {
            _bTester = new BooleanTester();
        }

        [Test]
        public void TwoBoxesBuggy()
        {
            DeformableObject obj = new DeformableObject(1);
            DeformableObject obj2 = new DeformableObject(1);

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\LoadedMeshesTest\\TwoBoxesBuggy.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 1, 25, 138, 46);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 1, 16, 84, 28);
        }

        [Test]
        public void TwoBoxes()
        {
            DeformableObject obj = new DeformableObject(1);
            DeformableObject obj2 = new DeformableObject(1);

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\LoadedMeshesTest\\TwoBoxes.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 1, 21, 114, 38);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 1, 14, 72, 24);
        }

        [Test]
        public void Cone()
        {
            DeformableObject obj = new DeformableObject();
            DeformableObject obj2 = new DeformableObject();

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\LoadedMeshesTest\\cone.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 1, 52, 300, 100);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 1, 75, 438, 146);
        }

        [Test]
        public void MultiSplitLines()
        {
            DeformableObject obj = new DeformableObject();
            DeformableObject obj2 = new DeformableObject();

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\LoadedMeshesTest\\multisplitlines.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            int[] splitlines = { 4, 4 };
            _bTester.Test(obj, obj2, true, 2, 9, 42, 14);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            splitlines[0] = 22;
            _bTester.Test(obj2, obj, true, 1, 27, 150, 50);
        }

        [Test]
        public void TwoBoxes2()
        {
            DeformableObject obj = new DeformableObject();
            DeformableObject obj2 = new DeformableObject();

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\LoadedMeshesTest\\TwoBoxes2.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 1, 16, 84, 28);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 1, 14, 72, 24);
        }

        [Test]
        public void TwoBoxes3()
        {
            DeformableObject obj = new DeformableObject(1);
            DeformableObject obj2 = new DeformableObject(1);

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\LoadedMeshesTest\\TwoBoxes3.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 1, 16, 84, 28);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 1, 9, 42, 14);
        }

        [Test]
        public void TwoBoxes4()
        {
            DeformableObject obj = new DeformableObject(1);
            DeformableObject obj2 = new DeformableObject(1);

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\LoadedMeshesTest\\TwoBoxes4.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 1,  16, 84, 28);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 1, 14, 72, 24);
        }

        [Test]
        public void SmallIntersection1()
        {
            DeformableObject obj = new DeformableObject(1);
            DeformableObject obj2 = new DeformableObject(1);

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\LoadedMeshesTest\\SmallIntersection1.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 1,  21, 114, 38);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 1, 25, 138, 46);
        }

        [Test]
        [Ignore]
        public void TwoPrisms()
        {
            DeformableObject obj = new DeformableObject(1);
            DeformableObject obj2 = new DeformableObject(1);

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\LoadedMeshesTest\\TwoPrisms.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 2,9, 42, 14);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, false, 2, 6, 24, 8);
            Assert.AreEqual(0, obj.HeMesh.VertexList.Count);
        }
    }
}
