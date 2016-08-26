using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace CNCSpecific.Milling
{
    class SnapshotCollector
    {
        private List<Mesh> _meshList = new List<Mesh>();

        internal void AddNextMesh(Mesh mesh)
        {
            _meshList.Add(mesh);
        }

        public List<Mesh> Meshes => _meshList;
    }
}
