using System.Collections.Generic;
using MessageHandling;

namespace Delegation
{
    public class MessageSender
    {
        private static MessageSender _instance = new MessageSender();

        public void SendMessage(Message message)
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
        private Dictionary<MessageType, List<IObserver>> _observerDictionary = new Dictionary<MessageType, List<IObserver>>(); 

        private static Dispatcher _instance = new Dispatcher();
        internal static Dispatcher Instance { get { return _instance; } }
        internal void SendMessage(Message m, MessageType messageType)
        {
            ProcessMessage(m);
        }

        internal void AddObserver(IObserver observer, MessageType messageType)
        {
            List<IObserver> list;
            _observerDictionary.TryGetValue(messageType, out list);
            if (list == null)
            {
                list = new List<IObserver>();
                _observerDictionary.Add(messageType, list);
            }
            list.Add(observer);
        }

        internal void ProcessMessage(Message m)
        {
            var observers = _observerDictionary[m.MessageType];
            foreach (var observer in observers)
            {
                observer.Notify(m);
            }
        }
    }
}
