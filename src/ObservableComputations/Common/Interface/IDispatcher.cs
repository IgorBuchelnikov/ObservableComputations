using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ObservableComputations
{
	public interface IDispatcher
	{
		void Invoke(Action action, object context);
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
			NotifyCollectionChangedAction notifyCollectionChangedAction,
			object newItem,
			object oldItem,
			int newIndex,
			int oldIndex);
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
