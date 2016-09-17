using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.SolverFoundation.Common;

namespace Shared.Geometry.HalfedgeMesh
{
    public class IncidentEdgesList : IEnumerable<HeHalfedge>
    {
        internal IncidentEdgesList(HeVertex owner)
        {
            _owner = owner;
        }
        private List<HeHalfedge> _incidentEdges = new List<HeHalfedge>();

        private Dictionary<HeHalfedge, List<HeHalfedge>>  _equalIncidentEdgeList =
            new Dictionary<HeHalfedge, List<HeHalfedge>> ();
        private HeVertex _owner;

        internal List<HeHalfedge> EqualEdges(HeHalfedge halfedge)
        {
            List<HeHalfedge> list = null;
            _equalIncidentEdgeList.TryGetValue(halfedge, out list);
            return list;
        }

        public int Count { get { return _incidentEdges.Count; } }
        internal void Add(HeHalfedge edge)
        {
            if (!edge.Origin.Equals(_owner))
                throw new Exception();

            if (_incidentEdges.Find(x => x.Equals(edge)) != null)
            {
                AddToEqualList(edge);
            }
            _incidentEdges.Add(edge);
        }

        private void AddToEqualList(HeHalfedge edge)
        {
            List<HeHalfedge> list = EqualEdges(edge);
            if (list == null)
            {
                list = new List<HeHalfedge>();
                var edge2 = _incidentEdges.Find(x => x.Equals(edge));
                list.Add(edge);
                list.Add(edge2);
                _equalIncidentEdgeList.Add(edge, list);
            }
            else
            {
                list.Add(edge);
            }
        }

        internal void Remove(HeHalfedge edge)
        {
            Debug.Assert(edge.Index > -1);
            var count = _incidentEdges.Count;
            _incidentEdges.RemoveAll(x => x.Index == edge.Index);
            Debug.Assert(count - 1 == edge.Origin.IncidentEdges.Count);

            var equalEdgeList = EqualEdges(edge);
            if (equalEdgeList != null)
            {
                count = equalEdgeList.RemoveAll(x => x.Index == edge.Index);
                Debug.Assert(count == 1);
                if (equalEdgeList.Count == 0)
                    _equalIncidentEdgeList[edge] = null;
            }
        }

        public IEnumerator<HeHalfedge> GetEnumerator()
        {
            return _incidentEdges.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        internal HeHalfedge this[int i]
        {
            get { return _incidentEdges[i]; }
        }

        public void ForEach(Action<HeHalfedge> action)
        {
            foreach (var incidentEdge in _incidentEdges)
            {
                action(incidentEdge);
            }
        }
    }

    public class HeVertex : IIndexable
    {
        private float _xd;
        private float _yd;
        private float _zd;

        private Rational _x;
        private Rational _y;
        private Rational _z;

        private Vector3m vector3m;


        public IncidentEdgesList IncidentEdges;
        public bool IsOnSweptVolumeSurface;

        public int Index { get; set; }

        public HeVertex(Rational x, Rational y, Rational z)
        {
            X = x;
            Y = y;
            Z = z;
            Index = -1;
            vector3m = new Vector3m(x, y, z);
            IncidentEdges = new IncidentEdgesList(this);
        }

        public Vector3m Vector3m
        {
            get { return vector3m; }
        }

        public Vector3d Vector3d
        {
            get { return new Vector3d(XD, YD, ZD); }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((HeVertex)obj);
        }

        protected bool Equals(HeVertex other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return "Vertex:" + " " + X.ToDouble() + " " + Y.ToDouble() + " " + Z.ToDouble() + " ";
        }

        internal float XD
        {
            get { return _xd; }
            set
            {
                _x = value;
                _xd = value;
                vector3m = new Vector3m(_x, _y, _z);
            }
        }

        internal float YD
        {
            get { return _yd; }
            set
            {
                _y = value;
                _yd = value;
                vector3m = new Vector3m(_x, _y, _z);
            }
        }

        internal float ZD
        {
            get { return _zd; }
            set
            {
                _z = value;
                _zd = value;
                vector3m = new Vector3m(_x, _y, _z);
            }
        }

        public Rational X
        {
            get { return _x; }
            set
            {
                _x = value;
                _xd = (float) value.ToDouble();
                vector3m = new Vector3m(_x, _y, _z);
            }
        }

        public Rational Y
        {
            get { return _y; }
            set
            {
                _y = value;
                _yd = (float)value.ToDouble();
                vector3m = new Vector3m(_x, _y, _z);
            }
        }

        public Rational Z
        {
            get { return _z; }
            set
            {
                _z = value;
                _zd = (float)value.ToDouble();
                vector3m = new Vector3m(_x, _y, _z);
            }
        }
    }
}
