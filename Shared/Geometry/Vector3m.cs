using Microsoft.SolverFoundation.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsEngine;
using GraphicsEngine.Math;
using Shared.Additional;
using Shared.Helper;

namespace Shared.Geometry
{
    public class Vector3m : ICloneable
    {
        public DynamicProperties DynamicProperties = new DynamicProperties();

        public Vector3m(Rational x, Rational y, Rational z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3m(Vector3m v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        public Vector3m(Vector3d v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        public static Vector3m Zero()
        {
            return new Vector3m(0, 0, 0);
        }

        public Rational X { get; set; }

        public Vector3m Absolute()
        {
            return new Vector3m(X.AbsoluteValue, Y.AbsoluteValue, Z.AbsoluteValue);
        }

        public Rational Y { get; set; }
        public Rational Z { get; set; }

        public object Clone()
        {
            return new Vector3m(X, Y, Z);
        }

        public void ImplizitNegated()
        {
            X = -X; Y = -Y; Z = -Z;
        }

        public Vector3m Negated()
        {
            return new Vector3m(-X, -Y, -Z);
        }

        public Vector3m Plus(Vector3m a)
        {
            return new Vector3m(this.X + a.X, this.Y + a.Y, this.Z + a.Z);
        }

        public Vector3m Minus(Vector3m a)
        {
            return new Vector3m(this.X - a.X, this.Y - a.Y, this.Z - a.Z);
        }

        public Vector3m Times(Rational a)
        {
            return new Vector3m(this.X * a, this.Y * a, this.Z * a);
        }

        public Vector3m DividedBy(Rational a)
        {
            return new Vector3m(this.X / a, this.Y / a, this.Z / a);
        }

        public Rational Dot(Vector3m a)
        {
            return this.X * a.X + this.Y * a.Y + this.Z * a.Z;
        }

        public Vector3m Lerp(Vector3m a, Rational t)
        {
            return this.Plus(a.Minus(this).Times(t));
        }

        public double Length()
        {
            return System.Math.Sqrt(Dot(this).ToDouble());
        }

        public Rational LengthSquared()
        {
            return Dot(this);
        }

        public Vector3m ShortenByLargestComponent()
        {
            if (this.LengthSquared() == 0)
                return new Vector3m(0, 0, 0);
            var absNormal = Absolute();
            Rational largestValue = 0;
            if (absNormal.X >= absNormal.Y && absNormal.X >= absNormal.Z)
                largestValue = absNormal.X;
            else if (absNormal.Y >= absNormal.X && absNormal.Y >= absNormal.Z)
                largestValue = absNormal.Y;
            else
            {
                largestValue = absNormal.Z;
            }
            Debug.Assert(largestValue != 0);
            return this / largestValue;
        }

        public Vector3m Cross(Vector3m a)
        {
            return new Vector3m(
            this.Y * a.Z - this.Z * a.Y,
            this.Z * a.X - this.X * a.Z,
            this.X * a.Y - this.Y * a.X
            );
        }

        public bool IsZero()
        {
            return X == 0 && Y == 0 && Z == 0;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Vector3m;

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

        public static Vector3m operator +(Vector3m a, Vector3m b)
        {
            return a.Plus(b);
        }

        public static Vector3m operator -(Vector3m a, Vector3m b)
        {
            return a.Minus(b);
        }

        public static Vector3m operator *(Vector3m a, Rational d)
        {
            return new Vector3m(a.X * d, a.Y * d, a.Z * d);
        }

        public static Vector3m operator /(Vector3m a, Rational d)
        {
            return a.DividedBy(d);
        }

        public override string ToString()
        {
            return "Vector:" + " " + X.ToDouble() + " " + Y.ToDouble() + " " + Z.ToDouble() + " ";
        }

        public static explicit operator Vector3d(Vector3m b)
        {
            Vector3d d = new Vector3d((float)b.X.ToDouble(), (float)b.Y.ToDouble(), (float)b.Z.ToDouble());
            return d;
        }

        public static Vector3m PlaneNormal(Vector3m v0, Vector3m v1, Vector3m v2)
        {
            Vector3m a = v1 - v0;
            Vector3m b = v2 - v0;
            return a.Cross(b);
        }

        public Rational DistanceSquared(Vector3m point)
        {
            Rational xDistSquared = MathHelper.Pow(this.X - point.X, 2);
            Rational yDistSquared = MathHelper.Pow(this.Y - point.Y, 2);
            Rational zDistSquared = MathHelper.Pow(this.Z - point.Z, 2);
            return (xDistSquared + yDistSquared + zDistSquared);
        }

        public Vector3d Vector3d
        {
            get { return new Vector3d((float)X.ToDouble(), (float)Y.ToDouble(), (float)Z.ToDouble()); }
        }

        public bool SameDirection(Vector3m he)
        {
            var res = this.Cross(he);
            return res.X == 0 && res.Y == 0 && res.Z == 0;
        }
    }
}
