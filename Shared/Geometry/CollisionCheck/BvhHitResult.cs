using Shared.Geometry.HalfedgeMesh;

namespace Shared.Geometry.CollisionCheck
{
    class BvhHitResult
    {
        public Ray3d ray;
        public double t_max;
        public double t_min;
        public double t;
        public BoundingVolumeHierarchyNode node;
        public HeFace face = null;
  
        public bool two_sided_check = true;
        public bool hit_frontface = true;
        public double u, v;
        public bool got_hit = false;

        internal BvhHitResult(Ray3d ray, double min, double max)
        {
           Set(ray, min, max);  
        }

        internal bool CheckIfCloser(double[] tuv, HeFace face, BoundingVolumeHierarchyNode node, bool hit_frontface)
        {
            if (t_min < tuv[0] && tuv[0] < t)
            {
                this.t = tuv[0];
                this.u = tuv[1];
                this.v = tuv[2];
                this.face = face;
                this.node = node;
                this.hit_frontface = hit_frontface;
                this.got_hit = true;
                return true;
            }
            return false;
        }

        internal void Set(Ray3d ray, double t_min, double t_max)
        {
            this.ray = ray;
            this.t_min = t_min;
            this.t_max = t_max;
            this.t = t_max;
            this.face = null;
            this.got_hit = false;
        }
    }
}
