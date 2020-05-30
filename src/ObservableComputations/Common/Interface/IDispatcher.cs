using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ObservableComputations
{
	public interface IDispatcher
	{
		void Invoke(Action action, IComputing computing);
	}

	public interface ICollectionDestinationDispatcher
	{
		void Invoke(
			Action action, 
			ICollectionComputing collectionDispatching,
			bool initializingFromSourceCollection,
			NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs,
			PropertyChangedEventArgs propertyChangedEventArgs,
			NotifyCollectionChangedAction notifyCollectionChangedAction,
			object newItem,
			int newItemIndex);
	}

	public interface IScalarDestinationDispatcher
	{
		void Invoke(
			Action action, 
			IScalarComputing scalarDispatching,
			PropertyChangedEventArgs propertyChangedEventArgs);
	}

	public interface IPropertyDestinationDispatcher
	{
		void Invoke(
			Action action, 
			IComputing propertyDispatching,
			PropertyChangedEventArgs propertyChangedEventArgs);
	}


	public interface IPropertySourceDispatcher
	{
		void Invoke(
			Action action, 
			IComputing propertyDispatching,
			bool initializing,
			object newValue);
	}

}
