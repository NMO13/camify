using System.Collections.Generic;
using GeometryCalculation.DataStructures;
using NUnit.Framework;
using Shared.Additional;
using Shared.Geometry;
using Shared.Helper;

namespace BooleanSubtractorTests
{
    [TestFixture]
    class ContourCalculationTest
    {
        [Test]
        public void SimpleContour1()
        {
            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\MeshSimplificationTest\\SimpleCube.dae");
            Mesh mesh = meshes[0];
            DeformableObject obj = new DeformableObject();
            obj.Initialize(mesh);
            obj.AddPostProcessStep(DeformableObject.PostprocessAlgorithm.ContourCalculator);
            obj.ExecutePostProcesses();
            var contourGroups = obj.ContourGroupManager.ContourGroups;
            Assert.AreEqual(contourGroups.Count, 6);

            Assert.AreEqual(contourGroups[0].InsideFaces.Count, 2);
            Assert.AreEqual(contourGroups[0].Contours.Count, 1);

            Assert.AreEqual(contourGroups[1].InsideFaces.Count, 5);
            Assert.AreEqual(contourGroups[1].Contours.Count, 1);

            Assert.AreEqual(contourGroups[2].InsideFaces.Count, 3);
            Assert.AreEqual(contourGroups[2].Contours.Count, 1);

            Assert.AreEqual(contourGroups[3].InsideFaces.Count, 2);
            Assert.AreEqual(contourGroups[3].Contours.Count, 1);

            Assert.AreEqual(contourGroups[4].InsideFaces.Count, 2);
            Assert.AreEqual(contourGroups[4].Contours.Count, 1);

            Assert.AreEqual(contourGroups[5].InsideFaces.Count, 2);
            Assert.AreEqual(contourGroups[5].Contours.Count, 1);

            foreach (var heFace in obj.HeMesh.FaceList)
            {
                Assert.False(heFace.DynamicProperties.ExistsKey(PropertyConstants.Marked));
            }

            Assert.AreEqual(obj.HeMesh.VertexList.Count, 10);
        }

        [Test]
        public void CubeWithHole()
        {
            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\MeshSimplificationTest\\CubeWithHole.dae");
            Mesh mesh = meshes[0];
            DeformableObject obj = new DeformableObject();
            obj.Initialize(mesh);
            obj.AddPostProcessStep(DeformableObject.PostprocessAlgorithm.ContourCalculator);
            obj.ExecutePostProcesses();
            var contourGroups = obj.ContourGroupManager.ContourGroups;
            Assert.AreEqual(contourGroups.Count, 11);
            Assert.AreEqual(contourGroups[0].InsideFaces.Count, 2);
            Assert.AreEqual(contourGroups[1].InsideFaces.Count, 8);
            Assert.AreEqual(contourGroups[2].InsideFaces.Count, 2);
            Assert.AreEqual(contourGroups[3].InsideFaces.Count, 2);
            Assert.AreEqual(contourGroups[4].InsideFaces.Count, 2);
            Assert.AreEqual(contourGroups[5].InsideFaces.Count, 2);
            Assert.AreEqual(contourGroups[6].InsideFaces.Count, 2);
            Assert.AreEqual(contourGroups[7].InsideFaces.Count, 2);
            Assert.AreEqual(contourGroups[8].InsideFaces.Count, 2);
            Assert.AreEqual(contourGroups[9].InsideFaces.Count, 2);
            Assert.AreEqual(contourGroups[0].InsideFaces.Count, 2);
            Assert.AreEqual(contourGroups[10].InsideFaces.Count, 2);
        }

        [Test]
        public void CubeWithTwoHoles()
        {
            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\MeshSimplificationTest\\CubeWithTwoHoles.dae");
            Mesh mesh = meshes[0];
            DeformableObject obj = new DeformableObject();
            obj.Initialize(mesh);
            obj.AddPostProcessStep(DeformableObject.PostprocessAlgorithm.ContourCalculator);
            obj.ExecutePostProcesses();
            var contourGroups = obj.ContourGroupManager.ContourGroups;
            Assert.AreEqual(contourGroups.Count, 15);
            Assert.AreEqual(contourGroups[1].Contours.Count, 2);
            Assert.AreEqual(contourGroups[14].Contours.Count, 1);
        }

        [Test]
        public void CubeWithThreeHoles()
        {
            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\MeshSimplificationTest\\CubeWithThreeHoles.dae");
            Mesh mesh = meshes[0];
            DeformableObject obj = new DeformableObject();
            obj.Initialize(mesh);
            obj.AddPostProcessStep(DeformableObject.PostprocessAlgorithm.ContourCalculator);
            obj.ExecutePostProcesses();
            var contourGroups = obj.ContourGroupManager.ContourGroups;
            Assert.AreEqual(contourGroups.Count, 19);
            Assert.AreEqual(contourGroups[1].Contours.Count, 2);
            Assert.AreEqual(contourGroups[14].Contours.Count, 1);
        }

        [Test]
        public void ComplexObject1()
        {
            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\ContourCalculationTest\\ComplexObject1.dae");
            Mesh mesh = meshes[0];
            DeformableObject obj = new DeformableObject();
            obj.Initialize(mesh);
            obj.AddPostProcessStep(DeformableObject.PostprocessAlgorithm.ContourCalculator);
            obj.ExecutePostProcesses();
            var contourGroups = obj.ContourGroupManager.ContourGroups;
            Assert.AreEqual(contourGroups.Count, 38);
        }
    }
}
