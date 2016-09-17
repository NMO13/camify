using System;
using System.Diagnostics;
using MessageHandling.SnapshotFormat;
using RenderEngine.GraphicObjects;
using RenderEngine.GraphicObjects.Deformable;
using RenderEngine.Rendering.Scene;
using Shared.Geometry;

namespace RenderEngine.Rendering
{
    public enum AnimationType
    {
        Interval,
        Smooth
    };

    public enum AnimationState
    {
        Stop, Pause, Play
    }

    class AnimationManager
    {
        private RenderObject _parent;
        private AnimationType _animationType;

        internal AnimationState AnimationState = AnimationState.Stop;
        internal AnimationManager()
        {
            Timer.Start();
        }
        private Stopwatch Timer = new Stopwatch();
        private int Counter = 0;

        internal void NextFrame()
        {
            if (Counter < SceneModel.Instance.CurrentCollector.Snapshots.Count)
            {
                var snapshot = SceneModel.Instance.CurrentCollector.Snapshots[Counter];
                if (Timer.ElapsedMilliseconds > snapshot.StopIntervalMillis)
                {
                    AnimateFrame(snapshot);
                }
            }
            else
            {
                SceneModel.Instance.CurrentAnimationState = AnimationState.Stop;
                SceneModel.Instance.LastAnimationState = AnimationState.Stop;
                Counter = 0;
            }
        }

        private void AnimateFrame(Snapshot snapshot)
        { 
            
            var amount = CalcTranslationAmount(snapshot);

            var mesh = SceneModel.Instance.RenderMeshes[snapshot.ToolId];
            mesh.Translate(amount);

            SceneModel.Instance.RenderMeshes[1] =
                RenderObjectFactory.CreateRenderObject(ObjectType.RenderMesh, snapshot.RoughpartSnapshot.RenderVertices,
                    snapshot.RoughpartSnapshot.Material, true) as RenderMesh;


            Timer.Restart();
            Counter++;
           
        }

        internal void Animate()
        {
            if (SceneModel.Instance.CurrentAnimationState == AnimationState.Stop)
            {
            }
            else if (SceneModel.Instance.CurrentAnimationState == AnimationState.Play)
            {
                // if we just started the animation
                if (SceneModel.Instance.LastAnimationState == AnimationState.Stop)
                {
                    SceneModel.Instance.LastAnimationState = AnimationState.Play;
                    Timer.Restart();
                }
                NextFrame();
            }
        }

        private Vector3d CalcTranslationAmount(Snapshot snapshot)
        {
            if (_animationType == AnimationType.Interval)
            {
                return snapshot.Path;
            }
            else // Smooth
            {
                throw new NotSupportedException();
            }
        }
    }
}
