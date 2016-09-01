using System;
using System.Collections.Generic;
using MessageHandling;
using Shared.Geometry;

namespace Model
{    
    public class MeshModel : AbstractModel
    {
        private event ModelHandler<AbstractModel> Changed;
        private List<Mesh> _meshes = new List<Mesh>(); 
        public override void AttachObserver(IObserver observer)
        {
            Changed += observer.Notified;
        }

        public override void AttachModelObserver(AbstractModel abstractModel)
        {
            Changed += abstractModel.ModelNotified;
        }

        public override void ModelNotified(AbstractModel sender, Message message)
        {
        }

        public void AddRoughParts(List<Mesh> meshes)
        {
            _meshes.AddRange(meshes);
            Changed(this, new MeshMessage(MessageType.NewRoughParts, meshes));
        }

        public void AddTools(List<Mesh> meshes)
        {
            _meshes.AddRange(meshes);
            Changed(this, new MeshMessage(MessageType.NewTools, meshes));
        }

        public void AddRoughPart(Mesh roughPart)
        {
            List<Mesh> roughParts = new List<Mesh>() {roughPart};
            AddRoughParts(roughParts);
        }

        public void ClearMeshList()
        {
            Changed(this, new MeshMessage(MessageType.ClearMeshes, _meshes));
        }
    }
}
