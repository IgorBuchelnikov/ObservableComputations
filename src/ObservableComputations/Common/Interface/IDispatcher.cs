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
		/// <param name="notifyCollectionChangedEventArgs">not null if source collection changes, null if initializing from source collection</param>
		/// <param name="propertyChangedEventArgs">not null if source collection scalar Value property changes</param>
		/// <param name="initializingFromSourceCollection">enumerating source collection and adding items from it</param>	
		void Invoke(
			Action action, 
			ICollectionComputing collectionDispatching,
			NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs,
			PropertyChangedEventArgs propertyChangedEventArgs,
			bool initializingFromSourceCollection);
	}


	public interface IScalarDestinationDispatcher
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="action"></param>
		/// <param name="scalarDispatching"></param>
		/// <param name="propertyChangedEventArgs">not null if source scalar changes, null if initializing from source scalar</param>
		void Invoke(
			Action action, 
			IScalarComputing scalarDispatching,
			PropertyChangedEventArgs propertyChangedEventArgs);
	}

	public interface IPropertyDestinationDispatcher
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="action"></param>
		/// <param name="propertyDispatching"></param>
		/// <param name="propertyChangedEventArgs">not null in source property changes, null if initializing from source property</param>
		void Invoke(
			Action action, 
			IComputing propertyDispatching,
			PropertyChangedEventArgs propertyChangedEventArgs);
	}


	public interface IPropertySourceDispatcher
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="action"></param>
		/// <param name="propertyDispatching"></param>
		/// <param name="initializing">false if setting property value</param>
		/// <param name="newValue">new value if setting property value </param>
		void Invoke(
			Action action, 
			IComputing propertyDispatching,
			bool initializing,
			object newValue);
	}

}
