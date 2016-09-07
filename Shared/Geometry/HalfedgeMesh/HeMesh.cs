using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GraphicsEngine.Math;
using Microsoft.SolverFoundation.Common;
using Shared.Additional;
using Shared.Geometry;
using Shared.Helper;

namespace GraphicsEngine.HalfedgeMesh
{
    public class HeMesh
    {
        private readonly ManagedList<HeHalfedge> _halfedgeList;
        private readonly ManagedList<HeFace> _faceList;
        private readonly ManagedList<HeVertex> _vertexList;

        private readonly List<IMeshObserver> _observerList = new List<IMeshObserver>();

        public HeMesh()
        {
            _vertexList = new ManagedList<HeVertex>(500000);
            _halfedgeList = new ManagedList<HeHalfedge>(10000000);
            _faceList = new ManagedList<HeFace>(500000);
        }

        public HeMesh(HeMesh other, Vector3m translate) : this()
        {            
            SetMesh(other, translate);
        }

        public void AddObserver(IMeshObserver obs)
        {
            _observerList.Add(obs);
        }

        public ManagedList<HeFace> FaceList
        {
            get { return _faceList; }
        }

        public ManagedList<HeVertex> VertexList
        {
            get { return _vertexList; }
        }

        public ManagedList<HeHalfedge> HalfedgeList
        {
            get { return _halfedgeList; }
        }

        public HeVertex AddVertex(HeVertex vertex)
        {
            if (vertex == null) return null;
            _vertexList.Add(vertex);
            NotifyNewVertex(vertex);
            return vertex;
        }

        public HeFace AddFace(int i0, int i1, int i2, Vector3d[] renderNormals)
        {
            if (renderNormals != null)
            {
                if (renderNormals.Length != 3)
                    throw new Exception("Exactly three normals are expected");
            }
            Vector3m[] normals = HeFace.CreateNormals(_vertexList[i0], _vertexList[i1], _vertexList[i2]);
            HeFace face = CreateHalfEdges(normals, renderNormals, i0, i1, i2);
            ReconnectEqualedgeFaces(face);
            NotifyNewFace(face);
            return face;
        }

        internal void ReconnectEqualedgeFaces(HeFace face)
        {
            foreach (var heHalfedge in face.GetFaceCirculator())
            {
                // get all equal edges
                var equalEdges = heHalfedge.Origin.IncidentEdges.EqualEdges(heHalfedge);
                // get all equal edge's twins
                var equalEdgesTwins =
                    heHalfedge.Twin.Origin.IncidentEdges.EqualEdges(heHalfedge.Twin);

                if (equalEdges == null)
                {
                    Debug.Assert(equalEdgesTwins == null);
                    continue;
                }

                Debug.Assert(equalEdges.Count == equalEdgesTwins.Count);

                bool areAllFacesPresent = true;
                for (int i = 0; i < equalEdges.Count; i++)
                {
                    if (equalEdges[i].IncidentFace == null)
                    {
                        areAllFacesPresent = false;
                        break;
                    }
                    if (equalEdgesTwins[i].IncidentFace == null)
                    {
                        areAllFacesPresent = false;
                        break;
                    }
                }

                if (!areAllFacesPresent)
                    continue;
                // take each equal edge from list and compare it with all twins, find the one with smallest value
                foreach (var equalEdge in equalEdges)
                {
                    var maxAngle = equalEdge.Normal.Dot(equalEdge.Normal);
                    var maxAngled = maxAngle.ToDouble();
                    maxAngle *= 4;
                    maxAngled = maxAngle.ToDouble();
                    HeHalfedge candidate = null;
                    foreach (var equalEdgesTwin in equalEdgesTwins)
                    {
                        var angle = Misc.CalcAngle(equalEdge.IncidentFace, equalEdgesTwin.IncidentFace, equalEdgesTwin.Next.Next.Origin.Vector3m);
                        Debug.Assert(angle > 0 && angle < equalEdge.Normal.Dot(equalEdge.Normal) * 4);
                        var angled = angle.ToDouble();
                        if (angle < maxAngle)
                        {
                            candidate = equalEdgesTwin;
                            maxAngle = angle;
                            maxAngled = angled;
                        }
                    }
                    // connect these two
                    equalEdge.Twin = candidate;
                    candidate.Twin = equalEdge;
                }
                
            }
        }

