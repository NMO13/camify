using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Geometry
{
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
