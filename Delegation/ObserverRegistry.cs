using MessageHandling;

namespace Delegation
{
    public static class ObserverRegistry
    {
        private static Dispatcher dispatcher = Dispatcher.Instance;
        public static void RegisterObserver(IObserver observer, MessageType messageType)
        {
            dispatcher.AddObserver(observer, messageType);
        }
    }
}
