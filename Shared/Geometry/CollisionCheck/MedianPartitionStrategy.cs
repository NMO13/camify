using System.Collections.Generic;
using System.Diagnostics;
using Geometry.Bounding_Volume_Hierarchy;
using GraphicsEngine.Geometry.CollisionCheck;
using GraphicsEngine.HalfedgeMesh;
using Microsoft.SolverFoundation.Common;
using Shared.Additional;

namespace GraphicsEngine.Geometry
{
    class MedianPartitionStrategy
    {
        private readonly IComparer<HeFace> _compareX = new CompareX();
        private readonly IComparer<HeFace> _compareY = new CompareY();
        private readonly IComparer<HeFace> _compareZ = new CompareZ();

        internal void ParitionObjects(AxisAlignedBoundingBox box, List<HeFace> left, List<HeFace> right, HeFace[] list)
        {
            // split along the longer axis (x or y)
            Debug.Assert(list != null);
            Debug.Assert(list.Length >= 2);
            Debug.Assert(left.Count == 0 && right.Count == 0);

            var lengthX = box.XMax - box.XMin;
            var lengthY = box.YMax - box.YMin;
            var lengthZ = box.ZMax - box.ZMin;

            if (lengthX >= lengthY && lengthX >= lengthZ)
            {
                var objectMedian = box.XMin + (box.XMax - box.XMin) / 2;
                foreach (var h in list)
                {
                    var median = h.Aabb.XMin + (h.Aabb.XMax - h.Aabb.XMin) / 2;
                    h.DynamicProperties.ChangeValue(PropertyConstants.Median, median);
                    if (median <= objectMedian)
                        left.Add(h);
                    else
                        right.Add(h);
                }
                left.Sort(new Smaller());
                right.Sort(new Smaller());
            }
            else if (lengthY >= lengthX && lengthY >= lengthZ)
            {
                var objectMedian = box.YMin + (box.YMax - box.YMin) / 2;
                foreach (var h in list)
                {
                    var median = h.Aabb.YMin + (h.Aabb.YMax - h.Aabb.YMin) / 2;
                    h.DynamicProperties.ChangeValue(PropertyConstants.Median, median);
                    if (median <= objectMedian)
                        left.Add(h);
                    else
                        right.Add(h);
                }
                left.Sort(new Smaller());
                right.Sort(new Smaller());
            }
            else
            {
                var objectMedian = box.ZMin + (box.ZMax - box.ZMin) / 2;
                foreach (var h in list)
                {
                    var median = h.Aabb.ZMin + (h.Aabb.ZMax - h.Aabb.ZMin) / 2;
                    h.DynamicProperties.ChangeValue(PropertyConstants.Median, median);
                    if (median <= objectMedian)
                        left.Add(h);
                    else
                        right.Add(h);
                }
                left.Sort(new Smaller());
                right.Sort(new Smaller());
            }

            // make sure that left and right have at least 1 element
            if (left.Count == 0)
            {
                Debug.Assert(right.Count >= 2);
                var e = right[0];
                right.RemoveAt(0);
                left.Add(e);
            }

            if (right.Count == 0)
            {
                Debug.Assert(left.Count >= 2);
                var e = left[left.Count - 1];
                left.RemoveAt(left.Count - 1);
                right.Add(e);
            }
        }

        class CompareX : IComparer<HeFace>
        {
            public int Compare(HeFace p1, HeFace p2)
            {
                var lineMedian1 = p1.Aabb.XMin + (p1.Aabb.XMax - p1.Aabb.XMin) / 2;
                var lineMedian2 = p2.Aabb.XMin + (p2.Aabb.XMax - p2.Aabb.XMin) / 2;
                if (lineMedian1 < lineMedian2)
                    return -1;
                if (lineMedian1 > lineMedian2)
                    return 1;
                return 0;
            }
        }

        class Smaller : IComparer<HeFace>
        {
            public int Compare(HeFace p1, HeFace p2)
            {
                if ((Rational) p1.DynamicProperties.GetValue(PropertyConstants.Median) < (Rational) p2.DynamicProperties.GetValue(PropertyConstants.Median))
                    return -1;
                if ((Rational)p1.DynamicProperties.GetValue(PropertyConstants.Median) > (Rational)p2.DynamicProperties.GetValue(PropertyConstants.Median))
                    return 1;
                return 0;
            }
        }

        class CompareY : IComparer<HeFace>
        {
            public int Compare(HeFace p1, HeFace p2)
            {
                var lineMedian1 = p1.Aabb.YMin + (p1.Aabb.YMax - p1.Aabb.YMin) / 2;
                var lineMedian2 = p2.Aabb.YMin + (p2.Aabb.YMax - p2.Aabb.YMin) / 2;
                if (lineMedian1 < lineMedian2)
                    return -1;
                if (lineMedian1 > lineMedian2)
                    return 1;
                return 0;
            }
        }

        class CompareZ : IComparer<HeFace>
        {
            public int Compare(HeFace p1, HeFace p2)
            {
                var lineMedian1 = p1.Aabb.ZMin + (p1.Aabb.ZMax - p1.Aabb.ZMin) / 2;
                var lineMedian2 = p2.Aabb.ZMin + (p2.Aabb.ZMax - p2.Aabb.ZMin) / 2;
                if (lineMedian1 < lineMedian2)
                    return -1;
                if (lineMedian1 > lineMedian2)
                    return 1;
                return 0;
            }
        }
    }
}
