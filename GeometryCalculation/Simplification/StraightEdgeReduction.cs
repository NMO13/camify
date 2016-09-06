using System.Collections.Generic;
using System.Diagnostics;
using GeometryCalculation.DataStructures;
using GeometryCalculation.Simplification;
using GraphicsEngine.Geometry.Triangulation;
using GraphicsEngine.HalfedgeMeshProcessing;
using Microsoft.SolverFoundation.Common;
using Shared.Additional;
using Shared.Geometry;

namespace GraphicsEngine.HalfedgeMesh.Simplification
{
    class StraightEdgeReduction : IPostProcess
    {
        public void VertexAdded(HeVertex v, HeMesh source)
        {
        }

        public void FaceAdded(HeFace face, HeMesh source)
        {
        }

        public void VertexDeleted(HeVertex v, HeMesh source)
        {
        }

        public void FaceDeleted(HeFace face, HeMesh source)
        {
        }

        public void Execute(DeformableObject obj)
        {
            int k = 0;
            foreach (var contourGroup in obj.ContourGroupManager.ContourGroups)
            {
                if (contourGroup.InsideFaces.Count == 1)
                {
                    Debug.Assert(contourGroup.Contours.Count == 1);
                    continue;
                }
                foreach (var contour in contourGroup.Contours)
                {
                    for (int i = 0; i < contour.HeList.Count; i++)
                    {
                        var j = (i + 1) % contour.HeList.Count;

                        var ce0 = contour.HeList[i];
                        var ce1 = contour.HeList[j];

                        Debug.Assert(ce0.Index >= 0 && ce1.Index >= 0);

                        var vec0 = ce0.Twin.Origin.Vector3m - ce0.Origin.Vector3m;
                        var vec1 = ce1.Twin.Origin.Vector3m - ce1.Origin.Vector3m;
                        if (vec0.SameDirection(vec1))
                        {
                            List<HeVertex> leftContour = new List<HeVertex>();
                            List<HeVertex> rightContour = new List<HeVertex>();

                            List<HeFace> leftFaces = new List<HeFace>();
                            List<HeFace> rightFaces = new List<HeFace>();
                            var res0 = AreOnSamePlane(ce0, ce1, leftContour, leftFaces);
                            var res1 = AreOnSamePlane(ce1.Twin, ce0.Twin, rightContour, rightFaces);
                            if (res0 && res1) // the vertex is mergable, so merge now
                            {
                                k++;
                                int i0 = ce0.Origin.Index;
                                int i1 = ce1.Twin.Origin.Index;

                                // create a triangulation for left side and right side
                                HeVertex removableVertex = ce1.Origin;

                                var indexListLeft = Triangulate(leftContour, ce0.Normal);
                                var indexListRight = Triangulate(rightContour, ce0.Twin.Normal);


                                // remove original faces
                                int ce0Index = ce0.Index;
                                int ce1Index = ce1.Index;
                                int ce0TIndex = ce0.Twin.Index;
                                int ce1TIndex = ce1.Twin.Index;
                                var newEdge = UpdateFaces(obj.HeMesh, leftFaces, obj.ContourGroupManager, indexListLeft, i0, i1);
                                UpdateFaces(obj.HeMesh, rightFaces, obj.ContourGroupManager, indexListRight, i0, i1);
                                // Restore indices
                                ce0.Index = ce0Index;
                                ce1.Index = ce1Index;
                                ce0.Twin.Index = ce0TIndex;
                                ce1.Twin.Index = ce1TIndex;

                                UpdateContour(ce0, ce1, newEdge, obj.ContourGroupManager);
                                UpdateContour(ce1.Twin, ce0.Twin, newEdge.Twin, obj.ContourGroupManager);

                                Debug.Assert(removableVertex.IncidentEdges.Count == 0);
                                obj.HeMesh.VertexList.Remove(removableVertex.Index);
                                i--; // one edge less now
                            }

                        }
                    }
                }
            }
        }

        private HeHalfedge UpdateFaces(HeMesh heMesh, List<HeFace> faces, ContourGroupManager contourGroupManager, List<int> indexList, int i0, int i1)
        {
            var groupIndex = RemoveOriginalFaces(heMesh, faces, contourGroupManager);
            return AddNewFaces(heMesh, indexList, i0, i1, groupIndex, contourGroupManager);
        }

