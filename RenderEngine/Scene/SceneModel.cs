using System;
using System.Collections.Generic;
using MessageHandling;
using Model;
using RenderEngine.Converter;
using RenderEngine.Rendering;
using Shared.Geometry;

namespace RenderEngine
{
    public class SceneModel : AbstractModel
    {
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

        internal List<IRenderable> RenderMeshes = new List<IRenderable>();
        internal int SceneWidth { get; set; }
        internal int SceneHeight { get; set; }
        internal bool MeshUpdated { get; set; } //TODO: declare which mesh has been modified! Only mesh that has been changed must be modified.

        //Matrices
        internal Matrix4d LookAtMatrix { get; set; }
        internal Matrix4d WorldTransformationMatrix { get; set; }
        internal Matrix4d ProjectionMatrix { get; set; }
        internal Matrix4d RotationMatrix { get; set; }

        //Singletong
        private static SceneModel _instance;
        private event ModelHandler<AbstractModel> Changed;

        //Constructor
        private SceneModel() { }

        internal void AddRenderObject(IRenderable renderObject)
        {
            RenderMeshes.Add(renderObject);
        }

        public override void AttachObserver(IObserver observer)
        {
            Changed += observer.Notified;
        }

        public override void AttachModelObserver(AbstractModel abstractModel)
        {
            throw new NotImplementedException();
        }

        public override void ModelNotified(AbstractModel sender, Message message)
        {
            if (message.MessageType == MessageType.NewRoughParts)
            {
                var meshMessage = message as MeshMessage;
                RenderMeshes.AddRange(MeshConverter.ToRenderMeshes(meshMessage.GetMeshes));
            }
        }
    }
}
