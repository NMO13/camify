using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GraphicsEngine.Additional;
using GraphicsEngine.Geometry;
using GraphicsEngine.Geometry.Triangulation;
using GraphicsEngine.HalfedgeMeshProcessing;
using GraphicsEngine.Math;
using Microsoft.SolverFoundation.Common;
using Shared;

namespace GraphicsEngine.HalfedgeMesh.Simplification
{
    class PlanarMerge : IPostProcess
    {
        internal void Triangulate(List<ProjectedContourGroup> projectedContourGroups)
        {
            foreach (var projectedContourGroup in projectedContourGroups)
            {
                if (projectedContourGroup.ProjectedContours.Count != 1)
                    throw new Exception("Projected contour is not valid");

                var holes = new List<List<Vector3m>>();
                projectedContourGroup.Holes.ForEach(x => holes.Add(x.ProjectedVertices));
                EarClipping earClipping = new EarClipping();
                earClipping.SetPoints(projectedContourGroup.ProjectedContours[0].ProjectedVertices, holes, projectedContourGroup.OriginalContourGroup.Normal);
                earClipping.Triangulate();
                var result = earClipping.Result;
                result.ForEach(x => projectedContourGroup.NewFaceIndices.Add((int) x.DynamicProperties.GetValue(PropertyConstants.HeVertexIndex)));
            }

        }

        private bool ClassifyAsHole(ProjectedContour contour, ProjectedContourGroup contourHolder, out Vector3m he0, out Vector3m he1, out Vector3m he2)
        {
            var index = FindConvexEdge(contour);
            he1 = contour.ProjectedVertices[index];
            if (index == 0)
                he0 = contour.ProjectedVertices[contour.ProjectedVertices.Count - 1];
            else
                he0 = contour.ProjectedVertices[index - 1];

            if (index == contour.ProjectedVertices.Count - 1)
                he2 = contour.ProjectedVertices[0];
            else
                he2 = contour.ProjectedVertices[index + 1];

            int orientation = Misc.GetOrientation(he0, he1, he2, contourHolder.OriginalContourGroup.Normal);
            Debug.Assert(orientation != 0);
            if (orientation == 1)
            {
                return false;
            }
            else
            {
                contourHolder.Holes.Add(contour);
                return true;
            }

        }

        private int FindConvexEdge(ProjectedContour contour)
        {
            Vector3m minimum = new Vector3m(double.MaxValue, double.MaxValue, double.MaxValue);
            int index = -1;
            int i = 0;
            foreach (var cur in contour.ProjectedVertices)
            {
                if (cur.X < minimum.X || cur.X == minimum.X && cur.Y < minimum.Y)
                {
                    minimum = cur;
                    index = i;
                }
                i++;
            }
            return index;
        }

        internal void ReplaceFaces(List<ProjectedContourGroup> projectedContourGroups, HeMesh mesh, ContourGroupManager manager)
        {
            foreach (var projectedContourGroup in projectedContourGroups)
            {
                // We don't have to update the contour edges since the halfedges' twin of the contour edges should never be deleted and so
                // the halfedge itself should also exist. Therefore, the contour edges should still be valid after ReplaceFaces

                var tempFaceArray = projectedContourGroup.OriginalContourGroup.InsideFaces.ToArray();
                foreach (var insideFace in tempFaceArray)
                {
                    Debug.Assert(insideFace.Index >= 0);
                    var res = manager.RemoveFace(insideFace);
                    Debug.Assert(res >= 0);
                    mesh.RemoveFace(insideFace, false);
                }
                Debug.Assert(projectedContourGroup.OriginalContourGroup.InsideFaces.Count == 0);
                Debug.Assert(projectedContourGroup.NewFaceIndices.Count % 3 == 0);
                for (int i = 0; i < projectedContourGroup.NewFaceIndices.Count; i += 3)
                {
                    var face = mesh.AddFace(projectedContourGroup.NewFaceIndices[i], projectedContourGroup.NewFaceIndices[i + 1],
                    projectedContourGroup.NewFaceIndices[i + 2]);
                    manager.AddFace(projectedContourGroup.OriginalContourGroup.Index, face);
                }
            }
        }

