using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace GeometryCalculation.DataStructures
{
    static class GeometryExtensions
    {
        public static Vector3d Multiply(this Matrix4d matrix, Vector3d v)
        {
            var x = matrix.M11 * v.X + matrix.M12 * v.Y + matrix.M13 * v.Z + matrix.M14;
            var y = matrix.M21 * v.X + matrix.M22 * v.Y + matrix.M23 * v.Z + matrix.M24;
            var z = matrix.M31 * v.X + matrix.M32 * v.Y + matrix.M33 * v.Z + matrix.M34;
            return new Vector3d(x, y, z);
        }
    }
}