        protected HeFace CreateHalfEdges(Vector3m[] normals, Vector3d[] renderNormals, params int[] vertices)
        {
            if (vertices.Length < 3)
                throw new ArgumentException("Too few vertices specified");
            HeHalfedge prev = null, first = null;
            var face = new HeFace();
            for (int i = 0; i < vertices.Length; i++)
            {
                var v0 = _vertexList[vertices[i]];
                var v1 = _vertexList[vertices[(i + 1) % vertices.Length]];
                if (v0.Equals(v1))
                    throw new ArgumentException("Vertices have same coordinates: " + v0);
                Vector3d renderNormal = null;
                if (renderNormals != null)
                    renderNormal = renderNormals[i];
                HeHalfedge current = CreateHalfedgePair(v0, v1, face, normals[i], renderNormal);

                first = (i == 0) ? current : first; // remember first
                if (prev != null)
                {
                    prev.Next = current;
                }
                current.Prev = prev;
                prev = current;
            }

            Debug.Assert(first != null, "first != null");
            first.Prev = prev;
            prev.Next = first;
            face.OuterComponent = first;
            _faceList.Add(face);

            return face;
        }

        private HeHalfedge CreateHalfedgePair(HeVertex v0, HeVertex v1, HeFace face, Vector3m normal, Vector3d renderNormal)
        {
            Debug.Assert(normal != null);
            var h0 = new HeHalfedge(v0);
            var h1 = new HeHalfedge(v1);
            h0.Twin = h1;
            h1.Twin = h0;
            HeHalfedge res;
            if (!GetFacelessHalfedge(v0, v1, out res))
            {
                _halfedgeList.Add(h0);
                h0.Origin.IncidentEdges.Add(h0);
                _halfedgeList.Add(h1);
                h1.Origin.IncidentEdges.Add(h1);
                h0.IncidentFace = face;
                h0.Normal = normal;
                h0.RenderNormal = renderNormal;
                return h0;
            }
           
            if(res.Twin == null)
                throw new Exception("Invalid halfedge (a twin already exists)");
            if(res.IncidentFace != null)
                throw new Exception("Invalid halfedge (an incident face already exists)");
            Debug.Assert(res.Next == null && res.Prev == null);
            res.IncidentFace = face;
            res.Normal = normal;
            res.RenderNormal = renderNormal;
            return res;
        }

        // we are looking for an existing halfedge which does not have a face attached to it
        // note that it is possible that identical halfedges could exist
        public bool GetFacelessHalfedge(HeVertex v0, HeVertex v1, out HeHalfedge res)
        {
            foreach (var incidentEdge in VertexList[v0.Index].IncidentEdges)
            {
                if (incidentEdge.Twin.Origin.Equals(v1))
                {
                    if (incidentEdge.IncidentFace == null)
                    {
                        res = incidentEdge;
                        return true;
                    }
                }
            }
            res = null;
            return false;
        }

        public void RemoveFace(HeFace f0, bool removeOrphanVertices)
        {
            Debug.Assert(f0.Index > -1);
            NotifyFaceDeleted(f0);
            _faceList.Remove(f0.Index);
            var fc = f0.GetFaceCirculator();
            var heHalfedges = fc as HeHalfedge[] ?? fc.ToArray();
            for (int i = 0; i < heHalfedges.Length; i++)
            {
                var halfedge = heHalfedges[i];
                halfedge.IncidentFace = null;
                if (halfedge.Twin.IncidentFace == null) // if halfedges are not needed any more, then delete them
                {
                    // remove halfedges from incident edge list
                    halfedge.Origin.IncidentEdges.Remove(halfedge);
                    _halfedgeList.Remove(halfedge.Index);
                    if (halfedge.Origin.IncidentEdges.Count == 0 && removeOrphanVertices)
                    {
                        NotifyVertexDeleted(halfedge.Origin);
                        _vertexList.Remove(halfedge.Origin.Index);
                        halfedge.Origin.Index = -1;
                    }

                    halfedge.Twin.Origin.IncidentEdges.Remove(halfedge.Twin);
                    _halfedgeList.Remove(halfedge.Twin.Index);
                    if (halfedge.Twin.Origin.IncidentEdges.Count == 0 && removeOrphanVertices)
                    {
                        NotifyVertexDeleted(halfedge.Twin.Origin);
                        _vertexList.Remove(halfedge.Twin.Origin.Index);
                        halfedge.Twin.Origin.Index = -1;
                    }
                }
                halfedge.Next = null;
                halfedge.Prev = null;
            }
        }

        private void NotifyNewVertex(HeVertex v)
        {
            _observerList.ForEach(obs => obs.VertexAdded(v, this));
        }

