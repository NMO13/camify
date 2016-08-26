using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace MessageHandling
{
    public abstract class Message
    {
        protected Message(MessageType type)
        {
            MessageType = type; 
        }
        public MessageType MessageType { get; }
    }
}
