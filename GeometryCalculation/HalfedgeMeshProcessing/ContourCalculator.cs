using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryCalculation.DataStructures;
using GraphicsEngine.Additional;
using GraphicsEngine.Geometry;
using GraphicsEngine.HalfedgeMesh;
using GraphicsEngine.HalfedgeMesh.Simplification;
using GraphicsEngine.Math;
using Shared;

namespace GraphicsEngine.HalfedgeMeshProcessing
{
    internal class ContourCalculator : IPostProcess
    {
        private readonly List<HeFace> _inspectedFaces = new List<HeFace>();
        public void Execute(DeformableObject obj)
        {
            //TODO don't merge everything but only changes
            var mergeList = CreateMergeList(obj.HeMesh.FaceList.ToRawArray());
            obj.ContourGroupManager = new ContourGroupManager();
            ProcessCurves(mergeList, obj.ContourGroupManager);
            _inspectedFaces.Clear();
        }

        public void FaceAdded(HeFace face, HeMesh source)
        {
            _inspectedFaces.Add(face);
        }

        public void FaceDeleted(HeFace face, HeMesh source)
        {
        }

        public void VertexAdded(HeVertex v, HeMesh source)
        {
        }

        public void VertexDeleted(HeVertex v, HeMesh source)
        {
        }

        private List<MergeableFaces> CreateMergeList(HeFace[] faceArr)
        {
            List<MergeableFaces> mergeList = new List<MergeableFaces>();

            for (int i = 0; i < faceArr.Length; i++)
            {
                var inspectedFace = faceArr[i];

                if (inspectedFace == null)
                    continue;               

                MergeableFaces mergeableFaces = new MergeableFaces();
                mergeList.Add(mergeableFaces);
                mergeableFaces.Normal = inspectedFace.OuterComponent.Normal;
                mergeableFaces.FaceList.Add(inspectedFace);
                inspectedFace.DynamicProperties.AddProperty(PropertyConstants.Marked, true);
                Stack<HeHalfedge> faceStack = new Stack<HeHalfedge>();
                CheckFace(inspectedFace.OuterComponent.Twin, mergeableFaces, faceStack);
                CheckFace(inspectedFace.OuterComponent.Next.Twin, mergeableFaces, faceStack);
                CheckFace(inspectedFace.OuterComponent.Next.Next.Twin, mergeableFaces, faceStack);

                while (faceStack.Count != 0)
                {
                    var he = faceStack.Pop();
                    CheckFace(he, mergeableFaces, faceStack);
                }

                foreach (var face in mergeableFaces.FaceList)
                {
                    Debug.Assert(face.DynamicProperties.ExistsKey(PropertyConstants.Marked));
                    face.DynamicProperties.RemoveKey(PropertyConstants.Marked);
                    faceArr[face.Index] = null;
                }
            }
            return mergeList;
        }

        private void CheckFace(HeHalfedge inspectedHe, MergeableFaces mergeableFaces, Stack<HeHalfedge> faceStack)
        {
            var inspectedFace = inspectedHe.IncidentFace;
            if (inspectedFace.DynamicProperties.ExistsKey(PropertyConstants.Marked))
                return;

            if (IsMergeable(inspectedFace, mergeableFaces.Normal))
            {
                inspectedFace.DynamicProperties.AddProperty(PropertyConstants.Marked, true);
                mergeableFaces.FaceList.Add(inspectedFace);
                faceStack.Push(inspectedHe.Next.Twin);
                faceStack.Push(inspectedHe.Next.Next.Twin);
            }
            else
            {
                mergeableFaces.Contouredge.Add(inspectedHe.Twin);
            }
        }

        private bool IsMergeable(HeFace inspectedFace, Vector3m normal)
        {
            //TODO save normal of face in HeFace and use HeHalfedge normal for rendering (curved surfaces have different normals)
            var result = inspectedFace.OuterComponent.Normal.Cross(normal);
            return result.IsZero();
        }

        private void ProcessCurves(List<MergeableFaces> mergeList, ContourGroupManager manager)
        {
            foreach (var mergeableFaces in mergeList)
            {
                ContourGroup newGroup = new ContourGroup();
                manager.AddGroup(newGroup);
                newGroup.CalculateContours(mergeableFaces.Contouredge);
                newGroup.InsideFaces = mergeableFaces.FaceList;
                manager.MapGroup(newGroup);
                newGroup.Normal = mergeableFaces.Normal;
            }
        }
    }

    class MergeableFaces
    {
        internal List<HeFace> FaceList = new List<HeFace>();
        internal List<HeHalfedge> Contouredge = new List<HeHalfedge>();
        internal Vector3m Normal;
    }
}
