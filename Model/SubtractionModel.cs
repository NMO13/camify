using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageHandling;
using Shared;

namespace Model
{
    public class SubtractionModel : AbstractModel
    {
        private event ModelHandler<AbstractModel> Changed;
        private List<Vector3m> _pathList = new List<Vector3m>();
        //private List<DeformableObject> deformableObjects = new List<DeformableObject>(); 
        

        public void AddPaths(List<Vector3m> paths)
        {
            _pathList.AddRange(paths);
        }

        public override void AttachModelObserver(AbstractModel abstractModel)
        {
            throw new NotImplementedException();
        }

        public override void AttachObserver(IObserver observer)
        {
            throw new NotImplementedException();
        }

        public void CalcSubtractions()
        {
            
        }

        public override void ModelNotified(AbstractModel sender, Message message)
        {
            throw new NotImplementedException();
        }
    }
}
