using System;
using System.Collections.Generic;
using GraphicsEngine.HalfedgeMesh;
using MessageHandling;
using MessageHandling.Messages;
using Shared.Geometry;

namespace Model
{    
    public class MeshModel : AbstractModel
    {
        private event ModelHandler<AbstractModel> Changed;
       // private List<HeMesh> _meshes = new List<HeMesh>(); 
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
            DistributeNewMesh(MessageType.NewRoughParts, meshes);
        }

        public void AddTools(List<Mesh> meshes)
        {
            DistributeNewMesh(MessageType.NewTools, meshes);
        }

        public void AddRoughPart(Mesh roughPart)
        {
            List<Mesh> roughParts = new List<Mesh>() {roughPart};
            AddRoughParts(roughParts);
        }

        public void AddTool(Mesh tool)
        {
            List<Mesh> tools = new List<Mesh>() { tool };
            AddTools(tools);
        }

        public void ClearMeshList()
        {
            //RoughParts.Clear();
            Changed(this, new MeshMessage(MessageType.ClearMeshes, null));
            Changed(this, new HeMeshMessage(MessageType.ClearMeshes, null));
        }

        private void DistributeNewMesh(MessageType messageType, List<Mesh> meshes)
        {
            List<HeMesh> heMeshes = new List<HeMesh>();
            foreach (var mesh in meshes)
            {
                heMeshes.Add(new HeMesh(mesh));
            }
            Changed(this, new HeMeshMessage(messageType, heMeshes));

            // convert them back to Mesh
            List<Mesh> augmentedMeshes = new List<Mesh>();
            foreach (var heMesh in heMeshes)
            {
                augmentedMeshes.Add(new Mesh(heMesh));
            }
            Changed(this, new MeshMessage(messageType, augmentedMeshes));
        }
    }
}
