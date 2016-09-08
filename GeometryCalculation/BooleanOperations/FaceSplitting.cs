using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GraphicsEngine.Geometry.CollisionCheck;
using GraphicsEngine.HalfedgeMesh;
using Microsoft.SolverFoundation.Common;
using Shared.Additional;
using Shared.Geometry;
using Shared.Geometry.HalfedgeMesh;
using Shared.Helper;

namespace GraphicsEngine.Geometry.Boolean_Ops
{
    internal class FaceSplitting
    {
        private HeMesh _mesh;
        private IntersectionResult _intersectionResult;
        private SplitResult _splitResult;
        internal void Split(IntersectionResult intersectionResult, HeMesh mesh, SplitResult splitResult)
        {
            _mesh = mesh;
            _intersectionResult = intersectionResult;
            _splitResult = splitResult;
            for (int aIndex = 0; aIndex < intersectionResult.List.Count; aIndex++)
            {
                var faceList = intersectionResult.List[aIndex];
                HeFace faceA = faceList.FaceA;
                for (int bIndex = 0; bIndex < faceList.FacesB.Count; bIndex++)
                {
                    if (faceList.BreakLoop)
                    {
                        break;
                    }
                    var faceB = faceList.FacesB[bIndex];
                    var facesB = faceList.FacesB;
                    var remainingFacesB = facesB.Skip(bIndex + 1).ToArray();

                    HeVertex faceAv0 = faceA.OuterComponent.Origin;
                    HeVertex faceAv1 = faceA.OuterComponent.Next.Origin;
                    HeVertex faceAv2 = faceA.OuterComponent.Next.Next.Origin;

                    HeVertex faceBv0 = faceB.OuterComponent.Origin;
                    HeVertex faceBv1 = faceB.OuterComponent.Next.Origin;
                    HeVertex faceBv2 = faceB.OuterComponent.Next.Next.Origin;

                    //distance from the faceA vertices to the faceB plane
                    var signFaceAVert0 = faceB.DistanceSign(faceAv0.Vector3m);
                    var signFaceAVert1 = faceB.DistanceSign(faceAv1.Vector3m);
                    var signFaceAVert2 = faceB.DistanceSign(faceAv2.Vector3m);

                    //if all the signs are zero, the planes are coplanar
                    //if all the signs are positive or negative, the planes do not intersect
                    //if the signs are not equal...
                    if (!(signFaceAVert0 == signFaceAVert1 && signFaceAVert1 == signFaceAVert2))
                    {
                        //distances signs from the face2 vertices to the face1 plane
                        var signFaceBVert0 = faceA.DistanceSign(faceBv0.Vector3m);
                        var signFaceBVert1 = faceA.DistanceSign(faceBv1.Vector3m);
                        var signFaceBVert2 = faceA.DistanceSign(faceBv2.Vector3m);

                        //if the signs are not equal...
                        if (!(signFaceBVert0 == signFaceBVert1 && signFaceBVert1 == signFaceBVert2))
                        {
                            var line = new IntersectionLine(faceA, faceB);

                            //intersection of the face1 and the plane of face2
                            var segment0 = new Segment(line, faceA, signFaceAVert0, signFaceAVert1, signFaceAVert2);

                            //intersection of the face2 and the plane of face1
                            var segment1 = new Segment(line, faceB, signFaceBVert0, signFaceBVert1, signFaceBVert2);

                            //if the two segments intersect...
                            if (segment0.Intersect(segment1))
                            {
                                bool split = false;
                                //PART II - SUBDIVIDING NON-COPLANAR POLYGONS
                                Vector3m spStart = null, spEnd = null;

                                var faces = SplitFace(faceA, segment0, segment1, ref split, ref spStart, ref spEnd);
                                if (split)
                                {
                                    Debug.Assert(spStart != null && spEnd != null);
                                    Debug.Assert(faces != null);
                                    UpdateIntersectionResultList(faces, remainingFacesB);
                                    CheckNeighboursOfA(faces);
                                }
                                if (faces != null)
                                {
                                    if(!split)
                                        Debug.Assert(faces.Count == 1);
                                    var splitedge = GetSplitHalfedge(faces, spStart, spEnd);
                                    SetSplitedge(splitedge);
                                }
                            }
                        }
                    }
                }
            }
                RemoveFaceListIndex(mesh);
        }

        private void RemoveFaceListIndex(HeMesh mesh)
        {
            foreach (var heFace in mesh.FaceList)
            {
                heFace.DynamicProperties.RemoveKey(PropertyConstants.FaceListIndex);
            }
        }

