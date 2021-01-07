// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Threading;

namespace ObservableComputations
{
	internal struct PropertyChangedEventSubscription
	{
		public readonly INotifyPropertyChanged Source;
		public readonly PropertyChangedEventHandler Handler;

		public PropertyChangedEventSubscription(INotifyPropertyChanged source, PropertyChangedEventHandler handler)
		{
			Source = source;
			Handler = handler;
		} 

	}

	internal struct MethodChangedEventSubscription
	{
		public readonly INotifyMethodChanged Source;
		public readonly EventHandler<MethodChangedEventArgs> Handler;

		public MethodChangedEventSubscription(INotifyMethodChanged source, EventHandler<MethodChangedEventArgs> handler)
		{
			Source = source;
			Handler = handler;
		}
	}

	internal struct Subscriptions
	{
		public readonly PropertyChangedEventSubscription[] PropertyChangedEventSubscriptions;
		public readonly MethodChangedEventSubscription[] MethodChangedEventSubscriptions;

		public Subscriptions(PropertyChangedEventSubscription[] propertyChangedEventSubscriptions, MethodChangedEventSubscription[] methodChangedEventSubscriptions)
		{
			PropertyChangedEventSubscriptions = propertyChangedEventSubscriptions;
			MethodChangedEventSubscriptions = methodChangedEventSubscriptions;
		}
	}

	internal static class EventUnsubscriber
	{
		static readonly ConcurrentQueue<Subscriptions> _subscriptionsQueue = new ConcurrentQueue<Subscriptions>();	
		private static readonly ManualResetEventSlim[] _newSubscriptionManualResetEvents;
		private readonly static Thread[] __threads;
		private static readonly int _threadsCount;

		public static void QueueSubscriptions(
			PropertyChangedEventSubscription[] propertyChangedEventSubscriptions,
			MethodChangedEventSubscription[] methodChangedEventSubscriptions)
		{
			_subscriptionsQueue.Enqueue(new Subscriptions(propertyChangedEventSubscriptions, methodChangedEventSubscriptions));
			for (int index = 0; index < _threadsCount; index++)			
				_newSubscriptionManualResetEvents[index].Set();
		}

		static EventUnsubscriber()
		{
			_threadsCount = Configuration.EventUnsubscriberThreadsCount;
			_newSubscriptionManualResetEvents = new ManualResetEventSlim[_threadsCount];
			__threads = new Thread[_threadsCount];

			void threadStart(object state)
			{
				int index = (int) state;
				while (true)
				{
					_newSubscriptionManualResetEvents[index].Wait();
					_newSubscriptionManualResetEvents[index].Reset();

					while (_subscriptionsQueue.TryDequeue(out Subscriptions subscriptions))
					{
						for (int i = 0; i < subscriptions.PropertyChangedEventSubscriptions.Length; i++)
						{
							PropertyChangedEventSubscription propertyChangedEventSubscription = subscriptions.PropertyChangedEventSubscriptions[i];

							if (propertyChangedEventSubscription.Handler != null) propertyChangedEventSubscription.Source.PropertyChanged -= propertyChangedEventSubscription.Handler;
						}

						for (int i = 0; i < subscriptions.MethodChangedEventSubscriptions.Length; i++)
						{
							MethodChangedEventSubscription methodChangedEventSubscription = subscriptions.MethodChangedEventSubscriptions[i];

							if (methodChangedEventSubscription.Handler != null) methodChangedEventSubscription.Source.MethodChanged -= methodChangedEventSubscription.Handler;
						}
					}
				}
			}

			for (int index = 0; index < _threadsCount; index++)
			{
				_newSubscriptionManualResetEvents[index] = new ManualResetEventSlim(false);
				__threads[index] = new Thread(threadStart)
				{
					Name = $"ObservableComputations events unsubscriber #{index}",
					IsBackground = true
				};
				__threads[index].Start(index);
			}
		}
	}
}
