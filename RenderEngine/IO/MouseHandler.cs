using System.Drawing;
using RenderEngine.Camera;

namespace RenderEngine.IO
{
    class MouseHandler
    {
        internal void Zoom(int delta)
        {
            if (delta > 0 && Objective.CurZoom > Objective.MinZoom)
                Objective.CurZoom += Objective.GranularityZoom;
            if (delta < 0 && Objective.CurZoom < Objective.MaxZoom)
                Objective.CurZoom -= Objective.GranularityZoom;
        }

        internal void StartRotation(Point pt)
        {
            throw new System.NotImplementedException();
        }

        internal void StopRotation(Point pt)
        {
            throw new System.NotImplementedException();
        }
    }
}
