using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Geometry.HalfedgeMesh;

namespace MessageHandling.Messages
{
    public class HeMeshMessage : Message
    {
        public List<HeMesh> Meshes = new List<HeMesh>();
        public HeMeshMessage(MessageType type, List<HeMesh> meshes) : base(type)
        {
            Meshes = meshes;
        }

        public List<HeMesh> GetMeshes { get { return Meshes; } }
    }
}
