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
        public MeshMessage(MessageType type) : base(type)
        {

        }

        public List<Mesh> GetMeshes { get { return Meshes;} } 
        public void AddMesh(Mesh mesh)
        {
            Meshes.Add(mesh);
        }
    }
}
