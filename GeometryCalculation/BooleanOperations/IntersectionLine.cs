using System;
using GraphicsEngine.HalfedgeMesh;
using Microsoft.SolverFoundation.Common;
using Shared.Geometry;

namespace GraphicsEngine.Geometry.Boolean_Ops
{
    class IntersectionLine
    {
        private readonly Vector3m _point;
        private readonly Vector3m _direction;

        internal IntersectionLine(HeFace faceA, HeFace faceB)
        {
            Vector3m normalFaceA = faceA.OuterComponent.Normal;
            Vector3m normalFaceB = faceB.OuterComponent.Normal;
            var direction = normalFaceA.Cross(normalFaceB);

            //if _direction length is not zero (the planes aren't parallel )...
            if (direction.LengthSquared().Sign == 1)
            {
                //getting a line _point, zero is set to a coordinate whose _direction 
                //component isn't zero (line intersecting its origin plan)
                var faceA_X = faceA.OuterComponent.Origin.X;
                var faceA_Y = faceA.OuterComponent.Origin.Y;
                var faceA_Z = faceA.OuterComponent.Origin.Z;

                var faceB_X = faceB.OuterComponent.Origin.X;
                var faceB_Y = faceB.OuterComponent.Origin.Y;
                var faceB_Z = faceB.OuterComponent.Origin.Z;

                var d1 = -(normalFaceA.X * faceA_X + normalFaceA.Y * faceA_Y + normalFaceA.Z * faceA_Z);
                var d2 = -(normalFaceB.X * faceB_X + normalFaceB.Y * faceB_Y + normalFaceB.Z * faceB_Z);
                _point = Vector3m.Zero();

                if (direction.X.Sign != 0)
                {
                    _point.X = 0;
                    _point.Y = (d2*normalFaceA.Z - d1*normalFaceB.Z)/direction.X;
                    _point.Z = (d1*normalFaceB.Y - d2*normalFaceA.Y)/direction.X;
                }
                else if (direction.Y.Sign != 0)
                {
                    _point.X = (d1*normalFaceB.Z - d2*normalFaceA.Z)/direction.Y;
                    _point.Y = 0;
                    _point.Z = (d2*normalFaceA.X - d1*normalFaceB.X)/direction.Y;
                }
                else if (direction.Z.Sign != 0)
                {
                    _point.X = (d2*normalFaceA.Y - d1*normalFaceB.Y)/direction.Z;
                    _point.Y = (d1*normalFaceB.X - d2*normalFaceA.X)/direction.Z;
                    _point.Z = 0;
                }
                else
                {
                    throw new Exception("Illegal splitline");
                }

            }
            _direction = direction;
        }

        /**
	     * Constructor for a ray
	     * 
	     * @param _direction _direction ray
	     * @param _point beginning of the ray
	     */
	    internal IntersectionLine(ref Vector3m direction, Vector3m point)
	    {
	        this._direction = direction;
	        this._point = point;
	    }

        internal Rational ComputePointToPointDistance(Vector3m otherPoint)
        {
            var distance = otherPoint.DistanceSquared(_point);
            var vec = new Vector3m(otherPoint.X - _point.X, otherPoint.Y - _point.Y, otherPoint.Z - _point.Z);
            return (vec.Dot(_direction).Sign == -1) ? -distance : distance;
        }

        /**
	     * Computes the _point resulting from the intersection with another line
	     * 
	     * @param otherLine the other line to apply the intersection. The lines are supposed
	     * to intersect
	     * @return _point resulting from the intersection. If the _point couldn't be obtained, return null
	     */
        public Vector3m ComputeLineIntersection(IntersectionLine otherLine)
        {
            //x = x1 + a1*t = x2 + b1*s
            //y = y1 + a2*t = y2 + b2*s
            //z = z1 + a3*t = z2 + b3*s

            Vector3m linePoint = otherLine._point;
            Vector3m lineDirection = otherLine._direction;

            Rational t;
            if ((_direction.Y * lineDirection.X - _direction.X * lineDirection.Y).AbsoluteValue.Sign == 1)
            {
                t = (-_point.Y * lineDirection.X + linePoint.Y * lineDirection.X + lineDirection.Y * _point.X - lineDirection.Y * linePoint.X) / (_direction.Y * lineDirection.X - _direction.X * lineDirection.Y);
            }
            else if ((-_direction.X * lineDirection.Z + _direction.Z * lineDirection.X).AbsoluteValue.Sign == 1)
            {
                t = -(-lineDirection.Z * _point.X + lineDirection.Z * linePoint.X + lineDirection.X * _point.Z - lineDirection.X * linePoint.Z) / (-_direction.X * lineDirection.Z + _direction.Z * lineDirection.X);
            }
            else if ((-_direction.Z*lineDirection.Y + _direction.Y*lineDirection.Z).AbsoluteValue.Sign == 1)
            {
                t = (_point.Z*lineDirection.Y - linePoint.Z*lineDirection.Y - lineDirection.Z*_point.Y +
                     lineDirection.Z*linePoint.Y)/(-_direction.Z*lineDirection.Y + _direction.Y*lineDirection.Z);
            }
            else
            {
                throw new Exception("Intersection line not correctly calculated.");
            }

            var x = _point.X + _direction.X * t;
            var y = _point.Y + _direction.Y* t;
            var z = _point.Z + _direction.Z * t;

            return new Vector3m(x, y, z);
        }
    }
}
