using System;
using System.Collections.Generic;
using Shared;
using Shared.Geometry;

namespace GraphicsEngine.HalfedgeMesh
{ 
    public class HeHalfedge : ICloneable, IIndexable
    {
        public HeHalfedge Next;
        public HeHalfedge Prev;
        public HeVertex Origin { get; private set; }
        public HeFace IncidentFace;
        public HeHalfedge Twin;

        public Vector3m Normal { get; set; }
        public int Index { get; set; }

        public bool IsSplitLine { get; private set; }

        internal HeHalfedge(HeVertex origin)
        {
            Origin = origin;
            IsSplitLine = false;
            Index = -1;
        }

        public IEnumerable<HeHalfedge> GetHalfedgeCirculator()
        {
            HeHalfedge h = this;
            int count = 0;
            do
            {
                yield return h;
                h = h.Twin.Next;
                if (count++ > 999) { throw new InvalidOperationException("Invalid Halfedge."); }
            }
            while (h != this);
        }

        protected bool Equals(HeHalfedge other)
        {
            return Twin.Origin.Equals(other.Twin.Origin) && Origin.Equals(other.Origin);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((HeHalfedge)obj);
        }

        public override string ToString()
        {
            return "Half-Edge: " + "v0: " + Origin + " t.v0: " + Twin.Origin + " ";
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Twin.Origin.GetHashCode() * 397) ^ Origin.GetHashCode();
            }
        }

        public object Clone()
        {
            var halfedge = new HeHalfedge(new HeVertex(Origin.X, Origin.Y, Origin.Z));
            halfedge.Twin = new HeHalfedge(new HeVertex(Twin.Origin.X, Twin.Origin.Y, Twin.Origin.Z));
            halfedge.Normal = Normal.Clone() as Vector3m;
            return halfedge;
        }

        public Vector3m Vector3D
        {
            get { return new Vector3m(Twin.Origin.X - this.Origin.X, Twin.Origin.Y - this.Origin.Y, Twin.Origin.Z - this.Origin.Z); }
        }

        public void SetSplitline(bool value)
        {
            IsSplitLine = value;
        }
    }
}
