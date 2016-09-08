using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using GraphicsEngine.Geometry.CollisionCheck;
using GraphicsEngine.HalfedgeMesh;
using Shared.Geometry;
using Shared.Geometry.HalfedgeMesh;

namespace GraphicsEngine.Geometry.Boolean_Ops
{
    class SharedSplitcurveCalculator
    {
        class MultipleValues
        {
            internal Dictionary<HeFace, List<HeFace>> a = new Dictionary<HeFace, List<HeFace>>();
            internal Dictionary<HeFace, List<HeFace>> b = new Dictionary<HeFace, List<HeFace>>();

            internal void AddNextFace(HeFace faceA, HeFace faceB, Dictionary<HeFace, List<HeFace>> dictionary)
            {
                List<HeFace> list;
                dictionary.TryGetValue(faceA, out list);
                if (list != null)
                    list.Add(faceB);
                else
                {
                    list = new List<HeFace>();
                    list.Add(faceB);
                    dictionary.Add(faceA, list);
                }

            }
        }

        internal bool FindSplitcurveParallel(List<FacePair> intersectionResult, HeMesh mesh, IntersectionResult a,
            IntersectionResult b)
        {
            Debug.Assert(mesh != null);
            bool intersect = false;

            Parallel.For(0, intersectionResult.Count,
                () => new MultipleValues { a = new Dictionary<HeFace, List<HeFace>>(), b = new Dictionary<HeFace, List<HeFace>>() },

                (aIndex, state, tlv) =>
                {
                    HeFace faceA = intersectionResult[aIndex].A;
                    HeFace faceB = intersectionResult[aIndex].B;

                    HeVertex faceAv0 = faceA.OuterComponent.Origin;
                    HeVertex faceAv1 = faceA.OuterComponent.Next.Origin;
                    HeVertex faceAv2 = faceA.OuterComponent.Next.Next.Origin;

                    HeVertex faceBv0 = faceB.OuterComponent.Origin;
                    HeVertex faceBv1 = faceB.OuterComponent.Next.Origin;
                    HeVertex faceBv2 = faceB.OuterComponent.Next.Next.Origin;

                    //distances signs from the face1 vertices to the face2 plane 
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
                                if (CheckSpecialCases(faceA, signFaceAVert0, signFaceAVert1, signFaceAVert2, faceB,
                                    signFaceBVert0, signFaceBVert1, signFaceBVert2))
                                {
                                    // we have a split

                                    var faceAClone = faceA.Clone() as HeFace;
                                    var faceBClone = faceB.Clone() as HeFace;
                                    tlv.AddNextFace(faceA, faceBClone, tlv.a);
                                    tlv.AddNextFace(faceB, faceAClone, tlv.b);
                                    intersect = true;
                                }
                            }
                        }
                    }
                    return tlv;
                },
                (MultipleValues x) =>
                {
                    lock (a)
                    {
                        foreach (var key in x.a.Keys)
                        {
                            List<HeFace> list;
                            x.a.TryGetValue(key, out list);
                            a.AddFaces(key, list.ToArray());
                        }

                        foreach (var key in x.b.Keys)
                        {
                            List<HeFace> list;
                            x.b.TryGetValue(key, out list);
                            b.AddFaces(key, list.ToArray());
                        }
                    }
                }
            );
            return intersect;
        }

        internal bool FindSplitcurve(List<FacePair> intersectionResult, HeMesh mesh, IntersectionResult a, IntersectionResult b)
        {
            Debug.Assert(mesh != null);
            bool intersect = false;

            for (int aIndex = 0; aIndex < intersectionResult.Count; aIndex++)
            {
                HeFace faceA = intersectionResult[aIndex].A;
                HeFace faceB = intersectionResult[aIndex].B;

                HeVertex faceAv0 = faceA.OuterComponent.Origin;
                HeVertex faceAv1 = faceA.OuterComponent.Next.Origin;
                HeVertex faceAv2 = faceA.OuterComponent.Next.Next.Origin;

                HeVertex faceBv0 = faceB.OuterComponent.Origin;
                HeVertex faceBv1 = faceB.OuterComponent.Next.Origin;
                HeVertex faceBv2 = faceB.OuterComponent.Next.Next.Origin;

                //distances signs from the face1 vertices to the face2 plane 
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
                            if (CheckSpecialCases(faceA, signFaceAVert0, signFaceAVert1, signFaceAVert2, faceB,
                                signFaceBVert0, signFaceBVert1, signFaceBVert2))
                            {
                                // we have a split
                                var faceBClone = faceB.Clone() as HeFace;
                                a.AddNextFace(faceA, faceBClone);
                                var faceAClone = faceA.Clone() as HeFace;
                                b.AddNextFace(faceB, faceAClone);
                                intersect = true;
                            }
                        }
                    }
                }
            }
            return intersect;
        }

        private bool CheckSpecialCases(HeFace faceA, int signFaceAVert0, int signFaceAVert1, int signFaceAVert2, HeFace faceB, int signFaceBVert0, int signFaceBVert1, int signFaceBVert2)
        {
            var resA = ClassifyWithRespectToOther(signFaceAVert0, signFaceAVert1, signFaceAVert2);
            var resB = ClassifyWithRespectToOther(signFaceBVert0, signFaceBVert1, signFaceBVert2);

            if (IsCrossing(resA, signFaceAVert0, signFaceAVert1, signFaceAVert2)) // A crosses the plane of B
            {
                // If two points of B lie on A, then it is has be decided where the the third point lies, inside or outside of A
                if (resB == 1) //v1B and v2B are on A
                {
                    return signFaceBVert0 == -1;
                }
                else if (resB == 2) //v0B and v2B are on A
                {
                    return signFaceBVert1 == -1;
                }
                else if (resB == 4) //v0B and v1B are on A
                {
                    return signFaceBVert2 == -1;
                }
                else // only one point of B on plane of A
                {
                    return IsCrossing(resB, signFaceBVert0, signFaceBVert1, signFaceBVert2);
                }
            }
            else if (IsCrossing(resB, signFaceBVert0, signFaceBVert1, signFaceBVert2)) // B crosses the plane of A
            {
                Debug.Assert(resA != 7);
                if (resA == 3)
                    Debug.Assert(signFaceAVert0 == signFaceAVert1);
                if (resA == 5)
                    Debug.Assert(signFaceAVert0 == signFaceAVert2);
                if (resA == 6)
                    Debug.Assert(signFaceAVert1 == signFaceAVert2);

                int sign1;
                int sign2;
                int sign3;
                // if two points of A lie on B, then it has to be decided whether or not B penetrates A
                if (resA == 1) //v1A and v2A are on B
                {
                    sign1 = IsBetween(faceB.OuterComponent.Origin.Vector3m, faceA, faceA.OuterComponent.Next.Twin);
                    sign2 = IsBetween(faceB.OuterComponent.Next.Origin.Vector3m, faceA, faceA.OuterComponent.Next.Twin);
                    sign3 = IsBetween(faceB.OuterComponent.Next.Next.Origin.Vector3m, faceA, faceA.OuterComponent.Next.Twin);
                    if (sign1 == -1 || sign2 == -1 || sign3 == -1)
                        return true;
                    return false;
                }
                else if (resA == 2) //v0A and v2A are on B
                {
                    sign1 = IsBetween(faceB.OuterComponent.Origin.Vector3m, faceA, faceA.OuterComponent.Next.Next.Twin);
                    sign2 = IsBetween(faceB.OuterComponent.Next.Origin.Vector3m, faceA, faceA.OuterComponent.Next.Next.Twin);
                    sign3 = IsBetween(faceB.OuterComponent.Next.Next.Origin.Vector3m, faceA, faceA.OuterComponent.Next.Next.Twin);
                    if (sign1 == -1 || sign2 == -1 || sign3 == -1)
                        return true;
                    return false;
                }
                else if (resA == 4) //v0A and v1A are on B
                {
                    sign1 = IsBetween(faceB.OuterComponent.Origin.Vector3m, faceA, faceA.OuterComponent.Twin);
                    sign2 = IsBetween(faceB.OuterComponent.Next.Origin.Vector3m, faceA, faceA.OuterComponent.Twin);
                    sign3 = IsBetween(faceB.OuterComponent.Next.Next.Origin.Vector3m, faceA, faceA.OuterComponent.Twin);
                    if (sign1 == -1 || sign2 == -1 || sign3 == -1)
                        return true;
                    return false;
                }
            }
            else // edge-edge overlap: check if triangle of B is between two triangles of A
            {
                HeHalfedge rotationEdge;

                Debug.Assert(resA != 7 && resB != 7);
                if (resA == 1) // v1A and v2A are on B
                {
                    rotationEdge = faceA.OuterComponent.Next.Twin;
                }
                else if (resA == 2) // v0A and v2A are on B
                {
                    rotationEdge = faceA.OuterComponent.Next.Next.Twin;
                }
                else if (resA == 4) // v0A and v1A are on B
                {
                    rotationEdge = faceA.OuterComponent.Twin;
                }
                else // only one point on plane
                {
                    if (resA == 3 && signFaceAVert0 != signFaceAVert1)
                        return true;
                    if (resA == 5 && signFaceAVert0 != signFaceAVert2)
                        return true;
                    if (resA == 6 && signFaceAVert1 != signFaceAVert2)
                        return true;
                    return false;
                }
                if (resB == 1)
                {
                    if (!IsBehindA(faceA, faceB.OuterComponent.Origin.Vector3m))
                        return false;
                    return IsBetween(faceB.OuterComponent.Origin.Vector3m, faceA, rotationEdge) == -1;
                }
                else if (resB == 2)
                {
                    if (!IsBehindA(faceA, faceB.OuterComponent.Next.Origin.Vector3m))
                        return false;
                    return IsBetween(faceB.OuterComponent.Next.Origin.Vector3m, faceA, rotationEdge) == -1;
                }
                else if (resB == 4)
                {
                    if (!IsBehindA(faceA, faceB.OuterComponent.Next.Next.Origin.Vector3m))
                        return false;
                    return IsBetween(faceB.OuterComponent.Next.Next.Origin.Vector3m, faceA, rotationEdge) == -1;
                }
                else // only one point on plane
                {
                    if (resB == 3 && signFaceBVert0 != signFaceBVert1)
                        return true;
                    if (resB == 5 && signFaceBVert0 != signFaceBVert2)
                        return true;
                    if (resB == 6 && signFaceBVert1 != signFaceBVert2)
                        return true;
                    return false;
                }
            }
            return false;
        }

        // If A's face has an edge angle > 180° then it is possible that halfedge of B would lie inside of A
        // but would calculate an inverted edge which would lead to a wrong split line.
        // So we test only halfedges of B which lie behind the edge of A
        // For a face angle < 180°, B's edge always lies behind the edges of A's face. 
        private bool IsBehindA(HeFace faceA, Vector3m testPoint)
        {
            int sign = faceA.DistanceSign(testPoint);
            return sign == -1;
        }

        private int IsBetween(Vector3m c, HeFace a, HeHalfedge b)
        {
            var vsca = a.DistanceSign(c);
            var vscb = b.IncidentFace.DistanceSign(c);

            var vA1 = b.Next.Next.Origin.Vector3m;
            int vsA1 = a.DistanceSign(vA1);

            if (vsA1 > 0)
            {
                if (vsca < 0 || vscb < 0)
                    return -1;
                else if (vsca == 0 || vscb == 0)
                    return 0;
                else
                    return 1;
            }
            else if (vsA1 < 0)
            {
                if (vsca < 0 && vscb < 0)
                    return -1;
                else if (vsca == 0 || vscb == 0)
                    return 0;
                else
                    return 1;
            }
            Debug.Assert(vsca == vscb);
            return vsca;
        }

        private bool IsCrossing(int res, int signFaceVert0, int signFaceVert1, int signFaceVert2)
        {
            if (res == 7)
            {
                Debug.Assert(!(signFaceVert0 == signFaceVert1 && signFaceVert1 == signFaceVert2));
                return true;
            }
            if (res == 3 && signFaceVert0 != signFaceVert1)
                return true; // only one point on plane
            if (res == 5 && signFaceVert0 != signFaceVert2)
                return true;
            if (res == 6 && signFaceVert1 != signFaceVert2)
                return true;
            return false;
        }

        private int ClassifyWithRespectToOther(int v0, int v1, int v2)
        {
            int bitfield = 0;
            bitfield |= v0 != 0 ? 1 : 0;
            bitfield |= v1 != 0 ? 2 : 0;
            bitfield |= v2 != 0 ? 4 : 0;
            return bitfield;
        }


    }
}
