using GeometryCalculation.DataStructures;
using GraphicsEngine.Geometry;
using GraphicsEngine.Geometry.Meshes;
using NUnit.Framework;
using Shared.Geometry;

namespace BooleanSubractorTests
{
    [TestFixture]
    internal class ManualSetMeshesTest
    {
        private BooleanTester _bTester;

        [SetUp]
        public void Setup()
        {
            _bTester = new BooleanTester();
        }

        [Test]
        public void Mesh1()
        {
            var mesh = DefaultMeshes.Box(1f, 1f, 1f);
            var obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj.TranslateAndBuildBvh(new Vector3m(0, 0, -1));

            Vector3d[] verts2 =
            {
                new Vector3d(0, 0.5f, -0.5f),
                new Vector3d(-0.5f, 0.8f, 0.5f),
                new Vector3d(0.5f, -0.5f, 0.5f),
                new Vector3d(-0.5f, 0, 0.5f),
            };

            int[] coords2 =
            {
                0, 1, 2,
                2, 3, 0,
                0, 3, 1,
                3, 2, 1
            };

            Mesh mesh2 = new Mesh(verts2, coords2, null, null);
            var obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 1, 16, 84, 28);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            obj.TranslateAndBuildBvh(new Vector3m(0, 0, -1));
            _bTester.Test(obj2, obj, true, 1, 10, 48, 16);

        }

