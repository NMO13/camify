using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GraphicsEngine.HalfedgeMesh;
using MessageHandling;
using Model;
using Shared.Geometry;

namespace RenderEngine
{
    public class MeshManager : AbstractModel
    {
        private event ModelHandler<AbstractModel> Changed;
        public MeshManager(SceneModel model)
        {
            AttachModelObserver(model);
        }

        public void AddMesh(Mesh mesh)
        {
            List<Mesh> meshes = new List<Mesh>() {mesh};
            Changed(this, new MeshMessage(MessageType.NewTools, meshes));
        }

        public override void AttachModelObserver(AbstractModel abstractModel)
        {
            Changed += abstractModel.ModelNotified;
        }

        public override void AttachObserver(IObserver observer)
        {
            throw new NotImplementedException();
        }

        public override void ModelNotified(AbstractModel sender, Message message)
        {
            throw new NotImplementedException();
        }
    }
}
