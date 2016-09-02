using System;
using System.Linq;
using Shared.Geometry;
using SharedMatrix4d = Shared.Geometry.Matrix4d;
using OpenTKMatrix4 = OpenTK.Matrix4;

namespace RenderEngine.Conversion
{
    public static class Extensions
    {
        internal static OpenTKMatrix4 ToOpenTKMatrix4D(this SharedMatrix4d matrix)
        {
            var openTkMatrix4D = new OpenTKMatrix4();
            openTkMatrix4D.M11 = (float) matrix.M11;
            openTkMatrix4D.M12 = (float) matrix.M12;
            openTkMatrix4D.M13 = (float) matrix.M13;
            openTkMatrix4D.M14 = (float) matrix.M14;

            openTkMatrix4D.M21 = (float) matrix.M21;
            openTkMatrix4D.M22 = (float) matrix.M22;
            openTkMatrix4D.M23 = (float) matrix.M23;
            openTkMatrix4D.M24 = (float) matrix.M24;

            openTkMatrix4D.M31 = (float) matrix.M31;
            openTkMatrix4D.M32 = (float) matrix.M32;
            openTkMatrix4D.M33 = (float) matrix.M33;
            openTkMatrix4D.M34 = (float) matrix.M34;

            openTkMatrix4D.M41 = (float) matrix.M41;
            openTkMatrix4D.M42 = (float) matrix.M42;
            openTkMatrix4D.M43 = (float) matrix.M43;
            openTkMatrix4D.M44 = (float) matrix.M44;

            return openTkMatrix4D;
        }

        internal static Vector3d Muliply(this Matrix4d matrix, Vector3d v)
        {
            var x = matrix.M11 * v.X + matrix.M12 * v.Y + matrix.M13 * v.Z + matrix.M14;
            var y = matrix.M21 * v.X + matrix.M22 * v.Y + matrix.M23 * v.Z + matrix.M24;
            var z = matrix.M31 * v.X + matrix.M32 * v.Y + matrix.M33 * v.Z + matrix.M34;
            return new Vector3d(x, y, z);
        }

        internal static SharedMatrix4d ToMatrix4D<T>(this T[] array, bool rowmajor)
        {
            if (array.Length != 16)
                throw new MException("Wrong dimensions of matrix!");

            float[] vals = array.ToArray() as float[];
            var m = new Matrix4d();
            if (rowmajor)
            {
                m.Column0 = new Vector4d(vals[0], vals[1], vals[2], vals[3]);
                m.Column1 = new Vector4d(vals[4], vals[5], vals[6], vals[7]);
                m.Column2 = new Vector4d(vals[8], vals[9], vals[10], vals[11]);
                m.Column3 = new Vector4d(vals[12], vals[13], vals[14], vals[15]);
            }
            else
            {
                m.Column0 = new Vector4d(vals[0], vals[4], vals[8], vals[12]);
                m.Column1 = new Vector4d(vals[1], vals[5], vals[9], vals[13]);
                m.Column2 = new Vector4d(vals[2], vals[6], vals[10], vals[14]);
                m.Column3 = new Vector4d(vals[3], vals[7], vals[11], vals[15]);
            }

            return m;
        }
    }
}
