using System.Collections.Generic;
using Shared.Geometry;

namespace MessageHandling
{
    public class MeshMessage : Message
    {
        public List<Mesh> Meshes = new List<Mesh>();
        public MeshMessage(MessageType type, List<Mesh> meshes) : base(type)
        {
            Meshes = meshes;
        }

        public List<Mesh> GetMeshes => Meshes;
    }
}
