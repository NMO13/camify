using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Geometry.Bounding_Volume_Hierarchy;
using GraphicsEngine.Additional;
using GraphicsEngine.HalfedgeMesh;

namespace GraphicsEngine.Geometry.CollisionCheck
{
    public class BoundingVolumeHierarchy : IMeshObserver
    {
        internal uint MaxItemCount = 1;
        internal int Height = 0;
        public BoundingVolumeHierarchyNode Root { get; private set; }

        public BoundingVolumeHierarchy(HeFace[] faces, uint maxItemCount)
        {
            if (maxItemCount == 0)
                throw new ArgumentException("Item count and max depth have to be greater zero");

            MaxItemCount = maxItemCount;
            var aabr = CreateAabrFromFaces(faces);
            Root = new BoundingVolumeHierarchyNode(0, 0, aabr, faces.Length);
            AddMedianProperty(faces);
            SubdivideIteratively(Root, faces);
        }

        internal static bool IsLeaf(BoundingVolumeHierarchyNode a)
        {
            return a.Left == null && a.Right == null;
        }

        protected bool NeedsSubdivision(BoundingVolumeHierarchyNode node)
        {
            if (node.ItemCount <= MaxItemCount) return false;
            //if (node.Depth >= MaxDepth) return false;
            return true;
        }

        protected AxisAlignedBoundingBox CreateAabrFromFaces(HeFace[] faces)
        {
            var aabr = AxisAlignedBoundingBox.Init();
            foreach (var face in faces)
            {
                face.CreateBoundingBox();
                aabr.Grow(face.Aabb);
            }
            return aabr;
        }

        protected void SubdivideIteratively(BoundingVolumeHierarchyNode node, HeFace[] faces)
        {
            Stack<BoundingVolumeHierarchyNode> nodeStack = new Stack<BoundingVolumeHierarchyNode>();
            nodeStack.Push(node);
            Stack<HeFace[]> faceStack = new Stack<HeFace[]>();
            faceStack.Push(faces);

            do
            {
                node = nodeStack.Pop();
                faces = faceStack.Pop();

                if (node.Depth > Height)
                    Height = node.Depth;

                if (!NeedsSubdivision(node))
                {
                    foreach (var heFace in faces)
                    {
                        heFace.DynamicProperties.RemoveKey(PropertyConstants.Median);
                    }
                    node.Faces = faces;
                    continue;
                }

                List<HeFace> subsetA = new List<HeFace>(0);
                List<HeFace> subsetB = new List<HeFace>(0);
                MedianPartitionStrategy str = new MedianPartitionStrategy();
                str.ParitionObjects(node.AABB, subsetA, subsetB, faces);

                node.Left = new BoundingVolumeHierarchyNode(2*node.Id + 1, node.Depth + 1, CreateAabrFromFaces(subsetA.ToArray()), subsetA.Count);
                node.Right = new BoundingVolumeHierarchyNode(2*node.Id + 2, node.Depth + 1, CreateAabrFromFaces(subsetB.ToArray()), subsetB.Count);
                nodeStack.Push(node.Left);
                nodeStack.Push(node.Right);
                faceStack.Push(subsetA.ToArray());
                faceStack.Push(subsetB.ToArray());

            } while (nodeStack.Count > 0);

        }

        public void VertexAdded(HeVertex v, HeMesh source)
        {
            //throw new NotImplementedException();
        }

        public void FaceAdded(HeFace face, HeMesh source)
        {
            // throw new NotImplementedException();
        }

        public void VertexDeleted(HeVertex v, HeMesh source)
        {
            // throw new NotImplementedException();
        }

        public void FaceDeleted(HeFace face, HeMesh source)
        {
            // throw new NotImplementedException();
        }

        //internal void RayIntersection(BvhHitResult hitResult)
        //{
        //    BvhRayCheck.TraverseStackSorted(hitResult, Root, Height+1);
        //}

        private void AddMedianProperty(HeFace[] faces)
        {
            foreach (var heFace in faces)
            {
                heFace.DynamicProperties.AddProperty(PropertyConstants.Median, 0);
            }
        }
    }
}
