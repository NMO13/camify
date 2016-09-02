using Shared.Geometry;
using Shared.Helper;
using Vector3d = Shared.Geometry.Vector3d;

namespace RenderEngine.Camera
{
    internal class Objective
    {
        internal static readonly Vector3d CameraPos = new Vector3d(0f, 0f, 0);
        internal static readonly Vector3d CameraFront = new Vector3d(0f, 0.0f, -1f);
        internal static readonly Vector3d CameraUp = new Vector3d(0.0f, 1.0f, 0.0f);

        internal static float CurZoom = Config.InitialZoom;
        internal static float MinZoom = Config.MinZoom;
        internal static float MaxZoom = Config.MaxZoom;
        internal static float GranularityZoom = Config.GranularityZoom;

        internal static Matrix4d InitialPitch
        {
            get
            {  
                Matrix4d left = Matrix4d.CreateRotationZ(MathHelper.DegreesToRadians(Config.PitchZ));
                Matrix4d right = Matrix4d.CreateRotationY(MathHelper.DegreesToRadians(Config.PitchY));
                Matrix4d rotationMatrix = Matrix4d.Mult(left, right);
                return rotationMatrix;
            }   
        }
    }
}
