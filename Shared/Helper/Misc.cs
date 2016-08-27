using System.Diagnostics;
using GraphicsEngine.HalfedgeMesh;
using Microsoft.SolverFoundation.Common;
using Shared.Geometry;
using Shared.Helper;

namespace GraphicsEngine.Math
{
    public class Misc
    {
        public static Rational CalcAngle(HeFace faceA, HeFace faceB, Vector3m testPointB)
        {
            var na = faceA.OuterComponent.Normal;
            var nb = faceB.OuterComponent.Normal.Negated();

            var max = na.Dot(na);
            var maxd = max.ToDouble();

            Rational res = 0;
            if (faceA.DistanceSign(testPointB) == -1)
            {
                if (na.Dot(nb) > 0) // 1st quarter
                {
                    res = MathHelper.Pow(na.Dot(nb), 2) / nb.Dot(nb);
                    res = max - res;
                }
                else if (na.Dot(nb) < 0) // 2nd quarter
                {
                    res = max + MathHelper.Pow(na.Dot(nb), 2) / nb.Dot(nb); ;
                }
                else // 0 => between 1st and 2nd quarter
                {
                    res = max;
                }
            }
            else if (faceA.DistanceSign(testPointB) == 1)
            {
                if (na.Dot(nb) > 0) // 4th quarter
                {
                    var tmp = MathHelper.Pow(na.Dot(nb), 2) / nb.Dot(nb);
                    tmp = max - tmp;
                    Debug.Assert(tmp > 0);
                    res = 3*max + tmp;
                }
                else if (na.Dot(nb) < 0) // 3rd quarter
                {
                    var tmp = MathHelper.Pow(na.Dot(nb), 2) / nb.Dot(nb);
                    tmp = max - tmp;
                    Debug.Assert(tmp > 0);
                    res = 2 * max + tmp;
                }
                else // 0 => between 3rd and 4th quarter
                {
                    res = 3*max;
                }
            }
            else
            {
                res = 2*max;
            }
            return res;
        }

        public static int GetOrientation(Vector3m v0, Vector3m v1, Vector3m v2, Vector3m normal)
        {
            var res = (v0 - v1).Cross(v2 - v1);
            if (res.LengthSquared() == 0)
                return 0;
            if (res.X.Sign != normal.X.Sign || res.Y.Sign != normal.Y.Sign || res.Z.Sign != normal.Z.Sign)
                return 1;
            return -1;
        }

        public static int GetOrientation(Vector2m he0, Vector2m he1, Vector2m he2)
        {
            // calculate determinant
            var xa = he0.X;
            var ya = he0.Y;

            var xb = he1.X;
            var yb = he1.Y;

            var xc = he2.X;
            var yc = he2.Y;

            var det = (xb - xa) * (yc - ya) - (xc - xa) * (yb - ya);
            return det < 0 ? -1 : det > 0 ? 1 : 0;
        }

        public static bool PointInOrOnTriangle(Vector3m prevPoint, Vector3m curPoint, Vector3m nextPoint, Vector3m nonConvexPoint, Vector3m normal)
        {
            var res0 = Misc.GetOrientation(prevPoint, nonConvexPoint, curPoint, normal);
            var res1 = Misc.GetOrientation(curPoint, nonConvexPoint, nextPoint, normal);
            var res2 = Misc.GetOrientation(nextPoint, nonConvexPoint, prevPoint, normal);
            return res0 != 1 && res1 != 1 && res2 != 1;
        }

        public static bool PointInOrOnTriangle(Vector2m prevPoint, Vector2m curPoint, Vector2m nextPoint, Vector2m nonConvexPoint)
        {
            var res0 = Misc.GetOrientation(prevPoint, nonConvexPoint, curPoint);
            var res1 = Misc.GetOrientation(curPoint, nonConvexPoint, nextPoint);
            var res2 = Misc.GetOrientation(nextPoint, nonConvexPoint, prevPoint);
            return res0 != 1 && res1 != 1 && res2 != 1;
        }

