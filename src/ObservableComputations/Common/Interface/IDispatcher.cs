using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

namespace ObservableComputations
{
	public interface IDispatcher
	{
		void Invoke(Action action);
	}

	public interface IDestinationCollectionDispatcher
	{
		void InvokeInitializeFromSourceCollection(Action action, ICollectionComputing collectionDispatching, object sourceItem, object sender, EventArgs eventArgs);
		void InvokeCollectionChange(Action action, ICollectionComputing collectionDispatching, object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs);
	}

	public interface ISourceCollectionDispatcher
	{
		void InvokeReadAndSubscribe(Action action, ICollectionComputing collectionDispatching);
	}

	public interface IDestinationScalarDispatcher
	{
		void InvokeSetValue(Action action, IScalarComputing scalarDispatching, object sender, PropertyChangedEventArgs propertyChangedEventArgs);
	}

	public interface ISourceScalarDispatcher
	{
		void InvokeReadAndSubscribe(Action action, IScalarComputing scalarDispatching);
	}
}
