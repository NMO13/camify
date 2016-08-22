using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SolverFoundation.Common;
using OpenTK.Graphics.OpenGL;

namespace GraphicsEngine.HalfedgeMesh.Simplification
{
    class MelaxSimplification : ISimplificationStrategy
    {
        private float _minLength;
        private HeMesh _mesh;

        public MelaxSimplification(float minLength, HeMesh mesh)
        {
            this._minLength = minLength;
            _mesh = mesh;
        }

        private Dictionary<HeVertex, X> vertexCandidates = new Dictionary<HeVertex, X>();
        private void SearchCandidates()
        {
            foreach (HeFace f in _mesh.FaceList)
            {
                foreach (var heHalfedge in f.GetFaceCirculator())
                {
                    if (!vertexCandidates.ContainsKey(heHalfedge.Origin))
                    {
                        ComputeEdgeCostAtVertex(heHalfedge.Origin);                        
                    }
                }
            }
        }

        public void Apply()
        {
            SearchCandidates();
            var list = vertexCandidates.ToList();
            for(int i = 0; i < list.Count; i++)
            {
                if (list[i].Value.ObjectDistance < _minLength)
                {
                    var edge = list[i].Value.CollapseTo;
                    List<HeVertex> neighbours = new List<HeVertex>();
                    edge.Origin.IncidentEdges.ForEach(x => neighbours.Add(x.Twin.Origin));
                    _mesh.Collapse(edge.Index);
                    neighbours.ForEach(ComputeEdgeCostAtVertex);
                    list.RemoveAt(i);
                    i = 0; // we have to start again in case that neighbours are now shorter than before due to updating
                }
            }
        }

        

        void ComputeEdgeCostAtVertex(HeVertex v)
        {
        //    // compute the edge collapse cost for all edges that start
        //    // from vertex v.  Since we are only interested in reducing
        //    // the object by selecting the min cost edge at each step, we
        //    // only cache the cost of the least cost edge at this vertex
        //    // (in member variable collapse) as well as the value of the 
        //    // cost (in member variable objdist).
        //    Debug.Assert(v.IncidentEdges.Count != 0);

        //    X x;
        //    vertexCandidates.TryGetValue(v, out x);
        //    if (x == null)
        //    {
        //        x = new X();
        //        vertexCandidates.Add(v, x);
        //    }
        //    // search all neighboring edges for "least cost" edge
        //    for (int i = 0; i < v.IncidentEdges.Count; i++)
        //    {
        //        float dist = (float) ComputeEdgeCollapseCost(v.IncidentEdges[i]).ToDouble();
        //        if (dist < x.ObjectDistance)
        //        {
        //            x.CollapseTo = v.IncidentEdges[i];  // candidate for edge collapse
        //            x.ObjectDistance = dist; // cost of the collapse
        //        }
        //    }
        }

        //Rational ComputeEdgeCollapseCost(HeHalfedge edge)
        //{
        //    // if we collapse edge uv by moving u to v then how 
        //    // much different will the model change, i.e. how much "error".
        //    // Texture, vertex normal, and border vertex code was removed
        //    // to keep this demo as simple as possible.
        //    // The method of determining cost was designed in order 
        //    // to exploit small and coplanar regions for
        //    // effective polygon reduction.
        //    // Is is possible to add some checks here to see if "folds"
        //    // would be generated.  i.e. normal of a remaining face gets
        //    // flipped.  I never seemed to run into this problem and
        //    // therefore never added code to detect this case.
        //    var edgelength = (edge.Twin.Origin.Vector3m - edge.Origin.Vector3m).Length();
        //    float curvature = 0;

        //    // use the triangle facing most away from the sides 
        //    // to determine our curvature term
        //    for (int i = 0; i < edge.Origin.IncidentEdges.Count; i++)
        //    {
        //        float mincurv = 1; // curve for face i and closer side to it
        //        var f0 = edge.IncidentFace;
        //        var f1 = edge.Twin.IncidentFace;

        //        float dotprod = (float) edge.Origin.IncidentEdges[i].Normal.Unit().Dot(edge.Normal.Unit()).ToDouble();
        //        float term = (1 - dotprod)/2;
        //        mincurv = mincurv < term ? mincurv : term;

        //        dotprod = (float) edge.Origin.IncidentEdges[i].Normal.Unit().Dot(edge.Twin.Normal.Unit()).ToDouble();
        //        term = (1 - dotprod) / 2;
        //        mincurv = mincurv < term ? mincurv : term;

        //        curvature = curvature > mincurv ? curvature : mincurv;
        //    }
        //    // the more coplanar the lower the curvature term   
        //    return edgelength * curvature;
        //}
    }

    class X
    {
        internal float ObjectDistance = float.MaxValue;
        internal HeHalfedge CollapseTo = null;
    }
}
