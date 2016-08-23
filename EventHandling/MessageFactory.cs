using System;
using System.Collections;
using Shared.Helper;

namespace MessageHandling
{
    public class MessageFactory
    {
        public static Message GenerateMessage(MessageType combinedType)
        {
            Message message = null, head = null;
            foreach (MessageType type in combinedType.GetFlags())
            {
                Message curMessage = null;
                if (type.IsFlagSet(MessageType.LoadFile))
                {
                    curMessage = new FileMessage(combinedType);
                }
                if (type.IsFlagSet(MessageType.NewMesh))
                {
                    curMessage = new MeshMessage(combinedType);
                }

                if (message != null)
                {
                    message.Next = curMessage;
                }
                else
                {
                    message = head = curMessage;
                }
            }
           
            if (head == null)
                throw new Exception("Unknown message type");

            return head;
        }

    }
}
