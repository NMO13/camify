using System.Drawing;
using RenderEngine.Camera;
using RenderEngine.Rendering.Scene;

namespace RenderEngine.IO
{
    class MouseHandler
    {
        private readonly SceneManager _manager;
        internal MouseHandler(SceneManager manager)
        {
            _manager = manager;
        }   

        internal void Zoom(int delta)
        {
            if (delta > 0 && Objective.CurZoom > Objective.MinZoom)
                Objective.CurZoom += Objective.GranularityZoom;
            if (delta < 0 && Objective.CurZoom < Objective.MaxZoom)
                Objective.CurZoom -= Objective.GranularityZoom;
        }

        internal void StartRotation(Point pt)
        {
            _manager.WorldRotator.StartDrag(pt);
        }

        internal void StopRotation(Point pt)
        {
            _manager.WorldRotator.StopDrag();
        }

        internal void RotateSceen(Point pt)
        {
            _manager.WorldRotator.Drag(pt);
        }
    }
}
