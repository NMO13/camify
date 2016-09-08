using System;
using System.Collections.Generic;
using GraphicsEngine.HalfedgeMesh;
using Shared.Geometry;
using Shared.Geometry.HalfedgeMesh;

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
                    Vector3d n0 = p.H0.RenderNormal;
                    Vector3d n1 = p.H1.RenderNormal;
                    Vector3d n2 = p.H2.RenderNormal;
                    Vector3d[] renderNormals = {n0, n1, n2};

                    var v0 = AddTranslatedVertex(p.H0, direction, buddies, m);
                    var v1 = AddTranslatedVertex(p.H1, direction, buddies, m);
                    var v2 = AddTranslatedVertex(p.H2, direction, buddies, m);

                    m.RemoveFace(p, true);
                    m.AddFace(v0.Index, v1.Index, v2.Index, renderNormals);
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

            CreateFace(v0, v1, v1Front, m);
            CreateFace(v0, v1Front, v0Front, m);
        }

        private static void CreateFace(HeVertex v0, HeVertex v1, HeVertex v2, HeMesh heMesh)
        {
            var normal = (v1.Vector3d - v0.Vector3d).Cross(v2.Vector3d - v0.Vector3d).Unit();
            Vector3d[] renderNormals = {normal, normal.Clone() as Vector3d, normal.Clone() as Vector3d};

            heMesh.AddFace(v0.Index, v1.Index, v2.Index, renderNormals);
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
