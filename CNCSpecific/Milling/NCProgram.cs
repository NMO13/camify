using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;
using Shared.Geometry;

namespace CNCSpecific.Milling
{
    public class NCProgram
    {
        private List<NcPath> _paths = new List<NcPath>();

        public void AddPath(Vector3m relativeAmount, int toolId)
        {
            var path = new NcPath();
            path.RelativePosition = relativeAmount;
            path.ActiveTool = toolId;
            _paths.Add(path);
        }

        public void AddPath(double x, double y, double z)
        {
            AddPath(new Vector3m(x, y, z), 0);
        }

        public List<NcPath> PathList { get { return _paths; } }  
    }
}