        private void CheckNeighboursOfA(List<HeFace> newFaces)
        {
            //Check twins of newly created triangles
            var orphanHalfedges = new List<HeHalfedge>();
            foreach (var newFace in newFaces)
            {
                HeHalfedge he0 = newFace.OuterComponent;
                HeHalfedge he1 = newFace.OuterComponent.Next;
                HeHalfedge he2 = newFace.OuterComponent.Next.Next;

                if (he0.Twin.IncidentFace == null)
                {
                    orphanHalfedges.Add(he0);
                }

                if (he1.Twin.IncidentFace == null)
                {
                    orphanHalfedges.Add(he1);
                }

                if (he2.Twin.IncidentFace == null)
                {
                    throw new Exception("Jay! Branch executed!");
                    orphanHalfedges.Add(he2);
                }
            }

            if (orphanHalfedges.Count == 0)
                return;
            if (orphanHalfedges.Count != 2 && orphanHalfedges.Count != 3 && orphanHalfedges.Count != 4)
                throw new Exception("Number of orphan halfedges not correct: " + orphanHalfedges.Count);

            LinkedList<HeHalfedge> orderedOrphans = new LinkedList<HeHalfedge>();
            orderedOrphans.AddLast(orphanHalfedges[0]);
            orphanHalfedges.RemoveAt(0);
            for (int i = 0; i < orphanHalfedges.Count; i++)
            {
                bool inserted = false;
                var selectedHalfedge = orphanHalfedges[i];
                for (int j = 0; j < orderedOrphans.Count; j++)
                {
                    var orderedHalfedge = orderedOrphans.ElementAt(j);

                    if (orderedHalfedge.Origin.Equals(selectedHalfedge.Twin.Origin))
                    {
                        throw new Exception("Jay! Branch executed!");
                        LinkedListNode<HeHalfedge> curHalfEdgeNode = orderedOrphans.Find(orderedHalfedge);
                        if (curHalfEdgeNode == null)
                            throw new NullReferenceException("Halfedge node not found in orderedOrphans list!");
                        orderedOrphans.AddBefore(curHalfEdgeNode, selectedHalfedge);
                        inserted = true;
                    }
                    else if (orderedHalfedge.Twin.Origin.Equals(selectedHalfedge.Origin))
                    {
                        LinkedListNode<HeHalfedge> curHalfEdgeNode = orderedOrphans.Find(orderedHalfedge);
                        if (curHalfEdgeNode == null)
                            throw new NullReferenceException("Halfedge node not found in orderedOrphans list!");
                        orderedOrphans.AddAfter(curHalfEdgeNode, selectedHalfedge);
                        inserted = true;
                    }
                    if (inserted) break;
                }

                if (!inserted)
                {
                    orphanHalfedges.Add(selectedHalfedge);
                    orphanHalfedges.RemoveAt(i);
                    i = -1;
                }
            }
            BreakNeighboursInTwoOrThree(orderedOrphans.ToList());
        }

        private void UpdateNeighbourFaceList(HeFace heFace, HeVertex heVertex, HeVertex newVertex0, HeVertex newVertex1)
        {
            var faceListA = _intersectionResult.TryGetFaceList(heFace);
            HeVertex v0 = heFace.V0;
            HeVertex v1 = heFace.V1;
            HeVertex v2 = heFace.V2;

            Vector3d normalNew = heFace.H0.Normal.Vector3d.Unit();
            Vector3d n0 = heFace.H0.RenderNormal;
            Vector3d n1 = heFace.H1.RenderNormal;
            Vector3d n2 = heFace.H2.RenderNormal;

            RemoveFaceFromMesh(heFace);


            List<HeFace> newFaces;
            if(newVertex1 == null)
                newFaces = CreateFaces(heVertex, newVertex0, v0, v1, v2, normalNew, n0, n1, n2);
            else
                newFaces = CreateFaces(heVertex, newVertex0, newVertex1, v0, v1, v2, normalNew, n0, n1, n2);
            if (faceListA != null)
            {
                var remainingFacesB = faceListA.FacesB.ToArray();
                UpdateIntersectionResultList(newFaces, remainingFacesB);
            }
        }

        private void BreakNeighboursInTwoOrThree(List<HeHalfedge> orphanHalfEdges)
        {
            if (orphanHalfEdges.Count == 4) //Two halfedges' twins have no incident faces
            {
                HeVertex heVertex0 = orphanHalfEdges[0].Origin;
                HeVertex newVertex0 = orphanHalfEdges[0].Next.Origin;
                HeVertex heVertex1 = orphanHalfEdges[1].Next.Origin;
                HeVertex heVertex2 = orphanHalfEdges[2].Origin;
                HeVertex newVertex1 = orphanHalfEdges[2].Next.Origin;
                HeVertex heVertex3 = orphanHalfEdges[3].Next.Origin;

                HeHalfedge halfEdge0, halfEdge1;

                _mesh.GetFacelessHalfedge(heVertex0, heVertex1, out halfEdge0);
                _mesh.GetFacelessHalfedge(heVertex2, heVertex3, out halfEdge1);

                if (halfEdge0 == null || halfEdge0.IncidentFace != null || halfEdge0.Twin == null)
                    throw new NullReferenceException("Halfedge is not valid");
                if (halfEdge1 == null || halfEdge1.IncidentFace != null || halfEdge1.Twin == null)
                    throw new NullReferenceException("Halfedge is not valid");

                halfEdge0 = halfEdge0.Twin;
                halfEdge1 = halfEdge1.Twin;

                Debug.Assert(!halfEdge0.IsSplitLine);
                Debug.Assert(!halfEdge1.IsSplitLine);
                HeFace heFace0 = halfEdge0.IncidentFace;
                HeFace heFace1 = halfEdge1.IncidentFace;

                UpdateNeighbourFaceList(heFace0, heVertex0, newVertex0, null);
                UpdateNeighbourFaceList(heFace1, heVertex1, newVertex1, null);
            }
            else if (orphanHalfEdges.Count == 3) //One halfedge's twin has no incident face
            {
                HeVertex heVertex0 = orphanHalfEdges[0].Origin;
                HeVertex newVertex0 = orphanHalfEdges[0].Next.Origin;
                HeVertex newVertex1 = orphanHalfEdges[1].Next.Origin;
                HeVertex heVertex1 = orphanHalfEdges[2].Next.Origin;

                HeHalfedge halfEdge0;
                _mesh.GetFacelessHalfedge(heVertex0, heVertex1, out halfEdge0);
                if (halfEdge0 == null || halfEdge0.IncidentFace != null || halfEdge0.Twin == null)
                    throw new NullReferenceException("Halfedge is not valid");
                halfEdge0 = halfEdge0.Twin;

                Debug.Assert(!halfEdge0.IsSplitLine);
                HeFace heFace0 = halfEdge0.IncidentFace;

                UpdateNeighbourFaceList(heFace0, heVertex0, newVertex0, newVertex1);
            }
            else //One halfedge's twin has no incident face
            {
                Debug.Assert(orphanHalfEdges.Count == 2);
                HeVertex heVertex0 = orphanHalfEdges[0].Origin;
                HeVertex newVertex0 = orphanHalfEdges[0].Next.Origin;
                HeVertex heVertex1 = orphanHalfEdges[1].Next.Origin;

                HeHalfedge halfEdge0;
                _mesh.GetFacelessHalfedge(heVertex0, heVertex1, out halfEdge0);
                if (halfEdge0 == null || halfEdge0.IncidentFace != null || halfEdge0.Twin == null)
                    throw new NullReferenceException("Halfedge is not valid");
                halfEdge0 = halfEdge0.Twin;

                Debug.Assert(!halfEdge0.IsSplitLine);
                HeFace heFace0 = halfEdge0.IncidentFace;

                UpdateNeighbourFaceList(heFace0, heVertex0, newVertex0, null);
            }
        }

