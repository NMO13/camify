using System;
using System.Drawing;
using RenderEngine.Conversion;
using Shared.Geometry;

namespace GraphicsEngine.Rotation
{
    class WorldRotator
    {
        private Matrix4F LastTransformation = new Matrix4F();
        private Matrix4F ThisTransformation = new Matrix4F();
        private Arcball m_arcBall = new Arcball(0, 0);
        private float[] matrix = new float[16];
        private Boolean _isRotating;

        public WorldRotator()
        {
            _isRotating = false;
            LastTransformation.SetIdentity(); // Reset Rotation
            ThisTransformation.SetIdentity(); // Reset Rotation
            ThisTransformation.get_Renamed(matrix);
        }

        public void StartDrag(Point mousePt)
        {
            LastTransformation.SetRenamed(ThisTransformation); // Set Last Static Rotation To Last Dynamic One
            m_arcBall.Click(mousePt); // Update Start Vector And Prepare For Dragging
            _isRotating = true;
        }

        public void Drag(Point mousePt)
        {
            if (_isRotating)
            {
                Quat4F thisQuat = new Quat4F();
                m_arcBall.Drag(mousePt, thisQuat);
                ThisTransformation.Pan = new Vector3F(0, 0, 0);
                ThisTransformation.Scale = 1.0f;
                ThisTransformation.Rotation = thisQuat;
                Matrix4F.MatrixMultiply(ThisTransformation, LastTransformation);
                ThisTransformation.get_Renamed(matrix);
            }
        }

        public Matrix4d GetRotationMatrix()
        {
            return matrix.ToMatrix4D(false);
        }

        public void StopDrag()
        {
            _isRotating = false;
        }

        public void SetBounds(int width, int height)
        {
            m_arcBall.SetBounds(width, height);
        }
    }
}
