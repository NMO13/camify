using System;
using System.Collections.Generic;
using GraphicsEngine.HalfedgeMesh;
using Shared.Geometry;

namespace GeometryCalculation
{
    internal static class SweptVolumeCalculator
    {
        private const int FRONT = 1;
        private const int BACK = -1;
        private const int COPLANAR = 0;
        internal static void Calculate(HeMesh m, Vector3m direction)
        {
            Dictionary<HeVertex, HeVertex> buddies = new Dictionary<HeVertex, HeVertex>();
            List<HeFace> backFaces = new List<HeFace>();
            // calculate all front and back faces
            foreach (HeFace p in m.FaceList)
            {
                Vector3m testPoint = p.OuterComponent.Origin.Vector3m.Plus(direction);
                int type = LooksIntoEmptySpace(testPoint, p);
                if (type == FRONT)
                {
                    var h0 = p.OuterComponent;
                    var h1 = p.OuterComponent.Next;
                    var h2 = p.OuterComponent.Next.Next;

                    var v0 = AddTranslatedVertex(h0, direction, buddies, m);
                    var v1 = AddTranslatedVertex(h1, direction, buddies, m);
                    var v2 = AddTranslatedVertex(h2, direction, buddies, m);

                    m.RemoveFace(p, true);
                    m.AddFace(v0.Index, v1.Index, v2.Index, null); //TODO
                }
                else
                {
                    backFaces.Add(p);
                }
            }

            HeHalfedge startedge = null;
            foreach (var backFace in backFaces)
            {
                if (backFace.OuterComponent.Twin.IncidentFace == null)
                {
                    startedge = backFace.OuterComponent.Twin;
                    break;
                }
                else if (backFace.OuterComponent.Next.Twin.IncidentFace == null)
                {
                    startedge = backFace.OuterComponent.Next.Twin;
                    break;
                }
                else if (backFace.OuterComponent.Next.Next.Twin.IncidentFace == null)
                {
                    startedge = backFace.OuterComponent.Next.Next.Twin;
                    break;
                }
            }

            if (startedge == null)
                throw new Exception("The start edge of the SV is not correct");
            // traverse boundary and create middle polygons
            JumpFrontFace(startedge, m, buddies);
        }

        private static HeVertex AddTranslatedVertex(HeHalfedge h0, Vector3m direction, Dictionary<HeVertex, HeVertex> buddies, HeMesh m)
        {
            HeVertex value;
            HeVertex newV0;
            if (!buddies.TryGetValue(h0.Origin, out value))
            {
                newV0 = new HeVertex(h0.Origin.X + direction.X, h0.Origin.Y + direction.Y, h0.Origin.Z + direction.Z);
                newV0.IsOnSweptVolumeSurface = true;
                newV0 = m.AddVertex(newV0);
                buddies.Add(h0.Origin, newV0);
            }
            else
            {
                newV0 = value;
            }
            return newV0;
        }

        private static void JumpFrontFace(HeHalfedge startedge, HeMesh m, Dictionary<HeVertex, HeVertex> buddies)
        {
            HeHalfedge boundaryedge = startedge;
            do
            {
                boundaryedge = GetNextBoundaryEdge(boundaryedge);
                if (boundaryedge.Twin.IncidentFace == null)
                    throw new Exception("The boundary edge of the SV is not correct");
                CreateQuad(boundaryedge, m, buddies);

            } while (boundaryedge != startedge);
        }

        private static void CreateQuad(HeHalfedge boundaryedge, HeMesh m, Dictionary<HeVertex, HeVertex> buddies)
        {
            HeVertex v0 = boundaryedge.Origin;
            HeVertex v1 = boundaryedge.Twin.Origin;

            HeVertex v0Front, v1Front;
            if (!buddies.TryGetValue(v0, out v0Front))
                throw new Exception("Vertex was not found in dictionary");
            if (!buddies.TryGetValue(v1, out v1Front))
                throw new Exception("Vertex was not found in dictionary");

            m.AddFace(v0.Index, v1.Index, v1Front.Index, null); //TODO
            m.AddFace(v0.Index, v1Front.Index, v0Front.Index, null);//TODO
        }

        private static HeHalfedge GetNextBoundaryEdge(HeHalfedge startEdge)
        {
            foreach (var incidentEdge in startEdge.Twin.Origin.IncidentEdges)
            {
                if (incidentEdge.IncidentFace == null)
                    return incidentEdge;
            }
            throw new Exception("No edge was found");
        }

        private static int LooksIntoEmptySpace(Vector3m testPoint, HeFace p)
        {
            return p.DistanceSign(testPoint);
        }
    }
}
