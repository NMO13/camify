using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHandling
{
    public interface IMessage
    {
        Object[] Payload { get; set; }
    }
}
