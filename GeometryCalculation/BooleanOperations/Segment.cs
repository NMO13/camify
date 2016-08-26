using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection.Emit;
using GraphicsEngine.HalfedgeMesh;
using GraphicsEngine.Math;
using Microsoft.SolverFoundation.Common;
using Shared;

namespace GraphicsEngine.Geometry.Boolean_Ops
{
    internal class Segment
    {
        private IntersectionLine _line;
        private int _index;


        internal Rational StartDist; /** distance from the segment starting point to the point defining the plane */
        internal Rational EndDist; /** distance from the segment ending point to the point defining the plane */

        internal PrimitiveType StartType { get; set; }  /** starting point status relative to the face */
        internal PrimitiveType MiddleType { get; set; } /** intermediate status relative to the face */
        internal PrimitiveType EndType { get; set; } /** ending point status relative to the face */
        internal HeHalfedge StartHalfedge { get; set; }

        internal HeVertex StartVertex { get; set; } /** nearest vertex from the starting point */
        internal HeVertex EndVertex { get; set; } /** nearest vertex from the ending point */
        internal HeHalfedge EndHalfedge { get; set; }

        internal Vector3m StartPos; /** start of the intersection point */
        internal Vector3m EndPos; /** end of the intersection point */

        internal HeHalfedge CutedgeStart;
        internal HeHalfedge CutedgeEnd;

        internal Segment(IntersectionLine line, HeFace face, int sign0, int sign1, int sign2)
        {
            _line = line;
            var he0 = face.OuterComponent;
            var he1 = face.OuterComponent.Next;
            var he2 = face.OuterComponent.Next.Next;

            //VERTEX is an end
            if (sign0 == 0)
            {
                SetVertex(he0);

                //other vertices on the same side - VERTEX-VERTEX VERTEX
                if (sign1 == sign2)
                {
                    SetVertex(he0);
                }
            }

            //VERTEX is an end
            if (sign1 == 0)
            {
                SetVertex(he1);

                //other vertices on the same side - VERTEX-VERTEX VERTEX
                if (sign0 == sign2)
                {
                    SetVertex(he1);
                }
            }

            //VERTEX is an end
            if (sign2 == 0)
            {
                SetVertex(he2);

                //other vertices on the same side - VERTEX-VERTEX VERTEX
                if (sign0 == sign1)
                {
                    SetVertex(he2);
                }
            }

            //There are undefined ends - one or more edges cut the planes intersection line
            if (GetNumEndsSet() != 2)
            {
                //EDGE is an end
                if ((sign0 == 1 && sign1 == -1) || (sign0 == -1 && sign1 == 1))
                {
                    SetEdge(face.OuterComponent);
                }
                //EDGE is an end
                if ((sign1 == 1 && sign2 == -1) || (sign1 == -1 && sign2 == 1))
                {
                    SetEdge(face.OuterComponent.Next);
                }
                //EDGE is an end
                if ((sign2 == 1 && sign0 == -1) || (sign2 == -1 && sign0 == 1))
                {
                    SetEdge(face.OuterComponent.Next.Next);
                }
            }
        }
  
        private void SetVertex(HeHalfedge halfedge)
        {
            //none end were defined - define starting point as VERTEX
            if (_index == 0)
            {
                StartVertex = halfedge.Origin;
                StartType = PrimitiveType.Vertex;
                StartDist = _line.ComputePointToPointDistance(halfedge.Origin.Vector3m);
                StartPos = StartVertex.Vector3m;
                StartHalfedge = halfedge;
                _index++;
                return;
            }
            //starting point were defined - define ending point as VERTEX
            if (_index == 1)
            {
                EndVertex = halfedge.Origin;
                EndType = PrimitiveType.Vertex;
                EndDist = _line.ComputePointToPointDistance(halfedge.Origin.Vector3m);
                EndPos = EndVertex.Vector3m;
                EndHalfedge = halfedge;
                _index++;

                //defining middle based on the starting point
                //VERTEX-VERTEX-VERTEX
                if (StartVertex.Equals(EndVertex))
                {
                    MiddleType = PrimitiveType.Vertex;
                }
                    //VERTEX-EDGE-VERTEX
                else if (StartType == PrimitiveType.Vertex)
                {
                    MiddleType = PrimitiveType.Edge;
                }

                //the ending point distance should be smaller than  starting point distance 
                if (StartDist > EndDist)
                {
                    SwapEnds();
                }
            }
        }

        /**
	     * Sets an end as edge (starting point if none end were defined, ending point otherwise)
	     * 
	     * @param vertex1 one of the vertices of the intercepted edge 
	     * @param vertex2 one of the vertices of the intercepted edge
	     * @return false if all ends were already defined, true otherwise
	     */

        private void SetEdge(HeHalfedge heHalfedge)
        {
            Vector3m point0 = heHalfedge.Origin.Vector3m;
            Vector3m point1 = heHalfedge.Next.Origin.Vector3m;
            var edgeDirection = new Vector3m(point1.X - point0.X, point1.Y - point0.Y, point1.Z - point0.Z);
            var edgeLine = new IntersectionLine(ref edgeDirection, point0);

            if (_index == 0)
            {
                StartVertex = heHalfedge.Origin;
                StartType = PrimitiveType.Edge;
                StartPos = _line.ComputeLineIntersection(edgeLine);
                StartDist = _line.ComputePointToPointDistance(StartPos);
                MiddleType = PrimitiveType.Face;
                StartHalfedge = heHalfedge;
                _index++;
                return;
            }
            if (_index == 1)
            {
                EndVertex = heHalfedge.Origin;
                EndType = PrimitiveType.Edge;
                EndPos = _line.ComputeLineIntersection(edgeLine);
                EndDist = _line.ComputePointToPointDistance(EndPos);
                MiddleType = PrimitiveType.Face;
                EndHalfedge = heHalfedge;
                _index++;

                //the ending point distance should be smaller than  starting point distance 
                if (StartDist > EndDist)
                {
                    SwapEnds();
                }
            }
        }

        /**
	     * Gets the number of ends already set
	     *
	     * @return number of ends already set
	     */

        internal int GetNumEndsSet()
        {
            return _index;
        }

        /** Swaps the starting point and the ending point */

        private void SwapEnds()
        {
            var distTemp = StartDist;
            StartDist = EndDist;
            EndDist = distTemp;

            PrimitiveType typeTemp = StartType;
            StartType = EndType;
            EndType = typeTemp;

            HeVertex vertexTemp = StartVertex;
            StartVertex = EndVertex;
            EndVertex = vertexTemp;

            Vector3m posTemp = StartPos;
            StartPos = EndPos;
            EndPos = posTemp;

            HeHalfedge heTmp = StartHalfedge;
            StartHalfedge = EndHalfedge;
            EndHalfedge = heTmp;
        }

        /**
	     * Checks if two segments intersect
	     * 
	     * @param segment the other segment to check the intesection
	     * @return true if the segments intersect, false otherwise
         * Note: We use 2*Epsilon here because we want the splitedge to be long enough so that it won't get collapsed due to roundoff errors later on
	     */

        internal bool Intersect(Segment segment)
        {
            return EndDist > segment.StartDist && segment.EndDist > StartDist;
        }

        internal enum PrimitiveType
        {
            Vertex,
            Face,
            Edge
        };
    }
}
