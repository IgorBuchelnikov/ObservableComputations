using System;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

namespace ObservableComputations
{
    internal struct PropertyChangedEventSubscription
    {
        public INotifyPropertyChanged Source;
        public PropertyChangedEventHandler Handler;

        public PropertyChangedEventSubscription(INotifyPropertyChanged source, PropertyChangedEventHandler handler)
        {
            Source = source;
            Handler = handler;
        } 

    }

    internal struct CollectionChangedEventSubscription
    {
        public INotifyCollectionChanged Source;
        public NotifyCollectionChangedEventHandler Handler;

        public CollectionChangedEventSubscription(INotifyCollectionChanged source, NotifyCollectionChangedEventHandler handler)
        {
            Source = source;
            Handler = handler;
        }
    }

    internal struct MethodChangedEventSubscription
    {
        public INotifyMethodChanged Source;
        public EventHandler<MethodChangedEventArgs> Handler;

        public MethodChangedEventSubscription(INotifyMethodChanged source, EventHandler<MethodChangedEventArgs> handler)
        {
            Source = source;
            Handler = handler;
        }
    }

    internal struct Subscriptions
    {
        public PropertyChangedEventSubscription[] PropertyChangedEventSubscriptions;
        public MethodChangedEventSubscription[] MethodChangedEventSubscriptions;

        public Subscriptions(PropertyChangedEventSubscription[] propertyChangedEventSubscriptions, MethodChangedEventSubscription[] methodChangedEventSubscriptions)
        {
            PropertyChangedEventSubscriptions = propertyChangedEventSubscriptions;
            MethodChangedEventSubscriptions = methodChangedEventSubscriptions;
        }
    }

	internal static class EventUnsubscriber
	{
        static ConcurrentQueue<Subscriptions> _subscriptionsQueue = new ConcurrentQueue<Subscriptions>();	
        private static ManualResetEventSlim _newSubscriptionManualResetEvent = new ManualResetEventSlim(false);
		internal static Thread _thread;

		public static void QueueSubscriptions(
            PropertyChangedEventSubscription[] propertyChangedEventSubscriptions,
            MethodChangedEventSubscription[] methodChangedEventSubscriptions)
		{
            _subscriptionsQueue.Enqueue(new Subscriptions(propertyChangedEventSubscriptions, methodChangedEventSubscriptions));
			_newSubscriptionManualResetEvent.Set();
		}

		static EventUnsubscriber()
		{
			_thread = new Thread(() =>
			{
				while (true)
				{
					_newSubscriptionManualResetEvent.Wait();
					_newSubscriptionManualResetEvent.Reset();

					while (_subscriptionsQueue.TryDequeue(out Subscriptions subscriptions))
					{
                        for (int i = 0; i < subscriptions.PropertyChangedEventSubscriptions.Length; i++)
                        {
                            PropertyChangedEventSubscription propertyChangedEventSubscription =
                                subscriptions.PropertyChangedEventSubscriptions[i];

                            if (propertyChangedEventSubscription.Handler != null)
                                propertyChangedEventSubscription.Source.PropertyChanged -=
                                    propertyChangedEventSubscription.Handler;
                        }

                        for (int i = 0; i < subscriptions.MethodChangedEventSubscriptions.Length; i++)
                        {
                            MethodChangedEventSubscription methodChangedEventSubscription =
                                subscriptions.MethodChangedEventSubscriptions[i];

                            if (methodChangedEventSubscription.Handler != null)
                                methodChangedEventSubscription.Source.MethodChanged -=
                                    methodChangedEventSubscription.Handler;
                        }
					}
				}

			});

			_thread.Name = "ObservableComputations events unsubscriber";
            _thread.IsBackground = true;
			_thread.Start();
		}
	}
}