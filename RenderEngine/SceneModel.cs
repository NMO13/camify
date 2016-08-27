using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageHandling;
using Model;
using RenderEngine.Rendering;

namespace RenderEngine
{
    public class SceneModel : AbstractModel
    {
        private event ModelHandler<AbstractModel> Changed;
        internal List<IRenderable> RenderMeshes = new List<IRenderable>();
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
    }
}
