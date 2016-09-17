using MessageHandling.SnapshotFormat;

namespace MessageHandling.Messages
{
    public class SnapshotMessage : Message
    {
        public SnapshotCollector SnapshotCollector;
        public SnapshotMessage(MessageType type, SnapshotCollector collector) : base(type)
        {
            SnapshotCollector = collector;
        }
    }
}
