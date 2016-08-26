using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandling
{
    [Flags]
    public enum MessageType 
    {
        //Use power of two values here!
        NewMeshes = 1,
        LoadFile = 2,
    }
}
