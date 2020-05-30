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
		/// <summary>
		/// 
		/// </summary>
		/// <param name="action"></param>
		/// <param name="collectionDispatching"></param>
		/// <param name="initializingFromSourceCollection"></param>
		/// <param name="notifyCollectionChangedEventArgs">not null if source collection changes, null if initializing from source collection</param>
		/// <param name="propertyChangedEventArgs"></param>
		/// <param name="notifyCollectionChangedAction"></param>
		/// <param name="newItem"></param>
		/// <param name="newItemIndex"></param>
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
