using System.Collections.Generic;
using GeometryCalculation.DataStructures;
using NUnit.Framework;
using Shared.Geometry;
using Shared.Geometry.Meshes;
using Shared.Helper;
using TestProject.BooleanSubtractionTests;

namespace TestProject.SweepingTests
{
    [TestFixture]
    class SweptVolumeTest
    {
        [Test]
        public void MovedCube()
        {
            var mesh = DefaultMeshes.Box(30, 30, 30);
            DeformableObject o = new DeformableObject();
            o.Initialize(mesh);
            var cl = o.Clone(Vector3m.Zero());
            o.SweepVolume(cl, new Vector3m(10, 0, 0));
            Assert.AreEqual(mesh.Vertices.Length, 8);
            Assert.AreEqual(mesh.Indices.Length, 36);
            TestFramework.CheckSanity(o);
        }

        [Test]
        public void MovedCylinder()
        {
            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\Collada_Files\\CNC_Milling\\Cylinder1.dae");
            Mesh mesh = meshes[0];
            DeformableObject o = new DeformableObject();
            o.Initialize(mesh);
            o.Translate(new Vector3m(0, 200, 0));
            var cl = o.Clone(Vector3m.Zero());
            o.SweepVolume(cl, new Vector3m(250, 0, 180));
            TestFramework.CheckSanity(o);
        }

        [Test]
        public void MovedCylinder2()
        {
            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\Collada_Files\\CNC_Milling\\Cylinder1.dae");
            Mesh mesh = meshes[0];
            DeformableObject o = new DeformableObject();
            o.Initialize(mesh);
            var cl = o.Clone(Vector3m.Zero());
            o.SweepVolume(cl, new Vector3m(-450, 0, 0));
            TestFramework.CheckSanity(o);
        }

        [Test]
        public void MovedCylinder3()
        {
            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\Collada_Files\\CNC_Milling\\Cylinder2.dae");
            Mesh mesh = meshes[0];
            DeformableObject o = new DeformableObject();
            o.Initialize(mesh);
            o.Translate(new Vector3m(0, 40, 0));

            var cl = o.Clone(Vector3m.Zero());
            o.SweepVolume(cl, new Vector3m(70, 0, 0));
            TestFramework.CheckSanity(o);
        }




    }
}
