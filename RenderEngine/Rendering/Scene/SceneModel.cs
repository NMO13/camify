using System;
using System.Collections.Generic;
using MessageHandling;
using MessageHandling.Messages;
using MessageHandling.SnapshotFormat;
using Model;
using RenderEngine.Conversion;
using RenderEngine.GraphicObjects;
using RenderEngine.GraphicObjects.ObjectTypes.Dynamic;
using RenderEngine.GraphicObjects.ObjectTypes.Static;
using Shared.Geometry;

namespace RenderEngine.Rendering.Scene
{
    public class SceneModel : AbstractModel
    {
        //Singleton
        private static SceneModel _instance;
        public static SceneModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SceneModel();
                }
                return _instance;
            }
        }

        public bool WireframeMode { get; set; }
        public bool ShowNormals { get; set; }

        internal List<DynamicRenderObject> DynamicRenderObjects = new List<DynamicRenderObject>();
        internal List<StaticRenderObject> StaticRenderObjects = new List<StaticRenderObject>();
        internal int SceneWidth { get; set; }
        internal int SceneHeight { get; set; }
        internal bool MeshUpdated { get; set; } //TODO: declare which mesh has been modified! Only mesh that has been changed must be modified.

        public SnapshotCollector CurrentCollector;

        public AnimationState CurrentAnimationState = AnimationState.Stop;
        public AnimationState LastAnimationState = AnimationState.Stop;

        //Matrices
        internal Matrix4d LookAtMatrix { get; set; } = Matrix4d.Identity;
        internal Matrix4d WorldTransformationMatrix { get; set; } = Matrix4d.Identity;
        internal Matrix4d ProjectionMatrix { get; set; } = Matrix4d.Identity;
        internal Matrix4d RotationMatrix { get; set; } = Matrix4d.Identity;

        public bool IsSnapshotCollectionValid
        {
            get { return CurrentCollector != null && CurrentCollector.Snapshots.Count > 0; }
        }

        //Constructor
        private SceneModel() { }

        internal void AddDynamicRenderObject(DynamicRenderObject dynamicObject)
        {
            DynamicRenderObjects.Add(dynamicObject);
        }

        internal void AddStaticObject(StaticRenderObject staticObject)
        {
            StaticRenderObjects.Add(staticObject);
        }

        public override void AttachObserver(IObserver observer)
        {
        }

        public override void AttachModelObserver(AbstractModel abstractModel)
        {
            throw new NotImplementedException();
        }

        public override void ModelNotified(AbstractModel sender, Message message)
        {
            if (message.MessageType == MessageType.NewRoughParts || message.MessageType == MessageType.NewTools)
            {
                var meshMessage = message as MeshMessage;
                if (meshMessage == null)
                    return;
                DynamicRenderObjects.AddRange(Converter.ToDynamicRenderObjects(meshMessage.GetMeshes));
            }
            else if (message.MessageType == MessageType.ClearMeshes)
            {
                var meshMessage = message as MeshMessage;
                if (meshMessage == null)
                    return;
                DynamicRenderObjects.Clear();
            }
            else if (message.MessageType == MessageType.MoveObject)
            {
                var transformationMessage = message as TransformationMessage;
                if (transformationMessage == null)
                    return;
                DynamicRenderObjects[transformationMessage.ToolId].Translate(transformationMessage.Transformation.Vector3d);
            }
            else if (message.MessageType == MessageType.NewSnapshotList)
            {
                var snapshotMessage = message as SnapshotMessage;
                if (snapshotMessage == null)
                    return;
                CurrentCollector = snapshotMessage.SnapshotCollector;
               // DebugAnimation();
            }
        }

        public void DebugAnimation(int id, List<Vector3d> paths, long stopInterval)
        {
            //RenderMeshes[id].AnimationManager.Paths = paths;
            //RenderMeshes[id].AnimationManager.StopInterval = stopInterval;
        }

        public void PlayAnimation()
        {
            CurrentAnimationState = AnimationState.Play;
        }
    }
}
