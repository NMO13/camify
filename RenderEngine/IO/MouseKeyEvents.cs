using System.Drawing;
using RenderEngine.Rendering.Scene;

namespace RenderEngine.IO
{
    class MouseKeyEvents
    {
        private readonly MouseHandler _mouseHandler;

        internal MouseKeyEvents(SceneManager manager)
        {
            _mouseHandler = new MouseHandler(manager);
        }

        public void MouseMove(object sender, Point pt)
        {
            _mouseHandler.RotateSceen(pt);
        }

        public void LeftMouseButtonDown(object sender, Point pt)
        {
        }

        public void LeftMouseButtonUp(object sender, Point pt)
        {
        }

        public void RightMouseButtonDown(object sender, Point pt)
        {
        }

        public void RightMouseButtonUp(object sender, Point pt)
        {
        }

        public void MiddleMouseButtonDown(object sender, Point pt)
        {
            _mouseHandler.StartRotation(pt);
        }

        public void MiddleMouseButtonUp(object sender, Point pt)
        {
            _mouseHandler.StopRotation(pt);
        }

        public void MouseWheel(object sender, int delta)
        {
            _mouseHandler.Zoom(delta);
        }
    }
}
