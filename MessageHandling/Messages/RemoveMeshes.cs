using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Geometry;

namespace MessageHandling.Messages
{
    public class RemoveMeshes : Message
    {
        public List<Mesh> Meshes = new List<Mesh>();
        public List<Mesh> GetMeshes { get { return Meshes; } }
        public RemoveMeshes(MessageType type, List<Mesh> meshes) : base(type)
        {
            Meshes = meshes;
        }
    }
}
