using System;
using Microsoft.SolverFoundation.Common;
using Shared.Geometry.HalfedgeMesh;

namespace Shared.Geometry.CollisionCheck
{
    public class AxisAlignedBoundingBox : IBoundingBox
    {
        /** maximum from the x coordinate */
        internal Rational XMax;
        /** minimum from the x coordinate */
        internal Rational XMin;
        /** maximum from the y coordinate */
        internal Rational YMax;
        /** minimum from the y coordinate */
        internal Rational YMin;
        /** maximum from the z coordinate */
        internal Rational ZMax;
        /** minimum from the z coordinate */
        internal Rational ZMin;

        /** tolerance value to test equalities */
        private const double Tol = 1e-10f;

        internal AxisAlignedBoundingBox(HeVertex p1, HeVertex p2, HeVertex p3)
        {
            XMax = XMin = p1.X;
            YMax = YMin = p1.Y;
            ZMax = ZMin = p1.Z;

            CheckVertex(p2);
            CheckVertex(p3);
        }

        private AxisAlignedBoundingBox()
        {
            
        }

        public bool Overlap(IBoundingBox other)
        {
            AxisAlignedBoundingBox box = other as AxisAlignedBoundingBox;
            if(box == null)
                throw new ArgumentException();
            if ((XMin > box.XMax) || (XMax < box.XMin) || (YMin > box.YMax) || (YMax < box.YMin) || (ZMin > box.ZMax) || (ZMax < box.ZMin))
            {
                return false;
            }
            return true;
        }

        public AxisAlignedBoundingBox Grow(AxisAlignedBoundingBox aabr)
        {
            if (aabr.XMin < XMin) XMin = aabr.XMin;
            if (aabr.YMin < YMin) YMin = aabr.YMin;
            if (aabr.ZMin < ZMin) ZMin = aabr.ZMin;
            if (aabr.XMax > XMax) XMax = aabr.XMax;
            if (aabr.YMax > YMax) YMax = aabr.YMax;
            if (aabr.ZMax > ZMax) ZMax = aabr.ZMax;
            return this;
        }

        private void CheckVertex(HeVertex vertex)
        {
            if (vertex.X > XMax)
            {
                XMax = vertex.X;
            }
            else if (vertex.X < XMin)
            {
                XMin = vertex.X;
            }

            if (vertex.Y > YMax)
            {
                YMax = vertex.Y;
            }
            else if (vertex.Y < YMin)
            {
                YMin = vertex.Y;
            }

            if (vertex.Z > ZMax)
            {
                ZMax = vertex.Z;
            }
            else if (vertex.Z < ZMin)
            {
                ZMin = vertex.Z;
            }
        }

        public static AxisAlignedBoundingBox Init()
        {
            AxisAlignedBoundingBox aabr = new AxisAlignedBoundingBox();
            aabr.XMin = aabr.YMin = aabr.ZMin = Double.MaxValue;
            aabr.XMax = aabr.YMax = aabr.ZMax = Double.MinValue;
            return aabr;
        }
    }
}
