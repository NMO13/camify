using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Geometry;

namespace MessageHandling.Messages
{
    public class TransformationMessage : Message
    {
        public int ToolId { get; private set; }
        public Vector3m Transformation { get; private set; }
        public TransformationMessage(MessageType type, int toolId, Vector3m transformation) : base(type) //TODO use matrix for rotation instead of Vector
        {
            ToolId = toolId;
            Transformation = transformation;
        }
    }
}
