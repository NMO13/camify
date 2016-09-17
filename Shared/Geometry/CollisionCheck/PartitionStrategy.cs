using System.Collections.Generic;
using Shared.Geometry.HalfedgeMesh;

namespace Shared.Geometry.CollisionCheck
{
    abstract class PartitionStrategy
    {
        /*
         * Partition one set of faces into two sets
         * Make sure that a and b have at least 1 element!
         */
        abstract internal void ParitionObjects(BoundingVolumeHierarchyNode node, List<HeFace> a, List<HeFace> b);
    }
}
