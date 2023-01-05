using System.Reflection;

namespace CDS.Core
{
    public sealed class WeakEventSubscriber<TPub, TSub, TArgs> where TPub: class 
                                                    where TSub : class
                                                    where TArgs : EventArgs
    {
        private readonly TPub publisher;
        private readonly string eventName;
        private readonly WeakReference<TSub> subscriber;
        private readonly Action<TSub, object?, TArgs> handler;
        private readonly SynchronizationContext? synchronizationContext;

        public WeakEventSubscriber(TPub publisher, 
                                    string eventName,
                                    TSub subscriber,
                                    Action<TSub, object?, TArgs> handler)
        {
            this.publisher = publisher;
            this.eventName = eventName;
            this.subscriber = new WeakReference<TSub>(subscriber, false);
            this.handler = handler;
            synchronizationContext = SynchronizationContext.Current;

            Subscriber();
        }

        private void Subscriber()
        {
            if(publisher.GetType().GetEvent(eventName) is EventInfo evt)
            {
                evt.AddEventHandler(publisher, new EventHandler<TArgs>(Handler));
            }
        }

        public void Unsubscribe()
        {
            if (publisher.GetType().GetEvent(eventName) is EventInfo evt)
            {
                evt.RemoveEventHandler(publisher, new EventHandler<TArgs>(Handler));
            }
        }

        private void Handler(object? sender, TArgs args)
        {
            void PostHandler(object? e)
            {
                if (e is Tuple<TSub, object?, TArgs> v)
                {
                    handler(v.Item1, v.Item2, v.Item3);
                }
            }

            if (subscriber.TryGetTarget(out TSub? sub))
            {
                if (synchronizationContext != null && synchronizationContext != SynchronizationContext.Current)
                {
                    synchronizationContext.Post(new SendOrPostCallback(PostHandler), new Tuple<TSub, object?, TArgs>(sub, sender, args));
                }
                else
                {
                    handler(sub, sender, args);
                }
            }
            else
            {
                Unsubscribe();
            }
        }
    }
}
