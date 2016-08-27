﻿using System.Collections.Generic;
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
            Changed += observer.Notify;
        }

        public override void AttachModelObserver(AbstractModel abstractModel)
        {
            Changed += abstractModel.ModelNotified;
        }

        public override void ModelNotified(AbstractModel sender, Message message)
        {
        }

        public void AddMeshes(List<Mesh> meshes)
        {
            _meshes.AddRange(meshes);
            Changed(this, new MeshMessage(MessageType.NewRoughParts, meshes));
        }

    }
}