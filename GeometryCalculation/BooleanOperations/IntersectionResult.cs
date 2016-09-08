using System;
using System.Collections;
using System.Collections.Generic;
using GraphicsEngine.HalfedgeMesh;
using Shared.Additional;
using Shared.Geometry.HalfedgeMesh;

namespace GraphicsEngine.Geometry.CollisionCheck
{
    internal class IntersectionResult
    {
        private readonly List<FaceList> _list = new List<FaceList>();

        internal void AddNextFace(HeFace a, HeFace b)
        {
            if (!a.DynamicProperties.ExistsKey(PropertyConstants.FaceListIndex))
            {
                a.DynamicProperties.AddProperty(PropertyConstants.FaceListIndex, -1);
            }
            var faceList = TryGetFaceList(a);
            if (faceList != null)
                faceList.FacesB.Add(b);
            else
            {
                var fl = new FaceList(a);
                fl.FacesB.Add(b);
                _list.Add(fl);
                a.DynamicProperties.ChangeValue(PropertyConstants.FaceListIndex, _list.Count - 1);
            }
        }

        internal void AddFaces(HeFace a, HeFace[] faces)
        {
            foreach (var heFace in faces)
            {
                AddNextFace(a, heFace);
            }
        }

        internal List<FaceList> List {get { return _list; }} 

        internal FaceList TryGetFaceList(HeFace a)
        {
            if (!a.DynamicProperties.ExistsKey(PropertyConstants.FaceListIndex))
                return null;
            var index = (int) a.DynamicProperties.GetValue(PropertyConstants.FaceListIndex);
            if (index == -1)
                return null;
            return _list[index];
        }

        internal class FaceList
        {
            internal FaceList(HeFace a)
            {
                FaceA = a;
                BreakLoop = false;
            }
            internal HeFace FaceA { get; private set; }
            internal List<HeFace> FacesB = new List<HeFace>();

            internal bool BreakLoop { get; set; }

            
        }

        internal void Reset()
        {
            _list.Clear();
        }

        internal void RemoveFaceList(FaceList faceListA)
        {
            _list.Remove(faceListA);
        }
    }
}
