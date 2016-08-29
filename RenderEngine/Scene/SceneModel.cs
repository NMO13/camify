using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MessageHandling;
using Model;
using RenderEngine.Rendering;

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
        internal bool MeshModified { get; set; } //declare which mesh has been modified!
        private static SceneModel _instance;
        private event ModelHandler<AbstractModel> Changed;

        //Constructor
        private SceneModel() { }
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
                //RenderMeshes = meshMessage.GetMeshes;
            }
        }

        internal void AddRenderObject(IRenderable renderObject)
        {
             RenderMeshes.Add(renderObject);
        }
    }
}