        private List<HeFace> CreateFaces(HeVertex heVertex, HeVertex newVertex0, HeVertex newVertex1, HeVertex v0, HeVertex v1, HeVertex v2, Vector3d normalNew, Vector3d n0, Vector3d n1, Vector3d n2)
        {
            HeFace newFace0, newFace1, newFace2;
            List<HeFace> newFaces = new List<HeFace>();
            if (heVertex.Equals(v0))
            {
                //throw new Exception("Jay! Branch executed!");
                newFace0 = AddFace(v0, v1, newVertex0, n0, n1, normalNew);
                newFaces.Add(newFace0);
                CheckSanity(v0, v1, v2, newFace0);
                newFace1 = AddFace(newVertex0, v1, newVertex1, normalNew, n1, normalNew);
                newFaces.Add(newFace1);
                CheckSanity(v0, v1, v2, newFace1);
                newFace2 = AddFace(newVertex1, v1, v2, normalNew, n1, n2);
                newFaces.Add(newFace2);
                CheckSanity(v0, v1, v2, newFace2);
            }
            else if (heVertex.Equals(v1))
            {
                //throw new Exception("Jay! Branch executed!");
                newFace0 = AddFace(v1, v2, newVertex0, n1, n2, normalNew);
                newFaces.Add(newFace0);
                CheckSanity(v0, v1, v2, newFace0);
                newFace1 = AddFace(newVertex0, v2, newVertex1, normalNew, n2, normalNew);
                newFaces.Add(newFace1);
                CheckSanity(v0, v1, v2, newFace1);
                newFace2 = AddFace(newVertex1, v2, v0, normalNew, n2, n0);
                newFaces.Add(newFace2);
                CheckSanity(v0, v1, v2, newFace2);
            }
            else
            {
                newFace0 = AddFace(v2, v0, newVertex0, n2, n0, normalNew);
                newFaces.Add(newFace0);
                CheckSanity(v0, v1, v2, newFace0);
                newFace1 = AddFace(newVertex0, v0, newVertex1, normalNew, n0, normalNew);
                newFaces.Add(newFace1);
                CheckSanity(v0, v1, v2, newFace1);
                newFace2 = AddFace(newVertex1, v0, v1, normalNew, n0, n1);
                newFaces.Add(newFace2);
                CheckSanity(v0, v1, v2, newFace2);
            }
            return newFaces;
        }

        private List<HeFace> CreateFaces(HeVertex heVertex, HeVertex newVertex, HeVertex v0, HeVertex v1, HeVertex v2, Vector3d normalNew, Vector3d n0, Vector3d n1, Vector3d n2)
        {
            HeFace newFace0, newFace1;
            List<HeFace> newFaces = new List<HeFace>();
            if (heVertex.Equals(v0))
            {
                newFace0 = AddFace(v0, v1, newVertex, n0, n1, normalNew);
                CheckSanity(v0, v1, v2, newFace0);
                newFaces.Add(newFace0);
                newFace1 = AddFace(v1, v2, newVertex, n1, n2, normalNew);
                CheckSanity(v0, v1, v2, newFace1);
                newFaces.Add(newFace1);
            }
            else if (heVertex.Equals(v1))
            {
                newFace0 = AddFace(v1, v2, newVertex, n1, n2, normalNew);
                CheckSanity(v0, v1, v2, newFace0);
                newFaces.Add(newFace0);
                newFace1 = AddFace(v2, v0, newVertex, n2, n0, normalNew);
                CheckSanity(v0, v1, v2, newFace1);
                newFaces.Add(newFace1);
            }
            else
            {
                newFace0 = AddFace(v2, v0, newVertex, n2, n0, normalNew);
                CheckSanity(v0, v1, v2, newFace0);
                newFaces.Add(newFace0);
                newFace1 = AddFace(v0, v1, newVertex, n0, n1, normalNew);
                CheckSanity(v0, v1, v2, newFace1);
                newFaces.Add(newFace1);
            }
            CheckSanity(v0, v1, v2, newFace0);
            CheckSanity(v0, v1, v2, newFace1);
            return newFaces;
        }

