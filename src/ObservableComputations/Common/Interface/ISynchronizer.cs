using System;
using System.Collections.Specialized;

namespace ObservableComputations
{
	public interface ISynchronizer
	{
		void Invoke(Action action, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs);
	}
}