        [Test]
        public void Mesh2()
        {
            Vector3d[] verts1 =
            {
                new Vector3d(-1, 1, -0.5f),
                new Vector3d(-1, 1, 0),
                new Vector3d(1, 1, -0.5f),
                new Vector3d(1, 1, 0),
                new Vector3d(1, -1, 0),
                new Vector3d(-1, -1, 0),
            };

            int[] coords1 =
            {
                0, 1, 2,
                2, 1, 3,
                3, 4, 2,
                2, 4, 0,
                0, 4, 5,
                5, 1, 0,
                4, 3, 5,
                5, 3, 1
            };

            Mesh mesh = new Mesh(verts1, coords1, null, null);
            var obj = new DeformableObject(1);
            obj.Initialize(mesh);

            Vector3d[] verts2 =
            {
                new Vector3d(-0.5f, -0.5f, -1),
                new Vector3d(0.5f, -0.5f, 0.5f),
                new Vector3d(-0.8f, -0.7f, 0.5f),
                new Vector3d(-0.5f, 1.5f, -1),
                new Vector3d(-0.5f, 0.5f, 0.5f),
                new Vector3d(1, 2, -1.5f),
                new Vector3d(0.5f, 0.8f, 1),
                new Vector3d(0.5f, 1, -1),
            };

            int[] coords2 =
            {
                0, 1, 2,
                2, 3, 0,
                3, 2, 4,
                4, 5, 3,
                5, 4, 6,
                6, 7, 5,
                7, 6, 1,
                1, 0, 7,
                2, 1, 4,
                4, 1, 6,
                0, 3, 7,
                7, 3, 5
            };

            Mesh mesh2 = new Mesh(verts2, coords2, null, null);
            var obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 1, 47, 270, 90);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 1, 47, 270, 90);

        }

        [Test]
        public void Mesh3()
        {
            Vector3d[] verts1 =
            {
                new Vector3d(0, 0, -1.5f),
                new Vector3d(1, 1, 0),
                new Vector3d(-2, 0, -1.5f),
                new Vector3d(-1, -1, 0),
                new Vector3d(1, -1, 0),
                new Vector3d(-1, 1, 0)
            };

            int[] coords1 =
            {
                0, 1, 4,
                4, 3, 0,
                0, 3, 2,
                2, 3, 5,
                5, 0, 2,
                0, 5, 1,
                1, 5, 4,
                4, 5, 3
            };

            Mesh mesh = new Mesh(verts1, coords1, null, null);
            var obj = new DeformableObject(1);
            obj.Initialize(mesh);

            Vector3d[] verts2 =
            {
                new Vector3d(0.5f, 2, -1),
                new Vector3d(0.5f, 0.8f, 1),
                new Vector3d(0.5f, -0.5f, -1),
                new Vector3d(-0.5f, -0.5f, -1),


            };

            int[] coords2 =
            {
                0, 1, 2,
                2, 1, 3,
                3, 1, 0,
                0, 2, 3
            };

            Mesh mesh2 = new Mesh(verts2, coords2, null, null);
            var obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 1, 35, 198, 66);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 1, 31, 174, 58);
        }

        [Test]
        public void Coplanar2()
        {
            Vector3d[] verts1 =
            {
                new Vector3d(-1, 1, -1),
                new Vector3d(-1, 1, 1),
                new Vector3d(1, 1, -1),
                new Vector3d(1, 1, 1),
                new Vector3d(1, -1, -1),
                new Vector3d(1, -1, 1),
                new Vector3d(-1, -1, -1),
                new Vector3d(-1, -1, 1),

            };

            int[] coords1 =
            {
                0, 1, 2,
                2, 1, 3,
                3, 4, 2,
                4, 3, 5,
                5, 6, 4,
                6, 5, 7,
                7, 0, 6,
                0, 7, 1,
                3, 7, 5,
                7, 3, 1,
                4, 0, 2,
                0, 4, 6
            };

            Mesh mesh = new Mesh(verts1, coords1, null, null);
            var obj = new DeformableObject(1);
            obj.Initialize(mesh);

            Vector3d[] verts2 =
            {
                new Vector3d(-0.4f, 1, -0.3f),
                new Vector3d(-0.4f, 1, 0.5f),
                new Vector3d(0.4f, 1, -0.3f),
                new Vector3d(0.4f, 1, 0.5f),
                new Vector3d(0.4f, 0, -0.3f),
                new Vector3d(0.4f, 0, 0.5f),
                new Vector3d(-0.4f, 0, -0.3f),
                new Vector3d(-0.4f, 0, 0.5f),
            };

            int[] coords2 =
            {
                0, 1, 2,
                2, 1, 3,
                3, 4, 2,
                4, 3, 5,
                5, 6, 4,
                6, 5, 7,
                7, 0, 6,
                0, 7, 1,
                3, 7, 5,
                7, 3, 1,
                4, 0, 2,
                0, 4, 6
            };

            Mesh mesh2 = new Mesh(verts2, coords2, null, null);
            var obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 1, 20, 108, 36);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, false, 0, 8, 36, 12);

            //TODO A lies completely in B but this won't recognized so far
        }

        [Test]
        public void Coplanar3()
        {
            Vector3d[] verts1 =
            {
                new Vector3d(-1, 1, -1),
                new Vector3d(-1, 1, 1),
                new Vector3d(1, 1, -1),
                new Vector3d(1, 1, 1),
                new Vector3d(1, -1, -1),
                new Vector3d(1, -1, 1),
                new Vector3d(-1, -1, -1),
                new Vector3d(-1, -1, 1),

            };

            int[] coords1 =
            {
                0, 1, 2,
                2, 1, 3,
                3, 4, 2,
                4, 3, 5,
                5, 6, 4,
                6, 5, 7,
                7, 0, 6,
                0, 7, 1,
                3, 7, 5,
                7, 3, 1,
                4, 0, 2,
                0, 4, 6
            };

            Mesh mesh = new Mesh(verts1, coords1, null, null);
            var obj = new DeformableObject(1);
            obj.Initialize(mesh);

            Vector3d[] verts2 =
            {
                new Vector3d(-0.4f, 1.5f, -0.5f),
                new Vector3d(-0.4f, 1.5f, -0.3f),
                new Vector3d(0.4f, 1.5f, -0.5f),
                new Vector3d(0.4f, 1.5f, -0.3f),
                new Vector3d(-0.4f, 1, -0.5f),
                new Vector3d(0.4f, 1, -0.5f),
                new Vector3d(-0.4f, 1, -0.3f),
                new Vector3d(0.4f, 1, -0.3f),
                new Vector3d(-0.4f, 1, 0.5f),
                new Vector3d(0.4f, 1, 0.5f),
                new Vector3d(-0.4f, 0, 0.5f),
                new Vector3d(0.4f, 0, 0.5f),
                new Vector3d(-0.4f, 0, -0.5f),
                new Vector3d(0.4f, 0, -0.5f),
                new Vector3d(0.4f, 0, -0.3f),
                new Vector3d(-0.4f, 0, -0.3f),
            };

            int[] coords2 =
            {
                0, 1, 2,
                2, 1, 3,
                2, 3, 5,
                5, 3, 7,
                7, 3, 6,
                6, 3, 1,
                4, 6, 0,
                0, 6, 1,
                4, 0, 5,
                5, 0, 2,
                6, 8, 7,
                7, 8, 9,
                7, 9, 14,
                14, 9, 11,
                14, 11, 15,
                15, 11, 10,
                15, 10, 6,
                6, 10, 8,
                8, 10, 9,
                9, 10, 11,
                5, 7, 13,
                13, 7, 14,
                13, 14, 12,
                12, 14, 15,
                12, 15, 4,
                4, 15, 6,
                5, 13, 4,
                4, 13, 12
            };

            Mesh mesh2 = new Mesh(verts2, coords2, null, null);
            var obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 1, 24, 132, 44);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 1, 13, 66, 22);
        }

        [Test]
        public void Coplanar4()
        {
            Vector3d[] verts1 =
            {
                new Vector3d(-1, 1, -1),
                new Vector3d(-1, 1, 1),
                new Vector3d(1, 1, -1),
                new Vector3d(1, 1, 1),
                new Vector3d(1, -1, -1),
                new Vector3d(1, -1, 1),
                new Vector3d(-1, -1, -1),
                new Vector3d(-1, -1, 1),

            };

            int[] coords1 =
            {
                0, 1, 2,
                2, 1, 3,
                3, 4, 2,
                4, 3, 5,
                5, 6, 4,
                6, 5, 7,
                7, 0, 6,
                0, 7, 1,
                3, 7, 5,
                7, 3, 1,
                4, 0, 2,
                0, 4, 6
            };

            Mesh mesh = new Mesh(verts1, coords1, null, null);
            var obj = new DeformableObject(1);
            obj.Initialize(mesh);

            Vector3d[] verts2 =
            {
                new Vector3d(-0.5f, 2.5f, -0.5f),
                new Vector3d(-0.5f, 2.5f, -0.3f),
                new Vector3d(0.5f, 2.5f, -0.5f),
                new Vector3d(0.5f, 2.5f, -0.3f),
                new Vector3d(-0.5f, 2.5f, 0.5f),
                new Vector3d(0.5f, 2.5f, 0.5f),
                new Vector3d(-0.5f, 1, -0.5f),
                new Vector3d(0.5f, 1, -0.5f),
                new Vector3d(-0.5f, 1, -0.3f),
                new Vector3d(0.5f, 1, -0.3f),
                new Vector3d(-0.5f, 1, 0.5f),
                new Vector3d(0.5f, 1, 0.5f),
                new Vector3d(-0.5f, 0.3f, -0.5f),
                new Vector3d(0.5f, 0.3f, -0.5f),
                new Vector3d(-0.5f, 0.3f, -0.3f),
                new Vector3d(0.5f, 0.3f, -0.3f),
            };

            int[] coords2 =
            {
                0, 1, 2,
                2, 1, 3,
                2, 3, 7,
                7, 3, 9,
                6, 8, 0,
                0, 8, 1,
                6, 0, 7,
                7, 0, 2,
                1, 4, 3,
                3, 4, 5,
                3, 5, 9,
                9, 5, 11,
                8, 10, 1,
                1, 10, 4,
                4, 10, 5,
                5, 10, 11,
                9, 11, 8,
                8, 11, 10,
                15, 9, 14,
                14, 9, 8,
                12, 14, 6,
                6, 14, 8,
                7, 9, 13,
                13, 9, 15,
                7, 13, 6,
                6, 13, 12,
                13, 15, 12,
                12, 15, 14
            };

            Mesh mesh2 = new Mesh(verts2, coords2, null, null);
            var obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 1, 18, 96, 32);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 1, 14, 72, 24);
        }

        [Test]
        public void Coplanar5()
        {
            Vector3d[] verts1 =
            {
                new Vector3d(-1, 1, -1),
                new Vector3d(-1, 1, 1),
                new Vector3d(1, 1, -1),
                new Vector3d(1, 1, 1),
                new Vector3d(1, -1, -1),
                new Vector3d(1, -1, 1),
                new Vector3d(-1, -1, -1),
                new Vector3d(-1, -1, 1),

            };

            int[] coords1 =
            {
                0, 1, 2,
                2, 1, 3,
                3, 4, 2,
                4, 3, 5,
                5, 6, 4,
                6, 5, 7,
                7, 0, 6,
                0, 7, 1,
                3, 7, 5,
                7, 3, 1,
                4, 0, 2,
                0, 4, 6
            };

            Mesh mesh = new Mesh(verts1, coords1, null, null);
            var obj = new DeformableObject(1);
            obj.Initialize(mesh);

            Vector3d[] verts2 =
            {
                new Vector3d(-0.4f, 2, -0.3f),
                new Vector3d(-0.4f, 2, 0.5f),
                new Vector3d(0.4f, 2, -0.3f),
                new Vector3d(0.4f, 2, 0.5f),
                new Vector3d(-0.4f, 1, -0.3f),
                new Vector3d(-0.4f, 1, 0.5f),
                new Vector3d(0.4f, 1, -0.3f),
                new Vector3d(0.4f, 1, 0.5f),
            };

            int[] coords2 =
            {
                0, 1, 2,
                2, 1, 3,
                2, 3, 6,
                6, 3, 7,
                4, 5, 0,
                0, 5, 1,
                6, 7, 4,
                4, 7, 5,
                1, 5, 3,
                3, 5, 7,
                4, 0, 6,
                6, 0, 2
            };

            Mesh mesh2 = new Mesh(verts2, coords2, null, null);
            var obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, false, 0, 8, 36, 12);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, false, 0, 8, 36, 12);
        }

        [Test]
        public void Coplanar6()
        {
            Vector3d[] verts1 =
            {
                new Vector3d(-1, 1, -1),
                new Vector3d(-1, 1, 1),
                new Vector3d(1, 1, -1),
                new Vector3d(1, 1, 1),
                new Vector3d(1, -1, -1),
                new Vector3d(1, -1, 1),
                new Vector3d(-1, -1, -1),
                new Vector3d(-1, -1, 1),
            };

            int[] coords1 =
            {
                0, 1, 2,
                2, 1, 3,
                3, 4, 2,
                4, 3, 5,
                5, 6, 4,
                6, 5, 7,
                7, 0, 6,
                0, 7, 1,
                3, 7, 5,
                7, 3, 1,
                4, 0, 2,
                0, 4, 6
            };

            Mesh mesh = new Mesh(verts1, coords1, null, null);
            var obj = new DeformableObject(1);
            obj.Initialize(mesh);

            Vector3d[] verts2 =
            {
                new Vector3d(0.4f, 0, -0.3f),
                new Vector3d(0.4f, 0, 0.5f),
                new Vector3d(-0.4f, 0, -0.3f),
                new Vector3d(-0.4f, 0, 0.5f),
                new Vector3d(0.4f, 1, 0.5f),
                new Vector3d(-0.4f, 1, 0.5f),
            };

            int[] coords2 =
            {
                0, 1, 2,
                2, 1, 3,
                2, 3, 5,
                1, 0, 4,
                2, 5, 0,
                0, 5, 4,
                1, 4, 3,
                3, 4, 5
            };

            Mesh mesh2 = new Mesh(verts2, coords2, null, null);
            var obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 0, 10, 48, 16);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, false, 0, 6, 24, 8);
        }

        [Test]
        public void Coplanar7()
        {
            Vector3d[] verts1 =
            {
                new Vector3d(0, 0, 2),
                new Vector3d(-5, 0, -4),
                new Vector3d(1, 0, -2),
                new Vector3d(0, 1.5f, 2),
                new Vector3d(1, 1.5f, -2),
                new Vector3d(-5, 1.5f, -4),
            };

            int[] coords1 =
            {
                0, 1, 2,
                3, 4, 5,
                0, 2, 3,
                3, 2, 4,
                2, 1, 4,
                4, 1, 5,
                1, 0, 5,
                5, 0, 3
            };

            Mesh mesh = new Mesh(verts1, coords1, null, null);
            var obj = new DeformableObject(1);
            obj.Initialize(mesh);

            Vector3d[] verts2 =
            {
                new Vector3d(1, -0.5f, -0.5f),
                new Vector3d(0.3f, 0, 0),
                new Vector3d(-0.5f, -1.2f, 0.5f),
                new Vector3d(-0.8f, 0.2f, 0.5f),
                new Vector3d(-1, 1, 0),
                new Vector3d(-1, 0, 0),
                new Vector3d(-1, 1, -1),
                new Vector3d(-0.5f, -0.5f, -0.5f),
                new Vector3d(0.5f, 0.2f, 0.5f)
            };

            int[] coords2 =
            {
                0, 2, 7,
                0, 1, 2,
                2, 1, 8,
                8, 3, 2,
                2, 3, 5,
                5, 7, 2,
                7, 5, 6,
                6, 0, 7,
                6, 1, 0,
                6, 5, 1,
                5, 4, 1,
                5, 3, 4,
                4, 3, 8,
                8, 1, 4
            };

            Mesh mesh2 = new Mesh(verts2, coords2, null, null);
            var obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 2, 26, 156, 52);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 3, 21, 102, 34);
        }

        [Test]
        public void TwoBoxes()
        {
            var cube = DefaultMeshes.Box(50f, 100f, 100f); //manager.DefaultMesh.Icosphere(6, 50);
            DeformableObject obj = new DeformableObject();
            obj.Initialize(cube);

            var cube2 = DefaultMeshes.Box(50f, 100f, 100f);
            var obj2 = new DeformableObject();
            obj2.Initialize(cube2);

            obj2.TranslateAndBuildBvh(new Vector3m(20, 20, 0));

            _bTester.Test(obj, obj2, true, 1, 20, 108, 36);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(cube);
            obj2 = new DeformableObject(1);
            obj2.Initialize(cube2);
            obj2.TranslateAndBuildBvh(new Vector3m(20, 20, 0));
            _bTester.Test(obj2, obj, true, 1, 18, 96, 32);
        }

        [Test]
        public void TwoBoxes2()
        {
            var cube = DefaultMeshes.Box(50f, 50f, 50f);
            DeformableObject obj = new DeformableObject();
            obj.Initialize(cube);

            var cube2 = DefaultMeshes.Box(50f, 50f, 50f);
            var obj2 = new DeformableObject();
            obj2.Initialize(cube2);

            obj2.TranslateAndBuildBvh(new Vector3m(25, 25, 25));

            _bTester.Test(obj, obj2, true, 1, 21, 114, 38);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(cube);
            obj2 = new DeformableObject(1);
            obj2.Initialize(cube2);
            obj2.TranslateAndBuildBvh(new Vector3m(20, 20, 0));
            _bTester.Test(obj2, obj, true, 1, 16, 84, 28);
        }

        [Test]
        public void TwoPyramids()
        {
            var pyramid = DefaultMeshes.Pyramid(50f, 50f, 50f);
            DeformableObject obj = new DeformableObject();
            obj.Initialize(pyramid);

            var pyramid2 = DefaultMeshes.Pyramid(50f, 50f, 50f);
            var obj2 = new DeformableObject();
            obj2.Initialize(pyramid2);

            obj2.TranslateAndBuildBvh(new Vector3m(20, 20, 0));
            _bTester.Test(obj, obj2, true, 1, 11, 54, 18);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(pyramid);
            obj2 = new DeformableObject(1);
            obj2.Initialize(pyramid2);
            obj2.TranslateAndBuildBvh(new Vector3m(20, 20, 0));
            _bTester.Test(obj2, obj, true, 1, 12, 60, 20);
        }

        [Test]
        public void TwoSpheres()
        {
            TestConfigurator.CheckSanity = false;
            var sphere1 = DefaultMeshes.Icosphere(5, 50);
            DeformableObject obj = new DeformableObject();
            obj.Initialize(sphere1);

            var sphere2 = DefaultMeshes.Icosphere(5, 50);
            var obj2 = new DeformableObject();
            obj2.Initialize(sphere2);
            obj2.TranslateAndBuildBvh(new Vector3m(20, 20, 0));
            _bTester.Test(obj, obj2, true, 1, 10955, 65718, 21906);
            obj.Clone(Vector3m.Zero());

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(sphere1);
            obj2 = new DeformableObject(1);
            obj2.Initialize(sphere2);
            obj2.TranslateAndBuildBvh(new Vector3m(20, 20, 0));
            _bTester.Test(obj2, obj, true, 1, 10954, 65712, 21904);
            TestConfigurator.CheckSanity = true;
        }

        [Test]
        [ExpectedException]
        public void SplitLineCreationTest1()
        {
            Vector3d[] verts1 =
            {
                new Vector3d(1, 1, 0),
                new Vector3d(-1, 1, 0),
                new Vector3d(-1, -1, 0),
                new Vector3d(1, -1, 0)
            };

            int[] coords1 =
            {
                0, 1, 2,
                0, 2, 3
            };

            Mesh mesh = new Mesh(verts1, coords1, null, null);
            var obj = new DeformableObject(1);
            obj.Initialize(mesh);

            Vector3d[] verts2 =
            {
                new Vector3d(2, 2, 0),
                new Vector3d(1, 1, 2),
                new Vector3d(-2, -2, 0),
                new Vector3d(0, 0, -2)
            };

            int[] coords2 =
            {
                0, 1, 2,
                2, 3, 0
            };

            Mesh mesh2 = new Mesh(verts2, coords2, null, null);
            var obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 2, 21, 114, 38);
        }

        [Test]
        public void VertexVertexVertex()
        {
            Vector3d[] verts1 =
            {
                new Vector3d(0, 50, 0),
                new Vector3d(50, 0, 50),
                new Vector3d(50, 0, -50),
                new Vector3d(80, 60, 0),
            };

            int[] coords1 =
            {
                0, 1, 3,
                3, 1, 2,
                2, 1, 0,
                0, 3, 2
            };

            Mesh mesh = new Mesh(verts1, coords1, null, null);
            var obj = new DeformableObject(1);
            obj.Initialize(mesh);

            Vector3d[] verts2 =
            {
                new Vector3d(20, 70, 0),
                new Vector3d(-30, 20, 50),
                new Vector3d(-30, 20, -50),
                new Vector3d(0, 150, 0),
            };

            int[] coords2 =
            {
                0, 1, 2,
                2, 1, 3,
                3, 1, 0,
                0, 2, 3
            };

            Mesh mesh2 = new Mesh(verts2, coords2, null, null);
            var obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, false, 0,  4, 12, 4);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, false, 0,  4, 12, 4);
        }

        [Test]
        public void SplitInFive1()
        {
            Vector3d[] verts1 =
            {
                new Vector3d(-1, -1, 0),
                new Vector3d(1, 3, 0),
                new Vector3d(-1, 3, 0),
                new Vector3d(0, 4, 0),
                new Vector3d(-1, -1, -0.2f),
                new Vector3d(1, 3, -0.2f),
                new Vector3d(-1, 3, -0.2f),
                new Vector3d(0, 4, -0.2f)

            };

            int[] coords1 =
            {
                0, 1, 2,
                4, 0, 6,
                6, 0, 2,
                5, 1, 4,
                4, 1, 0,
                6, 5, 4,
                1, 3, 2,
                6, 2, 7,
                7, 2, 3,
                7, 3, 5,
                5, 3, 1,
                6, 7, 5
            };

            Mesh mesh = new Mesh(verts1, coords1, null, null);
            var obj = new DeformableObject(1);
            obj.Initialize(mesh);

            Vector3d[] verts2 =
            {
                new Vector3d(0, -3, 1),
                new Vector3d(0, 3, -1),
                new Vector3d(0, 3, 1),
                new Vector3d(1, 1, 0),
            };

            int[] coords2 =
            {
                2, 1, 0,
                1, 3, 0,
                0, 3, 2,
                2, 3, 1
            };

            Mesh mesh2 = new Mesh(verts2, coords2, null, null);
            var obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 1, 19, 102, 34);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 1, 15, 78, 26);
        }

        [Test]
        public void SplitInFive2()
        {
            Vector3d[] verts1 =
            {
                new Vector3d(0, -3, 1),
                new Vector3d(0, 3, -1),
                new Vector3d(0, 3, 1),
                new Vector3d(1, 1, 0),
            };

            int[] coords1 =
            {
                2, 1, 0,
                1, 3, 0,
                0, 3, 2,
                2, 3, 1
            };

            Mesh mesh = new Mesh(verts1, coords1, null, null);
            var obj = new DeformableObject(1);
            obj.Initialize(mesh);

            Vector3d[] verts2 =
            {
                new Vector3d(-0.2f, -1, 0),
                new Vector3d(2, 3, 0),
                new Vector3d(-0.2f, 3, 0),
                new Vector3d(1, 4, 0),
                new Vector3d(-0.2f, -1, -0.2f),
                new Vector3d(2, 3, -0.2f),
                new Vector3d(-0.2f, 3, -0.2f),
                new Vector3d(1, 4, -0.2f)

            };

            int[] coords2 =
            {
                0, 1, 2,
                4, 0, 6,
                6, 0, 2,
                5, 1, 4,
                4, 1, 0,
                6, 5, 4,
                1, 3, 2,
                6, 2, 7,
                7, 2, 3,
                7, 3, 5,
                5, 3, 1,
                6, 7, 5
            };

            Mesh mesh2 = new Mesh(verts2, coords2, null, null);
            var obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 2, 17, 78, 26);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 2, 21, 126, 42);
        }

        [Test]
        public void SplitInFive3()
        {
            Vector3d[] verts1 =
            {
                new Vector3d(-1, -1, 0),
                new Vector3d(1, 3, 0),
                new Vector3d(-1, 3, 0),
                new Vector3d(0, 4, 0),
                new Vector3d(-1, -1, -0.2f),
                new Vector3d(1, 3, -0.2f),
                new Vector3d(-1, 3, -0.2f),
                new Vector3d(0, 4, -0.2f)

            };

            int[] coords1 =
            {
                0, 1, 2,
                4, 0, 6,
                6, 0, 2,
                5, 1, 4,
                4, 1, 0,
                6, 5, 4,
                1, 3, 2,
                6, 2, 7,
                7, 2, 3,
                7, 3, 5,
                5, 3, 1,
                6, 7, 5
            };

            Mesh mesh = new Mesh(verts1, coords1, null, null);
            var obj = new DeformableObject(1);
            obj.Initialize(mesh);

            Vector3d[] verts2 =
            {
                new Vector3d(0, -3, 1),
                new Vector3d(0, 3, -1),
                new Vector3d(0, 3, 1),
                new Vector3d(1, 1, 0),
            };

            int[] coords2 =
            {
                0, 2, 1,
                1, 3, 0,
                0, 3, 2,
                2, 3, 1
            };

            Mesh mesh2 = new Mesh(verts2, coords2, null, null);
            var obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 1, 19, 102, 34);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 1, 15, 78, 26);
        }

        [Test]
        public void SplitInFive4()
        {
            Vector3d[] verts1 =
            {
                new Vector3d(-1, -1, 0),
                new Vector3d(1, 3, 0),
                new Vector3d(-1, 3, 0),
                new Vector3d(0, 4, 0),
                new Vector3d(-1, -1, -0.2f),
                new Vector3d(1, 3, -0.2f),
                new Vector3d(-1, 3, -0.2f),
                new Vector3d(0, 4, -0.2f)

            };

            int[] coords1 =
            {
                0, 1, 2,
                4, 0, 6,
                6, 0, 2,
                5, 1, 4,
                4, 1, 0,
                6, 5, 4,
                1, 3, 2,
                6, 2, 7,
                7, 2, 3,
                7, 3, 5,
                5, 3, 1,
                6, 7, 5
            };

            Mesh mesh = new Mesh(verts1, coords1, null, null);
            var obj = new DeformableObject(1);
            obj.Initialize(mesh);

            Vector3d[] verts2 =
            {
                new Vector3d(0, -3, 1),
                new Vector3d(0, 3, -1),
                new Vector3d(0, 3, 1),
                new Vector3d(1, 1, 0),
            };

            int[] coords2 =
            {
                1, 0, 2,
                1, 3, 0,
                0, 3, 2,
                2, 3, 1
            };

            Mesh mesh2 = new Mesh(verts2, coords2, null, null);
            var obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 1, 19, 102, 34);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 1, 15, 78, 26);
        }

        [Test]
        public void FacesOnIntersectionCurve1()
        {
            Vector3d[] verts1 =
            {
                new Vector3d(-1, -1, -1),
                new Vector3d(-1, -1, 1),
                new Vector3d(-1, 1, -1),
                new Vector3d(-1, 1, 1),
                new Vector3d(1, 1, -1),
                new Vector3d(1, 1, 1),
                new Vector3d(1, -1, 1),
                new Vector3d(1, -1, -1),
                new Vector3d(1, 0, 0.2f),
                new Vector3d(1, -0.2f, -0.2f),
                new Vector3d(1, 0.2f, -0.2f),
            };

            int[] coords1 =
            {
                0, 1, 2,
                2, 1, 3,
                2, 3, 4,
                4, 3, 5,
                7, 6, 0,
                0, 6, 1,
                1, 6, 3,
                3, 6, 5,
                7, 0, 4,
                4, 0, 2,
                4, 5, 10,
                10, 5, 8,
                8, 5, 6,
                6, 9, 8,
                9, 6, 7,
                7, 4, 9,
                9, 4, 10,
                10, 8, 9
            };

            Mesh mesh = new Mesh(verts1, coords1, null, null);
            var obj = new DeformableObject(1);
            obj.Initialize(mesh);

            Vector3d[] verts2 =
            {
                new Vector3d(3, 0.2f, -0.2f),
                new Vector3d(3, 0, 0.2f),
                new Vector3d(3, -0.2f, -0.2f),
                new Vector3d(0, 0.2f, -0.2f),
                new Vector3d(0, 0, 0.2f),
                new Vector3d(0, -0.2f, -0.2f),
            };

            int[] coords2 =
            {
                0, 1, 2,
                2, 1, 5,
                5, 1, 4,
                0, 2, 3,
                3, 2, 5,
                3, 4, 0,
                0, 4, 1,
                5, 4, 3
            };

            Mesh mesh2 = new Mesh(verts2, coords2, null, null);
            var obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 1, 17, 90, 30);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 1, 9, 42, 14);
        }

        [Test]
        public void FacesOnIntersectionCurve2()
        {
            Vector3d[] verts1 =
            {
                new Vector3d(-1, -1, -1),
                new Vector3d(-1, -1, 1),
                new Vector3d(-1, 1, -1),
                new Vector3d(-1, 1, 1),
                new Vector3d(1, 1, -1),
                new Vector3d(1, 1, 1),
                new Vector3d(1, -1, 1),
                new Vector3d(1, -1, -1),
                new Vector3d(1, 0, 0.2f),
                new Vector3d(1, -0.2f, -0.2f),
                new Vector3d(1, 0.2f, -0.2f),
            };

            int[] coords1 =
            {
                0, 1, 2,
                2, 1, 3,
                2, 3, 4,
                4, 3, 5,
                7, 6, 0,
                0, 6, 1,
                1, 6, 3,
                3, 6, 5,
                7, 0, 4,
                4, 0, 2,
                4, 5, 10,
                10, 5, 8,
                8, 5, 6,
                6, 9, 8,
                9, 6, 7,
                7, 4, 9,
                9, 4, 10,
                10, 8, 9
            };

            Mesh mesh = new Mesh(verts1, coords1, null, null);
            var obj = new DeformableObject(1);
            obj.Initialize(mesh);

            Vector3d[] verts2 =
            {
                new Vector3d(3, 1, 1),
                new Vector3d(3, 0.2f, -0.2f),
                new Vector3d(3, 1, -1),
                new Vector3d(0.2f, 1, 1),
                new Vector3d(0.2f, 0.2f, -0.2f),
                new Vector3d(0.2f, 1, -1),
            };

            int[] coords2 =
            {
                0, 1, 2,
                2, 5, 0,
                0, 5, 3,
                3, 4, 1,
                1, 0, 3,
                2, 1, 5,
                5, 1, 4,
                4, 3, 5
            };

            Mesh mesh2 = new Mesh(verts2, coords2, null, null);
            var obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
             _bTester.Test(obj, obj2, true, 1, 17, 90, 30);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 1, 9, 42, 14);
        }

        [Test]
        public void VertexOnEdgeTest1()
        {
            Vector3d[] verts1 =
            {
                new Vector3d(50, -50, 50),
                new Vector3d(50, 50, 50),
                new Vector3d(-50, 50, 50),
                new Vector3d(-50, -50, 50),
                new Vector3d(50, -50, 10),
                new Vector3d(50, 50, 10),
                new Vector3d(-50, 50, 10),
                new Vector3d(-50, -50, 10)
            };

            int[] coords1 =
            {
                0, 1, 2,
                0, 2, 3,
                4, 6, 5,
                4, 7, 6,
                0, 4, 5,
                0, 5, 1,
                1, 5, 6,
                1, 6, 2,
                2, 6, 7,
                2, 7, 3,
                3, 7, 4,
                3, 4, 0
            };

            Mesh mesh = new Mesh(verts1, coords1, null, null);
            var obj = new DeformableObject(1);
            obj.Initialize(mesh);

            Vector3d[] verts2 =
            {
                new Vector3d(0, 0, 10),
                new Vector3d(50, 0, 50),
                new Vector3d(50, 0, 10),
                new Vector3d(50, 10, 10)
            };

            int[] coords2 =
            {
                2, 3, 1,
                1, 3, 0,
                0, 2, 1,
                0, 3, 2
            };

            Mesh mesh2 = new Mesh(verts2, coords2, null, null);
            var obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 1, 15, 78, 26);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, false, 1, 4, 12, 4);
        }

        [Test]
        public void VertexOnEdgeTest2()
        {
            Vector3d[] verts1 =
            {
                new Vector3d(50, -50, 50),
                new Vector3d(50, 50, 50),
                new Vector3d(-50, 50, 50),
                new Vector3d(-50, -50, 50),
                new Vector3d(50, -50, 10),
                new Vector3d(50, 50, 10),
                new Vector3d(-50, 50, 10),
                new Vector3d(-50, -50, 10)
            };

            int[] coords1 =
            {
                0, 1, 2,
                0, 2, 3,
                4, 6, 5,
                4, 7, 6,
                0, 4, 5,
                0, 5, 1,
                1, 5, 6,
                1, 6, 2,
                2, 6, 7,
                2, 7, 3,
                3, 7, 4,
                3, 4, 0
            };

            Mesh mesh = new Mesh(verts1, coords1, null, null);
            var obj = new DeformableObject(1);
            obj.Initialize(mesh);

            Vector3d[] verts2 =
            {
                new Vector3d(0, 0, 10),
                new Vector3d(50, 0, 50),
                new Vector3d(50, -50, 10),
                new Vector3d(50, 50, 10)
            };

            int[] coords2 =
            {
                2, 3, 1,
                1, 3, 0,
                0, 2, 1,
                0, 3, 2
            };

            Mesh mesh2 = new Mesh(verts2, coords2, null, null);
            var obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 1, 14, 72, 24);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, false, 1, 4, 12, 4);
        }

        [Test]
        public void VertexOnEdgeTest3()
        {
            Vector3d[] verts1 =
            {
                new Vector3d(50, -50, 50),
                new Vector3d(50, 50, 50),
                new Vector3d(-50, 50, 50),
                new Vector3d(-50, -50, 50),
                new Vector3d(50, -50, 10),
                new Vector3d(50, 50, 10),
                new Vector3d(-50, 50, 10),
                new Vector3d(-50, -50, 10)
            };

            int[] coords1 =
            {
                0, 1, 2,
                0, 2, 3,
                4, 6, 5,
                4, 7, 6,
                0, 4, 5,
                0, 5, 1,
                1, 5, 6,
                1, 6, 2,
                2, 6, 7,
                2, 7, 3,
                3, 7, 4,
                3, 4, 0
            };

            Mesh mesh = new Mesh(verts1, coords1, null, null);
            var obj = new DeformableObject(1);
            obj.Initialize(mesh);

            Vector3d[] verts2 =
            {
                new Vector3d(0, 0, 20),
                new Vector3d(50, 0, 50),
                new Vector3d(50, -50, 10),
                new Vector3d(50, 50, 10)
            };

            int[] coords2 =
            {
                2, 3, 1,
                1, 3, 0,
                0, 2, 1,
                0, 3, 2
            };

            Mesh mesh2 = new Mesh(verts2, coords2, null, null);
            var obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 1, 14, 72, 24);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, false, 1, 4, 12, 4);
        }

        [Test]
        public void VertexOnEdgeTest4()
        {
            Vector3d[] verts1 =
            {
                new Vector3d(50, -50, 50),
                new Vector3d(50, 50, 50),
                new Vector3d(-50, 50, 50),
                new Vector3d(-50, -50, 50),
                new Vector3d(50, -50, 10),
                new Vector3d(50, 50, 10),
                new Vector3d(-50, 50, 10),
                new Vector3d(-50, -50, 10)
            };

            int[] coords1 =
            {
                0, 1, 2,
                0, 2, 3,
                4, 6, 5,
                4, 7, 6,
                0, 4, 5,
                0, 5, 1,
                1, 5, 6,
                1, 6, 2,
                2, 6, 7,
                2, 7, 3,
                3, 7, 4,
                3, 4, 0
            };

            Mesh mesh = new Mesh(verts1, coords1, null, null);
            var obj = new DeformableObject(1);
            obj.Initialize(mesh);

            Vector3d[] verts2 =
            {
                new Vector3d(0, 0, 20),
                new Vector3d(50, 0, 50),
                new Vector3d(50, -50, 10),
                new Vector3d(60, 50, 10)
            };

            int[] coords2 =
            {
                2, 3, 1,
                1, 3, 0,
                0, 2, 1,
                0, 3, 2
            };

            Mesh mesh2 = new Mesh(verts2, coords2, null, null);
            var obj2 = new DeformableObject(1);
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
        public void VertexOnEdgeTest5()
        {
            Vector3d[] verts1 =
            {
                new Vector3d(50, -50, 50),
                new Vector3d(50, 50, 50),
                new Vector3d(-50, 50, 50),
                new Vector3d(-50, -50, 50),
                new Vector3d(50, -50, 10),
                new Vector3d(50, 50, 10),
                new Vector3d(-50, 50, 10),
                new Vector3d(-50, -50, 10)
            };
            
            int[] coords1 =
            {
                0, 1, 2,
                0, 2, 3,
                4, 6, 5,
                4, 7, 6,
                0, 4, 5,
                0, 5, 1,
                1, 5, 6,
                1, 6, 2,
                2, 6, 7,
                2, 7, 3,
                3, 7, 4,
                3, 4, 0
            };

            Mesh mesh = new Mesh(verts1, coords1, null, null);
            var obj = new DeformableObject(1);
            obj.Initialize(mesh);

            Vector3d[] verts2 =
            {
                new Vector3d(0, 0, 20),
                new Vector3d(50, 0, 50),
                new Vector3d(50, -50, 10),
                new Vector3d(50, 50, 10),
                new Vector3d(25, -25, 15), 
            };

            int[] coords2 =
            {
                2, 3, 1,
                1, 3, 0,
                0, 4, 1,
                4, 2, 1,
                0, 3, 4,
                4, 3, 2
            };

            Mesh mesh2 = new Mesh(verts2, coords2, null, null);
            var obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 1, 15, 78, 26);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, false, 1, 5, 18, 6);
        }

        [Test]
        public void SmallIntersection()
        {
            Vector3d[] verts1 =
            {
                new Vector3d(1, 1, 0),
                new Vector3d(-1, 1, 0),
                new Vector3d(1, -1, 0),
                new Vector3d(1, 1, -1),
                new Vector3d(-1, 1, -1),
                new Vector3d(1, -1, -1)
            };

            int[] coords1 =
            {
                0, 1, 2,
                3, 5, 4,
                0, 3, 1,
                3, 4, 1,
                2, 5, 0,
                5, 3, 0,
                1, 4, 2,
                4, 5, 2
            };

            Mesh mesh = new Mesh(verts1, coords1, null, null);
            var obj = new DeformableObject(1);
            obj.Initialize(mesh);

            Vector3d[] verts2 =
            {
                new Vector3d(Vector3d.Epsilon+0.000000001f, 0, 2),
                new Vector3d(-1, 0, 2),
                new Vector3d(-1, -1, 2),
                new Vector3d(Vector3d.Epsilon+0.000000001f, 0, -2),
                new Vector3d(-1, 0, -2),
                new Vector3d(-1, -1, -2) 
            };

            int[] coords2 =
            {
                0, 1, 2,
                3, 5, 4,
                0, 3, 1,
                3, 4, 1,
                2, 5, 0,
                5, 3, 0,
                1, 4, 2,
                4, 5, 2
            };

            Mesh mesh2 = new Mesh(verts2, coords2, null, null);
            var obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 1, 19, 102, 34);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 1, 19, 102, 34);
        }

        [Test]
        public void SmallPrism()
        {
            Vector3d[] verts1 =
            {
                new Vector3d(0, 0, 0),
                new Vector3d(Vector3d.Epsilon, 0, 0),
                new Vector3d(0, Vector3d.Epsilon, 0),
                new Vector3d(0, 0, Vector3d.Epsilon)
            };

            int[] coords1 =
            {
                0, 2, 1,
                0, 1, 3,
                1, 2, 3,
                2, 0, 3
            };

            Mesh mesh = new Mesh(verts1, coords1, null, null);
            var obj = new DeformableObject(1);
            obj.Initialize(mesh);
        }
    }
}
