using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventHandling;

namespace Delegation
{
    public static class ObserverRegistry
    {
        private static Dispatcher dispatcher = Dispatcher.Instance;
        public static void RegisterObserver(IObserver observer)
        {
            dispatcher.AddObserver(observer);
        }
    }
}
