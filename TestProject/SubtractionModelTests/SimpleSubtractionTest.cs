using CNCSpecific.Milling;
using Model;
using NUnit.Framework;
using Shared.Geometry;
using Shared.Helper;

namespace TestProject.SubtractionModelTests
{
    [TestFixture]
    class SimpleSubtraction
    {
        private MeshModel _meshModel;
        private SubtractionModel _subtractionModel;

        [SetUp]
        public void Setup()
        {
            _meshModel = new MeshModel();
            _subtractionModel = new SubtractionModel();
            _meshModel = new MeshModel();
            _meshModel.AttachModelObserver(_subtractionModel);
        }

        [Test]
        [Ignore] //TODO
        public void Subtraction1()
        {
            var program = new NCProgram();
            program.AddPath(new Vector3m(250, 0, 180), 0);
            program.AddPath(new Vector3m(0, -150, 0), 0);
            program.AddPath(new Vector3m(-450, 0, 0), 0);
            program.AddPath(new Vector3m(0, 0, -400), 0);
            program.AddPath(new Vector3m(380, 0, 0), 0);
            program.AddPath(new Vector3m(0, 0, 400), 0);
            program.AddPath(new Vector3m(-200, 0, -200), 0);
            program.AddPath(new Vector3m(200, 0, -200), 0);
            program.AddPath(new Vector3m(-200, 0, 200), 0);
            program.AddPath(new Vector3m(0, -30, 0), 0);
            program.AddPath(new Vector3m(0, 60, 0), 0);
            program.AddPath(new Vector3m(30, 0, 30), 0);
            program.AddPath(new Vector3m(0, -60, 0), 0);
            program.AddPath(new Vector3m(0, 60, 0), 0);
            program.AddPath(new Vector3m(-100, 0, -100), 0);
            program.AddPath(new Vector3m(0, -60, 0), 0);
            program.AddPath(new Vector3m(0, 60, 0), 0);
            program.AddPath(new Vector3m(0, 0, 100), 0);
            program.AddPath(new Vector3m(0, -60, 0), 0);
            _subtractionModel.NCProgram = program;

            var meshes = FileHelper.LoadFileFromDropbox(@"\BooleanOpEnv\Blender\Collada_Files\CNC_Milling\Cylinder1.dae");
            _meshModel.AddTools(meshes);

            var rps = FileHelper.LoadFileFromDropbox(@"\BooleanOpEnv\Blender\Collada_Files\CNC_Milling\Roughpart1.dae");
            _meshModel.AddRoughPart(rps[0]);

            _subtractionModel.BuildSnapshotList();
            var snapshotList = _subtractionModel.SnapshotList;
        }
    }
}