        /**
	     * Split an individual face
	     * 
	     * @param facePos face position on the array of faces
	     * @param segment1 segment representing the intersection of the face with the plane
	     * of another face
	     * @return segment2 segment representing the intersection of other face with the
	     * plane of the current face plane
	     */
        private List<HeFace> SplitFace(HeFace face, Segment segment0, Segment segment1, ref bool split, ref Vector3m splitedgeStart, ref Vector3m splitedgeEnd)
        {
            Vector3m startPos, endPos;
            Segment.PrimitiveType startType, endType, middleType;
            Rational startDist, endDist;
            List<HeFace> newFaces = null;

            HeVertex startVertex = segment0.StartVertex;
            HeVertex endVertex = segment0.EndVertex;

            //starting point: deeper starting point 		
            if (segment1.StartDist > segment0.StartDist)
            {
                startDist = segment1.StartDist;
                startType = segment0.MiddleType;
                startPos = segment1.StartPos;
            }
            else
            {
                startDist = segment0.StartDist;
                startType = segment0.StartType;
                startPos = segment0.StartPos;
            }

            //ending point: deepest ending point
            if (segment1.EndDist < segment0.EndDist)
            {
                endDist = segment1.EndDist;
                endType = segment0.MiddleType;
                endPos = segment1.EndPos;
            }
            else
            {
                endDist = segment0.EndDist;
                endType = segment0.EndType;
                endPos = segment0.EndPos;
            }

            //if (startPos.IsNearlyEqual(endPos)) // if start and end are too near, then there is no split
            //    return null;
            split = true;
            middleType = segment0.MiddleType;
            splitedgeStart = startPos;
            splitedgeEnd = endPos;

            // An edge already exists, no splitting has to be performed
            //VERTEX-_______-VERTEX
            if (startType == Segment.PrimitiveType.Vertex && endType == Segment.PrimitiveType.Vertex)
            {
                split = false;
                Debug.Assert(middleType == Segment.PrimitiveType.Edge || middleType == Segment.PrimitiveType.Vertex);
                if (middleType == Segment.PrimitiveType.Edge)
                {
                    newFaces = new List<HeFace> {face};
                }
                else
                {
                    return null;
                }
                return newFaces;
            }

            // if(isnearlyequal(startpos, endpos) ) => return

            //______-EDGE-______
            else if (middleType == Segment.PrimitiveType.Edge)
            {
                //gets the edge 
                int splitEdge;
                var v0 = face.OuterComponent.Origin;
                var v1 = face.OuterComponent.Next.Origin;
                var v2 = face.OuterComponent.Next.Next.Origin;
                if ((startVertex == v0 && endVertex == v1) || (startVertex == v1 && endVertex == v0))
                {
                    splitEdge = 1;
                }
                else if ((startVertex == v1 && endVertex == v2) || (startVertex == v2 && endVertex == v1))
                {
                    splitEdge = 2;
                }
                else
                {
                    splitEdge = 3;
                }

                //VERTEX-EDGE-EDGE
                if (startType == Segment.PrimitiveType.Vertex)
                {
                    newFaces = BreakFaceInTwo(face, endPos, splitEdge);
                    return newFaces;
                }

                //EDGE-EDGE-VERTEX
                else if (endType == Segment.PrimitiveType.Vertex)
                {
                    newFaces = BreakFaceInTwo(face, startPos, splitEdge);
                    return newFaces;
                }

                // EDGE-EDGE-EDGE
                else if (startDist == endDist)
                {
                    throw new Exception("Jay! Branch executed!");
                    newFaces = BreakFaceInTwo(face, endPos, splitEdge);
                }
                else
                {
                    if ((startVertex == v0 && endVertex == v1) || (startVertex == v1 && endVertex == v2) || (startVertex == v2 && endVertex == v0))
                    {
                        newFaces = BreakFaceInThree(face, startPos, endPos, splitEdge);
                    }
                    else
                    {
                        newFaces = BreakFaceInThree(face, endPos, startPos, splitEdge);
                    }
                }
                return newFaces;
            }

            //______-FACE-______

            //VERTEX-FACE-EDGE
            else if (startType == Segment.PrimitiveType.Vertex && endType == Segment.PrimitiveType.Edge)
            {
                newFaces = BreakFaceInTwo(face, endPos, endVertex);
            }
            //EDGE-FACE-VERTEX
            else if (startType == Segment.PrimitiveType.Edge && endType == Segment.PrimitiveType.Vertex)
            {
                newFaces = BreakFaceInTwo(face, startPos, startVertex);
            }
            //VERTEX-FACE-FACE
            else if (startType == Segment.PrimitiveType.Vertex && endType == Segment.PrimitiveType.Face)
            {
                newFaces = BreakFaceInThree(face, endPos, startVertex);
            }
            //FACE-FACE-VERTEX
            else if (startType == Segment.PrimitiveType.Face && endType == Segment.PrimitiveType.Vertex)
            {
                newFaces = BreakFaceInThree(face, startPos, endVertex);
            }
            //EDGE-FACE-EDGE
            else if (startType == Segment.PrimitiveType.Edge && endType == Segment.PrimitiveType.Edge)
            {
                newFaces = BreakFaceInThree(face, startPos, endPos, startVertex, endVertex);
            }
            //EDGE-FACE-FACE
            else if (startType == Segment.PrimitiveType.Edge && endType == Segment.PrimitiveType.Face)
            {
                newFaces = BreakFaceInFour(face, startPos, endPos, startVertex);
            }
            //FACE-FACE-EDGE
            else if (startType == Segment.PrimitiveType.Face && endType == Segment.PrimitiveType.Edge)
            {
                newFaces = BreakFaceInFour(face, endPos, startPos, endVertex);
            }
            //FACE-FACE-FACE
            else if (startType == Segment.PrimitiveType.Face && endType == Segment.PrimitiveType.Face)
            {
                Vector3m segmentVector = new Vector3m(startPos.X - endPos.X, startPos.Y - endPos.Y, startPos.Z - endPos.Z);

                //gets the vertex more lined with the intersection segment
                int linedVertex;
                HeVertex v0 = face.OuterComponent.Origin;
                HeVertex v1 = face.OuterComponent.Next.Origin;
                HeVertex v2 = face.OuterComponent.Next.Next.Origin;
                Vector3m linedVertexPos;

                Vector3m vertexVector0 = new Vector3m(endPos.X - v0.X, endPos.Y - v0.Y, endPos.Z - v0.Z);
                var dot0 = MathHelper.Pow(segmentVector.Dot(vertexVector0), 2) / vertexVector0.Dot(vertexVector0);

                Vector3m vertexVector1 = new Vector3m(endPos.X - v1.X, endPos.Y - v1.Y, endPos.Z - v1.Z);
                var dot1 = MathHelper.Pow(segmentVector.Dot(vertexVector1), 2) / vertexVector1.Dot(vertexVector1);

                Vector3m vertexVector2 = new Vector3m(endPos.X - v2.X, endPos.Y - v2.Y, endPos.Z - v2.Z);
                var dot2 = MathHelper.Pow(segmentVector.Dot(vertexVector2), 2) / vertexVector2.Dot(vertexVector2);

                if ((dot0 > dot1 && dot0 > dot2) || (dot0 == dot1) && dot0 > dot2)
                {
                    linedVertex = 0;
                    linedVertexPos = v0.Vector3m;
                }
                else if (dot1 > dot2 && dot1 > dot0 || (dot1 == dot2) && dot1 > dot0)
                {
                    linedVertex = 1;
                    linedVertexPos = v1.Vector3m;
                }
                else
                {
                    linedVertex = 2;
                    linedVertexPos = v2.Vector3m;
                }

                // Now find which of the intersection endpoints is nearest to that vertex.
                if (linedVertexPos.DistanceSquared(startPos) > linedVertexPos.DistanceSquared(endPos))
                {
                    newFaces = BreakFaceInFive(face, startPos, endPos, linedVertex);
                }
                else
                {
                    newFaces = BreakFaceInFive(face, endPos, startPos, linedVertex);
                }
            }
            return newFaces;
        }

