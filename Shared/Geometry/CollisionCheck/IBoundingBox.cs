namespace Shared.Geometry.CollisionCheck
{
    public interface IBoundingBox
    {
        bool Overlap(IBoundingBox other);
    }
}
