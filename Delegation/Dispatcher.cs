using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventHandling;

namespace Delegation
{
    public class MessageSender
    {
        private static MessageSender _instance = new MessageSender();

        public void SendMessage(IMessage message)
        {
            Dispatcher.Instance.ProcessMessage(message);
        }
        public static MessageSender Instance
        {
            get { return _instance; }
        }
    }

    internal class Dispatcher
    {
        private List<IObserver> _observerList = new List<IObserver>();

        private static Dispatcher _instance = new Dispatcher();
        internal static Dispatcher Instance { get { return _instance; } }
        public void ReceiveMessage(IMessage m)
        {
            ProcessMessage(m);
        }

        internal void AddObserver(IObserver observer)
        {
            _observerList.Add(observer);
        }

        internal void ProcessMessage(IMessage m)
        {
            foreach (var observer in _observerList)
            {
                observer.Notify(m);
            }
        }
    }
}
