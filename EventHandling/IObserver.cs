using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EventHandling
{
    public interface IObserver
    {
        void Notify(IMessage m);
    }
}
