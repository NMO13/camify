using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Geometry;

namespace RenderEngine.Rendering
{
    public enum AnimationType
    {
        Interval,
        Smooth
    };

    class AnimationManager
    {
        private RenderObject _parent;
        private AnimationType _animationType;
        internal AnimationManager(RenderObject parent, AnimationType animType)
        {
            Timer.Start();
            _parent = parent;
            _animationType = animType;
        }
        internal List<Vector3d> Paths { get; set; }
        internal long StopInterval { get; set; }
        private Stopwatch Timer = new Stopwatch();
        private int Counter = 0;

        internal void NextStep()
        {
            if (Timer.ElapsedMilliseconds > StopInterval)
            {
                if (Paths != null && Counter < Paths.Count)
                {
                    var amount = CalcTranslationAmount();
                    _parent.Translate(amount);

                    Timer.Restart();
                    Counter++;
                }
            }
        }

        private Vector3d CalcTranslationAmount()
        {
            if (_animationType == AnimationType.Interval)
            {
                return Paths[Counter];
            }
            else // Smooth
            {
                throw new NotSupportedException();
            }
        }
    }
}
