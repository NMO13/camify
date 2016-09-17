using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageHandling.Datatypes;

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
