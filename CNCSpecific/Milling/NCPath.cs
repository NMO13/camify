using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;
using Shared.Geometry;

namespace CNCSpecific.Milling
{
    public class NCPath
    {
        public Vector3m RelativePosition;

        public int ActiveTool { get; internal set; }
    }
}