        private void UpdateContour(HeHalfedge ce0, HeHalfedge ce1, HeHalfedge newEdge, ContourGroupManager contourGroupManager)
        {
            var group0 = contourGroupManager.GetContourGroupFromHalfedge(ce0);
            var group1 = contourGroupManager.GetContourGroupFromHalfedge(ce1);

            Debug.Assert(group0 == group1);
            bool merged = false;
            foreach (var c in group0.Contours)
            {
                merged = c.Merge(ce0, ce1, newEdge);
                if (merged)
                {
                    break;
                }
            }
            contourGroupManager.RemoveHalfedge(ce0);
            contourGroupManager.RemoveHalfedge(ce1);
            contourGroupManager.AddHalfedge(newEdge, group0);
            Debug.Assert(merged);
        }

        private HeHalfedge AddNewFaces(HeMesh heMesh, List<int> indexList, int i0, int i1, int contourGroupIndex, ContourGroupManager manager)
        {
            Debug.Assert(indexList.Count % 3 == 0);
            HeHalfedge mergedEdge = null;
            for (int i = 0; i < indexList.Count; i += 3)
            {
                HeFace face = heMesh.AddFace(indexList[i], indexList[i + 1], indexList[i + 2], null);
                manager.AddFace(contourGroupIndex, face);
                if (face.V0.Index == i0 && face.V1.Index == i1)
                    mergedEdge = face.OuterComponent;
                else if (face.V1.Index == i0 && face.V2.Index == i1)
                    mergedEdge = face.OuterComponent.Next;
                else if (face.V2.Index == i0 && face.V0.Index == i1)
                    mergedEdge = face.OuterComponent.Next.Next;
            }
            return mergedEdge;
        }

        private int RemoveOriginalFaces(HeMesh heMesh, List<HeFace> faces, ContourGroupManager manager)
        {
            int groupIndex = -1;
            foreach (var face in faces)
            {
                groupIndex = manager.RemoveFace(face);
                heMesh.RemoveFace(face, false);
            }
            return groupIndex;
        }

        private List<int> Triangulate(List<HeVertex> contourVertices, Vector3m normal)
        {
            var points = CreateInputGeometry(contourVertices, normal);
            EarClipping earClipping = new EarClipping();
            earClipping.SetPoints(points, normal:normal);
            earClipping.Triangulate();

            var result = earClipping.Result;
            Debug.Assert(result.Count % 3 == 0);
            List<int> newFaceIndices = new List<int>();
            result.ForEach(x => newFaceIndices.Add((int)x.DynamicProperties.GetValue(PropertyConstants.HeVertexIndex)));
            return newFaceIndices;
        }

        private List<Vector3m> CreateInputGeometry(List<HeVertex> contourVertices, Vector3m up)
        {
            var matrix = Matrix4<Rational>.CreateInverseRotationMatrix(up);
            List<Vector3m> points = new List<Vector3m>();
            foreach (var vertex in contourVertices)
            {
                var projectedVertex = Matrix4<Rational>.Multiply(matrix, vertex.Vector3m);
                var point = new Vector3m(vertex.Vector3m); //new Vector3m(projectedVertex.X, projectedVertex.Z);
                point.DynamicProperties.AddProperty(PropertyConstants.HeVertexIndex, vertex.Index);
                points.Add(point);
            }

            return points;
        }

        private bool AreOnSamePlane(HeHalfedge he0, HeHalfedge he1, List<HeVertex> contourVertices, List<HeFace> faces)
        {
            Debug.Assert(he0.Twin.Origin == he1.Origin);
            contourVertices.Add(he1.Next.Origin);
            var cur = he1.Prev;
            var oldCur = cur;
            var normal = he0.Normal;
            while (oldCur != he0)
            {
                if (!cur.Normal.SameDirection(normal))
                    return false;
                contourVertices.Add(cur.Origin);
                faces.Add(cur.IncidentFace);
                oldCur = cur;
                cur = cur.Twin.Prev;
            }
            return true;
        }

       
    }
}
