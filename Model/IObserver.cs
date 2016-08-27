using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MessageHandling;

namespace Model
{
    public interface IObserver
    {
        void Notified(AbstractModel abstractModel, Message m);
    }
}
