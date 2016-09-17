using System;
using System.Collections.Generic;
using Shared.Geometry;

namespace MessageHandling.SnapshotFormat
{
    public class SnapshotCollector
    {
        private bool CollectTsv;
        public SnapshotCollector(bool collectTsv)
        {
            CollectTsv = collectTsv;
        }

        public List<Snapshot> Snapshots = new List<Snapshot>(); 

        public void AddNextSnapshot(Mesh roughPartSnap, Mesh tsv, Vector3d path, int toolId)
        {
            if(CollectTsv && tsv == null)
                throw new ArgumentException("Tool swept volume is null");
            Snapshots.Add(new Snapshot(roughPartSnap, tsv, path, toolId, CollectTsv, 1000));
        }
    }

    public class Snapshot
    {
        public Snapshot(Mesh roughpartSnapshot, Mesh tsv, Vector3d path, int toolId, bool collectTsv, long stopIntervalMillis)
        {
            RoughpartSnapshot = roughpartSnapshot;
            Tsv = tsv;
            Path = path;
            ToolId = toolId;
            CollectTsv = collectTsv;
            StopIntervalMillis = stopIntervalMillis;
        }

        public Mesh RoughpartSnapshot;
        public Mesh Tsv;
        public Vector3d Path;
        public int ToolId;
        public bool CollectTsv;
        public long StopIntervalMillis;
    }
}
