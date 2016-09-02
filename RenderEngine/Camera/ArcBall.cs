using System.Drawing;
using Shared.Geometry;

namespace GraphicsEngine.Rotation
{
    /*
     * This code is a part of NeHe tutorials and was converted from C++ into C#. 
     * Matrix/Vector manipulations have been updated.
     */
    public class Arcball
    {
        private const float Epsilon = 1.0e-5f;

        private Vector3F StVec; //Saved click vector
        private Vector3F EnVec; //Saved drag vector
        private float _adjustWidth; //Mouse bounds width
        private float _adjustHeight; //Mouse bounds height

        public Arcball(float newWidth, float newHeight)
        {
            StVec = new Vector3F();
            EnVec = new Vector3F();
            SetBounds(newWidth, newHeight);
        }

        private void MapToSphere(Point point, Vector3F vector)
        {
            Point2F tempPoint = new Point2F(point.X, point.Y);

            //Adjust point coords and scale down to range of [-1 ... 1]
            tempPoint.X = (tempPoint.X * _adjustWidth) - 1.0f;
            tempPoint.Y = 1.0f - (tempPoint.Y * _adjustHeight);

            //Compute square of the length of the vector from this point to the center
            float length = (tempPoint.X * tempPoint.X) + (tempPoint.Y * tempPoint.Y);

            //If the point is mapped outside the sphere... (length > radius squared)
            if (length > 1.0f)
            {
                //Compute a normalizing factor (radius / sqrt(length))
                float norm = (float)(1.0 / System.Math.Sqrt(length));

                //Return the "normalized" vector, a point on the sphere
                vector.X = tempPoint.X * norm;
                vector.Y = tempPoint.Y * norm;
                vector.Z = 0.0f;
            }
            //Else it's inside
            else
            {
                //Return a vector to a point mapped inside the sphere sqrt(radius squared - length)
                vector.X = tempPoint.X;
                vector.Y = tempPoint.Y;
                vector.Z = (float)System.Math.Sqrt(1.0f - length);
            }
        }

        public void SetBounds(float newWidth, float newHeight)
        {
            //Set adjustment factor for width/height
            _adjustWidth = 1.0f / ((newWidth - 1.0f) * 0.5f);
            _adjustHeight = 1.0f / ((newHeight - 1.0f) * 0.5f);
        }

        //Mouse down
        public virtual void Click(Point newPt)
        {
            MapToSphere(newPt, StVec);
        }

        //Mouse drag, calculate rotation
        public void Drag(Point newPt, Quat4F newRot)
        {
            //Map the point to the sphere
            MapToSphere(newPt, EnVec);

            //Return the quaternion equivalent to the rotation
            if (newRot != null)
            {
                Vector3F perp = new Vector3F();

                //Compute the vector perpendicular to the begin and end vectors
                Vector3F.Cross(perp, StVec, EnVec);

                //Compute the length of the perpendicular vector
                if (perp.Length() > Epsilon)
                //if its non-zero
                {
                    //We're ok, so return the perpendicular vector as the transform after all
                    newRot.X = perp.X;
                    newRot.Y = perp.Y;
                    newRot.Z = perp.Z;
                    //In the quaternion values, w is cosine (theta / 2), where theta is the rotation angle
                    newRot.W = Vector3F.Dot(StVec, EnVec);
                }
                //if it is zero
                else
                {
                    //The begin and end vectors coincide, so return an identity transform
                    newRot.X = newRot.Y = newRot.Z = newRot.W = 0.0f;
                }
            }
        }
    }
}