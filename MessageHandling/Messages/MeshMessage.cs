using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace MessageHandling
{
    public class MeshMessage : Message
    {
        public List<Mesh> Meshes = new List<Mesh>();
        public MeshMessage(MessageType type, List<Mesh> meshes) : base(type)
        {
            Meshes = meshes;
        }

        public List<Mesh> GetMeshes { get { return Meshes;} } 
    }
}
