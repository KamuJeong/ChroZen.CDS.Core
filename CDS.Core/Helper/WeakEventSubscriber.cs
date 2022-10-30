namespace CDS.Core
{
    public sealed class WeakEventSubscriber<TSub, TArgs> where TSub : class
                                                    where TArgs : EventArgs
    {
        private readonly WeakReference<TSub> subscriber;
        private readonly Action<TSub, object?, TArgs> handler;
        private readonly Action<EventHandler<TArgs>> unsubscribe;
        private readonly SynchronizationContext? synchronizationContext;

        public WeakEventSubscriber(TSub subscriber,
                                        Action<TSub, object?, TArgs> handler,
                                        Action<EventHandler<TArgs>> subscribe,
                                        Action<EventHandler<TArgs>> unsubscribe)
        {
            this.subscriber = new WeakReference<TSub>(subscriber, false);
            this.handler = handler;
            this.unsubscribe = unsubscribe;
            synchronizationContext = SynchronizationContext.Current;

            subscribe(Handler);
        }

        public void Unsubscribe() => unsubscribe(Handler);

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
                unsubscribe(Handler);
            }
        }
    }
}