        private List<HeFace> BreakFaceInTwo(HeFace face, Vector3m endPos, int splitEdge)
        {
            HeVertex v0 = face.V0;
            HeVertex v1 = face.V1;
            HeVertex v2 = face.V2;

            Vector3d normalNew = face.H0.Normal.Vector3d.Unit();
            Vector3d n0 = face.H0.RenderNormal;
            Vector3d n1 = face.H1.RenderNormal;
            Vector3d n2 = face.H2.RenderNormal;

            var vertex = _mesh.AddVertexUnique(new HeVertex(endPos.X, endPos.Y, endPos.Z), v0, v1, v2);
            RemoveFaceFromMesh(face);
            HeFace newFace;
            List<HeFace> newFaces = new List<HeFace>();
            if (splitEdge == 1)
            {
                //addFace(face.v1, vertex, face.v3);
                newFace = AddFace(v0, vertex, v2, n0, normalNew, n2);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(vertex, face.v2, face.v3);
                newFace = AddFace(vertex, v1, v2, normalNew, n1, n2);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
            }
            else if (splitEdge == 2)
            {
                //addFace(face.v2, vertex, face.v1);
                newFace = AddFace(v1, vertex, v0, n1, normalNew, n0);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(vertex, face.v3, face.v1);
                newFace = AddFace(vertex, v2, v0, normalNew, n2, n0);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
            }
            else
            {
                //addFace(face.v3, vertex, face.v2);
                newFace = AddFace(v2, vertex, v1, n2, normalNew, n1);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(vertex, face.v1, face.v2);
                newFace = AddFace(vertex, v0, v1, normalNew, n0, n1);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
            }
            return newFaces;
        }

        private List<HeFace> BreakFaceInTwo(HeFace face, Vector3m endPos, HeVertex endVertex)
        {
            HeVertex v0 = face.V0;
            HeVertex v1 = face.V1;
            HeVertex v2 = face.V2;

            Vector3d normalNew = face.H0.Normal.Vector3d.Unit();
            Vector3d n0 = face.H0.RenderNormal;
            Vector3d n1 = face.H1.RenderNormal;
            Vector3d n2 = face.H2.RenderNormal;

            var vertex = _mesh.AddVertexUnique(new HeVertex(endPos.X, endPos.Y, endPos.Z), v0, v1, v2);
            RemoveFaceFromMesh(face);
            HeFace newFace;
            List<HeFace> newFaces = new List<HeFace>();
            if (endVertex.Equals(v0))
            {
                //addFace(face.v1, vertex, face.v3);
                newFace = AddFace(v0, vertex, v2, n0, normalNew, n2);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(vertex, face.v2, face.v3);
                newFace = AddFace(vertex, v1, v2, normalNew, n1, n2);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
            }
            else if (endVertex.Equals(v1))
            {
                // addFace(face.v2, vertex, face.v1);
                newFace = AddFace(v1, vertex, v0, n1, normalNew, n0);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                // addFace(vertex, face.v3, face.v1);
                newFace = AddFace(vertex, v2, v0, normalNew, n2, n0);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
            }
            else
            {
                //addFace(face.v3, vertex, face.v2);
                newFace = AddFace(v2, vertex, v1, n2, normalNew, n1);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(vertex, face.v1, face.v2);
                newFace = AddFace(vertex, v0, v1, normalNew, n0, n1);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
            }
            return newFaces;
        }

