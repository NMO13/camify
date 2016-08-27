using GeometryCalculation.DataStructures;
using GraphicsEngine.Geometry;
using NUnit.Framework;
using Shared.Geometry;

namespace BooleanSubtractorTests
{
    [TestFixture]
    class BoundingVolumeUpdateTest
    {
        [Test]
        public void Test1()
        {
            Vector3d[] verts =
            {
                new Vector3d(2, 2f, 0f),
                new Vector3d(4f, 2f, 0f),
                new Vector3d(3f, 4f, 0f),
                new Vector3d(6f, 3, 0f),
                new Vector3d(7f, 5, 0f),
                new Vector3d(5f, 5, 0f),
                new Vector3d(8f, 6, 0f),
                new Vector3d(10f, 6, 0f),
                new Vector3d(9f, 9, 0f),
            };

            int[] coords =
            {
                0, 1, 2,
                3, 4, 5,
                6, 7, 8
            };

            Mesh mesh = new Mesh(verts, coords, null, null);
            var obj2 = new DeformableObject(1);
            obj2.LoadMesh(mesh);
        }
    

        [Test]
        public void Test2()
        {
            Vector3d[] verts =
            {
                new Vector3d(2, 2f, 0f),
                new Vector3d(4f, 2f, 0f),
                new Vector3d(3f, 4f, 0f),

                new Vector3d(6f, 3, 0f),
                new Vector3d(7f, 5, 0f),
                new Vector3d(5f, 5, 0f),
                
                new Vector3d(8f, 6, 0f),
                new Vector3d(10f, 6, 0f),
                new Vector3d(9f, 9, 0f),
                
                new Vector3d(12f, 5, 0f),
                new Vector3d(13f, 2, 0f),
                new Vector3d(14f, 5, 0f),
                
                new Vector3d(140f, 50, 0f),
                new Vector3d(141f, 50, 0f),
                new Vector3d(141f, 51, 0f),
            };

            int[] coords =
            {
                0, 1, 2,
                3, 4, 5,
                6, 7, 8,
                9, 10, 11,
                12, 13, 14
            };

            Mesh mesh = new Mesh(verts, coords, null, null);
            var obj2 = new DeformableObject(1);
            obj2.LoadMesh(mesh);
        }
    }
}
