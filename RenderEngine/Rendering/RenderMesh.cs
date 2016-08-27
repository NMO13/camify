using Shared.Geometry;
using Matrix4d = Shared.Geometry.Matrix4d;

namespace RenderEngine.Rendering
{
    class RenderMesh : RenderObject
    {
        
        private Matrix4d transformationMatrix;

        public RenderMesh(Vector3d[] vertices, int[] indices, Matrix4d transformations) : base(vertices, indices)
        {

            SetupMesh();
            SetupTransformations(transformations);
        }

        public Matrix4d GetTransformationMatrix()
        {
            return transformationMatrix;
        }

        private void SetupTransformations(Matrix4d transformations)
        {
            //transformationMatrix.M11 = transformations.A1;
            //transformationMatrix.M12 = transformations.B1;
            //transformationMatrix.M13 = transformations.C1;
            //transformationMatrix.M14 = transformations.D1;

            //transformationMatrix.M21 = transformations.A2;
            //transformationMatrix.M22 = transformations.B2;
            //transformationMatrix.M23 = transformations.C2;
            //transformationMatrix.M24 = transformations.D2;

            //transformationMatrix.M31 = transformations.A3;
            //transformationMatrix.M32 = transformations.B3;
            //transformationMatrix.M33 = transformations.C3;
            //transformationMatrix.M34 = transformations.D3;

            //transformationMatrix.M41 = transformations.A4;
            //transformationMatrix.M42 = transformations.B4;
            //transformationMatrix.M43 = transformations.C4;
            //transformationMatrix.M44 = transformations.D4;
        }

        private void SetupMesh()
        {
          
        }
    }
}
