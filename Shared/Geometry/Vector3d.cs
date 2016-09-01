using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Geometry
{
    [Serializable]
    public class Vector3d : ICloneable
    {
        public const float Epsilon = 1e-7f;
        public Vector3d(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3d(Vector3d v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        public static Vector3d Zero()
        {
            return new Vector3d(0, 0, 0);
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public object Clone()
        {
            return new Vector3d(X, Y, Z);
        }

        public Vector3d ExplicitNegated()
        {
            return new Vector3d(-X, -Y, -Z);
        }

        public Vector3d Plus(Vector3d a)
        {
            return new Vector3d(this.X + a.X, this.Y + a.Y, this.Z + a.Z);
        }

        public Vector3d Minus(Vector3d a)
        {
            return new Vector3d(this.X - a.X, this.Y - a.Y, this.Z - a.Z);
        }

        public Vector3d Times(float a)
        {
            return new Vector3d(this.X * a, this.Y * a, this.Z * a);
        }

        public Vector3d DividedBy(float a)
        {
            return new Vector3d(this.X / a, this.Y / a, this.Z / a);
        }

        public double Dot(Vector3d a)
        {
            return this.X * a.X + this.Y * a.Y + this.Z * a.Z;
        }

        public Vector3d Lerp(Vector3d a, float t)
        {
            return this.Plus(a.Minus(this).Times(t));
        }

        public float Length()
        {
            return (float)System.Math.Sqrt(this.Dot(this));
        }

        public Vector3d Unit()
        {
            if (this.Length() == 0)
                return new Vector3d(0, 0, 0);
            return this.DividedBy(this.Length());
        }

        public Vector3d Cross(Vector3d a)
        {
            return new Vector3d(
            this.Y * a.Z - this.Z * a.Y,
            this.Z * a.X - this.X * a.Z,
            this.X * a.Y - this.Y * a.X
            );
        }

        public override bool Equals(object obj)
        {
            var other = obj as Vector3d;

            if (other == null)
            {
                return false;
            }

            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode();
        }

        public static Vector3d operator +(Vector3d a, Vector3d b)
        {
            return a.Plus(b);
        }

        public static Vector3d operator -(Vector3d a, Vector3d b)
        {
            return a.Minus(b);
        }

        public static Vector3d operator -(Vector3d a)
        {
            a.X = -a.X;
            a.Y = -a.Y;
            a.Z = -a.Z;
            return a;
        }

        public static Vector3d operator *(Vector3d a, double d)
        {
            return new Vector3d(a.X * d, a.Y * d, a.Z * d);
        }

        public static Vector3d operator /(Vector3d vec, double scale)
        {
            double mult = 1.0f / scale;
            vec.X *= mult;
            vec.Y *= mult;
            vec.Z *= mult;
            return vec;
        }

        public static explicit operator Vector3m(Vector3d b)
        {
            Vector3m d = new Vector3m(b.X, b.Y, b.Z);
            return d;
        }

        public override string ToString()
        {
            return "Vector:" + " " + X + " " + Y + " " + Z + " ";
        }

        public static Vector3d PlaneNormal(Vector3d v0, Vector3d v1, Vector3d v2)
        {
            Vector3d a = v1 - v0;
            Vector3d b = v2 - v0;
            return a.Cross(b);
        }

        public double LengthSquared
        {
            get
            {
                return X * X + Y * Y + Z * Z;
            }
        }
    }
}
