using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MessageHandling;
using Model;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using RenderEngine.Converter;
using RenderEngine.Rendering;
using Shared.Geometry;

namespace RenderEngine
{
    public partial class OpenTkControl : OpenTK.GLControl, IObserver
    {
        private bool _loaded;
        private SceneModel _sceneModel = new SceneModel();
        private List<RenderMesh> renderMeshes;

        public AbstractModel AbstractModel { get { return _sceneModel; } }

        public OpenTkControl() : base(new GraphicsMode(32, 24, 8, 8), 3, 0, GraphicsContextFlags.ForwardCompatible)
        {
            InitializeComponent();
            _sceneModel.AttachObserver(this);
        }

        private void OpenTkControl_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            _loaded = true;

            GL.ClearColor(0.4f, 0.4f, 0.4f, 1.0f);
            Application.Idle += Application_Idle;

        }

        private void OpenTkControl_Paint(object sender, PaintEventArgs e)
        {
            if (!_loaded) // Play nice
                return;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            SwapBuffers();
        }

        private void OpenTkControl_Resize(object sender, EventArgs e)
        {
            if (!_loaded)
                return;
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

        public void Notify(AbstractModel abstractModel, MessageHandling.Message m)
        {
            List <Mesh> meshes = (m as MeshMessage).GetMeshes;
            renderMeshes = MeshConverter.ToRenderMeshes(meshes);
        }
    }
}
