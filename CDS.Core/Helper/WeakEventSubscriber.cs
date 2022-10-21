namespace CDS.Core
{
    public abstract class WeakEventSubscriber<TSub, TPub> where TSub : class
                                                            where TPub : class
    {
        private WeakReference<TSub> Subscriber
        {
            get; init;
        }
        protected TPub Publisher
        {
            get; init;
        }

        public WeakEventSubscriber(TSub subscriber, TPub publisher)
        {
            Subscriber = new WeakReference<TSub>(subscriber, false);
            Publisher = publisher;
        }

        public abstract void SubScribe();
        public abstract void Unsubscribe();

        protected TSub? GetSubscriber()
        {
            if (Subscriber.TryGetTarget(out TSub? subscriber))
            {
                return subscriber;
            }

            Unsubscribe();
            return null;
        }
    }
}
