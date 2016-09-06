using System.Diagnostics;
using GraphicsEngine.Geometry.CollisionCheck;
using System;
using System.Collections.Generic;
using Microsoft.SolverFoundation.Common;
using Shared.Additional;
using Shared.Geometry;

namespace GraphicsEngine.HalfedgeMesh
{
    public class HeFace : ICloneable, IIndexable
    {
        private HeHalfedge _outerComponent;
        internal AxisAlignedBoundingBox Aabb;

        public DynamicProperties DynamicProperties = new DynamicProperties();

        public HeHalfedge OuterComponent
        {
            get { return _outerComponent; }
            set
            {
                _outerComponent = value;
            }
        }

        internal HeFace()
        {
            Index = -1;
        }

        public IEnumerable<HeHalfedge> GetFaceCirculator()
        {
            if (_outerComponent == null) { yield break; }
            HeHalfedge h = _outerComponent;
            int count = 0;
            do
            {
                yield return h;
                h = h.Next;
                if (count++ > 3) { throw new InvalidOperationException("Invalid face."); }
            }
            while (h != _outerComponent);
        }

        protected bool Equals(HeFace other)
        {
            return OuterComponent.Equals(other.OuterComponent) && OuterComponent.Next.Equals(other.OuterComponent.Next) && OuterComponent.Next.Next.Equals(other.OuterComponent.Next.Next);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((HeFace)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_outerComponent != null ? _outerComponent.GetHashCode() : 0) * 397);
            }
        }

        internal void CreateBoundingBox()
        {
            Debug.Assert(OuterComponent.Origin != null && OuterComponent.Next.Origin != null &&
                OuterComponent.Next.Next.Origin != null);
            Aabb = new AxisAlignedBoundingBox(OuterComponent.Origin, OuterComponent.Next.Origin,
                OuterComponent.Next.Next.Origin);
        }


        internal static Vector3m[] CreateNormals(HeVertex v0, HeVertex v1, HeVertex v2)
        {
            Vector3m[] normals = new Vector3m[3];
            Vector3m normal = (v1.Vector3m - v0.Vector3m).Cross(v2.Vector3m - v0.Vector3m);

            normal = normal.ShortenByLargestComponent();

            if(normal.X == 0 && normal.Y == 0 && normal.Z == 0)
                throw new Exception("Normal is not valid");
            normals[0] = normal;
            normals[1] = normal.Clone() as Vector3m;
            normals[2] = normal.Clone() as Vector3m;
            return normals;
        }

        public override string ToString()
        {
            return _outerComponent.Origin.ToString() + " " + _outerComponent.Next.Origin.ToString() + " " +
                   OuterComponent.Next.Next.Origin.ToString();
        }

        public int Index { get; set; }

        public object Clone()
        {
            var heFace = new HeFace();

            var he0 = _outerComponent.Clone() as HeHalfedge;
            var he01 = OuterComponent.Twin.Clone() as HeHalfedge;
            he0.Twin = he01;
            he01.Twin = he0;

            var he1 = OuterComponent.Next.Clone() as HeHalfedge;
            var he11 = OuterComponent.Next.Twin.Clone() as HeHalfedge;
            he1.Twin = he11;
            he11.Twin = he1;

            var he2 = _outerComponent.Next.Next.Clone() as HeHalfedge;
            var he21 = _outerComponent.Next.Next.Twin.Clone() as HeHalfedge;
            he2.Twin = he21;
            he21.Twin = he2;

            he0.Next = he1;
            he0.Prev = he2;
            he1.Next = he2;
            he1.Prev = he0;
            he2.Next = he0;
            he2.Prev = he1;

            he01.Next = he11;
            he01.Prev = he21;
            he11.Next = he21;
            he11.Prev = he01;
            he21.Next = he01;
            he21.Prev = he11;

            heFace.OuterComponent = he0;
            return heFace;
        }

        internal Rational ComputeDistance(Vector3m v)
        {
            Vector3m normal = OuterComponent.Normal;
            HeVertex v0 = OuterComponent.Origin;
            var a = normal.X;
            var b = normal.Y;
            var c = normal.Z;
            var d = -(a * v0.X + b * v0.Y + c * v0.Z);

            return a * v.X + b * v.Y + c * v.Z + d;
        }

        public int DistanceSign(Vector3m v)
        {
            var distance = ComputeDistance(v);
            return distance.Sign;
        }

        public HeVertex V0
        {
            get { return H0.Origin; }
        }

        public HeVertex V1
        {
            get { return H1.Origin; }
        }

        public HeVertex V2
        {
            get { return H2.Origin; }
        }

        public HeHalfedge H0
        {
            get { return OuterComponent; }
        }

        public HeHalfedge H1
        {
            get { return OuterComponent.Next; }
        }

        public HeHalfedge H2
        {
            get { return OuterComponent.Next.Next; }
        }

        public int VboBucketId { get; set; }

        public bool IsValid
        {
            get
            {
                var crossProduct = (V0.Vector3m - V1.Vector3m).Cross(V0.Vector3m - V2.Vector3m);
                return !crossProduct.IsZero();
            }
        }
    }
}