        internal List<ProjectedContourGroup> ProjectContours(List<ContourGroup> contourGroups)
        {
            List<ProjectedContourGroup> projectedContours = new List<ProjectedContourGroup>();
            foreach (var contourGroup in contourGroups)
            {
                if (contourGroup.Contours.Count == 1 && contourGroup.InsideFaces.Count == 1)
                    continue;
                ProjectedContourGroup projectedContourGroup = new ProjectedContourGroup(contourGroup);
                projectedContours.Add(projectedContourGroup);
                var matrix = Matrix4<Rational>.CreateInverseRotationMatrix(contourGroup.Normal);

                foreach (var contour in contourGroup.Contours)
                {
                    ProjectedContour projContour = new ProjectedContour();
                    projectedContourGroup.ProjectedContours.Add(projContour);
                    foreach (var heHalfedge in contour.HeList)
                    {
                        var projectedVertex = Matrix4<Rational>.Multiply(matrix, heHalfedge.Origin.Vector3m);
                        var point = new Vector3m(heHalfedge.Origin.Vector3m); //new Vector2m(projectedVertex.X, projectedVertex.Z);
                        point.DynamicProperties.AddProperty(PropertyConstants.HeVertexIndex, heHalfedge.Origin.Index);
                        projContour.ProjectedVertices.Add(point);
                    }
                }
            }
            return projectedContours;
        }

        internal void FindHoles(List<ProjectedContourGroup> projectedContours)
        {
            foreach (var group in projectedContours)
            {
                for (int c = 0; c < group.ProjectedContours.Count; c++)
                {
                    Vector3m pV0, pV1, pV2;
                    var contour = group.ProjectedContours[c];
                    if (ClassifyAsHole(contour, group, out pV0, out pV1, out pV2))
                    {
                        group.ProjectedContours.RemoveAt(c);
                        c--;
                    }
                }
                Debug.Assert(group.ProjectedContours.Count == 1); // we want exactly one "main" contour, all others are holes
            }
        }

        public void Execute(DeformableObject obj)
        {
            if (obj.ContourGroupManager.ContourGroups.Count == 0)
                return;
            var projectedContours = ProjectContours(obj.ContourGroupManager.ContourGroups);
            if (projectedContours.Count == 0)
                return;
            FindHoles(projectedContours);
            Triangulate(projectedContours);
            ReplaceFaces(projectedContours, obj.HeMesh, obj.ContourGroupManager);
            obj.HeMesh.RemoveOrphanVertices();
        }

        public void FaceAdded(HeFace face, HeMesh source)
        {
        }

        public void FaceDeleted(HeFace face, HeMesh source)
        {
        }

        public void VertexAdded(HeVertex v, HeMesh source)
        {
        }

        public void VertexDeleted(HeVertex v, HeMesh source)
        {
        }
    }

    class ProjectedContourGroup
    {
        internal List<ProjectedContour> Holes = new List<ProjectedContour>();
        internal List<ProjectedContour> ProjectedContours = new List<ProjectedContour>();
        internal List<int> NewFaceIndices = new List<int>();
        internal ContourGroup OriginalContourGroup { get; private set; }

        public ProjectedContourGroup(ContourGroup contourGroup)
        {
            OriginalContourGroup = contourGroup;
        }
    }

    class ProjectedContour
    {
        internal List<Vector3m> ProjectedVertices = new List<Vector3m>(); 
    }

    //TODO put that into a separate class



    struct Matrix4<T>
    {
        public Matrix4(T m11, T m12, T m13, T m14, T m21, T m22, T m23, T m24, T m31,
                      T m32, T m33, T m34, T m41, T m42, T m43, T m44)
        {
            M11 = m11;
            M12 = m12;
            M13 = m13;
            M14 = m14;
            M21 = m21;
            M22 = m22;
            M23 = m23;
            M24 = m24;
            M31 = m31;
            M32 = m32;
            M33 = m33;
            M34 = m34;
            M41 = m41;
            M42 = m42;
            M43 = m43;
            M44 = m44;
        }

        public static Vector3m Multiply(Matrix4<Rational> m, Vector3m vec4)
        {
            Vector3m res = Vector3m.Zero();
            res.X = m.M11 * vec4.X + m.M12 * vec4.Y + m.M13 * vec4.Z + m.M14;
            res.Y = m.M21 * vec4.X + m.M22 * vec4.Y + m.M23 * vec4.Z + m.M24;
            res.Z = m.M31 * vec4.X + m.M32 * vec4.Y + m.M33 * vec4.Z + m.M34;
            var w = m.M41*vec4.X + m.M42*vec4.Y + m.M43*vec4.Z + m.M44;
            return res;
        }

        public override string ToString()
        {
            return "{" + String.Format("M11:{0} M12:{1} M13:{2} M14:{3}", M11, M12, M13, M14) + "}"
                + " {" + String.Format("M21:{0} M22:{1} M23:{2} M24:{3}", M21, M22, M23, M24) + "}"
                + " {" + String.Format("M31:{0} M32:{1} M33:{2} M34:{3}", M31, M32, M33, M34) + "}"
                + " {" + String.Format("M41:{0} M42:{1} M43:{2} M44:{3}", M41, M42, M43, M44) + "}";
        }

