using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry.Bounding_Volume_Hierarchy;
using GraphicsEngine.Math;
using GraphicsEngine.Selection;
using Shared;

namespace GraphicsEngine.Geometry.CollisionCheck
{
    class BvhRayCheck
    {
        /* private List<BvhTraversalData> stack;
         private BoundingVolumeHierarchy bvh;

         internal BvhRayCheck(BoundingVolumeHierarchy bvh)
         {
             this.bvh = bvh;
             this.stack = new List<BvhTraversalData>();
         }*/

        internal static void TraverseStackSorted(BvhHitResult hitResult, BoundingVolumeHierarchyNode root, int maxElemns)
        {
            double tn = RayAABBIntersection.IntersectNear(hitResult.ray, root.AABB);
            if (tn >= 1.0)
                return;
            int stackPtr = 0;
            BvhTraversalData[] stack = new BvhTraversalData[maxElemns];
            InsertSorted(stack, stackPtr, root, tn);

            while (stackPtr >= 0)
            {
                BvhTraversalData cur = stack[stackPtr--];
                if (cur.tn >= 1.0) return;

                if (cur.Node.IsLeaf())
                {
                    IntersectRayObjects(cur.Node, hitResult);
                    continue;
                }

                BoundingVolumeHierarchyNode A = cur.Node.Left;
                BoundingVolumeHierarchyNode B = cur.Node.Right;
                double ta = RayAABBIntersection.IntersectNear(hitResult.ray, A.AABB);
                double tb = RayAABBIntersection.IntersectNear(hitResult.ray, B.AABB);

                if (ta > tb)
                {
                    if (ta < 1.0) InsertSorted(stack, ++stackPtr, A, ta);
                    if (tb < 1.0) InsertSorted(stack, ++stackPtr, B, tb);
                }
                else
                {
                    if (tb < 1.0) InsertSorted(stack, ++stackPtr, B, tb);
                    if (ta < 1.0) InsertSorted(stack, ++stackPtr, A, ta);
                }
            }
        }

        private static void InsertSorted(BvhTraversalData[] stack, int ptr, BoundingVolumeHierarchyNode node,
            double tn)
        {
            while ((ptr > 0) && (tn > stack[ptr - 1].tn))
            {
                stack[ptr] = stack[--ptr];
            }
            stack[ptr] = new BvhTraversalData(node, tn);
        }

        public class BvhTraversalData
        {
            public BoundingVolumeHierarchyNode Node;
            public double tn;

            public BvhTraversalData(BoundingVolumeHierarchyNode node, double tn)
            {
                this.Node = node;
                this.tn = tn;
            }
        }

        private static bool IntersectRayObjects(BoundingVolumeHierarchyNode node, BvhHitResult hit_result)
        {
            bool hitFrontFace = true;
            double[] tuv = new double[3];
            bool gotHit = false;
            foreach (var face in node.Faces)
            {
                Vector3d A = (Vector3d) face.OuterComponent.Origin.Vector3m;
                Vector3d B = (Vector3d) face.OuterComponent.Next.Origin.Vector3m;
                Vector3d C = (Vector3d) face.OuterComponent.Next.Next.Origin.Vector3m;

                if (RayTriangleIntersection.Intersect(hit_result.ray, A, B, C, out hitFrontFace, hit_result.two_sided_check, tuv))
                {
                    gotHit = true;
                    hit_result.CheckIfCloser(tuv, face, node, hitFrontFace);
                }
            }
            return gotHit;
        }
    }
}
