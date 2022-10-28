namespace CDS.Core
{
    public sealed class WeakEventSubscriber<TSub, TArgs> where TSub : class
                                                    where TArgs : EventArgs
    {
        private readonly WeakReference<TSub> subscriber;
        private readonly EventHandler<TArgs> handler;
        private readonly Action<EventHandler<TArgs>> unsubscribe;
        private readonly SynchronizationContext? synchronizationContext;

        public WeakEventSubscriber(TSub subscriber,
                                        EventHandler<TArgs> handler,
                                        Action<EventHandler<TArgs>> subscribe,
                                        Action<EventHandler<TArgs>> unsubscribe)
        {
            this.subscriber = new WeakReference<TSub>(subscriber, false);
            this.handler = handler;
            this.unsubscribe = unsubscribe;
            synchronizationContext = SynchronizationContext.Current;

            subscribe(Handler);
        }

        private void Handler(object? sender, TArgs args)
        {
            void PostHandler(object? e)
            {
                if (e is Tuple<object?, TArgs> v)
                {
                    handler(v.Item1, v.Item2);
                }
            }

            if (subscriber.TryGetTarget(out TSub? sub))
            {
                if (synchronizationContext != null && synchronizationContext != SynchronizationContext.Current)
                {
                    synchronizationContext.Post(new SendOrPostCallback(PostHandler), new Tuple<object?, TArgs>(sender, args));
                }
                else
                {
                    handler(sender, args);
                }
            }
            else
            {
                unsubscribe(Handler);
            }
        }
    }
}
