using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageHandling;

namespace Model
{
    public abstract class AbstractModel
    {
        protected delegate void ModelHandler<IModel>(IModel sender, Message e);
        public abstract void AttachObserver(IObserver observer);
        public abstract void AttachModelObserver(AbstractModel abstractModel);
        public abstract void ModelNotified(AbstractModel sender, Message message);

    }
}
