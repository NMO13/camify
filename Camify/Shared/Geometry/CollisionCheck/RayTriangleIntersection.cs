using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsEngine.Geometry;
using Shared;

namespace GraphicsEngine.Math
{
    class RayTriangleIntersection
    {
        internal static bool Intersect(Ray3d ray, Vector3d A, Vector3d B, Vector3d C, out bool hitFrontFace, bool two_sided, double[] tuv)
        {
            Vector3d E1 = B - A;
            Vector3d E2 = C - A;

            Vector3d n = E1.Cross(E2);
            if (n.Dot(ray.Direction) > 0.0) // angle > 0.0
            {
                hitFrontFace = false;   // if there is any intersection it would be with a backface
                if (two_sided)
                {
                    Vector3d E_TMP;
                    // so if the triangle is two_sided, switch edges for further computation
                    E_TMP = E1; E1 = E2; E2 = E_TMP;
                }
                else
                {
                    return false;      // otherwise return "no intersection"
                }
            }
            else
            {
                hitFrontFace = true; // no backface, so normal direction stays the same
            }

            Vector3d P = ray.Direction.Cross(E2);  // cross of ray-direction and edge2
            double det = E1.Dot(P);        // determinant
            if (System.Math.Abs(det) < Vector3d.Epsilon) return false;
            //    if( det < epsilon ) return false;

            Vector3d T = ray.Origin - A;   // vector: v0 -> ray-origin
            double u = T.Dot(P);        // param u, + testing bounds
            if (u < 0.0 || u > det) return false;

            Vector3d Q = T.Cross(E1); // cross of T and edge2
            double v = Q.Dot(ray.Direction);        // param v, + testing bounds
            if (v < 0.0 || (u + v) > det) return false;

            det = 1f / det;
            tuv[0] = det*Q.Dot(E2);
            tuv[1] = det * u;
            tuv[2] = det * v;
            return true;
        }
    }
}
