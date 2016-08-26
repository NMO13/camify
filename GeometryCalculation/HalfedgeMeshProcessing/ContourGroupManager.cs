using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsEngine.HalfedgeMesh;

namespace GraphicsEngine.HalfedgeMeshProcessing
{
    class ContourGroupManager
    {
        internal List<ContourGroup> ContourGroups = new List<ContourGroup>();
        private Dictionary<int, int> HalfedgeMapping = new Dictionary<int, int>();
        private Dictionary<int, int> FaceMapping = new Dictionary<int, int>();

        internal void AddGroup(ContourGroup group)
        {
            ContourGroups.Add(group);
            group.Index = ContourGroups.Count - 1;
        }

        internal void MapGroup(ContourGroup group)
        {
            foreach (var insideFace in group.InsideFaces)
            {
                FaceMapping.Add(insideFace.Index, group.Index);
            }
            foreach (var contour in group.Contours)
            {
                foreach (var heHalfedge in contour.HeList)
                {
                    HalfedgeMapping.Add(heHalfedge.Index, group.Index);
                }
            }
        }

        internal int RemoveFace(HeFace face)
        {
            var groupIndex = FaceMapping[face.Index];
            ContourGroups[groupIndex].InsideFaces.Remove(face);
            FaceMapping.Remove(face.Index);
            return groupIndex;
        }

        internal void AddFace(int contourGroupIndex, HeFace face)
        {
            FaceMapping.Add(face.Index, contourGroupIndex);
            ContourGroups[contourGroupIndex].InsideFaces.Add(face);
        }

        internal void RemoveHalfedge(HeHalfedge he)
        {
            HalfedgeMapping.Remove(he.Index);
        }

        internal void AddHalfedge(HeHalfedge he, ContourGroup cg)
        {
            HalfedgeMapping.Add(he.Index, cg.Index);
        }

        internal ContourGroup GetContourGroupFromHalfedge(HeHalfedge he)
        {
            return ContourGroups[HalfedgeMapping[he.Index]];
        }
    }
}
