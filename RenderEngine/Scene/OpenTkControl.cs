﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using RenderEngine.IO;
using RenderEngine.Scene;

namespace RenderEngine.Scene
{
    public partial class OpenTkControl : OpenTK.GLControl
    {
        private bool _loaded;
        private readonly SceneManager _sceneManager = new SceneManager();
        private MouseKeyEvents _mouseKeyEvents;

        public OpenTkControl() : base(new GraphicsMode(32, 24, 8, 8), 3, 0, GraphicsContextFlags.ForwardCompatible)
        {
            InitializeComponent();
            MouseWheel += OpenTkControl_MouseWheel;
        }

        private void OpenTkControl_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            _loaded = true;
            Application.Idle += Application_Idle;
            _sceneManager.Load(Width, Height);
            _mouseKeyEvents = new MouseKeyEvents();
        }

        private void OpenTkControl_Paint(object sender, PaintEventArgs e)
        {
            if (!_loaded) // Play nice
                return;
            _sceneManager.AdjustCamera();
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

        private void OpenTkControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!_loaded)
                return;

            _mouseKeyEvents.MouseWheel(sender, e.Delta);
        }

        private void OpenTkControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (!_loaded)
                return;

            if (e.Button == MouseButtons.Left)
                _mouseKeyEvents.LeftMouseButtonDown(sender, e.Location);
            else if (e.Button == MouseButtons.Right)
                _mouseKeyEvents.RightMouseButtonDown(sender, e.Location);
            if (e.Button == MouseButtons.Middle)
                _mouseKeyEvents.MiddleMouseButtonDown(sender, e.Location);
        }
    }
}
