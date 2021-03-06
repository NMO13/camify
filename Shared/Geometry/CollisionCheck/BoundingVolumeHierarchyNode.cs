﻿using System.Diagnostics;
using Shared.Geometry.HalfedgeMesh;

namespace Shared.Geometry.CollisionCheck
{
    public class BoundingVolumeHierarchyNode
    {
        protected int m_Depth;
        protected AxisAlignedBoundingBox MAabb;
        public HeFace[] Faces;
        public int ItemCount { get; private set; }
        public BoundingVolumeHierarchyNode Left, Right;
        internal int Id;

        public AxisAlignedBoundingBox AABB { get { return MAabb; } }
        internal BoundingVolumeHierarchyNode(int id, int depth, AxisAlignedBoundingBox aabb, int itemCount)
        {
            Id = id;
            m_Depth = depth;
            MAabb = aabb;
            ItemCount = itemCount;
        }

        public int Depth
        {
            get { return m_Depth; }
        }

        public bool IsLeaf()
        {
            Debug.Assert(!(Left == null && Right == null) || (Faces != null && Faces.Length > 0));
            return Left == null && Right == null;
        }
    }
}
