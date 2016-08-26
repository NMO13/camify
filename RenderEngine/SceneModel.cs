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
    class SceneModel : AbstractModel
    {
        private event ModelHandler<AbstractModel> Changed;
        List<RenderMesh> _renderMeshes = new List<RenderMesh>();
        public override void AttachObserver(IObserver observer)
        {
            Changed += observer.Notify;
        }

        public override void AttachModelObserver(AbstractModel abstractModel)
        {
            throw new NotImplementedException();
        }

        public override void ModelNotified(AbstractModel sender, Message message)
        {
            if (message.MessageType == MessageType.NewMeshes)
            {
                Changed(this, message);
            }
        }
    }
}
