using System.Drawing;

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

    public class Matrix4F
    {
        private float[,] _m;
        private float _scl = 1.0f;
        private Vector3F _pan = new Vector3F();

        public Matrix4F()
        {
            SetIdentity();
        }

        public void get_Renamed(float[] dest)
        {
            int k = 0;
            for (int i = 0; i <= 3; i++)
                for (int j = 0; j <= 3; j++)
                {
                    dest[k] = _m[j, i];
                    k++;
                }
        }

        public void SetIdentity()
        {
            _m = new float[4, 4]; // set to zero
            for (int i = 0; i <= 3; i++) _m[i, i] = 1.0f;
        }

        public void set_Renamed(Matrix4F m1)
        {
            _m = m1._m;
        }

        public static void MatrixMultiply(Matrix4F m1, Matrix4F m2)
        {
            float[] mulMat = new float[16];
            float elMat = 0.0f;
            int k = 0;

            for (int i = 0; i <= 3; i++)
                for (int j = 0; j <= 3; j++)
                {
                    for (int l = 0; l <= 3; l++) elMat += m1._m[i, l] * m2._m[l, j];
                    mulMat[k] = elMat;
                    elMat = 0.0f;
                    k++;
                }

            k = 0;
            for (int i = 0; i <= 3; i++)
                for (int j = 0; j <= 3; j++)
                {
                    m1._m[i, j] = mulMat[k];
                    k++;
                }
        }

        public Quat4F Rotation
        {
            set
            {
                float n, s;
                float xs, ys, zs;
                float wx, wy, wz;
                float xx, xy, xz;
                float yy, yz, zz;

                _m = new float[4, 4];

                n = (value.X * value.X) + (value.Y * value.Y) + (value.Z * value.Z) + (value.W * value.W);
                s = (n > 0.0f) ? 2.0f / n : 0.0f;

                xs = value.X * s;
                ys = value.Y * s;
                zs = value.Z * s;
                wx = value.W * xs;
                wy = value.W * ys;
                wz = value.W * zs;
                xx = value.X * xs;
                xy = value.X * ys;
                xz = value.X * zs;
                yy = value.Y * ys;
                yz = value.Y * zs;
                zz = value.Z * zs;

                // rotation
                _m[0, 0] = 1.0f - (yy + zz);
                _m[0, 1] = xy - wz;
                _m[0, 2] = xz + wy;

                _m[1, 0] = xy + wz;
                _m[1, 1] = 1.0f - (xx + zz);
                _m[1, 2] = yz - wx;

                _m[2, 0] = xz - wy;
                _m[2, 1] = yz + wx;
                _m[2, 2] = 1.0f - (xx + yy);

                _m[3, 3] = 1.0f;

                // translation (pan)
                _m[0, 3] = _pan.X;
                _m[1, 3] = _pan.Y;

                // scale (zoom)
                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                        _m[i, j] *= _scl;


            }
        }

        public float Scale
        {
            set { _scl = value; }

        }

        public Vector3F Pan
        {
            set { _pan = value; }

        }

    }

    public class Point2F
    {
        public float X, Y;

        public Point2F(float x, float y)
        {
            X = x;
            Y = y;
        }
    }

    public class Quat4F
    {
        public float X, Y, Z, W;
    }

    public class Vector3F
    {
        public float X, Y, Z;

        /// <summary>
        /// Constructor
        /// </summary>
        public Vector3F()
        { }

        /// <summary>
        /// Constructor - overload 1
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vector3F(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Cross Product of Two Vectors.
        /// </summary>
        /// <param name="result">Resultant Vector</param>
        /// <param name="v1">Vector 1</param>
        /// <param name="v2">Vector 2</param>
        public static void Cross(Vector3F result, Vector3F v1, Vector3F v2)
        {
            result.X = (v1.Y * v2.Z) - (v1.Z * v2.Y);
            result.Y = (v1.Z * v2.X) - (v1.X * v2.Z);
            result.Z = (v1.X * v2.Y) - (v1.Y * v2.X);
        }

        /// <summary>
        /// Dot Product of Two Vectors.
        /// </summary>
        /// <param name="v1">Vector 1</param>
        /// <param name="v2">Vector 2</param>
        /// <returns></returns>
        public static float Dot(Vector3F v1, Vector3F v2)
        {
            return (v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public float Length()
        {
            return (float)System.Math.Sqrt(X * X + Y * Y + Z * Z);
        }
    }


}