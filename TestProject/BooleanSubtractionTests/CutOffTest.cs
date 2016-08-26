using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooleanSubractorTests;
using DataManagement;
using GraphicsEngine.Geometry;
using NUnit.Framework;
using Shared;

namespace BooleanSubtractorTests
{
    [TestFixture]
    [Ignore]
    class CutOffTest
    {
        private BooleanTester _bTester;

        [SetUp]
        public void Setup()
        {
            _bTester = new BooleanTester();
        }

        [Test]
        public void EdgeEgeCutOff3()
        {
            DeformableObject obj = new DeformableObject();
            DeformableObject obj2 = new DeformableObject();

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\CutOffTest\\EdgeEdgeCutOff3.dae");
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
        public void EdgeOnFace2()
        {
            DeformableObject obj = new DeformableObject(1);
            DeformableObject obj2 = new DeformableObject(1);

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\CutOffTest\\EdgeOnFace2.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 2, 11, 60, 20);

            // try it the other way round
            obj = new DeformableObject();
            obj.Initialize(mesh);
            obj2 = new DeformableObject();
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 2, 10, 42, 14);
        }

        [Test]
        public void Cuboctahedron1()
        {
            DeformableObject obj = new DeformableObject();
            DeformableObject obj2 = new DeformableObject();

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\CutOffTest\\cuboctahedron1.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 3, 14, 72, 24);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, false, 3, 8, 36, 12);
        }

        [Test]
        public void Cuboctahedron2()
        {
            DeformableObject obj = new DeformableObject();
            DeformableObject obj2 = new DeformableObject();

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\CutOffTest\\cuboctahedron2.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 4, 22, 120, 40);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, false, 3, 12, 60, 20);
        }

        [Test]
        public void ZeroSplitlineLength()
        {
            DeformableObject obj = new DeformableObject(1);
            DeformableObject obj2 = new DeformableObject(1);

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\CutOffTest\\ZeroSplitlineLength.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 2, 16, 90, 30);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 2, 15, 72, 24);
        }

        [Test]
        public void ZeroSplitlineLength1()
        {
            DeformableObject obj = new DeformableObject(1);
            DeformableObject obj2 = new DeformableObject(1);

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\CutOffTest\\ZeroSplitlineLength1.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);
            _bTester.Test(obj, obj2, true, 2, 11, 60, 20);

            // try it the other way round
            obj = new DeformableObject(1);
            obj.Initialize(mesh);
            obj2 = new DeformableObject(1);
            obj2.Initialize(mesh2);
            _bTester.Test(obj2, obj, true, 2, 10, 42, 14);
        }
    }
}
