using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooleanSubractorTests;
using DataManagement;
using GeometryCalculation.DataStructures;
using GraphicsEngine;
using GraphicsEngine.Geometry;
using GraphicsEngine.Geometry.Boolean_Ops;
using GraphicsEngine.Geometry.Meshes;
using GraphicsEngine.Math;
using NUnit.Framework;
using Shared;
using Shared.Geometry;

namespace BooleanOpEnv
{
    [TestFixture]
    class SubtractionChainTest
    {
        private BooleanTester _bTester;

        [SetUp]
        public void Setup()
        {
            _bTester = new BooleanTester();
        }

        [Test]
        public void Chain1()
        {
            var rp = DefaultMeshes.Box(200, 40, 200);
            var roughpart = new DeformableObject();
            roughpart.Initialize(rp);

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox("\\BooleanOpEnv\\Blender\\Collada_Files\\CNC_Milling\\Cylinder1.dae");
            DeformableObject tool = new DeformableObject();
            tool.Initialize(meshes[0]);
            tool.CheckSanity();
            tool.Translate(new Vector3m(0, 200, 0));
            tool.Translate(new Vector3m(250, 0, 180));
            tool.Translate(new Vector3m(0, -150, 0));

            var tsv = tool.Clone(Vector3m.Zero());

            var translate = new Vector3m(-450, 0, 0);
            tsv.SweepVolume(tool, translate);
            BooleanModeller.SubtractSweptVolume(roughpart, tsv);

            tool.Translate(new Vector3m(translate.X, translate.Y, translate.Z));

            tool.CheckSanity();
            tsv.CheckSanity();
            roughpart.CheckSanity();

            translate = new Vector3m(0, 0, -400);
            tsv.SweepVolume(tool, translate);
            BooleanModeller.SubtractSweptVolume(roughpart, tsv);

        }

        [Test]
        public void EdgeOnFace3PlusCube()
        {
            DeformableObject obj = new DeformableObject(1);
            DeformableObject obj2 = new DeformableObject(1);

            List<Mesh> meshes = FileHelper.LoadFileFromDropbox(@"\\BooleanOpEnv\\Blender\\EdgeEdgeIntersectionTest\\EdgeOnFace3.dae");
            Mesh mesh = meshes[0];
            Mesh mesh2 = meshes[1];

            obj.Initialize(mesh);
            obj2.Initialize(mesh2);

            _bTester.Test(obj, obj2, true, 3, 29, 174, 58);
            obj.BuildBvh();

            DeformableObject obj3 = new DeformableObject(1);
            var mesh3 = DefaultMeshes.Box(50, 50, 50);
            obj3.Initialize(mesh3);
            obj3.TranslateAndBuildBvh(new Vector3m(0, 0, 90));

            _bTester.Test(obj, obj3, true, 3, 71, 426, 142);
            obj.BuildBvh();

            DeformableObject obj4 = new DeformableObject(1);
            obj4.Initialize(mesh3);
            obj4.TranslateAndBuildBvh(new Vector3m(0, 0, 90));
            obj4.TranslateAndBuildBvh(new Vector3m(70, 0, 0));

            _bTester.Test(obj, obj4, true, 3, 92, 540, 180);
            obj.BuildBvh();

            obj4.TranslateAndBuildBvh(new Vector3m(0, 0, -30.007));
            _bTester.Test(obj, obj4, true, 3, 117, 690, 230);
        }
    }
}
