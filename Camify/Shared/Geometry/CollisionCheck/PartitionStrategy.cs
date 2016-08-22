using Geometry.Bounding_Volume_Hierarchy;
using GraphicsEngine.HalfedgeMesh;
using System.Collections.Generic;

namespace MeshStructuresLib.Bounding_Volume_Hierarchy
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
