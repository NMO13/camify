using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNCSpecific.Milling;
using Model;
using NUnit.Framework;
using Shared;
using Shared.Geometry;

namespace TestProject.SubtractionModelTest
{
    [TestFixture]
    class SimpleSubtraction
    {
        [SetUp]
        public void Setup()
        {
            MeshModel m = new MeshModel();
            SubtractionModel s = new SubtractionModel();
            m.AttachModelObserver(s);
        }

        [Test]
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

        }
    }
}
