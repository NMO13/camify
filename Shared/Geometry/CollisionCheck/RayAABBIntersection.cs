using System;

namespace Shared.Geometry.CollisionCheck
{
    class RayAABBIntersection
    {
        private static double t_tmp;
        private static double t0_x, t0_y, t0_z;
        private static double t1_x, t1_y, t1_z;
        private static double tn, tf;

        static public double IntersectNear(Ray3d ray, AxisAlignedBoundingBox aabb)
        {
            Vector3d v = ray.Direction;
            v.X = 1 / v.X;
            v.Y = 1 / v.Y;
            v.Z = 1 / v.Z;
            t0_x = ((double)aabb.XMin-ray.Origin.X)*v.X;
            t1_x = ((double)aabb.XMax - ray.Origin.X) * v.X; 
            if( t0_x<0.0 && t1_x<0.0) return Double.MaxValue;

            t0_y = ((double)aabb.YMin - ray.Origin.Y) * v.Y;
            t1_y = ((double)aabb.YMax - ray.Origin.Y) * v.Y;
            if( t0_y<0.0 && t1_y<0.0) return Double.MaxValue;

            t0_z = ((double)aabb.ZMin - ray.Origin.Z) * v.Z;
            t1_z = ((double)aabb.ZMax - ray.Origin.Z) * v.Z;
                if (t0_z < 0.0 && t1_z < 0.0) return Double.MaxValue;
    
            // assure, that t0_xyz holds min values, and t1_xyz holds max values
            if( t0_x > t1_x) { t_tmp=t0_x; t0_x=t1_x; t1_x=t_tmp; }
            if( t0_y > t1_y) { t_tmp=t0_y; t0_y=t1_y; t1_y=t_tmp; }
            if( t0_z > t1_z) { t_tmp=t0_z; t0_z=t1_z; t1_z=t_tmp; }

            // get the max component of t0_xyz
            if( t0_x > t0_y){
              if( t0_x > t0_z) tn=t0_x; else tn=t0_z;
            } else {
              if( t0_y > t0_z) tn=t0_y; else tn=t0_z;
            }
    
            // get the min component of t1_xyz
            if( t1_x < t1_y){
              if( t1_x < t1_z) tf=t1_x; else tf=t1_z;
            } else {
              if( t1_y < t1_z) tf=t1_y; else tf=t1_z;
            }

            return (tn <= tf) ? tn : Double.MaxValue;
        }
    }
}