        public static object Multiply(Matrix4<Rational> matrix1, Matrix4<Rational> matrix2)
        {
            var m11 = (((matrix1.M11 * matrix2.M11) + (matrix1.M12 * matrix2.M21)) + (matrix1.M13 * matrix2.M31)) + (matrix1.M14 * matrix2.M41);
            var m12 = (((matrix1.M11 * matrix2.M12) + (matrix1.M12 * matrix2.M22)) + (matrix1.M13 * matrix2.M32)) + (matrix1.M14 * matrix2.M42);
            var m13 = (((matrix1.M11 * matrix2.M13) + (matrix1.M12 * matrix2.M23)) + (matrix1.M13 * matrix2.M33)) + (matrix1.M14 * matrix2.M43);
            var m14 = (((matrix1.M11 * matrix2.M14) + (matrix1.M12 * matrix2.M24)) + (matrix1.M13 * matrix2.M34)) + (matrix1.M14 * matrix2.M44);
            var m21 = (((matrix1.M21 * matrix2.M11) + (matrix1.M22 * matrix2.M21)) + (matrix1.M23 * matrix2.M31)) + (matrix1.M24 * matrix2.M41);
            var m22 = (((matrix1.M21 * matrix2.M12) + (matrix1.M22 * matrix2.M22)) + (matrix1.M23 * matrix2.M32)) + (matrix1.M24 * matrix2.M42);
            var m23 = (((matrix1.M21 * matrix2.M13) + (matrix1.M22 * matrix2.M23)) + (matrix1.M23 * matrix2.M33)) + (matrix1.M24 * matrix2.M43);
            var m24 = (((matrix1.M21 * matrix2.M14) + (matrix1.M22 * matrix2.M24)) + (matrix1.M23 * matrix2.M34)) + (matrix1.M24 * matrix2.M44);
            var m31 = (((matrix1.M31 * matrix2.M11) + (matrix1.M32 * matrix2.M21)) + (matrix1.M33 * matrix2.M31)) + (matrix1.M34 * matrix2.M41);
            var m32 = (((matrix1.M31 * matrix2.M12) + (matrix1.M32 * matrix2.M22)) + (matrix1.M33 * matrix2.M32)) + (matrix1.M34 * matrix2.M42);
            var m33 = (((matrix1.M31 * matrix2.M13) + (matrix1.M32 * matrix2.M23)) + (matrix1.M33 * matrix2.M33)) + (matrix1.M34 * matrix2.M43);
            var m34 = (((matrix1.M31 * matrix2.M14) + (matrix1.M32 * matrix2.M24)) + (matrix1.M33 * matrix2.M34)) + (matrix1.M34 * matrix2.M44);
            var m41 = (((matrix1.M41 * matrix2.M11) + (matrix1.M42 * matrix2.M21)) + (matrix1.M43 * matrix2.M31)) + (matrix1.M44 * matrix2.M41);
            var m42 = (((matrix1.M41 * matrix2.M12) + (matrix1.M42 * matrix2.M22)) + (matrix1.M43 * matrix2.M32)) + (matrix1.M44 * matrix2.M42);
            var m43 = (((matrix1.M41 * matrix2.M13) + (matrix1.M42 * matrix2.M23)) + (matrix1.M43 * matrix2.M33)) + (matrix1.M44 * matrix2.M43);
            var m44 = (((matrix1.M41 * matrix2.M14) + (matrix1.M42 * matrix2.M24)) + (matrix1.M43 * matrix2.M34)) + (matrix1.M44 * matrix2.M44);

            return new Matrix4<Rational>(m11, m12, m13, m14, m21, m22, m23, m24, m31, m32, m33, m34, m41, m42, m43, m44);
        }

        internal static Matrix4<Rational> CreateInverseRotationMatrix(Vector3m up)
        {
            Vector3m right;

            //up = up.DividedBy(up.Length());

            if (up.X.AbsoluteValue > up.Z.AbsoluteValue)
                right = up.Cross(new Vector3m(0, 0, 1));
            else
                right = up.Cross(new Vector3m(1, 0, 0));
            //right = right.DividedBy(right.Length());
            Debug.Assert(right.X.AbsoluteValue <= 1 && right.Y.AbsoluteValue <= 1 && right.Z.AbsoluteValue <= 1);
            var backward = right.Cross(up);
            Matrix4<Rational> m = new Matrix4<Rational>(right.X, right.Y, right.Z, 0, up.X, up.Y, up.Z, 0, backward.X, backward.Y, backward.Z, 0, 0, 0, 0, 1);
            return m;
        }

        internal T M11;
        internal T M12;
        internal T M13;
        internal T M14;
        internal T M21;
        internal T M22;
        internal T M23;
        internal T M24;
        internal T M31;
        internal T M32;
        internal T M33;
        internal T M34;
        internal T M41;
        internal T M42;
        internal T M43;
        internal T M44;
    }
}
