using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ObservableComputations
{
	public interface ISynchronizer
	{
		void Invoke(Action<object> action, object state);
	}

	public interface IDestinationCollectionSynchronizer
	{
		void InvokeInitializeFromSourceCollection(Action action, ICollectionComputing collectionSynchronizing, object sourceItem, object sourceScalarSender, PropertyChangedEventArgs sourceScalarPropertyChangedEventArgs);
		void InvokeCollectionChange(Action action, ICollectionComputing collectionSynchronizing, object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs);
		void InvokeRaiseConsistencyRestored(Action action, ICollectionComputing collectionSynchronizing);
	}

	public interface ISourceCollectionSynchronizer
	{
		void InvokeReadAndSubscribe(Action action, ICollectionComputing collectionSynchronizing);
	}

	public interface IDestinationScalarSynchronizer
	{
		void InvokeSetValue(Action action, IScalarComputing scalarSynchronizing, object sender, PropertyChangedEventArgs propertyChangedEventArgs);
	}

	public interface ISourceScalarSynchronizer
	{
		void InvokeReadAndSubscribe(Action action, IScalarComputing scalarSynchronizing);
	}

	public interface IPostingSynchronizer
	{
		void WaitLastPostComplete();
	}
}
