namespace Shared.Geometry.HalfedgeMesh
{
    public interface IMeshObserver
    {

        void VertexAdded(HeVertex v, HeMesh source);

        void FaceAdded(HeFace face, HeMesh source);

        void VertexDeleted(HeVertex v, HeMesh source);

        void FaceDeleted(HeFace face, HeMesh source);
    }
}