        private void NotifyNewFace(HeFace face)
        {
            _observerList.ForEach(obs => obs.FaceAdded(face, this));
        }

        private void NotifyVertexDeleted(HeVertex v)
        {
            _observerList.ForEach(obs => obs.VertexDeleted(v, this));
        }

        private void NotifyFaceDeleted(HeFace face)
        {
            _observerList.ForEach(obs => obs.FaceDeleted(face, this));
        }

        public void Translate(Vector3m amount)
        {
            foreach (var vertex in _vertexList)
            {
                vertex.X += amount.X;
                vertex.Y += amount.Y;
                vertex.Z += amount.Z;
            }

            // not needed for translation but we will improve this method in the future to support rotation and then, we will
            // need the normals to be recalculated
            foreach (var face in _faceList)
            {
                var normals = HeFace.CreateNormals(face.OuterComponent.Origin, face.OuterComponent.Next.Origin,
                    face.OuterComponent.Next.Next.Origin);
                Debug.Assert(normals.Length == 3);
                face.OuterComponent.Normal = normals[0];
                face.OuterComponent.Next.Normal = normals[1];
                face.OuterComponent.Next.Next.Normal = normals[2];
            }
        }

        public HeFace[] SplitFaceInTwo(HeHalfedge heA, HeVertex heVertex)
        {
            var faces = new HeFace[2];
            var face = heA.IncidentFace;

            var h0 = heA;
            var h1 = heA.Next;
            var h2 = heA.Next.Next;

            Vector3d normalNew = face.H0.Normal.Vector3d.Unit();
            Vector3d n0 = face.H0.RenderNormal;
            Vector3d n1 = face.H1.RenderNormal;
            Vector3d n2 = face.H2.RenderNormal;

            RemoveFace(face, false);

            Vector3d[] renderNormals = {n0, normalNew, n2};
            var face0 = AddFace(h0.Origin.Index, heVertex.Index, h2.Origin.Index, renderNormals);

            Vector3d[] renderNormals2 = {normalNew, n1, n2};
            var face1 = AddFace(heVertex.Index, h1.Origin.Index, h2.Origin.Index, renderNormals2);
            faces[0] = face0;
            faces[1] = face1;

            if (face.DynamicProperties.ExistsKey(PropertyConstants.Marked))
            {
                face0.DynamicProperties.AddProperty(PropertyConstants.Marked, face.DynamicProperties.GetValue(PropertyConstants.Marked));
                face1.DynamicProperties.AddProperty(PropertyConstants.Marked, face.DynamicProperties.GetValue(PropertyConstants.Marked));
            }

            face0.OuterComponent.SetSplitline(h0.IsSplitLine);
            face1.OuterComponent.SetSplitline(h0.IsSplitLine);
            face1.OuterComponent.Next.SetSplitline(h1.IsSplitLine);
            face0.OuterComponent.Next.Next.SetSplitline(h2.IsSplitLine);
            return faces;
        }

        public void ResetMesh(HeMesh other, Vector3m translate)
        {
            VertexList.Clear();
            HalfedgeList.Clear();
            FaceList.Clear();
            SetMesh(other, translate);
        }

