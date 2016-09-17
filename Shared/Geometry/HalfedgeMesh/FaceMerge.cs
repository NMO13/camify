using System.Collections.Generic;

namespace Shared.Geometry.HalfedgeMesh
{
    class FaceMerge : IMeshObserver
    {
        private readonly List<HeFace> InvalidFaces = new List<HeFace>();

        private void CheckFaceValidity(HeFace face, HeMesh source)
        {
            var v0 = face.V0;
            var v1 = face.V1;
            var v2 = face.V2;

            var normal = (v1.Vector3d - v0.Vector3d).Cross(v2.Vector3d - v0.Vector3d);
            int signX = normal.X < 0 ? -1 : normal.X > 0 ? 1 : 0;
            int signY = normal.Y < 0 ? -1 : normal.Y > 0 ? 1 : 0;
            int signZ = normal.Z < 0 ? -1 : normal.Z > 0 ? 1 : 0;

            // if one of the normal's component is different in sign => vertex was rounded to a line or to the opposite side
            var normalCompare = face.OuterComponent.Normal;

            if (signX != normalCompare.X.Sign || signY != normalCompare.Y.Sign || signZ != normalCompare.Z.Sign)
            {
                InvalidFaces.Add(face);
            }
        }

        internal static void TryMerge()
        {
        }

        void IMeshObserver.VertexAdded(HeVertex v, HeMesh source)
        {
        }

        void IMeshObserver.FaceAdded(HeFace face, HeMesh source)
        {
        }

        void IMeshObserver.VertexDeleted(HeVertex v, HeMesh source)
        {
        }

        void IMeshObserver.FaceDeleted(HeFace face, HeMesh source)
        {
        }
    }
}
