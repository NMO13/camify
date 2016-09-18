using System;
using System.Diagnostics;
using MessageHandling.SnapshotFormat;
using RenderEngine.GraphicObjects;
using RenderEngine.GraphicObjects.ObjectTypes;
using RenderEngine.GraphicObjects.ObjectTypes.Dynamic;
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

            var mesh = SceneModel.Instance.DynamicRenderObjects[snapshot.ToolId];
            mesh.Translate(amount);

            var container = new DynamicObjectDataContainer
            {
                Vertices = snapshot.RoughpartSnapshot.RenderVertices,
                Material = snapshot.RoughpartSnapshot.Material,
                HasNormals = true
            };

            SceneModel.Instance.DynamicRenderObjects[1] =
                RenderObjectFactory.Instance.BuildDynamicRenderObject(container) as DynamicRenderObject;


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