        private List<HeFace> BreakFaceInThree(HeFace face, Vector3m startPos, Vector3m endPos, int splitEdge)
        {
            HeVertex v0 = face.V0;
            HeVertex v1 = face.V1;
            HeVertex v2 = face.V2;

            Vector3d normalNew = face.H0.Normal.Vector3d.Unit();
            Vector3d n0 = face.H0.RenderNormal;
            Vector3d n1 = face.H1.RenderNormal;
            Vector3d n2 = face.H2.RenderNormal;

            var vertex0 = _mesh.AddVertexUnique(new HeVertex(startPos.X, startPos.Y, startPos.Z), v0, v1, v2);
            var vertex1 = _mesh.AddVertexUnique(new HeVertex(endPos.X, endPos.Y, endPos.Z), v0, v1, v2);
            RemoveFaceFromMesh(face);
            HeFace newFace;
            List<HeFace> newFaces = new List<HeFace>();
            if (splitEdge == 1)
            {
                //addFace(face.v1, vertex1, face.v3);
                newFace = AddFace(v0, vertex0, v2, n0, normalNew, n2);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(vertex1, vertex2, face.v3);
                newFace = AddFace(vertex0, vertex1, v2, normalNew, normalNew, n2);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(vertex2, face.v2, face.v3);
                newFace = AddFace(vertex1, v1, v2, normalNew, n1, n2);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
            }
            else if (splitEdge == 2)
            {
                //addFace(face.v2, vertex1, face.v1);
                newFace = AddFace(v1, vertex0, v0, n1, normalNew, n0);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(vertex1, vertex2, face.v1);
                newFace = AddFace(vertex0, vertex1, v0, normalNew, normalNew, n0);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(vertex2, face.v3, face.v1);
                newFace = AddFace(vertex1, v2, v0, normalNew, n2, n0);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
            }
            else
            {
                //addFace(face.v3, vertex1, face.v2);
                newFace = AddFace(v2, vertex0, v1, n2, normalNew, n1);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(vertex1, vertex2, face.v2);
                newFace = AddFace(vertex0, vertex1, v1, normalNew, normalNew, n1);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(vertex2, face.v1, face.v2);
                newFace = AddFace(vertex1, v0, v1, normalNew, n0, n1);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
            }
            return newFaces;
        }