        // Is testPoint between a and b in ccw order?
        // > 0 if strictly yes
        // < 0 if strictly no
        // = 0 if testPoint lies either on a or on b
        public static int IsBetween(Vector2m Origin, Vector2m a, Vector2m b, Vector2m testPoint)
        {
            var psca = GetOrientation(Origin, a, testPoint);
            var pscb = GetOrientation(Origin, b, testPoint);

            // where does b in relation to a lie? Left, right or collinear?
            var psb = GetOrientation(Origin, a, b);
            if (psb > 0) // left
            {
                // if left then testPoint lies between a and b iff testPoint left of a AND testPoint right of b
                if (psca > 0 && pscb < 0)
                    return 1;
                if (psca == 0)
                {
                    var t = a - Origin;
                    var t2 = testPoint - Origin;
                    if (t.X.Sign != t2.X.Sign || t.Y.Sign != t2.Y.Sign)
                        return -1;
                    return 0;
                }
                else if (pscb == 0)
                {
                    var t = b - Origin;
                    var t2 = testPoint - Origin;
                    if (t.X.Sign != t2.X.Sign || t.Y.Sign != t2.Y.Sign)
                        return -1;
                    return 0;
                }
            }
            else if (psb < 0) // right
            {
                // if right then testPoint lies between a and b iff testPoint left of a OR testPoint right of b
                if (psca > 0 || pscb < 0)
                    return 1;
                if (psca == 0)
                {
                    var t = a - Origin;
                    var t2 = testPoint - Origin;
                    if (t.X.Sign != t2.X.Sign || t.Y.Sign != t2.Y.Sign)
                        return 1;
                    return 0;
                }
                else if (pscb == 0)
                {
                    var t = b - Origin;
                    var t2 = testPoint - Origin;
                    if (t.X.Sign != t2.X.Sign || t.Y.Sign != t2.Y.Sign)
                        return 1;
                    return 0;
                }
            }
            else if (psb == 0)
            {
                if (psca > 0)
                    return 1;
                else if (psca < 0)
                    return -1;
                else
                    return 0;
            }
            return -1;
        }

        public static Rational PointLineDistance(Vector3m p1, Vector3m p2, Vector3m p3)
        {
            return (p2 - p1).Cross(p3 - p1).LengthSquared();
        }

        // Is testPoint between a and b in ccw order?
        // > 0 if strictly yes
        // < 0 if strictly no
        // = 0 if testPoint lies either on a or on b
        public static int IsBetween(Vector3m Origin, Vector3m a, Vector3m b, Vector3m testPoint, Vector3m normal)
        {
            var psca = GetOrientation(Origin, a, testPoint, normal);
            var pscb = GetOrientation(Origin, b, testPoint, normal);

            // where does b in relation to a lie? Left, right or collinear?
            var psb = GetOrientation(Origin, a, b, normal);
            if (psb > 0) // left
            {
                // if left then testPoint lies between a and b iff testPoint left of a AND testPoint right of b
                if (psca > 0 && pscb < 0)
                    return 1;
                if (psca == 0)
                {
                    var t = a - Origin;
                    var t2 = testPoint - Origin;
                    if (t.X.Sign != t2.X.Sign || t.Y.Sign != t2.Y.Sign)
                        return -1;
                    return 0;
                }
                else if (pscb == 0)
                {
                    var t = b - Origin;
                    var t2 = testPoint - Origin;
                    if (t.X.Sign != t2.X.Sign || t.Y.Sign != t2.Y.Sign)
                        return -1;
                    return 0;
                }
            }
            else if (psb < 0) // right
            {
                // if right then testPoint lies between a and b iff testPoint left of a OR testPoint right of b
                if (psca > 0 || pscb < 0)
                    return 1;
                if (psca == 0)
                {
                    var t = a - Origin;
                    var t2 = testPoint - Origin;
                    if (t.X.Sign != t2.X.Sign || t.Y.Sign != t2.Y.Sign)
                        return 1;
                    return 0;
                }
                else if (pscb == 0)
                {
                    var t = b - Origin;
                    var t2 = testPoint - Origin;
                    if (t.X.Sign != t2.X.Sign || t.Y.Sign != t2.Y.Sign)
                        return 1;
                    return 0;
                }
            }
            else if (psb == 0)
            {
                if (psca > 0)
                    return 1;
                else if (psca < 0)
                    return -1;
                else
                    return 0;
            }
            return -1;
        }

    }
}