        internal void SetMesh(HeMesh other, Vector3m translate)
        {
            for (int i = 0; i < other.VertexList.Count; i++)
            {
                var v = other.VertexList[i];
                if (v != null)
                {
                    HeVertex vNew = new HeVertex(v.X + translate.X, v.Y + translate.Y, v.Z + translate.Z);
                    vNew.Index = v.Index;
                    _vertexList.Add(vNew);
                }
                else
                {
                    _vertexList.Add(null);
                }
            }
            for (int i = 0; i < other.HalfedgeList.Count; i++)
            {
                var halfedge = other.HalfedgeList[i];
                if (halfedge != null)
                {
                    var halfedgeNew = new HeHalfedge(_vertexList[halfedge.Origin.Index]);
                    halfedgeNew.Normal = halfedge.Normal.Clone() as Vector3m;
                    halfedgeNew.Index = halfedge.Index;
                    if(halfedge.RenderNormal != null)
                        halfedgeNew.RenderNormal = halfedge.RenderNormal.Clone() as Vector3d;
                    _halfedgeList.Add(halfedgeNew);
                }
                else
                {
                    _halfedgeList.Add(null);
                }
            }
            for (int i = 0; i < other.FaceList.Count; i++)
            {
                var otherFace = other.FaceList[i];

                if (otherFace != null)
                {
                    var faceNew = new HeFace();
                    _faceList.Add(faceNew);

                    var h0 = otherFace.OuterComponent;
                    _halfedgeList[h0.Index].Twin = _halfedgeList[h0.Twin.Index];
                    _halfedgeList[h0.Index].Twin.Twin = _halfedgeList[h0.Index];
                    var h1 = otherFace.OuterComponent.Next;
                    _halfedgeList[h1.Index].Twin = _halfedgeList[h1.Twin.Index];
                    _halfedgeList[h1.Index].Twin.Twin = _halfedgeList[h1.Index];
                    var h2 = otherFace.OuterComponent.Next.Next;
                    _halfedgeList[h2.Index].Twin = _halfedgeList[h2.Twin.Index];
                    _halfedgeList[h2.Index].Twin.Twin = _halfedgeList[h2.Index];

                    _halfedgeList[h0.Index].Next = _halfedgeList[h1.Index];
                    _halfedgeList[h1.Index].Next = _halfedgeList[h2.Index];
                    _halfedgeList[h2.Index].Next = _halfedgeList[h0.Index];
                    _halfedgeList[h0.Index].Prev = _halfedgeList[h2.Index];
                    _halfedgeList[h1.Index].Prev = _halfedgeList[h0.Index];
                    _halfedgeList[h2.Index].Prev = _halfedgeList[h1.Index];
                    faceNew.OuterComponent = _halfedgeList[h0.Index];
                    _halfedgeList[h0.Index].IncidentFace = faceNew;
                    _halfedgeList[h1.Index].IncidentFace = faceNew;
                    _halfedgeList[h2.Index].IncidentFace = faceNew;
                    faceNew.Index = otherFace.Index;
                }
                else
                {
                    _faceList.Add(null);
                }
            }

            for (int i = 0; i < other.VertexList.Count; i++)
            {
                var v = other.VertexList[i];
                if (v != null)
                {
                    foreach (var incidentEdge in v.IncidentEdges)
                    {
                        _vertexList[v.Index].IncidentEdges.Add(_halfedgeList[incidentEdge.Index]);
                    }
                }
            }

            Debug.Assert(_vertexList.Count == other.VertexList.Count);
            Debug.Assert(_halfedgeList.Count == other.HalfedgeList.Count);
            Debug.Assert(_faceList.Count == other.FaceList.Count);
        }

        // two identical halfedges are conntected at the same vertices
        // so vertices are never duplicated
        // we check here if vertex already exists by checking the incident edges
        // of their test vertices
        public HeVertex AddVertexUnique(HeVertex vertex, params HeVertex[] testVertices)
        {
            foreach (var testVertex in testVertices)
            {
                foreach (var incidentEdge in testVertex.IncidentEdges)
                {
                    if (incidentEdge.Twin.Origin.Equals(vertex))
                        return incidentEdge.Twin.Origin;
                }
            }
            return AddVertex(vertex);
        }

        public void Collapse(int heHalfedgeIndex)
        {
            HeHalfedge e = HalfedgeList[heHalfedgeIndex];
            HeVertex u = e.Origin;

            // delete triangles on edge uv
            // remove e.IncidentFace
            RemoveFace(e.IncidentFace, false);
            // remove twin face
            RemoveFace(e.Twin.IncidentFace, false);

            // update all of the faces which had vertex u with vertex v
            var v = e.Twin.Origin;
            Vector3d vn = e.Twin.RenderNormal;

            while(u.IncidentEdges.Count > 0)
            {
                var incidentEdge = u.IncidentEdges[0];
                if (incidentEdge.IncidentFace == null)
                {
                    incidentEdge = incidentEdge.Twin.Next;
                }
                Debug.Assert(incidentEdge.Origin == u);
                var w0 = incidentEdge.Next.Origin;
                var w1 = incidentEdge.Next.Next.Origin;

                Vector3d w0n = incidentEdge.Next.RenderNormal;
                Vector3d w1n = incidentEdge.Next.Next.RenderNormal;

                RemoveFace(incidentEdge.IncidentFace, false);
                Vector3d[] normals = {vn, w0n, w1n};
                AddFace(v.Index, w0.Index, w1.Index, normals);
            }
            Debug.Assert(u.IncidentEdges.Count == 0);
            Debug.Assert(u.Index == -1);
        }

        public void RemoveOrphanVertices()
        {
            List<int> orphanIndices = new List<int>();
            foreach (var heVertex in VertexList)
            {
                if(heVertex.IncidentEdges.Count == 0)
                    orphanIndices.Add(heVertex.Index);
            }

            orphanIndices.ForEach(x => VertexList.Remove(x));
        }
    }
}