        private List<HeFace> BreakFaceInThree(HeFace face, Vector3m startPos, HeVertex endVertex)
        {
            HeVertex v0 = face.V0;
            HeVertex v1 = face.V1;
            HeVertex v2 = face.V2;

            Vector3d normalNew = face.H0.Normal.Vector3d.Unit();
            Vector3d n0 = face.H0.RenderNormal;
            Vector3d n1 = face.H1.RenderNormal;
            Vector3d n2 = face.H2.RenderNormal;

            var vertex = _mesh.AddVertexUnique(new HeVertex(startPos.X, startPos.Y, startPos.Z), v0, v1, v2);
            RemoveFaceFromMesh(face);
            HeFace newFace;
            List<HeFace> newFaces = new List<HeFace>();
            if (endVertex.Equals(v0))
            {
                //addFace(face.v1, face.v2, vertex);
                newFace = AddFace(v0, v1, vertex, n0, n1, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(face.v2, face.v3, vertex);
                newFace = AddFace(v1, v2, vertex, n1, n2, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(face.v3, face.v1, vertex);
                newFace = AddFace(v2, v0, vertex, n2, n0, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
            }
            else if (endVertex.Equals(v1))
            {
                //addFace(face.v2, face.v3, vertex);
                newFace = AddFace(v1, v2, vertex, n1, n2, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(face.v3, face.v1, vertex);
                newFace = AddFace(v2, v0, vertex, n2, n0, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(face.v1, face.v2, vertex);
                newFace = AddFace(v0, v1, vertex, n0, n1, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
            }
            else
            {
                //addFace(face.v3, face.v1, vertex);
                newFace = AddFace(v2, v0, vertex, n2, n0, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(face.v1, face.v2, vertex);
                newFace = AddFace(v0, v1, vertex, n0, n1, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(face.v2, face.v3, vertex);
                newFace = AddFace(v1, v2, vertex, n1, n2, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
            }
            return newFaces;
        }

        private List<HeFace> BreakFaceInThree(HeFace face, Vector3m startPos, Vector3m endPos, HeVertex startVertex,
            HeVertex endVertex)
        {
            HeVertex v0 = face.V0;
            HeVertex v1 = face.V1;
            HeVertex v2 = face.V2;

            Vector3d normalNew = face.H0.Normal.Vector3d.Unit();
            Vector3d n0 = face.H0.RenderNormal;
            Vector3d n1 = face.H1.RenderNormal;
            Vector3d n2 = face.H2.RenderNormal;

            var vertex0 = _mesh.AddVertexUnique(new HeVertex(startPos.X, startPos.Y, startPos.Z), v0, v1, v2);
            var vertex1 = _mesh.AddVertexUnique(new HeVertex(endPos.X, endPos.Y, endPos.Z), v0, v1, v2);
            RemoveFaceFromMesh(face);
            HeFace newFace;
            List<HeFace> newFaces = new List<HeFace>();
            if (startVertex.Equals(v0) && endVertex.Equals(v1))
            {
                //addFace(face.v1, vertex1, vertex2);
                newFace = AddFace(v0, vertex0, vertex1, n0, normalNew, normalNew);
                newFaces.Add(newFace);
                CheckSanity(v0, v1, v2, newFace);
                //addFace(face.v1, vertex2, face.v3);
                newFace = AddFace(v0, vertex1, v2, n0, normalNew, n2);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(vertex1, face.v2, vertex2);
                newFace = AddFace(vertex0, v1, vertex1, normalNew, n1, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
            }
            else if (startVertex.Equals(v1) && endVertex.Equals(v0))
            {
                //addFace(face.v1, vertex2, vertex1);
                newFace = AddFace(v0, vertex1, vertex0, n0, normalNew, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(face.v1, vertex1, face.v3);
                newFace = AddFace(v0, vertex0, v2, n0, normalNew, n2);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(vertex2, face.v2, vertex1);
                newFace = AddFace(vertex1, v1, vertex0, normalNew, n1, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
            }
            else if (startVertex.Equals(v1) && endVertex.Equals(v2))
            {
                //addFace(face.v2, vertex1, vertex2);
                newFace = AddFace(v1, vertex0, vertex1, n1, normalNew, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(face.v2, vertex2, face.v1)
                newFace = AddFace(v1, vertex1, v0, n1, normalNew, n0);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(vertex1, face.v3, vertex2);
                newFace = AddFace(vertex0, v2, vertex1, normalNew, n2, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
            }
            else if (startVertex.Equals(v2) && endVertex.Equals(v1))
            {
                //addFace(face.v2, vertex2, vertex1);
                newFace = AddFace(v1, vertex1, vertex0, n1, normalNew, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(face.v2, vertex1, face.v1);
                newFace = AddFace(v1, vertex0, v0, n1, normalNew, n0);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(vertex2, face.v3, vertex1);
                newFace = AddFace(vertex1, v2, vertex0, normalNew, n2, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
            }
            else if (startVertex.Equals(v2) && endVertex.Equals(v0))
            {
                //addFace(face.v3, vertex1, vertex2);
                newFace = AddFace(v2, vertex0, vertex1, n2, normalNew, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(face.v3, vertex2, face.v2);
                newFace = AddFace(v2, vertex1, v1, n2, normalNew, n1);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(vertex1, face.v1, vertex2);
                newFace = AddFace(vertex0, v0, vertex1, normalNew, n0, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
            }
            else
            {
                //addFace(face.v3, vertex2, vertex1);
                newFace = AddFace(v2, vertex1, vertex0, n2, normalNew, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(face.v3, vertex1, face.v2);
                newFace = AddFace(v2, vertex0, v1, n2, normalNew, n1);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(vertex2, face.v1, vertex1);
                newFace = AddFace(vertex1, v0, vertex0, normalNew, n0, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
            }
            return newFaces;
        }

        private List<HeFace> BreakFaceInFour(HeFace face, Vector3m startPos, Vector3m endPos, HeVertex endVertex)
        {
            HeVertex v0 = face.V0;
            HeVertex v1 = face.V1;
            HeVertex v2 = face.V2;

            Vector3d normalNew = face.H0.Normal.Vector3d.Unit();
            Vector3d n0 = face.H0.RenderNormal;
            Vector3d n1 = face.H1.RenderNormal;
            Vector3d n2 = face.H2.RenderNormal;

            var vertex0 = _mesh.AddVertexUnique(new HeVertex(startPos.X, startPos.Y, startPos.Z), v0, v1, v2);
            var vertex1 = _mesh.AddVertexUnique(new HeVertex(endPos.X, endPos.Y, endPos.Z), v0, v1, v2);
            RemoveFaceFromMesh(face);
            HeFace newFace;
            List<HeFace> newFaces = new List<HeFace>();
            if (endVertex.Equals(v0))
            {
                //addFace(face.v1, vertex1, vertex2);
                newFace = AddFace(v0, vertex0, vertex1, n0, normalNew, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(vertex1, face.v2, vertex2);
                newFace = AddFace(vertex0, v1, vertex1, normalNew, n1, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(face.v2, face.v3, vertex2);
                newFace = AddFace(v1, v2, vertex1, n1, n2, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(face.v3, face.v1, vertex2);
                newFace = AddFace(v2, v0, vertex1, n2, n0, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
            }
            else if (endVertex.Equals(v1))
            {
                //addFace(face.v2, vertex1, vertex2);
                newFace = AddFace(v1, vertex0, vertex1, n1, normalNew, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(vertex1, face.v3, vertex2);
                newFace = AddFace(vertex0, v2, vertex1, normalNew, n2, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(face.v3, face.v1, vertex2);
                newFace = AddFace(v2, v0, vertex1, n2, n0, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(face.v1, face.v2, vertex2);
                newFace = AddFace(v0, v1, vertex1, n0, n1, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
            }
            else
            {
                //addFace(face.v3, vertex1, vertex2);
                newFace = AddFace(v2, vertex0, vertex1, n2, normalNew, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(vertex1, face.v1, vertex2);
                newFace = AddFace(vertex0, v0, vertex1, normalNew, n0, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(face.v1, face.v2, vertex2);
                newFace = AddFace(v0, v1, vertex1, n0, n1, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(face.v2, face.v3, vertex2);
                newFace = AddFace(v1, v2, vertex1, n1, n2, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
            }
            return newFaces;
        }

        private List<HeFace> BreakFaceInFive(HeFace face, Vector3m startPos, Vector3m endPos, int linedVertex)
        {
            HeVertex v0 = face.V0;
            HeVertex v1 = face.V1;
            HeVertex v2 = face.V2;

            Vector3d normalNew = face.H0.Normal.Vector3d.Unit();
            Vector3d n0 = face.H0.RenderNormal;
            Vector3d n1 = face.H1.RenderNormal;
            Vector3d n2 = face.H2.RenderNormal;

            var vertex0 = _mesh.AddVertexUnique(new HeVertex(startPos.X, startPos.Y, startPos.Z), v0, v1, v2);
            var vertex1 = _mesh.AddVertexUnique(new HeVertex(endPos.X, endPos.Y, endPos.Z), v0, v1, v2);
            RemoveFaceFromMesh(face);
            HeFace newFace;
            List<HeFace> newFaces = new List<HeFace>();
            if (linedVertex == 0)
            {
                //addFace(face.v2, face.v3, vertex1);
                newFace = AddFace(v1, v2, vertex0, n1, n2, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(face.v2, vertex1, vertex2);
                newFace = AddFace(v1, vertex0, vertex1, n1, normalNew, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(face.v3, vertex2, vertex1);
                newFace = AddFace(v2, vertex1, vertex0, n2, normalNew, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(face.v2, vertex2, face.v1);
                newFace = AddFace(v1, vertex1, v0, n1, normalNew, n0);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(face.v3, face.v1, vertex2);
                newFace = AddFace(v2, v0, vertex1, n2, n0, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
            }
            else if (linedVertex == 1)
            {
                //addFace(face.v3, face.v1, vertex1);
                newFace = AddFace(v2, v0, vertex0, n2, n0, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(face.v3, vertex1, vertex2);
                newFace = AddFace(v2, vertex0, vertex1, n2, normalNew, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(face.v1, vertex2, vertex1);
                newFace = AddFace(v0, vertex1, vertex0, n0, normalNew, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(face.v3, vertex2, face.v2);
                newFace = AddFace(v2, vertex1, v1, n2, normalNew, n1);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(face.v1, face.v2, vertex2);
                newFace = AddFace(v0, v1, vertex1, n0, n1, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
            }
            else
            {
                //addFace(face.v1, face.v2, vertex1);
                newFace = AddFace(v0, v1, vertex0, n0, n1, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(face.v1, vertex1, vertex2);
                newFace = AddFace(v0, vertex0, vertex1, n0, normalNew, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(face.v2, vertex2, vertex1);
                newFace = AddFace(v1, vertex1, vertex0, n1, normalNew, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(face.v1, vertex2, face.v3);
                newFace = AddFace(v0, vertex1, v2, n0, normalNew, n2);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
                //addFace(face.v2, face.v3, vertex2);
                newFace = AddFace(v1, v2, vertex1, n1, n2, normalNew);
                CheckSanity(v0, v1, v2, newFace);
                newFaces.Add(newFace);
            }
            return newFaces;
        }

        private void RemoveFaceFromMesh(HeFace face)
        {
            var fl = _intersectionResult.TryGetFaceList(face);
            if (fl != null)
                fl.BreakLoop = true;
            _mesh.RemoveFace(face, false);

        }

        private HeFace AddFace(HeVertex v0, HeVertex v1, HeVertex v2, Vector3d n0, Vector3d n1, Vector3d n2)
        {
            Vector3d[] renderNormals = {n0, n1, n2};
            var newFace = _mesh.AddFace(v0.Index, v1.Index, v2.Index, renderNormals);
            return newFace;
        }


        internal static void CheckSanity(HeVertex v0, HeVertex v1, HeVertex v2, HeFace newFace)
        {
            
        }

        private void SetSplitedge(HeHalfedge splitedge)
        {
            if (splitedge == null)
                throw new Exception("A split edge was not set!");
            bool res = _splitResult.Splitlines.Add(splitedge);
            splitedge.SetSplitline(true);
        }

        private void UpdateIntersectionResultList(List<HeFace> newFaces, HeFace[] remainingFacesB)
        {
            newFaces.ForEach(x => _intersectionResult.AddFaces(x, remainingFacesB));
        }

        private HeHalfedge GetSplitHalfedge(List<HeFace> newFaces, Vector3m splitedgeStart, Vector3m splitEdgeEnd)
        {
            foreach (var newFace in newFaces)
            {
                foreach (var curHalfedge in newFace.GetFaceCirculator())
                {
                    if (curHalfedge.Origin.Vector3m.Equals(splitedgeStart) && curHalfedge.Next.Origin.Vector3m.Equals(splitEdgeEnd))
                    {
                        return curHalfedge;
                    }
                    if (curHalfedge.Twin.Origin.Vector3m.Equals(splitedgeStart) && curHalfedge.Twin.Next.Origin.Vector3m.Equals(splitEdgeEnd))
                    {
                        return curHalfedge.Twin;
                    }

                }
            }
            return null;
        }
    }
}
