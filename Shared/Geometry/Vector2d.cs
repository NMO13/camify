using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Shared.Geometry
{
    [Serializable]
    public struct Vector2d : IEquatable<Vector2d>
    {
        public static readonly Vector2d UnitX = new Vector2d(1.0, 0.0);
        public static readonly Vector2d UnitY = new Vector2d(0.0, 1.0);
        public static readonly Vector2d Zero = new Vector2d(0.0, 0.0);
        public static readonly Vector2d One = new Vector2d(1.0, 1.0);
        public static readonly int SizeInBytes = Marshal.SizeOf((object)new Vector2d());
        private static string listSeparator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
        public double X;
        public double Y;

        public double this[int index]
        {
            get
            {
                if (index == 0)
                    return this.X;
                if (index == 1)
                    return this.Y;
                throw new IndexOutOfRangeException("You tried to access this vector at index: " + (object)index);
            }
            set
            {
                if (index == 0)
                {
                    this.X = value;
                }
                else
                {
                    if (index != 1)
                        throw new IndexOutOfRangeException("You tried to set this vector at index: " + (object)index);
                    this.Y = value;
                }
            }
        }

        public double Length
        {
            get
            {
                return Math.Sqrt(this.X * this.X + this.Y * this.Y);
            }
        }

        public double LengthSquared
        {
            get
            {
                return this.X * this.X + this.Y * this.Y;
            }
        }

        public Vector2d PerpendicularRight
        {
            get
            {
                return new Vector2d(this.Y, -this.X);
            }
        }

        public Vector2d PerpendicularLeft
        {
            get
            {
                return new Vector2d(-this.Y, this.X);
            }
        }

        [XmlIgnore]
        public Vector2d Yx
        {
            get
            {
                return new Vector2d(this.Y, this.X);
            }
            set
            {
                this.Y = value.X;
                this.X = value.Y;
            }
        }

        public Vector2d(double value)
        {
            this.X = value;
            this.Y = value;
        }

        public Vector2d(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Vector2d operator +(Vector2d left, Vector2d right)
        {
            left.X += right.X;
            left.Y += right.Y;
            return left;
        }

        public static Vector2d operator -(Vector2d left, Vector2d right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            return left;
        }

        public static Vector2d operator -(Vector2d vec)
        {
            vec.X = -vec.X;
            vec.Y = -vec.Y;
            return vec;
        }

        public static Vector2d operator *(Vector2d vec, double f)
        {
            vec.X *= f;
            vec.Y *= f;
            return vec;
        }

        public static Vector2d operator *(double f, Vector2d vec)
        {
            vec.X *= f;
            vec.Y *= f;
            return vec;
        }

        public static Vector2d operator *(Vector2d vec, Vector2d scale)
        {
            vec.X *= scale.X;
            vec.Y *= scale.Y;
            return vec;
        }

        public static Vector2d operator /(Vector2d vec, double f)
        {
            double num = 1.0 / f;
            vec.X *= num;
            vec.Y *= num;
            return vec;
        }

        public static bool operator ==(Vector2d left, Vector2d right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector2d left, Vector2d right)
        {
            return !left.Equals(right);
        }
   
        public Vector2d Normalized()
        {
            Vector2d vector2d = this;
            vector2d.Normalize();
            return vector2d;
        }

        public void Normalize()
        {
            double num = 1.0 / this.Length;
            this.X *= num;
            this.Y *= num;
        }

        public static Vector2d Add(Vector2d a, Vector2d b)
        {
            Vector2d.Add(ref a, ref b, out a);
            return a;
        }

        public static void Add(ref Vector2d a, ref Vector2d b, out Vector2d result)
        {
            result = new Vector2d(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2d Subtract(Vector2d a, Vector2d b)
        {
            Vector2d.Subtract(ref a, ref b, out a);
            return a;
        }

        public static void Subtract(ref Vector2d a, ref Vector2d b, out Vector2d result)
        {
            result = new Vector2d(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2d Multiply(Vector2d vector, double scale)
        {
            Vector2d.Multiply(ref vector, scale, out vector);
            return vector;
        }

        public static void Multiply(ref Vector2d vector, double scale, out Vector2d result)
        {
            result = new Vector2d(vector.X * scale, vector.Y * scale);
        }

        public static Vector2d Multiply(Vector2d vector, Vector2d scale)
        {
            Vector2d.Multiply(ref vector, ref scale, out vector);
            return vector;
        }

        public static void Multiply(ref Vector2d vector, ref Vector2d scale, out Vector2d result)
        {
            result = new Vector2d(vector.X * scale.X, vector.Y * scale.Y);
        }

        public static Vector2d Divide(Vector2d vector, double scale)
        {
            Vector2d.Divide(ref vector, scale, out vector);
            return vector;
        }

        public static void Divide(ref Vector2d vector, double scale, out Vector2d result)
        {
            Vector2d.Multiply(ref vector, 1.0 / scale, out result);
        }

        public static Vector2d Divide(Vector2d vector, Vector2d scale)
        {
            Vector2d.Divide(ref vector, ref scale, out vector);
            return vector;
        }

        public static void Divide(ref Vector2d vector, ref Vector2d scale, out Vector2d result)
        {
            result = new Vector2d(vector.X / scale.X, vector.Y / scale.Y);
        }

        public static Vector2d Min(Vector2d a, Vector2d b)
        {
            a.X = a.X < b.X ? a.X : b.X;
            a.Y = a.Y < b.Y ? a.Y : b.Y;
            return a;
        }

        public static void Min(ref Vector2d a, ref Vector2d b, out Vector2d result)
        {
            result.X = a.X < b.X ? a.X : b.X;
            result.Y = a.Y < b.Y ? a.Y : b.Y;
        }

        public static Vector2d Max(Vector2d a, Vector2d b)
        {
            a.X = a.X > b.X ? a.X : b.X;
            a.Y = a.Y > b.Y ? a.Y : b.Y;
            return a;
        }

        public static void Max(ref Vector2d a, ref Vector2d b, out Vector2d result)
        {
            result.X = a.X > b.X ? a.X : b.X;
            result.Y = a.Y > b.Y ? a.Y : b.Y;
        }

        public static Vector2d Clamp(Vector2d vec, Vector2d min, Vector2d max)
        {
            vec.X = vec.X < min.X ? min.X : (vec.X > max.X ? max.X : vec.X);
            vec.Y = vec.Y < min.Y ? min.Y : (vec.Y > max.Y ? max.Y : vec.Y);
            return vec;
        }

        public static void Clamp(ref Vector2d vec, ref Vector2d min, ref Vector2d max, out Vector2d result)
        {
            result.X = vec.X < min.X ? min.X : (vec.X > max.X ? max.X : vec.X);
            result.Y = vec.Y < min.Y ? min.Y : (vec.Y > max.Y ? max.Y : vec.Y);
        }

        public static Vector2d Normalize(Vector2d vec)
        {
            double num = 1.0 / vec.Length;
            vec.X *= num;
            vec.Y *= num;
            return vec;
        }

        public static void Normalize(ref Vector2d vec, out Vector2d result)
        {
            double num = 1.0 / vec.Length;
            result.X = vec.X * num;
            result.Y = vec.Y * num;
        }

        public static double Dot(Vector2d left, Vector2d right)
        {
            return left.X * right.X + left.Y * right.Y;
        }

        public static void Dot(ref Vector2d left, ref Vector2d right, out double result)
        {
            result = left.X * right.X + left.Y * right.Y;
        }

        public static Vector2d Lerp(Vector2d a, Vector2d b, double blend)
        {
            a.X = blend * (b.X - a.X) + a.X;
            a.Y = blend * (b.Y - a.Y) + a.Y;
            return a;
        }

        public static void Lerp(ref Vector2d a, ref Vector2d b, double blend, out Vector2d result)
        {
            result.X = blend * (b.X - a.X) + a.X;
            result.Y = blend * (b.Y - a.Y) + a.Y;
        }

        public static Vector2d BaryCentric(Vector2d a, Vector2d b, Vector2d c, double u, double v)
        {
            return a + u * (b - a) + v * (c - a);
        }

        public static void BaryCentric(ref Vector2d a, ref Vector2d b, ref Vector2d c, double u, double v, out Vector2d result)
        {
            result = a;
            Vector2d result1 = b;
            Vector2d.Subtract(ref result1, ref a, out result1);
            Vector2d.Multiply(ref result1, u, out result1);
            Vector2d.Add(ref result, ref result1, out result);
            Vector2d result2 = c;
            Vector2d.Subtract(ref result2, ref a, out result2);
            Vector2d.Multiply(ref result2, v, out result2);
            Vector2d.Add(ref result, ref result2, out result);
        }

        public override string ToString()
        {
            return string.Format("({0}{2} {1})", (object)this.X, (object)this.Y, (object)Vector2d.listSeparator);
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector2d))
                return false;
            return this.Equals((Vector2d)obj);
        }

        public bool Equals(Vector2d other)
        {
            if (this.X == other.X)
                return this.Y == other.Y;
            return false;
        }
    }
}

