using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MessageHandling;
using Model;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using RenderEngine.Converter;
using RenderEngine.GraphicObjects;
using RenderEngine.Rendering;
using RenderEngine.Resources.Shader;
using RenderEngine.Scene;
using Shared.Geometry;

namespace RenderEngine
{
    public partial class OpenTkControl : OpenTK.GLControl
    {
        private bool _loaded;
        private readonly SceneManager _sceneManager = new SceneManager();

        public OpenTkControl() : base(new GraphicsMode(32, 24, 8, 8), 3, 0, GraphicsContextFlags.ForwardCompatible)
        {
            InitializeComponent();
        }

        private void OpenTkControl_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            _loaded = true;

            GL.ClearColor(0.4f, 0.4f, 0.4f, 1.0f);
            Application.Idle += Application_Idle;

            _sceneManager.Load();
        }

        private void OpenTkControl_Paint(object sender, PaintEventArgs e)
        {
            if (!_loaded) // Play nice
                return;
            _sceneManager.Paint();
            SwapBuffers();
        }

        private void OpenTkControl_Resize(object sender, EventArgs e)
        {
            if (!_loaded)
                return;
            _sceneManager.Resized(Width, Height);
            Invalidate();
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            // no guard needed -- we hooked into the event in Load handler

            // double milliseconds = Mesh.Rendering.PerformanceCounter.ComputeTimeSlice();
            // Mesh.Rendering.PerformanceCounter.Accumulate(milliseconds);
            // Render();
            Invalidate();

        }
    }
}
