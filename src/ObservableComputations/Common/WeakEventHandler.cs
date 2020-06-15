using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

namespace ObservableComputations
{
	public sealed class WeakMethodChangedEventHandler : IWeakEventHandler
	{
		private readonly WeakReference<EventHandler<MethodChangedEventArgs>> _weakReference;
		private readonly INotifyMethodChanged _eventSource;


		public WeakMethodChangedEventHandler(EventHandler<MethodChangedEventArgs> handler, INotifyMethodChanged eventSource)
		{
			_weakReference = new WeakReference<EventHandler<MethodChangedEventArgs>>(handler);
			_eventSource = eventSource;
		}

		public void Handle(object sender, MethodChangedEventArgs eventArgs)
		{			
			if (_weakReference.TryGetTarget(out var handler))
				handler(sender, eventArgs);
			else
				_eventSource.MethodChanged -= Handle;
		}

		#region Implementation of IWeakEventHandler

		public bool TryUnsubscribe()
		{
			if (_weakReference.TryGetTarget(out _))
				return false;

			_eventSource.MethodChanged -= Handle;
			return true;
		}

		#endregion
	}

	public sealed class WeakPropertyChangedEventHandler : IWeakEventHandler
	{
		private readonly WeakReference<PropertyChangedEventHandler> _weakReference;
		private readonly INotifyPropertyChanged _eventSource;

		public WeakPropertyChangedEventHandler(PropertyChangedEventHandler handler, INotifyPropertyChanged eventSource)
		{
			_weakReference = new WeakReference<PropertyChangedEventHandler>(handler);
			_eventSource = eventSource;
		}

		public void Handle(object sender, PropertyChangedEventArgs eventArgs)
		{			
			if (_weakReference.TryGetTarget(out var handler))
				handler(sender, eventArgs);
			else
				_eventSource.PropertyChanged -= Handle;
		}

		#region Implementation of IWeakEventHandler

		public bool TryUnsubscribe()
		{
			if (_weakReference.TryGetTarget(out _))
				return false;

			_eventSource.PropertyChanged -= Handle;
			return true;
		}

		#endregion
	}

	public sealed class WeakNotifyCollectionChangedEventHandler : IWeakEventHandler
	{
		private readonly WeakReference<NotifyCollectionChangedEventHandler> _weakReference;
		private readonly INotifyCollectionChanged _eventSource;

		public WeakNotifyCollectionChangedEventHandler(NotifyCollectionChangedEventHandler handler, INotifyCollectionChanged eventSource)
		{
			_weakReference = new WeakReference<NotifyCollectionChangedEventHandler>(handler);
			_eventSource = eventSource;
		}

		public void Handle(object sender, NotifyCollectionChangedEventArgs eventArgs)
		{
			if (_weakReference.TryGetTarget(out var handler))
				handler(sender, eventArgs);
			else
				_eventSource.CollectionChanged -= Handle;
		}

		#region Implementation of IWeakEventHandler

		public bool TryUnsubscribe()
		{
			if (_weakReference.TryGetTarget(out _))
				return false;

			_eventSource.CollectionChanged -= Handle;
			return true;
		}

		#endregion
	}

	internal interface IWeakEventHandler
	{
		bool TryUnsubscribe();
	}

	internal static class WeakEventCleaner
	{
		public static ConcurrentQueue<IWeakEventHandler> _singeWeakEventHandlersQueue = new ConcurrentQueue<IWeakEventHandler>();
		public static ConcurrentQueue<List<IWeakEventHandler>> _manyWeakEventHandlersQueue = new ConcurrentQueue<List<IWeakEventHandler>>();

		static WeakEventCleaner()
		{
			Thread thread = new Thread(() =>
			{
				do
				{
					Thread.Sleep(Configuration.WeakEventCleanPeriod);

					int count = _singeWeakEventHandlersQueue.Count;

					do
					{
						_singeWeakEventHandlersQueue.TryDequeue(out var weakEventHandler);
						if (!weakEventHandler.TryUnsubscribe())
							_singeWeakEventHandlersQueue.Enqueue(weakEventHandler);
						count--;

					} while (count > 0);

					count = _manyWeakEventHandlersQueue.Count;

					do
					{
						_manyWeakEventHandlersQueue.TryDequeue(out var weakEventHandlers);
						int removedCount = 0;
						int weakEventHandlersCount = weakEventHandlers.Count;
						for (int i = 0; i < weakEventHandlersCount; i++)
						{
							IWeakEventHandler weakEventHandler = weakEventHandlers[i - removedCount];
							if (weakEventHandler.TryUnsubscribe())
								weakEventHandlers.RemoveAt(i - removedCount);
						}

						if (removedCount < weakEventHandlersCount)
							_manyWeakEventHandlersQueue.Enqueue(weakEventHandlers);
						count--;

					} while (count > 0);
				} while (true);
			});

			thread.IsBackground = true;
			thread.Name = "ObservableComputations cleaner";
			thread.Start();
		}
	}
}
