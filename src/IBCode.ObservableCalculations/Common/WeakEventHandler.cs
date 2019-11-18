using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace IBCode.ObservableComputations.Common
{
	public sealed class WeakEventHandler<TEventArgs> where TEventArgs : EventArgs
	{
		private readonly WeakReference<EventHandler<TEventArgs>> _weakReference;

		public WeakEventHandler(EventHandler<TEventArgs> handler)
		{
			_weakReference = new WeakReference<EventHandler<TEventArgs>>(handler);
		}

		public void Handle(object sender, TEventArgs eventArgs)
		{
			EventHandler<TEventArgs> handler;
				
			if (_weakReference.TryGetTarget(out handler))
			{
				handler(sender, eventArgs);
			}
		}
	}

	public sealed class WeakPropertyChangedEventHandler
	{
		private readonly WeakReference<PropertyChangedEventHandler> _weakReference;

		public WeakPropertyChangedEventHandler(PropertyChangedEventHandler handler)
		{
			_weakReference = new WeakReference<PropertyChangedEventHandler>(handler);
		}

		public void Handle(object sender, PropertyChangedEventArgs eventArgs)
		{
			PropertyChangedEventHandler handler;
				
			if (_weakReference.TryGetTarget(out handler))
			{
				handler(sender, eventArgs);
			}
		}
	}

	public sealed class WeakNotifyCollectionChangedEventHandler
	{
		private readonly WeakReference<NotifyCollectionChangedEventHandler> _weakReference;

		public WeakNotifyCollectionChangedEventHandler(NotifyCollectionChangedEventHandler handler)
		{
			_weakReference = new WeakReference<NotifyCollectionChangedEventHandler>(handler);
		}

		public void Handle(object sender, NotifyCollectionChangedEventArgs eventArgs)
		{
			NotifyCollectionChangedEventHandler handler;
				
			if (_weakReference.TryGetTarget(out handler))
			{
				handler(sender, eventArgs);
			}
		}
	}
}
