using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooleanSubractorTests;
using DataManagement;
using GraphicsEngine;
using GraphicsEngine.Geometry;
using NUnit.Framework;
using Shared;

namespace BooleanSubtractorTests
{
    [TestFixture]
    class EdgeEdgeIntersectionTest
    {
        private BooleanTester _bTester;

        [SetUp]
        public void Setup()
        {
            _bTester = new BooleanTester();
        }


        [Test]
        public void EdgeOnFace()
        {
            DeformableObject obj = new DeformableObject(1);
            DeformableObject obj2 = new DeformableObject(1);

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\EdgeEdgeIntersectionTest\\EdgeOnFace.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 2, 24, 150, 50);

            // try it the other way round
            obj = new DeformableObject();
            obj.Initialize(mesh);
            obj2 = new DeformableObject();
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 2, 16, 84, 28);
        }

       
        [Test]
        public void EdgeOnFace3()
        {
            DeformableObject obj = new DeformableObject(1);
            DeformableObject obj2 = new DeformableObject(1);

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\EdgeEdgeIntersectionTest\\EdgeOnFace3.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 2, 29, 174, 58);

            // try it the other way round
            obj = new DeformableObject();
            obj.Initialize(mesh);
            obj2 = new DeformableObject();
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 2, 20, 108, 36);
        }

        [Test]
        public void EdgeOnFace4()
        {
            DeformableObject obj = new DeformableObject(1);
            DeformableObject obj2 = new DeformableObject(1);

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\EdgeEdgeIntersectionTest\\EdgeOnFace4.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 2, 26, 150, 50);

            // try it the other way round
            obj = new DeformableObject();
            obj.Initialize(mesh);
            obj2 = new DeformableObject();
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 2, 21, 114, 38);
        }

        [Test]
        public void EdgeEdgeIntersection()
        {
            DeformableObject obj = new DeformableObject(1);
            DeformableObject obj2 = new DeformableObject(1);

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\EdgeEdgeIntersectionTest\\EdgeEdgeIntersection.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 2, 25, 150, 50);

            // try it the other way round
            obj = new DeformableObject();
            obj.Initialize(mesh);
            obj2 = new DeformableObject();
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 2, 19, 102, 34);
        }

        [Test]
        public void EdgeEdgeIntersection2()
        {
            DeformableObject obj = new DeformableObject(1);
            DeformableObject obj2 = new DeformableObject(1);

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\EdgeEdgeIntersectionTest\\EdgeEdgeIntersection2.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 2, 22, 126, 42);

            // try it the other way round
            obj = new DeformableObject();
            obj.Initialize(mesh);
            obj2 = new DeformableObject();
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 2, 19, 102, 34);
        }

        [Test]
        public void EdgeEdgeIntersection3()
        {
            DeformableObject obj = new DeformableObject(1);
            DeformableObject obj2 = new DeformableObject(1);

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\EdgeEdgeIntersectionTest\\EdgeEdgeIntersection3.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 2, 28, 168, 56);

            // try it the other way round
            obj = new DeformableObject();
            obj.Initialize(mesh);
            obj2 = new DeformableObject();
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 2, 22, 120, 40);
        }

        [Test]
        public void EdgeEdgeIntersection4()
        {
            DeformableObject obj = new DeformableObject(1);
            DeformableObject obj2 = new DeformableObject(1);

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\EdgeEdgeIntersectionTest\\EdgeEdgeIntersection4.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 2, 32, 192, 64);

            // try it the other way round
            obj = new DeformableObject();
            obj.Initialize(mesh);
            obj2 = new DeformableObject();
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 2, 30, 156, 52);
        }

        [Test]
        public void EdgeEdgeIntersection5()
        {
            DeformableObject obj = new DeformableObject();
            DeformableObject obj2 = new DeformableObject();

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\EdgeEdgeIntersectionTest\\EdgeEdgeIntersection5.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 1, 18, 96, 32);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 1, 10, 48, 16);
        }

        public void EdgeEdgeIntersection6()
        {
            DeformableObject obj = new DeformableObject();
            DeformableObject obj2 = new DeformableObject();

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\EdgeEdgeIntersectionTest\\EdgeEdgeIntersection6.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 1, 32, 192, 64);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 1, 29, 150, 50);
        }

        [Test]
        public void EdgeEdgeEdge()
        {
            DeformableObject obj = new DeformableObject(1);
            DeformableObject obj2 = new DeformableObject(1);

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\EdgeEdgeIntersectionTest\\EdgeEdgeEdge.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 2, 33, 192, 64);

            // try it the other way round
            obj = new DeformableObject();
            obj.Initialize(mesh);
            obj2 = new DeformableObject();
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 2, 8, 36, 12);
        }

        [Test]
        public void EdgeEdgeEdge1()
        {
            DeformableObject obj = new DeformableObject(1);
            DeformableObject obj2 = new DeformableObject(1);

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\EdgeEdgeIntersectionTest\\EdgeEdgeEdge1.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 2, 45, 270, 90);

            // try it the other way round
            obj = new DeformableObject();
            obj.Initialize(mesh);
            obj2 = new DeformableObject();
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 2, 18, 96, 32);
        }

        [Test]
        public void EdgeEdgeEdge2()
        {
            DeformableObject obj = new DeformableObject(1);
            DeformableObject obj2 = new DeformableObject(1);

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\EdgeEdgeIntersectionTest\\EdgeEdgeEdge2.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 2, 36, 210, 70);

            // try it the other way round
            obj = new DeformableObject();
            obj.Initialize(mesh);
            obj2 = new DeformableObject();
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 2, 9, 42, 14);
        }

        [Test]
        public void EdgeEdgeEdge3()
        {
            DeformableObject obj = new DeformableObject(1);
            DeformableObject obj2 = new DeformableObject(1);

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\EdgeEdgeIntersectionTest\\EdgeEdgeEdge3.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 2, 33, 192, 64);

            // try it the other way round
            obj = new DeformableObject();
            obj.Initialize(mesh);
            obj2 = new DeformableObject();
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 2, 8, 36, 12);
        }

        [Test]
        public void EdgeEdgeEdge4()
        {
            DeformableObject obj = new DeformableObject(1);
            DeformableObject obj2 = new DeformableObject(1);

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\EdgeEdgeIntersectionTest\\EdgeEdgeEdge4.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 2, 40, 234, 78);

            // try it the other way round
            obj = new DeformableObject();
            obj.Initialize(mesh);
            obj2 = new DeformableObject();
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 2, 11, 54, 18);
        }

        [Test]
        [Ignore]
        public void EdgeEdgeEdge5()
        {
            DeformableObject obj = new DeformableObject(1);
            DeformableObject obj2 = new DeformableObject(1);

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\EdgeEdgeIntersectionTest\\EdgeEdgeEdge5.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            //_bTester.Test(obj, obj2, true, 2, 54, 336, 112);

            // try it the other way round
            obj = new DeformableObject();
            obj.Initialize(mesh);
            obj2 = new DeformableObject();
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 2, 11, 54, 18);
        }

        [Test]
        [Ignore]
        public void EdgeEdgeEdge6()
        {
            DeformableObject obj = new DeformableObject(1);
            DeformableObject obj2 = new DeformableObject(1);

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\EdgeEdgeIntersectionTest\\EdgeEdgeEdge6.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            //_bTester.Test(obj, obj2, true, 2, 54, 336, 112);

            // try it the other way round
            obj = new DeformableObject();
            obj.Initialize(mesh);
            obj2 = new DeformableObject();
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 2, 11, 54, 18);
        }

        [Test]
        // This test checks if mesh A is valid at startup.
        // More precisely, the faces which are incident with halfedge halfedge (-0.8/0/1 - -1/0/1) might need to be split an aligned with
        // the halfedge. But this is (as far as we know it so far) not necessary. It is true that "wrong" splitlines will be created but these
        // splitlines will be removed by "RemoveDegenerateEdges".
        public void EdgeEdgeEdge7()
        {
            DeformableObject obj = new DeformableObject(1);
            DeformableObject obj2 = new DeformableObject(1);

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\EdgeEdgeIntersectionTest\\EdgeEdgeEdge7.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 2, 27, 156, 52);

            // try it the other way round
            obj = new DeformableObject();
            obj.Initialize(mesh);
            obj2 = new DeformableObject();
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, false, 2, 6, 24, 8);
        }

    }
}
