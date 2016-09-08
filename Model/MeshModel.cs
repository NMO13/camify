using System;
using System.Collections.Generic;
using GraphicsEngine.HalfedgeMesh;
using MessageHandling;
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
            List<HeMesh> heMeshes = new List<HeMesh>();
            foreach (var mesh in meshes)
            {
                heMeshes.Add(new HeMesh(mesh));
            }
            //Changed(this, new MeshMessage(MessageType.NewRoughParts, heMeshes));

            // convert them back to Mesh
            List<Mesh> augmentedMeshes = new List<Mesh>();
            foreach (var heMesh in heMeshes)
            {
                augmentedMeshes.Add(new Mesh(heMesh));
            }
            Changed(this, new MeshMessage(MessageType.NewRoughParts, augmentedMeshes));

        }

        public void AddTools(List<Mesh> meshes)
        {
            //TODO convert to hemeshes here
            //_meshes.AddRange(meshes);
            //Changed(this, new MeshMessage(MessageType.NewTools, meshes));
        }

        public void AddRoughPart(Mesh roughPart)
        {
            List<Mesh> roughParts = new List<Mesh>() {roughPart};
            AddRoughParts(roughParts);
        }

        public void ClearMeshList()
        {
            //RoughParts.Clear();
            //Changed(this, new MeshMessage(MessageType.ClearMeshes, _meshes));
        }

        //public List<Mesh> RoughParts { get { return _meshes;} } 
    }
}
